using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefabs;
    public float shootingInterval;
    public float bulletOffsetX;
    public float bulletOffsetY;

    public bool isRight;
    
    public PlayerController player;

    float timeShooting;
    float timeAnim;
    float random;
    bool bFire;
    float SHOOTING_RANGE_X = 2f;
    float SHOOTING_RANGE_Y = 1f;
    float originalSpeed;
    
    Vector3 shootingSpot;
    Animator animator;
    GameObject bullet;
    Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        player = FindObjectOfType<PlayerController>();

        timeShooting = 0f;
        timeAnim = 0f;
        bFire = false;
        originalSpeed = enemy.speed;

        random = Random.Range(0.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.isDead) return;

        timeShooting += Time.deltaTime;
        timeAnim += Time.deltaTime;

        // 플레이어를 바라 보도록 함
        if (this.transform.position.x - player.transform.position.x > 0)
        {
            isRight = false;
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        else 
        {
            isRight = true;
            this.transform.localScale = new Vector3(-1, 1, 1);
        }

        // 플레이어와 가까이 있을 때만 발사
        float distanceX = player.transform.position.x - this.transform.position.x;
        float distanceY = player.transform.position.y - this.transform.position.y;
        if (distanceX > - SHOOTING_RANGE_X && distanceX < SHOOTING_RANGE_X)
        {
            if (distanceY > - SHOOTING_RANGE_Y && distanceY < SHOOTING_RANGE_Y && enemy.isGrounded)
            {
                if (timeShooting > shootingInterval * random)
                {
                    timeShooting = 0f;

                    random = Random.Range(0.5f, 1.5f);

                    if (isRight)
                    {
                        shootingSpot = new Vector3(this.transform.position.x + bulletOffsetX,
                                                    this.transform.position.y + bulletOffsetY,
                                                    this.transform.position.z);
                    }
                    else
                    {
                        shootingSpot = new Vector3(this.transform.position.x - bulletOffsetX,
                                                    this.transform.position.y + bulletOffsetY,
                                                    this.transform.position.z);
                    }

                    bFire = true;

                    animator.SetTrigger("Fire");
                    timeAnim = 0f;
                }
            }
        }

        // 총알 생성
        if (bFire && !animator.GetCurrentAnimatorStateInfo(0).IsName("Fire") && timeAnim > 0.1f)
        {
            bFire = false;
            bullet = Instantiate(bulletPrefabs, shootingSpot, this.transform.rotation);
            bullet.gameObject.SetActive(true);
        }

        // 발사 동안 정지
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Fire"))
        {
            enemy.speed = 0f;
        }
        else
        {
            enemy.speed = originalSpeed;
        }

    }
}
