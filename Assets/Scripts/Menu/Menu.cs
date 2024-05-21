using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField] List<Button> _buttons = new List<Button>();
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
            if (_menuInputs.navigate.y > 0)
            {
                if (_prevStickState == StickState.None || _prevStickState == StickState.Down)
                {
                    _selectedButtonIndex = (_selectedButtonIndex - 1 + _buttons.Count) % _buttons.Count;
                    _buttons[_selectedButtonIndex].Select();
                    _prevStickState = StickState.Up;
                }
            }
            else if (_menuInputs.navigate.y < 0)
            {
                if (_prevStickState == StickState.None || _prevStickState == StickState.Up)
                {
                    _selectedButtonIndex = (_selectedButtonIndex + 1) % _buttons.Count;
                    _buttons[_selectedButtonIndex].Select();
                    _prevStickState = StickState.Down;
                }
            }
            else
            {
                _prevStickState = StickState.None;
            }
        }
    }
}
