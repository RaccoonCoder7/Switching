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

    // 사용할 수 있는 스킬을 바꿈
    public void SetSkills(int skillSet)
    {
        for (int i = 0; i < 4 - skillSet; i++)
        {
            int index = SwapNum(3 - i);
            skillImages[index].enabled = false;
            buttonImages[index].sprite = lockSprite;
            buttonImages[index].GetComponent<Collider>().enabled = false;
        }
        for (int i = 0; i < skillSet; i++)
        {
            int index = SwapNum(i);
            skillImages[index].enabled = true;
            buttonImages[index].sprite = inactivateSprite;
            buttonImages[index].GetComponent<Collider>().enabled = true;
        }
        ChangeSprites(0);
    }

    // 스테이지넘버를 스킬셋으로 활용하기 위해 숫자를 바꿔줌
    private int SwapNum(int num)
    {
        switch(num){
            case 1:
                return 3;
            case 2:
                return 1;
            case 3:
                return 2;
            default:
                return 0;
        }
    }

    // 패널의 스킬셋의 이미지를 바꿈
    public void ChangeSprites(TouchMgr.SkillMode modeNum)
    {
        int index = (int)modeNum;
        buttonImages[prevBtn].sprite = inactivateSprite;
        buttonImages[index].sprite = panelSprites[index];
        prevBtn = index;
    }
}
