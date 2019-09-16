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

    // 보스 애니메이션
    private Animator animBoss;
    private float resurrectionTime = 3.0f;

    void Start()
    {
        // 위치 초기화 및 생성
        beamStart = Instantiate(beamStart, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        beamEnd = Instantiate(beamEnd, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        beam = Instantiate(beam, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        line = beam.GetComponent<LineRenderer>();
        animBoss = GameObject.Find("boss").GetComponent<Animator>();
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

            // boss가 죽어있을 경우에는 raycast가 충돌판정 안함
            if (animBoss.GetBool("death"))
            {
                if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity,
                ~(1 << LayerMask.NameToLayer("BOSS"))))
                {
                    ShootBeam();
                }
            }
            else
            {
                if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
                {
                    ShootBeam();
                }
            }
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
        if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            // 플레이어 사망
        }
        else if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("BOSS")))
        {
            // 보스 사망
            animBoss.SetBool("death", true);
            StartCoroutine("BossResurrection");
        }
    }

    private IEnumerator BossResurrection()
    {
        yield return new WaitForSeconds(resurrectionTime);
        animBoss.SetBool("death", false);
    }
}
