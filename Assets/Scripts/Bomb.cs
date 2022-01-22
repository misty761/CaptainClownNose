using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject boomEffect;
    public float FORCE_SHOOTING_X = 7500f;
    public float FORCE_SHOOTING_Y = 11500f;

    PlayerController player;
    Rigidbody2D thisRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        thisRigidbody = GetComponent<Rigidbody2D>();


        Vector3 playerPosition = player.transform.position;
        Vector3 thisPosition = this.transform.position;
        float distanceX = playerPosition.x - thisPosition.x;
        //float distanceY = playerPosition.y - thisPosition.y;

        // 플레이어가 왼쪽에 있으면
        if (distanceX < 0)
        {
            thisRigidbody.AddForce(new Vector2(-FORCE_SHOOTING_X, FORCE_SHOOTING_Y));
        }
        // 플레이어가 오른쪽에 있으면
        else
        {
            thisRigidbody.AddForce(new Vector2(FORCE_SHOOTING_X, FORCE_SHOOTING_Y));
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag != "Enemy" && collision.transform.tag != "Cannon")
        {
            Instantiate(boomEffect, this.transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(this.gameObject);
        }
    }
}
