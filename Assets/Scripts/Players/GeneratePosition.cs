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
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Enemy.EnemyAttack enemyAttack))
            {
                isCollide = true;
                _colliders.Add(other);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Enemy.EnemyAttack enemyAttack))
            {
                isCollide = false;
                _colliders.Remove(other);
            }
        }
    }
}
