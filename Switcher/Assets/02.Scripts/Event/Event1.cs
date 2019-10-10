using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDedlegate; // 필수

public class Event1 : EventMgr // EventMgr을 상속받을것.
{
    private GameObject barrier;
    private TouchMgr touchMgr;
    private Timer timer;

    public GameObject arrow;
    public GameObject rightController;
    public Transform frontDoor;
    public Transform blueManastone;
    public Transform redManastone;
    public Transform connectLine;

    void Start()
    {
        base.Start(); // Start를 사용할때엔 필수로 기입할 것.
        arrow = Instantiate(arrow, transform);
        arrow.SetActive(false);
        rightController.SetActive(false);
        GameObject player = GameObject.Find("Player");
        barrier = player.transform.Find("BeamupCylinderGreen").gameObject;
        touchMgr = player.GetComponent<TouchMgr>();
        timer = FindObjectOfType<Timer>();

        // Text에서 false가 나올 때 마다 EventList에 담긴 이벤트를 실행함.
        EventList[0] = new Deleg(EV1);
        EventList[1] = new Deleg(EV2);
        EventList[2] = new Deleg(EV3);
        EventList[3] = new Deleg(EV4);
        EventList[4] = new Deleg(EV5);
        EventList[5] = new Deleg(EV6);
    }

    private void EV1()
    {
        StartCoroutine("FadeInOutBarrier");
    }

    private void EV2()
    {
        arrow.SetActive(true);
        Vector3 pos = blueManastone.transform.position + new Vector3(0, 3, 0);
        arrow.transform.position = pos;
        arrow.transform.rotation = Quaternion.Euler(90, 0, 0);

        touchMgr.ChangeMode(TouchMgr.SkillMode.switching);

        Event1a event1a = blueManastone.gameObject.GetComponent<Event1a>();
        event1a.isReady = true;
        event1a.arrow = arrow;
        event1a.rightController = rightController;
        rightController.SetActive(true);
        Debug.Log("1");
        ControllerAnim ca = rightController.GetComponent<ControllerAnim>();
        Debug.Log("2");
        StartCoroutine(ca.AnimateContrlloer());
        Debug.Log("3");
    }

    private void EV3()
    {
        arrow.SetActive(true);
        arrow.transform.position = frontDoor.transform.position + new Vector3(0, 3, 0);
        arrow.transform.rotation = Quaternion.Euler(0, -45, -90);
        CallChat();
    }

    private void EV4()
    {
        arrow.transform.position = redManastone.transform.position + new Vector3(0, 3, 0);
        arrow.transform.rotation = Quaternion.Euler(90, 0, 90);
        CallChat();
    }

    private void EV5()
    {
        arrow.transform.position = connectLine.transform.position + new Vector3(0, 3, 0);
        CallChat();
    }

    private void EV6()
    {
        touchMgr.ChangeMode(TouchMgr.SkillMode.switching);
        arrow.SetActive(false);

        // 타이머시작
        timer.StartTime();
    }

    private IEnumerator FadeInOutBarrier()
    {
        barrier.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        barrier.SetActive(false);
        CallChat();
    }
}
