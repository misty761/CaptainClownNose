using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    public GameObject canvas;
    public GameObject myEvent;

    AudioSource audioSource;
    PlayerController player;

    bool bTouch;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<PlayerController>();

        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry_TouchUp = new EventTrigger.Entry();
        entry_TouchUp.eventID = EventTriggerType.PointerUp;
        entry_TouchUp.callback.AddListener((data) => { TouchUp(); });
        eventTrigger.triggers.Add(entry_TouchUp);

        bTouch = false;
    }

    private void Update()
    {
        if (bTouch && !audioSource.isPlaying)
        {
            bTouch = false;

            GameManager.instance.NewGame();
            GameManager.instance.NewLevel();

            SceneManager.LoadScene("SelectStage");
        }
    }

    private void TouchUp()
    {
        if (SoundControl.bSoundOn)
        {
            audioSource.Play();
        }
        bTouch = true;
    }
}
