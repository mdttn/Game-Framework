using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerStateMachine stateMachine;

        private static PlayerController current;
        private static List<PlayerController> instances = new List<PlayerController>();

        public static PlayerController[] Instances
        {
            get
            {
                if (instances == null) return new PlayerController[0];
                return instances.ToArray();
            }
        }

        public PlayerStateMachine StateMachine => stateMachine;
        public  static PlayerController Current => current;

        public const string PLAYER_LAYER_NAME = "Player";

        protected virtual void Awake() {
            stateMachine = new PlayerStateMachine(this);    
            stateMachine.AddOnStateEnterListener (state => { if (state != null) Debug.Log($"Entering {state.GetStateName()} State | {GetTransitions(state.GetTransitionConditions())}"); });
            stateMachine.AddOnStateExitListener  (state => { if (state != null) Debug.Log($"Exiting { state.GetStateName()} State | {GetTransitions(state.GetTransitionConditions())}"); });
            instances.Add(this);
        }

        protected virtual void Start() {
            current = this;
            stateMachine.ChangeState(PlayerStateMachine.IdolState.STATE_NAME);
        }


        protected virtual void Update() {
            if (stateMachine != null) stateMachine.Update();
        }

        protected virtual void LateUpdate() {
            if (stateMachine != null) stateMachine.LateUpdate();
        }

        private void OnDestroy() {
            if(instances.Contains(this)) instances.Remove(this);
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

        public static int GetLayer() {
            return LayerMask.NameToLayer(PLAYER_LAYER_NAME);
        }

        public static void SetCurrent(int index) {
            SetCurrent(GetController(index));
        }

        public static void SetCurrent(string controllerName) {
            SetCurrent(GetController(controllerName));
        }

        public static void SetCurrent(PlayerController controller) {
            Disable();
            current = controller;
            Enable();
        }

        public static void Disable() {
            if (current != null) current.enabled = false;
        }

        public static void Enable() {
            if (current != null) current.enabled = true;
        }

        public static void CleanControllers() {
            if(instances != null) instances = instances.Where(x => x != null).ToList();
        }


        public static PlayerController GetController(int index) {
            if(instances.Count == 0 || index < 0 || index >= instances.Count)
                return null;
            return instances[index];
        }

        public static PlayerController GetController(string controllerName)
        {
            if(instances == null || string.IsNullOrEmpty(controllerName)) return null;

            var results = instances.Where(x => x != null)
                                     .Where(x => x.name.ToLower() == controllerName.ToLower());

            if (results.Count() > 0) return results.First();
            return null;
        }
    }
}
