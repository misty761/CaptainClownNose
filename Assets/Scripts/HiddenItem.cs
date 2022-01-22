using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenItem : MonoBehaviour
{
    public GameObject silverCoin;
    public GameObject goldCoin;
    public GameObject heart;
    public GameObject healthPotion;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            float random = Random.Range(0f, 1.1f);
            if (random < 0.1f)
            {
                // 10 골드
                SpawnObject(goldCoin);
            }
            else if (random < 0.2f)
            {
                // 회복약(체력 100% 회복)
                SpawnObject(healthPotion);
            }
            else if (random < 0.6f)
            {
                // 하트(체력 +10)
                SpawnObject(heart);
            }
            else 
            {
                // 1 골드
                SpawnObject(silverCoin);
            }

            Destroy(this.gameObject);
        }
        
    }

    void SpawnObject(GameObject item)
    {
        GameObject spawnItem = Instantiate(item, new Vector2(transform.position.x, transform.position.y + 0.1f), transform.rotation);
    }
}

