using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float positionMinX;
    public float positionMaxX;
    public float positionMinY;
    public float positionMaxY;
    public float power = 10f;

    float speed = 0.5f;
    bool isRight;
    bool isUp;

    // Start is called before the first frame update
    void Start()
    {
        isRight = true;
        isUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (positionMaxX - positionMinX != 0)
        {
            if (this.transform.position.x < positionMaxX && isRight)
            {
                this.transform.Translate(Vector3.right * Time.deltaTime * speed);
            }
            else if (this.transform.position.x > positionMaxX && isRight)
            {
                isRight = false;
            }
            else if (this.transform.position.x > positionMinX && !isRight)
            {
                this.transform.Translate(Vector3.left * Time.deltaTime * speed);
            }
            else if (this.transform.position.x < positionMinX && !isRight)
            {
                isRight = true;
            }
        }

        if (positionMaxY - positionMinY != 0)
        {
            if (this.transform.position.y < positionMaxY && isUp)
            {
                this.transform.Translate(Vector3.up * Time.deltaTime * speed);
            }
            else if (this.transform.position.y > positionMaxY && isUp)
            {
                isUp = false;
            }
            else if (this.transform.position.y > positionMinY && !isUp)
            {
                this.transform.Translate(Vector3.down * Time.deltaTime * speed);
            }
            else if (this.transform.position.y < positionMinY && !isUp)
            {
                isUp = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            PlayerController.instance.TakeDamage(power);
        }
    }
}
