using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool bOpen;
    public Animator animator;

    AudioSource audioSource;
    bool atDoor;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        atDoor = false;
        bOpen = false;
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && atDoor && Controller.isJoysticUp && !ExitHospital.isPlayerInHospital)
        {
            if (SoundControl.bSoundOn) audioSource.Play();
            animator.SetTrigger("Open");
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Opened") && !bOpen)
        {
            bOpen = true;
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
        if(collision.tag == "Player")
        {
            atDoor = false;
        }
    }

}
