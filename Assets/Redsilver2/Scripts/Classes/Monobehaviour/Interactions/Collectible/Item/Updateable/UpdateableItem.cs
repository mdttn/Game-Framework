using RedSilver2.Framework.Player.Inventories;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Items
{
    [RequireComponent(typeof(Animator))]
    public abstract partial class UpdateableItem : Item
    {
        [Space]
        [SerializeField] private GameObject leftHand, rightHand;

        [Space]
        [SerializeField] private AnimationClip equipAnimation, unequipAnimation, dropAnimation;
        
        private bool                       isEquipped;
        private UnityEvent                 onUpdate, onLateUpdate, onDisable, onEnable;
        private UnityEvent<UpdateableItem> onEquip, onUnequip;

        private Animator    animator;

        public string EquipAnimationName
        {
            get {
                if (equipAnimation == null) return string.Empty;
                return equipAnimation.name;
            }
        }

        public string UnequipAnimationName
        {
            get {
                if (unequipAnimation == null) return string.Empty;
                return unequipAnimation.name;
            }
        }
       
        private ItemHandType handType;
        public  ItemHandType HandType => handType;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();

            onEnable     = new UnityEvent();
            onDisable    = new UnityEvent();

            onUpdate     = new UnityEvent();
            onLateUpdate = new UnityEvent();

            onEquip      = new UnityEvent<UpdateableItem>();
            onUnequip    = new UnityEvent<UpdateableItem>();

            isEquipped   = false;
            SetHandType(ref handType);
        }

        protected override void Start() {
            base.Start();
           
            AddOnEnableListener(OnEnableListener);
            AddOnDisableListener(OnDisableListener);

            SetAnimatorState(false);
            SetHandsVisibility(false);
            SetVisiblity(true, true);

            isEquipped = false;
            enabled    = false;
        }

        private void OnDisable() {
            if (onDisable != null) onDisable.Invoke(); 
        }

        private void OnEnable() {
            if (onEnable != null) onEnable.Invoke();
        }

        private void Update() {
            if(onUpdate != null) onUpdate.Invoke();
        }

        private void LateUpdate() {
            if(onLateUpdate != null) onLateUpdate.Invoke();
        }

        public void AddOnUpdateListener   (UnityAction action) {
            if(onUpdate != null && action != null)
                onUpdate.AddListener(action);
        }
        public void RemoveOnUpdateListener(UnityAction action)
        {
            if (onUpdate != null && action != null)
                onUpdate.RemoveListener(action);
        }

        public void AddOnLateUpdateListener(UnityAction action)
        {
            if (onLateUpdate != null && action != null)
                onLateUpdate.AddListener(action);
        }
        public void RemoveOnLateUpdateListener(UnityAction action)
        {
            if (onLateUpdate != null && action != null)
                onLateUpdate.RemoveListener(action);
        }

        public void AddOnEnableListener(UnityAction action)
        {
            if (onEnable != null && action != null)
                onEnable.AddListener(action);
        }
        public void RemoveOnEnableListener(UnityAction action)
        {
            if (onEnable != null && action != null)
                onEnable.RemoveListener(action);
        }

        public void AddOnDisableListener(UnityAction action)
        {
            if (onDisable != null && action != null)
                onDisable.AddListener(action);
        }
        public void RemoveOnDisableListener(UnityAction action)
        {
            if (onDisable != null && action != null)
                onDisable.RemoveListener(action);
        }

        public void AddOnEquipListener(UnityAction<UpdateableItem> action)
        {
            if (onEquip != null && action != null)
                onEquip.AddListener(action);
        }
        public void RemoveOnEquipListener(UnityAction<UpdateableItem> action)
        {
            if (onEquip != null && action != null)
                onEquip.RemoveListener(action);
        }

        public void AddOnUnequipListener(UnityAction<UpdateableItem> action)
        {
            if (onUnequip != null && action != null)
                onUnequip.AddListener(action);
        }
        public void RemoveOnUnequipListener(UnityAction<UpdateableItem> action)
        {
            if (onUnequip != null && action != null)
                onUnequip.RemoveListener(action);
        }

        public void SetHand(ItemHandSide handSide) {
            SetAnimatorController(handSide);
            SetHandsVisibility(handSide);
        }

        public async void Equip() {
            await CrossFadeAnimationAsync(equipAnimation, OnEquipAnimationStarted, OnEquipAnimationFinished);
        }
        public async void Unequip() {
            await CrossFadeAnimationAsync(unequipAnimation, OnUnequipAnimationStarted, OnUnequipAnimationFinished);
        }

        public sealed override async void Drop(){
            await CrossFadeAnimationAsync(dropAnimation, OnDropAnimationStarted, OnDropAnimationFinished);
        }

        private void SetAnimatorController(ItemHandSide handSide) {
            if(animator != null) animator.runtimeAnimatorController = GetAnimatorController(handSide);
        }

        protected async Awaitable PlayAnimationAsync(AnimationClip clip) {
            await PlayAnimationAsync(clip, null, null);
        }
        protected async Awaitable PlayAnimationAsync(AnimationClip clip, UnityAction onStarted) {
            await PlayAnimationAsync(clip, onStarted, null);
        }
        protected async Awaitable PlayAnimationAsync(AnimationClip clip, UnityAction onStarted, UnityAction onFinished) 
        {
            if(animator != null) await animator.PlayAnimationAsync(clip, onStarted, onFinished);
        }

        protected async Awaitable CrossFadeAnimationAsync(AnimationClip clip)
        {
            await CrossFadeAnimationAsync(clip, null, null);
        }
        protected async Awaitable CrossFadeAnimationAsync(AnimationClip clip, UnityAction onStarted)
        {
            await CrossFadeAnimationAsync(clip, onStarted, null);
        }
        protected async Awaitable CrossFadeAnimationAsync(AnimationClip clip, UnityAction onStarted, UnityAction onFinished)
        {
            if (animator != null) {
                await animator.AsyncCrossFadeAnimation(clip, 0.02f, onStarted, onFinished);
            }
        }


        protected virtual void OnDisableListener()
        {
            isEquipped = false;
        }

        protected virtual void OnEnableListener(){
            isEquipped = true;
        }

        protected virtual void OnDropAnimationStarted() 
        {
            enabled    = false;
            SetAnimatorState(true);
            SetHandsVisibility(false);
            SetVisiblity(false, true);
        }
        protected virtual void OnEquipAnimationStarted() 
        {
            enabled = false;
            SetAnimatorState(true);
            SetVisiblity(false, true);
        }
        protected virtual void OnUnequipAnimationStarted() 
        {
            enabled    = false;
            SetAnimatorState(true);
            SetVisiblity(false, true);
        }


        protected virtual void OnDropAnimationFinished() {
            Inventory inventory = Inventory.GetInventoryWithItem(this);

            if (inventory != null) inventory.RemoveItem(this, out bool isRemoved);
            SetAnimatorState(false);
            SetVisiblity(true, true);
        }
        protected virtual void OnEquipAnimationFinished() {
            enabled    = true;
            SetVisiblity(false, true);
        }
        protected virtual void OnUnequipAnimationFinished() {
            SetAnimatorState(false);
            SetVisiblity(false, false);
        }

        private void SetHandsVisibility(ItemHandSide handSide)
        {
            SetLeftHandVisibility(handSide == ItemHandSide.Left ? true : false);
            SetRightHandVisibility(handSide == ItemHandSide.Right ? true : false);
        }

        public void SetHandsVisibility(bool isVisible)
        {
            SetLeftHandVisibility(isVisible);
            SetRightHandVisibility(isVisible);
        }

        private void SetLeftHandVisibility(bool isVisible) {
            if(leftHand != null) leftHand.SetActive(handType == ItemHandType.OneHanded ? isVisible : true);
        }

        private void SetRightHandVisibility(bool isVisible)  {
            if(rightHand != null) rightHand.SetActive(handType == ItemHandType.OneHanded ? isVisible : true);
        }

        protected void SetAnimatorState(bool isEnabled)
        {
            if(animator != null) animator.enabled = isEnabled;
        }

        public bool IsPlayingAnimation(string animationName) {
            if(animator == null) return false;
            return animator.CompareCurrentAnimationName(animationName);
        }


        protected abstract void SetHandType(ref ItemHandType handType);
        protected abstract RuntimeAnimatorController GetAnimatorController(ItemHandSide handSide);
    }
}
