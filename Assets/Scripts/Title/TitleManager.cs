using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title
{
    public class TitleManager : MonoBehaviour
    {
        [System.Serializable]
        enum TitleManagerState
        {
            Title,
            Image1,
            Image2,
            Movie
        }

        TitleManagerState _titleManagerState = TitleManagerState.Title;
        [SerializeField] Menu.MenuInputs _menuInputs;

        [SerializeField] private GameObject _title;
        [SerializeField] private GameObject _image1;
        [SerializeField] private GameObject _image2;
        [SerializeField] private GameObject _movie;

        bool _prevPress = false;

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
                case TitleManagerState.Image1:
                    if (_menuInputs.press && !_prevPress)
                    {
                        TransitionToImage2();
                    }
                    break;
                case TitleManagerState.Image2:
                    if (_menuInputs.press && !_prevPress)
                    {
                        TransitionToMovie();
                    }
                    break;
                case TitleManagerState.Movie:
                    MainGameManager.instance.LoadScene("Main");
                    break;
            }

            _prevPress = _menuInputs.press;
        }

        public void OnStartButton()
        {
            TransitionToImage1();
            _prevPress = true;
        }

        void TransitionToImage1()
        {
            _title.SetActive(false);
            _image1.SetActive(true);
            _titleManagerState = TitleManagerState.Image1;
        }

        void TransitionToImage2()
        {
            _image1.SetActive(false);
            _image2.SetActive(true);
            _titleManagerState = TitleManagerState.Image2;
        }

        void TransitionToMovie()
        {
            _image2.SetActive(false);
            _movie.SetActive(true);
            _titleManagerState = TitleManagerState.Movie;
        }
    }
}
