using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleBar : MonoBehaviour
{
    public Image circleBar;
    float selectedTime = 10.0f;
    float passedTime = 10.0f;

    private Transform tr;
    private Transform camTr;

    void Start()
    {
        tr = GetComponent<Transform>();
        camTr = Camera.main.GetComponent<Transform>();
        selectedTime = gameObject.transform.parent.parent.gameObject.GetComponent<ManastoneFire>().liveTime;
        passedTime = selectedTime;
    }
    
    void Update()
    {
        passedTime -= Time.deltaTime;
        circleBar.fillAmount = passedTime / selectedTime;
        if (passedTime <= 0.0f)
        {
            passedTime = selectedTime;
        }
    }

    void LateUpdate()
    {
        tr.LookAt(camTr.position);
    }
}
