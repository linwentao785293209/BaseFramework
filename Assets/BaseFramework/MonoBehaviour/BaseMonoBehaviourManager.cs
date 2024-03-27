using System.Collections.Generic;

namespace BaseFramework
{
    /// <summary>
    /// BaseMonoBehaviourManager 是一个MonoBehaviour行为管理器的单例类。
    /// </summary>
    public class BaseMonoBehaviourManager : BaseSingletonInMonoBehaviour<BaseMonoBehaviourManager>
    {
        // 存储生命周期事件处理器的字典
        private Dictionary<string, BaseMonoBehaviourLifeCycleEventHandler> eventHandlerDictionary =
            new Dictionary<string, BaseMonoBehaviourLifeCycleEventHandler>();


        public BaseMonoBehaviourLifeCycleEventHandler onUpdate = new BaseMonoBehaviourLifeCycleEventHandler();
        public BaseMonoBehaviourLifeCycleEventHandler onFixedUpdate = new BaseMonoBehaviourLifeCycleEventHandler();
        public BaseMonoBehaviourLifeCycleEventHandler onLateUpdate = new BaseMonoBehaviourLifeCycleEventHandler();
        public BaseMonoBehaviourLifeCycleEventHandler onDestroy = new BaseMonoBehaviourLifeCycleEventHandler();


        protected virtual void Awake()
        {
            Init();
        }

        void Init()
        {
            // 添加各个生命周期事件处理器到字典中
            AddEventToDictionary("onUpdate", onUpdate);
            AddEventToDictionary("onFixedUpdate", onFixedUpdate);
            AddEventToDictionary("onLateUpdate", onLateUpdate);
            AddEventToDictionary("onDestroy", onDestroy);
        }

        // 添加事件处理器到字典中
        protected virtual void AddEventToDictionary(string key, BaseMonoBehaviourLifeCycleEventHandler handler)
        {
            if (!eventHandlerDictionary.ContainsKey(key))
            {
                eventHandlerDictionary.Add(key, handler);
            }
        }

        protected virtual void Update()
        {
            onUpdate.Trigger();
        }

        protected virtual void FixedUpdate()
        {
            onFixedUpdate.Trigger();
        }

        protected virtual void LateUpdate()
        {
            onLateUpdate.Trigger();
        }

        protected virtual void OnDestroy()
        {
            onDestroy.Trigger();

            // 清空字典
            eventHandlerDictionary.Clear();
        }
    }
}