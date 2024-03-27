using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    // 组合键按下事件类
    public class BaseComboKeyDownEvent : BaseAbstractBaseInputEvent
    {
        private List<KeyCode> keyCodes; // 组合按键码
        private float comboTimeThreshold = BaseInputConst.defaultComboTimeThreshold; // 组合键时间阈值
        private Dictionary<KeyCode, float> keyPressTimes; // 记录按键按下的时间

        private bool eventTriggered = false; // 事件是否已触发的标志

        // 构造函数，初始化组合按键码和组合键时间阈值
        public BaseComboKeyDownEvent(List<KeyCode> keyCodes,
            float comboTimeThreshold = BaseInputConst.defaultComboTimeThreshold)
        {
            this.keyCodes = keyCodes;
            this.comboTimeThreshold = comboTimeThreshold;
            keyPressTimes = new Dictionary<KeyCode, float>();
        }

        // 获取事件的唯一标识符，这里将组合按键码排序后拼接成字符串，并加上组合键时间阈值作为唯一标识符
        public override string GetUniqueKey()
        {
            string uniqueKey = "";
            foreach (KeyCode keyCode in keyCodes)
            {
                uniqueKey += keyCode.ToString();
            }

            uniqueKey += comboTimeThreshold;
            return uniqueKey;
        }

        // 获取事件类型，这里是组合键按下
        public override EBaseInputEventType GetBaseInputEventType()
        {
            return EBaseInputEventType.ComboKeyDown;
        }

        // 执行事件，检查组合按键是否同时按下，如果是则触发事件
        public override void ExecuteEvent()
        {
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

            // 遍历按键列表
            foreach (KeyCode keyCode in keyCodes)
            {
                // 检查是否有按键被按下
                if (Input.GetKeyDown(keyCode))
                {
                    // 记录按键按下的时间
                    keyPressTimes[keyCode] = Time.time;
                }
            }

            // 检查按键按下的顺序和时间间隔
            bool comboPressed = CheckComboPressed();

            // 如果组合按键被按下
            if (comboPressed)
            {
                // 触发输入事件
                InputEvent.Invoke();

                // 清空按键按下时间记录
                keyPressTimes.Clear();

                // 标记事件已经触发过
                eventTriggered = true;
            }
        }

        // 检查组合按键是否被按下
        private bool CheckComboPressed()
        {
            // 获取当前时间
            float currentTime = Time.time;

            // 按照组合按键的顺序检查每个按键的按下时间和时间间隔
            for (int i = 1; i < keyCodes.Count; i++)
            {
                // 获取当前按键和前一个按键的按键码
                KeyCode nowkeyCode = keyCodes[i];
                KeyCode frontkeyCode = keyCodes[i - 1];

                // 检查当前按键和前一个按键是否都记录了按键按下时间
                if (!keyPressTimes.ContainsKey(frontkeyCode) || !keyPressTimes.ContainsKey(nowkeyCode))
                {
                    return false;
                }

                // 计算当前按键和前一个按键的按键按下时间间隔
                float duration = keyPressTimes[nowkeyCode] - keyPressTimes[frontkeyCode];

                // 如果按键按下的时间间隔在组合键时间阈值内，继续检查下一个按键
                if (duration >= 0 && duration <= comboTimeThreshold)
                {
                    continue;
                }
                // 如果按键按下的时间间隔超出了组合键时间阈值，返回 false
                else
                {
                    return false;
                }
            }

            // 如果所有按键的按键按下时间间隔都在组合键时间阈值内，返回 true
            return true;
        }
    }
}