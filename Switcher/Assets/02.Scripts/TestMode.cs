﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestMode : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private GameMgr gameMgr;
    private AudioSource audio;
    private TestMode testMode;
    private TouchMgr touchMgr;
    private PlayerState playerState;
    private StageCtrl sc;
    private GameObject chatCanvas;
    private iTweenMgr iTween;
    private int startBtnLayer;
    private int continueBtnLayer;
    private int retryBtnLayer;
    private int UIButtonLayer;
    private bool isStarted;

    private Image img;
    public Color originalColor;
    public Color pressedColor;

    public LineRenderer laser;
    public AudioClip UISound;
    iTweenMgr itMgr;

    private int layerMask;

    void Awake()
    {
        touchMgr = GetComponent<TouchMgr>();
        touchMgr.enabled = false;
        playerState = GetComponent<PlayerState>();
        playerState.enabled = false;
        chatCanvas = transform.Find("ChatCanvas").gameObject;
        chatCanvas.SetActive(false);
        itMgr = GetComponent<iTweenMgr>();
    }

    void Start()
    {
        iTween = FindObjectOfType<iTweenMgr>();
        gameMgr = FindObjectOfType<GameMgr>();
        audio = GetComponent<AudioSource>();
        touchMgr = GetComponent<TouchMgr>();
        touchMgr.enabled = false;
        playerState = GetComponent<PlayerState>();
        playerState.enabled = false;
        sc = FindObjectOfType<StageCtrl>();
        testMode = this;
        startBtnLayer = LayerMask.NameToLayer("START");
        continueBtnLayer = LayerMask.NameToLayer("CONTINUE");
        retryBtnLayer = (1 << LayerMask.NameToLayer("RETRY")) & 
            (((-1) - (1 << LayerMask.NameToLayer("PLAYER"))) | 
            ((-1) - (1 << LayerMask.NameToLayer("SKILLBUTTON"))));
        UIButtonLayer = LayerMask.NameToLayer("UIBUTTON");



        // SKILLBUTTON, PLAYER 레이어만 제외하고 충돌 체크함
        layerMask = ((1 << LayerMask.NameToLayer("PLAYER")) |
            (1 << LayerMask.NameToLayer("SKILLBUTTON")));
        layerMask = ~layerMask;

        DontDestroyOnLoad(gameObject);
        laser.SetColors(Color.green, Color.green);
    }

    void Update()
    {
        ray = new Ray(laser.transform.position, laser.transform.forward);
        if (isStarted)
        {
            if (Physics.Raycast(ray, out hit, 5.0f, layerMask))
            {
                float dist = hit.distance;
                laser.SetPosition(1, new Vector3(0, 0, dist));
            }
            else
            {
                laser.SetPosition(1, new Vector3(0, 0, 5));
            }
        }
        else
        {
            if (Physics.Raycast(ray, out hit, 12.0f, layerMask))
            {
                float dist = hit.distance;
                laser.SetPosition(1, new Vector3(0, 0, dist));
            }
            else
            {
                laser.SetPosition(1, new Vector3(0, 0, 12));
            }
            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.collider.tag.Equals("UIBUTTON"))
                    {
                        img = hit.transform.GetComponent<Image>();
                        originalColor = img.color;
                        img.color = pressedColor;
                    }
                }
            }
            if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
            {
                if (img)
                {
                    img.color = originalColor;
                }
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.collider.tag.Equals("UIBUTTON"))
                    {
                        int hitLayer = hit.collider.gameObject.layer;
                        audio.PlayOneShot(UISound);
                        if (hitLayer.Equals(UIButtonLayer))
                        {

                            hit.collider.gameObject.GetComponent<Button>().onClick.Invoke();
                            return;
                        }
                        if (hitLayer.Equals(startBtnLayer))
                        {
                            StartCoroutine(StartGame(1));
                        }
                        else if (hitLayer.Equals(continueBtnLayer))
                        {
                            StartCoroutine(StartGame(gameMgr.GetPrevStageNum()));
                        }
                    }
                }
            }

            // TODO: 테스트용 코드 지우기
            // if (Input.GetMouseButtonUp(0))
            // {
            //     StartCoroutine(StartGame(1));
            //     // PlayerPrefs.SetInt("isCleared", 1);
            // }
        }
    }

    public IEnumerator StartGame(int stageNum)
    {
        isStarted = true;
        if (stageNum.Equals(1))
        {
            gameMgr.NewGame();
        }
        gameMgr.ChangeScreanImage();
        yield return StartCoroutine(gameMgr.FadeIn());
        yield return StartCoroutine(sc.CreateStageAsync(stageNum, true));
        touchMgr.enabled = true;
        sc.touchMgr = touchMgr;
        playerState.enabled = true;
        yield return StartCoroutine(gameMgr.FadeOut());
        iTween.isEnable = true;
        testMode.enabled = false;
    }
}
