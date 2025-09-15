using RedSilver2.Framework.Inputs;
using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    public abstract class InteractionHandler 
    {
        private KeyboardKey keyboardKey;
        private GamepadKey  gamepadKey;
        private InteractionHandlerModule module;

        public bool IsInputHeld     => InputManager.GetKey(keyboardKey, gamepadKey); 
        public bool IsInputPressed  => InputManager.GetKeyDown(keyboardKey, gamepadKey);
        public bool IsInputReleased => InputManager.GetKeyUp(keyboardKey, gamepadKey);


        private static Dictionary<Collider, InteractionModule> interactionModuleInstances = new Dictionary<Collider, InteractionModule>();

        protected InteractionHandler()
        {
            this.keyboardKey = KeyboardKey.E;
            this.gamepadKey  = GamepadKey.ButtonEast;
        }

        protected InteractionHandler(InteractionHandlerModule module)
        {
            this.keyboardKey = KeyboardKey.E;
            this.gamepadKey  = GamepadKey.ButtonEast;
            this.module      = module;
        }

        protected InteractionHandler(KeyboardKey keyboardKey, GamepadKey gamepadKey, InteractionHandlerModule module)
        {
            this.keyboardKey = keyboardKey;
            this.gamepadKey  = gamepadKey;
            this.module      = module;
        }

  
        public void Update()
        {
            if (module != null)
            {
                InteractionModule interactionModule = GetInteractionModuleInstance(GetCollider(module.InteractionRange));
                if (interactionModule != null) interactionModule.Interact(this);
            }
        }

        protected abstract Collider GetCollider(float interactionRange);

        public static InteractionModule GetInteractionModuleInstance(Collider collider)
        {
            if (interactionModuleInstances == null || collider == null) return null;
            if (interactionModuleInstances.ContainsKey(collider)) return interactionModuleInstances[collider];  
            return null;
        }

        public static void AddInteractionModuleInstance(Collider collider, InteractionModule module)
        {
            if(collider != null && module != null && interactionModuleInstances != null)
            {
                if (!interactionModuleInstances.ContainsKey(collider))
                    interactionModuleInstances.Add(collider, module);
            }
        }
    }
}
