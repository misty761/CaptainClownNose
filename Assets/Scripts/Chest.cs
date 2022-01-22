using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public bool isOpened;
    public GameObject lockPrefabs;
    public GameObject silverCoinPrefabs;
    public GameObject goldCoinPrefabs;
    public GameObject heartPrefabs;
    public GameObject potionPrefabs;
    public GameObject diamondPrefabs;
    public GameObject saphirePrefabs;
    public GameObject rubyPrefabs;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        isOpened = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && GameManager.instance.haveKey)
        {
            animator.SetTrigger("Open");

            isOpened = true;
            
            SpawnObject(lockPrefabs);

            GetTreasure();

            for (int i = 0; i < 10; i++)
            {
                float random = Random.Range(0f, 1.0f);
                if (random < 0.9f) GetItem();
            }

            GameManager.instance.haveKey = false;
        }
    }

    void GetTreasure()
    {
        float random = Random.Range(0f, 1.0f);

        if (random < 0.01f) SpawnObject(diamondPrefabs);
        else if (random < 0.1f) SpawnObject(saphirePrefabs);
        else SpawnObject(rubyPrefabs);
    }

    void GetItem()
    {
        float random = Random.Range(0f, 1.0f);

        if (random < 0.01f)
        {
            // 10 골드
            SpawnObject(goldCoinPrefabs);
        }
        else if (random < 0.02f)
        {
            // 회복약(체력 100% 회복)
            SpawnObject(potionPrefabs);
        }
        else if (random < 0.1f)
        {
            // 하트(체력 +10)
            SpawnObject(heartPrefabs);
        }
        else
        {
            // 1 골드
            SpawnObject(silverCoinPrefabs);
        }

    }

    void SpawnObject(GameObject item)
    {
        Vector3 position = new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, this.transform.position.z);
        Instantiate(item, position, transform.rotation);
    }
}
