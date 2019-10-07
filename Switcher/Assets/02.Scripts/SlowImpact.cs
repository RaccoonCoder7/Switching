using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowImpact : MonoBehaviour
{
    private float PlayerTrBullet;

    private void Start()
    {
        PlayerTrBullet = GameObject.Find("Player").GetComponent<TouchMgr>().trBullet;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer.Equals(LayerMask.NameToLayer("TURRET")))
        {
            // 터렛 레이저 발사시간 조절
            LaserFire laser = collision.gameObject.GetComponentInChildren<LaserFire>();
            laser.stopTime = 16.0f;
            laser.shootTime = 0.5f;
            laser.slowEffect.SetActive(true);
        }
        else if (collision.collider.gameObject.layer.Equals(LayerMask.NameToLayer("PLAYER")))
        {
            // 플레이어 탄환 발사시간 조절(전이)
            TouchMgr tMgr = collision.gameObject.GetComponent<TouchMgr>();
            tMgr.slowTime = 7.0f;
            tMgr.slowEffect.SetActive(true);
        }
    }
}
