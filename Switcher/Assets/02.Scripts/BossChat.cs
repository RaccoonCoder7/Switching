using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MyDedlegate
{
    public delegate void mDeleg();

    public class BossChat : MonoBehaviour
    {
        GameMgr gameMgr;
        TextAsset textData;
        StringReader sr;
        List<string> textList;
        string textFile;
        string helperTextList;
        int textCount;
        int continueCnt;

        public Text text;
        public bool helpCheck;
        public Deleg[] chatEventList = new Deleg[6];

        TextAsset bossTextData;
        StringReader bossSr;
        List<string> bossTextList;
        string bossTextFile;
        int bossTextCount;
        public Text bossText;
        bool bossCheck = false;
        public GameObject bossCanvas;


        int paragraphCnt;
        State nowState;
        private StageCtrl sc;
        public TouchMgr touchMgr;
        private TouchMgr.SkillMode prevMode;



        AudioSource audio;

        enum State
        {
            Next,
            Playing
        }
        void Start()
        {
            helperTextList = "대화를 다시 보려면 트리거버튼, 스테이지를 다시 시작하려면 다시하기버튼을 누르세요";
            gameMgr = FindObjectOfType<GameMgr>();
            audio = GetComponent<AudioSource>();
            sc = FindObjectOfType<StageCtrl>();
            // touchMgr = FindObjectOfType<TouchMgr>();
            TextSet("test");
            BossTextSet();
            bossCanvas.SetActive(false);

        }

        // Update is called once per frame
        void Update()
        {
            if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
            {
                NextText();
            }
            //if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
            //{
            //    CallHelper();
            //}
        }

        public void ResetText()
        {
            continueCnt = 0;
            textCount = 0;
            paragraphCnt = 0;
            NextText();
        }

        public void TextSet(string str)
        {
            Debug.Log("t1");
            continueCnt = 0;
            textCount = 1;
            textList = new List<string>();
            textData = Resources.Load(str + "Text", typeof(TextAsset)) as TextAsset;
            Debug.Log("t2");
            sr = new StringReader(textData.text);
            textFile = sr.ReadLine();
            textList.Add(textFile);
            Debug.Log("t3");
            StartCoroutine(PlayLine(textList[0]));
            Debug.Log("t4");
            while (textFile != null)
            {
                textFile = sr.ReadLine();
                textList.Add(textFile);
            }
            Debug.Log("t5");
            // prevMode = touchMgr.mode;
            // touchMgr.ChangeMode(TouchMgr.SkillMode.chat);
            Debug.Log("t6");
        }

        public void BossTextSet()
        {
            bossCanvas.SetActive(false);
            bossTextCount = 0;
            bossTextList = new List<string>();
            bossTextData = Resources.Load("boss", typeof(TextAsset)) as TextAsset;
            bossSr = new StringReader(bossTextData.text);
            bossTextFile = bossSr.ReadLine();
            bossTextList.Add(bossTextFile);
            while (bossTextFile != null)
            {
                bossTextFile = bossSr.ReadLine();
                bossTextList.Add(bossTextFile);
            }
        }

        public void NextText()
        {
            if (nowState.Equals(State.Next))
            {
                audio.Play();
                //불러온 텍스트중 false가 있으면 아래 실행
                if (textList[textCount].Equals("false"))
                {
                    //textCount++;
                    text.text = "";
                    textCount++;
                    Debug.Log("PC: " + paragraphCnt);
                    //chatEventList[paragraphCnt]();
                    // paragraphCnt++;
                    // touchMgr.ChangeMode(prevMode);
                    gameObject.SetActive(false);
                }
                //불러온 텍스트중 clear가 있으면 아래 실행
                else if (textList[textCount].Equals("clear"))
                {
                    // gameMgr.Clear();
                    sc.StartCoroutine(sc.ClearStage());
                }
                else if (textList[textCount].Equals("boss"))
                {
                    text.text = "";
                    textCount++;
                    gameObject.SetActive(false);
                    bossCanvas.SetActive(true);
                    StartCoroutine(BossPlayLine(bossTextList[bossTextCount]));
                    bossTextCount++;
                    bossCheck = true;
                }
                else if (bossTextList[bossTextCount].Equals("bossfalse"))
                {
                    bossText.text = "";
                    bossTextCount++;
                    bossCanvas.SetActive(false);
                }
                else if (bossCheck)
                {
                    StartCoroutine(BossPlayLine(bossTextList[bossTextCount]));
                    bossTextCount++;
                }
                else if (!bossCheck)
                {
                    StartCoroutine(PlayLine(textList[textCount]));
                    //text.text = textList[textCount];
                    textCount++;
                }
            }
        }

        //조력자를 불렀을 때 사용하는 함수
        public void CallHelper()
        {
            prevMode = touchMgr.mode;
            Debug.Log("before");
            touchMgr.ChangeMode(TouchMgr.SkillMode.chat);
            Debug.Log("after");
            StartCoroutine(PlayLine(helperTextList));
            textCount = continueCnt;
            helpCheck = true;
        }
        public void FadeHelper()
        {
            touchMgr.ChangeMode(prevMode);
            textCount = continueCnt;
            gameObject.SetActive(false);
            helpCheck = false;

        }

        //다음 대화를 진행할 때 부르는 메소드
        public void NextChat()
        {
            continueCnt = textCount;
            Debug.Log("NextChat");
            // gameObject.SetActive(true);
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
                text.text = setText.Substring(0, i);
            }
            yield return new WaitForSeconds(0.5f);
            nowState = State.Next;
        }
        IEnumerator BossPlayLine(string setText)
        {
            nowState = State.Playing;
            for (int i = 0; i < setText.Length + 1; i += 1)
            {
                yield return new WaitForSeconds(0.02f);
                bossText.text = setText.Substring(0, i);
            }
            yield return new WaitForSeconds(0.5f);
            nowState = State.Next;
        }
    }
}