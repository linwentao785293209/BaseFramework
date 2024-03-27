using UnityEngine;
using UnityEngine.Events;

namespace BaseFramework
{
    /// <summary>
    /// 基础抽象输入事件类，实现了IBaseInputEvent接口
    /// </summary>
    public abstract class BaseAbstractBaseInputEvent : IBaseInputEvent
    {
        /// <summary>
        /// 输入事件的UnityEvent，用于添加和移除监听器
        /// </summary>
        public UnityEvent InputEvent { get; } = new UnityEvent();

        /// <summary>
        /// 获取唯一的键，用于标识特定的输入事件
        /// </summary>
        /// <returns>唯一键</returns>
        public abstract string GetUniqueKey();

        /// <summary>
        /// 获取基础输入事件的类型
        /// </summary>
        /// <returns>基础输入事件类型</returns>
        public abstract EBaseInputEventType GetBaseInputEventType();

        /// <summary>
        /// 向输入事件添加监听器
        /// </summary>
        /// <param name="action">要添加的监听器</param>
        public void AddListener(UnityAction action)
        {
            InputEvent.AddListener(action);
        }

        /// <summary>
        /// 从输入事件中移除监听器
        /// </summary>
        /// <param name="action">要移除的监听器</param>
        public void RemoveListener(UnityAction action)
        {
            InputEvent.RemoveListener(action);
        }

        /// <summary>
        /// 执行输入事件的操作
        /// </summary>
        public abstract void ExecuteEvent();
    }
}