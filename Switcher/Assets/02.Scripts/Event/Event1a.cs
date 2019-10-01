using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event1a : EventMgr
{
    private int abilityLayer;
    private TouchMgr touchMgr;

    public bool isReady;
    public GameObject arrow;

    void Start()
    {
        base.Start();
        abilityLayer = LayerMask.NameToLayer("ABILITY");
        GameObject player = GameObject.Find("Player");
        touchMgr = player.GetComponent<TouchMgr>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!isReady) return;

        if (other.gameObject.layer.Equals(abilityLayer))
        {
            isReady = false;
            arrow.SetActive(false);
            touchMgr.ChangeMode(TouchMgr.SkillMode.chat);
            CallChat();
        }
    }
}
