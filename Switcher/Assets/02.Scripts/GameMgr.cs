using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{


    public static int stage;
    public GameObject ctnBtn;
    public GameObject screen;
    public GameObject logo;
    public Image screenImage;
    public Material fadeMaterial;
    public Material blackMaterial;

    AsyncOperation async;
    Color color;
    void Start()
    {   
        DontDestroyOnLoad(gameObject);
        screen = GameObject.Find("FadeCanvas").transform.Find("FadePanel").gameObject;
        screenImage = screen.GetComponent<Image>();
        stage = PlayerPrefs.GetInt("Stage");
        Debug.Log(stage);

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
    }

    //이어하기, 다시하기
    public int GetPrevStageNum()
    {
        return stage;
    }

    //저장
    public void Clear()
    {
        //현재 씬 이름 저장
        PlayerPrefs.SetInt("Stage", PlayerPrefs.GetInt("Stage")+1);
        stage = PlayerPrefs.GetInt("Stage");
        Debug.Log(stage);
        StartCoroutine("Load");
    }
    public IEnumerator FadeIn()
    {
        screen.SetActive(true);
        Debug.Log(screen);
        Debug.Log(fadeMaterial);
        Color color = fadeMaterial.color;
        while (color.a < 1f)
        {
            color.a += 0.03f;
            fadeMaterial.color = color;
            yield return new WaitForSeconds(0.02f);
        }
        logo.SetActive(true);
        //color.a = 0f;
        //fadeMaterial.color = color;
    }

    public IEnumerator FadeOut()
    {
        Color color = fadeMaterial.color;
        while (color.a > 0f)
        {
            color.a -= 0.03f;
            fadeMaterial.color = color;
            yield return new WaitForSeconds(0.02f);
        }
        screen.SetActive(false);
        //logo.SetActive(false);
        Timer.canvasCheck = false;
    }
    public IEnumerator FadeInOut()
    {
        screen.SetActive(true);
        Debug.Log(screen);
        Debug.Log(fadeMaterial);
        Color color = fadeMaterial.color;
        while (color.a < 0.4f)
        {
            color.a += 0.03f;
            fadeMaterial.color = color;
            yield return new WaitForSeconds(0.02f);
        }
        StartCoroutine("FadeOut");
    }

    public void ChangeScreanImage()
    {
        screenImage.material = fadeMaterial;
        fadeMaterial = blackMaterial;
    }

    public IEnumerator TestClear()
    {
        screenImage.material = fadeMaterial;
        fadeMaterial = blackMaterial;
        async = SceneManager.LoadSceneAsync("StartScene"); // 열고 싶은 씬
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            //yield return true;
            yield return StartCoroutine("FadeIn");
            async.allowSceneActivation = true;
            Color color = fadeMaterial.color;
            color.a = 0f;
            fadeMaterial.color = color;
        }
    }
}