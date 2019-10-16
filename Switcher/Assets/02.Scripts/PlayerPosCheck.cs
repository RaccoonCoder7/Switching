using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosCheck : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    public Transform playerPos;
    public Transform controllerPos;
    float dis;
    TouchMgr touchMgr;
    Timer timer;
    bool wallCheck;
    public GameObject posEffect;
    bool check;
    // Start is called before the first frame update
    void Start()
    {
        touchMgr = GetComponent<TouchMgr>();
        timer = FindObjectOfType<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        dis = Vector3.Distance(playerPos.position, controllerPos.position);
        if (dis > 2.1f && timer.chatFinish)
        {
            if (!check)
            {
                touchMgr.canFire = false;
                check = true;
                posEffect.SetActive(true);
            }
            
            
        }
        else if(dis< 2.1f && timer.chatFinish)
        {
            if (check)
            {
                touchMgr.canFire = true;
                check = false;
                posEffect.SetActive(false);
            }
            
        }

        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            ray = new Ray(playerPos.position, controllerPos.position);
            if (Physics.Raycast(ray, out hit, dis, 1 << LayerMask.NameToLayer("WALL")))
            {
                touchMgr.canFire = false;
                wallCheck = true;
            }
        }
        

        if (wallCheck)
        {
            InvokeRepeating("OnRaycast", 0.1f, 0.1f);
            wallCheck = false;
        }

    }

    void OnRaycast()
    {
        ray = new Ray(playerPos.position, controllerPos.position);
        Debug.Log("asdasd");
        if(!Physics.Raycast(ray, out hit, dis, 1 << LayerMask.NameToLayer("WALL")))
        {
            CancelInvoke("OnRaycast");
            touchMgr.canFire = true;
        }
    }


}
