using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radish : MonoBehaviour
{
    public GameObject leaf1;
    public GameObject leaf2;

    Animator animator;
    Enemy enemy;

    bool haveLeaves;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        haveLeaves = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            enemy.speed = 1f;
            enemy.movingDistance = 1f;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") && haveLeaves)
        {
            Instantiate(leaf1, this.transform.position, this.transform.rotation);
            Instantiate(leaf2, this.transform.position, this.transform.rotation);
            haveLeaves = false;
        }
    }
}
