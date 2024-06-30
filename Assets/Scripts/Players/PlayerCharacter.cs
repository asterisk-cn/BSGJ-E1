using GameManagers;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Windows;


namespace Players
{
    [System.Serializable]
    public class CharacterParameters
    {
        // [SerializeField] [Tooltip("移動速度")] public float moveSpeed;
        [SerializeField][Tooltip("最大速度")] public float maxSpeed;
        [SerializeField][Tooltip("加速度")] public float acceleration;
        [SerializeField][Tooltip("減速度")] public float deceleration;
        [SerializeField][Tooltip("体力")] public int health;
    }

    public class PlayerCharacter : MonoBehaviour
    {
        bool isAlive;

        [SerializeField] private CharacterParameters _defaultParameters;
        private CharacterParameters _currentParameters;

        private PlayerCore _core;

        private CharacterController _characterController;

        private Vector3 _velocity = Vector3.zero;

        private Animator _animator;

        private bool _hitDamage;

        private bool _down;

        [SerializeField] float _invincibleTime;
        [SerializeField] float _downTime;

        void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
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
            if (_hitDamage) return;
            _animator.SetTrigger("Down");
            _core.TakeDamage(damage);
            StartCoroutine(InvincibleTime(_invincibleTime));
            StartCoroutine(DownTime(_downTime));
        }

        IEnumerator InvincibleTime(float time)
        {
            _hitDamage = true;
            yield return new WaitForSeconds(time);
            _hitDamage = false;
        }



        public void Move(Vector3 direction)
        {
            if (MainGameManager.instance.gameState != GameState.Main) return;
            if (_down) return;
            var maxSpeed = _currentParameters.maxSpeed;
            var acceleration = _currentParameters.acceleration;
            var deceleration = _currentParameters.deceleration;

            if (direction.magnitude > 0)
            {
                _animator.SetTrigger("Walk");
                _velocity += direction * acceleration * Time.deltaTime;
                if (_velocity.magnitude > maxSpeed)
                {
                    _velocity = _velocity.normalized * maxSpeed;
                }

                Quaternion targetRotation =Quaternion.LookRotation(direction);

                transform.rotation = targetRotation;

            }
            else
            {
                _animator.SetTrigger("Idle");
                if (_velocity.magnitude > 0)
                {
                    _velocity -= _velocity.normalized * deceleration * Time.deltaTime;
                    if (_velocity.magnitude <= 0.01f) { _velocity = Vector3.zero; }
                    
                }
            }

            // direction.y = direction.y + (Physics.gravity.y * Time.deltaTime);
            // _characterController.Move(direction * _currentParameters.moveSpeed);

            var gravity = Physics.gravity.y * Time.deltaTime;
            Vector3 velocity = _velocity + new Vector3(0, gravity, 0);
            _characterController.Move(velocity);
        }

        IEnumerator DownTime(float time)
        {
            _down = true;
            yield return new WaitForSeconds(time);
            _down = false;
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
        //アニメーションが実装されたらAnimationEventで呼び出す
    }
}

