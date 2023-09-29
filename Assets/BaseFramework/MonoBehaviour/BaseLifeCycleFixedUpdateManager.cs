/// <summary>
/// BaseLifeCycleFixedUpdateManager 继承自 BaseLifeCycleManager 类，用于管理 FixedUpdate 生命周期事件。
/// </summary>
public class BaseLifeCycleFixedUpdateManager : BaseLifeCycleManager
{
    // Unity 的 FixedUpdate 方法会在固定的时间间隔内调用，这里重写了它。
    private void FixedUpdate()
    {
        // 调用基类的 InvokeEvent 方法，触发 FixedUpdate 生命周期事件。
        // 这会通知所有订阅了事件的监听器，以便执行与 FixedUpdate 生命周期相关的操作。
        InvokeEvent();
    }
}
