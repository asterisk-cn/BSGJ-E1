using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    public class GeneratePosition : MonoBehaviour
    {

        public bool isCollide = false;

        List<Collider> _colliders = new List<Collider>();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            _colliders.RemoveAll(collider => collider == null);
            isCollide = _colliders.Count > 0;
        }

        void OnTriggerEnter(Collider other)
        {
            var comp = other.GetComponentInParent<Enemy.EnemyAttack>();
            if (comp)
            {
                isCollide = true;
                _colliders.Add(other);
            }
        }
    }
}
