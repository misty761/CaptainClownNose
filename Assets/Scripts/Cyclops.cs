using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyclops : MonoBehaviour
{
    public GameObject laser;
    public AudioClip audioLaser;

    Enemy enemy;
    Animator animator;
    Collider2D[] hitPlayer;
    AudioSource audioSource;
    Vector2 attackPoint;
    Vector2 boxSize;
    float originalSpeed;
    float timeAttack;
    
    bool bShoot;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        originalSpeed = enemy.speed;
        bShoot = false;
        timeAttack = 0f;
        boxSize = new Vector2(enemy.attackBoxX, enemy.attackBoxY);
        hitPlayer = Physics2D.OverlapBoxAll(attackPoint, boxSize, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.health <= 0f) return;

        timeAttack += Time.deltaTime;

        // 레이저 발사 시 정지
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PrepareShooting")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Shooting"))
        {
            enemy.speed = 0;

            if (!bShoot && animator.GetCurrentAnimatorStateInfo(0).IsName("Shooting"))
            {
                if (!enemy.enemyRight) laser.transform.localScale = new Vector3(-1, 1, 1);
                else laser.transform.localScale = new Vector3(1, 1, 1);
                Instantiate(laser, this.transform.position, Quaternion.Euler(0, 0, 0));
                
                timeAttack = 0f;

                if (enemy.enemyRight)
                {
                    attackPoint = new Vector2(this.transform.position.x + enemy.attackOffsetX, this.transform.position.y + enemy.attackOffsetY);
                }
                else
                {
                    attackPoint = new Vector2(this.transform.position.x - enemy.attackOffsetX, this.transform.position.y + enemy.attackOffsetY);
                }

                hitPlayer = Physics2D.OverlapBoxAll(attackPoint, boxSize, 0);

                if (SoundControl.bSoundOn)
                {
                    audioSource.PlayOneShot(audioLaser);
                }

                bShoot = true;

            }

            try
            {
                foreach (Collider2D col in hitPlayer)
                {
                    PlayerController player = col.GetComponent<PlayerController>();
                    if (player && timeAttack > 0.3f && timeAttack < 0.8f)
                    {
                        player.TakeDamage(enemy.power + 10);
                    }
                }
            }
            catch
            {
                Debug.Log(hitPlayer);
            }
            
        }
        else
        {
            enemy.speed = originalSpeed;
            bShoot = false;
        }

        
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(attackPoint, boxSize);
    }
}
