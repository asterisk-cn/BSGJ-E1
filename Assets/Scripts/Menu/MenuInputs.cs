using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Menu
{
    public class MenuInputs : MonoBehaviour
    {
        [HideInInspector] public Vector2 navigate;
        public bool press;

        [SerializeField] private bool useJoycon = true;

        void FixedUpdate()
        {
            press = false;

            if (useJoycon)
            {
                UpdateJoyconInputs();
            }
        }

        private List<Joycon> _joycons;
        private float _minDeadZone = 0.125f;
        private float _maxDeadZone = 0.925f;

        void Start()
        {
            _joycons = JoyconManager.Instance.j;
        }

        void OnNavigate(InputValue value)
        {
            var axis = value.Get<Vector2>();
            navigate = axis;
        }

        void OnSubmit(InputValue value)
        {
            press = value.isPressed;
        }

        void UpdateJoyconInputs()
        {
            for (int i = 0; i < _joycons.Count; i++)
            {
                Joycon joycon = _joycons[i];

                if (joycon.isLeft)
                {
                    Vector2 _tmp = new Vector2(joycon.GetStick()[0], joycon.GetStick()[1]);
                    if (_tmp.magnitude < _minDeadZone)
                    {
                        _tmp = Vector2.zero;
                    }
                    else if (_tmp.magnitude > _maxDeadZone)
                    {
                        _tmp.Normalize();
                    }

                    navigate = _tmp;
                }
            }
        }
    }
}
