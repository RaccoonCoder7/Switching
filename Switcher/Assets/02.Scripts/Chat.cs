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
    int textCnt;
    void Start()
    {
        gameMgr = new GameMgr();
        TextSet();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TextSet()
    {
        textCnt = 1;
        textList = new List<string>();
        textData = Resources.Load(SceneManager.GetActiveScene().name+"Text", typeof(TextAsset)) as TextAsset;
        Debug.Log(SceneManager.GetActiveScene().name + "Text");
        sr = new StringReader(textData.text);
        textFile = sr.ReadLine();
        textList.Add(textFile);
        text.text = textList[0];
        while (textFile != null)
        {
            textFile = sr.ReadLine();
            textList.Add(textFile);
        }
    }
    public void NextText()
    {
        //textCnt+=1;
        //Debug.Log(textCnt);
        //Debug.Log(textList[textCnt]);
        //text.text = textList[textCnt];
        if (textList[textCnt].Equals("false")){
            textCnt++;
            text.text = textList[textCnt];
            gameObject.SetActive(false);
            textCnt++;
        }else if(textList[textCnt].Equals("clear")){
            gameMgr.Clear();
        }
        else
        {
            text.text = textList[textCnt];
            textCnt++;
            Debug.Log(textList[textCnt]);
        }
    }


}
