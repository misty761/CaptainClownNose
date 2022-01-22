using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyPotion : MonoBehaviour
{
    public AudioClip audioNegative;
    public AudioClip audioCoin;
    public int price;
    public int effect;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry_TouchUp = new EventTrigger.Entry();
        entry_TouchUp.eventID = EventTriggerType.PointerUp;
        entry_TouchUp.callback.AddListener((data) => { TouchUp(); });
        eventTrigger.triggers.Add(entry_TouchUp);
    }

    private void TouchUp()
    {
        if (PlayerController.instance.health >= 100 || GameManager.instance.money < price)
        {
            if (SoundControl.bSoundOn)
            {
                audioSource.clip = audioNegative;
                audioSource.Play();
            }
        }
        else if (GameManager.instance.money >= price)
        {
            if (SoundControl.bSoundOn)
            {
                audioSource.clip = audioCoin;
                audioSource.Play();
            }

            GameManager.instance.SpendMoney(price);
            PlayerController.instance.health += effect;
            if (PlayerController.instance.health > 100)
            {
                PlayerController.instance.health = 100;
            }
        }
    }
}
