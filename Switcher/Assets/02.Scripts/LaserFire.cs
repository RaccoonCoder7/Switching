using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFire : MonoBehaviour
{
    // 레이저
    public GameObject beamStart;
    public GameObject beamEnd;
    public GameObject beam;
    private LineRenderer line;
    private RaycastHit hit;

    // 레이저 발사 시간대
    public float shootTime = 5.0f;
    public float stopTime = 1.0f;
    private float timer = 0.0f;

    // 보스 상태
    public GameObject boss;
    private BossState bossState;
    private Animator bossAnim;

    // 플레이어 상태
    private PlayerState playerSt;

    void Start()
    {
        // 위치 초기화 및 생성
        beamStart = Instantiate(beamStart, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform.parent) as GameObject;
        beamEnd = Instantiate(beamEnd, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform.parent) as GameObject;
        beam = Instantiate(beam, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform.parent) as GameObject;
        line = beam.GetComponent<LineRenderer>();
        if (boss)
        {
            bossState = boss.GetComponent<BossState>();
            bossAnim = boss.GetComponent<Animator>();
        }
        BeamActive(false);
    }

    void Update()
    {
        // 타이머
        timer += Time.deltaTime;

        // 레이저 활성화 체크
        if (beam.activeSelf)
        {
            if (timer > shootTime)
            {
                BeamActive(false);
                timer = 0.0f;
            }

            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
            {
                ShootBeam();
            }

            // boss가 죽어있을 경우에는 raycast가 충돌판정 안함
            //if (bossAnim && bossAnim.GetBool("death"))
            //{
            //    if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity,
            //    ~(1 << LayerMask.NameToLayer("BOSS"))))
            //    {
            //        ShootBeam();
            //    }
            //}
            //else
            //{

            //}
        }
        else
        {
            if (timer > stopTime)
            {
                BeamActive(true);
                timer = 0.0f;
            }
        }
    }

    // 레이저 비활성화, 활성화
    private void BeamActive(bool shootFl)
    {
        beamStart.SetActive(shootFl);
        beamEnd.SetActive(shootFl);
        beam.SetActive(shootFl);
    }

    // 레이저 위치 설정
    private void ShootBeam()
    {
        // 레이저 시작지점
        line.SetPosition(0, transform.position);
        beamStart.transform.position = transform.position;

        // 레이저 끝지점
        beamEnd.transform.position = hit.point;
        line.SetPosition(1, hit.point);

        // 플레이어의 경우 플레이어 사망
        if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("PLAYER")))
        {
            // 플레이어 사망
            if(!playerSt) {
                Debug.Log("111");
                playerSt = FindObjectOfType<PlayerState>();
            }
            Debug.Log(playerSt);
            Debug.Log(playerSt.isDead);
            if (!playerSt.isDead)
            {
                Debug.Log("222");
                playerSt.PlayerDie();
            }
        }
        else if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("BOSS")))
        {
            // 보스 사망
            if (bossState.deathCount == 1)
            {
                bossState.deathCount = 2;
            }
            bossState.isDeath = true;
        }
    }
}
