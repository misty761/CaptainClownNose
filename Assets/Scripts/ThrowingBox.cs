using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingBox : MonoBehaviour
{
    public GameObject part1;
    public GameObject part2;
    public GameObject part3;
    public GameObject part4;
    public GameObject pig;
    public GameObject silverCoin;
    public GameObject goldCoin;
    public GameObject heart;
    public GameObject healthPotion;

    PlayerController player;
    Rigidbody2D thisRigidbody;
    Enemy enemy;
    Animator animator;
    bool bDestroy;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        thisRigidbody = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();

        bDestroy = false;

        Vector3 playerPosition = player.transform.position;
        Vector3 thisPosition = this.transform.position;
        float distanceX = playerPosition.x - thisPosition.x;
        //float distanceY = playerPosition.y - thisPosition.y;

        // 플레이어가 왼쪽에 있으면
        if (distanceX < 0)
        {
            thisRigidbody.AddForce(new Vector2(-enemy.jumpForce/2f ,enemy.jumpForce/1.3f));
        }
        // 플레이어가 오른쪽에 있으면
        else
        {
            thisRigidbody.AddForce(new Vector2(enemy.jumpForce/2f , enemy.jumpForce/1.3f));
        }
       
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Destroyed"))
        {
            SpawnObject(part1);
            SpawnObject(part2);
            SpawnObject(part3);
            SpawnObject(part4);
            float random = Random.Range(0f, 1.0f);
            if (random < 0.5f) GetItem();
            Destroy(this.gameObject);
        }
    }

    void GetItem()
    {
        float random = Random.Range(0f, 1.0f);

        if (random < 0.01f)
        {
            // 10 골드
            SpawnObject(goldCoin);
        }
        else if (random < 0.02f)
        {
            // 회복약(체력 100% 회복)
            SpawnObject(healthPotion);
        }
        else if (random < 0.1f)
        {
            // 하트(체력 +10)
            SpawnObject(heart);
        }
        else if (random < 0.5f)
        {
            // 실버 코인
            SpawnObject(silverCoin);
        }
        else
        {
            // 1 골드
            SpawnObject(pig);
        }

    }

    void SpawnObject(GameObject item)
    {
        Instantiate(item, transform.position, transform.rotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!bDestroy && collision.transform.tag != "Enemy")
        {
            animator.SetTrigger("Collision");
            bDestroy = true;
        }
    }
}
