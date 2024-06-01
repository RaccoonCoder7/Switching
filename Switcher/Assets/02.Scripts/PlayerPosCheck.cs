using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPosCheck : MonoBehaviour
{
    public Transform playerPos;
    public Transform controllerPos;
    public GameObject warningScreen;
    public GameObject posEffect;
    public Text warningText;
    public Material posCheckMaterial;

    private int wallLayer;
    private string outOfRangeText = "자리를 벗어났습니다.\n빛이 나는 자리로 돌아가세요.";
    private string overTheWallText = "손이 벽을 넘었습니다.\n빛이 나는 자리로 돌아가세요.";
    private Ray ray;
    private RaycastHit hit;
    private float dist;
    private Vector3 dir;
    private bool isOutOfRange;
    private bool isOverTheWall;
    private Color color;
    private TouchMgr touchMgr;
    private Coroutine fadeInCoroutine;
    private Coroutine fadeOutCoroutine;


    void Start()
    {
        wallLayer = 1 << LayerMask.NameToLayer("WALL");
        touchMgr = GetComponent<TouchMgr>();
        color = posCheckMaterial.color;
        color.a = 0;
        posCheckMaterial.color = color;
    }

    void Update()
    {
        // 거리 확인
        dist = Vector3.Distance(playerPos.position, controllerPos.position);
        if (dist > 1.5f)
        {
            if (!isOutOfRange && !isOverTheWall)
            {
                isOutOfRange = true;
                SetActiveFire(false);
                if (fadeOutCoroutine != null)
                {
                    StopCoroutine(fadeOutCoroutine);
                }
                fadeInCoroutine = StartCoroutine(WarningFadeIn(outOfRangeText));
            }
            return;
        }
        else if (dist <= 1.5f)
        {
            if (isOutOfRange)
            {
                isOutOfRange = false;
                SetActiveFire(true);
                if (fadeInCoroutine != null)
                {
                    StopCoroutine(fadeInCoroutine);
                }
                fadeOutCoroutine = StartCoroutine(WarningFadeOut());
            }
        }

        // 벽 확인
        dir = (controllerPos.position - playerPos.position);
        ray = new Ray(playerPos.position, dir);
        if (Physics.Raycast(ray, out hit, dist, wallLayer))
        {
            if (!isOverTheWall)
            {
                isOverTheWall = true;
                SetActiveFire(false);
                if (fadeOutCoroutine != null)
                {
                    StopCoroutine(fadeOutCoroutine);
                }
                fadeInCoroutine = StartCoroutine(WarningFadeIn(overTheWallText));
            }
        }
        else
        {
            if (isOverTheWall)
            {
                isOverTheWall = false;
                SetActiveFire(true);
                if (fadeInCoroutine != null)
                {
                    StopCoroutine(fadeInCoroutine);
                }
                fadeOutCoroutine = StartCoroutine(WarningFadeOut());
            }
        }
    }

    private IEnumerator WarningFadeIn(string text)
    {
        warningScreen.SetActive(true);
        warningText.text = text;
        while (color.a < 0.8f)
        {
            color.a += 0.03f;
            posCheckMaterial.color = color;
            yield return new WaitForSeconds(0.02f);
        }
        fadeInCoroutine = null;
    }

    private IEnumerator WarningFadeOut()
    {
        while (color.a > 0f)
        {
            color.a -= 0.03f;
            posCheckMaterial.color = color;
            yield return new WaitForSeconds(0.02f);
        }
        warningScreen.SetActive(false);
        fadeOutCoroutine = null;
    }

    private void SetActiveFire(bool state)
    {
        touchMgr.canFire = state;
        posEffect.SetActive(!state);
    }
}
