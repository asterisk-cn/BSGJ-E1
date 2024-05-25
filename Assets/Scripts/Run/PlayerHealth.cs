using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    private TextMeshProUGUI _text;
    [SerializeField] private Players.PlayerCore _playerCore;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = _playerCore.GetCurrentHealth().ToString();
    }
}
