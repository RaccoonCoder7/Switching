using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    GameMgr gameMgr;
    public Text text;
    int leftTime = 0;
    int m;
    int s;
    private PlayerState ps;

    public Material fadeMaterial1;
    public Material fadeMaterial2;
    public static bool canvasCheck;
    public GameObject stick;

    public bool chatFinish;

    public GameObject retryBtn;

    private AudioSource audio;
    public AudioClip[] stateClips;
    TouchFinger tf;

    public GameObject warningText;

    bool clearCheck;

    void Start()
    {
        gameMgr = FindObjectOfType<GameMgr>();
        audio = GetComponent<AudioSource>();
        tf = FindObjectOfType<TouchFinger>();
    }

    // 매개변수로 들어온 시간으로 패널의 시간을 재설정함
    public void ResetTime(int time)
    {
        
        StopCoroutine("SetTime");
        leftTime = time;
    }

    public void StartTime()
    {
        clearCheck = false;
        ChatFinish();
        if (text.text.Equals("00:00"))
        {
            StartCoroutine("SetTime");
        }
    }

    private IEnumerator SetTime()
    {
        while (leftTime > 0)
        {
            int m = leftTime / 60;
            int s = leftTime % 60;
            text.text = string.Format("{0:00}:{1:00}", m, s);

            if (text.text.Equals("01:00"))
            {
                gameMgr.screenImage.material = fadeMaterial1;
                gameMgr.fadeMaterial = fadeMaterial1;
                if (canvasCheck == false)
                {
                    audio.PlayOneShot(stateClips[0]);
                    StartCoroutine(Warning("60"));
                }
                canvasCheck = true;
            }
            else if (text.text.Equals("00:30"))
            {
                text.color = Color.red;
                gameMgr.screenImage.material = fadeMaterial2;
                gameMgr.fadeMaterial = fadeMaterial2;
                if (canvasCheck == false)
                {
                    audio.PlayOneShot(stateClips[1]);
                    StartCoroutine(Warning("30"));
                }
                canvasCheck = true;
            }

            yield return new WaitForSeconds(1.0f);
            if (!clearCheck)
            {
                leftTime--;
            }
            
        }
        if (!ps)
        {
            ps = FindObjectOfType<PlayerState>();
        }
        ps.PlayerDie();
    }

    public void ChatFinish()
    {
        retryBtn.SetActive(true);
        tf.ActiveTrueBtn();
        chatFinish = true;
    }
    public void ChatFinishReset()
    {
        clearCheck = true;
        retryBtn.SetActive(false);
        chatFinish = false;
    }
    IEnumerator Warning(string s)
    {

        warningText.GetComponent<Text>().text = s + "초 남았습니다";
        warningText.SetActive(true);
        gameMgr.StartCoroutine(gameMgr.FadeInOut());
        yield return new WaitForSeconds(1.0f);
        gameMgr.StartCoroutine(gameMgr.FadeInOut());
        yield return new WaitForSeconds(1.0f);
        warningText.SetActive(false);

    }
}
