using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchFinger : MonoBehaviour
{
    private int skillButtonLayer;
    private TouchMgr touchMgr;
    private Collider sphere;

    void Start()
    {
        skillButtonLayer = LayerMask.NameToLayer("SKILLBUTTON");
        touchMgr = GameObject.Find("Player").GetComponent<TouchMgr>();
        sphere = GetComponent<Collider>();
        sphere.enabled = false;
    }

    private void Update()
    {
        if (!OVRInput.Get(OVRInput.NearTouch.SecondaryIndexTrigger))
        {
            sphere.enabled = true;
        }
        else
        {
            sphere.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(skillButtonLayer))
        {
            string buttonName = other.gameObject.name;

            switch (buttonName)
            {
                case "switching":
                    touchMgr.mode = TouchMgr.SkillMode.switching;
                    break;
                case "pull":
                    touchMgr.mode = TouchMgr.SkillMode.pull;
                    break;
                case "push":
                    touchMgr.mode = TouchMgr.SkillMode.push;
                    break;
                case "switchBomb":
                    touchMgr.mode = TouchMgr.SkillMode.switchBomb;
                    break;
            }
        }
    }
}
