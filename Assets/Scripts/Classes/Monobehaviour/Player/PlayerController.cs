using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerStateMachine stateMachine;
        public PlayerStateMachine StateMachine => stateMachine;

        public const string PLAYER_LAYER_NAME = "Player";

        private void Awake()
        {
            stateMachine = new PlayerStateMachine(this);
            
            stateMachine.AddOnStateEnterListener(state => { if (state != null) Debug.Log($"Entering {state.GetStateName()} State | {GetTransitions(state.GetTransitionConditions())}"); });
            stateMachine.AddOnStateExitListener(state => { if (state != null) Debug.Log($"Exiting {state.GetStateName()} State | {GetTransitions(state.GetTransitionConditions())}"); });
        }

        private void Start()
        {
            stateMachine.ChangeState(PlayerStateMachine.IdolState.STATE_NAME);
        }

        private void Update()
        {
            if (stateMachine != null) stateMachine.Update();
        }

        private void LateUpdate()
        {
            if (stateMachine != null) stateMachine.LateUpdate();
        }

        public string GetTransition(PlayerStateMachine.PlayerStateTransitionCondition transitionCondition)
        {
            if (transitionCondition == null) return string.Empty;
            return transitionCondition.ToString() + "\n";
        }


        public string GetTransitions(PlayerStateMachine.PlayerState state)
        {
            if (state == null) return string.Empty;
            return GetTransitions(state.GetTransitionConditions());
        }

        public string GetTransitions(PlayerStateMachine.PlayerStateTransitionCondition[] conditions)
        {
            string result = string.Empty;

            foreach (var condition in conditions)
            {
                result += GetTransition(condition);
            }

            return result;
        }

        public static int GetLayer()
        {
            return LayerMask.NameToLayer(PLAYER_LAYER_NAME);
        }
    }
}
