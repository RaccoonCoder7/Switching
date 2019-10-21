using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : MonoBehaviour
{


    public GameObject startBtn;
    public GameObject continueBtn;
    public GameObject onOffText;
    bool start;
    bool checkText;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("OnOffText", 0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger) && !start)
        {
            start = true;
            CancelInvoke("OnOffText");
            onOffText.SetActive(false);
            Invoke("ButtonOn",1f);
            
        }
    }

    void OnOffText()
    {
        if (!checkText)
        {
            onOffText.SetActive(false);
            checkText = true;
        }
        else
        {
            onOffText.SetActive(true);
            checkText = false;
        }
    }
    void ButtonOn()
    {
        startBtn.SetActive(true);
        if (GameMgr.stage.Equals(0))
        {
            continueBtn.SetActive(false);
        }
    }
}
