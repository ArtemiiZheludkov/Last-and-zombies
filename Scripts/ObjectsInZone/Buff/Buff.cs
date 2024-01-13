using UnityEngine;

namespace LastAndZombies
{
    [RequireComponent(typeof(BoxCollider))]
    public class Buff : MonoBehaviour
    {
        [SerializeField] private BuffType _type;
        [SerializeField] private int _amount;

        [Header("ANIMATION SETTINGS")]
        [SerializeField] private float _maxScale;
        [SerializeField] private int _maxStartRotated;
        [SerializeField] private float _rotateSpeed;

        private void Awake()
        {
            float scale = Random.Range(1f, _maxScale);
            transform.localScale = new Vector3(scale, scale, scale);
            
            float randY = Random.Range(0, _maxStartRotated);
            transform.localRotation = Quaternion.Euler(0, randY, 0);
        }

        private void FixedUpdate()
        {
            transform.Rotate(0f, _rotateSpeed * Time.fixedDeltaTime, 0f, Space.Self);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                player.TakeBuff(_type, _amount);
                gameObject.SetActive(false);
            }
        }
    }
}