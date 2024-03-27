using UnityEngine;
using UnityEngine.Events;

namespace BaseFramework
{
    // 单键长按事件类
    public class BaseSingleKeyHoldEvent : BaseAbstractBaseInputEvent
    {
        private KeyCode keyCode; // 按键码
        private float holdTime; // 长按时间
        private bool isNeedKeyUpAgain; // 是否需要再次抬起按键才能触发事件

        private bool isHolding = false; // 是否正在长按
        private float holdTimer = 0f; // 长按计时器

        // 构造函数，初始化按键码、长按时间和是否需要再次抬起按键
        public BaseSingleKeyHoldEvent(KeyCode keyCode, float holdTime, bool isNeedKeyUpAgain = false)
        {
            this.keyCode = keyCode;
            this.holdTime = holdTime;
            this.isNeedKeyUpAgain = isNeedKeyUpAgain;
        }

        // 获取事件的唯一标识符，这里使用按键码、长按时间和是否需要再次抬起按键作为唯一标识符
        public override string GetUniqueKey()
        {
            return keyCode.ToString() + holdTime + isNeedKeyUpAgain;
        }

        // 获取事件类型，这里是单键长按
        public override EBaseInputEventType GetBaseInputEventType()
        {
            return EBaseInputEventType.SingleKeyHold;
        }

        // 执行事件，检查按键是否按下并且长按时间是否达到要求，如果满足则触发事件
        public override void ExecuteEvent()
        {
            if (Input.GetKeyDown(keyCode))
            {
                isHolding = true;
                holdTimer = 0f;
            }

            if (Input.GetKey(keyCode))
            {
                if (isHolding)
                {
                    holdTimer += Time.deltaTime;
                    if (holdTimer >= holdTime)
                    {
                        InputEvent.Invoke();

                        // 需要重新抬起才能再次触发
                        if (isNeedKeyUpAgain)
                        {
                            isHolding = false;
                        }
                        // 不需要重新抬起重置计时器就行
                        else
                        {
                            holdTimer = 0f;
                        }
                    }
                }
            }
            else
            {
                isHolding = false;
            }
        }
    }
}