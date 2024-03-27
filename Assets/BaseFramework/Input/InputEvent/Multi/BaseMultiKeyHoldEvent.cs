using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    // 多键长按事件类
    public class BaseMultiKeyHoldEvent : BaseAbstractBaseInputEvent
    {
        private List<KeyCode> keyCodes; // 多个按键码
        private float holdTime; // 长按时间
        private bool[] isHolding; // 是否正在长按
        private float[] holdTimers; // 长按计时器
        private bool eventTriggered = false; // 事件是否已触发的标志

        private bool isNeedKeyUpAgain = true;

        public BaseMultiKeyHoldEvent(List<KeyCode> keyCodes, float holdTime,bool isNeedKeyUpAgain = true)
        {
            this.keyCodes = keyCodes;
            this.holdTime = holdTime;
            this.isNeedKeyUpAgain = isNeedKeyUpAgain;
            isHolding = new bool[keyCodes.Count];
            holdTimers = new float[keyCodes.Count];
        }

        public override string GetUniqueKey()
        {
            keyCodes.Sort();

            string uniqueKey = "";
            foreach (KeyCode keyCode in keyCodes)
            {
                uniqueKey += keyCode.ToString();
            }

            uniqueKey += holdTime;
            uniqueKey += isNeedKeyUpAgain;
            return uniqueKey;
        }

        public override EBaseInputEventType GetBaseInputEventType()
        {
            return EBaseInputEventType.MultiKeyHold;
        }

        public override void ExecuteEvent()
        {
            // 如果事件已经触发过，要等三个键都抬起过才能重新触发
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


            // 标记是否所有按键都被按下
            bool allKeysPressed = true;

            // 遍历所有按键
            for (int i = 0; i < keyCodes.Count; i++)
            {
                // 检查当前按键是否没有被按下
                if (!Input.GetKey(keyCodes[i]))
                {
                    // 如果有按键没有被按下，则不满足所有按键被按下的条件
                    allKeysPressed = false;

                    // 标记当前按键不再被长按
                    isHolding[i] = false;

                    // 重置当前按键的长按计时器
                    holdTimers[i] = 0f;
                }
                // 如果当前按键被按下并且之前未被标记为正在长按
                else if (!isHolding[i])
                {
                    // 标记当前按键为正在长按
                    isHolding[i] = true;

                    // 重置当前按键的长按计时器
                    holdTimers[i] = 0f;
                }

                // 如果当前按键正在长按
                if (isHolding[i])
                {
                    // 增加当前按键的长按时间
                    holdTimers[i] += Time.deltaTime;

                    // 如果当前按键的长按时间未达到设定的长按时间
                    if (holdTimers[i] < holdTime)
                    {
                        // 不满足所有按键被按下的条件
                        allKeysPressed = false;
                    }
                }
            }

            // 如果所有按键都被按下
            if (allKeysPressed)
            {
                // 触发输入事件
                InputEvent.Invoke();

                if (isNeedKeyUpAgain)
                {
                    // 标记事件已经触发过 要抬起才能重新触发
                    eventTriggered = true;
                }


                // 重置所有长按计时器和标志位，以便重新检查是否满足触发条件
                ResetHoldTimersAndFlags();
            }
        }

        // 重置所有长按计时器和标志位
        private void ResetHoldTimersAndFlags()
        {
            for (int i = 0; i < keyCodes.Count; i++)
            {
                isHolding[i] = false;
                holdTimers[i] = 0f;
            }
        }
    }
}