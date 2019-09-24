using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCtrl : MonoBehaviour
{
    private NowStage stage;
    private StageData sd;
    private GameMgr gameMgr;
    private AudioSource audio;
    private Transform playerTr;

    public GameObject[] Maps;
    public AudioClip[] BGMClips;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        stage = new NowStage();
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();
        audio = GetComponent<AudioSource>();
        playerTr = GameObject.Find("Player").transform;
    }

    public void CreateStage(int stageNum)
    {
        stage.stageNum = stageNum;
        ShowLoadingScene();
        CreateMap();
        stage.skillSet = GetSkillSet();
        audio.clip = GetBGM();
    }

    private void StartStage()
    {
        audio.Play();
        // TODO: 스킬셋변경
        playerTr = stage.playerTr;
        StartCoroutine(gameMgr.FadeOut());
    }

    private void ShowLoadingScene()
    {
        StartCoroutine(gameMgr.FadeIn());
    }

    private void CreateMap()
    {
        stage.map = Instantiate(Maps[stage.stageNum]);
        sd = stage.map.GetComponent<StageData>();
        stage.playerTr = sd.playerTr;
    }

    private AudioClip GetBGM()
    {
        if (sd.customBGM)
        {
            return sd.customBGM;
        }
        if (stage.stageNum <= 3)
        {
            return BGMClips[0];
        }
        return BGMClips[1];
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
