using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 负责管理场景加载的类。
/// </summary>
public class BaseSceneManager : BaseSingletonInCSharp<BaseSceneManager>
{
    /// <summary>
    /// 同步加载场景，并在加载完成后执行回调。
    /// </summary>
    /// <param name="sceneName">要加载的场景名称。</param>
    /// <param name="callback">加载完成后执行的回调函数。</param>
    public void LoadScene(string sceneName, UnityAction callback)
    {
        // 使用SceneManager加载场景
        SceneManager.LoadScene(sceneName);

        // 如果回调不为null，则调用回调
        callback?.Invoke();
    }

    /// <summary>
    /// 异步加载场景，并在加载完成后执行回调。
    /// </summary>
    /// <param name="sceneName">要加载的场景名称。</param>
    /// <param name="callback">加载完成后执行的回调函数。</param>
    public void LoadSceneAsyn(string sceneName, UnityAction callback)
    {
        // 使用协程来异步加载场景
        BaseMonoBehaviourManager.Instance.StartBaseCoroutine(ReallyLoadSceneAsyn(sceneName, callback));
    }

    /// <summary>
    /// 协程函数，用于异步加载场景，并在加载完成后执行回调。
    /// </summary>
    /// <param name="sceneName">要加载的场景名称。</param>
    /// <param name="callback">加载完成后执行的回调函数。</param>
    private IEnumerator ReallyLoadSceneAsyn(string sceneName, UnityAction callback)
    {
        // 异步加载场景，并将加载操作赋给asyncOperation
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // 在异步加载完成前等待
        while (!asyncOperation.isDone)
        {
            // 返回加载进度，可以用于显示进度条等
            yield return asyncOperation;
        }

        // 如果回调不为null，则调用回调
        callback?.Invoke();
    }
}
