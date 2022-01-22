using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigBox : MonoBehaviour
{
    public GameObject part1;
    public GameObject part2;
    public GameObject part3;
    public GameObject part4;
    public GameObject pig;

    PlayerController player;
    Animator animator;
    Rigidbody2D thisRigidbody;
    AudioSource audioSource;

    float DISTANCE_JUMP_X = 1f;
    float DISTANCE_JUMP_Y = 0.5f;
    float FORCE_JUMP = 150f;
    bool isGrounded;
    bool isFalling;
    bool bJump;
    bool bDestroy;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        thisRigidbody = GetComponent<Rigidbody2D>();

        bJump = false;
        isGrounded = false;
        isFalling = false;
        bDestroy = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 thisPosition = this.transform.position;
        float distanceX = playerPosition.x - thisPosition.x;
        float distanceY = playerPosition.y - thisPosition.y;

        // 플레이어가 접근하면 점프
        if (distanceX > - DISTANCE_JUMP_X && distanceX < DISTANCE_JUMP_X)
        {
            if (distanceY > - DISTANCE_JUMP_Y && distanceY < DISTANCE_JUMP_Y)
            {
                animator.SetTrigger("PrepareJump");
            }
        }

        // 플레이어 위치에 따라 힘을 줌
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !bJump)
        {
            bJump = true;

            if (distanceX < 0)
            {
                thisRigidbody.AddForce(new Vector2(-FORCE_JUMP / 3f, FORCE_JUMP / 1.3f));
            }
            else
            {
                thisRigidbody.AddForce(new Vector2(FORCE_JUMP / 3f, FORCE_JUMP / 1.3f));
            }
        }

        // 땅에 닿으면
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            bDestroy = true;
        }

        // 상자 파괴 및 돼지 출현
        if (bDestroy && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if (SoundControl.bSoundOn) audioSource.Play();

            Instantiate(part1, this.transform.position, this.transform.rotation);
            Instantiate(part2, this.transform.position, this.transform.rotation);
            Instantiate(part3, this.transform.position, this.transform.rotation);
            Instantiate(part4, this.transform.position, this.transform.rotation);

            Instantiate(pig, this.transform.position, this.transform.rotation);

            Destroy(this.gameObject);

            bDestroy = false;
        }

        // Y 방행 속도에 따라 fall 판단
        float velocityY = thisRigidbody.velocity.y;
        if (velocityY < 0) isFalling = true;
        else isFalling = false;

        animator.SetBool("Fall", isFalling);
        animator.SetBool("Grounded", isGrounded);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.9f)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
