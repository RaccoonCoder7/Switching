using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Chat : MonoBehaviour
{
    GameMgr gameMgr;
    TextAsset textData;
    StringReader sr;
    public Text text;
    List<string> textList;
    string textFile;
    string helperTextList;
    int textCount;
    int continueCnt;
    void Start()
    {
        helperTextList = "대화를 다시 보려면 트리거버튼, 스테이지를 다시 시작하려면 다시하기버튼을 누르세요";
        gameMgr = new GameMgr();
        continueCnt = 0;
        TextSet();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TextSet()
    {
        textCount = 1;
        textList = new List<string>();
        textData = Resources.Load(SceneManager.GetActiveScene().name+"Text", typeof(TextAsset)) as TextAsset;
        Debug.Log(SceneManager.GetActiveScene().name + "Text");
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
    public void NextText()
    {
        //불러온 텍스트중 false가 있으면 아래 실행
        if (textList[textCount].Equals("false")){
            //textCount++;
            //text.text = textList[textCount];
            gameObject.SetActive(false);
            textCount++;
        }
        //불러온 텍스트중 clear가 있으면 아래 실행
        else if (textList[textCount].Equals("clear")){
            gameMgr.Clear();
        }
        else
        {
            StartCoroutine(PlayLine(textList[textCount]));
            //text.text = textList[textCount];
            textCount++;
            Debug.Log(textList[textCount]);
        }
    }

    //조력자를 불렀을 때 사용하는 함수
    public void CallHelper()
    {
        StartCoroutine(PlayLine(helperTextList));
        textCount = continueCnt;
    }

    //다음 대화를 진행할 때 부르는 메소드
     public void NextChat()
    {
        continueCnt = textCount;
        gameObject.SetActive(true);
        textCount++;
        StartCoroutine(PlayLine(textList[textCount]));
    }

    IEnumerator PlayLine(string setText)
    {
        for (int i = 0; i < setText.Length + 1; i += 1)
        {
            yield return new WaitForSeconds(0.02f);
            text.text = setText.Substring(0, i);
        }
    }




}
