using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDedlegate;

public class Event3 : EventMgr
{
    bool modeCheck;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        EventList[0] = new Deleg(EV1);
        EventList[1] = new Deleg(EV2);
        EventList[2] = new Deleg(EV3);
        touchMgr.canFire = false;
    }
    private void Update()
    {
        if (modeCheck && touchMgr.mode.Equals(TouchMgr.SkillMode.pull))
        {
            touchMgr.ChangeMode(TouchMgr.SkillMode.chat);
            OffPanelEffect();
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
        modeCheck = true;
        UsePanelEffect(2);
    }
    private void EV3()
    {
        touchMgr.ChangeMode(chat.prevMode);
        timer.StartTime();
        touchMgr.canFire = true;
    }

    private IEnumerator FadeInOutBarrier()
    {
        ps.NewSkillSound();
        barrier[2].SetActive(true);
        yield return new WaitForSeconds(3.0f);
        barrier[2].SetActive(false);
        CallChat();
    }
}
