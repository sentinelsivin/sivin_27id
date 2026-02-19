using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Services.StateMachine
{
    public class StateModeBehavior : MonoBehaviour
    {
      
        [SerializeField] private List<StateMachineBehavior> _states;
        private StateMachine _stateMachine;
        private const int DefaultState = 0;

        private void Awake() => InitStates();
        
        private void Start() => _stateMachine.Change(_states[DefaultState].GetType());

        private void OnDestroy() => _stateMachine.Release();

        private void InitStates()
        {
            _stateMachine = new StateMachine(_states);
            _states.ForEach(InitGameState);
        }

        private void InitGameState(StateMachineBehavior state)
        {
            foreach (var view in state.GetViews())
            {
                view.SetActive(false);
            }

            state.gameObject.SetActive(false);
        }

        protected void ChangeState<T>() where T : IStateMachine => _stateMachine.Change(typeof(T));
    }
}
