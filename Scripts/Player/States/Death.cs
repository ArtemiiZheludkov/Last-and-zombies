using LastAndZombies.States;
using UnityEngine;

namespace LastAndZombies
{
    public class Death : PlayerState
    {
        private float _backForce;
        private Rigidbody _rigidbody;

        public Death(PlayerComponents components, DeathConfig config) : base(components)
        {
            _rigidbody = components.Rigidbody;
            _backForce = config.backForce;
        }

        public override void Enter()
        {
            base.Enter();

            if (_animator != null)
                _animator.CrossFade(_animations.Dying, 0.1f);
            
            _rigidbody.AddForce(Vector3.back * _backForce, ForceMode.VelocityChange);
        }
    }
}