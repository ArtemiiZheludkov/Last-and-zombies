using UnityEngine;

namespace LastAndZombies
{
    public abstract class PlayerState
    {
        protected Animator _animator;
        protected HashAnimation _animations;

        public PlayerState(PlayerComponents components)
        {
            _animator = components.Animator;
            _animations = components.Animations;
        }

        public virtual void Enter()
        {
            
        }

        public virtual float TimeExit()
        {
            return 0f;
        }
        
        public virtual void Update()
        {
        }

        public virtual void OnCollisionEnter(Collision collision)
        {
            
        }

        public virtual void Exit()
        {
            
        }
    }
}

