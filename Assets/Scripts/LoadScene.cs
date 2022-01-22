using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string sceneName;

    bool bitTouch;
    bool isADshowed;

    // Start is called before the first frame update
    void Start()
    {
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry_TouchUp = new EventTrigger.Entry();
        entry_TouchUp.eventID = EventTriggerType.PointerUp;
        entry_TouchUp.callback.AddListener((data) => { TouchUp(); });
        eventTrigger.triggers.Add(entry_TouchUp);

        bitTouch = false;
        isADshowed = false;
    }

    private void Update()
    {
        if (bitTouch)
        {
            if (GoogleMobileAdsReward.instance.rewardedAd.IsLoaded())
            {
                bitTouch = false;
                isADshowed = true;
                GoogleMobileAdsReward.instance.rewardedAd.Show();
            }
        }

        if (isADshowed && GoogleMobileAdsReward.instance.isRewarded && GoogleMobileAdsReward.instance.bCloseAD)
        {
            isADshowed = false;

            GameManager.instance.NewGame();
            GameManager.instance.NewLevel();

            SetActiveUI();

            Time.timeScale = 1;

            SceneManager.LoadScene(sceneName);
        }
    }

    private void TouchUp()
    {
        GameManager.instance.PlayClickSound();

        if (sceneName == "SelectStage")
        {
            SceneManager.LoadScene(sceneName);
        }
        else if(sceneName == "Scene01")
        {
            SetActiveUI();

            Time.timeScale = 1;

            SceneManager.LoadScene(sceneName);
        }
        else
        {
            bitTouch = true;
        }

    }

    void SetActiveUI()
    {
        UIManager.instance.jumpButton.gameObject.SetActive(true);
        UIManager.instance.attackButton.gameObject.SetActive(true);
        UIManager.instance.touchController.gameObject.SetActive(true);
        UIManager.instance.healthBar.gameObject.SetActive(true);
        UIManager.instance.score.gameObject.SetActive(true);
        UIManager.instance.money.gameObject.SetActive(true);
        UIManager.instance.playerLevel.gameObject.SetActive(true);
    }
}
