using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDedlegate;

public class Event2 : EventMgr
{
    private GameObject barrier;
    private Timer timer;
    private TouchMgr touchMgr;
    bool panelCheck;
    bool modeCheck;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        GameObject player = GameObject.Find("Player");
        barrier = player.transform.Find("BeamupCylinderGreen").gameObject;
        timer = FindObjectOfType<Timer>();
        touchMgr = player.GetComponent<TouchMgr>();
        touchMgr.mode.Equals(TouchMgr.SkillMode.chat);

        EventList[0] = new Deleg(EV1);
        EventList[1] = new Deleg(EV2);
        EventList[2] = new Deleg(EV3);
        EventList[3] = new Deleg(EV4);
    }
    private void Update()
    {
        if (panelCheck && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
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
    }
    private void EV4()
    {
        touchMgr.ChangeMode(TouchMgr.SkillMode.switchBomb);
        timer.StartTime();
    }

    private IEnumerator FadeInOutBarrier()
    {
        barrier.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        barrier.SetActive(false);
        CallChat();
    }

    private IEnumerator PanelIn()
    {
        yield return new WaitForSeconds(1.0f);
        CallChat();
    }

}
