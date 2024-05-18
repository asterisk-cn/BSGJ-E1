using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using GameManagers;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager instance;
    public GameState gameState;

    [SerializeField] private float _mainTime = 60;
    [SerializeField] private float _fightTime = 10;

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
        if (scene.name == "Main")
        {
            OnRunLoaded();
        }

        if (scene.name == "Fight")
        {
            OnFightLoaded();
        }
    }

    /**
     * @brief メインシーンがロードされた時の処理
     */
    void OnRunLoaded()
    {
        gameState = GameState.Main;
        GameTimeManager.instance.AddListenerOnTimeUp(() => SceneFadeManager.instance.FadeOut(GameState.Fight));
        GameTimeManager.instance.StartTimer(_mainTime, true);
    }

    /**
     * @brief ファイトシーンがロードされた時の処理
     */
    void OnFightLoaded()
    {
        gameState = GameState.Fight;
        GameTimeManager.instance.StartTimer(_fightTime, true);
    }
}
