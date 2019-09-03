using UnityEngine;
using System.Collections;
using System;

public class LookAt : MonoBehaviour
{
    public Transform target = null;
    public float rotationSpeed = 10f;
    public bool findCamera = true;
    public Vector3 offset = new Vector3();

    void Start()
    {
        if (findCamera)
        {
            if (target == null)
            {
                if (Camera.allCamerasCount > 0)
                {
                    target = Camera.allCameras[0].transform;
                }
            }
        }
    }

    public void FixedUpdate()
    {
        if (target != null)
        {
            Quaternion intendedRotation = Quaternion.LookRotation(target.position - transform.position);
            intendedRotation = Quaternion.Euler(intendedRotation.eulerAngles + offset);
            transform.rotation = Quaternion.Slerp(transform.rotation, intendedRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
