using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatBird : MonoBehaviour
{
    Animator animator;
    Enemy enemy;
    Rigidbody2D myRigidBody;
    bool goUp;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        myRigidBody = GetComponent<Rigidbody2D>();
        goUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.isGrounded && !animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded") && !goUp)
        {
            goUp = true;
        }

        if (this.transform.position.y > enemy.originalPosition.y)
        {
            goUp = false;
        }

        if (goUp)
        {
            myRigidBody.velocity = Vector3.zero;
            myRigidBody.AddForce(new Vector3(0, enemy.jumpForce / 9f, 0));
        }
    }
}
