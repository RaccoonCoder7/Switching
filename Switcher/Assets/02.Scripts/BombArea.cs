using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombArea : MonoBehaviour
{
    private int manaStoneLayer;
    private Transform playerTr;
    private TouchMgr touchMgr;
    private PlayerState playerState;
    private Vector3 shortestPos;
    private float minDistance = 10000f;
    private Transform targetTr;
    // private List<Transform> objList = new List<Transform>();
    // private List<Vector3> objPosList = new List<Vector3>();
    private float diff = 0.82f;

    public enum teleportStyle
    {
        teleport, lerp
    }
    public teleportStyle tpStyle = teleportStyle.teleport;

    void Start()
    {
        manaStoneLayer = LayerMask.NameToLayer("MANASTONE");
        playerTr = GameObject.Find("Player").transform;
        touchMgr = playerTr.GetComponent<TouchMgr>();
        playerState = playerTr.GetComponent<PlayerState>();
        StartCoroutine("Translation");
    }

    // 전체 전이
    IEnumerator Translation()
    {
        yield return new WaitForSeconds(0.6f);
        float waitTime = 0.2f;
        if (!minDistance.Equals(10000f))
        {
            shortestPos.y = shortestPos.y - diff;
            if (tpStyle.Equals(teleportStyle.teleport))
            {
                // for (int i = 0; i < objList.Count; i++)
                // {
                //     objList[i].position = objPosList[i];
                // }
                targetTr.position = playerTr.position;
                playerTr.position = shortestPos;
            }
            else if (tpStyle == teleportStyle.lerp)
            {
                touchMgr.StartLerp(targetTr, playerTr.position, shortestPos);
            }
            waitTime = 1f;
            playerState.DisableDmg(waitTime);
        }
        touchMgr.EnableFire(waitTime);
        touchMgr.EnableFireTestBomb(waitTime);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(manaStoneLayer))
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Vector3 targetPos = other.transform.position;
            // Vector3 direction = targetPos - transform.position;
            float distance = Vector3.Distance(targetPos, transform.position);
            // Vector3 playerPos = playerTr.transform.position;

            if (minDistance > distance)
            {
                minDistance = distance;
                shortestPos = targetPos;
                targetTr = other.transform;
            }

            // float playerY = playerPos.y;
            // playerPos += direction;
            // playerPos.y = playerY;
            // playerPos.y = playerPos.y + diff/2;

            // objList.Add(other.transform);
            // objPosList.Add(playerPos);
        }
    }
}
