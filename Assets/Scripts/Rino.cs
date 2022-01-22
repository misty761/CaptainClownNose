using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rino : MonoBehaviour
{
    Animator animator;
    Enemy enemy;
    Rigidbody2D myRigidbody;
    float random;
    float timeRun;
    float INTERVAL_RUN = 2f;
    float SPEED_RUN = 2f;
    bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        myRigidbody = GetComponent<Rigidbody2D>();

        random = Random.Range(0.5f, 1.5f);
        timeRun = 0f;
        isRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        timeRun += Time.deltaTime;

        if (timeRun * random > INTERVAL_RUN && !isRunning)
        {
            timeRun = 0f;
            isRunning = true;
            random = Random.Range(0.5f, 1.5f);
        }

        if (isRunning)
        {
            enemy.speed = SPEED_RUN;
        }
        else
        {
            enemy.speed = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isRunning)
        {
            isRunning = false;
            animator.SetTrigger("Collision");
            
            if (myRigidbody.velocity.y <= 0)
            {
                myRigidbody.velocity = Vector3.zero;
                if (enemy.enemyRight) myRigidbody.AddForce(new Vector3(-enemy.jumpForce / 4f, enemy.jumpForce / 2f, 0));
                else myRigidbody.AddForce(new Vector3(enemy.jumpForce / 4f, enemy.jumpForce / 2f, 0));
            }
            else
            {
                myRigidbody.velocity = Vector3.zero;
                if (enemy.enemyRight) myRigidbody.AddForce(new Vector3(-enemy.jumpForce / 4f, 0, 0));
                else myRigidbody.AddForce(new Vector3(enemy.jumpForce / 4f, 0, 0));
            }
            
        }
    }
}
