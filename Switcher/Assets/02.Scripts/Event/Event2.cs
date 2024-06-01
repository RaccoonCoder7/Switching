using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDedlegate;

public class Event2 : EventMgr
{
    bool panelCheck;
    bool modeCheck;
    bool btnCheck;
    bool isEffectOn;
    TouchFinger tf;

    private GameObject smallRing;

    void Start()
    {
        base.Start();
        tf = FindObjectOfType<TouchFinger>();
        smallRing = timer.transform.Find("smallRing").gameObject;
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
            chat.prevMode = touchMgr.mode;
            touchMgr.ChangeMode(TouchMgr.SkillMode.chat);
            CallChat();
            modeCheck = false;
            OffPanelEffect();
        }
        if (btnCheck)
        {
            if (tf.fullBtn)
            {
                tf.stageCheck = false;
                tf.fullBtn = false;
                smallRing.SetActive(false);
                chat.prevMode = touchMgr.mode;
                touchMgr.ChangeMode(TouchMgr.SkillMode.chat);
                tf.ActiveFalseBtn();
                CallChat();
                btnCheck = false;
                return;
            }
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                if (!isEffectOn)
                {
                    isEffectOn = true;
                    StartCoroutine("EffectOn");
                }
                return;
            }
            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
            {
                isEffectOn = false;
                StopCoroutine("EffectOn");
                smallRing.SetActive(false);
            }
        }
    }

    private void EV1()
    {
        StartCoroutine("FadeInOutBarrier");
        touchMgr.canFire = false;
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
        tf.stageCheck = true;
    }
    private void EV5()
    {
        touchMgr.ChangeMode(chat.prevMode);
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

    private IEnumerator EffectOn()
    {
        yield return new WaitForSeconds(0.8f);
        smallRing.SetActive(true);
    }

}
