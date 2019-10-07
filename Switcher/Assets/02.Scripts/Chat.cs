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

        GameMgr gameMgr;
        TextAsset textData;
        StringReader sr;
        List<string> textList;
        string textFile;
        string helperTextList;
        int textCount;
        int continueCnt;
        public int paragraphCnt;
        State nowState;
        private StageCtrl sc;
        public TouchMgr touchMgr;
        private TouchMgr.SkillMode prevMode;
        public Text text;
        public bool helpCheck;
        public Deleg[] chatEventList = new Deleg[6];

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
                    laser.enabled = false;
                    StartCoroutine(sc.ResetStage(null));
                }
                else
                {
                    NextText();
                }
            }
        }

        // 텍스트를 처음부터 다시 보여주도록 함
        public void ResetText()
        {
            continueCnt = 0;
            textCount = 0;
            paragraphCnt = 0;
            touchMgr.ChangeMode(TouchMgr.SkillMode.chat);
            NextText();
        }

        // 텍스트를 리소스에서 로드하여 저장함
        public void TextSet(string str)
        {
            continueCnt = 0;
            textCount = 1;
            textList = new List<string>();
            textData = Resources.Load(str + "Text", typeof(TextAsset)) as TextAsset;
            sr = new StringReader(textData.text);
            textFile = sr.ReadLine();
            textList.Add(textFile);
            StartCoroutine(PlayLine(textList[0]));
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
                    text.text = "";
                    textCount++;
                    chatEventList[paragraphCnt]();
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
            StartCoroutine(PlayLine(helperTextList));
            textCount = continueCnt;
            helpCheck = true;
        }

        // 조력자를 없앰
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
    }
}