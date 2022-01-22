using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : MonoBehaviour
{
    public GameObject body;
    public GameObject shell;
    public GameObject silverCoin;
    public GameObject goldCoin;
    public GameObject heart;
    public GameObject healthPotion;

    Enemy enemy;
    bool dead;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.health <= 0 && !dead)
        {
            dead = true;
            Instantiate(body, this.transform.position, this.transform.rotation);
            Instantiate(shell, this.transform.position, this.transform.rotation);
            float random = Random.Range(0f, 1.0f);
            if (random < 0.9f) GetItem();
            GameManager.instance.AddScore(enemy.point);
            Destroy(enemy.hpBar.gameObject);
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
        else
        {
            // 1 골드
            SpawnObject(silverCoin);
        }

    }

    void SpawnObject(GameObject item)
    {
        Instantiate(item, transform.position, transform.rotation);
    }
}
