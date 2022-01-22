using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public int addHealth;
    public GameObject hitEffect;

    Rigidbody2D myRigidbody;
    AudioSource audioSource;
    float jumpForce = 110f;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        if (SoundControl.bSoundOn) audioSource.Play();

        myRigidbody.velocity = Vector2.zero;
        myRigidbody.AddForce(new Vector2(0, jumpForce));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController.instance.health += addHealth;
            if (PlayerController.instance.health > 100) PlayerController.instance.health = 100;

            Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
