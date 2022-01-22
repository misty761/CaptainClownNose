using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    Vector3 originalPosition;
    Rigidbody2D myRigidbody;
    Enemy thisEnemy;
    Animator animator;
    PlayerController player;
    float DISTANCE_ACTION_X = 3.5f;
    float DISTANCE_ACTION_Y = 1.5f;
    float DISTANCE_MOVE = 1f;
    float speed = 1f;
    float speedOriginal;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = this.transform.position;
        myRigidbody = GetComponent<Rigidbody2D>();
        thisEnemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        speedOriginal = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (thisEnemy.health <= 0)
        {
            myRigidbody.velocity = Vector3.zero;
            return;
        }

        // 하늘을 날도록 위로 힘을 줌
        if (this.transform.position.y < originalPosition.y)
        {
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.AddForce(new Vector3(0, thisEnemy.jumpForce / 6f, 0));
        }

        // 땅에 닿으면 위로 힘을 줌
        if (thisEnemy.isGrounded)
        {
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.AddForce(new Vector3(0, thisEnemy.jumpForce / 5f, 0));
        }

        Vector3 playerPosition = player.transform.position;
        Vector3 thisPosition = this.transform.position;
        float distanceX = playerPosition.x - thisPosition.x;
        float distanceY = playerPosition.y - thisPosition.y;

        // 플레이어 근처에 있을 때만 움직임
        if (distanceX > -DISTANCE_ACTION_X && distanceX < DISTANCE_ACTION_X
            && distanceY > -DISTANCE_ACTION_Y && distanceY < DISTANCE_ACTION_Y)
        {
            if (!thisEnemy.isAttacking && !animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") && !thisEnemy.isGrounded)
            {
                if (this.transform.position.x > originalPosition.x - DISTANCE_MOVE * thisEnemy.randomDistance && !thisEnemy.enemyRight)
                {
                    this.transform.Translate(Vector3.left * Time.deltaTime * speed);
                    if (speed > 0) thisEnemy.isMoving = true;
                }
                else if (this.transform.position.x < originalPosition.x + DISTANCE_MOVE * thisEnemy.randomDistance && !thisEnemy.enemyRight)
                {
                    thisEnemy.enemyRight = true;
                    thisEnemy.randomDistance = Random.Range(0.5f, 1.5f);
                }
                else if (this.transform.position.x < originalPosition.x + DISTANCE_MOVE * thisEnemy.randomDistance && thisEnemy.enemyRight)
                {
                    this.transform.Translate(Vector3.right * Time.deltaTime * speed);
                    if (speed > 0) thisEnemy.isMoving = true;
                }
                else if (this.transform.position.x > originalPosition.x + DISTANCE_MOVE * thisEnemy.randomDistance && thisEnemy.enemyRight)
                {
                    thisEnemy.enemyRight = false;
                    thisEnemy.randomDistance = Random.Range(0.5f, 1.5f);
                }
            }
        }

        // 총알 발사시 정지 
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Fire"))
        {
            speed = 0f;
        }
        else
        {
            speed = speedOriginal;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.transform.tag == "Ground" || collision.transform.tag == "Wall")
            && (collision.contacts[0].normal.x > 0.9 || collision.contacts[0].normal.x < -0.9)) 
        {
            thisEnemy.enemyRight = !thisEnemy.enemyRight;
        }
    }

}
