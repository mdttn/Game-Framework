using RedSilver2.Framework.Interactions.Items;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public class InventoryUINavigatorTransitionsHandler : InventoryUI
    {
        [SerializeField] private bool allowInventoryCloseTransition;

        private Dictionary<GameObject, IEnumerator> currentTransitions;
        private List<InventoryUITransition> transitions;

        private UnityEvent onTransitionStarted, onTransitionFinished;
        private CancellationTokenSource transitionsTokenSource;

        public bool IsCompleted
        {
            get
            {
                if (transitions == null) return true;

                foreach (InventoryUITransition transition in transitions.Where(x => x != null))
                    if (!transition.IsFinished()) return false;

                return true;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            onTransitionStarted = new UnityEvent();
            onTransitionFinished = new UnityEvent();

            currentTransitions = new Dictionary<GameObject, IEnumerator>();
            transitions = new List<InventoryUITransition>();

            if (navigator != null)
                AddTransitions(navigator.GetComponentsInChildren<InventoryUITransition>());
        }
        protected override void Start()
        {
            SetNavigatorEvents(true);
        }

        protected sealed override void OnDestroy()
        {
            if (transitionsTokenSource != null) {
                transitionsTokenSource.Cancel();
                transitionsTokenSource = null;
            }

            SetNavigatorEvents(false);
            StopTransitions();
        }

        private void SetNavigatorEvents(bool addEvents)
        {
            if (navigator == null) return;

            if (addEvents)
            {
                navigator.AddOnOpenUIListener(OnOpenUI);
                navigator.AddOnCloseUIListener(OnCloseUI);
                navigator.AddOnItemAddedListener(OnItemAdded);
                navigator.AddOnItemRemovedListener(OnItemRemoved);
            }
            else {
                navigator.RemoveOnOpenUIListener(OnOpenUI);
                navigator.RemoveOnCloseUIListener(OnCloseUI);
                navigator.RemoveOnItemAddedListener(OnItemAdded);
                navigator.RemoveOnItemRemovedListener(OnItemRemoved);
            }

            if (navigator is PageInventoryUINavigator) { SetNavigatorEvents(navigator as PageInventoryUINavigator, addEvents); }
        }

        private void SetNavigatorEvents(PageInventoryUINavigator navigator, bool addEvents)
        {
            if (navigator == null) return;

            if (addEvents)
                navigator.AddOnPageIndexChangedListener(OnPageIndexChangedListener);
            else
                navigator.RemoveOnPageIndexChangedListener(OnPageIndexChangedListener);
        }

        private async void OnPageIndexChangedListener(int pageIndex)
        {
            bool wasCancelled;
            if (navigator == null) return;

            navigator.UpdateItems();
            wasCancelled = await AsyncTransition(false);

            Debug.LogWarning("------------ | " + wasCancelled);

            navigator.UpdateModels();
            if (!wasCancelled) await AsyncTransition(true);
        }

        private async void OnItemAdded(Item item) {
            if (navigator == null || item == null) return;
            await AsyncTransition(item, true);
        }

        private async void OnItemRemoved(Item item) {
            if (navigator == null || item == null) return;
            await AsyncTransition(item, false);
        }

        private async void OnOpenUI()
        {
            Inventory inventory = InventoryUINavigator.GetInventory(navigator);
            if (navigator == null || inventory == null) return;

            navigator.UpdateItems();
            navigator.UpdateModels();

            await AsyncTransition(true);
        }


        private async void OnCloseUI()
        {
            Inventory inventory = InventoryUINavigator.GetInventory(navigator);
            bool wasCancelled = false;

            if (navigator == null || inventory == null) return;
            if (allowInventoryCloseTransition) wasCancelled = await AsyncTransition(false);

            if (!wasCancelled) {
                inventory.DisableUI();
                navigator.ClearModels();
            }
        }

        public void AddOnTransitionStartedListener(UnityAction action)
        {
            if (onTransitionStarted != null && action != null)
                onTransitionStarted.AddListener(action);
        }
        public void RemoveOnTransitionStartedListener(UnityAction action)
        {
            if (onTransitionStarted != null && action != null)
                onTransitionStarted.RemoveListener(action);
        }

        public void AddOnTransitionFinishedListener(UnityAction action)
        {
            if (onTransitionFinished != null && action != null)
                onTransitionFinished.AddListener(action);
        }
        public void RemoveOnTransitionFinishedListener(UnityAction action)
        {
            if (onTransitionFinished != null && action != null)
                onTransitionFinished.RemoveListener(action);
        }


        private void AddTransitions(InventoryUITransition[] _transitions)
        {
            if (transitions == null || _transitions == null) return;

            foreach (InventoryUITransition transition in _transitions.Where(x => x != null))
                transitions.Add(transition);

            transitions = transitions.Distinct().ToList();
        }

        private void StopTransitions()
        {
            if (currentTransitions == null) return;
            Debug.LogWarning($"Stopping {currentTransitions.Count} Page Transitions");

            foreach (GameObject model in currentTransitions.Keys)
                StopTransition(model);

            currentTransitions.Clear();
        }


        private void StopTransition(GameObject model)
        {
            if (model == null || currentTransitions == null) return;

            if (currentTransitions.ContainsKey(model)) {
                StopCoroutine(currentTransitions[model]);

                foreach (InventoryUITransition transition in transitions.Where(x => x != null))
                    transition.StopTransition(model);
            }
        }

        public async void Transition(bool isShowingModels) {
            await AsyncTransition(isShowingModels);
        }

        public async Awaitable<bool> AsyncTransition(bool isShowingModels)
        {
            CancellationToken token = ResetToken();

            StopTransitions();
            StartTransitions(isShowingModels);

            await AwaitTransition(token);
            return token.IsCancellationRequested;
        }

        public async Awaitable<bool> AsyncTransition(Item item, bool isShowingModels)
        {
            int horizontalIndex;
            if (navigator == null || item == null) return true;

            horizontalIndex = navigator.GetHorizontalIndex(item);

            if (navigator is VerticalInventoryUINavigator)
                return await AsyncTransition(item, navigator as VerticalInventoryUINavigator, horizontalIndex, isShowingModels);

            return await AsyncTransition(item, navigator as SimpleInventoryUINavigator, horizontalIndex, isShowingModels);
        }

        private async Awaitable<bool> AsyncTransition(Item item, SimpleInventoryUINavigator navigator, int horizontalIndex, bool isShowingModels)
        {
            if (navigator == null || item == null) return true;
            return await AsyncTransition(navigator.GetModel(horizontalIndex), 0, horizontalIndex, isShowingModels);
        }

        private async Awaitable<bool> AsyncTransition(Item item, VerticalInventoryUINavigator navigator, int horizontalIndex, bool isShowingModels)
        {
            int verticalIndex;
            if (navigator == null || item == null) return true;

            verticalIndex = navigator.GetVerticalIndex(item);
            return await AsyncTransition(navigator.GetModel(verticalIndex, horizontalIndex), verticalIndex, horizontalIndex, isShowingModels); ;
        }

        private async Awaitable<bool> AsyncTransition(GameObject model, int verticalIndex, int horizontalIndex, bool isShowingModels)
        {
            CancellationToken token = ResetToken();

            StopTransition(model);
            AddTransition(model, verticalIndex, horizontalIndex, isShowingModels);
            StartTransition(model);

            await AwaitTransition(token);
            return !token.IsCancellationRequested;
        }

        public async Awaitable AwaitTransition()
        {
            while (!IsCompleted)
                await Awaitable.NextFrameAsync();
        }

        private async Awaitable AwaitTransition(CancellationToken token)
        {
            if (onTransitionStarted != null && !token.IsCancellationRequested) onTransitionStarted.Invoke();

            while (!IsCompleted && !token.IsCancellationRequested)
                await Awaitable.NextFrameAsync();

            if (onTransitionFinished != null && !token.IsCancellationRequested) onTransitionFinished.Invoke();
        }

        private CancellationToken ResetToken()
        {
            if (transitionsTokenSource != null) transitionsTokenSource.Cancel();
            transitionsTokenSource = new CancellationTokenSource();
            return transitionsTokenSource.Token;
        }


        private void StartTransitions(bool isShowingModels)
        {
            if (navigator == null) return;

            if (navigator is SimpleInventoryUINavigator) StartTransitions((navigator as SimpleInventoryUINavigator).Models, isShowingModels);
            else if (navigator is VerticalInventoryUINavigator) StartTransitions((navigator as VerticalInventoryUINavigator).Models, isShowingModels);
            Debug.LogWarning($"Started {currentTransitions.Count} Page Transitions");
        }

        private void StartTransitions(GameObject[] models, bool isShowingModels)
        {
            if (models == null || models.Length == 0) return;

            for (int i = 0; i < models.Length; i++)
                AddTransition(models[i], 0, i, isShowingModels);

            StartTransitions();
        }

        private void StartTransitions(GameObject[,] models, bool isShowingModels)
        {
            if (models == null || models.GetLength(1) == 0 || models.GetLength(1) == 0) return;

            for (int i = 0; i < models.GetLength(0); i++)
                for (int j = 0; j < models.GetLength(1); j++)
                    AddTransition(models[i, j], i, j, isShowingModels);

            StartTransitions();
        }

        private void StartTransitions()
        {
            if (currentTransitions == null) return;

            foreach (IEnumerator enumerator in currentTransitions.Values)
                StartCoroutine(enumerator);
        }

        private void StartTransition(GameObject model)
        {
            if (model == null || currentTransitions == null) return;

            if (currentTransitions.ContainsKey(model))
                StartCoroutine(currentTransitions[model]);
        }

        private void AddTransition(GameObject model, int verticalIndex, int horizontalIndex, bool isShowingModels)
        {
            if (model == null) return;

            foreach (InventoryUITransition transition in transitions.Where(x => x != null)) {
                AddTransition(model, isShowingModels ? transition.ShowTransition(model, verticalIndex, horizontalIndex) :
                                                       transition.HideTransition(model, verticalIndex, horizontalIndex));
            }
        }

        private void AddTransition(GameObject model, IEnumerator enumerator)
        {
            if(model == null || enumerator == null || currentTransitions == null) return;
            if (!currentTransitions.ContainsKey(model)) currentTransitions.Add(model, null);
            currentTransitions[model] = enumerator;
        }
    }
}