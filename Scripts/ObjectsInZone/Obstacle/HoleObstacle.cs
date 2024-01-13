using UnityEngine;

namespace LastAndZombies
{
    public class HoleObstacle : MonoBehaviour
    {
        private bool _playerInZone;
        
        private void Awake()
        {
            _playerInZone = false;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                if (_playerInZone == false)
                    player.InZone(ZoneType.RelaxJump);
                else
                    player.OutRelax();

                _playerInZone = !_playerInZone;
            }
        }
    }
}