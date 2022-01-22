using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryPig : MonoBehaviour
{
    Animator animator;
    Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            enemy.speed = 1f;
            enemy.power = 20;
            enemy.jumpInterval = 5f;
        }
    }
}
