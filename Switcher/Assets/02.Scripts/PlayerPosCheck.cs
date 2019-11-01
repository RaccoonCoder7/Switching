using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPosCheck : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    public Transform playerPos;
    public Transform controllerPos;
    float dis;
    Vector3 dir;
    TouchMgr touchMgr;
    Timer timer;
    bool wallCheck;
    public GameObject posEffect;
    bool check;
    bool canCheck;

    public GameObject warningScreen;
    public GameObject warningText;
    public Image warningImage;
    public Material posCheckMaterial;
    Color color;


    // Start is called before the first frame update
    void Start()
    {
        touchMgr = GetComponent<TouchMgr>();
        timer = FindObjectOfType<Timer>();
        warningImage = warningScreen.GetComponent<Image>();
        color = posCheckMaterial.color;
        color.a = 0;
        posCheckMaterial.color = color;

    }

    // Update is called once per frame
    void Update()
    {
        dis = Vector3.Distance(playerPos.position, controllerPos.position);   
        if (dis > 1.5f)
        {
            if (!check)
            {
                touchMgr.canFire = false;
                check = true;
                posEffect.SetActive(true);
                StopCoroutine("WarningFadeOut");
                StartCoroutine("WarningFadeIn");
            }
        }
        else if(dis< 1.5f)
        {
            if (check)
            {
                touchMgr.canFire = true;
                check = false;
                posEffect.SetActive(false);
                StopCoroutine("WarningFadeIn");
                StartCoroutine("WarningFadeOut");
            }
        }

        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            dir = (controllerPos.position - playerPos.position);
            ray = new Ray(playerPos.position, dir);
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
        dir = (controllerPos.position - playerPos.position);
        ray = new Ray(playerPos.localPosition, dir);
        if(!Physics.Raycast(ray, out hit, dis, 1 << LayerMask.NameToLayer("WALL")))
        {
            CancelInvoke("OnRaycast");
            touchMgr.canFire = true;
        }
    }

    public IEnumerator WarningFadeIn()
    {
        warningScreen.SetActive(true);
        while (color.a < 0.8f)
        {
            color.a += 0.03f;
            posCheckMaterial.color = color;
            yield return new WaitForSeconds(0.02f);
        }
        warningText.SetActive(true);
        //color.a = 0f;
        //fadeMaterial.color = color;
    }

    public IEnumerator WarningFadeOut()
    {
        warningText.SetActive(false);
        while (color.a > 0f)
        {
            color.a -= 0.03f;
            posCheckMaterial.color = color;
            yield return new WaitForSeconds(0.02f);
        }
        warningScreen.SetActive(false);

    }




}
