using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public GameObject smallerRock;

    Enemy enemy;
    Rigidbody2D myRigidbody;
    bool bDestroy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();

        myRigidbody = GetComponent<Rigidbody2D>();
        if (Controller.playerRight) myRigidbody.AddForce(new Vector3(enemy.jumpForce / 3f, enemy.jumpForce / 1.3f));
        else myRigidbody.AddForce(new Vector3(-enemy.jumpForce / 3f, enemy.jumpForce / 1.3f));
        enemy.enemyRight = !enemy.enemyRight;

        bDestroy = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.health <= 0 && !bDestroy)
        {
            Instantiate(smallerRock, this.transform.position, this.transform.rotation);
            Instantiate(smallerRock, this.transform.position, this.transform.rotation);
            bDestroy = true;
        }
    }
}
