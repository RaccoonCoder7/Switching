using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDedlegate;

public class EventMgr : MonoBehaviour
{
    protected Chat chat;
    protected Deleg[] EventList;

    protected void Start()
    {
        chat = FindObjectOfType<Chat>();
        EventList = chat.chatEventList;
    }

    protected void CallChat()
    {
        StartCoroutine("WaitAndCallChat");
    }

    private IEnumerator WaitAndCallChat()
    {
        yield return new WaitForSeconds(0.1f);
        chat.gameObject.SetActive(true);
        chat.NextChat();
    }
}
