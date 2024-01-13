using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace LastAndZombies
{
    public class Boss : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private int _healthHits;
        [SerializeField] private Animator _animator;
        [SerializeField] private Slider _healtBar;

        private Action _win, _died;
        
        private Rigidbody _rigidbody;
        private readonly HashAnimation _animations = new HashAnimation();
        private Vector3 _target;
        private bool _isDead;
        private int _currentHealth;

        private void Awake()
        {
            int randSkin = Random.Range(1, _animator.transform.childCount);
            _animator.transform.GetChild(randSkin).gameObject.SetActive(true);
            _healtBar.gameObject.SetActive(true);
            _healtBar.maxValue = 1f;
            _healtBar.value = _healtBar.maxValue;
            _currentHealth = _healthHits;
        }

        public void Stay()
        {
            _isDead = true;
            _rigidbody = GetComponent<Rigidbody>();
            _animator.CrossFade(_animations.Idle, 0.1f);
        }
        
        public void Scream()
        {
            _healtBar.gameObject.SetActive(true);
            _healtBar.value = _healtBar.maxValue;
            _currentHealth = _healthHits;
            _animator.CrossFade(_animations.Scream, 0.1f);
        }

        public void MoveTo(Vector3 target, Action win,Action died)
        {
            _win = win;
            _died = died;
            
            _isDead = false;
            _target = target;
            transform.LookAt(target);
            _animator.CrossFade(_animations.Run, 0f);
        }

        public void Hit()
        {
            _currentHealth -= 1;
            _healtBar.value = _currentHealth / (float)_healthHits;

            if (_currentHealth <= 0)
                Die();
        }

        private void Die()
        {
            if (_isDead == true)
                return;
            
            _isDead = true;
            _animator.CrossFade(_animations.Dying, 0f);
            _died?.Invoke();
            _healtBar.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (_isDead == false)
                Move();
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

        private void OnTriggerEnter(Collider other)
        {
            if (_isDead == false)
            {
                if (other.TryGetComponent(out Player player))
                {
                    _animator.CrossFade(_animations.Attack, 0f);
                    player.Kill();
                    _healtBar.gameObject.SetActive(false);
                    _speed = 0f;
                    _win?.Invoke();
                }
            }
        }
    }
}