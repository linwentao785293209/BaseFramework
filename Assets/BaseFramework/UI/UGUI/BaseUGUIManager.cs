using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace BaseFramework
{
    // 基础UI管理器类，用于管理UI面板的创建、显示和隐藏
    public class BaseUGUIManager : BaseSingletonInCSharp<BaseUGUIManager>
    {
        private Dictionary<string, BaseUGUIPanel> panelDictionary = new Dictionary<string, BaseUGUIPanel>(); // 存储面板的容器

        private Transform UGUICanvasTransform; // Canvas对象的引用

        public  const string UI_SAVE_PATH = "BaseFramework/UI/UGUI/";
        public  const string CANVAS_NAME = "UGUICanvas";
        
        public BaseUGUIManager()
        {
            // 在构造函数中获取Canvas对象

            try
            {
                UGUICanvasTransform = GameObject.Find(CANVAS_NAME).transform;

                Debug.Log("UICanvas have found.");
            }
            catch
            {
                if (UGUICanvasTransform == null)
                {
                    UGUICanvasTransform = GameObject.Instantiate(Resources.Load<GameObject>(UI_SAVE_PATH+CANVAS_NAME)).transform;

                    Debug.Log(CANVAS_NAME + "Instantiate.");
                }
            }

            if (UGUICanvasTransform == null)
            {
                Debug.LogError(CANVAS_NAME + " not found.");
            }
            else
            {
                GameObject.DontDestroyOnLoad(UGUICanvasTransform.gameObject);
            }
        }

        // 显示面板
        public T ShowPanel<T>(string path = UI_SAVE_PATH) where T : BaseUGUIPanel
        {
            string panelName = typeof(T).Name;

            if (panelDictionary.ContainsKey(panelName))
            {
                return panelDictionary[panelName] as T;
            }

            // 加载面板预制体
            GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>(path + panelName));
            panelObj.transform.SetParent(UGUICanvasTransform, false);

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
        public void HidePanel<T>(bool isFade = true) where T : BaseUGUIPanel
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
        public T GetPanel<T>() where T : BaseUGUIPanel
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

        // 是否有该面板
        public bool HasPanel<T>() where T : BaseUGUIPanel
        {
            string panelName = typeof(T).Name;
            if (panelDictionary.ContainsKey(panelName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        /// 给控件添加自定义事件监听
        /// </summary>
        /// <param name="uIElement">控件对象</param>
        /// <param name="eventTriggerType">事件类型</param>
        /// <param name="callBack">事件的响应函数</param>
        public static void AddCustomEventListener(UIBehaviour uIElement, EventTriggerType eventTriggerType,
            UnityAction<BaseEventData> callBack)
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
}