using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerAnim : MonoBehaviour
{
    private MeshRenderer rend;

    public Texture defaultTexture;
    public Texture highlightTexture;

    void Start()
    {
        rend = GetComponent<MeshRenderer>();
    }

    public IEnumerator AnimateContrlloer()
    {
        while (!rend)
        {
            yield return null;
        }
        while (true)
        {
            Debug.Log("5");
            rend.material.mainTexture = highlightTexture;
            yield return new WaitForSeconds(0.5f);
            Debug.Log("6");
            rend.material.mainTexture = defaultTexture;
            yield return new WaitForSeconds(0.5f);
            Debug.Log("7");
        }
    }
}
