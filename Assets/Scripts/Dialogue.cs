using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    float time;
    float TIME_OUT = 2f;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        time = 0f;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > TIME_OUT) animator.SetTrigger("Out");

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("End"))
        {
            Destroy(this.gameObject);
        }
    }
}
