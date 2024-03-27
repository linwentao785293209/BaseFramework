namespace BaseFramework
{
    /// <summary>
    /// 基础输入事件类型枚举，用于标识不同类型的输入事件
    /// </summary>
    public enum EBaseInputEventType
    {
        /// <summary>
        /// 单个按键按下事件
        /// </summary>
        SingleKeyDown,

        /// <summary>
        /// 单个按键长按事件
        /// </summary>
        SingleKeyHold,

        /// <summary>
        /// 单个按键松开事件
        /// </summary>
        SingleKeyUp,

        /// <summary>
        /// 多个按键长按事件
        /// </summary>
        MultiKeyHold,

        /// <summary>
        /// 多个按键同时按下事件
        /// </summary>
        MultiKeyDown,

        /// <summary>
        /// 组合按键按下事件
        /// </summary>
        ComboKeyDown
    }
}