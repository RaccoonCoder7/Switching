using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDedlegate;

public class EventMgr : MonoBehaviour
{
    protected Chat chat;

    protected Deleg[] EventList; // 이벤트들을 담아둘 곳

    protected GameObject[] barrier = new GameObject[4];
    protected GameObject barrier1;
    protected Timer timer;
    protected iTweenMgr iTween;
    public TouchMgr touchMgr;
    protected PlayerState ps;
    PlayerPosCheck pps;
    public GameObject playerAnchor;

    protected TouchFinger tf;

    protected void Start()
    {
        pps = FindObjectOfType<PlayerPosCheck>();
        pps.StartCoroutine("WarningFadeOut");
        GameObject player = GameObject.Find("Player");
        barrier[0] = player.transform.Find("SkillEffect1").gameObject;
        barrier[1] = player.transform.Find("SkillEffect2").gameObject;
        barrier[2] = player.transform.Find("SkillEffect3").gameObject;
        barrier[3] = player.transform.Find("SkillEffect4").gameObject;
        timer = FindObjectOfType<Timer>();
        timer.text.color = Color.white;
        timer.text.text = "00:00";
        touchMgr = player.GetComponent<TouchMgr>();
        ps = player.GetComponent<PlayerState>();
        touchMgr.canFire = false;
        touchMgr.laser.SetColors(Color.green, Color.green);
        tf = FindObjectOfType<TouchFinger>();
        tf.ActiveFalseBtn();
        //playerAnchor.transform.rotation = Quaternion.Euler(0, 0, 0);

        chat = FindObjectOfType<Chat>();
        EventList = chat.chatEventList;
    }

    // 이것을 호출하여 Chat의 NextChat을 실행시킴
    public void CallChat()
    {
        StartCoroutine("WaitAndCallChat");
    }

    // 패널의 버튼에 이펙트를 표시함. 이펙트의 위치(파라미터)는 아래와 같음.
    // 0: 전이, 1: 전이폭탄, 3: 인력, 4: 척력
    protected void UsePanelEffect(int index)
    {
        if (!iTween)
        {
            iTween = FindObjectOfType<iTweenMgr>();
        }
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            iTween.ForceCtrlEffect(true);
        }
        iTween.useEffect = true;
        iTween.ChangeEffectPos(index);
    }

    protected void OffPanelEffect()
    {
        iTween.ForceCtrlEffect(false);
        iTween.useEffect = false;
    }

    private IEnumerator WaitAndCallChat()
    {
        yield return new WaitForSeconds(0.1f);
        chat.gameObject.SetActive(true);
        chat.NextChat();
    }
}
