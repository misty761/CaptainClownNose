using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStage : MonoBehaviour
{
    public GameObject UI;
    public GameObject exitGameMenu;
    public GameObject stageNotCleared2;
    public GameObject stageNotCleared3;
    public GameObject stageNotCleared4;
    public GameObject stageNotCleared5;
    public GameObject stageNotCleared6;
    public GameObject stageNotCleared7;
    public GameObject stageNotCleared8;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;

        UIManager.instance.jumpButton.gameObject.SetActive(false);
        UIManager.instance.attackButton.gameObject.SetActive(false);
        UIManager.instance.touchController.gameObject.SetActive(false);
        UIManager.instance.healthBar.gameObject.SetActive(false);
        UIManager.instance.score.gameObject.SetActive(false);
        UIManager.instance.money.gameObject.SetActive(false);
        UIManager.instance.playerLevel.gameObject.SetActive(false);
        UIManager.instance.gotoSelectStageMenu.gameObject.SetActive(false);

        int lastStage = PlayerPrefs.GetInt("LastStage", 1);
        if (lastStage == 2)
        {
            stageNotCleared2.SetActive(false);
        }
        else if (lastStage == 3)
        {
            stageNotCleared2.SetActive(false);
            stageNotCleared3.SetActive(false);
        }
        else if (lastStage == 4)
        {
            stageNotCleared2.SetActive(false);
            stageNotCleared3.SetActive(false);
            stageNotCleared4.SetActive(false);
        }
        else if (lastStage == 5)
        {
            stageNotCleared2.SetActive(false);
            stageNotCleared3.SetActive(false);
            stageNotCleared4.SetActive(false);
            stageNotCleared5.SetActive(false);
        }
        else if (lastStage == 6)
        {
            stageNotCleared2.SetActive(false);
            stageNotCleared3.SetActive(false);
            stageNotCleared4.SetActive(false);
            stageNotCleared5.SetActive(false);
            stageNotCleared6.SetActive(false);
        }
        else if (lastStage == 7)
        {
            stageNotCleared2.SetActive(false);
            stageNotCleared3.SetActive(false);
            stageNotCleared4.SetActive(false);
            stageNotCleared5.SetActive(false);
            stageNotCleared6.SetActive(false);
            stageNotCleared7.SetActive(false);
        }
        else if (lastStage == 8)
        {
            stageNotCleared2.SetActive(false);
            stageNotCleared3.SetActive(false);
            stageNotCleared4.SetActive(false);
            stageNotCleared5.SetActive(false);
            stageNotCleared6.SetActive(false);
            stageNotCleared7.SetActive(false);
            stageNotCleared8.SetActive(false);
        }

        GameManager.instance.NewGame();
        GameManager.instance.NewLevel();
    }

    private void Update()
    {
        // back 버튼 처리
        if (Input.GetKey(KeyCode.Escape))
        {
            exitGameMenu.SetActive(true);
        }
    }
}
