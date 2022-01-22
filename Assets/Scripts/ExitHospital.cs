using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ExitHospital : MonoBehaviour
{
    public static bool isPlayerInHospital;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry_TouchUp = new EventTrigger.Entry();
        entry_TouchUp.eventID = EventTriggerType.PointerUp;
        entry_TouchUp.callback.AddListener((data) => { TouchUp(); });
        eventTrigger.triggers.Add(entry_TouchUp);

        audioSource = GetComponent<AudioSource>();

        isPlayerInHospital = true;
    }

    private void Update()
    {
        if (!isPlayerInHospital && !audioSource.isPlaying)
        {
            SceneManager.UnloadSceneAsync("Hospital");

            Time.timeScale = 1;
        }
    }

    private void TouchUp()
    {
        if (SoundControl.bSoundOn) audioSource.Play();

        isPlayerInHospital = false;
    }
}
