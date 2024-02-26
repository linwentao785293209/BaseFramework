using System.Collections.Generic;
using UnityEngine.Events;

// 事件管理器
public class BaseEventManager : BaseSingletonInCSharp<BaseEventManager>
{
    // key —— 事件的名字（比如：怪物死亡，玩家死亡，通关等等）
    // value —— 对应的是监听这个事件对应的委托函数们
    private Dictionary<string, IBaseEventListener> baseEventListenerDictionary = new Dictionary<string, IBaseEventListener>();

    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="name">事件的名字</param>
    /// <param name="action">准备用来处理事件的委托函数</param>
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        // 有没有对应的事件监听
        // 有的情况
        if (baseEventListenerDictionary.ContainsKey(name))
        {
            (baseEventListenerDictionary[name] as BaseEventListener<T>).actions += action;
        }
        // 没有的情况
        else
        {
            baseEventListenerDictionary.Add(name, new BaseEventListener<T>(action));
        }
    }

    /// <summary>
    /// 监听不需要参数传递的事件
    /// </summary>
    /// <param name="name">事件的名字</param>
    /// <param name="action">用来处理事件的委托函数</param>
    public void AddEventListener(string name, UnityAction action)
    {
        // 有没有对应的事件监听
        // 有的情况
        if (baseEventListenerDictionary.ContainsKey(name))
        {
            (baseEventListenerDictionary[name] as BaseEventListener).actions += action;
        }
        // 没有的情况
        else
        {
            baseEventListenerDictionary.Add(name, new BaseEventListener(action));
        }
    }

    /// <summary>
    /// 移除对应的事件监听
    /// </summary>
    /// <param name="name">事件的名字</param>
    /// <param name="action">对应之前添加的委托函数</param>
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (baseEventListenerDictionary.ContainsKey(name))
            (baseEventListenerDictionary[name] as BaseEventListener<T>).actions -= action;
    }

    /// <summary>
    /// 移除不需要参数的事件
    /// </summary>
    /// <param name="name">事件的名字</param>
    /// <param name="action">对应之前添加的委托函数</param>
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (baseEventListenerDictionary.ContainsKey(name))
            (baseEventListenerDictionary[name] as BaseEventListener).actions -= action;
    }

    /// <summary>
    /// 事件触发
    /// </summary>
    /// <param name="name">哪一个名字的事件触发了</param>
    public void EventTrigger<T>(string name, T info)
    {
        // 有没有对应的事件监听
        // 有的情况
        if (baseEventListenerDictionary.ContainsKey(name))
        {
            // 使用 null 条件运算符安全地触发事件
            (baseEventListenerDictionary[name] as BaseEventListener<T>)?.actions?.Invoke(info);
        }
    }

    /// <summary>
    /// 事件触发（不需要参数的）
    /// </summary>
    /// <param name="name">哪一个名字的事件触发了</param>
    public void EventTrigger(string name)
    {
        // 有没有对应的事件监听
        // 有的情况
        if (baseEventListenerDictionary.ContainsKey(name))
        {
            // 使用 null 条件运算符安全地触发事件
            (baseEventListenerDictionary[name] as BaseEventListener)?.actions?.Invoke();
        }
    }

    /// <summary>
    /// 清空事件中心
    /// 主要用在场景切换时
    /// </summary>
    public void Clear()
    {
        baseEventListenerDictionary.Clear();
    }
}