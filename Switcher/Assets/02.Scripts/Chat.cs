using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace MyDedlegate
{
    public delegate void Deleg();

    public class Chat : MonoBehaviour
    {
        private Ray ray;
        private RaycastHit hit;
        public LineRenderer laser;
        public GameObject[] controllers;
        public Texture[] textures;
        private MeshRenderer[] renderers = new MeshRenderer[2];
        // private Renderer RightRend;

        GameMgr gameMgr;
        TextAsset textData;
        StringReader sr;
        List<string> textList;
        string textFile;
        string helperTextList;
        public int textCount;
        int continueCnt;
        public int paragraphCnt;
        State nowState;
        private StageCtrl sc;
        public TouchMgr touchMgr;
        public TouchMgr.SkillMode prevMode;
        public Text text;
        public bool helpCheck = false;
        public ImageCtrl imageCtrl;
        public Deleg[] chatEventList = new Deleg[6];
        public AudioClip chatClip;
        public GameObject reBtn;

        AudioSource audio;

        public Timer timer;

        public bool bossStart;



        enum State
        {
            Next,
            Playing
        }
        void Start()
        {
            prevMode = TouchMgr.SkillMode.chat;
            helperTextList = "대화를 다시 보려면 오른손 검지버튼, 대화창을 끄려면 스킵하기버튼을 누르세요.";
            gameMgr = FindObjectOfType<GameMgr>();
            audio = GetComponent<AudioSource>();
            sc = FindObjectOfType<StageCtrl>();
            renderers[0] = controllers[0].GetComponent<MeshRenderer>();
            renderers[1] = controllers[1].GetComponent<MeshRenderer>();
            controllers[0].SetActive(false);
            controllers[1].SetActive(false);
            ShowController(1, 0);
        }

        void Update()
        {
            ray = new Ray(laser.transform.position, laser.transform.forward);
            if (Physics.Raycast(ray, out hit, 16.0f))
            {
                float dist = hit.distance;
                laser.SetPosition(1, new Vector3(0, 0, dist));
            }
            laser.enabled = true;
            if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("RETRY")))
                {
                    FadeHelper();
                    timer.StartTime();
                    textCount = GetLastParagraph();
                    continueCnt = textCount;
                    if (sc.stage.stageNum.Equals(6) && !bossStart)
                    {
                        bossStart = true;
                        paragraphCnt = 4;
                        continueCnt = 7;
                        textCount = 16;
                        chatEventList[paragraphCnt]();
                    }
                }
                else
                {
                    if (timer.chatFinish)
                    {
                        reBtn.SetActive(false);
                    }
                    if (gameMgr.fadeChk)
                    {
                        NextText();
                    }

                }
            }
        }

        private int GetLastParagraph()
        {
            List<int> falseArr = new List<int>();
            for (int i = 0; i < textList.Count -1; i++)
            {
                if (textList[i].Equals("false"))
                {
                    falseArr.Add(i);
                }
            }
            if (falseArr.Count < 2) return 0;
            return falseArr[falseArr.Count-2] + 1;
        }

        // 첫 번째 파라미터: 컨트롤러. (0: 왼손, 1: 오른손)
        // 두 번째 파라미터: 버튼. (0: 트리거, 1: 핸드)
        public void ShowController(int ctrlNum, int btnNum)
        {
            if (ctrlNum < 0 || ctrlNum > 1 || btnNum < 0 || btnNum > 1) return;
            controllers[ctrlNum].SetActive(true);
            StartCoroutine(ControllerAnim(ctrlNum, btnNum));
        }

        // 첫 번째 파라미터: 컨트롤러. (0: 왼손, 1: 오른손)
        public void HideController(int ctrlNum)
        {
            if (ctrlNum < 0 || ctrlNum > 1) return;
            controllers[ctrlNum].SetActive(false);
        }

        // 첫 번째 파라미터: 컨트롤러. (0: 왼손, 1: 오른손)
        // 두 번째 파라미터: 버튼. (0: 트리거, 1: 핸드, 2: 트리거&핸드)
        private IEnumerator ControllerAnim(int ctrlNum, int btnNum)
        {
            while (renderers[ctrlNum])
            {
                renderers[ctrlNum].material.mainTexture = textures[btnNum];
                yield return new WaitForSeconds(0.5f);
                renderers[ctrlNum].material.mainTexture = textures[2];
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void OnEnable()
        {
            ShowController(1, 0);
        }

        private void OnDisable()
        {
            StopCoroutine("ControllerAnim");
        }

        // 텍스트를 처음부터 다시 보여주도록 함
        public void ResetText()
        {
            if (timer.chatFinish)
            {
                reBtn.SetActive(true);
            }
            continueCnt = 0;
            textCount = 0;
            paragraphCnt = 0;
            timer.endText = false;
            prevMode = TouchMgr.SkillMode.switching;
            imageCtrl.ChangeSprites(TouchMgr.SkillMode.switching);
            touchMgr.ChangeMode(TouchMgr.SkillMode.chat);
            NextText();
        }

        // 텍스트를 리소스에서 로드하여 저장함
        public void TextSet(string str)
        {
            if (timer.chatFinish)
            {
                reBtn.SetActive(true);
            }
            continueCnt = 0;
            textCount = 0;
            paragraphCnt = 0;
            timer.endText = false;
            imageCtrl.ChangeSprites(TouchMgr.SkillMode.switching);
            prevMode = TouchMgr.SkillMode.switching;
            //touchMgr.ChangeMode(TouchMgr.SkillMode.chat);
            textList = new List<string>();
            textData = Resources.Load(str + "Text", typeof(TextAsset)) as TextAsset;
            sr = new StringReader(textData.text);
            textFile = sr.ReadLine();
            textList.Add(textFile);
            //StartCoroutine(PlayLine(textList[0]));
            NextText();
            while (textFile != null)
            {
                textFile = sr.ReadLine();
                textList.Add(textFile);
            }
        }

        // 다음줄을 읽음.
        public void NextText()
        {
            if (nowState.Equals(State.Next))
            {
                audio.Play();



                //불러온 텍스트중 false가 있으면 아래 실행
                if (textList[textCount].Equals("false"))
                {
                    if (timer.chatFinish)
                    {
                        reBtn.SetActive(true);
                    }
                    text.text = "";
                    textCount++;
                    if (!timer.chatFinish || !timer.endText)
                    {
                        chatEventList[paragraphCnt]();
                    }
                    else
                    {
                        imageCtrl.ChangeSprites(prevMode);
                        touchMgr.ChangeMode(prevMode);
                        touchMgr.canFire = true;
                    }
                    helpCheck = false;
                    gameObject.SetActive(false);

                }
                //불러온 텍스트중 clear가 있으면 아래 실행
                else if (textList[textCount].Equals("clear"))
                {
                    sc.StartCoroutine(sc.ClearStage());
                }
                else
                {
                    StartCoroutine(PlayLine(textList[textCount]));
                    textCount++;

                }
            }
        }

        // 조력자를 불렀을 때 사용하는 함수
        public void CallHelper()
        {
            prevMode = touchMgr.mode;
            touchMgr.ChangeMode(TouchMgr.SkillMode.chat);
            //StartCoroutine(PlayLine(helperTextList));
            text.text = helperTextList;
            textCount = continueCnt;
            helpCheck = true;
        }

        // 조력자를 없앰
        public void FadeHelper()
        {
            if (prevMode.Equals(TouchMgr.SkillMode.chat))
            {
                imageCtrl.ChangeSprites(TouchMgr.SkillMode.switching);
                touchMgr.ChangeMode(TouchMgr.SkillMode.switching);
                touchMgr.canFire = true;
            }
            else
            {
                imageCtrl.ChangeSprites(prevMode);
                touchMgr.ChangeMode(prevMode);
                touchMgr.canFire = true;
            }

            textCount = continueCnt;
            helpCheck = false;
            gameObject.SetActive(false);
        }

        //다음 대화를 진행할 때 부르는 메소드
        public void NextChat()
        {
            continueCnt = textCount;
            StartCoroutine(PlayLine(textList[textCount]));
            paragraphCnt++;
            textCount++;
        }

        IEnumerator PlayLine(string setText)
        {
            nowState = State.Playing;
            for (int i = 0; i < setText.Length + 1; i += 1)
            {
                yield return new WaitForSeconds(0.02f);
                if (i != 0 && setText.Substring(i - 1, 1) == "<")
                {
                    i = GetEndOfTag(setText, i);
                }
                text.text = setText.Substring(0, i);

                if (i % 3 == 0 && i > 1 && !setText.Substring(i - 1, 1).Equals(" "))
                {
                    audio.PlayOneShot(chatClip);
                }
            }
            yield return new WaitForSeconds(0.2f);
            nowState = State.Next;
        }

        private int GetEndOfTag(string text, int i)
        {
            int count = 0;
            for (int j = i; j < text.Length + 1; j++)
            {
                if (text.Substring(j, 1) == ">")
                {
                    count++;
                    if (count == 2)
                    {
                        return j + 1;
                    }
                }
            }
            return 0;
        }
    }
}