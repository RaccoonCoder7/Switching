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
            collision.gameObject.GetComponentInChildren<LaserFire>().stopTime = 16.0f;
            collision.gameObject.GetComponentInChildren<LaserFire>().shootTime = 0.5f;
        }
        else if (collision.collider.gameObject.layer.Equals(LayerMask.NameToLayer("PLAYER")))
        {
            // 플레이어 탄환 발사시간 조절(전이)
            collision.gameObject.GetComponent<TouchMgr>().slowTime = 3.0f;
        }
    }
}
