using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;


namespace Players
{
    [System.Serializable]
    public class CharacterParameters
    {
        public float moveSpeed;
        public int health;
    }

    public class PlayerCharacter : MonoBehaviour
    {
        bool isAlive;

        [SerializeField] private CharacterParameters _defaultParameters;
        private CharacterParameters _currentParameters;

        //追加 05/21
        private CharacterController _characterController;

        // Start is called before the first frame update
        void Start()
        {
            _currentParameters = _defaultParameters;
            _characterController = gameObject.AddComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TakeDamage(int damage)
        {


        }

        public void Move(Vector3 direction)
        {
            _characterController.Move(direction*_currentParameters.moveSpeed);
        }

        public void ScaleAroundFoot(float newScale)
        {
            // 足元を軸に拡大縮小
            Vector3 targetPos = transform.localPosition;
            Vector3 diff = new Vector3(0, 1.0f * transform.localScale.y, 0);
            Vector3 pivot = targetPos - diff;
            float relativeScale = newScale / gameObject.transform.localScale.x;

            Vector3 resultPos = pivot + diff * relativeScale;
            gameObject.transform.localScale = new Vector3(newScale, newScale, newScale);
            gameObject.transform.localPosition = resultPos;
        }

        public void Die()
        {

        }
    }
}

