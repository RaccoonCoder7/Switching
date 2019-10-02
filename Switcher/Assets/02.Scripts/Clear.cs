using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear : MonoBehaviour
{
    // private GameMgr mgr;
    private StageCtrl sc;
    private bool clearDelay = true;

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
            if (clearDelay)
            {
                sc.StartCoroutine(sc.ClearStage());
                clearDelay = false;
            }
        }
    }

    private IEnumerator SetTime()
    {
        yield return new WaitForSeconds(10.0f);
        clearDelay = true;
    }
}
