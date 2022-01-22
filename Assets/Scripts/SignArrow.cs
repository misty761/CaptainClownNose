using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignArrow : MonoBehaviour
{
    public GameObject silverCoin;
    public GameObject goldCoin;
    public GameObject heart;
    public GameObject healthPotion;

    Animator animator;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Destroyed"))
        {
            float random = Random.Range(0f, 1.0f);
            if (random < 0.9f) GetItem();

            Destroy(this.gameObject);
        }
    }

    public void DestroyObject()
    {
        if (SoundControl.bSoundOn) audioSource.Play();

        animator.SetTrigger("Damage");
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
