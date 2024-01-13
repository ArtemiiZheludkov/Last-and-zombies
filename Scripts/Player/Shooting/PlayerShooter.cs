using UnityEngine;
using UnityEngine.Serialization;

namespace LastAndZombies
{
    public class PlayerShooter : MonoBehaviour
    {
        [SerializeField] private Bullet _bullet;
        [SerializeField] private Transform _spawn;
        [SerializeField] private float _shotSpeed;
        [SerializeField] private float _battleBulletSpeed;
        private PlayerBagUI _bag;
        private Animator _animator;
        private HashAnimation _animations;

        private float _nextShotTime;

        public void Init(PlayerBagUI bag, Animator animator, HashAnimation animations)
        {
            _bag = bag;
            _animator = animator;
            _animations = animations;
            _nextShotTime = 0f;
        }

        public void TryKill(Enemy enemy)
        {
            if (_bag.Ammo < 1)
                return;

            if (Time.time >= _nextShotTime)
            {
                Bullet bullet = Instantiate(_bullet, _spawn.position, Quaternion.identity);
                bullet.Init(enemy.transform.position);
                
                _bag.ChangeAmmo(-1);
                _nextShotTime = Time.time + _shotSpeed;
                
                enemy.ReturnOnDied(EnemyDied);
            }
        }

        public void ShotForward()
        {
            if (_bag.Ammo < 1)
                return;

            if (Time.time >= _nextShotTime)
            {
                Bullet bullet = Instantiate(_bullet, _spawn.position, Quaternion.identity);
                bullet.InitDirection(Vector3.forward, _battleBulletSpeed);
                
                _bag.ChangeAmmo(-1);
                _nextShotTime = Time.time + _shotSpeed;
                _animator.Play(_animations.StayAndShot, 0);
            }
        }
        
        private void EnemyDied()
        {
            _bag.EnemiesKilled();
        }
    }
}