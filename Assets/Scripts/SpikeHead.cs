using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHead : MonoBehaviour
{
    public bool clockWise;

    float SPEED_MAX = 2f;
    float speed;
    float power = 10f;
    int direction;          // 0:left, 1:up, 2:right, 3: down
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        speed = 0f;
        direction = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            speed += Time.deltaTime;
            if (speed > SPEED_MAX) speed = SPEED_MAX;
        }
        else speed = 0f;
        
        // 왼쪽으로 움직임
        if (direction == 0)
        {
            transform.Translate(Vector2.left * Time.deltaTime * speed);
        }

        // 위로 움직임
        if (direction == 1)
        {
            transform.Translate(Vector2.up * Time.deltaTime * speed);
        }

        // 오른쪽으로 움직임
        if (direction == 2)
        {
            transform.Translate(Vector2.right * Time.deltaTime * speed);
        }

        // 아래로 움직임
        if (direction == 3)
        {
            transform.Translate(Vector2.down * Time.deltaTime * speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 왼쪽 오브젝트와 충돌
        if (collision.contacts[0].normal.x > 0.99f)
        {
            animator.SetTrigger("LeftHit");
            if (clockWise) direction = 1;
            else direction = 3;
        }
        // 오른쪽 오브젝트와 충돌
        else if (collision.contacts[0].normal.x < -0.99f)
        {
            animator.SetTrigger("RightHit");
            if (clockWise) direction = 3;
            else direction = 1;
        }
        // 아래 오브젝트와 충돌
        else if (collision.contacts[0].normal.y > 0.99f)
        {
            animator.SetTrigger("BottomHit");
            if (clockWise) direction = 0;
            else direction = 2;
        }
        // 위 오브젝트와 충돌
        else if (collision.contacts[0].normal.y < -0.99f)
        {
            animator.SetTrigger("TopHit");
            if (clockWise) direction = 2;
            else direction = 0;
        }

        // 플레이어와 충돌시
        if (collision.transform.tag == "Player")
        {
            PlayerController.instance.TakeDamage(power);
        }
    }
}
