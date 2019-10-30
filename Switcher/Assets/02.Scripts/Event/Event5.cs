using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDedlegate;

public class Event5 : EventMgr
{
    bool modeCheck;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        EventList[0] = new Deleg(EV1);
    }

    private void EV1()
    {
        touchMgr.ChangeMode(TouchMgr.SkillMode.switching);
        timer.StartTime();
        touchMgr.canFire = true;
    }
}
