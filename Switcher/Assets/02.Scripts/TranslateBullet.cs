using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateBullet : MonoBehaviour
{
    private int manaStoneLayer;
    private int manaStoneLayerCannon;
    private int bossLayer;
    private int mirrorLayer;
    private Transform playerTr;
    private TouchMgr touchMgr;
    private PlayerState playerState;
    private Ray ray;
    private RaycastHit hit;
    private Rigidbody rb;
    private AudioSource audio;
    private float diff = 0.82f;
    private GameObject clone;
    // private bool isReflected;

    public float speed = 10.0f;
    public Vector3 shootPos;
    public GameObject explosion;
    public AudioClip exlposionClip;
    public enum teleportStyle
    {
        teleport, lerp
    }
    public teleportStyle tpStyle = teleportStyle.teleport;

    void Start()
    {
        manaStoneLayer = LayerMask.NameToLayer("MANASTONE");
        manaStoneLayerCannon = LayerMask.NameToLayer("MANASTONE_C");
        bossLayer = LayerMask.NameToLayer("BOSS");
        mirrorLayer = LayerMask.NameToLayer("MIRROR");
        playerTr = GameObject.Find("Player").transform;
        touchMgr = playerTr.GetComponent<TouchMgr>();
        playerState = playerTr.GetComponent<PlayerState>();
        ray = new Ray();
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        clone = Instantiate(explosion);
        Destroy(clone);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("!!! " + other.collider.name);
        // if (isReflected)
        // {
        //     Debug.Log("!!! Cancel");
        //     return;
        // }

        if (other.gameObject.layer.Equals(mirrorLayer))
        {
            // StartCoroutine(ReflectDelay());
            Vector3 incoming = transform.position - shootPos;
            Debug.Log("!!! " + other.contacts[0].thisCollider.name);
            Vector3 normal = other.contacts[0].normal;
            Vector3 direction = Vector3.Reflect(incoming, normal).normalized;
            Debug.Log("!!! " + direction.x + "/" + direction.y + "/" + direction.z);
            rb.velocity = direction * 8f;
            audio.Play();
            return;
        }

        float waitTime = 0f;
        if (other.gameObject.layer.Equals(manaStoneLayer)
            || other.gameObject.layer.Equals(bossLayer) || other.gameObject.layer.Equals(manaStoneLayerCannon))
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Vector3 targetPos = other.transform.position;
            Vector3 playerPos = playerTr.transform.position;
            float targetPosY = targetPos.y;
            targetPos.y = targetPos.y - diff;
            playerPos.y = playerPos.y + diff / 2;

            if (tpStyle == teleportStyle.teleport)
            {
                playerTr.position = targetPos;
                other.transform.position = playerPos;
            }
            else if (tpStyle == teleportStyle.lerp)
            {
                touchMgr.StartLerp(other.transform, targetPos, playerPos);
            }
            waitTime = 1f;
            playerState.DisableDmg(waitTime);
        }
        else
        {
            audio.PlayOneShot(exlposionClip);
            GameObject exp = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(exp, 0.5f);
        }

        touchMgr.EnableFire(waitTime);
        gameObject.SetActive(false);

        StopCoroutine("activeFalseSelf");
    }

    // 전이능력을 비활성화
    public void DoActiveFalse()
    {
        StartCoroutine("ActiveFalseSelf");
    }

    private IEnumerator ActiveFalseSelf()
    {
        yield return new WaitForSeconds(3.0f);
        touchMgr.EnableFire(0f);
        gameObject.SetActive(false);
    }

    // private IEnumerator ReflectDelay()
    // {
    //     isReflected = true;
    //     yield return new WaitForSeconds(0.1f);
    //     isReflected = false;
    // }
}
