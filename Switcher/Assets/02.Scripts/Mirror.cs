using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    private Ray reflectRay;
    private RaycastHit reflectHit;
    private Rigidbody pullObjectRb;
    private int manaStoneLayer;
    private RigidbodyConstraints originRbConst;
    private RigidbodyConstraints movingRbConst;
    private GameObject pullEffClone;
    private VRCSDK2.VRC_MirrorReflection reflectionModule;
    private static Transform playerTr;

    public LineRenderer laser;

    void Start()
    {
        manaStoneLayer = 1 << LayerMask.NameToLayer("MANASTONE");
        laser.enabled = false;
        originRbConst = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX
                        | RigidbodyConstraints.FreezePositionZ;
        movingRbConst = RigidbodyConstraints.FreezeRotation;

        if (playerTr == null)
        {
            playerTr = FindObjectOfType<TouchMgr>().transform;
        }
        if (reflectionModule == null)
        {
            reflectionModule = GetComponent<VRCSDK2.VRC_MirrorReflection>();
        }
    }

    void Update()
    {
        if (playerTr)
        {
            var dist = Vector3.Distance(transform.position, playerTr.position);
            bool enable = dist < 25f;
            if (reflectionModule.enabled != enable)
            {
                reflectionModule.enabled = enable;
            }
        }
    }

    // 인력을 반사
    public void ReflectRay(Vector3 hitPos, Vector3 direction, GameObject pullEffect)
    {
        laser.SetPosition(0, hitPos);
        if (!laser.enabled)
        {
            laser.enabled = true;
        }

        reflectRay = new Ray(hitPos, direction);

        if (Physics.Raycast(reflectRay, out reflectHit, 12, manaStoneLayer))
        {
            laser.SetPosition(1, reflectHit.point);

            if (!pullObjectRb)
            {
                pullObjectRb = reflectHit.collider.gameObject.GetComponent<Rigidbody>();
                if (!pullEffClone)
                {
                    pullEffClone = pullEffect;
                }
                pullEffClone.SetActive(true);
                pullEffClone.transform.position = reflectHit.transform.position;
                pullEffClone.transform.parent = reflectHit.collider.gameObject.transform;
                pullObjectRb.constraints = movingRbConst;
            }

            float distance = Vector3.Distance(reflectHit.point, transform.position);
            Vector3 targetPos = pullObjectRb.transform.position;
            Vector3 directionReverse = hitPos - targetPos;
            directionReverse = directionReverse.normalized;

            float dist = reflectHit.distance;
            if (dist < 1f)
            {
                pullObjectRb.velocity = Vector3.zero;
                return;
            }

            float speed = 4f;
            pullObjectRb.velocity = directionReverse * speed;
        }
        else
        {
            laser.SetPosition(1, hitPos + direction * 12);
            if (pullObjectRb)
            {
                pullObjectRb.constraints = originRbConst;
                if (pullEffClone)
                {
                    pullEffClone.SetActive(false);
                    pullEffClone.transform.parent = null;
                    pullEffClone = null;
                }
                pullObjectRb.velocity = Vector3.zero;
                pullObjectRb = null;
            }
        }
    }

    public void ReflectRayOff()
    {
        laser.enabled = false;
    }

}
