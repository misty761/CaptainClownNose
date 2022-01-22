using UnityEngine;
using UnityEngine.SceneManagement;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    public float JUMP_FORCE = 150f;        // 점프 힘
    public float health;
    public float attackTime;
    public float timeDustAnim;
    public int jumpCount;                  // 누적 점프 횟수
    public int attackPattern;
    public bool isGrounded;                // 바닥에 닿았는지 나타냄
    public bool isDead;                    // 사망 상태
    public bool isFalling;

    public AudioSource playerAudio;        // 사용할 오디오 소스 컴포넌트

    public AudioClip audioDeath;                  // 사망시 재생할 오디오 클립
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamage;

    public Animator animator;              // 사용할 애니메이터 컴포넌트
    public Rigidbody2D playerRigidbody;    // 사용할 리지드바디 컴포넌트

    public GameObject dustRunPrefabs;
    public GameObject dustJumpPrefabs;
    public GameObject dustFallPrefabs;
    public GameObject attackEffectPrefabs1;
    public GameObject attackEffectPrefabs2;
    public GameObject attackEffectPrefabs3;
    public GameObject airAttackEffectPrefabs1;
    public GameObject airAttackEffectPrefabs2;
    public GameObject prefHitEffect;

    // hp bar
    public GameObject prfHpBar;
    public GameObject canvas;
    public float hpBarOffsetY;
    RectTransform hpBar;
    float timeHpBar;
    float healthLast;

    float damage = 10f;                          // 공격시 적에게 주는 기본 데미지
    float OFFSET_DUST_Y = -0.02f;
    float DELAY_DAMAGE = 1f;
    float timeDamage;

    Vector2 attackPoint;
    Vector2 boxSize;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("씬에 두개 이상의 플레이어가 존재합니다!");
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        // 게임 오브젝트로부터 사용할 컴포넌트들을 가져와 변수에 할당
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        Initialize();

        // 공격 범위 초기값
        attackPoint = new Vector2(transform.position.x, transform.position.y);
        boxSize = new Vector2(0, 0);

        // scene이 삭제되더라도 player object 유지
        DontDestroyOnLoad(this.gameObject);

        // hp bar
        hpBar = Instantiate(prfHpBar, canvas.transform).GetComponent<RectTransform>();
    }

    public void Initialize()
    {
        attackPattern = 0;
        attackTime = 0f;
        jumpCount = 0;
        isGrounded = false;
        isDead = false;
        isFalling = false;
        health = 100f;
        timeDustAnim = 0f;
        timeDamage = 0f;
        timeHpBar = 0f;
        healthLast = health;
    }

    private void Update()
    {
        if (isDead)
        {
            hpBar.gameObject.SetActive(false);
            return;
        }

        // 캐릭터 죽음
        if (!isDead && health <= 0) Die();

        timeDamage += Time.deltaTime;

        // 마우스 왼쪽 버튼을 눌렀으며 && 최대 점프 횟수(2)에 도달하지 않았다면
        if (JumpButton.bTouchUp && (JumpButton.isTouching || Input.GetKeyDown(KeyCode.S)) && jumpCount == 0 && !ExitHospital.isPlayerInHospital)
        {
            // 오디오 소스 재생
            if (SoundControl.bSoundOn)
            {
                playerAudio.clip = audioJump;
                playerAudio.Play();
            }

            // 점프 더스트 효과 위치
            Vector3 position = new Vector3(this.transform.position.x,
                                            this.transform.position.y + OFFSET_DUST_Y,
                                            this.transform.position.z);

            // 점프 더스트 효과 추가
            Instantiate(dustJumpPrefabs, position, transform.rotation);

            attackPattern = 0;
            // 점프 횟수 증가
            jumpCount++;
            // 점프 직전에 속도를 순간적으로 제로(0, 0)로 변경
            playerRigidbody.velocity = Vector2.zero;
            // 리지드바디에 위쪽으로 힘 주기
            playerRigidbody.AddForce(new Vector2(0, JUMP_FORCE));

            JumpButton.bTouchUp = false;
        }
        // 마우스 왼쪽 버튼에서 손을 떼는 순간 && 속도의 y 값이 양수라면(위로 상승 중)
        else if (!JumpButton.isTouching && playerRigidbody.velocity.y > 0)
        {
            // 현재 속도를 절반으로 변경(오래 누르고 있으면 높이 점프하도록 구현하기 위해)
            //playerRigidbody.velocity = playerRigidbody.velocity * 0.5f;
        }

        // 공격
        attackTime += Time.deltaTime;
        if ((AttackButton.isTouching || Input.GetKeyDown(KeyCode.A)) && attackTime > 0.3f && !ExitHospital.isPlayerInHospital)
        {
            attackTime = 0f;
            Attack();
        }

        // 5초 동안 공격을 하지 않으면 어택 패턴 초기화
        if (attackTime > 5f) attackPattern = 0;

        // 애니메이터의 Grounded 파라미터를 isGrounded 값으로 갱신
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("Moving", Controller.moveFlag);
        animator.SetBool("Falling", isFalling);

        // Y 방향 속도에 따라 falling 판단
        float velocityPlayerY = playerRigidbody.velocity.y;
        if (!isGrounded)
        {
            if (velocityPlayerY < 0) isFalling = true;
            else isFalling = false;
        }
        else
        {
            isFalling = false;
        }

        // 먼지 효과 표시
        DustEffect();
        timeDustAnim += Time.deltaTime;

        // hp bar
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + hpBarOffsetY, 0));
        hpBar.position = _hpBarPos;
        timeHpBar += Time.deltaTime;
        if (timeHpBar > 1f)
        {
            if (health <= 30f)
            {
                hpBar.gameObject.SetActive(true);
            }
            else if (health != healthLast)
            {
                timeHpBar = 0f;
                hpBar.gameObject.SetActive(true);
            }
            else
            {
                hpBar.gameObject.SetActive(false);
            }
            healthLast = health;
        }
        
    }

    void DustEffect()
    {
        // 달리때
        if (isGrounded && Controller.moveFlag && timeDustAnim > 0.5f)
        {
            // 먼지 효과 생성 위치
            Vector3 position = new Vector3(this.transform.position.x, this.transform.position.y + OFFSET_DUST_Y, this.transform.position.z);

            // 먼지 효과 생성
            GameObject dustRunEffect = Instantiate(dustRunPrefabs, position, transform.rotation);
            // 왼쪽을 보고 있다면
            if (!Controller.playerRight)
            {
                // 방향을 바꿔 줌
                dustRunEffect.transform.localScale = new Vector3(-1, 1, 1);
            }
            // 시간 초기화
            timeDustAnim = 0f;
        }
    }

    void Attack()
    {
        // 공격 사운드 재생
        if (SoundControl.bSoundOn)
        {
            playerAudio.clip = audioAttack;
            playerAudio.Play();
        }

        // 애니매이션 전이 조건 setting
        animator.SetTrigger("Attacking");

        // 공격 범위
        if (isGrounded) // 땅에서 공격 할 때
        {
            // 공격 패턴 3
            if (Controller.isJoysticUp)
            {
                AttackPattern(3);
            }
            // 공격 패턴 2
            else if (Controller.isJoysticDown)
            {
                AttackPattern(2);
            }
            // 공격 패턴 1
            else
            {
                // 다음 공격 패턴
                attackPattern++;

                AttackPattern(attackPattern);
            }

            if (attackPattern >= 3) attackPattern = 0;

        }
        // 공중 공격
        else
        {
            // 다음 공격 패턴
            attackPattern++;

            if (attackPattern == 1)
            {
                animator.SetInteger("AttackPattern", attackPattern);
                boxSize = new Vector2(0.35f, 0.13f);
                if (Controller.playerRight)
                {
                    // 공격 범위
                    attackPoint = new Vector2(transform.position.x + 0.1f, transform.position.y - 0.2f);

                    // 공격 효과
                    Vector2 effectPosition = new Vector2(transform.position.x + 0.1f, transform.position.y - 0.3f);
                    Instantiate(airAttackEffectPrefabs1, effectPosition, transform.rotation);
                }
                else
                {
                    // 공격 범위
                    attackPoint = new Vector2(transform.position.x - 0.1f, transform.position.y - 0.2f);

                    // 공격 효과
                    Vector2 effectPosition = new Vector2(transform.position.x - 0.1f, transform.position.y - 0.3f);
                    GameObject attakEffect = Instantiate(airAttackEffectPrefabs1, effectPosition, transform.rotation);
                    attakEffect.transform.localScale = new Vector3(-1, 1, 1);
                }
            }
            else if (attackPattern == 2)
            {
                animator.SetInteger("AttackPattern", attackPattern);
                boxSize = new Vector2(0.35f, 0.24f);
                if (Controller.playerRight)
                {
                    // 공격 범위
                    attackPoint = new Vector2(transform.position.x + 0.1f, transform.position.y - 0.12f);

                    // 공격 효과
                    Vector2 effectPosition = new Vector2(transform.position.x + 0.2f, transform.position.y - 0.2f);
                    Instantiate(airAttackEffectPrefabs2, effectPosition, transform.rotation);
                }
                else
                {
                    // 공격 범위
                    attackPoint = new Vector2(transform.position.x - 0.1f, transform.position.y - 0.12f);

                    // 공격 효과
                    Vector2 effectPosition = new Vector2(transform.position.x - 0.2f, transform.position.y - 0.2f);
                    GameObject attakEffect = Instantiate(airAttackEffectPrefabs2, effectPosition, transform.rotation);
                    attakEffect.transform.localScale = new Vector3(-1, 1, 1);
                }
            }

            if (attackPattern >= 2) attackPattern = 0;
        }

        // 히트 박스 만들기
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint, boxSize, 0);
        // 히트 박스에서 맞은 적들의 정보 추출 TakeDamage 함수 불러오기
        foreach (Collider2D col in hitEnemies)
        {
            float random = Random.Range(0.5f, 1.5f);

            // 지역 변수 enemy에 닿은 녀석은 Enemy 클래스를 넣음
            Enemy enemy = col.GetComponent<Enemy>();

            if (random > 1.4f && enemy != null) enemy.isCriticalAttacked = true;

            // enemy에 클래서 Enemy가 있는지 검사
            if (enemy && attackTime < 0.1f) enemy.TakeDamage(random * (damage + GameManager.instance.level));

            // barrel과 box 에 데미지를 가함
            Stuff stuff = col.GetComponent<Stuff>();
            if (stuff && attackTime < 0.1f) stuff.TakeDamage(random * (damage + GameManager.instance.level));

            // arrow sign 파괴
            SignArrow signArrow = col.GetComponent<SignArrow>();
            if (signArrow && attackTime < 0.1f) signArrow.DestroyObject();

            // hit 효과 표시
            if (enemy != null)
            {
                //Instantiate(prefHitEffect, col.transform.position, Quaternion.Euler(Vector3.zero));
            } 
        }
    }

    void AttackPattern(int pattern)
    {
        animator.SetInteger("AttackPattern", pattern);

        if (pattern == 1)
        {
            boxSize = new Vector2(0.2f, 0.05f);

            // 오른쪽을 보고 있을 때
            if (Controller.playerRight)
            {
                // 공격 범위
                attackPoint = new Vector2(transform.position.x + 0.22f, transform.position.y - 0.03f);

                // 공격 효과
                Vector2 effectPosition = new Vector2(transform.position.x + 0.4f, transform.position.y);
                Instantiate(attackEffectPrefabs1, effectPosition, transform.rotation);
            }
            else
            {
                // 공격 범위
                attackPoint = new Vector2(transform.position.x - 0.22f, transform.position.y - 0.03f);

                // 공격 효과
                Vector2 effectPosition = new Vector2(transform.position.x - 0.4f, transform.position.y);
                GameObject attakEffect = Instantiate(attackEffectPrefabs1, effectPosition, transform.rotation);
                attakEffect.transform.localScale = new Vector3(-1, 1, 1);
            }

        }
        else if (pattern == 2)
        {
            boxSize = new Vector2(0.15f, 0.2f);

            if (Controller.playerRight)
            {
                // 공격 범위
                attackPoint = new Vector2(transform.position.x + 0.2f, transform.position.y - 0.03f);

                // 공격 효과
                Vector2 effectPosition = new Vector2(transform.position.x + 0.4f, transform.position.y);
                Instantiate(attackEffectPrefabs2, effectPosition, transform.rotation);
            }
            else
            {
                // 공격 범위
                attackPoint = new Vector2(transform.position.x - 0.2f, transform.position.y - 0.03f);

                // 공격 효과
                Vector2 effectPosition = new Vector2(transform.position.x - 0.4f, transform.position.y);
                GameObject attakEffect = Instantiate(attackEffectPrefabs2, effectPosition, transform.rotation);
                attakEffect.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else if (pattern == 3)
        {
            boxSize = new Vector2(0.17f, 0.25f);

            if (Controller.playerRight)
            {
                // 공격 범위
                attackPoint = new Vector2(transform.position.x + 0.2f, transform.position.y + 0.06f);

                // 공격 효과
                Vector2 effectPosition = new Vector2(transform.position.x + 0.45f, transform.position.y + 0.05f);
                Instantiate(attackEffectPrefabs3, effectPosition, transform.rotation);
            }
            else
            {
                // 공격 범위
                attackPoint = new Vector2(transform.position.x - 0.2f, transform.position.y + 0.06f);

                // 공격 효과
                Vector2 effectPosition = new Vector2(transform.position.x - 0.45f, transform.position.y + 0.05f);
                GameObject attakEffect = Instantiate(attackEffectPrefabs3, effectPosition, transform.rotation);
                attakEffect.transform.localScale = new Vector3(-1, 1, 1);
            }
        }

    }

    // 눈에 보이지 않는 박스를 보이게 하는 역할
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(attackPoint, boxSize);
    }

    private void Die()
    {
        // 사망 효과음 재생
        if (SoundControl.bSoundOn)
        {
            // 오디오 소스에 할당된 오디오 클립을 deathClip으로 변경
            playerAudio.clip = audioDeath;
            playerAudio.Play();
        }

        // 애니메이터의 Die 트리거 파라미터를 셋
        animator.SetTrigger("Die");

        // 속도를 제로(0, 0)로 변경
        playerRigidbody.velocity = Vector2.zero;
        // 사망 상태를 true로 변경
        isDead = true;

        GameManager.instance.OnPlayerDead();
    }


    /*  
    private void OnTriggerEnter2D(Collider2D other) 
    {

    }
    */


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 어떤 골라이더와 닿았으며, 충돌 표면이 위쪽을 보고 있으면
        if (collision.contacts[0].normal.y > 0f)
        {
            SetGround();

            Instantiate(dustFallPrefabs, this.transform.position, this.transform.rotation);
        }

        // 적과 충돌시
        if ((collision.gameObject.tag == "Enemy"
             || collision.gameObject.tag == "Boss"
             || collision.gameObject.tag == "Shooter") && !isDead)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            // 캐릭터가 적을 밟음
            if (enemy.health > 0)
            {
                if (collision.contacts[0].normal.y > 0.1f)
                {
                    // 오디오 소스 재생
                    if (SoundControl.bSoundOn)
                    {
                        playerAudio.clip = audioJump;
                        playerAudio.Play();
                    }

                    playerRigidbody.velocity = Vector2.zero;
                    playerRigidbody.AddForce(new Vector2(0, JUMP_FORCE / 1.3f));
                    float random = Random.Range(0.5f, 1.5f);
                    if (random > 1.4f) enemy.isCriticalAttacked = true;
                    enemy.TakeDamage(random * (damage + GameManager.instance.level));
                }
                else
                {
                    if (enemy.power > 0) TakeDamage(enemy.power);
                }
            }

        }

        // 창에 찔렸을 때
        if (collision.gameObject.tag == "Obstacle" && !isDead)
        {
            Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
            if (obstacle.power > 0) TakeDamage(obstacle.power);
        }

    }

    public void TakeDamage(float damage)
    {
        if (timeDamage > DELAY_DAMAGE)
        {
            // 사운드 재생
            if (SoundControl.bSoundOn)
            {
                playerAudio.clip = audioDamage;
                playerAudio.Play();
            }

            timeDamage = 0f;

            // 애니매이션 트리거 세팅
            animator.SetTrigger("Damage");

            // 플레이어를 뛰움
            if (playerRigidbody.velocity.y <= 0f)
            {
                jumpCount++;
                playerRigidbody.velocity = Vector2.zero;
                if (Controller.playerRight) playerRigidbody.AddForce(new Vector2(-JUMP_FORCE / 3f, JUMP_FORCE / 1.3f));
                else playerRigidbody.AddForce(new Vector2(JUMP_FORCE / 3f, JUMP_FORCE / 1.3f));
            }

            // 체력 --
            health -= damage;
            if (health < 0) health = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (playerRigidbody.velocity.y == 0 && collision.contacts[0].normal.y > 0.9f) SetGround();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 어떤 콜라이더에서 떼어진 경우 isGrounded를 false로 변경
        isGrounded = false;
    }

    void SetGround()
    {
        // isGrounded를 true로 변경하고, 누적 점프 횟수를 0으로 리셋
        isGrounded = true;
        jumpCount = 0;
    }
}