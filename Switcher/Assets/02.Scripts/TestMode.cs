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

    void Awake()
    {
        touchMgr = GetComponent<TouchMgr>();
        touchMgr.enabled = false;
        playerState = GetComponent<PlayerState>();
        playerState.enabled = false;
    }

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
        DontDestroyOnLoad(gameObject);
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
                StartCoroutine(StartGame(1));
            }
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("CONTINUE")))
            {
                StartCoroutine(StartGame(gameMgr.GetPrevStageNum()));
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
        audio.PlayOneShot(UISound);
        gameMgr.ChangeScreanImage();
        // sc.CreateStageAsync(stageNum); // 이걸 기달
        yield return StartCoroutine(sc.CreateStageAsync(stageNum));
        touchMgr.enabled = true;
        playerState.enabled = true;
        yield return StartCoroutine(gameMgr.FadeOut());
        testMode.enabled = false;
    }
}
