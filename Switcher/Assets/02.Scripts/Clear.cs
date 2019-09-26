using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear : MonoBehaviour
{
    // private GameMgr mgr;
    private StageCtrl sc;

    private void Start()
    {
        // mgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();
        sc = FindObjectOfType<StageCtrl>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("PLAYER")))
        {
            // StartCoroutine(mgr.TestClear());
            sc.StartCoroutine(sc.ClearStage());
        }
    }
}
