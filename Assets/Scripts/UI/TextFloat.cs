using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Menu
{
    public class TextButton : MonoBehaviour
    {
        private bool _isSelect;
        public float AMP, SPD;
        Vector3 _defaultPos;
        private TextMeshProUGUI _text;
        private Button _button;

        void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _button = GetComponent<Button>();

            _defaultPos = transform.position;
        }

        void Update()
        {
            if (_isSelect)
            {
                _text.transform.position = _defaultPos + new Vector3(0, AMP * Mathf.Sin(SPD * Time.time), 0);
            }
            else
            {
                _text.transform.position = _defaultPos;
            }
        }

        public void Select()
        {
            _isSelect = true;
        }

        public void Deselect()
        {
            _isSelect = false;
        }

        public void Click()
        {
            _button.onClick.Invoke();
        }
    }
}
