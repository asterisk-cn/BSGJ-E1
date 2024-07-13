using GameManagers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace MidMovie
{
    public class MidMovieManager : MonoBehaviour
    {
        [System.Serializable]
        enum MidMovieManagerState
        {
            Movie
        }

        MidMovieManagerState _midMovieManagerState = MidMovieManagerState.Movie;
        [SerializeField] Menu.MenuInputs _menuInputs;

        [SerializeField] private GameObject _movie;

        private VideoPlayer _videoPlayer;

        bool _prevPress = false;
        bool _prevHold = false;

        void Awake()
        {
            _videoPlayer = _movie.GetComponent<VideoPlayer>();

        }

        // Start is called before the first frame update
        void Start()
        {
            _videoPlayer.targetTexture.Release();
            _movie.SetActive(true);
            _videoPlayer.Play();
            _videoPlayer.loopPointReached += LoopPointReached;
        }

        // Update is called once per frame
        void Update()
        {
            switch (_midMovieManagerState)
            {
                case MidMovieManagerState.Movie:
                    if (_menuInputs.hold && !_prevHold)
                    {
                        LoopPointReached(_videoPlayer);
                    }
                    break;
            }

            _prevHold = _menuInputs.hold;
        }

        // ビデオ再生終了時に呼ばれる
        public void LoopPointReached(VideoPlayer vp)
        {
            MainGameManager.instance.LoadScene("Fight");
        }
    }
}
