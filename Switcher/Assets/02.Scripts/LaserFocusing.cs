using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFocusing : MonoBehaviour
{
    private Vector3 targetPosition;
    private Transform pointerTr;
    public MagicCircle[] mc;
    private bool checkManstone = false;
    public ParticleSystem canCtrl;
    public Transform playerTr;
    private Vector3 playerPos;
    public Event6 ev;
    public bool isLaserCtrl = false;
    public Material whiteVer;
    public Material blackVer;
    private Material myMaterial;

    private void Start()
    {
        if (mc.Length != 0)
        {
            StartCoroutine("LaserMoveCheck");
        }

        playerPos = new Vector3(playerTr.position.x, playerTr.position.y, playerTr.position.z);
        myMaterial = GetComponent<Material>();
    }

    private void Update()
    {
        if (!pointerTr)
        {
            pointerTr = GameObject.Find("Player").GetComponent<TouchMgr>().pointer.GetComponent<Transform>();
        }
    }

    // 레이저가 움직일 조건을 갖췄는지 체크
    IEnumerator LaserMoveCheck()
    {
        canCtrl.Stop();
        ev.touchMgr.laserCtrl.SetActive(false);
        myMaterial = blackVer;
        while (!checkManstone)
        {
            // 원래 위치로 방향 전환
            for (int i = 0; i < mc.Length; i++)
            {
                if (i == 0) checkManstone = true;

                checkManstone = checkManstone && mc[i].manastone;
            }
            yield return null;
        }
        StartCoroutine("LaserNoMoveCheck");
    }

    // 레이저가 안움직일 조건을 갖췄는지 체크
    IEnumerator LaserNoMoveCheck()
    {
        canCtrl.Play();
        ev.touchMgr.laserCtrl.SetActive(true);
        myMaterial = whiteVer;
        while (checkManstone)
        {
            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) &&
                ev.touchMgr.mode != TouchMgr.SkillMode.chat)
            {
                // laserFocusing 방향을 따라 회전
                targetPosition = new Vector3(pointerTr.position.x - 1.0f, pointerTr.position.y - 1.0f, pointerTr.position.z - 1.0f);
                transform.LookAt(targetPosition);
            }
            
            if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
            {
                transform.LookAt(playerPos);
            }

            for (int i = 0; i < mc.Length; i++)
            {
                if (i == 0) checkManstone = true;

                checkManstone = checkManstone && mc[i].manastone;
            }
            yield return null;
        }
        StartCoroutine("LaserMoveCheck");
    }
}
