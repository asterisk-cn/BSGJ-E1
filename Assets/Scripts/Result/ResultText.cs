using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultText : MonoBehaviour
{
    private TextMeshProUGUI _text;

    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (MainGameManager.instance.isClear)
        {
            _text.text = "Clear!";
        }
        else
        {
            _text.text = "Game Over";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
