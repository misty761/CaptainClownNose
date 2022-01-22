using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public GameObject particle;

    Animator animator;
    Enemy enemy;
    Vector3 positionParticle;
    bool isAppeared;
    float timeParticle;
    readonly float INTERVAL_PARTICLE = 0.3f;
    readonly float OFFSET_X = -0.05f;
    readonly float DISTANCE_APPEAR_X = 2f;
    readonly float DISTANCE_APPEAR_Y = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();

        timeParticle = 0f;
        isAppeared = false;
    }

    // Update is called once per frame
    void Update()
    {
        timeParticle += Time.deltaTime;

        float distanceX = PlayerController.instance.transform.position.x - this.transform.position.x;
        float distanceY = PlayerController.instance.transform.position.y - this.transform.position.y;
        if (distanceX > -DISTANCE_APPEAR_X && distanceX < DISTANCE_APPEAR_X
            && distanceY > -DISTANCE_APPEAR_Y && distanceY < DISTANCE_APPEAR_Y) isAppeared = true;
        else isAppeared = false;
        animator.SetBool("Appeared", isAppeared);

        if (!enemy.enemyRight)
        {
            positionParticle = new Vector3(this.transform.position.x + OFFSET_X, 
                                            this.transform.position.y, 
                                            this.transform.position.z);
        }
        else
        {
            positionParticle = new Vector3(this.transform.position.x - OFFSET_X,
                                            this.transform.position.y,
                                            this.transform.position.z);
        }

        if (timeParticle > INTERVAL_PARTICLE && isAppeared)
        {
            Instantiate(particle, positionParticle, this.transform.rotation);
            timeParticle = 0f;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) enemy.power = 10;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetTrigger("Collision");
    }
}
