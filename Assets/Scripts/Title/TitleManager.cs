using GameManagers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Title
{
    public class TitleManager : MonoBehaviour
    {
        [System.Serializable]
        enum TitleManagerState
        {
            Title,
            Movie1,
            Text1,
            Movie2,
            Text2_1,
            Text2_2,
        }

        TitleManagerState _titleManagerState = TitleManagerState.Title;
        [SerializeField] Menu.MenuInputs _menuInputs;

        [SerializeField] private GameObject _title;
        [SerializeField] private GameObject _movie1;
        [SerializeField] private GameObject _movie2;
        [SerializeField] private GameObject _text;

        private VideoPlayer _videoPlayer1;
        private VideoPlayer _videoPlayer2;

        private OPTextManager _textManager;

        bool _prevPress = false;
        bool _prevHold = false;

        void Awake()
        {
            _videoPlayer1 = _movie1.GetComponent<VideoPlayer>();
            _videoPlayer2 = _movie2.GetComponent<VideoPlayer>();

            _textManager = _text.GetComponent<OPTextManager>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            switch (_titleManagerState)
            {
                case TitleManagerState.Title:
                    break;
                case TitleManagerState.Movie1:
                    if (_menuInputs.hold && !_prevHold)
                    {
                        // LoopPointReached1(_videoPlayer1);
                        JumpToLastFrame(_videoPlayer1);
                    }
                    break;
                case TitleManagerState.Text1:
                    if (_menuInputs.press && !_prevPress && _textManager.IsDoneAddText())
                    {
                        TransitionToMovie2();
                        _titleManagerState = TitleManagerState.Movie2;
                    }
                    break;
                case TitleManagerState.Movie2:
                    if (_menuInputs.hold && !_prevHold)
                    {
                        LoopPointReached2(_videoPlayer2);
                    }
                    break;
                case TitleManagerState.Text2_1:
                    if (_menuInputs.press && !_prevPress && _textManager.IsDoneAddText())
                    {
                        _textManager.NextText();
                        _titleManagerState = TitleManagerState.Text2_2;
                    }
                    break;
                case TitleManagerState.Text2_2:
                    if (_menuInputs.press && !_prevPress && _textManager.IsDoneAddText())
                    {
                        MainGameManager.instance.LoadScene("Main");
                    }
                    break;
            }

            _prevPress = _menuInputs.press;
            _prevHold = _menuInputs.hold;
        }

        void JumpToLastFrame(VideoPlayer videoPlayer)
        {
            if (videoPlayer.frameCount > 0)
            {
                // frameCountはlong型なので、frameプロパティに設定する前にlongからlongへキャストする
                videoPlayer.frame = (long)(videoPlayer.frameCount - 1);
            }
        }

        public void OnStartButton()
        {
            TransitionToMovie1();
            _prevPress = true;
            AudioManager.Instance.PlaySE("Button_SE");
            //タイトルを止める
            AudioManager.Instance.StopBGM();
            //OPを再生
            AudioManager.Instance.PlayBGM("OP_BGM");
        }

        void TransitionToMovie1()
        {
            _title.SetActive(false);
            _movie1.SetActive(true);
            _videoPlayer1.Play();
            _videoPlayer1.loopPointReached += LoopPointReached1;
            _titleManagerState = TitleManagerState.Movie1;
        }

        void TransitionToMovie2()
        {
            _movie1.SetActive(false);
            _text.SetActive(false);
            _movie2.SetActive(true);
            _videoPlayer2.Play();
            _videoPlayer2.loopPointReached += LoopPointReached2;
            _titleManagerState = TitleManagerState.Movie2;
        }
        // ビデオ再生終了時に呼ばれる
        public void LoopPointReached1(VideoPlayer vp)
        {
            // TransitionToMovie2();
            _text.SetActive(true);
            _textManager.StartOPText1();
            _titleManagerState = TitleManagerState.Text1;
        }

        public void LoopPointReached2(VideoPlayer vp)
        {
            // MainGameManager.instance.LoadScene("Main");
            _text.SetActive(true);
            _textManager.StartOPText2();
            _titleManagerState = TitleManagerState.Text2_1;
        }
    }
}
