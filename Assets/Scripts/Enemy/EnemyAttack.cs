using GameManagers;
using Players;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemy
{
    [System.Serializable]
    public class AttackParameters
    {
        [Tooltip("移動速度")] public float moveSpeed;
        [Tooltip("攻撃速度")] public float attackSpeed;
        [Tooltip("追尾時間")] public float chaseTime;
        [Tooltip("攻撃猶予時間")] public float attackTime;
        [Tooltip("残存時間")] public float remainTime;
        [Tooltip("追尾するか")] public bool isChase;
        [Tooltip("攻撃が地面に残るか")] public bool isStay;
    }

    public class EnemyAttack : MonoBehaviour
    {
        [Header("調整用パラメータ")]
        [Header("敵のパラメータ")]
        [Header("調整用パラメータ")]
        [Header("敵のパラメータ")]
        [SerializeField] private AttackParameters _defaultParameters;
        //戻る速度
        [SerializeField][Tooltip("上に上がる速度")] float upSpeed = 0.2f;

        //元の高さ
        [SerializeField][Tooltip("生成位置")] float _defaultHeight = 10.0f;

        //ステージの高さ
        float _stageHeight = 0;

        [Header("-----------------------------")]
        [Space(10)]

        private AttackParameters _currentParameters;

        //追跡するオブジェクト
        [SerializeField] public Transform _targetTransform;

        //攻撃関数用のフラグ
        private bool isAttacking = false;

        [HideInInspector] public bool isActive = true;

        private bool _isMoving = true;

        private bool _isUp = false;

        [SerializeField] private float frequency = 20.0f;

        [SerializeField] private float amplitude = 0.02f;

        private Vector3 originalPosition;

        private Collider[] _colliders;
        private Rigidbody _rigidbody;

        private MeshRenderer[] _meshRenderers;
        private SkinnedMeshRenderer[] _skinsMesh;
        private TrailRenderer _trailrenderer;

        private bool coruStop;
        private bool isAttack = false;

        [SerializeField] GameObject hitEffect;
        [SerializeField] Vector3 effectScale = Vector3.one;

        private EnemyCore _enemyCore;

        private void Awake()
        {
            _colliders = GetComponentsInChildren<Collider>();
            _rigidbody = GetComponent<Rigidbody>();

            _meshRenderers = GetComponentsInChildren<MeshRenderer>();

            _skinsMesh = GetComponentsInChildren<SkinnedMeshRenderer>();
            _trailrenderer = GetComponentInChildren<TrailRenderer>();

            foreach (var skinsMesh in _skinsMesh)
            {
                float alpha = 0.3f;
                var color = skinsMesh.material.color;
                color.a = alpha;
                skinsMesh.material.color = color;
            }

            foreach (var meshRenderer in _meshRenderers)
            {
                float alpha = 0.3f;
                var color = meshRenderer.material.color;
                color.a = alpha;
                meshRenderer.material.color = color;
            }

            _currentParameters = _defaultParameters;
            if(_trailrenderer != null)
            _trailrenderer.enabled = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (_targetTransform == null)
            {
                _currentParameters.isChase = false;
            }

            StartCoroutine(DelayCoroutine(_currentParameters.chaseTime, () => { Attack(); }));
        }

        // Update is called once per frame
        void Update()
        {

        }
        /**
         * @brief 敵の移動
         * 
         */
        void Move()
        {
            if (_targetTransform == null) return;
            //プレイヤーを追跡する
            if (_currentParameters.isChase)
            {
                Vector3 distance = new Vector3(_targetTransform.position.x - this.transform.position.x, 0, _targetTransform.position.z - this.transform.position.z);
                if (distance.magnitude > _currentParameters.moveSpeed)
                {
                    distance = distance.normalized;
                    distance.Scale(new Vector3(_currentParameters.moveSpeed, 0, _currentParameters.moveSpeed));
                    this.transform.position += distance;
                }
                else
                {
                    // this.transform.position = new Vector3(_targetTransform.position.x, this.transform.position.y, _targetTransform.position.z);
                    this.transform.localPosition += distance;
                }
            }
        }

        void Attack()
        {
            _isMoving = false;
            originalPosition = transform.position;
            StartCoroutine(Shake());
            
        }

        IEnumerator Shake()
        {
            float remainingTime = _currentParameters.attackTime;

            while (remainingTime >0)
            {
                float shake = Mathf.Sin(remainingTime* frequency *(Mathf.PI)) * amplitude;

                transform.position = new Vector3(originalPosition.x + shake, originalPosition.y, originalPosition.z);

                remainingTime -= Time.deltaTime;
                yield return null;
            }
            AudioManager.Instance.PlaySE("Main_FallStart_SE");
            transform.position = originalPosition;
            //isShake = false;
            isAttacking = true;
            isAttack = true;
            if (_trailrenderer != null) 
            _trailrenderer.enabled = true;
        }

        void AttackMove()
        {
            if (!isAttacking) return; 
            //武器を降ろす
            if (isAttacking&&!_isUp)
            {
                // transform.position -= _currentParameters.attackSpeed * transform.up;
                transform.localPosition -= _currentParameters.attackSpeed * transform.up;
                if (coruStop) return;
                StartCoroutine(FadeIn(0.5f));
                coruStop = true;
            }

            //攻撃がステージに到達
            // if (isAttacking && transform.position.y - transform.localScale.y / 2 <= _stageHeight)
            // {
            //     if (_currentParameters.isStay)
            //     {
            //         Deactivate();
            //         StartCoroutine(DelayCoroutine(_currentParameters.remainTime, () => { DestroyWithFade(); }));
            //         isAttacking = false;
            //     }
            //     else
            //     {
            //         _isUp = true;
            //     }
            //     //ステージに埋まらないようにする
            //     float buff = transform.localScale.y / 2;
            //     transform.position = new Vector3(transform.position.x, _stageHeight + buff, transform.position.z);
            // }

            //元の高さに戻る
            if (_isUp) { transform.position += upSpeed * transform.up; }
            //元の高さに到達
            if (_isUp && transform.position.y >= _defaultHeight)
            {
                Destroy(gameObject);
            }
        }

        public void SetTarget(Transform target)
        {
            _targetTransform = target;
        }

        public void Initialize(float startHeight, float stageHeight, Transform target)
        {
            _defaultHeight = startHeight;
            _stageHeight = stageHeight;
            _targetTransform = target;
            transform.localPosition = new Vector3(_targetTransform.position.x, _defaultHeight, _targetTransform.position.z);
        }

        public void SetCore(EnemyCore core)
        {
            _enemyCore = core;
        }

        void Activate()
        {
            foreach( var collider in _colliders)
            {
                collider.isTrigger = true;
            }
            isActive = true;
        }

        void Deactivate()
        {
            foreach( var collider in _colliders)
            {
                collider.isTrigger =false;
                //Debug.Log($"コライダーの状態:{collider},{collider.isTrigger}");
            }
            isActive = false;
        }

        IEnumerator FadeIn(float fadeTime)
        {
            float alpha = 0.3f;
            float interval = 0.1f;
            while (alpha < 1.0f)
            {
                alpha += interval / fadeTime;
                foreach (var meshRenderer in _meshRenderers)
                {
                    var color = meshRenderer.material.color;
                    color.a = alpha;
                    meshRenderer.material.color = color;
                }
                foreach (var skinsMesh in _skinsMesh)
                {
                    var color = skinsMesh.material.color;
                    color.a = alpha;
                    skinsMesh.material.color = color;
                }
                yield return new WaitForSeconds(interval);
            }
        }

        IEnumerator FadeOut(float fadeTime)
        {
            float alpha = 1.0f;
            float interval = 0.1f;
            while (alpha > 0.0f)
            {
                alpha -= interval / fadeTime;
                foreach (var meshRenderer in _meshRenderers)
                {
                    var color = meshRenderer.material.color;
                    color.a = alpha;
                    meshRenderer.material.color = color;
                }
                foreach (var skinsMesh in _skinsMesh)
                {
                    var color = skinsMesh.material.color;
                    color.a = alpha;
                    skinsMesh.material.color = color;
                }
                yield return new WaitForSeconds(interval);
            }

            Destroy(gameObject);
        }

        void DestroyWithFade()
        {
            StartCoroutine(FadeOut(1.0f));
        }

        IEnumerator DelayCoroutine(float waitTime, System.Action action)
        {
            yield return new WaitForSeconds(waitTime);
            action?.Invoke();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && isAttack && isAttacking)
            {
                isAttack = false;
                //エフェクト再生
                GameObject effect = Instantiate(hitEffect,other.transform);
                effect.transform.localScale = effectScale;
                ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
                if(particleSystem != null)particleSystem.Play();
                _enemyCore.ShakeCamera();
                if (other.gameObject.TryGetComponent<PlayerCharacter>(out var player))
                {
                    isAttacking = false;
                    player.TakeDamage(1);
                    Destroy(gameObject);
                }
                if (other.gameObject.TryGetComponent<PlayerPartial>(out var partial))
                {
                    partial.TakeDamage(1);
                }
            }
            else if (other.gameObject.tag == "Stage" && isAttacking)
            {
                if (_currentParameters.isStay)
                {
                    Deactivate();
                    StartCoroutine(DelayCoroutine(_currentParameters.remainTime, () => { DestroyWithFade(); }));
                    PlaySE();
                    _enemyCore.ShakeCamera();
                    isAttacking = false;
                }
                else
                {
                    _isUp = true;
                }
            }
        }

        private void FixedUpdate()
        {
            if (isAttacking)
            {
                AttackMove();
            }
            else if (_isMoving)
            {
                Move();
            }
        }

        public bool GetIsChase()
        {
            return _currentParameters.isChase;
        }

        public virtual void PlaySE() { }
    }
}
