using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour
{
    private bool canDmg = true;
    private AudioSource audio;

    public AudioClip[] stateClips;
    public GameObject barrier;
    private GameMgr mgr;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        barrier.SetActive(false);
        mgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();
    }

    public void DisableDmg(float waitTime)
    {
        audio.PlayOneShot(stateClips[0]);
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

    public void PlayerDeath()
    {
        mgr.Continue();
    }
}
