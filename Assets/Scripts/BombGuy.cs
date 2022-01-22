using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGuy : MonoBehaviour
{
    public GameObject bomb;

    Enemy enemy;

    PlayerController player;
    float OFFSET_SPAWN_BOMB_X = 0.1f;
    float OFFSET_SPAWN_BOMB_Y = 0f;
    float DISTANCE_FIRE_X = 2f;
    float DISTANCE_FIRE_Y = 1f;
    float INTERVAL_FIRE = 5f;
    float timeFire;
    float random;
    bool bFire;
    Vector3 positionFire;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        enemy = GetComponent<Enemy>();
        bFire = false;
        timeFire = 0f;
        random = Random.Range(0.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.health <= 0) return;

        timeFire += Time.deltaTime;
        Vector3 playerPosition = player.transform.position;
        Vector3 thisPosition = this.transform.position;
        float distanceX = playerPosition.x - thisPosition.x;
        float distanceY = playerPosition.y - thisPosition.y;

        // timeFire * random 시간이 지난 후에
        if (timeFire > INTERVAL_FIRE * random)
        {
            // 플레이어가 가까이 있을 때
            if (distanceX > - DISTANCE_FIRE_X && distanceX < DISTANCE_FIRE_X)
            {
                if (distanceY > - DISTANCE_FIRE_Y && distanceY < DISTANCE_FIRE_Y)
                {
                    random = Random.Range(0.5f, 1.5f);
                    timeFire = 0;
                    bFire = true;
                }
            }
            
        }

        // 폭탄을 던짐
        if (bFire)
        {
            bFire = false;
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
        }      
    }
}
