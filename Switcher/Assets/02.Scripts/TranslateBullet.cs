using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateBullet : MonoBehaviour
{
    private int manaStoneLayer;
    private int bossLayer;
    private int wireLayer;
    private int mirrorLayer;
    private Transform playerTr;
    private TouchMgr touchMgr;
    private PlayerState playerState;
    private Ray ray;
    private RaycastHit hit;
    private Rigidbody rb;
    private AudioSource audio;
    private float diff = 0.82f;

    public float speed = 10.0f;
    public Vector3 shootPos;
    public enum teleportStyle
    {
        teleport, lerp
    }
    public teleportStyle tpStyle = teleportStyle.teleport;

    void Start()
    {
        manaStoneLayer = LayerMask.NameToLayer("MANASTONE");
        bossLayer = LayerMask.NameToLayer("BOSS");
        wireLayer = LayerMask.NameToLayer("WIRE");
        mirrorLayer = LayerMask.NameToLayer("MIRROR");
        playerTr = GameObject.Find("Player").transform;
        touchMgr = playerTr.GetComponent<TouchMgr>();
        playerState = playerTr.GetComponent<PlayerState>();
        ray = new Ray();
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer.Equals(mirrorLayer))
        {
            Vector3 incoming = transform.position - shootPos;
            Vector3 normal = other.contacts[0].normal;
            Vector3 direction = Vector3.Reflect(incoming, normal).normalized;
            rb.velocity = direction * 8f;
            audio.Play();
            return;
        }

        float waitTime = 0f;
        if (other.gameObject.layer.Equals(manaStoneLayer) 
            || other.gameObject.layer.Equals(bossLayer))
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Vector3 targetPos = other.transform.position;
            Vector3 playerPos = playerTr.transform.position;
            float targetPosY = targetPos.y;
            targetPos.y = targetPos.y - diff;
            playerPos.y = playerPos.y + diff/2;

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

        if (!other.gameObject.layer.Equals(wireLayer))
        {
            touchMgr.EnableFire(waitTime);
            gameObject.SetActive(false);
            StopCoroutine("activeFalseSelf");
        }
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
}
