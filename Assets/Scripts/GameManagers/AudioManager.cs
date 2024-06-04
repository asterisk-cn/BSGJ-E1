using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameManagers
{
    public class AudioManager : MonoBehaviour
    {
        private const float BGM_VOLUME_DEFAULT = 0.2f;
        private const float SE_VOLUME_DEFAULT = 0.2f;
        public float BGMVolume { get; private set; } = BGM_VOLUME_DEFAULT;
        public float SEVolume { get; private set; } = SE_VOLUME_DEFAULT;

        private const string BGM_PATH = "Audio/BGM";
        private const string SE_PATH = "Audio/SE";

        public const float BGM_FADE_SPEED_RATE_HIGH = 0.9f;
        public const float BGM_FADE_SPEED_RATE_LOW = 0.3f;
        private float _bgmFadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH;

        private string _nextBGMName;
        private string _nextSEName;

        private bool _isFadingOut = false;

        private AudioSource _bgmSource;
        private List<AudioSource> _seSourceList;
        private const int SE_SOURCE_NUM = 10;

        private Dictionary<string, AudioClip> _bgmDictionary, _seDictionary;

        public static AudioManager Instance { get; private set; } = null;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // gameObject.AddComponent<AudioListener>();
            for (int i = 0; i < SE_SOURCE_NUM + 1; i++)
            {
                gameObject.AddComponent<AudioSource>();
            }

            AudioSource[] audioSourceArray = GetComponents<AudioSource>();
            _seSourceList = new List<AudioSource>();

            for (int i = 0; i < audioSourceArray.Length; i++)
            {
                audioSourceArray[i].playOnAwake = false;

                if (i == 0)
                {
                    audioSourceArray[i].loop = true;
                    _bgmSource = audioSourceArray[i];
                    _bgmSource.volume = BGMVolume;
                }
                else
                {
                    _seSourceList.Add(audioSourceArray[i]);
                    audioSourceArray[i].volume = SEVolume;
                }

            }

            _bgmDictionary = new Dictionary<string, AudioClip>();
            _seDictionary = new Dictionary<string, AudioClip>();

            object[] bgmList = Resources.LoadAll(BGM_PATH);
            object[] seList = Resources.LoadAll(SE_PATH);

            foreach (AudioClip bgm in bgmList)
            {
                _bgmDictionary[bgm.name] = bgm;
            }
            foreach (AudioClip se in seList)
            {
                _seDictionary[se.name] = se;
            }

        }

        //=================================================================================
        //SE
        //=================================================================================

        public void PlaySE(string seName, float delay = 0.0f)
        {
            if (!_seDictionary.ContainsKey(seName))
            {
                Debug.Log("There is no SE with the name " + seName);
                return;
            }

            _nextSEName = seName;
            Invoke("DelayPlaySE", delay);
        }

        private void DelayPlaySE()
        {
            foreach (AudioSource seSource in _seSourceList)
            {
                if (!seSource.isPlaying)
                {
                    seSource.PlayOneShot(_seDictionary[_nextSEName] as AudioClip);
                    return;
                }
            }
        }

        //=================================================================================
        //BGM
        //=================================================================================

        public void PlayBGM(string bgmName, float fadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH)
        {
            if (!_bgmDictionary.ContainsKey(bgmName))
            {
                Debug.Log("There is no BGM with the name " + bgmName);
                return;
            }

            if (!_bgmSource.isPlaying)
            {
                _nextBGMName = "";
                _bgmSource.clip = _bgmDictionary[bgmName] as AudioClip;
                _bgmSource.Play();
            }
            else if (_bgmSource.clip.name != bgmName)
            {
                _nextBGMName = bgmName;
                FadeOutBGM(fadeSpeedRate);
            }
        }

        public void StopBGM()
        {
            _bgmSource.Stop();
        }

        public void FadeOutBGM(float fadeSpeedRate = BGM_FADE_SPEED_RATE_LOW)
        {
            _bgmFadeSpeedRate = fadeSpeedRate;
            _isFadingOut = true;
        }

        private void Update()
        {
            if (!_isFadingOut)
            {
                return;
            }

            _bgmSource.volume -= Time.deltaTime * _bgmFadeSpeedRate;
            if (_bgmSource.volume <= 0)
            {
                _bgmSource.Stop();
                _bgmSource.volume = BGMVolume;
                _isFadingOut = false;

                if (!string.IsNullOrEmpty(_nextBGMName))
                {
                    PlayBGM(_nextBGMName);
                }
            }

        }

        //=================================================================================
        // Settings
        //=================================================================================

        public void SetBGMVolume(float bgmVolume)
        {
            if (!_isFadingOut)
            {
                _bgmSource.volume = bgmVolume;
            }
            BGMVolume = bgmVolume;
        }

        public void SetSEVolume(float seVolume)
        {
            foreach (AudioSource seSource in _seSourceList)
            {
                seSource.volume = seVolume;
            }
            SEVolume = seVolume;
        }

    }
}
