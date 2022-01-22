using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigWithMatch : MonoBehaviour
{
    public GameObject pig;
    public Animator animator;
    public bool fireRight = false;

    PlayerController player;
    Enemy enemy;
    
    readonly float DISTANCE_LIGHT_X = 1.6f;
    readonly float DISTANCE_LIGHT_Y = 1.6f;
    readonly float DISTANCE_MOVE_X = 0.5f;
    readonly float DISTANCE_MOVE_Y = 0.5f;
    readonly float INTERVAL_LIGHT = 3f;
    float timeLight;
    float random;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();

        timeLight = 0f;
        random = Random.Range(0.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        timeLight += Time.deltaTime;
        Vector3 playerPosition = player.transform.position;
        Vector3 thisPosition = this.transform.position;
        float distanceX = playerPosition.x - thisPosition.x;
        float distanceY = playerPosition.y - thisPosition.y;

        // 플레이어가 왼쪽에 있을 때만 성냥에 불을 붙임
        if (!fireRight)
        {
            // 플레이어가 접근하면 성냥에 불을 붙임
            if (distanceX > -DISTANCE_LIGHT_X && distanceX < 0)
            {
                if (distanceY > -DISTANCE_LIGHT_Y && distanceY < DISTANCE_LIGHT_Y)
                {
                    if (timeLight > INTERVAL_LIGHT * random)
                    {
                        timeLight = 0f;
                        random = Random.Range(0.5f, 1.5f);
                        animator.SetTrigger("Light");
                    }
                }
            }
        }
        // 플레이어가 오른쪽에 있을 때만 성냥에 불을 붙임
        else
        {
            // 플레이어가 접근하면 성냥에 불을 붙임
            if (distanceX > 0 && distanceX < DISTANCE_LIGHT_X)
            {
                if (distanceY > -DISTANCE_LIGHT_Y && distanceY < DISTANCE_LIGHT_Y)
                {
                    if (timeLight > INTERVAL_LIGHT * random)
                    {
                        timeLight = 0f;
                        random = Random.Range(0.5f, 1.5f);
                        animator.SetTrigger("Light");
                    }
                }
            }
        }
      
        // 플레이어가 더 가까이 접근하면 움직임
        if (distanceX > -DISTANCE_MOVE_X && distanceX < DISTANCE_MOVE_X)
        {
            if (distanceY > -DISTANCE_MOVE_Y && distanceY < DISTANCE_MOVE_Y)
            {
                Instantiate(pig, this.transform.position, Quaternion.Euler(0,0,0));
                Destroy(this.gameObject);
            }
        }
    }
}
