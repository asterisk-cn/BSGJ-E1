using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFlash : MonoBehaviour
{
    public float SPD;
    TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _text.color = new Color(1, 1, 1, Mathf.Abs(Mathf.Sin(SPD * Time.time)));
    }
}
