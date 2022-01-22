using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur : MonoBehaviour
{
    public float attackOffsetX1;
    public float attackOffsetY1;
    public float attackOffsetX2;
    public float attackOffsetY2;
    public float attackOffsetX3;
    public float attackOffsetY3;
    public float attackBoxX1;
    public float attackBoxY1;
    public float attackBoxX2;
    public float attackBoxY2;
    public float attackBoxX3;
    public float attackBoxY3;

    Animator animator;
    AudioSource audioSource;
    Enemy enemy;
    Vector2 attackPoint;
    Vector2 boxSize;
    int attackPattern;
    bool bAttack;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        enemy = GetComponent<Enemy>();
        attackPattern = 0;
        bAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            attackPattern = Random.Range(0, 3);                     // random int : 0~2
            animator.SetInteger("AttackPattern", attackPattern);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PrepareAttack")) bAttack = true;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && bAttack)
        {  
            bAttack = false;
            enemy.timeAttack = 0f;
            if (SoundControl.bSoundOn) audioSource.PlayOneShot(enemy.audioAttack);
            boxSize = new Vector2(attackBoxX1, attackBoxY1);
            if (enemy.enemyRight)
            {
                attackPoint = new Vector2(this.transform.position.x + attackOffsetX1, this.transform.position.y + attackOffsetY1);
            }
            else
            {
                attackPoint = new Vector2(this.transform.position.x - attackOffsetX1, this.transform.position.y + attackOffsetY1);
            }

            Collider2D[] hitPlayer = Physics2D.OverlapBoxAll(attackPoint, boxSize, 0);
            foreach (Collider2D col in hitPlayer)
            {
                PlayerController player = col.GetComponent<PlayerController>();
                if (player && enemy.timeAttack < 0.1f)
                {
                    player.TakeDamage(enemy.power);
                }
            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && bAttack)
        {
            bAttack = false;
            enemy.timeAttack = 0f;
            if (SoundControl.bSoundOn) audioSource.PlayOneShot(enemy.audioAttack);
            boxSize = new Vector2(attackBoxX2, attackBoxY2);
            if (enemy.enemyRight)
            {
                attackPoint = new Vector2(this.transform.position.x + attackOffsetX2, this.transform.position.y + attackOffsetY2);
            }
            else
            {
                attackPoint = new Vector2(this.transform.position.x - attackOffsetX2, this.transform.position.y + attackOffsetY2);
            }

            Collider2D[] hitPlayer = Physics2D.OverlapBoxAll(attackPoint, boxSize, 0);
            foreach (Collider2D col in hitPlayer)
            {
                PlayerController player = col.GetComponent<PlayerController>();
                if (player && enemy.timeAttack < 0.1f)
                {
                    player.TakeDamage(enemy.power);
                }
            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3") && bAttack)
        {
            bAttack = false;
            enemy.timeAttack = 0f;
            if (SoundControl.bSoundOn) audioSource.PlayOneShot(enemy.audioAttack);
            boxSize = new Vector2(attackBoxX3, attackBoxY3);
            if (enemy.enemyRight)
            {
                attackPoint = new Vector2(this.transform.position.x + attackOffsetX3, this.transform.position.y + attackOffsetY3);
            }
            else
            {
                attackPoint = new Vector2(this.transform.position.x - attackOffsetX3, this.transform.position.y + attackOffsetY3);
            }

            Collider2D[] hitPlayer = Physics2D.OverlapBoxAll(attackPoint, boxSize, 0);
            foreach (Collider2D col in hitPlayer)
            {
                PlayerController player = col.GetComponent<PlayerController>();
                if (player && enemy.timeAttack < 0.1f)
                {
                    player.TakeDamage(enemy.power);
                }
            }
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(attackPoint, boxSize);
    }
}
