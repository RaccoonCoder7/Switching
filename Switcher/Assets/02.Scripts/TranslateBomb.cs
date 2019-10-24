using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateBomb : MonoBehaviour
{
    private GameObject clone;
    private int skillButtonLayer;
    private int touchFingerLayer;

    public TouchMgr touchMgr;
    public GameObject bombArea;
    public bool isTestBomb = false;

    void Start()
    {
        skillButtonLayer = LayerMask.NameToLayer("SKILLBUTTON");
        touchFingerLayer = LayerMask.NameToLayer("TOUCHFINGER");
        if (!isTestBomb)
        {
            clone = Instantiate(bombArea);
            Destroy(clone);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(skillButtonLayer)
            || other.gameObject.layer.Equals(touchFingerLayer))
        {
            return;
        }
        // GameObject area = Instantiate(bombArea, transform.position, transform.rotation);
        if (isTestBomb)
        {
            touchMgr.testBombs.Remove(gameObject);
        }
        else
        {
            Instantiate(bombArea, transform.position, transform.rotation);
        }
        gameObject.SetActive(false);
    }
}
