using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private bool canDmg = true;
    public GameObject barrier;

    void Start()
    {
        barrier.SetActive(false);
    }

    public void DisableDmg(float waitTime)
    {
        canDmg = false;
        StartCoroutine(showBarrier(waitTime));
    }

    private IEnumerator showBarrier(float waitTime)
    {
        barrier.SetActive(true);
        yield return new WaitForSeconds(waitTime + 1f);
        barrier.SetActive(false);
        canDmg = true;
    }
}
