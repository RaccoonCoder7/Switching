using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManastone : MonoBehaviour
{
    private MagicCircle mc;
    private int magicCircleLayer;

    void Start()
    {
        magicCircleLayer = LayerMask.NameToLayer("MAGICCIRCLE");
    }

    public void BeforeDestroy()
    {
        if (mc)
        {
            mc.inCount--;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer.Equals(magicCircleLayer))
        {
            mc = other.gameObject.GetComponent<MagicCircle>();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer.Equals(magicCircleLayer) && mc)
        {
            mc = null;
        }
    }
}
