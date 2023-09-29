using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 基础游戏对象池类
public class BaseGameObjectPool
{
    // 游戏对象的资源路径
    public string gameObjectPath;

    // 游戏对象池的根节点
    public GameObject baseGameObjectPoolNode;

    // 存储游戏对象的列表
    public List<GameObject> baseGameObjectPoolGameObjectList;

    // 构造函数，用于初始化游戏对象池
    public BaseGameObjectPool(GameObject baseGameObjectPoolManagerNode, string gameObjectPath, GameObject gameObject)
    {
        // 创建游戏对象池的根节点，并命名
        baseGameObjectPoolNode = new GameObject($"{gameObjectPath} BaseGameObjectPoolNode");
        this.gameObjectPath = gameObjectPath;

        // 将游戏对象池的根节点设为传入的管理节点的子节点
        baseGameObjectPoolNode.transform.parent = baseGameObjectPoolManagerNode.transform;

        // 初始化游戏对象列表
        baseGameObjectPoolGameObjectList = new List<GameObject>();

        // 将传入的游戏对象添加到游戏对象池中
        ReleaseGameObject(gameObject);
    }

    // 释放游戏对象到对象池中
    public void ReleaseGameObject(GameObject gameObject)
    {
        // 将游戏对象设置为不活跃状态
        gameObject.SetActive(false);

        // 将游戏对象添加到对象池列表中
        baseGameObjectPoolGameObjectList.Add(gameObject);

        // 将游戏对象的父节点设为对象池的根节点
        gameObject.transform.parent = baseGameObjectPoolNode.transform;
    }

    // 从对象池中获取游戏对象
    public GameObject GetGameObject()
    {
        GameObject gameObject = null;

        // 从对象池列表中取出第一个游戏对象
        gameObject = baseGameObjectPoolGameObjectList[0];

        // 从对象池列表中移除该游戏对象
        baseGameObjectPoolGameObjectList.RemoveAt(0);

        // 将游戏对象设置为活跃状态
        gameObject.SetActive(true);

        // 将游戏对象的父节点设为null，使其脱离对象池的控制
        gameObject.transform.parent = null;

        // 返回获取的游戏对象
        return gameObject;
    }
}
