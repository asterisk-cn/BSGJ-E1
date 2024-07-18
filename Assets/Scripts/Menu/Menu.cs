using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    public class MenuSelector : MonoBehaviour
    {
        enum StickState
        {
            Up,
            Down,
            None
        }

        [SerializeField] List<TextButton> _buttons = new List<TextButton>();
        [SerializeField] MenuInputs _menuInputs;
        StickState _prevStickState = StickState.None;

        int _selectedButtonIndex = 0;

        // Start is called before the first frame update
        void Start()
        {
            _buttons[_selectedButtonIndex].Select();
        }

        // Update is called once per frame
        void Update()
        {
            if (_menuInputs.navigate.x < 0)
            {
                if (_prevStickState == StickState.None || _prevStickState == StickState.Down)
                {
                    _selectedButtonIndex = Mathf.Max(0, _selectedButtonIndex - 1);
                    _buttons[_selectedButtonIndex].Select();
                    _prevStickState = StickState.Up;
                }
            }
            else if (_menuInputs.navigate.x > 0)
            {
                if (_prevStickState == StickState.None || _prevStickState == StickState.Up)
                {
                    _selectedButtonIndex = Mathf.Min(_buttons.Count - 1, _selectedButtonIndex + 1);
                    _buttons[_selectedButtonIndex].Select();
                    _prevStickState = StickState.Down;
                }
            }
            else
            {
                _prevStickState = StickState.None;
            }

            if (_menuInputs.press)
            {
                _buttons[_selectedButtonIndex].Click();
            }

            UpdateButtonState();
        }

        void UpdateButtonState()
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                if (i == _selectedButtonIndex)
                {
                    _buttons[i].Select();
                }
                else
                {
                    _buttons[i].Deselect();
                }
            }
        }
    }
}
