using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public GameObject flyingPlatform;

    Animator animator;
    bool flying;

    private void Start()
    {
        animator = GetComponent<Animator>();
        flying = false;
    }

    private void Update()
    {
        animator.SetBool("Flying", flying);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            flyingPlatform.transform.position = this.transform.position;
            flyingPlatform.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
