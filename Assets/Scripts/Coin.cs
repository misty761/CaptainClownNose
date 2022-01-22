using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int priceMin;
    public int priceMax;
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
            Instantiate(hitEffect, this.transform.position, this.transform.rotation);
            Destroy(gameObject);
            int random = Random.Range(priceMin, priceMax + 1);
            GameManager.instance.AddMoney(random);
        }
    }
}
