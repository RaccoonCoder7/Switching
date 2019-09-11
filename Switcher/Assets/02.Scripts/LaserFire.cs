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

    void Start()
    {
        // 위치 초기화 및 생성
        beamStart = Instantiate(beamStart, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        beamEnd = Instantiate(beamEnd, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        beam = Instantiate(beam, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        line = beam.GetComponent<LineRenderer>();
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
            ShootBeam();
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
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            // 레이저 시작지점
            line.SetPosition(0, transform.position);
            beamStart.transform.position = transform.position;

            // 레이저 끝지점
            beamEnd.transform.position = hit.point;
            line.SetPosition(1, hit.point);
        }
    }
}
