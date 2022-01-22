using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPlatform : MonoBehaviour
{
    public GameObject fallingPlatform;
    
    Vector2 originalPosition;
    float SPEED = 0.5f;
    bool flying;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        originalPosition = this.transform.position;
        flying = true;
    }

    private void Update()
    {
        float positionY = this.transform.position.y;
        if (positionY < originalPosition.y)
        {
            this.transform.Translate(Vector3.up * Time.deltaTime * SPEED);
        }

        animator.SetBool("Flying", flying);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y < -0.9f)
        {
            if (collision.transform.tag == "Player")
            {
                fallingPlatform.transform.position = this.transform.position;
                fallingPlatform.SetActive(true);
                this.gameObject.SetActive(false);
            } 
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            fallingPlatform.transform.position = this.transform.position;
            fallingPlatform.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
