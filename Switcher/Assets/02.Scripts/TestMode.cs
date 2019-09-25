using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public LineRenderer laser;
    public AudioClip UISound;

    void Start()
    {
        gameMgr = FindObjectOfType<GameMgr>();
        audio = GetComponent<AudioSource>();
        touchMgr = GetComponent<TouchMgr>();
        touchMgr.enabled = false;
        playerState = GetComponent<PlayerState>();
        playerState.enabled = false;
        sc = FindObjectOfType<StageCtrl>();
        testMode = this;
    }

    void Update()
    {
        ray = new Ray(laser.transform.position, laser.transform.forward);
        if (Physics.Raycast(ray, out hit, 16.0f))
        {
            float dist = hit.distance;
            laser.SetPosition(1, new Vector3(0, 0, dist));
        }
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("START")))
            {
                StartGame(1);
            }
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("CONTINUE")))
            {
                StartGame(gameMgr.GetPrevStageNum());
            }
        }
    }

    private void StartGame(int stageNum)
    {
        audio.PlayOneShot(UISound);
        touchMgr.enabled = true;
        playerState.enabled = true;
        testMode.enabled = false;
        sc.CreateStage(stageNum);
        gameMgr.ChangeScreanImage();
        gameMgr.StartCoroutine(gameMgr.FadeInOut());
    }
}
