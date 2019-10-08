using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event1a : EventMgr
{
    private int abilityLayer;

    public bool isReady;
    public GameObject arrow;

    void Start()
    {
        base.Start();
        abilityLayer = LayerMask.NameToLayer("ABILITY");
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!isReady) return;

        // 능력에 닿았을 경우 CallChat 실행
        if (other.gameObject.layer.Equals(abilityLayer))
        {
            isReady = false;
            arrow.SetActive(false);
            touchMgr.ChangeMode(TouchMgr.SkillMode.chat);
            CallChat();
        }
    }
}
