using System.Collections;
using System.Collections.Generic;
using BaseFramework;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// AssetBundle管理器
/// </summary>
public class AssetBundleManager : BaseSingletonInMonoBehaviour<AssetBundleManager>
{
    #region 变量

    private AssetBundle mainAssetBundle = null; // 主 AssetBundle
    private AssetBundleManifest assetBundleManifest = null; // AssetBundle 清单
    private Dictionary<string, AssetBundle> assetBundleDictionary = new Dictionary<string, AssetBundle>(); // AssetBundle 字典

    /// <summary>
    /// 获取 AssetBundle 文件夹的路径
    /// </summary>
    private string assetBundleFolderPath
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }

    /// <summary>
    /// 获取主 AssetBundle 的名称
    /// </summary>
    private string mainAssetBundleName
    {
        get
        {
#if UNITY_IOS
            return "IOS";
#elif UNITY_ANDROID
            return "Android";
#else
            return "PC";
#endif
        }
    }

    #endregion

    #region 加载包

    /// <summary>
    /// 加载主 AssetBundle 和 AssetBundle 清单
    /// </summary>
    private void LoadMainAssetBundleAndAssetBundleManifest()
    {
        if (mainAssetBundle == null)
        {
            mainAssetBundle = AssetBundle.LoadFromFile(assetBundleFolderPath + mainAssetBundleName);
            assetBundleManifest = mainAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
    }

    /// <summary>
    /// 加载 AssetBundle 的依赖项
    /// </summary>
    private void LoadAssetBundleDependencies(string assetBundleName)
    {
        LoadMainAssetBundleAndAssetBundleManifest();

        string[] strs = assetBundleManifest.GetAllDependencies(assetBundleName);
        for (int i = 0; i < strs.Length; i++)
        {
            if (!assetBundleDictionary.ContainsKey(strs[i]))
            {
                AssetBundle ab = AssetBundle.LoadFromFile(assetBundleFolderPath + strs[i]);
                assetBundleDictionary.Add(strs[i], ab);
            }
        }
    }

    /// <summary>
    /// 加载目标 AssetBundle
    /// </summary>
    private void LoadAssetBundleTarget(string assetBundleName)
    {
        if (!assetBundleDictionary.ContainsKey(assetBundleName))
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundleFolderPath + assetBundleName);
            assetBundleDictionary.Add(assetBundleName, assetBundle);
        }
    }

    /// <summary>
    /// 加载目标 AssetBundle 和其依赖项
    /// </summary>
    private void LoadAssetBundleTargetAndDependencies(string assetBundleName)
    {
        LoadAssetBundleDependencies(assetBundleName);
        LoadAssetBundleTarget(assetBundleName);
    }

    #endregion

    #region 同步加载

    /// <summary>
    /// 如果资源是 GameObject，则实例化；否则返回原始资源
    /// </summary>
    private T InstantiateIfGameObject<T>(T resource) where T : Object
    {
        if (resource is GameObject)
        {
            return Instantiate(resource);
        }
        else
        {
            return resource;
        }
    }

    /// <summary>
    /// 同步加载 AssetBundle 中的资源
    /// </summary>
    public T LoadAssetBundleResource<T>(string assetBundleName, string resourceName) where T : Object
    {
        LoadAssetBundleTargetAndDependencies(assetBundleName);

        T resource = assetBundleDictionary[assetBundleName].LoadAsset<T>(resourceName);

        return InstantiateIfGameObject<T>(resource);
    }

    /// <summary>
    /// 同步加载 AssetBundle 中的资源，指定资源类型
    /// </summary>
    public Object LoadAssetBundleResource(string assetBundleName, string resourceName, System.Type type)
    {
        LoadAssetBundleTargetAndDependencies(assetBundleName);

        Object resource = assetBundleDictionary[assetBundleName].LoadAsset(resourceName, type);

        return InstantiateIfGameObject(resource);
    }

    /// <summary>
    /// 同步加载 AssetBundle 中的资源
    /// </summary>
    public Object LoadAssetBundleResource(string assetBundleName, string resourceName)
    {
        LoadAssetBundleTargetAndDependencies(assetBundleName);

        Object resource = assetBundleDictionary[assetBundleName].LoadAsset(resourceName);

        return InstantiateIfGameObject(resource);
    }

    #endregion

    #region 异步加载

    /// <summary>
    /// 如果资源是 GameObject，则实例化后执行回调；否则直接执行回调
    /// </summary>
    private void InstantiateIfGameObjectAndCallBack<T>(T resource, UnityAction<T> callBack) where T : Object
    {
        if (resource is GameObject)
            callBack(Instantiate(resource) as T);
        else
            callBack(resource as T);
    }

    /// <summary>
    /// 异步加载 AssetBundle 中的资源
    /// </summary>
    public void LoadAssetBundleResourceAsync<T>(string assetBundleName, string resourceName, UnityAction<T> callBack) where T : Object
    {
        StartCoroutine(LoadAssetBundleResourceAsyncCoroutine<T>(assetBundleName, resourceName, callBack));
    }

    /// <summary>
    /// 异步加载 AssetBundle 中的资源，指定资源类型
    /// </summary>
    private IEnumerator LoadAssetBundleResourceAsyncCoroutine<T>(string assetBundleName, string resourceName, UnityAction<T> callBack) where T : Object
    {
        LoadAssetBundleTargetAndDependencies(assetBundleName);

        AssetBundleRequest assetBundleRequest = assetBundleDictionary[assetBundleName].LoadAssetAsync<T>(resourceName);
        yield return assetBundleRequest;

        InstantiateIfGameObjectAndCallBack<T>(assetBundleRequest.asset as T, callBack);
    }

    /// <summary>
    /// 异步加载 AssetBundle 中的资源，指定资源类型
    /// </summary>
    public void LoadAssetBundleResourceAsync(string assetBundleName, string resourceName, System.Type type, UnityAction<Object> callBack)
    {
        StartCoroutine(LoadAssetBundleResourceAsyncCoroutine(assetBundleName, resourceName, type, callBack));
    }

    /// <summary>
    /// 异步加载 AssetBundle 中的资源
    /// </summary>
    private IEnumerator LoadAssetBundleResourceAsyncCoroutine(string assetBundleName, string resourceName, System.Type type, UnityAction<Object> callBack)
    {
        LoadAssetBundleTargetAndDependencies(assetBundleName);

        AssetBundleRequest assetBundleRequest = assetBundleDictionary[assetBundleName].LoadAssetAsync(resourceName, type);
        yield return assetBundleRequest;

        InstantiateIfGameObjectAndCallBack(assetBundleRequest.asset, callBack);
    }

    /// <summary>
    /// 异步加载 AssetBundle 中的资源
    /// </summary>
    public void LoadAssetBundleResourceAsync(string assetBundleName, string resourceName, UnityAction<Object> callBack)
    {
        StartCoroutine(LoadAssetBundleResourceAsyncCoroutine(assetBundleName, resourceName, callBack));
    }

    /// <summary>
    /// 异步加载 AssetBundle 中的资源
    /// </summary>
    private IEnumerator LoadAssetBundleResourceAsyncCoroutine(string assetBundleName, string resourceName, UnityAction<Object> callBack)
    {
        LoadAssetBundleTargetAndDependencies(assetBundleName);

        AssetBundleRequest assetBundleRequest = assetBundleDictionary[assetBundleName].LoadAssetAsync(resourceName);
        yield return assetBundleRequest;

        InstantiateIfGameObjectAndCallBack(assetBundleRequest.asset, callBack);
    }

    #endregion

    #region 卸载包

    /// <summary>
    /// 卸载指定名称的 AssetBundle
    /// </summary>
    public void UnLoadAssetBundle(string name)
    {
        if (assetBundleDictionary.ContainsKey(name))
        {
            assetBundleDictionary[name].Unload(false);
            assetBundleDictionary.Remove(name);
        }
    }

    /// <summary>
    /// 清理所有 AssetBundle
    /// </summary>
    public void ClearAssetBundle()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        assetBundleDictionary.Clear();
        mainAssetBundle = null;
        assetBundleManifest = null;
    }

    #endregion
}
