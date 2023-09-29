/// <summary>
/// BaseLifeCycleLateUpdateManager 继承自 BaseLifeCycleManager 类，用于管理 LateUpdate 生命周期事件。
/// </summary>
public class BaseLifeCycleLateUpdateManager : BaseLifeCycleManager
{
    // Unity 的 LateUpdate 方法会在每一帧的最后调用，这里重写了它。
    private void LateUpdate()
    {
        // 调用基类的 InvokeEvent 方法，触发 LateUpdate 生命周期事件。
        // 这会通知所有订阅了事件的监听器，以便执行与 LateUpdate 生命周期相关的操作。
        InvokeEvent();
    }
}
