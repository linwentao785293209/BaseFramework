using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Internal;

/// <summary>
/// BaseMonoBehaviourManager 是一个MonoBehaviour行为管理器的单例类。
/// </summary>
public class BaseMonoBehaviourManager : BaseSingletonInMonoBehaviour<BaseMonoBehaviourManager>
{
    // 用于存储各种生命周期事件管理器的字典。
    private Dictionary<Type, BaseLifeCycleManager> baseLifeCycleManagerDictionary = new Dictionary<Type, BaseLifeCycleManager>();

    private BaseCoroutineManager baseCoroutineManager;

    protected override void Awake()
    {
        base.Awake();

        Init();
    }

    private void Init()
    {
        InitBaseLifeCycleUpdateManagers();
        InitBaseCoroutineManager();
    }

    private void InitBaseLifeCycleUpdateManagers()
    {
        AddBaseLifeCycleManager<BaseLifeCycleUpdateManager>();
        AddBaseLifeCycleManager<BaseLifeCycleLateUpdateManager>();
        AddBaseLifeCycleManager<BaseLifeCycleFixedUpdateManager>();
    }

    private void InitBaseCoroutineManager()
    {
        GameObject baseCoroutineManagerGameObject = new GameObject("BaseCoroutineManager");

        baseCoroutineManager = baseCoroutineManagerGameObject.AddComponent<BaseCoroutineManager>();

        baseCoroutineManagerGameObject.transform.parent = transform;
    }

    /// <summary>
    /// 向字典中添加生命周期事件管理器。
    /// </summary>
    /// <typeparam name="T">要添加的生命周期事件管理器的类型。</typeparam>
    public void AddBaseLifeCycleManager<T>() where T : BaseLifeCycleManager
    {
        Type baseLifeCycleManagerType = typeof(T);

        if (!baseLifeCycleManagerDictionary.ContainsKey(baseLifeCycleManagerType))
        {
            // 创建一个新的 GameObject 作为生命周期事件管理器的容器。
            GameObject baseLifeCycleManagerGameObject = new GameObject(baseLifeCycleManagerType.Name);

            // 在容器上添加指定类型的生命周期事件管理器组件。
            BaseLifeCycleManager nowBaseLifeCycleManager = baseLifeCycleManagerGameObject.AddComponent<T>();

            // 设置容器为当前对象的子对象，以便在场景中组织。
            baseLifeCycleManagerGameObject.transform.parent = transform;

            // 将生命周期事件管理器添加到字典中。
            baseLifeCycleManagerDictionary.Add(baseLifeCycleManagerType, nowBaseLifeCycleManager);

            Debug.Log($"{baseLifeCycleManagerType.Name} 在字典中成功添加");
        }
        else
        {
            Debug.Log($"{baseLifeCycleManagerType.Name} 在字典中已存在");
        }
    }

    /// <summary>
    /// 从字典中移除生命周期事件管理器。
    /// </summary>
    /// <typeparam name="T">要移除的生命周期事件管理器的类型。</typeparam>
    public void RemoveBaseLifeCycleManager<T>() where T : BaseLifeCycleManager
    {
        Type baseLifeCycleManagerType = typeof(T);

        if (baseLifeCycleManagerDictionary.TryGetValue(baseLifeCycleManagerType, out BaseLifeCycleManager nowBaseLifeCycleManager))
        {
            // 销毁生命周期事件管理器的 GameObject。
            Destroy(nowBaseLifeCycleManager.gameObject);

            // 从字典中移除生命周期事件管理器。
            baseLifeCycleManagerDictionary.Remove(baseLifeCycleManagerType);

            Debug.Log($"{baseLifeCycleManagerType.Name} 在字典中成功移除");
        }
        else
        {
            Debug.Log($"{baseLifeCycleManagerType.Name} 在字典中不存在");
        }
    }

    /// <summary>
    /// 添加监听器到指定类型的生命周期事件管理器。
    /// </summary>
    /// <typeparam name="T">要添加监听器的生命周期事件管理器的类型。</typeparam>
    /// <param name="listener">要添加的监听器。</param>
    public void AddBaseLifeCycleManagerListener<T>(UnityAction listener) where T : BaseLifeCycleManager
    {
        Type baseLifeCycleManagerType = typeof(T);

        if (baseLifeCycleManagerDictionary.TryGetValue(baseLifeCycleManagerType, out BaseLifeCycleManager manager))
        {
            // 调用生命周期事件管理器的方法，添加监听器。
            manager.AddListener(listener);
        }
        else
        {
            Debug.Log($"{baseLifeCycleManagerType.Name} 在字典中不存在");
        }
    }

    /// <summary>
    /// 从指定类型的生命周期事件管理器中移除监听器。
    /// </summary>
    /// <typeparam name="T">要移除监听器的生命周期事件管理器的类型。</typeparam>
    /// <param name="listener">要移除的监听器。</param>
    public void RemoveBaseLifeCycleManagerListener<T>(UnityAction listener) where T : BaseLifeCycleManager
    {
        Type baseLifeCycleManagerType = typeof(T);

        if (baseLifeCycleManagerDictionary.TryGetValue(baseLifeCycleManagerType, out BaseLifeCycleManager manager))
        {
            // 调用生命周期事件管理器的方法，移除监听器。
            manager.RemoveListener(listener);
        }
        else
        {
            Debug.Log($"{baseLifeCycleManagerType.Name} 在字典中不存在");
        }
    }

    public Coroutine StartBaseCoroutine(IEnumerator routine)
    {
        return baseCoroutineManager.StartCoroutine(routine);
    }

    public void StopBaseCoroutine(IEnumerator routine)
    {
        baseCoroutineManager.StopCoroutine(routine);
    }

    public void StopBaseCoroutine(Coroutine routine)
    {
        baseCoroutineManager.StopCoroutine(routine);
    }
}
