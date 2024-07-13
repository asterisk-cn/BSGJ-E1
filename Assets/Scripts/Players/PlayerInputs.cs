using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.InputSystem.Haptics;

namespace Players
{
    public class PlayerInputs : MonoBehaviour
    {
        public Vector3 leftMoveStick;
        public Vector3 rightMoveStick;
        public bool leftAttack;
        public bool rightAttack;

        public float leftAttackValue;
        public float rightAttackValue;

        [SerializeField] private bool useJoycon = false;
        public bool UseJoycon => useJoycon;

        PlayerInput _playerInput;

        void FixedUpdate()
        {
            if (_joycons.Count == 0)
            {
                useJoycon = false;
            }
            else
            {
                useJoycon = true;
            }

            if (useJoycon)
            {
                UpdateJoyconInputs();
                if (leftAccelaration.magnitude > 2f)
                {
                    leftAttack = true;
                    leftAttackValue = leftAccelaration.magnitude;
                }
                if (rightAccelaration.magnitude > 2f)
                {
                    rightAttack = true;
                    rightAttackValue = rightAccelaration.magnitude;
                }
            }
        }

        void LateUpdate()
        {
            leftAttack = false;
            rightAttack = false;

            leftAttackValue = 0;
            rightAttackValue = 0;
        }

        public Vector3 leftAccelaration;
        public Vector3 rightAccelaration;

        private List<Joycon> _joycons;
        private float _minDeadZone = 0.125f;
        private float _maxDeadZone = 0.925f;

        void Start()
        {
            _playerInput = GetComponent<PlayerInput>();

            _joycons = JoyconManager.Instance.j;
        }

        void OnLeftMove(InputValue value)
        {
            var axis = value.Get<Vector2>();

            leftMoveStick = new Vector3(axis.x, 0, axis.y);
        }

        void OnRightMove(InputValue value)
        {
            var axis = value.Get<Vector2>();

            rightMoveStick = new Vector3(axis.x, 0, axis.y);
        }

        void OnFireLeft(InputValue value)
        {
            //デバック用のダメージ(クリック)
            leftAttack = value.isPressed;
            leftAttackValue = 10.0f;
        }

        void OnFireRight(InputValue value)
        {
            rightAttack = value.isPressed;
        }

        void UpdateJoyconInputs()
        {
            for (int i = 0; i < _joycons.Count; i++)
            {
                Joycon joycon = _joycons[i];

                Vector2 _tmp = new Vector2(joycon.GetStick()[0], joycon.GetStick()[1]);
                if (_tmp.magnitude < _minDeadZone)
                {
                    _tmp = Vector2.zero;
                }
                else if (_tmp.magnitude > _maxDeadZone)
                {
                    _tmp.Normalize();
                }

                if (joycon.isLeft)
                {
                    leftMoveStick = new Vector3(_tmp.x, 0, _tmp.y);
                    leftAccelaration = joycon.GetAccel();
                }
                else
                {
                    rightMoveStick = new Vector3(_tmp.x, 0, _tmp.y);
                    rightAccelaration = joycon.GetAccel();
                }
            }
        }

        public void RumbleLeft(float lowFreq, float highFreq, float amp, float time)
        {
            foreach (var joycon in _joycons)
            {
                if (joycon.isLeft)
                {
                    joycon.SetRumble(lowFreq, highFreq, amp, (int)time);
                    StartCoroutine(StopJoyconRumble(time, joycon));
                }
            }

            StartCoroutine(RumbleLeft(amp, time));
        }

        public void RumbleRight(float lowFreq, float highFreq, float amp, float time)
        {
            foreach (var joycon in _joycons)
            {
                if (!joycon.isLeft)
                {
                    joycon.SetRumble(lowFreq, highFreq, amp, (int)time);
                    StartCoroutine(StopJoyconRumble(time, joycon));
                }
            }

            StartCoroutine(RumbleRight(amp, time));
        }

        private IEnumerator RumbleLeft(float amp, float time)
        {
            if (_playerInput.devices.FirstOrDefault(x => x is IDualMotorRumble) is not IDualMotorRumble gamepad)
            {
                yield break;
            }

            // 振動
            gamepad.SetMotorSpeeds(amp, 0.0f);
            yield return new WaitForSecondsRealtime(time);

            gamepad.SetMotorSpeeds(0.0f, 0.0f);
        }

        private IEnumerator RumbleRight(float amp, float time)
        {
            if (_playerInput.devices.FirstOrDefault(x => x is IDualMotorRumble) is not IDualMotorRumble gamepad)
            {
                yield break;
            }

            // 振動
            gamepad.SetMotorSpeeds(0.0f, amp);
            yield return new WaitForSecondsRealtime(time);

            gamepad.SetMotorSpeeds(0.0f, 0.0f);
        }

        private IEnumerator StopJoyconRumble(float time, Joycon joycon)
        {
            yield return new WaitForSecondsRealtime(time);
            joycon.SetRumble(0, 0, 0, 0);
        }
    }
}
