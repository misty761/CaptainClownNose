using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour
{
    public static bool isTouching;
    public static bool bTouchUp;    // 터치 업 후에 터치해야 점프 가능하도록하기 위해 사용

    // Start is called before the first frame update
    void Start()
    {
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry_TouchDown = new EventTrigger.Entry();
        entry_TouchDown.eventID = EventTriggerType.PointerDown;
        entry_TouchDown.callback.AddListener((data) => { TouchDown(); });
        eventTrigger.triggers.Add(entry_TouchDown);

        EventTrigger.Entry entry_TouchUp = new EventTrigger.Entry();
        entry_TouchUp.eventID = EventTriggerType.PointerUp;
        entry_TouchUp.callback.AddListener((data) => { TouchUp(); });
        eventTrigger.triggers.Add(entry_TouchUp);

        isTouching = false;
        bTouchUp = true;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.S)) bTouchUp = true;
    }

    private void TouchDown()
    {
        isTouching = true;
    }

    private void TouchUp()
    {
        isTouching = false;
        bTouchUp = true;
    }
}
