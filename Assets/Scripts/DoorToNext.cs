using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToNext : MonoBehaviour
{
    public bool bOpen;
    public string sceneName;
    public Animator animator;
    public Chest chest;

    AudioSource audioSource;
    bool atDoor;
    bool isLocked;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        atDoor = false;
        bOpen = false;
        isLocked = true;
    }

    private void Update()
    {
        if (chest.isOpened) isLocked = false;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") 
            && atDoor && Controller.isJoysticUp && !ExitHospital.isPlayerInHospital && !isLocked)
        {
            if (SoundControl.bSoundOn) audioSource.Play();

            animator.SetTrigger("Open");
            
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Opened") && !bOpen)
        {
            bOpen = true;
            Time.timeScale = 0;
            SceneManager.LoadScene(sceneName);
        }
        else bOpen = false;  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            atDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            atDoor = false;
        }
    }

}
