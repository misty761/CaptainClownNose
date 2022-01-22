using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float jumpForce;

    PlayerController player;
    Animator animator;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (collision.contacts[0].normal.y < -0.1f)
            {
                if (SoundControl.bSoundOn) audioSource.Play();
                animator.SetTrigger("Jump");
                player.playerRigidbody.velocity = Vector2.zero;
                player.playerRigidbody.AddForce(new Vector2(0, jumpForce));
            }   
        }
    }
}
