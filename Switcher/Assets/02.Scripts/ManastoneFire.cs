using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManastoneFire : MonoBehaviour
{
    private Rigidbody rig;
    
    // 연결되있는 마법진
    public MagicCircle mc;

    // 마나스톤 유지, 삭제 시간
    public float liveTime = 7.0f;
    public float DeathTime = 8.0f;

    // 사용할 대포 마나스톤
    public GameObject cannonMCFix;
    private GameObject cannonMC;
    
    void Start()
    {
        cannonMC = Instantiate(cannonMCFix, gameObject.transform);
        Destroy(cannonMC);
    }

    private void Update()
    {
        if (mc.manastone && mc.collisionEnterFl)
        {
            StartCoroutine("FireLoop");
        }
    }

    // 마나스톤 활성화 및 발사
    public void Fire()
    {
        if (!cannonMC)
        {
            cannonMC = Instantiate(cannonMCFix, gameObject.transform);
            rig = cannonMC.GetComponent<Rigidbody>();
            rig.AddRelativeForce(Vector3.up * 700.0f);
            StartCoroutine("ManastoneDestoryDelay");
        }
    }

    // 마나스톤 파괴 딜레이
    public IEnumerator ManastoneDestoryDelay()
    {
        yield return new WaitForSeconds(liveTime);
        if (cannonMC)
        {
            Destroy(cannonMC);
            cannonMC = null;
        }
    }

    // 마나스톤 발사 반복
    public IEnumerator FireLoop()
    {
        if (mc.manastone)
        {
            Fire();
            yield return new WaitForSeconds(DeathTime);
            StartCoroutine("FireLoop");
        }
    }
}
