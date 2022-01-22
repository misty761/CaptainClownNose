using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

// 게임 오버 상태를 표현하고, 게임 점수와 UI를 관리하는 게임 매니저
// 씬에는 단 하나의 게임 매니저만 존재할 수 있다.
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤을 할당할 전역 변수
    public Text scoreText;              // 점수를 출력할 UI 텍스트
    public Text bestScoreText;          // 최고 점수를 출력할 UI 텍스트
    public Text moneyText;
    public Text levelText;
    public GameObject gameoverUI;       // 게임 오버시 활성화 할 UI 게임 오브젝트
    public GameObject textLevelUp;
    public GameObject exitGameMenu;
    public GameObject goToSelectStageMenu;
    public AudioClip audioLevelUp;
    public AudioClip audioFanfare;
    public AudioClip audioClick;

    public int score;                   // 게임 점수
    public int money;
    public int level;
    public int scoreLevelUp;
    public bool haveKey;
    public bool isGameover;             // 게임 오버 상태

    AudioSource audioSource;

    // 게임 시작과 동시에 싱글톤을 구성
    void Awake()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        // 싱글톤 변수 instance가 비어있는가?
        if (instance == null)
        {
            // instance가 비어있다면(null) 그곳에 자기 자신을 할당
            instance = this;
        }
        else
        {
            // instance에 이미 다른 GameManager 오브젝트가 할당되어 있는 경우
            // 씬에 두개 이상의 GameManager 오브젝트가 존재한다는 의미.
            // 싱글톤 오브젝트는 하나만 존재해야 하므로 자신의 게임 오브젝트를 파괴
            Debug.LogWarning("씬에 두개 이상의 게임 매니저가 존재합니다!");
            Destroy(this.gameObject);
        }

    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        level = 1;
        scoreLevelUp = 15;

        Initilize();

        DontDestroyOnLoad(this.gameObject);
    }

    public void Initilize()
    {
        score = 0;
        haveKey = false;
        money = 0;
        isGameover = false; 
    }

    void Update()
    {
        try
        {
            if (isGameover)
            {
                // 게임 오버 UI 표시
                gameoverUI.SetActive(true);
            }
            else gameoverUI.SetActive(false);
        }
        catch
        {
            Debug.Log("game over UI error");
        }

        // H 키를 누르면(개발용)
        if (Input.GetKeyDown(KeyCode.H))
        {
            // 플레이어 만피
            PlayerController.instance.health = 100f;
        }

        /*
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Home))
            {
                //home button
            }
            else if (Input.GetKey(KeyCode.Escape))
            {
                //back button
            }
            else if (Input.GetKey(KeyCode.Menu))
            {
                //menu button
            }
        }
        */

        // back 버튼 처리
        if (Input.GetKey(KeyCode.Escape))
        {
            // select stage scene
            if (SceneManager.GetActiveScene().name != "SelectStage")
            {
                // select stage scene 으로 이동할지 물어 봄
                Time.timeScale = 0;
                goToSelectStageMenu.SetActive(true);
            }
           
        }

    }

    public void LogInPlayGames()
    {
        //이미 인증된 사용자는 바로 로그인 성공됩니다.
        if (Social.localUser.authenticated)
        {
            Debug.Log(Social.localUser.userName);
        }
        else
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log(Social.localUser.userName);
                }
                else
                {
                    Debug.Log("Login Fail");
                }
            });
    }

    /*
    public void GetAchievement()
    {
        LogInPlayGames();
        
        if (score >= 10)
        {
            Social.ReportProgress(GPGSIds.achievement_trainee, 100f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Get Achievement");
                }
                else
                {
                    Debug.Log("Fail to get Achievement");
                }
            });
        }

        if (score >= 20)
        {
            Social.ReportProgress(GPGSIds.achievement_beginner, 100f, (bool success) =>
            {
                if (success)
                {
                    //Debug.Log("Get Achievement");
                }
                else
                {
                    //Debug.Log("Fail to get Achievement");
                }
            });
        }

        if (score >= 30)
        {
            Social.ReportProgress(GPGSIds.achievement_practiced, 100f, (bool success) =>
            {
                if (success)
                {
                    //Debug.Log("Get Achievement");
                }
                else
                {
                    //Debug.Log("Fail to get Achievement");
                }
            });
        }

        if (score >= 40)
        {
            Social.ReportProgress(GPGSIds.achievement_expert, 100f, (bool success) =>
            {
                if (success)
                {
                    //Debug.Log("Get Achievement");
                }
                else
                {
                    //Debug.Log("Fail to get Achievement");
                }
            });
        }

        if (score >= 50)
        {
            Social.ReportProgress(GPGSIds.achievement_master, 100f, (bool success) =>
            {
                if (success)
                {
                    //Debug.Log("Get Achievement");
                }
                else
                {
                    //Debug.Log("Fail to get Achievement");
                }
            });
        }
    }
    */


    // 점수를 증가시키는 메서드
    public void AddScore(int newScore)
    {
        // 게임오버가 아니라면
        if (!isGameover)
        {
            // 점수를 증가
            score += newScore;
            scoreText.text = "Score : " + score;

            if (PlayerController.instance.health > 0) LevelUp(score);
        }
    }

    void LevelUp(int currentScore)
    {
        if (currentScore >= scoreLevelUp)
        {
            if (SoundControl.bSoundOn)
            {
                audioSource.PlayOneShot(audioLevelUp);
            }

            scoreLevelUp = scoreLevelUp * 2;

            level++;

            levelText.text = "Lv." + level;

            Vector3 position = new Vector3(PlayerController.instance.transform.position.x,
                                            PlayerController.instance.transform.position.y + 0.5f,
                                            PlayerController.instance.transform.position.z);

            Instantiate(textLevelUp, position, PlayerController.instance.transform.rotation);
        }
    }

    // 머니를 증가 시키는 메서드
    public void AddMoney(int newMoney)
    {
        if (!isGameover)
        {
            money += newMoney;
            moneyText.text = "" + money;
        }
    }

    // 머니를 사용하는 메서드
    public void SpendMoney(int newMoney)
    {
        if (!isGameover)
        {
            money -= newMoney;
            moneyText.text = "" + money;
        }
    }

    // 플레이어 캐릭터가 사망시 게임 오버를 실행하는 메서드
    public void OnPlayerDead()
    {
        AddScore(money);

        // 최고 점수 저장
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (score > bestScore)
        {
            if (SoundControl.bSoundOn)
            {
                audioSource.PlayOneShot(audioFanfare);
            }

            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
        }
        bestScoreText.text = "Best score : " + bestScore;

        isGameover = true;
    }

    public void MoveToOtherScene(GameObject obj, string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.MoveGameObjectToScene(obj, scene);
    }

    public void PlayClickSound()
    {
        if (SoundControl.bSoundOn) audioSource.PlayOneShot(audioClick);
    }

    // 케릭터 레벨을 제외하고 모든 값 초기화
    public void NewGame()
    {
        PlayerController.instance.animator.SetTrigger("Revive");
        PlayerController.instance.Initialize();
        Controller.playerRight = true;
        Initilize();
        AddMoney(0);
        AddScore(0);
        GoogleMobileAdsReward.instance.MyLoadAD();
    }

    public void NewLevel()
    {
        level = 1;
        levelText.text = "Lv." + level;
        scoreLevelUp = 15;
    }

}