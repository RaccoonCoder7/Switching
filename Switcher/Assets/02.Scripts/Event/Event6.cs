using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDedlegate;

public class Event6 : EventMgr
{
    bool modeCheck;
    protected BossChat bossChat;
    public GameObject bossChatCanvas;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        bossChat = bossChatCanvas.GetComponent<BossChat>();
        bossChat.bossTextCount = 1;
        bossChat.paragraphCnt = 0;
        bossChatCanvas.SetActive(false);
        EventList[0] = new Deleg(EV1);
        EventList[1] = new Deleg(EV2);
        EventList[2] = new Deleg(EV3);
        EventList[3] = new Deleg(EV4);
        EventList[4] = new Deleg(EV5);
    }

    private void EV1()
    {
        bossChatCanvas.SetActive(true);
        bossChat.BossTextSet();
    }
    private void EV2()
    {
        CallChat();
    }
    private void EV3()
    {
        CallBossChat();
    }
    private void EV4()
    {
        CallChat();
    }
    private void EV5()
    {
        touchMgr.canFire = true;
        timer.StartTime();
    }

    protected void CallBossChat()
    {
        StartCoroutine("WaitAndCallBossChat");
    }

    private IEnumerator WaitAndCallBossChat()
    {
        yield return new WaitForSeconds(0.1f);
        bossChatCanvas.SetActive(true);
        bossChat.NextChat();
    }
}
