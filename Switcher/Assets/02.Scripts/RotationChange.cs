using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationChange : MonoBehaviour
{
    // 마나스톤 각도 조절 및 회전
    private void OnCollisionEnter(Collision collision)
    {
        transform.rotation = Quaternion.Euler(-90.0f, transform.rotation.y, transform.rotation.z);
    }

    // 올바른 마법진에 매칭되었을 때 회전
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == gameObject.tag)
        {
            transform.Rotate(new Vector3(0, 0, 0.5f));
        }
    }
}
