﻿using System.Collections;
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
        // ps = playerTr.GetComponent<PlayerState>();
    }

    public IEnumerator CreateStageAsync(int stageNum)
    {
        stage.stageNum = stageNum;
        yield return StartCoroutine(gameMgr.FadeIn());
        // ShowLoadingScene();
        async = SceneManager.LoadSceneAsync("LoadStage");
        async.allowSceneActivation = true;
        while (!async.isDone)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2.0f);
        CreateMap();
        stage.skillSet = GetSkillSet();
        audio.clip = GetBGM();
        StartStage();
    }

    public void ResetStage()
    {
        // 오브젝트위치
        GameObject temp = stage.map;
        Destroy(stage.map);
        stage.map = Instantiate(temp);

        // 플레이어위치
        playerTr = stage.playerTr;

        // 스테이지시간
        timer.ResetTime(stage.stageTime);
    }

    private void StartStage()
    {
        audio.Play();
        imgCtrl.SetSkills(stage.skillSet);
        timer.ResetTime(stage.stageTime);
        playerTr.position = stage.playerTr.position;
        playerTr.rotation = stage.playerTr.rotation;
    }

    private void CreateMap()
    {
        stage.map = Instantiate(Maps[stage.stageNum-1]);
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
