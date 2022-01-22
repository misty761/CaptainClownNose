using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    public float power = 10f;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y < -0.9f)
        {
            if (collision.transform.tag == "Player")
            {
                animator.SetTrigger("FireOn");
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            animator.SetTrigger("FireOff");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("FireOn"))
            {
                PlayerController.instance.TakeDamage(power);
            }
        }
    }
}
