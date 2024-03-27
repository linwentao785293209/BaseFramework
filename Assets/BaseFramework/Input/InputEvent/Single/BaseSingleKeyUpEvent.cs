using UnityEngine;

namespace BaseFramework
{
    // 单键抬起事件类
    public class BaseSingleKeyUpEvent : BaseAbstractBaseInputEvent
    {
        private KeyCode keyCode; // 按键码

        // 构造函数，初始化按键码
        public BaseSingleKeyUpEvent(KeyCode keyCode)
        {
            this.keyCode = keyCode;
        }

        // 获取事件的唯一标识符，这里使用按键码作为唯一标识符
        public override string GetUniqueKey()
        {
            return keyCode.ToString();
        }

        // 获取事件类型，这里是单键抬起
        public override EBaseInputEventType GetBaseInputEventType()
        {
            return EBaseInputEventType.SingleKeyUp;
        }

        // 执行事件，检查按键是否抬起，如果抬起则触发事件
        public override void ExecuteEvent()
        {
            if (Input.GetKeyUp(keyCode))
            {
                InputEvent.Invoke();
            }
        }
    }
}