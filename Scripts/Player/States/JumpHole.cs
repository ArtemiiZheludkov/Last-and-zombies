using LastAndZombies.States;
using UnityEngine;

namespace LastAndZombies
{
    public class JumpHole : PlayerState
    {
        private float _zSpeed;
        private float _xSpeed;
        private float _limitX;
        private float _jumpForce;
        private Rigidbody _rigidbody;
        private Vector3 _lastMousePosition;
        private float _exitTime;

        public JumpHole(PlayerComponents components, JumpHoleConfig config) : base(components)
        {
            _rigidbody = components.Rigidbody;
            
            _zSpeed = config.zSpeed;
            _xSpeed = config.xSpeed;
            _limitX = config.limitX;
            _jumpForce = config.jumpForce;
            
            SetExitTime();
        }
        
        public override void Enter()
        {
            base.Enter();

            _animator.CrossFade(_animations.InJumpHole, 0.1f);
            _lastMousePosition = Input.mousePosition;
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
        }

        public override float TimeExit()
        {
            if (_animator != null)
                _animator.CrossFade(_animations.OutJumpHole, 0.1f);
            
            return _exitTime;
        }
        
        private void SetExitTime()
        {
            AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
            
            foreach (AnimationClip clip in clips)
            {
                if(clip.name == "OutJumpHole")
                {
                    _exitTime = clip.length;
                    _exitTime /= 2f;
                    break;
                }
            }
        }

        public override void Update()
        {
            if (Input.GetMouseButtonDown(0))
                _lastMousePosition = Input.mousePosition;
            else if (Input.GetMouseButton(0))
                MoveSide();

            MoveForward();
        }

        private void MoveForward()
        {
            Vector3 newPosition = _rigidbody.position +  Vector3.forward * (_zSpeed * Time.deltaTime);
            
            _rigidbody.MovePosition(newPosition);
        }
        
        private void MoveSide()
        {
            Vector3 deltaMousePos = Input.mousePosition - _lastMousePosition;
            float x = deltaMousePos.x / Screen.width;

            Vector3 offset = new Vector3(x * _xSpeed, 0f, 0f) * Time.deltaTime;
            Vector3 newPosition = _rigidbody.position + offset;
            newPosition.x = Mathf.Clamp(newPosition.x, -_limitX, _limitX);
            
            _rigidbody.MovePosition(newPosition);
            _lastMousePosition = Input.mousePosition;
        }
    }
}