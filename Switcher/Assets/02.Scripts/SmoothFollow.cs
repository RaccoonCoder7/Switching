using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    float dist = -5;
    float height = 2;
    public Transform target;
    float speedRot = 5;

    // Start is called before the first frame update
    void Start()
    {
        //transform.position = new Vector3(0, height, dist);
        //transform.parent = GameObject.Find("Player").transform;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LateUpdate()
    {
        Vector3 pos = transform.position;
        Vector3 tarPos = target.position;
        pos = Vector3.Lerp(pos, tarPos,  Time.deltaTime);
        transform.position = pos;

        Quaternion rot = transform.rotation;       //Vector3로 구하고 싶다면 오일러 앵글 사용 에러날 수 있음ㅠ
        Quaternion tarRot = target.rotation;
        tarRot.z = 0;
        rot = Quaternion.Lerp(rot, tarRot, speedRot * Time.deltaTime);
        transform.rotation = rot;
    }
}
