using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    private static GameMgr _instance = null;

    public static GameMgr Instance
    {
        get
        {
            _instance = FindObjectOfType(typeof(GameMgr)) as GameMgr;
            return _instance;
        }
    }



    public static int stage;
    public GameObject ctnBtn;
    public GameObject screen;
    public GameObject logo;
    public Material fadeMaterial;
    AsyncOperation async;
    bool canOpen = true;
    void Start()
    {   
        DontDestroyOnLoad(gameObject);
        stage = PlayerPrefs.GetInt("Stage");
        if (stage.Equals(0))
        {
            ctnBtn.SetActive(false);
        }

    }


     //새로하기 함수
    public void NewGame()
    {
        //PlayerPrefs Stage에 Stage1저장
        PlayerPrefs.SetInt("Stage", 1);
        stage = 1;
        StartCoroutine("Load");


    }

    //이어하기, 다시하기
    public void Continue()
    {
        //PlayerPrefs Stage 값 불러와 씬 로드
        StartCoroutine("Load");
    }
    IEnumerator Load()
    {
        async = SceneManager.LoadSceneAsync("Stage" + stage); // 열고 싶은 씬
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            //yield return true;
            yield return StartCoroutine("FadeIn");

                async.allowSceneActivation = true;
        }
        
    }

    //저장
    public void Clear()
    {
        //현재 씬 이름 저장
        PlayerPrefs.SetInt("Stage", PlayerPrefs.GetInt("Stage")+1);
        stage = PlayerPrefs.GetInt("Stage");
        Debug.Log(stage);
        //StartCoroutine("Load");
        Instance.StartCoroutine("Load");
    }
    IEnumerator FadeIn()
    {
        screen.SetActive(true);
        Color color = fadeMaterial.color;
        while (color.a < 1f)
        {
            color.a += 0.03f;
            fadeMaterial.color = color;
            yield return new WaitForSeconds(0.02f);
        }
        logo.SetActive(true);
    }

    IEnumerator FadeOut()
    {
        Color color = fadeMaterial.color;
        while (color.a > 0f)
        {
            color.a -= 0.03f;
            fadeMaterial.color = color;
            yield return new WaitForSeconds(0.02f);
        }
        screen.SetActive(false);
    }
}
