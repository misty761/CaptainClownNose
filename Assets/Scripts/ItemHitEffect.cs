using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHitEffect : MonoBehaviour
{
    Animator animator;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (SoundControl.bSoundOn) audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            if (!audioSource.isPlaying)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
