using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GooglePlayGames;

public class LeaderBoard : MonoBehaviour
{
    AudioSource audioButton;
    string LEADER_BOARD_ID = "CgkIj_XSk_sFEAIQAA";

    // Start is called before the first frame update
    void Start()
    {
        audioButton = GetComponent<AudioSource>();

        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry_TouchDown = new EventTrigger.Entry();
        entry_TouchDown.eventID = EventTriggerType.PointerDown;
        entry_TouchDown.callback.AddListener((data) => { TouchDown(); });
        eventTrigger.triggers.Add(entry_TouchDown);

        EventTrigger.Entry entry_TouchUp = new EventTrigger.Entry();
        entry_TouchUp.eventID = EventTriggerType.PointerUp;
        entry_TouchUp.callback.AddListener((data) => { TouchUp(); });
        eventTrigger.triggers.Add(entry_TouchUp);
    }

    private void TouchDown()
    {
        if (SoundControl.bSoundOn) audioButton.Play();
    }

    private void TouchUp()
    {
        RankButtonClick();
    }

    public void RankButtonClick()
    {
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate(AuthenticateHandler);
    }

    void AuthenticateHandler(bool isSuccess)
    {
        if (isSuccess)
        {
            int highScore = PlayerPrefs.GetInt("BestScore");
            Social.ReportScore((long) highScore, LEADER_BOARD_ID, (bool success) =>
            {
                if (success)
                {
                    PlayGamesPlatform.Instance.ShowLeaderboardUI(LEADER_BOARD_ID);
                    Debug.Log("Show Leader Board UI : " + success);
                    Debug.Log("highScore : " + highScore);
                }
                else
                {
                    Debug.Log("Show Leader Board UI : " + success);
                }
            });
        }
        else
        {
            // login failed
            Debug.Log("Login failed to Google Play Games : " + isSuccess);
        }
    }
}
