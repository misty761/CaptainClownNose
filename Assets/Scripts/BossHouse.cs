using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHouse : MonoBehaviour
{
    public Door door;
    public string sceneName;

    public bool isEntered;

    // Start is called before the first frame update
    void Start()
    {
        isEntered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (door.bOpen)
        {
            Time.timeScale = 0;
            if (!isEntered)
            {
                isEntered = true;
                EnterBossStage();
            }
        }
    }

    private void EnterBossStage()
    {
        SceneManager.LoadScene(sceneName);
    }
}
