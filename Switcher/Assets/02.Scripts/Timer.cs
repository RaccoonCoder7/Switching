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


    void Start()
    {
        gameMgr = FindObjectOfType<GameMgr>();
    }

    public void ResetTime(int time)
    {
        StopCoroutine("SetTime");
        leftTime = time;
        StartCoroutine("SetTime");
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
                    gameMgr.StartCoroutine(gameMgr.FadeInOut());
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
                    gameMgr.StartCoroutine(gameMgr.FadeInOut());
                }
                canvasCheck = true;
            }

            yield return new WaitForSeconds(1.0f);
            leftTime--;
        }
        if (!ps)
        {
            ps = FindObjectOfType<PlayerState>();
        }
        ps.PlayerDie();
    }
}
