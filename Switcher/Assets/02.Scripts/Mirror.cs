using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    private Ray reflectRay;
    private RaycastHit reflectHit;
    private Rigidbody pullObjectRb;

    public LineRenderer laser;

    void Start()
    {
        laser.enabled = false;
    }

    // 인력을 반사
    public void ReflectRay(Vector3 hitPos, Vector3 direction)
    {
        laser.SetPosition(0, hitPos);
        if (!laser.enabled)
        {
            laser.enabled = true;
        }

        reflectRay = new Ray(hitPos, direction);
        if (Physics.Raycast(reflectRay, out reflectHit, 12))
        {
            laser.SetPosition(1, reflectHit.point);
            pullObjectRb = reflectHit.collider.gameObject.GetComponent<Rigidbody>();
            float distance = Vector3.Distance(reflectHit.point, transform.position);

            Vector3 targetPos = pullObjectRb.transform.position;
            Vector3 directionReverse = hitPos - targetPos;
            directionReverse = directionReverse.normalized;
            float dist = reflectHit.distance;
            if(dist < 1f){
                return;
            }
            float speed = 50 / (dist * dist);
            pullObjectRb.velocity = directionReverse * speed;
        }
        else
        {
            laser.SetPosition(1, hitPos + direction * 12);
        }
    }

    public void ReflectRayOff()
    {
        laser.enabled = false;
    }

}
