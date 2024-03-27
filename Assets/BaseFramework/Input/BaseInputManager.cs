using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace BaseFramework
{
    public class BaseInputManager : BaseSingletonInCSharp<BaseInputManager>
    {
        private bool isCheckInput = false;

        private Dictionary<EBaseInputEventType, Dictionary<string, IBaseInputEvent>> inputEventDictionarys =
            new Dictionary<EBaseInputEventType, Dictionary<string, IBaseInputEvent>>();

        private Dictionary<EBaseInputEventType, Dictionary<string, UnityAction>> inputEventToRemoveDictionarys =
            new Dictionary<EBaseInputEventType, Dictionary<string, UnityAction>>();


        public BaseInputManager()
        {
            BaseMonoBehaviourManager.Instance.onUpdate.AddListener(OnUpdateCheckInput);
        }

        public void SetCheckInput(bool isCheckInput)
        {
            this.isCheckInput = isCheckInput;
        }

        private void OnUpdateCheckInput()
        {
            if (!isCheckInput)
                return;

            UpdateCheckInputEvents();
        }

        private void UpdateCheckInputEvents()
        {
            foreach (KeyValuePair<EBaseInputEventType, Dictionary<string, IBaseInputEvent>> dic in
                     inputEventDictionarys)
            {
                foreach (KeyValuePair<string, IBaseInputEvent> item in dic.Value)
                {
                    item.Value.ExecuteEvent();
                }
            }

            foreach (KeyValuePair<EBaseInputEventType, Dictionary<string, UnityAction>> dic in
                     inputEventToRemoveDictionarys)
            {
                foreach (KeyValuePair<string, UnityAction> item in dic.Value)
                {
                    if (!inputEventDictionarys.ContainsKey(dic.Key)) return;

                    if (inputEventDictionarys[dic.Key].ContainsKey(item.Key))
                    {
                        inputEventDictionarys[dic.Key][item.Key].RemoveListener(item.Value);
                    }
                }

                dic.Value.Clear();
            }
        }

        #region AddInputEvent

        public void AddInputEvent(IBaseInputEvent inputEvent, UnityAction action)
        {
            if (!inputEventDictionarys.ContainsKey(inputEvent.GetBaseInputEventType()))
            {
                inputEventDictionarys[inputEvent.GetBaseInputEventType()] =
                    new Dictionary<string, IBaseInputEvent>();
            }

            Dictionary<string, IBaseInputEvent> nowInputEventTypeDic =
                inputEventDictionarys[inputEvent.GetBaseInputEventType()];

            if (!nowInputEventTypeDic.ContainsKey(inputEvent.GetUniqueKey()))
            {
                nowInputEventTypeDic.Add(inputEvent.GetUniqueKey(), inputEvent);
            }

            nowInputEventTypeDic[inputEvent.GetUniqueKey()].AddListener(action);
        }


        public void AddSingleKeyDownInputEvent(KeyCode keyCode, UnityAction action)
        {
            IBaseInputEvent inputEvent = new BaseSingleKeyDownEvent(keyCode);

            AddInputEvent(inputEvent, action);
        }

        public void AddSingleKeyHoldInputEvent(KeyCode keyCode, float holdTime, UnityAction action,
            bool isNeedKeyUpAgain = true)
        {
            IBaseInputEvent inputEvent = new BaseSingleKeyHoldEvent(keyCode, holdTime, isNeedKeyUpAgain);

            AddInputEvent(inputEvent, action);
        }

        public void AddSingleKeyUpInputEvent(KeyCode keyCode, UnityAction action)
        {
            IBaseInputEvent inputEvent = new BaseSingleKeyUpEvent(keyCode);

            AddInputEvent(inputEvent, action);
        }

        public void AddMultiKeyHoldInputEvent(List<KeyCode> keyCodes, float holdTime, UnityAction action,
            bool isNeedKeyUpAgain = true)
        {
            IBaseInputEvent inputEvent = new BaseMultiKeyHoldEvent(keyCodes, holdTime, isNeedKeyUpAgain);

            AddInputEvent(inputEvent, action);
        }

        public void AddMultiKeyDownInputEvent(List<KeyCode> keyCodes, UnityAction action)
        {
            IBaseInputEvent inputEvent = new BaseMultiKeyDownEvent(keyCodes);

            AddInputEvent(inputEvent, action);
        }

        public void AddComboKeyDownInputEvent(List<KeyCode> keyCodes, UnityAction action,
            float comboTimeThreshold = BaseInputConst.defaultComboTimeThreshold)
        {
            IBaseInputEvent inputEvent = new BaseComboKeyDownEvent(keyCodes, comboTimeThreshold);

            AddInputEvent(inputEvent, action);
        }

        #endregion


        #region Remove

        public void RemoveInputEvent(IBaseInputEvent inputEvent, UnityAction action)
        {
            if (!inputEventDictionarys.ContainsKey(inputEvent.GetBaseInputEventType()))
            {
                return;
            }

            Dictionary<string, IBaseInputEvent> nowInputEventTypeDic =
                inputEventDictionarys[inputEvent.GetBaseInputEventType()];

            if (!nowInputEventTypeDic.ContainsKey(inputEvent.GetUniqueKey()))
            {
                return;
            }

            // 不直接移除 避免边遍历边移除 而是记录到字典中
            // nowInputEventTypeDic[inputEvent.GetUniqueKey()].RemoveListener(action);

            if (!inputEventToRemoveDictionarys.ContainsKey(inputEvent.GetBaseInputEventType()))
            {
                inputEventToRemoveDictionarys.Add(inputEvent.GetBaseInputEventType(),
                    new Dictionary<string, UnityAction>());
            }

            inputEventToRemoveDictionarys[inputEvent.GetBaseInputEventType()].Add(inputEvent.GetUniqueKey(), action);
        }

        public void RemoveSingleKeyDownInputEvent(KeyCode keyCode, UnityAction action)
        {
            IBaseInputEvent inputEvent = new BaseSingleKeyDownEvent(keyCode);

            RemoveInputEvent(inputEvent, action);
        }

        public void RemoveSingleKeyHoldInputEvent(KeyCode keyCode, float holdTime, UnityAction action,
            bool isNeedKeyUpAgain = true)
        {
            IBaseInputEvent inputEvent = new BaseSingleKeyHoldEvent(keyCode, holdTime, isNeedKeyUpAgain);

            RemoveInputEvent(inputEvent, action);
        }

        public void RemoveSingleKeyUpInputEvent(KeyCode keyCode, UnityAction action)
        {
            IBaseInputEvent inputEvent = new BaseSingleKeyUpEvent(keyCode);

            RemoveInputEvent(inputEvent, action);
        }

        public void RemoveMultiKeyHoldInputEvent(List<KeyCode> keyCodes, float holdTime, UnityAction action,
            bool isNeedKeyUpAgain = true)
        {
            IBaseInputEvent inputEvent = new BaseMultiKeyHoldEvent(keyCodes, holdTime, isNeedKeyUpAgain);

            RemoveInputEvent(inputEvent, action);
        }

        public void RemoveMultiKeyDownInputEvent(List<KeyCode> keyCodes, UnityAction action)
        {
            IBaseInputEvent inputEvent = new BaseMultiKeyDownEvent(keyCodes);

            RemoveInputEvent(inputEvent, action);
        }

        public void RemoveComboKeyDownInputEvent(List<KeyCode> keyCodes, UnityAction action)
        {
            IBaseInputEvent inputEvent = new BaseComboKeyDownEvent(keyCodes);

            RemoveInputEvent(inputEvent, action);
        }

        #endregion
    }
}