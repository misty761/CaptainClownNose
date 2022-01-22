using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public float timeAnim = 0.5f;
    float timeDestroy;

    // Start is called before the first frame update
    void Start()
    {
        timeDestroy = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeDestroy += Time.deltaTime;

        if (timeDestroy >= timeAnim)
        {
            Destroy(gameObject);
        }
    }
}
