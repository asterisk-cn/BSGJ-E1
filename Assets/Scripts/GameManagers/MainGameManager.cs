using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using GameManagers;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager instance;
    public GameState gameState;

    [SerializeField] private float _mainTime = 180;
    [SerializeField] private float _fightTime = 10;

    [Header("スコアランクの閾値")]

    [SerializeField][Tooltip("Sランクの閾値")] private float _scoreRankS = 50;
    [SerializeField][Tooltip("Aランクの閾値")] private float _scoreRankA = 70;
    [SerializeField][Tooltip("Bランクの閾値")] private float _scoreRankB = 90;

    private float _score = 0;
    private float _clearTime = 0;
    private int _soulDeadCount = 0;
    private string _scoreRank;

    public bool isClear = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        Cursor.visible = false;
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        PlayBGM(gameState);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.Main)
        {

        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Title")
        {
            OnTitleLoaded();
        }

        if (scene.name == "Main")
        {
            OnRunLoaded();
        }

        if (scene.name == "Fight")
        {
            OnFightLoaded();
        }

        if (scene.name == "Result")
        {
            OnResultLoaded();
        }
    }

    void OnTitleLoaded()
    {
        gameState = GameState.Title;
        PlayBGM(gameState);
    }

    /**
     * @brief メインシーンがロードされた時の処理
     */
    void OnRunLoaded()
    {
        gameState = GameState.Main;
        GameTimeManager.instance.AddListenerOnTimeUp(() => ForceEnd());
        GameTimeManager.instance.StartTimer(_mainTime, true);
        PlayBGM(gameState);
        Reset();
    }

    void ForceEnd()
    {
        instance.LoadScene("Result");
    }

    /**
     * @brief ファイトシーンがロードされた時の処理
     */
    void OnFightLoaded()
    {
        gameState = GameState.Fight;
        GameTimeManager.instance.AddListenerOnTimeUp(() => instance.LoadScene("Result"));
        GameTimeManager.instance.StartTimer(_fightTime, true);
        PlayBGM(gameState);
    }

    void OnResultLoaded()
    {
        gameState = GameState.Result;
        PlayBGM(gameState);
    }

    public void LoadScene(string sceneName, bool isFade = true)
    {
        if (isFade)
        {
            SceneFadeManager.instance.FadeOut(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    void Reset()
    {
        isClear = false;
        _score = 0;
        _clearTime = 0;
        _soulDeadCount = 0;
        _scoreRank = "";
    }

    void PlayBGM(GameState _state)
    {
        switch (_state)
        {
            case GameState.Title:
                AudioManager.Instance.PlayBGM("Title_BGM");
                break;

            case GameState.Main:
                AudioManager.Instance.PlayBGM("Main_BGM");
                break;

            case GameState.Fight:
                AudioManager.Instance.PlayBGM("Fight_BGM");
                break;

            case GameState.Result:
                AudioManager.Instance.StopBGM();
                if(!isClear) AudioManager.Instance.PlayBGM("GameOver_BGM");
                if (isClear) AudioManager.Instance.PlaySE("GameClear_Jingle");
                break;
        }
    }

    public void SetScore(float clearTime, int soulDeadCount)
    {
        _clearTime = clearTime;
        _soulDeadCount = soulDeadCount;
        _score = clearTime + soulDeadCount * 5;

        if (_score <= _scoreRankS)
        {
            _scoreRank = "S";
        }
        else if (_score <= _scoreRankA)
        {
            _scoreRank = "A";
        }
        else if (_score <= _scoreRankB)
        {
            _scoreRank = "B";
        }
        else
        {
            _scoreRank = "C";
        }
    }

    public string GetScoreRank()
    {
        return _scoreRank;
    }

    public float GetClearTime()
    {
        return _clearTime;
    }

    public int GetSoulDeadCount()
    {
        return _soulDeadCount;
    }


}
