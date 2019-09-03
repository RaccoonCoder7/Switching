using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour
{
    public Vector3 axisMagnitude = new Vector3(0f, 1f, 0f);
    public float speed = 10f;
    public Transform spinningObject;
    //
    public Space space;
    //
    public Vector3 randomiseMagnitude = new Vector3(0f, 0f, 0f);
    public float randomiseSpeed = 0f;

    // Use this for initialization
    void Start()
    {
        if (spinningObject == null)
        {
            spinningObject = gameObject.transform;
        }
        //
        axisMagnitude += new Vector3(Random.Range(-randomiseMagnitude.x, randomiseMagnitude.x),
            Random.Range(-randomiseMagnitude.y, randomiseMagnitude.y),
            Random.Range(-randomiseMagnitude.z, randomiseMagnitude.z));
        //
        speed += Random.Range(-randomiseSpeed, randomiseSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        spinningObject.Rotate(axisMagnitude * speed * Time.deltaTime, space);
    }
}
