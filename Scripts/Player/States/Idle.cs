using LastAndZombies.States;

namespace LastAndZombies
{
    public class Idle : PlayerState
    {
        public Idle(PlayerComponents components, IdleConfig config = null) : base(components)
        {
            
        }
        
        public override void Enter()
        {
            base.Enter();

            if (_animator != null)
                _animator.CrossFade(_animations.Idle, 0.5f);
        }
    }
}