using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour
{
    private bool canDmg = true;
    private AudioSource audio;
    private StageCtrl sc;

    public bool isDead;
    public AudioClip[] stateClips;
    public AudioClip newSkillClip;
    public GameObject barrier;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        barrier.SetActive(false);
        sc = FindObjectOfType<StageCtrl>();
    }

    // 플레이어가 데미지를 받지 않도록 설정
    public void DisableDmg(float waitTime)
    {
        audio.PlayOneShot(stateClips[0]);
        canDmg = false;
        StartCoroutine(ShowBarrier(waitTime));
    }

    // 배리어 생성
    private IEnumerator ShowBarrier(float waitTime)
    {
        barrier.SetActive(true);
        yield return new WaitForSeconds(waitTime + 0.5f);
        barrier.SetActive(false);
        canDmg = true;
    }

    // 플레이어 죽기
    public void PlayerDie()
    {
        isDead = true;
        audio.PlayOneShot(stateClips[1]);
        OVRInput.SetControllerVibration(0.5f, 0.5f, OVRInput.Controller.RTouch);
        OVRInput.SetControllerVibration(0.5f, 0.5f, OVRInput.Controller.LTouch);
        sc.StartCoroutine(sc.ResetStage(stateClips[2]));
        // mgr.Continue();
    }

    public void NewSkillSound()
    {
        audio.PlayOneShot(newSkillClip);
    }
}
