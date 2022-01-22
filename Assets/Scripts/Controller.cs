using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.CodeDom;
using System;

public class Controller : MonoBehaviour
{
    public static Controller instance;

    public Image stick;

    public static bool moveFlag;
    public static bool playerRight;
    public static bool isJoysticUp;
    public static bool isJoysticDown;

    Vector3 orignPos = Vector3.zero;
    Vector3 joyVec;

    PlayerController player;

    float PLAYER_MAX_SPEED = 1f;
    float speed;
    float stickRadius = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("두개의 콘트롤러가 존재합니다!");
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        orignPos = stick.transform.position;
        stickRadius = stick.rectTransform.sizeDelta.x * 1.5f;

        speed = PLAYER_MAX_SPEED;

        // 캔버스 크기에 대한 반지름 조절
        float can = transform.parent.GetComponent<RectTransform>().localScale.x;
        stickRadius *= can;

        moveFlag = false;
        playerRight = true;
        isJoysticUp = false;
        isJoysticDown = false;
        
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry_Drag = new EventTrigger.Entry();
        entry_Drag.eventID = EventTriggerType.Drag;
        entry_Drag.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        eventTrigger.triggers.Add(entry_Drag);

        EventTrigger.Entry entry_EndDrag = new EventTrigger.Entry();
        entry_EndDrag.eventID = EventTriggerType.EndDrag;
        entry_EndDrag.callback.AddListener((data) => { OnEndDrag(); });
        eventTrigger.triggers.Add(entry_EndDrag);

        EventTrigger.Entry entry_EndTouch = new EventTrigger.Entry();
        entry_EndTouch.eventID = EventTriggerType.PointerUp;
        entry_EndTouch.callback.AddListener((data) => { TouchUp((PointerEventData)data); });
        eventTrigger.triggers.Add(entry_EndTouch);

        EventTrigger.Entry entry_BeginTouch = new EventTrigger.Entry();
        entry_BeginTouch.eventID = EventTriggerType.PointerDown;
        entry_BeginTouch.callback.AddListener((data) => { TouchDown((PointerEventData)data); });
        eventTrigger.triggers.Add(entry_BeginTouch);

    }

    

    void Update()
    {
        if (GameManager.instance.isGameover)
        {
            stick.rectTransform.position = orignPos;
            moveFlag = false;
            return;
        }

        //if (moveFlag) player.transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (moveFlag)
        {
            if (playerRight) player.transform.Translate(Vector3.right * Time.deltaTime * speed);
            else player.transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        // 키보드로 캐릭터 조정
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveFlag = true;
            playerRight = false;
            player.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow)) moveFlag = false;
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveFlag = true;
            playerRight = true;
            player.transform.localScale = new Vector3(1, 1, 1);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow)) moveFlag = false;

        if (Input.GetKeyDown(KeyCode.UpArrow)) isJoysticUp = true;
        else if (Input.GetKeyUp(KeyCode.UpArrow)) isJoysticUp = false;

        if (Input.GetKeyDown(KeyCode.DownArrow)) isJoysticDown = true;
        else if (Input.GetKeyUp(KeyCode.DownArrow)) isJoysticDown = false;
    }

    public void OnDrag(PointerEventData touch)
    {
        ControlPlayer(touch);
    }

    private void TouchDown(PointerEventData touch)
    {
        ControlPlayer(touch);
    }

    public void OnEndDrag()
    {
        EndTouch();
    }

    private void TouchUp(PointerEventData touch)
    {
        EndTouch();
    }

    void EndTouch()
    {
        // 드래그가 끝나면, 터치가 끝난 것임으로, 조이스틱을 원위치로 이동 시킨다.
        if (stick == null) return;

        stick.rectTransform.position = orignPos;
        moveFlag = false;
        isJoysticUp = false;
        isJoysticDown = false;
    }

    void ControlPlayer(PointerEventData touch)
    {
        // 게임 오버 시 콘트롤러 작동 안 함
        if (GameManager.instance.isGameover) return;

        // Touch는 모바일장치에서만 동작하니 주의, PC에서는 오동작할 수 있음

        if (stick == null) return;

        Vector3 dir = (new Vector3(touch.position.x, touch.position.y, orignPos.z) - orignPos).normalized;

        float touchAreaRadius = Vector3.Distance(orignPos, new Vector3(touch.position.x, touch.position.y, orignPos.z));
        if (touchAreaRadius > stickRadius)
        {
            // 반경을 넘어가는 경우는, 현재 가려는 방향으로, 반지름 만큼만 가도록 설정한다.
            stick.rectTransform.position = orignPos + (dir * stickRadius);
        }
        else
        {
            // 조이스틱이 반경내로 움직일때만, 드래그 된 위치로 설정한다.
            stick.rectTransform.position = touch.position;
        }


        Vector3 pos = touch.position;

        // 조이스틱을 이동시킬 방향을 구함
        joyVec = (pos - orignPos).normalized;

        // 조이스틱의 처음 위치와 현재 내가 터치하고있는 위치의 거리를 구한다.
        float dis = Vector3.Distance(pos, orignPos);

        speed = PLAYER_MAX_SPEED * dis / 100;
        if (speed > PLAYER_MAX_SPEED)
        {
            speed = PLAYER_MAX_SPEED;
        }

        if (dis > 30)
        {
            
            if (joyVec.x < -0.9f)
            {
                moveFlag = true;
                playerRight = false;
                player.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (joyVec.x > 0.9f)
            {
                moveFlag = true;
                playerRight = true;
                player.transform.localScale = new Vector3(1, 1, 1);
            }

            if (joyVec.y > 0.9f) isJoysticUp = true;
            else isJoysticUp = false;

            if (joyVec.y < -0.9f) isJoysticDown = true;
            else isJoysticDown = false;
        }
        else
        {
            moveFlag = false;
            isJoysticUp = false;
            isJoysticDown = false;
        }

        //player.eulerAngles = new Vector3(0, Mathf.Atan2(joyVec.x, joyVec.y) * Mathf.Rad2Deg, 0);
    }

    
}
