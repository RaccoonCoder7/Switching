using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleBarLaser : MonoBehaviour
{
    public Image circleBar;
    [HideInInspector]
    public float selectedTime = 10.0f;
    float passedTime = 10.0f;
    float stopTime = 0.0f;

    private Transform tr;
    private Transform camTr;

    public GameObject laser;

    public bool goCheck = false;

    void Start()
    {
        tr = GetComponent<Transform>();
        camTr = Camera.main.GetComponent<Transform>();
        selectedTime = laser.GetComponent<LaserFire>().stopTime;
        passedTime = selectedTime;
        stopTime = laser.GetComponent<LaserFire>().shootTime;
    }
    
    void Update()
    {
        if (!goCheck) return;

        if (selectedTime != laser.GetComponent<LaserFire>().stopTime)
        {
            selectedTime = laser.GetComponent<LaserFire>().stopTime;
            passedTime = selectedTime;
        }

        if (stopTime != laser.GetComponent<LaserFire>().shootTime)
        {
            stopTime = laser.GetComponent<LaserFire>().shootTime;
        }

        passedTime -= Time.deltaTime;
        circleBar.fillAmount = passedTime / selectedTime;
        if (passedTime <= -stopTime)
        {
            passedTime = selectedTime;
        }
    }

    void LateUpdate()
    {
        tr.LookAt(camTr.position);
    }
}
