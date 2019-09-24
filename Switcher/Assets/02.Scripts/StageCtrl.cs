using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCtrl : MonoBehaviour
{
    private int nowStage;
    private GameMgr gameMgr;
    private AudioSource audio;

    public AudioClip[] BGMClips;

    private void Start()
    {
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();
        audio = GetComponent<AudioSource>();
    }

    public void CreateStage(int stageNum)
    {
        nowStage = stageNum;
        ShowLoadingScene();
    }

    private void ShowLoadingScene()
    {
        StartCoroutine(gameMgr.FadeIn());
    }

    private AudioClip GetBGM()
    {
        return null;
    }

    private void CreateMap()
    {

    }

    private Vector3 GetSpawnPos()
    {
        return Vector3.zero;
    }
}
