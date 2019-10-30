using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : MonoBehaviour
{
    // 대포에서 생성할 마나스톤
    public GameObject manastone;

    // 마나스톤이 마법진에 몇개 들어왔는지 카운트
    public int inCount = 0;

    private AudioSource audio;
    public AudioClip onClip;
    public AudioClip offClip;

    public ParticleSystem particle;
    public SpriteRenderer[] changeNaviStraight;
    public SpriteRenderer[] changeNaviConer;
    public Sprite greenNaviStraight;
    public Sprite greenNaviConer;
    public Sprite redNaviStraight;
    public Sprite redNaviConer;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 같은 마법진에 닿았는지 확인
        if (collision.gameObject.tag == gameObject.tag)
        {
            inCount++;
            // 들어온 마나스톤이 하나일경우
            if (inCount == 1)
            {
                // 닿은 마나스톤을 저장
                manastone = collision.gameObject;
                audio.clip = onClip;
                audio.Play();
                particle.Play();
                for (int i = 0; i < changeNaviStraight.Length; i++)
                {
                    changeNaviStraight[i].sprite = greenNaviStraight;
                }
                for (int i = 0; i < changeNaviConer.Length; i++)
                {
                    changeNaviConer[i].sprite = greenNaviConer;
                }
            }
        }
    }

    private void Update()
    {
        if (inCount == 0)
        {
            manastone = null;
            EffectOff();
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
                manastone = null;
                EffectOff();
            }
        }
    }

    private void EffectOff()
    {
        audio.clip = offClip;
        audio.Play();
        particle.Stop();
        for (int i = 0; i < changeNaviStraight.Length; i++)
        {
            changeNaviStraight[i].sprite = redNaviStraight;
        }
        for (int i = 0; i < changeNaviConer.Length; i++)
        {
            changeNaviConer[i].sprite = redNaviConer;
        }
    }
}
