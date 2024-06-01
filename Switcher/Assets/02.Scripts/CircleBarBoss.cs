using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleBarBoss : MonoBehaviour
{
    public Image circleBar;
    float selectedTime = 10.0f;
    float passedTime = 10.0f;

    private Transform tr;
    private Transform camTr;

    public BossState bossSt;

    void Start()
    {
        tr = GetComponent<Transform>();
        camTr = Camera.main.GetComponent<Transform>();
        selectedTime = bossSt.deathResetTime;
        passedTime = selectedTime;
        circleBar.color = new Color(1, 1, 1, 0);
    }
    
    void Update()
    {
        if (bossSt.deathCount == 1)
        {
            circleBar.color = new Color(1, 1, 1, 1);
            passedTime -= Time.deltaTime;
            circleBar.fillAmount = passedTime / selectedTime;
            if (passedTime <= 0.0f)
            {
                passedTime = selectedTime;
            }
        }
        else if(circleBar.color.a != 0)
        {
            circleBar.color = new Color(1, 1, 1, 0);
        }
    }

    void LateUpdate()
    {
        tr.LookAt(camTr.position);
    }
}
