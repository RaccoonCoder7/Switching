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
            if (playing.Equals(false))
            {
                MoveR();
            }
        }
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            triggerDown = true;
            if (playing.Equals(false))
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
        playing = false;
        if (triggerDown.Equals(true))
        {
            MoveL();
        }
        triggerDown = false;
        
    }
    void CheckTriggerUp()
    {
        playing = false;
        if(triggerUp.Equals(true)){
            MoveR();
        }
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
