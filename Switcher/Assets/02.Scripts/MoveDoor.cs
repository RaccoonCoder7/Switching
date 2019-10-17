using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDoor : MonoBehaviour
{
    private Hashtable ht;
    public float moveDistance = 4.15f;
    private AudioSource audio;

    // 해당 오브젝트의 초기 position z 값
    private float z;

    // 오브젝트 움직일 방향
    public string xyz;

    // 문의 이동이 다 되었는지 체크
    private bool check;

    public MagicCircle[] mc;

    private bool checkManstone = false;

    public MeshRenderer meshRenderer;
    public Material[] openTexture;
    public Material[] closeTexture;
    public GameObject[] particle;

    void Start()
    {
        ht = new Hashtable();
        
        if (xyz.Equals("y"))
        {
            z = gameObject.transform.localPosition.y;
        }
        else if (xyz.Equals("x") || xyz.Equals("z"))
        {
            z = gameObject.transform.localPosition.x;
        }
        audio = GetComponent<AudioSource>();
        if (mc.Length != 0)
        {
            StartCoroutine("MoveOpenCheck");
        }
    }

    // 문이 열릴 조건을 갖췄는지 체크
    IEnumerator MoveOpenCheck()
    {
        while (!checkManstone)
        {
            for (int i = 0; i < mc.Length; i++)
            {
                if (i == 0) checkManstone = true;

                checkManstone = checkManstone && mc[i].manastone;
            }
            yield return null;
        }

        MoveOpen();
        StartCoroutine("MoveCloseCheck");
    }

    // 문이 닫힐 조건을 갖췄는지 체크
    IEnumerator MoveCloseCheck()
    {
        while (checkManstone)
        {
            for (int i = 0; i < mc.Length; i++)
            {
                if (i == 0) checkManstone = true;

                checkManstone = checkManstone && mc[i].manastone;
            }
            yield return null;
        }

        MoveClose();
        StartCoroutine("MoveOpenCheck");
    }

    // 문을 여는 메소드
    public void MoveOpen()
    {
        ht = new Hashtable();
        if (check)
        {
            if (xyz.Equals("y"))
            {
                ht.Add(xyz, moveDistance - Mathf.Abs(z - transform.localPosition.y));
            }
            else if (xyz.Equals("x") || xyz.Equals("z"))
            {
                ht.Add(xyz, moveDistance - Mathf.Abs(z - transform.localPosition.x));
            }
        }
        else
        {
            ht.Add(xyz, moveDistance);
        }
        ht.Add("time", 0.3f);
        ht.Add("easetype", iTween.EaseType.linear);
        //htOpen.Add("oncomplete", "CheckTriggerUp");
        audio.Play();
        if (meshRenderer)
        {
            meshRenderer.materials = openTexture;
        }
        if (particle.Length > 0)
        {
            for (int i =0; i< particle.Length; i++)
            {
                particle[i].SetActive(!particle[i].activeSelf);
            }

        }
        iTween.MoveBy(gameObject, ht);
    }

    // 문을 닫는 메소드
    public void MoveClose()
    {
        ht = new Hashtable();
        check = true;
        if (xyz.Equals("y"))
        {
            ht.Add(xyz, -Mathf.Abs(z - transform.localPosition.y));
        }
        else if (xyz.Equals("x") || xyz.Equals("z"))
        {
            ht.Add(xyz, -Mathf.Abs(z - transform.localPosition.x));
        }
        ht.Add("time", 0.3f);
        ht.Add("easetype", iTween.EaseType.linear);
        ht.Add("oncomplete", "CheckTriggerUp");
        audio.Play();
        if (meshRenderer)
        {
            meshRenderer.materials = closeTexture;
        }
        if (particle.Length > 0)
        {
            for (int i = 0; i < particle.Length; i++)
            {
                particle[i].SetActive(!particle[i].activeSelf);
            }
        }
        iTween.MoveBy(gameObject, ht);
    }

    // 문이 움직이는걸 마쳤는지 확인
    void CheckTriggerUp()
    {
        check = false;
    }
}
