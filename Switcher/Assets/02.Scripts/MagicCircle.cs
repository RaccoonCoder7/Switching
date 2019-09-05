using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : MonoBehaviour
{
    // 대포에서 생성할 마나스톤
    private GameObject manastone;

    // 마나스톤대포
    public ManastoneFire manaCannon;
    // 마나스톤이 마법진에 몇개 들어왔는지 카운트
    private int inCount = 0;

    // 마나스톤 발사 멈춤
    private bool isStop = false;
    
    private void OnCollisionEnter(Collision collision)
    {
        // 같은 마법진에 닿았는지 확인
        if(collision.gameObject.tag == gameObject.tag)
        {
            inCount++;
            // 들어온 마나스톤이 하나일경우
            if (inCount == 1)
            {
                isStop = false;
                // 닿은 마나스톤을 저장
                manastone = collision.gameObject;
                StartCoroutine("FireLoop");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // 마나스톤이 마법진에서 나갈때
        if (collision.gameObject.tag == gameObject.tag)
        {
            inCount--;
            // 마나스톤 다 빠져나가면 발사 멈춤
            if (inCount == 0)
            {
                isStop = true;
            }
        }
    }

    // 마나스톤 발사 반복
    public IEnumerator FireLoop()
    {
        if (!isStop && manaCannon)
        {
            manaCannon.Fire();
            yield return new WaitForSeconds(8.0f);
            StartCoroutine("FireLoop");
        }
    }
}
