using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFocusing : MonoBehaviour
{
    public Transform laserTr;
    private Vector3 targetPosition;

    void Update()
    {
        // laserFocusing 방향을 따라 회전
        targetPosition = new Vector3(laserTr.position.x, laserTr.position.y, laserTr.position.z);
        transform.LookAt(targetPosition);
    }
}
