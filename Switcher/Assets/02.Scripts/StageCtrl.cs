using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageCtrl : MonoBehaviour
{
    private NowStage stage;
    private StageData sd;
    private GameMgr gameMgr;
    private AudioSource audio;
    private Transform playerTr;
    private Timer timer;
    private AsyncOperation async;
    private PlayerState ps;
    // private Rigidbody playerRb;
    // private PlayerState ps;

    public ImageCtrl imgCtrl;
    public GameObject[] Maps;
    public AudioClip[] BGMClips;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        stage = new NowStage();
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();
        audio = GetComponent<AudioSource>();
        playerTr = GameObject.Find("Player").transform;
        timer = imgCtrl.gameObject.GetComponent<Timer>();
        ps = playerTr.GetComponent<PlayerState>();
        // playerRb = playerTr.GetComponent<Rigidbody>();
        // ps = playerTr.GetComponent<PlayerState>();
    }

    public IEnumerator CreateStageAsync(int stageNum, bool isFirst)
    {
        stage.stageNum = stageNum;
        Debug.Log("stageNum: " + stageNum);
        yield return StartCoroutine(gameMgr.FadeIn());
        Debug.Log("1");
        // ShowLoadingScene();
        if (isFirst)
        {
            async = SceneManager.LoadSceneAsync("LoadStage");
            async.allowSceneActivation = true;
            while (!async.isDone)
            {
                yield return null;
            }
        }
        yield return new WaitForSeconds(2.0f);
        Debug.Log("2");
        CreateMap(isFirst);
        stage.skillSet = GetSkillSet();
        AudioClip clip = GetBGM();
        if (!audio.clip.Equals(clip))
        {
            audio.Stop();
            audio.clip = clip;
        }
        Debug.Log("3");
        StartStage();
        Debug.Log("4");
    }

    public IEnumerator ResetStage(AudioClip clip)
    {
        // playerRb.isKinematic = true;
        gameMgr.ChangeScreanImage();
        yield return StartCoroutine(gameMgr.FadeIn());
        // 오브젝트위치
        CreateMap(false);

        // 플레이어위치
        playerTr.position = stage.playerTr.position;
        playerTr.rotation = stage.playerTr.rotation;

        // 스테이지시간
        timer.ResetTime(stage.stageTime);
        yield return new WaitForSeconds(2.0f);
        ps.isDead = false;
        // playerRb.isKinematic = false;
        if (clip)
        {
            audio.PlayOneShot(clip);
        }
        yield return StartCoroutine(gameMgr.FadeOut());
    }

    public IEnumerator ClearStage()
    {
        gameMgr.SaveClearData();
        gameMgr.ChangeScreanImage();
        // yield return StartCoroutine(gameMgr.FadeIn());
        // 현재맵없애기
        yield return StartCoroutine(CreateStageAsync(gameMgr.GetPrevStageNum(), false));
        yield return StartCoroutine(gameMgr.FadeOut());
    }

    private void StartStage()
    {
        if (!audio.isPlaying)
        {
            audio.Play();
        }
        imgCtrl.SetSkills(stage.skillSet);
        timer.ResetTime(stage.stageTime);
        playerTr.position = stage.playerTr.position;
        playerTr.rotation = stage.playerTr.rotation;
    }

    private void CreateMap(bool isFirst)
    {
        if (!isFirst)
        {
            Debug.Log("destroyMap");
            Destroy(stage.map);
        }
        stage.map = Instantiate(Maps[stage.stageNum - 1]);
        sd = stage.map.GetComponent<StageData>();
        stage.playerTr = sd.playerTr;
        stage.stageTime = sd.stageTime;
    }

    private AudioClip GetBGM()
    {
        if (sd.customBGM)
        {
            return sd.customBGM;
        }
        if (stage.stageNum <= 3)
        {
            return BGMClips[1];
        }
        return BGMClips[2];
    }

    private int GetSkillSet()
    {
        if (!sd.customSkillSet.Equals(0))
        {
            return sd.customSkillSet;
        }
        if (stage.stageNum < 4)
        {
            return stage.stageNum;
        }
        return 4;
    }
}
