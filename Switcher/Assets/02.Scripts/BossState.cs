using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : MonoBehaviour
{
    private Transform camTr;
    private Vector3 targetPosition;

    private float attackTime = 10.0f;
    private float timer = 0.0f;

    public PolygonFireProjectile slowFire;

    private Animator anim;

    void Start()
    {
        camTr = Camera.main.GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 플레이어 방향을 따라 회전
        targetPosition = new Vector3(camTr.position.x, transform.position.y, camTr.position.z);
        transform.LookAt(targetPosition);

        // 타이머
        timer += Time.deltaTime;

        // 공격시간마다 공격
        if (timer > attackTime)
        {
            anim.SetTrigger("attack");
            StartCoroutine(slowFire.SlowFire());
            timer = 0.0f;
        }
    }
}
