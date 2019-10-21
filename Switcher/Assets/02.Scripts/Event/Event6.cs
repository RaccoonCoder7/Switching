using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDedlegate;

public class Event6 : EventMgr
{
    bool modeCheck;
    protected BossChat bossChat;
    public GameObject bossChatCanvas;
    public BossState bossSt;
    public LaserFire[] laserFire; // stopTime 15
    public CircleBarLaser[] circleBarLaser;
    public bool clearCheck;
    public GameMgr gameMgr;

    private void Awake()
    {
        QualitySettings.SetQualityLevel(2, true);
    }

    void Start()
    {
        base.Start();
        bossChat = bossChatCanvas.GetComponent<BossChat>();
        gameMgr = FindObjectOfType<GameMgr>();
        bossChat.bossTextCount = 0;
        bossChat.paragraphCnt = 0;
        bossChat.chat = chat;
        bossChatCanvas.SetActive(false);
        EventList[0] = new Deleg(EV1);
        EventList[1] = new Deleg(EV2);
        EventList[2] = new Deleg(EV3);
        EventList[3] = new Deleg(EV4);
        EventList[4] = new Deleg(EV5);
        EventList[5] = new Deleg(EV6);
    }
    private void Update()
    {
        if (clearCheck)
        {

        }
    }

    private void EV1()
    {
        bossChatCanvas.SetActive(true);
        bossChat.BossTextSet();
        //CallBossChat();
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
        bossSt.isChat = false;
        for (int i = 0; i < laserFire.Length; i++)
        {
            laserFire[i].stopTime = 15;
            laserFire[i].timer = 0.0f;
        }

        for (int i = 0; i < circleBarLaser.Length; i++)
        {
            circleBarLaser[i].goCheck = true;
        }
        touchMgr.ChangeMode(TouchMgr.SkillMode.switching);
        timer.StartTime();
    }
    private void EV6()
    {
        gameMgr.StartCoroutine("FinishFadeInOut");
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
