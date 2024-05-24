using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    private TextMeshProUGUI _text;
    [SerializeField] private Enemy.EnemyCore _enemyCore;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = _enemyCore.GetCurrentHealth().ToString();
    }
}
