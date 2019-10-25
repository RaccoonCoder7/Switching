using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDedlegate;

public class Event2 : EventMgr
{
    bool panelCheck;
    bool modeCheck;
    bool btnCheck;
    TouchFinger tf;

    void Start()
    {
        base.Start();
        tf = FindObjectOfType<TouchFinger>();
        tf.stageCheck = true;
        EventList[0] = new Deleg(EV1);
        EventList[1] = new Deleg(EV2);
        EventList[2] = new Deleg(EV3);
        EventList[3] = new Deleg(EV4);
        EventList[4] = new Deleg(EV5);
    }

    private void Update()
    {
        if (panelCheck && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            touchMgr.ChangeMode(TouchMgr.SkillMode.chat);
            StartCoroutine("PanelIn");
            panelCheck = false;
        }
        if (modeCheck && touchMgr.mode.Equals(TouchMgr.SkillMode.switchBomb))
        {
            touchMgr.ChangeMode(TouchMgr.SkillMode.chat);
            CallChat();
            modeCheck = false;
            OffPanelEffect();
        }
        if (btnCheck && tf.fullBtn)
        {
            tf.stageCheck = false;
            tf.fullBtn = false;
            touchMgr.ChangeMode(TouchMgr.SkillMode.chat);
            tf.ActiveFalseBtn();
            CallChat();
            btnCheck = false;
        }
    }

    private void EV1()
    {
        StartCoroutine("FadeInOutBarrier");
    }
    private void EV2()
    {
        touchMgr.ChangeMode(TouchMgr.SkillMode.switching);
        panelCheck = true;
    }
    private void EV3()
    {
        touchMgr.ChangeMode(TouchMgr.SkillMode.switching);
        modeCheck = true;
        UsePanelEffect(1);
    }
    private void EV4()
    {
        touchMgr.ChangeMode(TouchMgr.SkillMode.switchBomb);
        tf.ActiveTrueBtn();
        btnCheck = true;
    }
    private void EV5()
    {
        touchMgr.ChangeMode(TouchMgr.SkillMode.switchBomb);
        timer.StartTime();
        touchMgr.canFire = true;
    }

    private IEnumerator FadeInOutBarrier()
    {
        ps.NewSkillSound();
        barrier[1].SetActive(true);
        yield return new WaitForSeconds(3.0f);
        barrier[1].SetActive(false);
        CallChat();
    }

    private IEnumerator PanelIn()
    {
        yield return new WaitForSeconds(1.0f);
        CallChat();
    }

}
