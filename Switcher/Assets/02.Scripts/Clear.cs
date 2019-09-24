using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear : MonoBehaviour
{
    private GameMgr mgr;

    private void Start()
    {
        mgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("PLAYER")))
        {
            StartCoroutine(mgr.TestClear());
        }
    }
}
