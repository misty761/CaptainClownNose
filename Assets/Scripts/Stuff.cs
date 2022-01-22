using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stuff : MonoBehaviour
{
    public float jumpForce;
    public float dustOffsetY;
    public float health;
    public GameObject silverCoin;
    public GameObject goldCoin;
    public GameObject heart;
    public GameObject healthPotion;
    public GameObject partPrefabs1;
    public GameObject partPrefabs2;
    public GameObject partPrefabs3;
    public GameObject partPrefabs4;
    public GameObject dustFallPrefabs;

    Animator animator;
    Rigidbody2D myRigidbody;
    AudioSource audioSource;
    bool isDestroyed;
    int point;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        isDestroyed = false;
        point = ((int) health) / 10;
    }

    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            myRigidbody.velocity = Vector2.zero;
            if (Controller.playerRight) myRigidbody.AddForce(new Vector2(jumpForce / 3f, jumpForce / 1.3f));
            else myRigidbody.AddForce(new Vector2(-jumpForce / 3f, jumpForce / 1.3f));
            health -= damage;
            if (health <= 0)
            {
                animator.SetTrigger("Damage");
                isDestroyed = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.contacts[0].normal.y > 0.7f)
        {
            if (SoundControl.bSoundOn) audioSource.Play();
            Vector2 pos = new Vector2(this.transform.position.x, this.transform.position.y + dustOffsetY);
            GameObject dustFall = Instantiate(dustFallPrefabs, pos, this.transform.rotation);
        }

        if (isDestroyed)
        {
            animator.SetTrigger("Destroy");

            //GameManager.instance.AddScore(point);

            float random = Random.Range(0f, 1.0f);

            if (random < 0.9f) GetItem();

            float dis = 0.05f;

            Vector2 position1 = new Vector2(this.transform.position.x - dis, this.transform.position.y + dis);
            Vector2 position2 = new Vector2(this.transform.position.x + dis, this.transform.position.y + dis);
            Vector2 position3 = new Vector2(this.transform.position.x - dis, this.transform.position.y - dis);
            Vector2 position4 = new Vector2(this.transform.position.x + dis, this.transform.position.y - dis);

            Instantiate(partPrefabs1, position1, this.transform.rotation);
            Instantiate(partPrefabs2, position2, this.transform.rotation);
            Instantiate(partPrefabs3, position3, this.transform.rotation);
            Instantiate(partPrefabs4, position4, this.transform.rotation);

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
        GameObject spawnItem = Instantiate(item, transform.position, Quaternion.Euler(0,0,0));
    }

}
