using UnityEngine;
using UnityEngine.Events;

namespace BaseFramework
{
    /// <summary>
    /// 输入事件接口，定义了输入事件的基本结构和功能
    /// </summary>
    public interface IBaseInputEvent
    {
        /// <summary>
        /// 输入事件的触发事件，用于添加和移除监听器
        /// </summary>
        UnityEvent InputEvent { get; }

        /// <summary>
        /// 获取输入事件的唯一键，用于识别不同的输入事件
        /// </summary>
        /// <returns>输入事件的唯一键</returns>
        string GetUniqueKey();

        /// <summary>
        /// 获取输入事件的类型
        /// </summary>
        /// <returns>输入事件的类型</returns>
        EBaseInputEventType GetBaseInputEventType();

        /// <summary>
        /// 添加监听器到输入事件
        /// </summary>
        /// <param name="action">要添加的监听器</param>
        void AddListener(UnityAction action);

        /// <summary>
        /// 从输入事件中移除监听器
        /// </summary>
        /// <param name="action">要移除的监听器</param>
        void RemoveListener(UnityAction action);

        /// <summary>
        /// 执行输入事件，触发所有添加的监听器
        /// </summary>
        void ExecuteEvent();
    }
}