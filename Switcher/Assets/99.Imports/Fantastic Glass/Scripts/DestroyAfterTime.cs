using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour
{
    public float timeToWait;
    bool timing;
    public bool debug = false;

    public void Delayed_Destroy()
    {
        Debug.Log("DestroyAfterTime: destroyed object '" + gameObject.ToString() + "'");
        Destroy(gameObject);
    }

    void Awake()
    {
        if (!timing)
        {
            timing = true;
            if (timeToWait == 0f)
                Delayed_Destroy();
            else
                Invoke("Delayed_Destroy", timeToWait);
        }
    }
}
