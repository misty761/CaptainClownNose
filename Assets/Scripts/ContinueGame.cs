using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ContinueGame : MonoBehaviour
{
    public GameObject canvas;
    //public GameObject myEvent;

    bool bTouch;

    // Start is called before the first frame update
    void Start()
    {
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry_TouchUp = new EventTrigger.Entry();
        entry_TouchUp.eventID = EventTriggerType.PointerUp;
        entry_TouchUp.callback.AddListener((data) => { TouchUp(); });
        eventTrigger.triggers.Add(entry_TouchUp);

        bTouch = false;
    }

    private void Update()
    {
        if (bTouch)
        {
            if (GoogleMobileAdsReward.instance.rewardedAd.IsLoaded())
            {
                bTouch = false;
                GoogleMobileAdsReward.instance.rewardedAd.Show();
            }
        }

        if (GoogleMobileAdsReward.instance.isRewarded && GoogleMobileAdsReward.instance.bCloseAD)
        {
            string currentScene = SceneManager.GetActiveScene().name;

            GameManager.instance.NewGame();

            SceneManager.LoadScene(currentScene);
        }
    }

    private void TouchUp()
    {
        GameManager.instance.PlayClickSound();

        bTouch = true;
    }
}
