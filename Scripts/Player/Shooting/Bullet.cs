using UnityEngine;

namespace LastAndZombies
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet  : MonoBehaviour
    {
        [SerializeField] private GameObject impactParticle;
        [SerializeField] private GameObject projectileParticle;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed;

        public void Init(in Vector3 target)
        {
            projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation, transform);
            
            Vector3 direction = target - transform.position;
            direction.y = 0;
            _rigidbody.velocity = direction * _speed;
        }
        
        public void InitDirection(Vector3 direction, float speed)
        {
            projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation, transform);
            _rigidbody.velocity = direction * speed;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy zombie))
            {
                zombie.Hit();
                BulletDestroy();
            }
            else if (other.TryGetComponent(out Boss boss))
            {
                boss.Hit();
                BulletDestroy();
            }
        }

        private void BulletDestroy()
        {
            GameObject impactP = Instantiate(impactParticle, transform.position, Quaternion.identity);

            Destroy(projectileParticle, 3f);
            Destroy(impactP, 3.5f);
            Destroy(gameObject);
        }
    }
}