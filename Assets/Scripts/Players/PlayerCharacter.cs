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

        private PlayerCore _core;

        private CharacterController _characterController;

        [SerializeField]
        [Tooltip("加速度")] private float acceleration;

        [SerializeField]
        [Tooltip("減速度")] private float deceleration;

        [SerializeField]
        [Tooltip("最大速度")] private float maxSpeed;

        private Vector3 velocity = Vector3.zero;

        void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _core = GetComponentInParent<PlayerCore>();

            _currentParameters = _defaultParameters;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TakeDamage(int damage)
        {
            _core.TakeDamage(damage);
        }

        public void Move(Vector3 direction)
        {
            if(direction.magnitude > 0)
            {
                velocity += direction * acceleration * Time.deltaTime;
                if(velocity.magnitude > maxSpeed)
                {
                    velocity = velocity.normalized * maxSpeed;
                }
            }
            else
            {
                if(velocity.magnitude > 0)
                {
                    velocity -= velocity.normalized * deceleration * Time.deltaTime;
                    if(velocity.magnitude < 0.1f) { velocity = Vector3.zero; }
                }
            }

            velocity.y = velocity.y + (Physics.gravity.y * Time.deltaTime);
            _characterController.Move(velocity*_currentParameters.moveSpeed);
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

        public void UnitePartial(PlayerPartial playerPartial)
        {
            _core.UnitePartial();
        }
    }
}

