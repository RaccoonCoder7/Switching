using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDedlegate;

public class iTweenMgr : MonoBehaviour
{
    public GameObject timerCanvas;
    public GameObject[] timerPanel;
    public Animator anim;
    public GameObject buttonEffect;
    public List<Transform> buttons = new List<Transform>();
    float x;
    bool playing;
    bool triggerUp;
    bool triggerDown;
    private StageCtrl sc;
    public GameObject chatCanvas;
    Chat chat;
    Timer timer;

    public bool isEnable;
    public bool useEffect;

    private void Awake()
    {
        timer = FindObjectOfType<Timer>();
        chat = chatCanvas.GetComponent<Chat>();
        chatCanvas.SetActive(false);
    }

    private void Start()
    {
        sc = FindObjectOfType<StageCtrl>();
        buttons.Add(timerCanvas.transform.Find("switching"));
        buttons.Add(timerCanvas.transform.Find("switchBomb"));
        buttons.Add(timerCanvas.transform.Find("pull"));
        buttons.Add(timerCanvas.transform.Find("push"));
    }


    private void Update()
    {
        if (!isEnable) return;
        //if(triggerDown && triggerUp)
        //{
        //    timerCanvas.SetActive(false);
        //}
        //else
        //{
        //    timerCanvas.SetActive(true);
        //}

        //전환패널
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (sc.GetStageNum().Equals(1))
            {
                return;
            }
            triggerUp = true;
            triggerDown = false;

            //if (playing.Equals(false))
            //{
            MoveR();
            //}

        }
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            triggerDown = true;
            triggerUp = false;
            if (!playing)
            {
                MoveL();
            }
        }
        //조력자 소환
        if (timer.chatFinish)
        {
            if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
            {
                if (!chat.helpCheck)
                {
                    chatCanvas.SetActive(true);
                    chat.CallHelper();
                }
                else
                {
                    chat.FadeHelper();
                }
            }
        }
    }

    public void MoveLeft()
    {
        playing = true;
        Hashtable ht1 = new Hashtable();
        ht1.Add("x", -0.32f);
        ht1.Add("time", 0.25f);
        ht1.Add("easetype", iTween.EaseType.easeInBack);
        ht1.Add("oncomplete", "CheckTriggerUp");
        iTween.MoveBy(timerCanvas, ht1);

    }

    public void MoveRight()
    {
        playing = true;
        Hashtable ht1 = new Hashtable();
        ht1.Add("x", 0.32f);
        ht1.Add("time", 0.25f);
        ht1.Add("easetype", iTween.EaseType.easeOutBack);
        ht1.Add("oncompletetarget", this.gameObject);
        ht1.Add("oncomplete", "CheckTriggerDown");
        iTween.MoveBy(timerCanvas, ht1);
    }

    public void ForceCtrlEffect(bool b)
    {
        buttonEffect.SetActive(b);
    }

    public void ChangeEffectPos(int index)
    {
        Vector3 pos = new Vector3(buttons[index].localPosition.x
                                , buttons[index].localPosition.y
                                , buttonEffect.transform.localPosition.z);
        buttonEffect.transform.localPosition = pos;
    }

    void CheckTriggerDown()
    {
        playing = false;
        if (useEffect)
        {
            buttonEffect.SetActive(true);
        }
        if (triggerDown)
        {
            MoveL();
        }
        triggerDown = false;

    }
    void CheckTriggerUp()
    {
        playing = false;
        if (triggerUp)
        {
            MoveR();
        }
        triggerUp = false;
    }



    void MoveL()
    {
        if (useEffect)
        {
            buttonEffect.SetActive(false);
        }
        anim.SetBool("PanelUp", false);
    }

    void MoveR()
    {
        anim.SetBool("PanelUp", true);
    }

    void PanelFalse()
    {
        for (int i = 0; i < timerPanel.Length; i++)
        {
            timerPanel[i].SetActive(false);
        }
    }
    void PanelTrue()
    {
        for (int i = 0; i < timerPanel.Length; i++)
        {
            timerPanel[i].SetActive(true);
        }
    }
}
