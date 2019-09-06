using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{


    //TextAsset saveData;
    //StringReader sr;
    int stage;
    public GameObject ctnBtn;
    public Material fadeMaterial;
    void Start()
    {
        StartCoroutine("FadeIn");
        //StartCoroutine("FadeOut");
        DontDestroyOnLoad(gameObject);
        stage = PlayerPrefs.GetInt("Stage");
        if (stage.Equals(0))
        {
            ctnBtn.SetActive(false);
        }
        //saveData = Resources.Load("SaveFile", typeof(TextAsset)) as TextAsset;
        //sr = new StringReader(saveData.text);
        //string savetext = sr.ReadLine();
        //Debug.Log(savetext);
    }


     //새로하기 함수
    public void NewGame()
    {
        SceneManager.LoadScene("Stage1");
        //PlayerPrefs Stage에 Stage1저장
        PlayerPrefs.SetInt("Stage", 1);
    }

    //이어하기, 다시하기
    public void Continue()
    {
        //PlayerPrefs Stage 값 불러와 씬 로드
        SceneManager.LoadScene("Stage"+stage);
    }

    //저장
    public void Clear()
    {
        //현재 씬 이름 저장
        PlayerPrefs.SetInt("Stage", PlayerPrefs.GetInt("Stage")+1);
        stage = PlayerPrefs.GetInt("Stage");
        SceneManager.LoadScene("Stage" + stage);
    }
    IEnumerator FadeIn()
    {
        Color color = fadeMaterial.color;
        while (color.a < 1f)
        {
            color.a += 0.03f;
            fadeMaterial.color = color;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator FadeOut()
    {
        Color color = fadeMaterial.color;
        while (color.a > 0f)
        {
            color.a -= 0.03f;
            fadeMaterial.color = color;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
