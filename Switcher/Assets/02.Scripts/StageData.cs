using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData : MonoBehaviour
{
    [Tooltip("필수: 플레이어가 생성될 위치. y값은 바닥높이와 같게 할 것.")]
    public Transform playerTr;

    [Tooltip("선택: 커스텀 BGM을 사용하고싶을경우 사용.")]
    public AudioClip customBGM;

    [Tooltip("선택: 커스텀 BGM을 사용하고싶을경우 사용.")]
    public int customSkillSet = 0;
}
