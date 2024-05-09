using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
    public bool isFadeIn;
    public bool isFadeOut;
    public bool isFading;

    [SerializeField]
    private Image fadeImage;

    private float alpha = 0;

    [SerializeField]
    private float fadeTime;

    public static SceneFadeManager instance;

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

    void Fade()
    {
        if (isFadeIn)
        {
            alpha -= fadeTime * Time.deltaTime;
            Alpha();
            if (alpha <= 0)
            {
                alpha = 0;
                isFadeIn = false;
            }
        }

        if (isFadeOut)
        {
            isFading = true;
            alpha += fadeTime * Time.deltaTime;
            Alpha();
            if (alpha >= 1)
            {
                alpha = 1;
                isFadeOut = false;
                isFading = false;
            }
        }
    }

    void Alpha()
    {
        fadeImage.color = new Color(0, 0, 0, alpha);
    }
}
