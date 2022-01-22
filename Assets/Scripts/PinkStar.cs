using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkStar : MonoBehaviour
{
    Animator animator;
    Enemy thisEnemy;
    float ATTACK_SPEED = 1.8f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        thisEnemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && thisEnemy.isGrounded)
        {
            if (thisEnemy.enemyRight)
            {
                this.transform.Translate(Vector3.right * Time.deltaTime * ATTACK_SPEED);
            }
            else
            {
                this.transform.Translate(Vector3.left * Time.deltaTime * ATTACK_SPEED);
            }
        }
    }
}
