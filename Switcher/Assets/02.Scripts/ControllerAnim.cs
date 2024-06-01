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

    // 컨트롤러 깜빡깜빡
    public IEnumerator AnimateContrlloer()
    {
        while (!rend)
        {
            yield return null;
        }
        while (true)
        {
            rend.material.mainTexture = highlightTexture;
            yield return new WaitForSeconds(0.5f);
            rend.material.mainTexture = defaultTexture;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
