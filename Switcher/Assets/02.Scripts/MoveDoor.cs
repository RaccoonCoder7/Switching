using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDoor : MonoBehaviour
{
    private Hashtable ht;

    public float moveDistance = 4.15f;

    // 해당 오브젝트의 초기 position z 값
    private float z;

    // 문의 이동이 다 되었는지 체크
    private bool check;

    void Start()
    {
        ht = new Hashtable();
        z = gameObject.transform.localPosition.x;
    }

    // 문을 여는 메소드
    public void MoveOpen()
    {
        ht = new Hashtable();
        if (check)
        {
            ht.Add("z", moveDistance-Mathf.Abs(z - transform.localPosition.x));
        }
        else
        {
            ht.Add("z", moveDistance);
        }
        ht.Add("time", 0.3f);
        ht.Add("easetype", iTween.EaseType.linear);
        //htOpen.Add("oncomplete", "CheckTriggerUp");
        iTween.MoveBy(gameObject, ht);
    }

    // 문을 닫는 메소드
    public void MoveClose()
    {
        ht = new Hashtable();
        check = true;
        ht.Add("z", -Mathf.Abs(z -transform.localPosition.x));
        ht.Add("time", 0.3f);
        ht.Add("easetype", iTween.EaseType.linear);
        ht.Add("oncomplete", "CheckTriggerUp");
        iTween.MoveBy(gameObject, ht);
    }

    // 문이 움직이는걸 마쳤는지 확인
    void CheckTriggerUp()
    {
        check = false;
    }
}
