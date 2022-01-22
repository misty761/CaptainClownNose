using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmTree : MonoBehaviour
{
    public GameObject ground;

    private void Update()
    {
        if (Controller.isJoysticDown)
        {
            ground.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other.gameObject.transform.position.y > this.gameObject.transform.position.y)
        {
            ground.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ground.SetActive(false);
        }
    }
}
