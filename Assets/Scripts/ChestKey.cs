using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestKey : MonoBehaviour
{
    public GameObject hitEffect;
    
    float jumpForce = 110f;
    Rigidbody2D myRigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myRigidbody.velocity = Vector2.zero;
        myRigidbody.AddForce(new Vector2(0, jumpForce));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(hitEffect, this.transform.position, this.transform.rotation);
            Destroy(gameObject);
            GameManager.instance.haveKey = true;
        }
    }
}
