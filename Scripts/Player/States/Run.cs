using LastAndZombies.States;
using UnityEngine;

namespace LastAndZombies
{
    public class Run : PlayerState
    {
        private float _zSpeed;
        private float _xSpeed;
        private float _limitX;
        private float _rotationAngle;
        private float _rotationSpeed;
        private Rigidbody _rigidbody;
        private Vector3 _lastMousePosition;
        private Vector3 _normal;
        private Quaternion _targetRotation;
        
        private readonly Vector3 _forwardNormalized = Vector3.forward.normalized;

        public Run(PlayerComponents components, RunConfig config)  : base(components)
        {
            _rigidbody = components.Rigidbody;
            
            _zSpeed = config.zSpeed;
            _xSpeed = config.xSpeed;
            _limitX = config.limitX;
            _rotationAngle = config.rotationAngle;
            _rotationSpeed = config.rotationSpeed;
        }
        
        public override void Enter()
        {
            base.Enter();
            
            if (_animator != null)
                _animator.CrossFade(_animations.Run, 0.1f);
            
            _lastMousePosition = Input.mousePosition;
        }

        public override void Update()
        {
            if (Input.GetMouseButtonDown(0))
                _lastMousePosition = Input.mousePosition;
            else if (Input.GetMouseButton(0))
                MoveSide();
            else if (Input.GetMouseButtonUp(0))
                RotateDirection(0);

            MoveForward();
        }

        private void MoveForward()
        {
            Vector3 direction = _forwardNormalized - Vector3.Dot(_forwardNormalized, _normal) * _normal;
            Vector3 newPosition = _rigidbody.position +  direction * (_zSpeed * Time.deltaTime);
            
            _rigidbody.MovePosition(newPosition);
        }
        
        private void MoveSide()
        {
             Vector3 deltaMousePos = Input.mousePosition - _lastMousePosition;
             float x = deltaMousePos.x / Screen.width;

             Vector3 offset = new Vector3(x * _xSpeed, 0f, 0f) * Time.deltaTime;
             Vector3 newPosition = _rigidbody.position + offset;
             newPosition.x = Mathf.Clamp(newPosition.x, -_limitX, _limitX);
             
             RotateDirection(offset.x);
            _rigidbody.MovePosition(newPosition);
            _lastMousePosition = Input.mousePosition;
        }

        private void RotateDirection(float x)
        {
            if (x < 0)
                _targetRotation = Quaternion.Euler(0f, -_rotationAngle, 0f);
            else if (x > 0)
                _targetRotation = Quaternion.Euler(0f, _rotationAngle, 0f);
            else
                _targetRotation = Quaternion.Euler(0f, 0f, 0f);
            
            _animator.transform.localRotation = Quaternion.Lerp(
                _animator.transform.localRotation, 
                _targetRotation, 
                Time.deltaTime * _rotationSpeed);
        }

        public override void OnCollisionEnter(Collision collision)
        {
            Vector3 normal = collision.contacts[0].normal;
            
            if (normal.y > 0f && normal.z > -0.5f)
                _normal = normal;
        }
    }
}
