using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    GameMgr gameMgr;
    public Text text;
    public float setTime;
    int m;
    int s;
    public Material fadeMaterial1;
    public Material fadeMaterial2;
    public static bool canvasCheck;

    // Start is called before the first frame update
    void Start()
    {
        gameMgr = FindObjectOfType<GameMgr>();
        setTime = 70f;
    }

    // Update is called once per frame
    void Update()
    {
        SetTime();
    }
    void SetTime()
    {
        if(setTime > 0)
        {
            setTime -= Time.deltaTime;
            m = Mathf.FloorToInt(setTime / 60);
            s = Mathf.FloorToInt(setTime % 60);
            
        }
        if (text.text.Equals("01:00"))
        {
            gameMgr.screenImage.material = fadeMaterial1;
            gameMgr.fadeMaterial = fadeMaterial1;
            Debug.Log(gameMgr.fadeMaterial);
            if (canvasCheck==false)
            {
                gameMgr.StartCoroutine(gameMgr.FadeInOut());
            }
            canvasCheck = true;


        }
        else if(text.text.Equals("00:50"))
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
        text.text = string.Format("{0:00}:{1:00}",m,s);
    }
}
