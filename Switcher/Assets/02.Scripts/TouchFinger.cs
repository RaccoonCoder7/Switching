using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchFinger : MonoBehaviour
{
    private int skillButtonLayer;
    private TouchMgr touchMgr;
    private Collider sphere;
    private AudioSource audio;
    private Material mat;

    public MeshRenderer gun;
    public Material white;
    public Material green;
    public Material blue;
    public Material red;
    public Material cyan;
    public AudioClip changeClip;
    public ImageCtrl imageCtrl;

    void Start()
    {
        skillButtonLayer = LayerMask.NameToLayer("SKILLBUTTON");
        touchMgr = GameObject.Find("Player").GetComponent<TouchMgr>();
        sphere = GetComponent<Collider>();
        sphere.enabled = false;
        audio = GetComponent<AudioSource>();
        mat = green;
    }

    private void Update()
    {
        if (!OVRInput.Get(OVRInput.NearTouch.SecondaryIndexTrigger))
        {
            sphere.enabled = true;
        }
        else
        {
            sphere.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(skillButtonLayer))
        {
            if (touchMgr.mode == TouchMgr.SkillMode.chat)
            {
                return;
            }
            TouchMgr.SkillMode mode = TouchMgr.SkillMode.switching;
            string buttonName = other.gameObject.name;
            switch (buttonName)
            {
                case "switching":
                    mode = TouchMgr.SkillMode.switching;
                    mat = green;
                    break;
                case "pull":
                    mode = TouchMgr.SkillMode.pull;
                    mat = blue;
                    break;
                case "push":
                    mode = TouchMgr.SkillMode.push;
                    mat = red;
                    break;
                case "switchBomb":
                    mode = TouchMgr.SkillMode.switchBomb;
                    mat = cyan;
                    break;
            }
            if (!CheckModeIsChanged(mode)) return;
            audio.Play();
            StopCoroutine("ChangeMaterial");
            StartCoroutine("ChangeMaterial");
            touchMgr.ChangeMode(mode);
            imageCtrl.ChangeSprites(touchMgr.mode);
        }
    }

    private bool CheckModeIsChanged(TouchMgr.SkillMode nowMode)
    {
        if (touchMgr.mode.Equals(nowMode))
        {
            return false;
        }
        return true;
    }

    public IEnumerator ChangeMaterial()
    {
        gun.material = white;
        yield return new WaitForSeconds(0.3f);
        audio.PlayOneShot(changeClip);
        gun.material = mat;
    }
}
