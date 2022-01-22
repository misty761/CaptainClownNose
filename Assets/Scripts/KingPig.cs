using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingPig : MonoBehaviour
{
    public GameObject enemy1;
    public GameObject enemy2;

    Animator animator;

    float POSITION_PIG_SPAWN_X = 0f;
    float POSITION_PIG_SPAWN_Y = -1.13f;

    bool bSpawn;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        bSpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 공격시 돼지 소환
        if (!bSpawn && animator.GetCurrentAnimatorStateInfo(0).IsName("PrepareAttack"))
        {
            bSpawn = true;
            
            Vector2 position = new Vector2(POSITION_PIG_SPAWN_X, POSITION_PIG_SPAWN_Y);
            float random = Random.Range(0f, 1f);
            if (random < 0.5f)
            {
                Instantiate(enemy1, position, Quaternion.Euler(0, 0, 0));
            }
            else
            {
                Instantiate(enemy2, position, Quaternion.Euler(0, 0, 0));
            }
            
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) bSpawn = false;
    }
}
