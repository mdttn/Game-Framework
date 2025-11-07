using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions.Items;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class VerticalInventoryUINavigator : InventoryUINavigator
    {
        [Space]
        [SerializeField] private bool canWarpVerticalIndex = true;

        private UnityEvent<int> onVerticalIndexChanged;
        private UnityEvent<GameObject[,]> onModelsChanged;
        private UnityEvent<int, int, GameObject, VerticalInventoryUINavigator> onUpdateModel;

        private OverrideablePressInput nextVerticalPressInput;
        private OverrideablePressInput previousVerticalPressInput;

        protected int verticalIndex;
        public  int VerticalIndex => verticalIndex;

        public const string NEXT_VERTICAL_INPUT_NAME     = "Next Vertical Navigator Input";
        public const string PREVIOUS_VERTICAL_INPUT_NAME = "Previous Vertical Navigator Input";

        private Item      [,] items;
        private GameObject[,] models;

        public Item      [,] Items  => items;
        public GameObject[,] Models => models;

        protected override void Awake()
        {
            base.Awake();

            verticalIndex = 0;
            items = new Item[0, 0];

            onVerticalIndexChanged = new UnityEvent<int>();
            onModelsChanged = new UnityEvent<GameObject[,]>();
            onUpdateModel = new UnityEvent<int, int, GameObject, VerticalInventoryUINavigator>();

           nextVerticalPressInput = GetNextVerticalInput();
            previousVerticalPressInput = GetPreviousVerticalInput();

            nextVerticalPressInput.Enable();
            previousVerticalPressInput.Enable();
        }


        protected override void Start()
        {
            AddOnVerticalIndexChangedListener(OnIndexChanged);
            AddOnModelsChangedListener(OnModelsChanged);
            base.Start();
        }

        protected sealed override void DisableInputs()
        {
            base.DisableInputs();
            if (nextVerticalPressInput != null)  nextVerticalPressInput.Disable(); 
            if (previousVerticalPressInput != null) previousVerticalPressInput.Disable();
        }
        protected sealed override void EnableInputs()
        {
            base.EnableInputs();
            if (nextVerticalPressInput != null) nextVerticalPressInput.Enable();
            if (previousVerticalPressInput != null) previousVerticalPressInput.Enable();
        }

        public void AddOnUpdateModelListener     (UnityAction<int, int, GameObject, VerticalInventoryUINavigator> action)
        {
            if(onUpdateModel != null && action != null)
                onUpdateModel.AddListener(action);
        }
        public void RemoveOnUpdateModelListener  (UnityAction<int, int, GameObject, VerticalInventoryUINavigator> action)
        {
            if (onUpdateModel != null && action != null)
                onUpdateModel.RemoveListener(action);
        }

        public void AddOnModelsChangedListener   (UnityAction<GameObject[,]> action)
        {
            if (onModelsChanged != null && action != null)
                onModelsChanged.AddListener(action);
        }
        public void RemoveOnModelsChangedListener(UnityAction<GameObject[,]> action)
        {
            if (onModelsChanged != null && action != null)
                onModelsChanged.RemoveListener(action);
        }

        public void AddOnVerticalIndexChangedListener(UnityAction<int> action)
        {
            if (onVerticalIndexChanged != null && action != null)
                onVerticalIndexChanged.AddListener(action);
        }

        public void RemoveOnVerticalIndexChangedListener(UnityAction<int> action)
        {
            if (onVerticalIndexChanged != null && action != null)
                onVerticalIndexChanged.RemoveListener(action);
        }


        private void OnModelsChanged(GameObject[,] models)
        {
            if (models == null) return;

            for (int i = 0; i < models.GetLength(0); i++)
                for (int j = 0; j < models.GetLength(1); j++)
                    if (models[i, j] != null) SetModelParent(models[i, j]);
        }

        protected sealed override void OnUpdateItemModel()
        {
            if (models == null) return;

            for (int i = 0; i < models.GetLength(0); i++)
                for (int j = 0; j < models.GetLength(1); j++)
                    if (models[i, j] != null && onUpdateModel != null)
                        onUpdateModel.Invoke(i, j, models[i, j], this);
        }


        public sealed override void ClearModels()
        {
            ItemModel.ReturnBorrowedModels(models);
            models= new GameObject[0, 0];
        }

        protected override void OnOpenInventoryUI()
        {
            base.OnOpenInventoryUI();
            UpdateModels();
        }

        public sealed override void UpdateItems() {
            items = GetItems();
        }

        public sealed override void UpdateModels() 
        {
            ItemModel.ReturnBorrowedModels(models);
            models = ItemModel.GetModels(this);
            if(onModelsChanged != null && models != null) { onModelsChanged.Invoke(models); }
        }

        protected sealed override void UpdateInput() 
        {
            base.UpdateInput();

            if (nextVerticalPressInput != null && previousVerticalPressInput != null) {
                nextVerticalPressInput.Update();
                previousVerticalPressInput.Update();

                if      (nextVerticalPressInput.Value)     IncrementVerticalIndex();
                else if (previousVerticalPressInput.Value) DecrementVerticalIndex();
            }
        }

        public override int GetHorizontalIndex(Item item) {
            return GetItemIndex(item, false);
        }

        public virtual int GetVerticalIndex(Item item) {
            return GetItemIndex(item, true);
        }

        private int GetItemIndex(Item item, bool getVerticalIndex)
        {
            if (items == null || item == null) return -1;

            for(int i = 0; i < items.GetLength(0); i++)
                for(int j = 0; i < items.GetLength(1); j++)
                    if (items[i, j] == item) return getVerticalIndex ? i : j; 

            return -1;
        }

        private void DecrementVerticalIndex()  
        {
            verticalIndex--;
            ClampDecrementVerticalIndex(ref verticalIndex, canWarpVerticalIndex);
            if(onVerticalIndexChanged != null) onVerticalIndexChanged.Invoke(verticalIndex);
        }

        private void IncrementVerticalIndex()  
        {
            verticalIndex++;
            ClampIncrementVerticalIndex(ref verticalIndex, canWarpVerticalIndex);
            if (onVerticalIndexChanged != null) onVerticalIndexChanged.Invoke(verticalIndex);
        }

        protected virtual void ClampIncrementVerticalIndex(ref int verticalIndex, bool canWarpVerticalIndex)
        {
            int maxVerticalIndex = GetMaxVerticalIndex();
           
            if (verticalIndex >= maxVerticalIndex)
            {
               if(canWarpVerticalIndex) verticalIndex = 0;
               else                     verticalIndex = maxVerticalIndex - 1;
            }
        }

        protected virtual void ClampDecrementVerticalIndex(ref int verticalIndex, bool canWarpVerticalIndex)
        {
            if (verticalIndex < 0)
            {
               if(canWarpVerticalIndex) verticalIndex = GetMaxVerticalIndex() - 1;
               else                     verticalIndex = 0;
            }
        }

        public bool DoesModelExist(int verticalIndex, int horizontalIndex) {
            return GetModel(verticalIndex, horizontalIndex) != null;
        }

        public GameObject GetModel(int verticalIndex, int horizontalIndex)
        {
            if (models == null || models.GetLength(0) == 0 || models.GetLength(1) == 0) return null;
            if (verticalIndex < 0 || horizontalIndex < 0 || verticalIndex >= models.GetLength(0) || horizontalIndex >= models.GetLength(1)) return null;
            return models[verticalIndex, horizontalIndex];
        }

        public abstract int GetMaxVerticalIndex();
        public abstract Item[,] GetItems();

        public static OverrideablePressInput GetNextVerticalInput() {
            return InputManager.GetOrCreateOverrideablePressInput(NEXT_VERTICAL_INPUT_NAME, KeyboardKey.S, GamepadButton.DpadDown); 
        }


        public OverrideablePressInput GetPreviousVerticalInput() {
            return InputManager.GetOrCreateOverrideablePressInput(PREVIOUS_VERTICAL_INPUT_NAME, KeyboardKey.W, GamepadButton.DpadUp);
        }
    }
}
