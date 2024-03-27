using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    // 多键按下事件类
    public class BaseMultiKeyDownEvent : BaseAbstractBaseInputEvent
    {
        private List<KeyCode> keyCodes; // 多个按键码

        private bool eventTriggered = false; // 事件是否已触发的标志

        // 构造函数，初始化多个按键码
        public BaseMultiKeyDownEvent(List<KeyCode> keyCodes)
        {
            this.keyCodes = keyCodes;
        }

        // 获取事件的唯一标识符，这里将按键码排序后拼接成字符串作为唯一标识符
        public override string GetUniqueKey()
        {
            keyCodes.Sort();

            string uniqueKey = "";
            foreach (KeyCode keyCode in keyCodes)
            {
                uniqueKey += keyCode.ToString();
            }

            return uniqueKey;
        }

        // 获取事件类型，这里是多键按下
        public override EBaseInputEventType GetBaseInputEventType()
        {
            return EBaseInputEventType.MultiKeyDown;
        }

        // 执行事件，检查多个按键是否同时按下，如果是则触发事件
        public override void ExecuteEvent()
        {
            // 如果事件已经触发过，要等所有按键都抬起过才能重新触发
            if (eventTriggered)
            {
                bool allKeysUp = true;
                for (int i = 0; i < keyCodes.Count; i++)
                {
                    // 检查当前按键有没有抬起
                    if (Input.GetKey(keyCodes[i]) || Input.GetKeyDown(keyCodes[i]))
                    {
                        allKeysUp = false;
                    }
                }

                if (allKeysUp)
                {
                    eventTriggered = false;
                }
                else
                {
                    return;
                }
            }

            // 检查多个按键是否同时按下
            bool allKeysPressed = true;
            foreach (KeyCode keyCode in keyCodes)
            {
                if (!Input.GetKey(keyCode) && !Input.GetKeyDown(keyCode))
                {
                    allKeysPressed = false;
                    break;
                }
            }

            // 如果多个按键同时按下，则触发事件
            if (allKeysPressed)
            {
                InputEvent.Invoke();

                eventTriggered = true;
            }
        }
    }
}