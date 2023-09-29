using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// BaseLifeCycleManager 是一个用于管理生命周期事件的基类。
/// </summary>
public class BaseLifeCycleManager : MonoBehaviour
{
    private event UnityAction lifeCycleEvent; // 声明一个 UnityAction 类型的事件。

    /// <summary>
    /// 调用生命周期事件。
    /// </summary>
    public void InvokeEvent()
    {
        lifeCycleEvent?.Invoke(); // 如果生命周期事件不为空，则触发它。
    }

    /// <summary>
    /// 添加监听器到生命周期事件。
    /// </summary>
    /// <param name="listener">要添加的监听器。</param>
    public void AddListener(UnityAction listener)
    {
        if (listener != null) // 检查监听器是否为非空。
        {
            lifeCycleEvent += listener; // 向生命周期事件添加监听器。
        }
    }

    /// <summary>
    /// 从生命周期事件中移除监听器。
    /// </summary>
    /// <param name="listener">要移除的监听器。</param>
    public void RemoveListener(UnityAction listener)
    {
        if (listener != null) // 检查监听器是否为非空。
        {
            lifeCycleEvent -= listener; // 从生命周期事件中移除监听器。
        }
    }

    /// <summary>
    /// 当对象销毁时，移除所有监听器以防止内存泄漏。
    /// </summary>
    private void OnDestroy()
    {
        lifeCycleEvent = null; // 清空生命周期事件，以释放对监听器的引用。
    }
}
