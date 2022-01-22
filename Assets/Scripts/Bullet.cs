using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float power;
    public float speed;
    public Shooter shooter;

    Animator animator;
    Vector3 shootingDirection;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (shooter.isRight)
        {
            shootingDirection = Vector3.right;
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            shootingDirection = Vector3.left;
            this.transform.localScale = new Vector3(1, 1, 1);
        }      
    }

    void Update()
    {
        this.transform.Translate(shootingDirection * Time.deltaTime * speed);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Destroyed")) Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Wall" || other.tag == "Ground")
        {
            speed = 0f;

            animator.SetTrigger("Collision");

            if (other.tag == "Player") PlayerController.instance.TakeDamage(power);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground")
        {
            animator.SetTrigger("Collision");

            if (collision.gameObject.tag == "Player") PlayerController.instance.TakeDamage(power);
        }
    }

}
