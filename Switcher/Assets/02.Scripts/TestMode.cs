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

    Image img;
    public Color originalColor;
    public Color pressedColor;

    public LineRenderer laser;
    public AudioClip UISound;

    void Awake()
    {
        touchMgr = GetComponent<TouchMgr>();
        touchMgr.enabled = false;
        playerState = GetComponent<PlayerState>();
        playerState.enabled = false;
        chatCanvas = transform.Find("ChatCanvas").gameObject;
        chatCanvas.SetActive(false);
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
        DontDestroyOnLoad(gameObject);
        laser.SetColors(Color.green, Color.green);
    }

    void Update()
    {
        ray = new Ray(laser.transform.position, laser.transform.forward);
        if (Physics.Raycast(ray, out hit, 16.0f))
        {
            float dist = hit.distance;
            laser.SetPosition(1, new Vector3(0, 0, dist));
        }
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("START")))
            {
                img = hit.transform.GetComponent<Image>();
                img.color = pressedColor;
                
            }
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("CONTINUE")))
            {
                img = hit.transform.GetComponent<Image>();
                img.color = pressedColor;
            }
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("RETRY")))
            {
                img = hit.transform.GetComponent<Image>();
                img.color = pressedColor;
            }
        }
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            img.color = originalColor;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("START")))
            {
                
                StartCoroutine(StartGame(1));
            }
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("CONTINUE")))
            {
                StartCoroutine(StartGame(gameMgr.GetPrevStageNum()));
            }
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("RETRY")))
            {
                StartCoroutine(sc.ResetStage(null));
            }
        }

        // TODO: 테스트용 코드 지우기
        // if (Input.GetMouseButtonUp(0))
        // {
        //     StartCoroutine(StartGame(1));
        // }
    }


    private IEnumerator StartGame(int stageNum)
    {
        if (stageNum.Equals(1))
        {
            gameMgr.NewGame();
        }
        audio.PlayOneShot(UISound);
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
