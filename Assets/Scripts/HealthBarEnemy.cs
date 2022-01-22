using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarEnemy : MonoBehaviour
{
    public Image health;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Image>();
    }

    private void Update()
    {
        if (Time.timeScale == 0)
        {
            Destroy(this.gameObject);
        }
    }

}
