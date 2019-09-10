using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private bool canDmg = true;
    public IntersectionFx barrier;

    void Start()
    {
        barrier.gameObject.SetActive(false);
    }


    public void disableDmg(float waitTime)
    {
        canDmg = false;
        barrier.gameObject.SetActive(true);
        StartCoroutine(showBarrier(waitTime));
        // Invoke("change2EnableDmg", waitTime);
    }

    private IEnumerator showBarrier(float waitTime)
    {
        barrier.m_MainColor.a = 0.5f;
        barrier.m_IntersectionColor.a = 0.5f;
        float alpha = barrier.m_MainColor.a / 25;
        float frame = 0.05f;
        for (float time = 0; time < 1; time += frame)
        {
            barrier.m_MainColor.a -= alpha;
            barrier.m_IntersectionColor.a -= alpha;
            Quaternion rotation = barrier.gameObject.transform.rotation;
            rotation.y += 3 * Time.deltaTime;
            barrier.gameObject.transform.rotation = rotation;
            yield return new WaitForSeconds(frame);
        }
        // yield return new WaitForSeconds(waitTime);

        barrier.gameObject.SetActive(false);
        canDmg = true;
    }
}
