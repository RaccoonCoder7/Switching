using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NowStage
{
    public int stageNum; // 스테이지 번호
    public GameObject map; // 맵 프리팹
    public Transform playerTr; // 플레이어가 생성될 위치
    public AudioClip BGM; // 맵에 사용되는 BGM
    public int skillSet; // 어떤 스킬을 사용할 수 있는지 정함
    public int stageTime; // 스테이지 제한시간
}
