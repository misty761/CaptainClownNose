using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigThrow : MonoBehaviour
{
    public GameObject throwItem;
    public GameObject pig;
    public float THROW_OFFSET_X;
    public float THROW_OFFSET_Y;

    PlayerController player;
    Animator animator;
    Enemy enemy;

    float DISTANCE_JUMP_X = 1f;
    float DISTANCE_JUMP_Y = 0.4f;
    
    bool bThrow;
    Vector2 positionThrow;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();

        bThrow = false;
        positionThrow = new Vector2(this.transform.position.x, this.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 thisPosition = this.transform.position;
        float distanceX = playerPosition.x - thisPosition.x;
        float distanceY = playerPosition.y - thisPosition.y;

        // 플레이어가 접근하면 박스를 던짐
        if (distanceX > -DISTANCE_JUMP_X && distanceX < DISTANCE_JUMP_X)
        {
            if (distanceY > -DISTANCE_JUMP_Y && distanceY < DISTANCE_JUMP_Y)
            {
                enemy.speed = 0f;   // 멈춰서 던짐
                bThrow = true;
            }
        }

        // 던지는 애니애이션이 끝나면
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") 
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            if (bThrow)
            {
                bThrow = false;
                // 플레이어가 왼쪽에 있으면
                if (distanceX < 0)
                {
                    this.transform.localScale = new Vector3(1, 1, 1);
                    positionThrow = new Vector2(this.transform.position.x - THROW_OFFSET_X, 
                                                this.transform.position.y + THROW_OFFSET_Y);
                }
                // 플레이어가 오른쪽에 있으면
                else
                {
                    this.transform.localScale = new Vector3(-1, 1, 1);
                    positionThrow = new Vector2(this.transform.position.x + THROW_OFFSET_X, 
                                                this.transform.position.y + THROW_OFFSET_Y);
                }
                
                Instantiate(throwItem, positionThrow, this.transform.rotation);
                Instantiate(pig, thisPosition, this.transform.rotation);
                if (enemy.hpBar != null) Destroy(enemy.hpBar.gameObject);
                Destroy(this.gameObject);
            }
   
        }

    }
}
