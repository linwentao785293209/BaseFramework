using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// 资源管理器基类
public class BaseResourceManager : BaseSingletonInCSharp<BaseResourceManager>
{
    // 同步加载资源
    public T Load<T>(string resourcePath) where T : Object
    {
        T resource = Resources.Load<T>(resourcePath);

        // 如果对象是一个GameObject类型的，实例化后返回
        if (resource is GameObject)
        {
            return GameObject.Instantiate(resource);
        }
        else
        {
            return resource;
        }
    }

    // 异步加载资源
    public void LoadAsync<T>(string resourcePath, UnityAction<T> callback) where T : Object
    {
        BaseMonoBehaviourManager.Instance.StartBaseCoroutine(LoadAsyncCoroutine(resourcePath, callback));
    }

    // 协程函数，用于异步加载资源
    private IEnumerator LoadAsyncCoroutine<T>(string resourcePath, UnityAction<T> callback) where T : Object
    {
        ResourceRequest resourceRequest = Resources.LoadAsync<T>(resourcePath);
        yield return resourceRequest;

        T resource;

        if (resourceRequest.asset is GameObject)
        {
            resource = GameObject.Instantiate(resourceRequest.asset) as T;
        }
        else
        {
            resource = resourceRequest.asset as T;
        }

        // 执行回调函数
        callback?.Invoke(resource);
    }
}
