using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Run
{
    public class UnionCount : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentUnionCount;
        [SerializeField] private TextMeshProUGUI _targetUnionCount;
        [SerializeField] private Players.PlayerCore _playerCore;
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            _currentUnionCount.text = _playerCore.GetCurrentUnionCount().ToString();
            _targetUnionCount.text = _playerCore.GetTargetUnionCount().ToString();
        }
    }
}
