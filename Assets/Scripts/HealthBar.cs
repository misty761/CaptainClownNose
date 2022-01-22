using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Image health;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        health.fillAmount = PlayerController.instance.health / 100f;
    }
}
