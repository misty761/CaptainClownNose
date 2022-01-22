using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hospital : MonoBehaviour
{
    public Door door;

    // Update is called once per frame
    void Update()
    {
        if (door.bOpen && !ExitHospital.isPlayerInHospital)
        {
            Time.timeScale = 0;
            SceneManager.LoadScene("Hospital", LoadSceneMode.Additive);
            ExitHospital.isPlayerInHospital = true;
            door.animator.SetTrigger("Close");
            door.bOpen = false;
            //Debug.Log("load hospital");
        }
    }
}
