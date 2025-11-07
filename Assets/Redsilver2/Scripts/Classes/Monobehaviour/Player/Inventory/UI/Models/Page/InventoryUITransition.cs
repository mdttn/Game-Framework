using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class InventoryUITransition : MonoBehaviour {

        [SerializeField] private float transitionDelay;

        [Space]
        [SerializeField] private float exponentialTimeMultiplier;

        [Space]
        [SerializeField] private bool  isExponentialDelay;
        private List<GameObject> operationsValidated;

        private void Awake() {
            operationsValidated = new List<GameObject>();
        }

        public virtual IEnumerator ShowTransition(GameObject model, int verticalIndex, int horizontalIndex) {
            yield return StartCoroutine(Transition(model, verticalIndex, horizontalIndex, true));
        }

        public virtual IEnumerator HideTransition(GameObject model, int verticalIndex, int horizontalIndex) {
            yield return StartCoroutine(Transition(model, verticalIndex, horizontalIndex, false));
        }

        private IEnumerator Transition(GameObject model , int verticalIndex, int horizontalIndex, bool isShowingModel)
        {
            if (model != null)
            {
                operationsValidated.Add(model);
                yield return new WaitForSeconds(transitionDelay);
                yield return new WaitForSeconds(GetTransitionDelay((verticalIndex == 0 ? 1 : verticalIndex + 1) * (horizontalIndex == 0 ? 1 : horizontalIndex + 1)));

                yield return StartCoroutine(Transition(model, isShowingModel));
                operationsValidated.Remove(model);
            }
        }

        protected abstract IEnumerator Transition(GameObject model, bool isShowingModel);

        public void StopTransitions() {
           if(operationsValidated != null) operationsValidated.Clear();
        }

        public void StopTransition(GameObject modle)
        {
            if(modle != null && operationsValidated != null){
                if(operationsValidated.Contains(modle))
                   operationsValidated.Remove(modle); 
            }
        }

        protected float GetTransitionDelay(int index){
            return transitionDelay * (1 + (isExponentialDelay ?  index * exponentialTimeMultiplier : 0f));
        }

        public bool IsFinished()
        {
            if(operationsValidated == null || operationsValidated.Count == 0) return true;
            return false;
        }
    }
}
