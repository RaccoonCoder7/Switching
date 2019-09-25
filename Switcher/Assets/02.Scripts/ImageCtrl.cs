using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCtrl : MonoBehaviour
{
    private Image[] skillImages = new Image[4];
    private int prevBtn = 0;

    public Image[] buttonImages;
    public Sprite[] panelSprites;
    public Sprite inactivateSprite;
    public Sprite lockSprite;

    void Start()
    {
        for (int i = 0; i < buttonImages.Length; i++)
        {
            string buttonName = buttonImages[i].gameObject.name;
            skillImages[i] = buttonImages[i].transform.Find(buttonName + "Image").GetComponent<Image>();
        }
        ChangeSprites(0);
    }

    public void SetSkills(int skillSet)
    {
        for(int i = skillSet; i > 0; i--){
            skillImages[i].enabled = false;
            buttonImages[i].sprite = lockSprite;
        }
    }

    public void ChangeSprites(TouchMgr.SkillMode modeNum)
    {
        int index = (int)modeNum;
        buttonImages[prevBtn].sprite = inactivateSprite;
        buttonImages[index].sprite = panelSprites[index];
        prevBtn = index;
    }
}
