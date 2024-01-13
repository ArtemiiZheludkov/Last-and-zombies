using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LastAndZombies
{
    [RequireComponent(typeof(Rigidbody))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private int _healthHits;
        [SerializeField] private Animator _animator;

        [Header("DETECTION PLAYER")]
        [SerializeField] private float _rayDistance;
        [SerializeField] private Transform[] _rayPoints;

        [Header("HEALTH VIEW")]
        [SerializeField] private TextMeshPro _healthText;
        [SerializeField] private Material _backHealth;
        [SerializeField] private Color _killable;
        [SerializeField] private Color _dontKillable;

        private Action _died;
        
        private Rigidbody _rigidbody;
        private readonly HashAnimation _animations = new HashAnimation();
        private Vector3 _target;
        private bool _isDead;

        private PlayerBagUI _playerBag;

        private void Awake()
        {
            int randSkin = Random.Range(1, _animator.transform.childCount);
            _animator.transform.GetChild(randSkin).gameObject.SetActive(true);
            _healthText.gameObject.SetActive(false);
        }

        public void Stay()
        {
            _isDead = true;
            _rigidbody = GetComponent<Rigidbody>();
            _animator.CrossFade(_animations.Idle, 0.1f);
            _healthText.text = _healthHits.ToString();
            _backHealth.color = _dontKillable;
        }

        public void MoveTo(Vector3 target, PlayerBagUI bag)
        {
            _isDead = false;
            _target = target;
            transform.LookAt(target);
            _animator.StopPlayback();
            _animator.CrossFade(_animations.Run, 0.1f);
            _playerBag = bag;
            UpdateHealthView();
            _healthText.gameObject.SetActive(true);
        }

        public void Hit()
        {
            _healthHits -= 1;

            if (_healthHits <= 0)
                Die();
            else
                _healthText.text = _healthHits.ToString();
        }

        public void ReturnOnDied(Action died)
        {
            _died = died;
        }

        private void UpdateHealthView()
        {
            if (_playerBag.Ammo >= _healthHits)
                _backHealth.color = _killable;
        }

        private void Die()
        {
            if (_isDead == true)
                return;
            
            _isDead = true;
            _animator.CrossFade(_animations.Dying, 0f);
            _died?.Invoke();
            _healthText.transform.parent.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (_isDead == false)
            {
                Move();
                DetectPlayer();
                UpdateHealthView();
            }
        }

        private void Move()
        {
            Vector3 offset = (_target - _rigidbody.position).normalized * (_speed * Time.deltaTime);
            _rigidbody.MovePosition(_rigidbody.position + offset);

            if (Vector3.Distance(_target, _rigidbody.position) <= 0.01f)
            {
                _isDead = true;
                Stay();
            }
        }

        private void DetectPlayer()
        {
            RaycastHit hit;
            Ray ray;

            for (int i = 0; i < _rayPoints.Length; i++)
            {
                ray = new Ray(_rayPoints[i].position, _rayPoints[i].forward);
                Debug.DrawRay(_rayPoints[i].position, _rayPoints[i].forward * _rayDistance, Color.red);

                if (Physics.Raycast(ray, out hit, _rayDistance))
                {
                    if (hit.collider.TryGetComponent(out PlayerShooter player))
                    {
                        player.TryKill(this);
                        return;
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isDead == false)
            {
                if (other.TryGetComponent(out Player player))
                {
                    _animator.CrossFade(_animations.Attack, 0f);
                    player.TakeEnemyAttack();
                }
            }
        }
    }
}