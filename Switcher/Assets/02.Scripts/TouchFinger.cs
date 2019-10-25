using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyDedlegate;

public class TouchFinger : MonoBehaviour
{
    private int skillButtonLayer;
    private TouchMgr touchMgr;
    private StageCtrl sc;
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
    float time;

    public Image btn1;
    public Image btn2;

    public Image[] upImages;
    public Image[] buttonImages;
    public Sprite inactivateSprite;
    public Sprite lockSprite;

    public GameObject chatCanvas;

    Chat chat;

    public bool stageCheck;
    public bool fullBtn;

    void Start()
    {
        skillButtonLayer = LayerMask.NameToLayer("SKILLBUTTON");
        touchMgr = GameObject.Find("Player").GetComponent<TouchMgr>();
        sphere = GetComponent<Collider>();
        sphere.enabled = false;
        audio = GetComponent<AudioSource>();
        mat = green;
        chat = chatCanvas.GetComponent<Chat>();

        sc = FindObjectOfType<StageCtrl>();
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
                default:
                    return;
            }
            if (!CheckModeIsChanged(mode)) return;
            audio.Play();
            StopCoroutine("ChangeMaterial");
            StartCoroutine("ChangeMaterial");
            touchMgr.ChangeMode(mode);
            imageCtrl.ChangeSprites(touchMgr.mode);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (touchMgr.mode == TouchMgr.SkillMode.chat)
        {
            return;
        }
        switch (other.gameObject.name)
        {
            case "Retry":
                if (btn1.fillAmount < 1)
                {
                    btn2.fillAmount = 0;
                    btn1.fillAmount += 0.5f * Time.deltaTime;
                }
                if (btn1.fillAmount >= 1)
                {
                    if (!stageCheck)
                    {
                        StartCoroutine(sc.ResetStage(null));
                    }
                    else
                    {
                        ActiveFalseBtn();
                        fullBtn = true;
                    }
                    
                }
                break;
            case "ShowText":
                if (btn2.fillAmount < 1)
                {
                    btn1.fillAmount = 0;
                    btn2.fillAmount += 0.5f * Time.deltaTime;
                }
                if (btn2.fillAmount >= 1)
                {
                    if (!chat.helpCheck && !stageCheck)
                    {
                        btn2.fillAmount = 0;
                        chatCanvas.SetActive(true);
                        chat.CallHelper();
                    }
                    else
                    {
                        ActiveFalseBtn();
                        fullBtn = true;
                    }
                }
                
                break;
        }

    }
        private void OnTriggerExit(Collider other)
    {
        if (touchMgr.mode == TouchMgr.SkillMode.chat)
        {
            return;
        }
        switch (other.gameObject.name)
        {
            case "Retry":
                btn1.fillAmount = 0;
                break;
            case "ShowText":
                btn2.fillAmount = 0;
                break;
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

    public void ActiveTrueBtn()
    {
        for (int i = 0; i < 2; i++)
        {
            btn1.fillAmount = 0;
            btn2.fillAmount = 0;
            upImages[i].enabled = true;
            buttonImages[i].sprite = inactivateSprite;
            buttonImages[i].GetComponent<Collider>().enabled = true;
        }
    }

    public void ActiveFalseBtn()
    {
        for (int i = 0; i < 2; i++)
        {
            upImages[i].enabled = false;
            buttonImages[i].sprite = lockSprite;
            btn1.fillAmount = 0;
            btn2.fillAmount = 0;
            buttonImages[i].GetComponent<Collider>().enabled = false;
        }
    }
    //public IEnumerator PressedBtn(Image img)
    //{
    //    while (img.fillAmount < 1f)
    //    {
    //        img.fillAmount += 0.025f;
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //    //StartCoroutine(sc.ResetStage(null));
    //}

    //public IEnumerator OutBtn(Image img)
    //{
    //    while (img.fillAmount > 0f)
    //    {
    //        img.fillAmount -= 0.025f;
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //    //StartCoroutine(sc.ResetStage(null));
    //}

}
