using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
    public bool isFadeIn;
    public bool isFadeOut;

    [SerializeField]
    private Image fadeImage;

    private float alpha = 0;

    [SerializeField]
    private float fadeTime;

    public static SceneFadeManager instance;

    private string afterScene;

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

    /**
     * @brief �t�F�[�h�A�E�g���āA���̃V�[���ɑJ�ڂ���
     * @param nextScene �J�ڐ�̃V�[����
     */
    public void FadeOut(GameManagers.GameState nextScene)
    {
        // �t�F�[�h�A�E�g�̃t���O���グ��
        isFadeOut = true;

        // �J�ڐ�̃V�[������Enum���當����ɕϊ�
        afterScene = nextScene.ToString();
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
            alpha += fadeTime * Time.deltaTime;
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
