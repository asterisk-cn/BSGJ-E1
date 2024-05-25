using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using GameManagers;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager instance;
    public GameState gameState;

    // [SerializeField] private float _mainTime = 60;
    [SerializeField] private float _fightTime = 10;

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
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
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
    }

    /**
     * @brief メインシーンがロードされた時の処理
     */
    void OnRunLoaded()
    {
        gameState = GameState.Main;
    }

    /**
     * @brief ファイトシーンがロードされた時の処理
     */
    void OnFightLoaded()
    {
        gameState = GameState.Fight;
        GameTimeManager.instance.AddListenerOnTimeUp(() => instance.LoadScene("Result"));
        GameTimeManager.instance.StartTimer(_fightTime, true);

        Reset();
    }

    void OnResultLoaded()
    {
        gameState = GameState.Result;
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
    }
}
