using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManastoneFire : MonoBehaviour
{
    private Rigidbody rig;

    void Start()
    {
        rig = gameObject.GetComponent<Rigidbody>();
        gameObject.SetActive(false);
    }

    // 마나스톤 활성화 및 발사
    public void Fire()
    {
        gameObject.SetActive(true);
        rig.AddRelativeForce(Vector3.up * 700.0f);
        StartCoroutine("ManastoneDestoryDelay");
    }

    // 마나스톤 파괴 딜레이
    public IEnumerator ManastoneDestoryDelay()
    {
        yield return new WaitForSeconds(7.0f);
        gameObject.SetActive(false);
        rig.velocity = Vector3.zero;
        transform.position = gameObject.transform.parent.position;
    }
}
