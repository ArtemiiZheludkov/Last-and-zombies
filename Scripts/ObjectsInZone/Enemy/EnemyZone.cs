using UnityEngine;

namespace LastAndZombies
{
    public class EnemyZone : MonoBehaviour
    {
        [SerializeField] private Enemy enemy;
        [SerializeField] private Transform _target;

        private void Awake()
        {
            enemy.Stay();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                enemy.MoveTo(_target.position, player.Components.Bag);
            }
        }
    }
}