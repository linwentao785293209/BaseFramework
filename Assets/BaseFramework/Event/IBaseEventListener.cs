using UnityEngine.Events;

// 事件监听器接口
public interface IBaseEventListener
{

}

// 泛型事件监听器
public class BaseEventListener<T> : IBaseEventListener
{
    public UnityAction<T> actions; // 存储事件的委托函数

    // 构造函数，用于添加事件监听器
    public BaseEventListener(UnityAction<T> action)
    {
        actions += action; // 添加事件监听器
    }
}

// 非泛型事件监听器
public class BaseEventListener : IBaseEventListener
{
    public UnityAction actions; // 存储事件的委托函数

    // 构造函数，用于添加事件监听器
    public BaseEventListener(UnityAction action)
    {
        actions += action; // 添加事件监听器
    }
}