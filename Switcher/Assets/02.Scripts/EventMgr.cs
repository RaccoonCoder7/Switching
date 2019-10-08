using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDedlegate;

public class EventMgr : MonoBehaviour
{
    protected Chat chat;
    protected BossChat bossChat;
    protected Deleg[] EventList; // 이벤트들을 담아둘 곳

    protected GameObject barrier;
    protected Timer timer;
    protected TouchMgr touchMgr;

    protected void Start()
    {
        GameObject player = GameObject.Find("Player");
        barrier = player.transform.Find("BeamupCylinderGreen").gameObject;
        timer = FindObjectOfType<Timer>();
        touchMgr = player.GetComponent<TouchMgr>();
        touchMgr.canFire = false;

        chat = FindObjectOfType<Chat>();
        EventList = chat.chatEventList;
    }

    // 이것을 호출하여 Chat의 NextChat을 실행시킴
    protected void CallChat()
    {
        StartCoroutine("WaitAndCallChat");
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

    private IEnumerator WaitAndCallChat()
    {
        yield return new WaitForSeconds(0.1f);
        chat.gameObject.SetActive(true);
        chat.NextChat();
    }
}
