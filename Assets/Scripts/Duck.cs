using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{
    Animator animator;
    Enemy enemy;
    Rigidbody2D myRigidbody;
    readonly float JUMP_INTERVAL = 1.5f;
    float timeJump;
    float randomJump;
    float randomDistance;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        myRigidbody = GetComponent<Rigidbody2D>();

        timeJump = 0f;
        randomJump = Random.Range(0.5f, 1.5f);
        randomDistance = Random.Range(0.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.health < 0) return;

        timeJump += Time.deltaTime;

        if (timeJump > JUMP_INTERVAL * randomJump)
        {
            animator.SetTrigger("PrepareJump");
            timeJump = 0f;
            randomJump = Random.Range(0.5f, 1.5f);
        }

        if (enemy.isGrounded && !enemy.isAttacking && !animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            if (this.transform.position.x > enemy.originalPosition.x - enemy.movingDistance * randomDistance && !enemy.enemyRight)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
                {
                    myRigidbody.velocity = Vector3.zero;
                    myRigidbody.AddForce(new Vector2(-enemy.jumpForce / 3f, enemy.jumpForce / 1.3f));
                }
            }
            else if (this.transform.position.x < enemy.originalPosition.x + enemy.movingDistance * randomDistance && !enemy.enemyRight)
            {
                enemy.enemyRight = true;
                randomDistance = Random.Range(0.5f, 1.5f);
            }
            else if (this.transform.position.x < enemy.originalPosition.x + enemy.movingDistance * randomDistance && enemy.enemyRight)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
                {
                    myRigidbody.velocity = Vector3.zero;
                    myRigidbody.AddForce(new Vector2(enemy.jumpForce / 3f, enemy.jumpForce / 1.3f));
                }
            }
            else if (this.transform.position.x > enemy.originalPosition.x + enemy.movingDistance * randomDistance && enemy.enemyRight)
            {
                enemy.enemyRight = false;
                randomDistance = Random.Range(0.5f, 1.5f);
            }
        }
    }
}
