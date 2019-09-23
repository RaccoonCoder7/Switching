using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchFinger : MonoBehaviour
{
    private int skillButtonLayer;
    private TouchMgr touchMgr;
    private Collider sphere;

    public ImageCtrl imageCtrl;

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
            TouchMgr.SkillMode mode = TouchMgr.SkillMode.switching;
            string buttonName = other.gameObject.name;
            switch (buttonName)
            {
                case "switching":
                    mode = TouchMgr.SkillMode.switching;
                    break;
                case "pull":
                    mode = TouchMgr.SkillMode.pull;
                    break;
                case "push":
                    mode = TouchMgr.SkillMode.push;
                    break;
                case "switchBomb":
                    mode = TouchMgr.SkillMode.switchBomb;
                    break;
            }
            touchMgr.ChangeMode(mode);
            imageCtrl.ChangeSprites(touchMgr.mode);
        }
    }
}
