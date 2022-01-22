using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public PigWithMatch pigWithMatch;
    public GameObject bomb;

    Animator animator;
    AudioSource audioSource;
    PlayerController player;
    bool bFire;
    float OFFSET_SPAWN_BOMB_X = 0.1f;
    float OFFSET_SPAWN_BOMB_Y = 0f;
    Vector3 positionFire;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<PlayerController>();

        bFire = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pigWithMatch == null) return;

        // 돼지가 불을 붙이면 대포 발사
        if(!bFire && pigWithMatch.animator.GetCurrentAnimatorStateInfo(0).IsName("MatchOff"))
        {
            if (SoundControl.bSoundOn) audioSource.Play();
            animator.SetTrigger("Fire");

            Vector3 playerPosition = player.transform.position;
            Vector3 thisPosition = this.transform.position;
            float distanceX = playerPosition.x - thisPosition.x;
            //float distanceY = playerPosition.y - thisPosition.y;

            // 플레이어가 왼쪽에 있으면
            if (distanceX < 0)
            {
                positionFire = new Vector2(this.transform.position.x - OFFSET_SPAWN_BOMB_X,
                                            this.transform.position.y + OFFSET_SPAWN_BOMB_Y);
            }
            // 플레이어가 오른쪽에 있으면
            else
            {
                positionFire = new Vector2(this.transform.position.x + OFFSET_SPAWN_BOMB_X,
                                            this.transform.position.y + OFFSET_SPAWN_BOMB_Y);
            }

            // 폭탄 생성
            Instantiate(bomb, positionFire, Quaternion.Euler(0, 0, 0));

            bFire = true;
        }

        // 발사 초기화
        if (pigWithMatch.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) bFire = false;
    }
}
