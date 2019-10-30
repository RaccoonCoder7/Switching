using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyDedlegate;

public class StageCtrl : MonoBehaviour
{
    public NowStage stage;
    private StageData sd;
    private GameMgr gameMgr;
    private AudioSource audio;
    private Transform playerTr;
    private Timer timer;
    private AsyncOperation async;
    private PlayerState ps;
    public TouchMgr touchMgr;
    public Chat chat;
    public GameObject playerOVR;
    // private Rigidbody playerRb;
    // private PlayerState ps;

    public ImageCtrl imgCtrl;
    public GameObject[] Maps;
    public AudioClip[] BGMClips;

    TouchFinger tf;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        stage = new NowStage();
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();
        audio = GetComponent<AudioSource>();
        playerTr = GameObject.Find("Player").transform;
        timer = imgCtrl.gameObject.GetComponent<Timer>();
        tf = FindObjectOfType<TouchFinger>();
        // ps = playerTr.GetComponent<PlayerState>();
        // touchMgr = playerTr.GetComponent<TouchMgr>();
    }

    public IEnumerator CreateStageAsync(int stageNum, bool isFirst)
    {
        stage.stageNum = stageNum;

        if (isFirst)
        {
            async = SceneManager.LoadSceneAsync("LoadStage");
            async.allowSceneActivation = true;
            while (!async.isDone)
            {
                yield return null;
            }
        }

        chat.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        CreateMap(isFirst);
        stage.skillSet = GetSkillSet();
        AudioClip clip = GetBGM();
        chat.TextSet("Stage" + stageNum);
        if (touchMgr)
        {
            touchMgr.mode = TouchMgr.SkillMode.chat;
        }

        if (!audio.clip.Equals(clip))
        {
            audio.Stop();
            audio.clip = clip;
        }

        StartStage();
    }

    public IEnumerator ResetStage(AudioClip clip)
    {
        //playerOVR.transform.rotation = Quaternion.Euler(0, 0, 0);
        gameMgr.ChangeScreanImage();
        yield return StartCoroutine(gameMgr.FadeIn());
        // 오브젝트위치
        CreateMap(false);
        // 플레이어위치
        playerTr.position = stage.playerTr.position;
        playerTr.rotation = stage.playerTr.rotation;
        // 스테이지시간
        timer.ResetTime(stage.stageTime);
        // 조력자와의대화
        if (!chat.gameObject.activeSelf)
        {
            chat.gameObject.SetActive(true);
        }
        tf.ActiveTrueBtn();
        chat.ResetText();

        yield return new WaitForSeconds(2.0f);
        if (!ps)
        {
            ps = playerTr.GetComponent<PlayerState>();
        }

        ps.isDead = false;

        if (clip)
        {
            audio.PlayOneShot(clip);
        }
        yield return StartCoroutine(gameMgr.FadeOut());
    }

    public IEnumerator ClearStage()
    {
        touchMgr.canFire = false;
        gameMgr.SaveClearData();
        gameMgr.ChangeScreanImage();
        yield return StartCoroutine(gameMgr.FadeIn());

        // 현재맵없애기
        yield return StartCoroutine(CreateStageAsync(gameMgr.GetPrevStageNum(), false));
        //chat.ResetText();
        yield return StartCoroutine(gameMgr.FadeOut());
    }

    public int GetStageNum()
    {
        return stage.stageNum;
    }

    private void StartStage()
    {
        //playerOVR.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (!audio.isPlaying)
        {
            audio.Play();
        }
        imgCtrl.SetSkills(stage.skillSet);
        timer.ResetTime(stage.stageTime);
        playerTr.position = stage.playerTr.position;
        playerTr.rotation = stage.playerTr.rotation;
        tf.ActiveFalseBtn();
    }


    private void CreateMap(bool isFirst)
    {
        if (!isFirst)
        {
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
