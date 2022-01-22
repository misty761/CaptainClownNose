using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public float DISTANCE_GO_DOWN = 1f;
    public GameObject batPrefabs;

    Animator animator;
    Vector3 playerPosition;
    Vector3 originalPosition;
    bool isflying;
    readonly float ATTACK_DISTANCE_X = 1.7f;
    readonly float ATTACK_DISTANCE_Y = 1.1f;
    readonly float SPEED = 1f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        isflying = false;
        originalPosition = this.transform.position;
    }

    private void Update()
    {
        animator.SetBool("Fly", isflying);

        playerPosition = PlayerController.instance.transform.position;

        // 플레이어 접근하면
        if (playerPosition.x - this.transform.position.x > -ATTACK_DISTANCE_X
            && playerPosition.x - this.transform.position.x < ATTACK_DISTANCE_X)
        {
            if (playerPosition.y - this.transform.position.y > -ATTACK_DISTANCE_Y
                && playerPosition.y - this.transform.position.y < 0.3f)
            {
                isflying = true;

                // DISTANCE_GO_DOWN 만큼 내려 가서 physics를 가진 박쥐 생성
                if (this.transform.position.y > originalPosition.y - DISTANCE_GO_DOWN)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Fly"))
                    {
                        this.transform.Translate(Vector3.down * Time.deltaTime * SPEED);
                    }
                }
                else
                {
                    Instantiate(batPrefabs, this.transform.position, this.transform.rotation);
                    Destroy(this.gameObject);
                }

            }
        }
   
    }
}
