using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyText : MonoBehaviour
{
    float presentTime;

    // Start is called before the first frame update
    void Start()
    {
        presentTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        presentTime += Time.deltaTime;

        if (presentTime > 2f) Destroy(this.gameObject);
    }
}
