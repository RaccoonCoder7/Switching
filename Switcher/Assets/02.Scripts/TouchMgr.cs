using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMgr : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private Camera cam;
    private Rigidbody bulletRb;
    private Rigidbody bombRb;
    private Rigidbody testBombRb;
    public GameObject pointer;
    private Mirror mirror;
    private TranslateBullet tb;
    public bool canFire = true;
    private bool canFireTestBomb = true;
    private int manaStoneLayer;
    private int manaStoneCannonLayer;
    private int mirrorLayer;
    private Rigidbody pullObjectRb;
    private Transform playerTr;
    private float coolTime;
    private AudioSource audio;
    private Rigidbody rb;
    private RigidbodyConstraints originRbConst;
    private RigidbodyConstraints movingRbConst;
    private GameObject pullEffClone;
    private PlayerState ps;

    public SkillMode mode = SkillMode.chat;
    public enum SkillMode
    {
        switching, pull, push, switchBomb, chat
    }
    public LineRenderer laser;
    public GameObject wind;
    public GameObject translateBullet;
    public GameObject translateBomb;
    public GameObject testTranslateBomb;
    public GameObject blur;
    public GameObject pullEffect;
    public List<GameObject> testBombs = new List<GameObject>();
    public GameObject[] ring;
    public AudioClip[] shootClips;
    public GameObject translateSlowBullet;
    public float bombSpeed = 500.0f;

    // 레이져 사거리(인력, 척력) 조정
    public int laserRange = 12;
    public float trBullet = 10.0f;

    // 느려짐지속시간
    [HideInInspector]
    public float slowTime = 0.0f;

    // 슬로우 이펙트
    public GameObject slowEffect;

    // 인력 머테리얼
    public GameObject pullLaser;
    public GameObject pullParticle;

    // 밀고있는 척력 오브젝트
    private GameObject pullObjectNow;

    public GameObject laserCtrl;

    private Rigidbody bulletRbSlow;
    private TranslateBullet tbSlow;
    private Rigidbody bulletRbOrig;
    private TranslateBullet tbOrig;

    void Start()
    {
        manaStoneLayer = LayerMask.NameToLayer("MANASTONE");
        manaStoneCannonLayer = LayerMask.NameToLayer("MANASTONE_C");
        mirrorLayer = LayerMask.NameToLayer("MIRROR");
        cam = Camera.main;
        translateBullet = Instantiate(translateBullet);
        translateSlowBullet = Instantiate(translateSlowBullet);
        ps = GetComponent<PlayerState>();
        ps.translateBullet = translateBullet;
        ps.translateSlowBullet = translateSlowBullet;
        bulletRb = translateBullet.GetComponent<Rigidbody>();
        bulletRbOrig = translateBullet.GetComponent<Rigidbody>();
        bulletRbSlow = translateSlowBullet.GetComponent<Rigidbody>();
        translateBomb = Instantiate(translateBomb);
        ps.translateBomb = translateBomb;
        bombRb = translateBomb.GetComponent<Rigidbody>();
        translateBomb.SetActive(false);
        GameObject clone = Instantiate(testTranslateBomb);
        Destroy(clone);
        // testBombRb = testTranslateBomb.GetComponent<Rigidbody>();
        tb = translateBullet.GetComponent<TranslateBullet>();
        tbOrig = translateBullet.GetComponent<TranslateBullet>();
        tbSlow = translateSlowBullet.GetComponent<TranslateBullet>();
        translateBullet.SetActive(false);
        translateSlowBullet.SetActive(false);
        pointer = GameObject.Find("Pointer");
        pointer.SetActive(false);
        laser.enabled = false;
        wind.SetActive(false);
        pullParticle.SetActive(false);
        pullLaser.SetActive(false);
        playerTr = GameObject.Find("Player").transform;
        blur.SetActive(false);
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        slowEffect.SetActive(false);
        originRbConst = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX
                        | RigidbodyConstraints.FreezePositionZ;
        movingRbConst = RigidbodyConstraints.FreezeRotation;
        pullEffClone = Instantiate(pullEffect);
        pullEffClone.SetActive(false);

        for (int i = 0; i < ring.Length; i++)
        {
            ring[i].SetActive(false);
        }
        ChangeMode(TouchMgr.SkillMode.chat);
    }

    void Update()
    {
        if (slowTime > 0.0f)
        {
            slowTime -= Time.deltaTime;
            trBullet = 3.0f;
            if (slowTime <= 0.1f)
            {
                trBullet = 10.0f;
            }
        }
        else if (slowEffect.activeSelf)
        {
            slowEffect.SetActive(false);
        }

        if (!canFire || ps.isDead) return;

        switch (mode)
        {
            case SkillMode.switching:
                OnSwitching();
                break;
            case SkillMode.pull:
                //laser.SetColors(new Color(0, 138, 255), new Color(0, 138, 255));
                pullLaser.SetActive(true);
                OnPull();
                break;
            case SkillMode.push:
                OnPush();
                break;
            case SkillMode.switchBomb:
                OnSwitchBomb();
                break;
            case SkillMode.chat:
                laser.SetPosition(1, new Vector3(0, 0, 5));
                laser.SetColors(Color.green, Color.green);
                return;
        }
    }

    private void OnSwitchBomb()
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (!canFireTestBomb) return;
            GameObject cloneTb = Instantiate(testTranslateBomb, laser.transform.position, laser.transform.rotation);
            TranslateBomb tb = cloneTb.GetComponent<TranslateBomb>();
            tb.touchMgr = this;
            // testTranslateBomb.transform.position = laser.transform.position;
            // testBombRb.velocity = Vector3.zero;
            testBombs.Add(cloneTb);
            cloneTb.GetComponent<Rigidbody>().AddForce(laser.transform.forward * bombSpeed);
            canFireTestBomb = false;
            EnableFireTestBomb(0.5f);
        }

        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            for (int i = 0; i < testBombs.Count; i++)
            {
                Destroy(testBombs[i]);
            }
            testBombs = new List<GameObject>();
            audio.Play();
            translateBomb.SetActive(true);
            translateBomb.transform.position = laser.transform.position;
            bombRb.velocity = Vector3.zero;
            bombRb.AddForce(laser.transform.forward * bombSpeed);
            canFire = false;
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
            if (!audio.isPlaying)
            {
                audio.Play();
            }
            if (Physics.Raycast(ray, out hit, laserRange - 1.5f))
            {
                if (!pointer.activeSelf)
                {
                    pointer.SetActive(true);
                }
                float dist = hit.distance;
                pointer.transform.position = hit.point;
                pointer.transform.LookAt(cam.transform.position);
                pointer.transform.position += pointer.transform.forward * 0.2f;

                if (hit.collider.gameObject.layer.Equals(manaStoneLayer) ||
                    hit.collider.gameObject.layer.Equals(manaStoneCannonLayer))
                {
                    if (!pullObjectRb || pullObjectNow != hit.collider.gameObject)
                    {
                        pullObjectNow = hit.collider.gameObject;
                        pullObjectRb = hit.collider.gameObject.GetComponent<Rigidbody>();
                        pullObjectRb.constraints = movingRbConst;
                    }
                    Vector3 targetPos = pullObjectRb.transform.position;
                    Vector3 direction = new Vector3(targetPos.x, 0, targetPos.z)
                                        - new Vector3(playerTr.position.x, 0, playerTr.position.z);
                    direction = direction.normalized;
                    float speed = 4f;
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
            audio.Stop();
            pointer.SetActive(false);
            nullifyPullObj();
        }
    }

    private void OnPull()
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }
            ray = new Ray(laser.transform.position, laser.transform.forward);
            if (Physics.Raycast(ray, out hit, laserRange))
            {
                if (!pointer.activeSelf)
                {
                    pointer.SetActive(true);
                }
                float dist = hit.distance;
                //laser.enabled = true;
                laser.SetPosition(1, new Vector3(0, 0, dist));

                if (hit.collider.gameObject.layer.Equals(manaStoneLayer) ||
                    hit.collider.gameObject.layer.Equals(manaStoneCannonLayer))
                {
                    pointer.transform.position = hit.point;
                    pointer.transform.LookAt(cam.transform.position);
                    pointer.transform.position += pointer.transform.forward * 0.2f;

                    if (!pullObjectRb)
                    {
                        pullObjectRb = hit.collider.gameObject.GetComponent<Rigidbody>();
                        if (!pullEffClone)
                        {
                            pullEffClone = Instantiate(pullEffect);
                        }
                        pullEffClone.SetActive(true);
                        pullEffClone.transform.position = hit.transform.position;
                        pullEffClone.transform.parent = hit.collider.gameObject.transform;
                        pullObjectRb.constraints = movingRbConst;
                    }
                    float distance = Vector3.Distance(hit.collider.gameObject.transform.position, 
                        playerTr.position);
                    if (distance < 2.5f)
                    {
                        pullObjectRb.velocity = Vector3.zero;
                        return;
                    }
                    Vector3 targetPos = pullObjectRb.transform.position;
                    Vector3 direction = playerTr.position - targetPos;
                    direction = direction.normalized;
                    float speed = 4f;
                    pullObjectRb.velocity = direction * speed;
                }
                else if (hit.collider.gameObject.layer.Equals(mirrorLayer))
                {
                    pointer.transform.position = hit.point;
                    pointer.transform.LookAt(cam.transform.position);
                    pointer.transform.position += pointer.transform.forward * 0.2f;

                    Vector3 incoming = hit.point - laser.transform.position;
                    Vector3 normal = hit.normal;
                    Vector3 direction = Vector3.Reflect(incoming, normal).normalized;
                    if (!mirror)
                    {
                        mirror = hit.collider.gameObject.GetComponent<Mirror>();
                    }
                    if (!pullEffClone)
                    {
                        pullEffClone = Instantiate(pullEffect);
                    }
                    mirror.ReflectRay(hit.point, direction, pullEffClone);
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

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && !pullParticle.activeSelf)
        {
            pullParticle.SetActive(true);
        }

        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            audio.Stop();
            pointer.SetActive(false);
            laser.enabled = false;
            nullifyPullObj();
            pullParticle.SetActive(false);
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
                pointer.transform.position += pointer.transform.forward * 0.2f;
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
            audio.Play();
            if (slowTime > 0.0f)
            {
                translateSlowBullet.SetActive(true);
                translateSlowBullet.transform.position = laser.transform.position;
                Vector3 direction = pointer.transform.position - laser.transform.position;
                direction = direction.normalized;
                bulletRbSlow.velocity = direction * trBullet;
                tbSlow.shootPos = laser.transform.position;
                pointer.SetActive(false);
                tbSlow.DoActiveFalse();
                canFire = false;
            }
            else if (slowTime <= 0.1f)
            {
                translateBullet.SetActive(true);
                translateBullet.transform.position = laser.transform.position;
                Vector3 direction = pointer.transform.position - laser.transform.position;
                direction = direction.normalized;
                bulletRb.velocity = direction * trBullet;
                //bulletRb.velocity = direction * 8f;
                tb.shootPos = laser.transform.position;
                pointer.SetActive(false);
                tb.DoActiveFalse();
                canFire = false;
            }
        }
    }

    private void nullifyPullObj()
    {
        if (pullObjectRb)
        {
            pullObjectRb.constraints = originRbConst;
            pullObjectRb.velocity = Vector3.zero;
            pullObjectRb = null;
        }
        if (mirror)
        {
            mirror.ReflectRayOff();
            mirror = null;
        }
        if (pullEffClone)
        {
            pullEffClone.transform.parent = null;
            pullEffClone.SetActive(false);
        }
    }

    public void ChangeMode(SkillMode skillMode)
    {
        mode = skillMode;
        switch (mode)
        {
            case SkillMode.switching:
                audio.loop = false;
                audio.clip = shootClips[0];
                ring[0].SetActive(false);
                ring[1].SetActive(false);
                break;
            case SkillMode.pull:
                audio.loop = true;
                audio.clip = shootClips[1];
                ring[0].SetActive(true);
                ring[1].SetActive(false);
                break;
            case SkillMode.push:
                audio.loop = true;
                audio.clip = shootClips[2];
                ring[0].SetActive(false);
                ring[1].SetActive(true);
                break;
            case SkillMode.switchBomb:
                audio.loop = false;
                audio.clip = shootClips[3];
                ring[0].SetActive(false);
                ring[1].SetActive(false);
                break;
            case SkillMode.chat:
                audio.loop = false;
                audio.clip = null;
                ring[0].SetActive(false);
                ring[1].SetActive(false);
                break;
        }
        if (pullLaser.activeSelf)
        {
            pullLaser.SetActive(false);
        }
        laser.enabled = false;
        wind.SetActive(false);
        pointer.SetActive(false);
        nullifyPullObj();
        return;
    }

    public void EnableFire(float waitTime)
    {
        Invoke("change2CanFire", waitTime + 0.5f);
    }

    private void change2CanFire()
    {
        canFire = true;
    }

    public void EnableFireTestBomb(float waitTime)
    {
        Invoke("change2CanFireTestBomb", waitTime);
    }

    private void change2CanFireTestBomb()
    {
        canFireTestBomb = true;
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
        rb.isKinematic = true;

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
        rb.isKinematic = false;
    }

    // public void StartLerpAll(List<Transform> objList, List<Vector3> objPosList, Vector3 farthestPos)
    // {
    //     StartCoroutine(LerpAndTeleportAll(objList, objPosList, farthestPos));
    // }

    // private IEnumerator LerpAndTeleportAll(List<Transform> objList, List<Vector3> objPosList, Vector3 farthestPos)
    // {
    //     int lerpFrame = 20;
    //     float lerpSpeed = 0.3f;
    //     Transform originPlayerTr = playerTr;
    //     blur.SetActive(true);
    //     rb.isKinematic = true;

    //     for (int i = 0; i < lerpFrame; i++)
    //     {
    //         playerTr.position = Vector3.Lerp(playerTr.position, farthestPos, Time.deltaTime * lerpSpeed);
    //         for (int j = 0; j < objList.Count; j++)
    //         {
    //             objList[j].position = Vector3.Lerp(objList[j].position, objPosList[j], Time.deltaTime * lerpSpeed);
    //         }
    //         yield return null;
    //     }

    //     playerTr.position = Vector3.Lerp(playerTr.position, farthestPos, 0.8f);
    //     for (int i = 0; i < objList.Count; i++)
    //     {
    //         objList[i].position = Vector3.Lerp(objList[i].position, objPosList[i], 0.8f);
    //     }

    //     lerpFrame = 40;
    //     lerpSpeed = 5f;
    //     for (int i = 0; i < lerpFrame; i++)
    //     {
    //         playerTr.position = Vector3.Lerp(playerTr.position, farthestPos, Time.deltaTime * lerpSpeed);
    //         for (int j = 0; j < objList.Count; j++)
    //         {
    //             objList[j].position = Vector3.Lerp(objList[j].position, objPosList[j], Time.deltaTime * lerpSpeed);
    //         }
    //         yield return null;
    //     }

    //     playerTr.position = farthestPos;
    //     for (int i = 0; i < objList.Count; i++)
    //     {
    //         objList[i].position = objPosList[i];
    //     }
    //     blur.SetActive(false);
    //     rb.isKinematic = false;
    // }
}
