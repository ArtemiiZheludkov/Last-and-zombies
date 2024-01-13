using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LastAndZombies.States;

namespace LastAndZombies
{
    public class PlayerStateMachine
    {
        public PlayerState CurrentState { get; private set; }

        private readonly Dictionary<Type, PlayerState> _states;
        
        public PlayerStateMachine(PlayerComponents components, IdleConfig idle, RunConfig run, SlideConfig slide, 
            JumpObstacleConfig jumpObstacle, JumpHoleConfig jumpHole, DeathConfig death)
        {
            _states = new Dictionary<Type, PlayerState>();
            _states.Add(typeof(Idle), new Idle(components, idle));
            _states.Add(typeof(Run), new Run(components, run));
            _states.Add(typeof(Slide), new Slide(components, slide));
            _states.Add(typeof(JumpObstacle), new JumpObstacle(components, jumpObstacle));
            _states.Add(typeof(JumpHole), new JumpHole(components, jumpHole));
            _states.Add(typeof(Death), new Death(components, death));
            _states.Add(typeof(FinalyBattle), new FinalyBattle(components, run));
            
            foreach (var state in _states)
                state.Value.Exit();
        }

        private void EnterState<T>() where T : PlayerState
        {
            CurrentState = _states[typeof(T)];
            CurrentState.Enter();
        }
        
        public void ChangeState<T>() where T : PlayerState
        {
            if (CurrentState == _states[typeof(Death)])
                return;
            
            if (CurrentState != null)
                CurrentState.Exit();

            EnterState<T>();
        }

        public IEnumerator WaitAndChange<T>(float waitTime = 0f) where T : PlayerState
        {
            if (CurrentState != null && waitTime <= 0f)
                waitTime = CurrentState.TimeExit();

            yield return new WaitForSeconds(waitTime);
            ChangeState<T>();
        }

        public T GetState<T>() where T : PlayerState
        {
            return (T)_states[typeof(T)];
        }
    }
}
