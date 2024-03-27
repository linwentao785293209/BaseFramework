using UnityEngine;
using UnityEngine.Events;

namespace BaseFramework
{
    // 单键按下事件类
    public class BaseSingleKeyDownEvent : BaseAbstractBaseInputEvent
    {
        private KeyCode keyCode; // 按键码

        // 构造函数，初始化按键码
        public BaseSingleKeyDownEvent(KeyCode keyCode)
        {
            this.keyCode = keyCode;
        }

        // 获取事件的唯一标识符，这里使用按键码作为唯一标识符
        public override string GetUniqueKey()
        {
            return keyCode.ToString();
        }

        // 获取事件类型，这里是单键按下
        public override EBaseInputEventType GetBaseInputEventType()
        {
            return EBaseInputEventType.SingleKeyDown;
        }

        // 执行事件，检查按键是否按下，如果按下则触发事件
        public override void ExecuteEvent()
        {
            if (Input.GetKeyDown(keyCode))
            {
                InputEvent.Invoke();
            }
        }
    }
}