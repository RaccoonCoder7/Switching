using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowImpact : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer.Equals(LayerMask.NameToLayer("TURRET")))
        {
            // 터렛 레이저 발사시간 조절
            collision.gameObject.GetComponentInChildren<LaserFire>().stopTime = 15.0f;
        }else if (collision.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            // 플레이어 탄환 발사시간 조절(전이)
            // collision.gameObject.
        }
    }
}
