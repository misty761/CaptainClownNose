using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float health;
    public int power;
    public float jumpForce;
    public float jumpInterval;
    public float attackInterval;
    public float attackOffsetX;
    public float attackOffsetY;
    public float attackEffectOffsetX;
    public float attackEffectOffsetY;
    public float attackBoxX;
    public float attackBoxY;
    public float speed;
    public float movingDistance;
    public float dustOffsetX;
    public float dustOffsetY;
    public bool spriteLookRight;
    public GameObject silverCoin;
    public GameObject goldCoin;
    public GameObject heart;
    public GameObject healthPotion;
    public GameObject dustRunPrefabs;
    public GameObject dustFallPrefabs;
    public GameObject attackEffect;
    public GameObject keyPrefabs;
    public GameObject part1;
    public GameObject part2;
    public GameObject part3;
    public GameObject part4;
    public AudioClip audioAttack;
    public Rigidbody2D myRigidbody;

    public float randomDistance;
    public float timeAttack;
    public bool isMoving;
    public bool enemyRight;
    public bool isGrounded;
    public bool isDead;
    public bool isAttacking;
    public int point;
    public Vector2 originalPosition;

    // hp bar
    public GameObject prfHpBar;
    UIManager UI;
    public float hpBarOffsetY;
    public Image nowHpbar;
    public RectTransform hpBar;
    float maxHp;

    // text damage;
    public GameObject prfTextDamage;
    public GameObject prfTextDamageCritical;
    public bool isCriticalAttacked;
    float OFFSET_TEXT_DAMAGE_Y = 0.2f;
    RectTransform damageText;

    float timeDustAnim;
    float timeJump;
    float timePrepareAttack;
    float timeDie;

    float randomJump;
    float randomAttack;
    float DISTANCE_ACTION_X = 3.5f;
    float DISTANCE_ACTION_Y = 1.5f;
    float DISTANCE_HP_BAR_X = 1f;
    float DISTANCE_HP_BAR_Y = 1f;

    bool isFalling;
    bool bAttack;

    Animator animator;
    AudioSource audioSource;
    PlayerController player;

    Vector2 posDust;
    Vector2 attackPoint;
    Vector2 boxSize;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<PlayerController>();
        UI = FindObjectOfType<UIManager>();

        timeDustAnim = 0f;
        timeJump = 0f;
        timePrepareAttack = 0f;
        timeAttack = 0f;
        timeDie = 0f;
        enemyRight = false;
        isGrounded = false;
        isMoving = false;
        isFalling = true;
        isAttacking = false;
        isDead = false;
        isCriticalAttacked = false;
        bAttack = false;
        maxHp = health;
        attackPoint = Vector2.zero;
        boxSize = new Vector2(attackBoxX, attackBoxY);
        randomDistance = Random.Range(0.5f, 1.5f);
        randomJump = Random.Range(0.5f, 1.5f);
        randomAttack = Random.Range(0.5f, 1.5f);
        originalPosition = this.transform.position;
        point = ((int)health + power) / 10;
    }

    // Update is called once per frame
    void Update()
    {
        // 게임 오버
        if (GameManager.instance.isGameover)
        {
            if (hpBar != null) Destroy(hpBar.gameObject);
            Destroy(this.gameObject);
            return;
        }

        // 적이 죽음
        if (health <= 0 && !isDead)
        {
            animator.SetTrigger("Die");
            isDead = true;
            timeDie = 0f;
        }
        if (isDead)
        {
            if (this.tag == "Boss")
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dead")) Die();
            }
            else
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") && timeDie > 0.1f) Die();
            }
            
        }

        // 시간 계산
        timeDustAnim += Time.deltaTime;
        timeJump += Time.deltaTime;
        timePrepareAttack += Time.deltaTime;
        timeAttack += Time.deltaTime;
        timeDie += Time.deltaTime;

        // falling 판단
        if (myRigidbody.velocity.y < 0) isFalling = true;
        else isFalling = false;

        Vector3 playerPosition = player.transform.position;
        Vector3 thisPosition = this.transform.position;
        float distanceX = playerPosition.x - thisPosition.x;
        float distanceY = playerPosition.y - thisPosition.y;

        // 플레이어 근처에 있을 때
        if (distanceX > -DISTANCE_ACTION_X && distanceX < DISTANCE_ACTION_X
            && distanceY > -DISTANCE_ACTION_Y && distanceY < DISTANCE_ACTION_Y)
        {
            if (movingDistance > 0 && speed > 0)
            {
                // 체력이 있고 땅에 있을 때만 이동
                if (health > 0 && isGrounded && !isAttacking && !animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
                {
                    if (this.transform.position.x > originalPosition.x - movingDistance * randomDistance && !enemyRight)
                    {
                        this.transform.Translate(Vector3.left * Time.deltaTime * speed);
                        if (speed > 0) isMoving = true;
                    }
                    else if (this.transform.position.x < originalPosition.x + movingDistance * randomDistance && !enemyRight)
                    {
                        enemyRight = true;
                        randomDistance = Random.Range(0.5f, 1.5f);
                    }
                    else if (this.transform.position.x < originalPosition.x + movingDistance * randomDistance && enemyRight)
                    {
                        this.transform.Translate(Vector3.right * Time.deltaTime * speed);
                        if (speed > 0) isMoving = true;
                    }
                    else if (this.transform.position.x > originalPosition.x + movingDistance * randomDistance && enemyRight)
                    {
                        enemyRight = false;
                        randomDistance = Random.Range(0.5f, 1.5f);
                    }
                }
                else isMoving = false;
            }

            // 공격 준비
            if (attackInterval > 0)
            {
                if (timePrepareAttack > attackInterval * randomAttack && isGrounded && !isAttacking)
                {
                    timePrepareAttack = 0;
                    randomAttack = Random.Range(0.5f, 1.5f);
                    LookPlayer();
                    animator.SetTrigger("PrepareAttack");
                    bAttack = true;
                }
            }

            // 점프
            if (health > 0 && jumpInterval > 0)
            {
                if (timeJump > jumpInterval * randomJump && isGrounded && !isAttacking)
                {
                    LookPlayer();
                    timeJump = 0;
                    randomJump = Random.Range(0.5f, 1.5f);
                    float forceX = jumpForce / 2.5f * Random.Range(0.5f, 1f);
                    if (!enemyRight) forceX = -forceX;
                    if (myRigidbody.velocity.y <= 0f)
                    {
                        myRigidbody.velocity = Vector2.zero;
                        myRigidbody.AddForce(new Vector2(forceX, jumpForce * Random.Range(0.5f, 1f)));
                    }
                    else
                    {
                        //myRigidbody.velocity = Vector2.zero;
                        //myRigidbody.AddForce(new Vector2(forceX, 0));
                    }
                }
            }
        }
        
        // 공격
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && bAttack)
        {
            if (SoundControl.bSoundOn) audioSource.PlayOneShot(audioAttack);

            bAttack = false;
            timeAttack = 0f;
            if (enemyRight)
            {
                Vector2 vector = new Vector2(this.transform.position.x + attackEffectOffsetX,
                                            this.transform.position.y + attackEffectOffsetY);
                GameObject effect = Instantiate(attackEffect, vector, this.transform.rotation);
                effect.transform.localScale = new Vector3(-1, 1, 1);
                attackPoint = new Vector2(this.transform.position.x + attackOffsetX, this.transform.position.y + attackOffsetY);
            }
            else
            {
                Vector2 vector = new Vector2(this.transform.position.x - attackEffectOffsetX,
                                            this.transform.position.y + attackEffectOffsetY);
                Instantiate(attackEffect, vector, this.transform.rotation);
                attackPoint = new Vector2(this.transform.position.x - attackOffsetX, this.transform.position.y + attackOffsetY);
            }

            Collider2D[] hitPlayer = Physics2D.OverlapBoxAll(attackPoint, boxSize, 0);
            foreach (Collider2D col in hitPlayer)
            {
                PlayerController player = col.GetComponent<PlayerController>();
                if (player && timeAttack < 0.1f)
                {
                    player.TakeDamage(power);
                }
            }
        }

        // 공격중인지 판단
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PrepareAttack")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1")    // 미노타우르스의 경우 공격 패턴이 3개임
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3")) isAttacking = true;
        else isAttacking = false;

        // 애니메이터
        animator.SetBool("Moving", isMoving);
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("Falling", isFalling);

        // 먼지 효과 표시
        DustEffect();

        // 방향에 따라 스프라이트 회전
        if (!spriteLookRight)
        {
            if (enemyRight) this.transform.localScale = new Vector3(-1, 1, 1);
            else this.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            if (enemyRight) this.transform.localScale = new Vector3(1, 1, 1);
            else this.transform.localScale = new Vector3(-1, 1, 1);
        }

        // hpBar(플레이어 근처에서만 표시)
        if (distanceX > -DISTANCE_HP_BAR_X && distanceX < DISTANCE_HP_BAR_X
            && distanceY > -DISTANCE_HP_BAR_Y && distanceY < DISTANCE_HP_BAR_Y)
        {
            // hpBar 생성
            if (hpBar == null)
            {
                hpBar = Instantiate(prfHpBar, UI.transform).GetComponent<RectTransform>();
                nowHpbar = hpBar.transform.GetChild(1).GetComponent<Image>();
                UpdateHpBar();
            }
            // hpBar update
            else
            {
                if (health >= 0)
                {
                    UpdateHpBar();
                }
                else health = 0f;
            }
        }
        // hpBar(플레이어와 멀어지면 삭제)
        else
        {
            if (hpBar != null) Destroy(hpBar.gameObject);
        }

        // hpBar 삭제
        if (hpBar != null)
        {
            if (Time.timeScale == 0) Destroy(hpBar.gameObject);
        }

    }

    void UpdateHpBar()
    {
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + hpBarOffsetY, 0));
        hpBar.position = _hpBarPos;
        nowHpbar.fillAmount = health / maxHp;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(attackPoint, boxSize);
    }

    void LookPlayer()
    {
        // 플레이어가 왼쪽 적이 오른쪽
        if (player.transform.position.x - this.transform.position.x < 0)
        {
            enemyRight = false;
        }
        else
        {
            enemyRight = true;
        }
    }

    void DustEffect()
    {
        // 달리때
        if (isGrounded && isMoving && timeDustAnim > 0.5f)
        {
            if (!enemyRight)
            {
                posDust = new Vector2(this.transform.position.x + dustOffsetX, this.transform.position.y + dustOffsetY);
            }
            else
            {
                posDust = new Vector2(this.transform.position.x - dustOffsetX, this.transform.position.y + dustOffsetY);
            }
            // 먼지 효과 생성
            GameObject dustRunEffect = Instantiate(dustRunPrefabs, posDust, this.transform.rotation);
            // 왼쪽을 보고 있다면
            if (!enemyRight)
            {
                // 방향을 바꿔 줌
                dustRunEffect.transform.localScale = new Vector3(-1, 1, 1);
            }
            // 시간 초기화
            timeDustAnim = 0f;
        }
    }

    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            //if (SoundControl.bSoundOn) audioSource.PlayOneShot(audioPain);

            animator.SetTrigger("Damage");
            int forceX = 53;
            int forceY = 115;
            if (myRigidbody.velocity.y <= 0f)
            {
                myRigidbody.velocity = Vector2.zero;
                if (Controller.playerRight) myRigidbody.AddForce(new Vector2(forceX, forceY));
                else myRigidbody.AddForce(new Vector2(-forceX, forceY));
            }
            else
            {
                myRigidbody.velocity = Vector2.zero;
                if (Controller.playerRight) myRigidbody.AddForce(new Vector2(forceX, 0));
                else myRigidbody.AddForce(new Vector2(-forceX, 0));
            }

            health -= damage;

            // text damage
            if (isCriticalAttacked)
            {
                damageText = Instantiate(prfTextDamageCritical, UI.transform).GetComponent<RectTransform>();
            }
            else
            {
                damageText = Instantiate(prfTextDamage, UI.transform).GetComponent<RectTransform>();
            }
            isCriticalAttacked = false;
            Vector3 _pos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + OFFSET_TEXT_DAMAGE_Y, 0));
            damageText.transform.position = _pos;
            damageText.gameObject.transform.GetComponent<Text>().text = "" + (int) damage;
            
        }
    }

    void Die()
    {
        if (gameObject.tag == "Shooter")
        {
            Instantiate(part1, this.transform.position, this.transform.rotation);
            Instantiate(part2, this.transform.position, this.transform.rotation);
            Instantiate(part3, this.transform.position, this.transform.rotation);
            Instantiate(part4, this.transform.position, this.transform.rotation);
        }

        GameManager.instance.AddScore(point);

        float random = Random.Range(0f, 1.0f);
        if (random < 0.9f) GetItem();

        // 보스는 죽을 때 키를 준다.
        if (gameObject.tag == "Boss")
        {
            Instantiate(keyPrefabs, this.transform.position, this.transform.rotation);
        }

        if (hpBar != null) Destroy(hpBar.gameObject);
        Destroy(gameObject);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 어떤 골라이더와 닿았으며, 충돌 표면이 위쪽을 보고 있으면
        if (collision.contacts[0].normal.y > 0.9f)
        {
            posDust = new Vector2(this.transform.position.x, this.transform.position.y + dustOffsetY);

            Instantiate(dustFallPrefabs, posDust, this.transform.rotation);

            isGrounded = true;
        }

        // 플레이어와 충돌시
        if (collision.gameObject.tag == "Player" && health > 0)
        {
            LookPlayer();
            // 공격 애니메이션 재생
            animator.SetTrigger("Attack");
        }
        // 방향 전환
        else if (speed > 0) enemyRight = !enemyRight;

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.9f)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 어떤 콜라이더에서 떼어진 경우 isGrounded를 false로 변경
        isGrounded = false;
    }
}
