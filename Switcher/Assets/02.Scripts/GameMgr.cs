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
    public GameObject FinishText;
    public GameObject player;

    AsyncOperation async;
    Color color;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        screen = GameObject.Find("FadeCanvas").transform.Find("FadePanel").gameObject;
        screenImage = screen.GetComponent<Image>();
        stage = PlayerPrefs.GetInt("Stage");

    }


    //새로하기 함수
    public void NewGame()
    {
        //PlayerPrefs Stage에 Stage1저장
        PlayerPrefs.SetInt("Stage", 1);
        stage = 1;
    }

    public int GetPrevStageNum()
    {
        return stage;
    }

    //저장
    public void SaveClearData()
    {
        //현재 씬 이름 저장
        PlayerPrefs.SetInt("Stage", PlayerPrefs.GetInt("Stage") + 1);
        stage = PlayerPrefs.GetInt("Stage");
    }

    public IEnumerator FadeIn()
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
        logo.SetActive(false);
        FinishText.SetActive(false);
        Timer.canvasCheck = false;
    }
    public IEnumerator FadeInOut()
    {
        screen.SetActive(true);
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
        screenImage.material = blackMaterial;
        fadeMaterial = blackMaterial;
    }

    public IEnumerator FinishFadeInOut()
    {
        screen.SetActive(true);
        Color color = fadeMaterial.color;
        while (color.a < 1f)
        {
            color.a += 0.03f;
            fadeMaterial.color = color;
            yield return new WaitForSeconds(0.02f);
        }
        
        
        player = GameObject.Find("Player");
        GameObject sc = FindObjectOfType<StageCtrl>().gameObject;
        SceneManager.MoveGameObjectToScene(player, SceneManager.GetActiveScene());
        SceneManager.MoveGameObjectToScene(sc, SceneManager.GetActiveScene());
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        StageCtrl sctrl = FindObjectOfType<StageCtrl>();
        Destroy(sctrl.stage.map);
        FinishText.SetActive(true);
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("StartScene");
    }
}