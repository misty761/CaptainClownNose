using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPosition : MonoBehaviour
{
    public float playerPositionX;
    public float playerPositionY;

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.instance.playerRigidbody.position = new Vector2(playerPositionX, playerPositionY);

        // 마자막으로 갔었던 스테이지 저장
        string sceneName = SceneManager.GetActiveScene().name;
        int lastStage = PlayerPrefs.GetInt("LastStage", 1);
        int currentStage = 1;
        if (sceneName == "Scene02") currentStage = 2;
        else if (sceneName == "Scene03") currentStage = 3;
        else if (sceneName == "Scene04") currentStage = 4;
        else if (sceneName == "Scene05") currentStage = 5;
        else if (sceneName == "Scene06") currentStage = 6;
        else if (sceneName == "Scene07") currentStage = 7;
        else if (sceneName == "Scene08") currentStage = 8;
        if (currentStage > lastStage) PlayerPrefs.SetInt("LastStage", currentStage);

        Time.timeScale = 1;
    }
}
