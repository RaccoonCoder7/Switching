using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateBomb : MonoBehaviour
{
    private GameObject clone;
    private TouchMgr touchMgr;

    public GameObject bombArea;
    public bool isTestBomb = false;

    void Start()
    {
        clone = Instantiate(bombArea);
        Destroy(clone);
        if (isTestBomb)
        {
            touchMgr = GameObject.Find("Player").GetComponent<TouchMgr>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject area = Instantiate(bombArea, transform.position, transform.rotation);
        if (isTestBomb)
        {
            touchMgr.EnableFireTestBomb(1f);
            Destroy(area, 0.8f);
        }
        gameObject.SetActive(false);
    }
}
