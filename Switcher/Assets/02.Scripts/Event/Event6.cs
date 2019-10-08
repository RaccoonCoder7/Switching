using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDedlegate;

public class Event6 : EventMgr
{
    bool modeCheck;
    protected BossChat bossChat;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        bossChat = FindObjectOfType<BossChat>();

        EventList[0] = new Deleg(EV1);
        EventList[1] = new Deleg(EV2);
        EventList[2] = new Deleg(EV3);
    }
    private void Update()
    {

    }

    private void EV1()
    {
        CallChat();
    }
    private void EV2()
    {
        CallBossChat();
    }
    private void EV3()
    {
        CallChat();
    }

    protected void CallBossChat()
    {
        StartCoroutine("WaitAndCallBossChat");
    }

    private IEnumerator WaitAndCallBossChat()
    {
        yield return new WaitForSeconds(0.1f);
        bossChat.gameObject.SetActive(true);
        bossChat.NextChat();
    }
}
