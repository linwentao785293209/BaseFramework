/// <summary>
/// BaseLifeCycleUpdateManager 继承自 BaseLifeCycleManager 类，用于管理 OnUpdate 生命周期事件。
/// </summary>
public class BaseLifeCycleUpdateManager : BaseLifeCycleManager
{
    // Unity 的 OnUpdate 方法会在每一帧调用，这里重写了它。
    private void Update()
    {
        // 调用基类的 InvokeEvent 方法，触发生命周期事件。
        // 这会通知所有订阅了事件的监听器，以便执行与 OnUpdate 生命周期相关的操作。
        InvokeEvent();
    }
}
