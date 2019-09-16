using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PolygonFireProjectile : MonoBehaviour
{
    RaycastHit hit;
    public GameObject projectiles;
    public Transform spawnPosition;
    public float speed = 10;

    private float animDelayTime = 1.0f;

    private Transform camTr;
    GameObject projectile;
    private Vector3 playerPos;

    void Start()
    {
        camTr = Camera.main.GetComponent<Transform>();
        projectile = Instantiate(projectiles, spawnPosition.position, Quaternion.identity);
        Destroy(projectile);
    }

    public IEnumerator SlowFire()
    {
        yield return new WaitForSeconds(animDelayTime);
        playerPos = new Vector3(camTr.position.x, camTr.position.y - 0.4f, camTr.position.z);
        projectile = Instantiate(projectiles, spawnPosition.position, Quaternion.identity);
        projectile.transform.LookAt(playerPos);
        projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * speed);
    }
}