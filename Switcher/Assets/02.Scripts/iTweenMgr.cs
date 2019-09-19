using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iTweenMgr : MonoBehaviour
{
    public GameObject timerCanvas;
    public Animator anim;
    float x;
    bool playing;
    bool triggerUp;
    bool triggerDown;



    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            triggerUp = true;
            triggerDown = false;
            //마법
            //if (!playing)
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

    }
    public void MoveLeft()
    {
            playing = true;
            Hashtable ht1 = new Hashtable();
            ht1.Add("x", -0.32f);
            ht1.Add("time", 1f);
            ht1.Add("easetype", iTween.EaseType.easeInBack);
            ht1.Add("oncomplete", "CheckTriggerUp");
            iTween.MoveBy(timerCanvas, ht1);

    }
    public void MoveRight()
    {
            playing = true;
            Hashtable ht1 = new Hashtable();
            ht1.Add("x", 0.32f);
            ht1.Add("time", 1f);
            ht1.Add("easetype", iTween.EaseType.easeOutBack);
            ht1.Add("oncompletetarget", this.gameObject);
            ht1.Add("oncomplete", "CheckTriggerDown");
            iTween.MoveBy(timerCanvas, ht1);
    }

    void CheckTriggerDown()
    {
        
        if (triggerDown.Equals(true))
        {
            MoveL();
        }
        playing = false;
        triggerDown = false;

    }
    void CheckTriggerUp()
    {
        
        if(triggerUp.Equals(true)){
            MoveR();
        }
        playing = false;
        triggerUp = false;
    }



    void MoveL()
    {
        anim.SetBool("PanelUp", false);
    }

    void MoveR()
    {
        
        anim.SetBool("PanelUp", true);
    }
}
