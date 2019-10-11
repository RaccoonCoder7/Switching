using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MyDedlegate;

public class BossChat : MonoBehaviour
{

    TextAsset bossTextData;
    StringReader bossSr;
    List<string> bossTextList;
    string bossTextFile;
    public int bossTextCount;
    public Text bossText;
    bool bossCheck = false;

    Chat chat;


    public int paragraphCnt;
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

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        sc = FindObjectOfType<StageCtrl>();
        chat = FindObjectOfType<Chat>();
    }
    private void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            BossNextText();
        }
    }

    public void BossTextSet()
    {
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
        BossNextText();
    }

    public void BossNextText()
    {
        if (nowState.Equals(State.Next))
        {
            audio.Play();
            //불러온 텍스트중 false가 있으면 아래 실행
            if (bossTextList[bossTextCount].Equals("false"))
            {
                //textCount++;
                bossText.text = "";
                bossTextCount++;
                Debug.Log("PC: " + chat.paragraphCnt);
                chat.gameObject.SetActive(true);
                chat.NextChat();
                chat.chatEventList[chat.paragraphCnt]();
                // paragraphCnt++;
                // touchMgr.ChangeMode(prevMode);
                gameObject.SetActive(false);
            }
            //불러온 텍스트중 clear가 있으면 아래 실행
            else if (bossTextList[bossTextCount].Equals("clear"))
            {
                // gameMgr.Clear();
                sc.StartCoroutine(sc.ClearStage());
            }
            else
            {
                StartCoroutine(BossPlayLine(bossTextList[bossTextCount]));
                //text.text = textList[textCount];
                bossTextCount++;
            }
        }
    }

    //다음 대화를 진행할 때 부르는 메소드
    public void NextChat()
    {
        StartCoroutine(BossPlayLine(bossTextList[bossTextCount]));
        chat.paragraphCnt++;
        bossTextCount++;
    }

    IEnumerator BossPlayLine(string setText)
    {
        nowState = State.Playing;
        for (int i = 0; i < setText.Length + 1; i += 1)
        {
            yield return new WaitForSeconds(0.02f);
            bossText.text = setText.Substring(0, i);
        }
        yield return new WaitForSeconds(0.2f);
        nowState = State.Next;
    }
}
