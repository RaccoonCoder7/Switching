using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : MonoBehaviour
{
    private Transform camTr;
    private Vector3 targetPosition;

    private float attackTime = 8.0f;
    private float timer = 0.0f;

    // 보스 알파값 줄어드는 시간
    private float timeA = 1.0f;

    public PolygonFireProjectile slowFire;

    private Animator anim;

    // 보스 죽음
    [HideInInspector]
    public bool isDeath = false;
    [HideInInspector]
    public int deathCount = 0;

    // 보스 부활시간
    private float resurrectionTime = 2.0f;

    // 죽음리셋 시간
    private float deathResetTime = 1.0f;

    private Renderer render;

    private AudioSource audio;
    public AudioClip bossRewindClip;
    public AudioClip bossDieClip;

    void Start()
    {
        camTr = Camera.main.GetComponent<Transform>();
        anim = gameObject.transform.GetComponent<Animator>();
        render = GetComponent<Renderer>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 플레이어 방향을 따라 회전
        targetPosition = new Vector3(camTr.position.x, transform.position.y, camTr.position.z);
        transform.LookAt(targetPosition);

        // 타이머
        timer += Time.deltaTime;

        // 공격시간마다 공격
        if (timer > attackTime && !isDeath && deathCount != 2)
        {
            anim.SetTrigger("attack");
            StartCoroutine(slowFire.SlowFire());
            timer = 0.0f;
        }

        // 보스 가짜 죽음
        if (isDeath)
        {
            isDeath = false;
            anim.SetBool("death", true);
            StartCoroutine("BossResurrection");
        }

        // 보스 진짜 죽음
        if (deathCount == 2)
        {
            if(timeA > 0)
            {
                timeA -= Time.deltaTime;
                anim.SetBool("realDeath", true);
                render.material.color = new Color(1, 1, 1, timeA);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    // 보스 부활
    private IEnumerator BossResurrection()
    {
        yield return new WaitForSeconds(resurrectionTime);
        anim.SetBool("death", false);
        deathCount = 1;

        // 죽음 초기화
        yield return new WaitForSeconds(deathResetTime);
        if (deathCount == 1)
        {
            deathCount = 0;
        }
    }
}
