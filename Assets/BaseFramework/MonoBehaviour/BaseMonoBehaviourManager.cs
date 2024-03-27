using System.Collections.Generic;

namespace BaseFramework
{
    /// <summary>
    /// BaseMonoBehaviourManager ��һ��MonoBehaviour��Ϊ�������ĵ����ࡣ
    /// </summary>
    public class BaseMonoBehaviourManager : BaseSingletonInMonoBehaviour<BaseMonoBehaviourManager>
    {
        // �洢���������¼����������ֵ�
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
            // ��Ӹ������������¼����������ֵ���
            AddEventToDictionary("onUpdate", onUpdate);
            AddEventToDictionary("onFixedUpdate", onFixedUpdate);
            AddEventToDictionary("onLateUpdate", onLateUpdate);
            AddEventToDictionary("onDestroy", onDestroy);
        }

        // ����¼����������ֵ���
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

            // ����ֵ�
            eventHandlerDictionary.Clear();
        }
    }
}