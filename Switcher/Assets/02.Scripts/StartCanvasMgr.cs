using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCanvasMgr : MonoBehaviour
{
    public GameObject startBtn;
    public GameObject continueBtn;
    public GameObject onOffText;
    public GameObject tpStylePanel;
    public GameObject selectStagePanel;
    public Color selectedColor;
    public TranslateBullet tb;
    public BombArea ba;

    private Image tpBtn;
    private Image lerpBtn;
    private GameObject tpHuman;
    private GameObject tpDiamond;
    private GameObject lerpHuman;
    private GameObject lerpDiamond;
    private Color white = Color.white;
    private bool isStarted;
    private bool isReversed;
    private Animator lerpAnim;
    private TestMode tm;

    void Start()
    {
        QualitySettings.SetQualityLevel(2, true);
        tm = FindObjectOfType<TestMode>();
        tpBtn = tpStylePanel.transform.Find("Teleport").GetComponent<Image>();
        lerpBtn = tpStylePanel.transform.Find("Lerp").GetComponent<Image>();
        tpHuman = tpBtn.transform.Find("human").gameObject;
        tpDiamond = tpBtn.transform.Find("diamond").gameObject;
        lerpHuman = lerpBtn.transform.Find("human").gameObject;
        lerpDiamond = lerpBtn.transform.Find("diamond").gameObject;
        lerpAnim = lerpBtn.GetComponent<Animator>();
        startBtn.SetActive(false);
        continueBtn.SetActive(false);
        tpStylePanel.SetActive(false);
        selectStagePanel.SetActive(false);
        // PlayerPrefs.SetInt("isCleared", 1); // TODO: 테스트코드
        StartCoroutine("BlinkText");
    }

    void Update()
    {
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger) && !isStarted)
        {
            isStarted = true;
            StopCoroutine("BlinkText");
            onOffText.SetActive(false);
            StartCoroutine("WaitAndShow");
        }
    }

    private IEnumerator WaitAndShow()
    {
        yield return new WaitForSeconds(1f);
        ShowPanels();
        OnClickTP();
    }

    private IEnumerator BlinkText()
    {
        while (!isStarted)
        {
            onOffText.SetActive(!onOffText.activeSelf);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void ShowPanels()
    {
        startBtn.SetActive(true);
        if (!GameMgr.stage.Equals(0))
        {
            continueBtn.SetActive(true);
        }
        tpStylePanel.SetActive(true);
        if (PlayerPrefs.GetInt("isCleared").Equals(1))
        {
            selectStagePanel.SetActive(true);
        }
    }

    public void OnClickTP()
    {
        tpBtn.color = selectedColor;
        lerpBtn.color = white;
        lerpAnim.SetBool("isLerp", false);
        StartCoroutine("TeleportAnim");
        tb.tpStyle = TranslateBullet.teleportStyle.teleport;
        ba.tpStyle = BombArea.teleportStyle.teleport;
    }

    public void OnClickLerp()
    {
        lerpBtn.color = selectedColor;
        tpBtn.color = white;
        StopCoroutine("TeleportAnim");
        if (isReversed)
        {
            ChangeTpPos();
        }
        lerpAnim.SetBool("isLerp", true);
        tb.tpStyle = TranslateBullet.teleportStyle.lerp;
        ba.tpStyle = BombArea.teleportStyle.lerp;
    }

    public void OnClickStage(int stage)
    {
        PlayerPrefs.SetInt("Stage", stage);
        tm.StartCoroutine(tm.StartGame(stage));
    }

    private IEnumerator TeleportAnim()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            ChangeTpPos();
        }
    }

    private void ChangeTpPos()
    {
        isReversed = !isReversed;
        Vector3 temp = tpHuman.transform.position;
        tpHuman.transform.position = tpDiamond.transform.position;
        tpDiamond.transform.position = temp;
    }
}
