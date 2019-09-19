using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMgr : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private Camera cam;
    private Rigidbody bulletRb;
    private GameObject pointer;
    private Mirror mirror;
    private TranslateBullet tb;
    private bool canFire = true;
    private int manaStoneLayer;
    private int mirrorLayer;
    private Rigidbody pullObjectRb;
    private Transform playerTr;
    private SkillMode mode = SkillMode.switching;
    private enum SkillMode
    {
        switching, pull, push
    }

    public LineRenderer laser;
    public GameObject wind;
    public GameObject translateBullet;
    public GameObject blur;
    public GameObject[] ring;

    void Start()
    {
        manaStoneLayer = LayerMask.NameToLayer("MANASTONE");
        mirrorLayer = LayerMask.NameToLayer("MIRROR");
        cam = Camera.main;
        bulletRb = translateBullet.GetComponent<Rigidbody>();
        tb = translateBullet.GetComponent<TranslateBullet>();
        translateBullet.SetActive(false);
        pointer = GameObject.Find("Pointer");
        pointer.SetActive(false);
        laser.enabled = false;
        wind.SetActive(false);
        playerTr = GameObject.Find("Player").transform;
        blur.SetActive(false);
        for (int i = 0; i < ring.Length; i++)
        {
            ring[i].SetActive(false);
        }
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            switch (mode)
            {
                case SkillMode.switching:
                    mode = SkillMode.pull;
                    ring[0].SetActive(true);
                    break;
                case SkillMode.pull:
                    mode = SkillMode.push;
                    laser.enabled = false;
                    ring[0].SetActive(false);
                    ring[1].SetActive(true);
                    break;
                case SkillMode.push:
                    mode = SkillMode.switching;
                    wind.SetActive(false);
                    ring[1].SetActive(false);
                    break;
            }
            nullifyPullObj();
            return;
        }

        if (!canFire) return;

        switch (mode)
        {
            case SkillMode.switching:
                OnSwitching();
                break;
            case SkillMode.pull:
                OnPull();
                break;
            case SkillMode.push:
                OnPush();
                break;
        }
    }

    private void OnPush()
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            ray = new Ray(laser.transform.position, laser.transform.forward);
            if (!wind.activeSelf)
            {
                wind.SetActive(true);
            }
            if (Physics.Raycast(ray, out hit, 11))
            {
                if (!pointer.activeSelf)
                {
                    pointer.SetActive(true);
                }
                float dist = hit.distance;
                pointer.transform.position = hit.point;
                pointer.transform.LookAt(cam.transform.position);
                pointer.transform.position += pointer.transform.forward * 0.5f;

                if (hit.collider.gameObject.layer.Equals(manaStoneLayer))
                {
                    pullObjectRb = hit.collider.gameObject.GetComponent<Rigidbody>();
                    Vector3 targetPos = pullObjectRb.transform.position;
                    Vector3 direction = new Vector3(targetPos.x, 0, targetPos.z)
                                        - new Vector3(playerTr.position.x, 0, playerTr.position.z);
                    direction = direction.normalized;
                    float speed = 50 / (dist * dist);
                    pullObjectRb.velocity = direction * speed;
                }
                else
                {
                    nullifyPullObj();
                }
            }
            else
            {
                if (pointer.activeSelf)
                {
                    pointer.SetActive(false);
                }
            }
        }
        else
        {
            if (wind.activeSelf)
            {
                wind.SetActive(false);
            }
        }

        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            pointer.SetActive(false);
            nullifyPullObj();
        }
    }

    private void OnPull()
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            ray = new Ray(laser.transform.position, laser.transform.forward);
            if (Physics.Raycast(ray, out hit, 12))
            {
                if (!pointer.activeSelf)
                {
                    pointer.SetActive(true);
                }
                float dist = hit.distance;
                laser.enabled = true;
                laser.SetPosition(1, new Vector3(0, 0, dist));

                if (hit.collider.gameObject.layer.Equals(manaStoneLayer))
                {
                    pointer.transform.position = hit.point;
                    pointer.transform.LookAt(cam.transform.position);
                    pointer.transform.position += pointer.transform.forward * 0.5f;

                    pullObjectRb = hit.collider.gameObject.GetComponent<Rigidbody>();
                    float distance = Vector3.Distance(hit.point, playerTr.position);
                    if (distance < 2.5f)
                    {
                        pullObjectRb.velocity = Vector3.zero;
                        return;
                    }
                    Vector3 targetPos = pullObjectRb.transform.position;
                    Vector3 direction = playerTr.position - targetPos;
                    direction = direction.normalized;
                    float speed = 50 / (dist * dist);
                    pullObjectRb.velocity = direction * speed;
                }
                else if (hit.collider.gameObject.layer.Equals(mirrorLayer))
                {
                    pointer.transform.position = hit.point;
                    pointer.transform.LookAt(cam.transform.position);
                    pointer.transform.position += pointer.transform.forward * 0.5f;

                    Vector3 incoming = hit.point - laser.transform.position;
                    Vector3 normal = hit.normal;
                    Vector3 direction = Vector3.Reflect(incoming, normal).normalized;
                    Debug.DrawRay(hit.point, -incoming, Color.green, 100f);
                    Debug.DrawRay(hit.point, direction, Color.red, 100f);
                    if (!mirror)
                    {
                        mirror = hit.collider.gameObject.GetComponent<Mirror>();
                    }
                    mirror.ReflectRay(hit.point, direction);
                    return;
                }
                else
                {
                    if (pointer.activeSelf)
                    {
                        pointer.SetActive(false);
                    }
                    nullifyPullObj();
                }
            }
            else
            {
                if (pointer.activeSelf)
                {
                    pointer.SetActive(false);
                }
                laser.SetPosition(1, new Vector3(0, 0, 12));
                nullifyPullObj();
            }
        }

        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            pointer.SetActive(false);
            laser.enabled = false;
            nullifyPullObj();
        }
    }

    private void OnSwitching()
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            ray = new Ray(laser.transform.position, laser.transform.forward);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (!pointer.activeSelf)
                {
                    pointer.SetActive(true);
                }
                float dist = hit.distance;
                pointer.transform.position = hit.point;
                pointer.transform.LookAt(cam.transform.position);
                pointer.transform.position += pointer.transform.forward * 0.5f;
            }
            else
            {
                if (pointer.activeSelf)
                {
                    pointer.SetActive(false);
                }
            }
        }
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            translateBullet.SetActive(true);
            translateBullet.transform.position = laser.transform.position;
            Vector3 direction = pointer.transform.position - laser.transform.position;
            direction = direction.normalized;
            bulletRb.velocity = direction * 8f;
            tb.shootPos = laser.transform.position;
            pointer.SetActive(false);
            tb.DoActiveFalse();
            canFire = false;
        }
    }

    private void nullifyPullObj()
    {
        if (pullObjectRb)
        {
            pullObjectRb.velocity = Vector3.zero;
            pullObjectRb = null;
        }
        if (mirror)
        {
            mirror.ReflectRayOff();
            mirror = null;
        }
    }

    public void EnableFire(float waitTime)
    {
        Invoke("change2CanFire", waitTime);
    }

    private void change2CanFire()
    {
        canFire = true;
    }

    public void StartLerp(Transform objTr, Vector3 targetPos, Vector3 playerPos)
    {
        StopCoroutine("LerpAndTeleport");
        StartCoroutine(LerpAndTeleport(objTr.transform, targetPos, playerPos));
    }

    private IEnumerator LerpAndTeleport(Transform objTr, Vector3 targetPos, Vector3 playerPos)
    {
        int lerpFrame = 20;
        float lerpSpeed = 0.3f;
        Transform originPlayerTr = playerTr;
        Transform originObjTr = objTr;
        blur.SetActive(true);

        for (int i = 0; i < lerpFrame; i++)
        {
            playerTr.position = Vector3.Lerp(playerTr.position, targetPos, Time.deltaTime * lerpSpeed);
            objTr.position = Vector3.Lerp(objTr.position, playerPos, Time.deltaTime * lerpSpeed);
            yield return null;
        }

        playerTr.position = Vector3.Lerp(playerTr.position, targetPos, 0.8f);
        objTr.position = Vector3.Lerp(objTr.position, playerPos, 0.8f);

        lerpFrame = 40;
        lerpSpeed = 5f;
        for (int i = 0; i < lerpFrame; i++)
        {
            playerTr.position = Vector3.Lerp(playerTr.position, targetPos, Time.deltaTime * lerpSpeed);
            objTr.position = Vector3.Lerp(objTr.position, playerPos, Time.deltaTime * lerpSpeed);
            yield return null;
        }

        playerTr.position = targetPos;
        objTr.position = playerPos;
        blur.SetActive(false);
    }
}
