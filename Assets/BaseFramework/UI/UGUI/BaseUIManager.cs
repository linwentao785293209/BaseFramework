using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// 基础UI管理器类，用于管理UI面板的创建、显示和隐藏
public class BaseUIManager : BaseSingletonInCSharp<BaseUIManager>
{
    private Dictionary<string, BaseUIPanel> panelDictionary = new Dictionary<string, BaseUIPanel>(); // 存储面板的容器

    private Transform UICanvasTransform; // Canvas对象的引用

    public BaseUIManager()
    {
        // 在构造函数中获取Canvas对象

        try
        {
            UICanvasTransform = GameObject.Find("UICanvas").transform;

            Debug.Log("UICanvas have found.");
        }
        catch
        {
            if (UICanvasTransform == null)
            {
                UICanvasTransform = GameObject.Instantiate(Resources.Load<GameObject>("UI/UICanvas")).transform;

                Debug.Log("UICanvas Instantiate.");
            }
        }

        if (UICanvasTransform == null)
        {
            Debug.LogError("UICanvas not found.");
        }
        else
        {
            GameObject.DontDestroyOnLoad(UICanvasTransform.gameObject);
        }
    }

    // 显示面板
    public T ShowPanel<T>() where T : BaseUIPanel
    {
        string panelName = typeof(T).Name;

        if (panelDictionary.ContainsKey(panelName))
        {
            return panelDictionary[panelName] as T;
        }

        // 加载面板预制体
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        panelObj.transform.SetParent(UICanvasTransform, false);

        // 获取面板脚本
        T panel = panelObj.GetComponent<T>();
        if (panel != null)
        {
            panelDictionary.Add(panelName, panel);
            panel.ShowMe();
        }
        else
        {
            Debug.LogError("Failed to get panel script.");
        }

        return panel;
    }

    // 隐藏面板
    public void HidePanel<T>(bool isFade = true) where T : BaseUIPanel
    {
        string panelName = typeof(T).Name;

        if (panelDictionary.ContainsKey(panelName))
        {
            if (isFade)
            {
                panelDictionary[panelName].HideMe(() =>
                {
                    // 面板淡出成功后销毁面板
                    GameObject.Destroy(panelDictionary[panelName].gameObject);
                    panelDictionary.Remove(panelName);
                });
            }
            else
            {
                // 直接销毁面板
                GameObject.Destroy(panelDictionary[panelName].gameObject);
                panelDictionary.Remove(panelName);
            }
        }
        else
        {
            Debug.LogWarning("Panel not found: " + panelName);
        }
    }

    // 获取面板
    public T GetPanel<T>() where T : BaseUIPanel
    {
        string panelName = typeof(T).Name;
        if (panelDictionary.ContainsKey(panelName))
        {
            return panelDictionary[panelName] as T;
        }
        else
        {
            Debug.LogWarning("Panel not found: " + panelName);
            return null;
        }
    }

    /// <summary>
    /// 给控件添加自定义事件监听
    /// </summary>
    /// <param name="uIElement">控件对象</param>
    /// <param name="eventTriggerType">事件类型</param>
    /// <param name="callBack">事件的响应函数</param>
    public static void AddCustomEventListener(UIBehaviour uIElement, EventTriggerType eventTriggerType, UnityAction<BaseEventData> callBack)
    {
        EventTrigger trigger = uIElement.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = uIElement.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventTriggerType;
        entry.callback.AddListener(callBack);

        trigger.triggers.Add(entry);
    }
}
