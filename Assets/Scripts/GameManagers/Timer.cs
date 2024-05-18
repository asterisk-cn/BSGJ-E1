using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameManagers
{
    public class GameTimeManager : MonoBehaviour
    {
        public static GameTimeManager instance;
        public UnityEvent _onTimeUp = new UnityEvent();
        float _startTime;
        float _time;
        float _duration;
        bool _isInterrupted = false;

        public bool IsWorking { get { return _duration > 0; } }


        private void Awake()
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

        }

        // Update is called once per frame
        void Update()
        {
            if (_isInterrupted)
            {
                _startTime += Time.deltaTime;
                return;
            }

            if (IsWorking)
            {
                _time = Time.time - _startTime;

                if (_time >= _duration)
                {
                    Debug.Log(_onTimeUp);
                    _onTimeUp?.Invoke();
                    ResetTimer();
                }
            }
        }

        public bool StartTimer(float duration, bool force = false)
        {
            if (IsWorking && !force)
            {
                return false;
            }

            _startTime = Time.time;
            _duration = duration;
            return true;
        }

        public void StopTimer()
        {
            _isInterrupted = true;
        }

        public void ResumeTimer()
        {
            _isInterrupted = false;
        }

        void ResetTimer()
        {
            _time = 0;
            _duration = 0;
            _startTime = 0;
            _isInterrupted = false;
            _onTimeUp = new UnityEvent();
        }

        /**
         * @brief タイマーが終了したときに実行する処理を登録する
         * @param action 実行する処理
         */
        public void AddListenerOnTimeUp(UnityAction action)
        {
            _onTimeUp.AddListener(action);
        }

        /**
         * @brief 経過時間を取得する
         * @return 経過時間
        */
        public float GetTime()
        {
            return IsWorking ? _time : 0f;
        }

        /**
         * @brief 経過時間を秒単位で取得する
         * @return 経過時間(秒)
        */
        public int GetTimeSeconds()
        {
            return IsWorking ? Mathf.FloorToInt(_time) : 0;
        }

        /**
         * @brief 残り時間を取得する
         * @return 残り時間
        */
        public float GetTimeLeft()
        {
            return IsWorking ? _duration - _time : 0f;
        }

        /**
         * @brief 残り時間を秒単位で取得する
         * @return 残り時間(秒)
        */
        public int GetTimeSecondsLeft()
        {
            return IsWorking ? Mathf.CeilToInt(_duration - _time) : 0;
        }
    }
}
