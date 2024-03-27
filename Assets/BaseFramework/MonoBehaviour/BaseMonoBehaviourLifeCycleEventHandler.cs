using System.Collections.Generic;
using UnityEngine.Events;

namespace BaseFramework
{
    public class BaseMonoBehaviourLifeCycleEventHandler
    {
        private List<UnityAction> lifecycleEventHandlers = new List<UnityAction>();

        public void AddListener(UnityAction action)
        {
            lifecycleEventHandlers.Add(action);
        }

        public void RemoveListener(UnityAction action)
        {
            lifecycleEventHandlers.Remove(action);
        }

        public void Trigger()
        {
            for (int i = lifecycleEventHandlers.Count - 1; i >= 0; i--)
            {
                lifecycleEventHandlers[i]?.Invoke();
            }
        }

        public void Clear()
        {
            lifecycleEventHandlers.Clear();
        }
    }
}