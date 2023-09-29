using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 游戏对象池管理器类，继承自 C# 基础单例类
public class BaseGameObjectPoolManager : BaseSingletonInCSharp<BaseGameObjectPoolManager>
{
    // 存储游戏对象池的字典，键为游戏对象的资源路径，值为游戏对象池
    private Dictionary<string, BaseGameObjectPool> baseGameObjectPoolDictionary = new Dictionary<string, BaseGameObjectPool>();

    // 游戏对象池管理器的根节点
    private GameObject baseGameObjectPoolManagerNode;

    // 从对象池中获取游戏对象的方法，包括资源路径和回调函数
    public void GetGameObject(string gameObjectPath, UnityAction<GameObject> callBack)
    {
        // 检查游戏对象池字典中是否包含指定资源路径的对象池，并且对象池中是否有可用的游戏对象
        if (baseGameObjectPoolDictionary.ContainsKey(gameObjectPath) && baseGameObjectPoolDictionary[gameObjectPath].baseGameObjectPoolGameObjectList.Count > 0)
        {
            // 如果有可用的游戏对象，调用回调函数返回游戏对象
            callBack(baseGameObjectPoolDictionary[gameObjectPath].GetGameObject());
        }
        else
        {
            // 如果没有可用的游戏对象，通过资源管理器异步加载游戏对象
            BaseResourceManager.Instance.LoadAsync<GameObject>(gameObjectPath, (gameObject) =>
            {
                // 设置加载的游戏对象的名称为资源路径
                gameObject.name = gameObjectPath;

                // 调用回调函数返回加载的游戏对象
                callBack(gameObject);
            });
        }
    }

    // 释放游戏对象到对象池的方法，包括资源路径和游戏对象
    public void ReleaseGameObject(string gameObjectPath, GameObject gameObject)
    {
        // 如果游戏对象池管理器的根节点尚未创建，创建一个新的根节点
        if (baseGameObjectPoolManagerNode == null)
        {
            baseGameObjectPoolManagerNode = new GameObject("BaseGameObjectPoolManagerNode");
        }

        // 检查游戏对象池字典中是否包含指定资源路径的对象池
        if (baseGameObjectPoolDictionary.ContainsKey(gameObjectPath))
        {
            // 如果包含，将游戏对象释放到对应的对象池中
            baseGameObjectPoolDictionary[gameObjectPath].ReleaseGameObject(gameObject);
        }
        else
        {
            // 如果不包含，创建一个新的对象池，并添加到对象池字典中
            baseGameObjectPoolDictionary.Add(gameObjectPath, new BaseGameObjectPool(baseGameObjectPoolManagerNode, gameObjectPath, gameObject));
        }
    }

    // 清空游戏对象池管理器的方法
    public void Clear()
    {
        baseGameObjectPoolDictionary.Clear(); // 清空对象池字典
        baseGameObjectPoolManagerNode = null; // 重置对象池管理器的根节点
    }
}
