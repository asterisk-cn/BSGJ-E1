using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
   public bool isFadeIn { private set;  get; }
   public bool isFadeOut { private set; get; }

    [SerializeField]
    private Image fadeImage;

    private float alpha = 0;

    [SerializeField]
    private float fadeTime;

    public static SceneFadeManager instance;

    private string afterScene;

    public bool isFade { private set; get; }

    

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
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += FadeIn;
        Alpha();
    }

    // Update is called once per frame
    void Update()
    {
        Fade();
    }

    private void FadeIn(Scene scene, LoadSceneMode mode)
    {
        isFadeIn = true;
    }

    /**
     * @brief フェードアウトして、次のシーンに遷移する
     * @param nextScene 遷移先のシーン名
     */
    public void FadeOut(string nextScene)
    {
        // フェードアウトのフラグを上げる
        isFadeOut = true;
        isFade = true;
        afterScene = nextScene;
    }


    void Fade()
    {
        if (isFadeIn)
        {
            alpha -= fadeTime * Time.unscaledDeltaTime;
            Alpha();
            if (alpha <= 0)
            {
                isFade = false;
                Time.timeScale = 1;
                alpha = 0;
                isFadeIn = false;
            }
        }

        if (isFadeOut)
        {
            Time.timeScale = 0;
            alpha += fadeTime * Time.unscaledDeltaTime;
            Alpha();
            if (alpha >= 1)
            {
                alpha = 1;
                isFadeOut = false;
                SceneManager.LoadScene(afterScene);
            }
        }
    }

    void Alpha()
    {
        fadeImage.color = new Color(0, 0, 0, alpha);
    }
}
