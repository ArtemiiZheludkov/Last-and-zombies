using System;
using LastAndZombies.States;
using UnityEngine;

namespace LastAndZombies
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerShooter))]
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public PlayerComponents Components { get; private set;}
        [SerializeField] private ParticleSystem _dropMoney;

        [Header("STATE CONFIGS")] 
        [SerializeField] private IdleConfig _idle;
        [SerializeField] private RunConfig _run;
        [SerializeField] private SlideConfig _slide;
        [SerializeField] private JumpObstacleConfig _jumpObstacleConfig;
        [SerializeField] private JumpHoleConfig _jumpHole;
        [SerializeField] private DeathConfig _death;

        private Action _dead;
        private PlayerStateMachine _stateMachine;

        public void Init(Vector3 start)
        {
            _stateMachine = new PlayerStateMachine(Components, _idle, _run, _slide, _jumpObstacleConfig, _jumpHole, _death);
            _stateMachine.ChangeState<Idle>();
            
            Components.Bag.Deactivate();
            transform.position = start;
        }
        
        private void Update() => _stateMachine.CurrentState.Update();

        private void OnCollisionEnter(Collision collision) => _stateMachine.CurrentState.OnCollisionEnter(collision);

        public void Play(Action dead)
        {
            _dead = dead;

            Components.Shooter.Init(Components.Bag, Components.Animator, Components.Animations);
            Components.Bag.Init(50);
            
            Run();
        }
        
        public void Stay() => _stateMachine.ChangeState<Idle>();

        public void Run() => _stateMachine.ChangeState<Run>();

        public void InZone(ZoneType type)
        {
            switch (type)
            {
                case ZoneType.Deffault:
                    break;
                case ZoneType.RelaxSlide:
                    _stateMachine.ChangeState<Slide>();
                    break;
                case ZoneType.RelaxJump:
                    _stateMachine.ChangeState<JumpHole>();
                    break;
                case ZoneType.BossZone:
                    _stateMachine.ChangeState<FinalyBattle>();
                    break;
            }
        }

        public void OutRelax() => StartCoroutine(_stateMachine.WaitAndChange<Run>());

        public void JumpOverObstacle()
        {
            StopAllCoroutines();
            _stateMachine.ChangeState<JumpObstacle>();
        }

        public void Kill()
        {
            _stateMachine.ChangeState<Death>();
            _dead?.Invoke();
        }

        public void TakeEnemyAttack()
        {
            Components.Animator.CrossFade(Components.Animations.OnHitRunning, 0f);
            _dropMoney.Play();
            Components.Bag.EnemiesAttacked();
        }

        public void TakeBuff(BuffType type, int amount)
        {
            switch (type)
            {
                case BuffType.Ammo:
                    Components.Bag.ChangeAmmo(amount);
                    break;
                case BuffType.Money:
                    Components.Bag.AddMoney(amount);
                    break;
            }
        }

        public void MoveToStartBattle(Vector3 target, Action onPoint)
        {
            FinalyBattle finalyBattle = _stateMachine.GetState<FinalyBattle>();
            finalyBattle.MoveToTarget(target, onPoint);
        }

        public void Celebrate()
        {
            _stateMachine.ChangeState<Idle>();
            Components.Animator.CrossFade(Components.Animations.Dance, 0f);
        }
    }
}