using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFocusing : MonoBehaviour
{
    private Vector3 targetPosition;
    private Transform pointerTr;
    public MagicCircle[] mc;
    private bool checkManstone = false;

    private void Start()
    {
        if (mc.Length != 0)
        {
            StartCoroutine("LaserMoveCheck");
        }
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
        while (!checkManstone)
        {
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
        while (checkManstone)
        {
            // laserFocusing 방향을 따라 회전
            targetPosition = new Vector3(pointerTr.position.x - 1.0f, pointerTr.position.y - 1.0f, pointerTr.position.z - 1.0f);
            transform.LookAt(targetPosition);
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
