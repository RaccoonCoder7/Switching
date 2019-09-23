using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateBomb : MonoBehaviour
{
    private GameObject clone;
    private TouchMgr touchMgr;
    private int skillButtonLayer;
    private int touchFingerLayer;

    public GameObject bombArea;
    public bool isTestBomb = false;

    void Start()
    {
        skillButtonLayer = LayerMask.NameToLayer("SKILLBUTTON");
        touchFingerLayer = LayerMask.NameToLayer("TOUCHFINGER");
        clone = Instantiate(bombArea);
        Destroy(clone);
        if (isTestBomb)
        {
            touchMgr = GameObject.Find("Player").GetComponent<TouchMgr>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(skillButtonLayer)
            || other.gameObject.layer.Equals(touchFingerLayer))
        {
            return;
        }
        GameObject area = Instantiate(bombArea, transform.position, transform.rotation);
        if (isTestBomb)
        {
            touchMgr.EnableFireTestBomb(1f);
            Destroy(area, 0.8f);
        }
        gameObject.SetActive(false);
    }
}
