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
            Movie2
        }

        TitleManagerState _titleManagerState = TitleManagerState.Title;
        [SerializeField] Menu.MenuInputs _menuInputs;

        [SerializeField] private GameObject _title;
        [SerializeField] private GameObject _movie1;
        [SerializeField] private GameObject _movie2;

        private VideoPlayer _videoPlayer1;
        private VideoPlayer _videoPlayer2;

        bool _prevPress = false;
        bool _prevHold = false;

        void Awake()
        {
            _videoPlayer1 = _movie1.GetComponent<VideoPlayer>();
            _videoPlayer2 = _movie2.GetComponent<VideoPlayer>();

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
                        LoopPointReached1(_videoPlayer1);
                    }
                    break;
                case TitleManagerState.Movie2:
                    if (_menuInputs.hold && !_prevHold)
                    {
                        LoopPointReached2(_videoPlayer2);
                    }
                    break;
            }

            _prevPress = _menuInputs.press;
            _prevHold = _menuInputs.hold;
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
            _movie2.SetActive(true);
            _videoPlayer2.Play();
            _videoPlayer2.loopPointReached += LoopPointReached2;
            _titleManagerState = TitleManagerState.Movie2;
        }
        // ビデオ再生終了時に呼ばれる
        public void LoopPointReached1(VideoPlayer vp)
        {
            TransitionToMovie2();
        }

        public void LoopPointReached2(VideoPlayer vp)
        {
            MainGameManager.instance.LoadScene("Main");
        }
    }
}
