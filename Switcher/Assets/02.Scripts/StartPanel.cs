using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : MonoBehaviour
{

    Color color;
    public GameObject startText;
    public Material fadeMaterial;
    public GameObject screen;
    public GameObject logo;
    bool start;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger) && !start)
        {
            start = true;
            StartCoroutine("StartFadeOut");
        }
    }

    public IEnumerator StartFadeOut()
    {
        Color color = fadeMaterial.color;
        while (color.a > 0f)
        {
            color.a -= 0.03f;
            fadeMaterial.color = color;
            yield return new WaitForSeconds(0.02f);
        }
        screen.SetActive(false);
        logo.SetActive(false);
        startText.SetActive(false);
    }
}
