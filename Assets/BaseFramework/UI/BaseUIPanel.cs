using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 基础UI面板类，所有UI面板应该继承自此类
public abstract class BaseUIPanel : MonoBehaviour
{
    private CanvasGroup canvasGroup; // 用于控制UI面板的淡入淡出效果的CanvasGroup组件
    private float alphaSpeed = 10; // 淡入淡出的速度
    private bool isShow; // 标记UI面板是否正在显示
    private UnityAction hideCallBack; // 当UI面板淡出成功时要执行的委托函数

    //通过里式转换原则 来存储所有的控件 是通过控件名字符串绑定框架的 因为可能有同名的控件 放在同一个List里
    private Dictionary<string, List<UIBehaviour>> UIElementDictionary = new Dictionary<string, List<UIBehaviour>>();

    // Awake方法在对象实例化后立即调用
    protected virtual void Awake()
    {
        // 获取或添加CanvasGroup组件
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                Debug.LogError("Failed to add CanvasGroup component.");
            }
        }

        //初始化找Panel下所有控件
        FindPanelUIElement<Button>();
        FindPanelUIElement<Image>();
        FindPanelUIElement<Text>();
        FindPanelUIElement<Toggle>();
        FindPanelUIElement<Slider>();
        FindPanelUIElement<ScrollRect>();
        FindPanelUIElement<InputField>();
    }

    /// <summary>
    /// 找到子对象的对应控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindPanelUIElement<T>() where T : UIBehaviour
    {
        //获得子对象中所有当前类型的控件脚本
        T[] controls = this.GetComponentsInChildren<T>();

        //
        for (int i = 0; i < controls.Length; ++i)
        {
            string objName = controls[i].gameObject.name;
            if (UIElementDictionary.ContainsKey(objName))
                UIElementDictionary[objName].Add(controls[i]);
            else
                UIElementDictionary.Add(objName, new List<UIBehaviour>() { controls[i] });

            //如果是按钮控件
            if (controls[i] is Button)
            {
                (controls[i] as Button).onClick.AddListener(() =>
                {
                    OnButtonInitClick(objName);
                });
            }

            //如果是单选框或者多选框
            else if (controls[i] is Toggle)
            {
                (controls[i] as Toggle).onValueChanged.AddListener((value) =>
                {
                    OnToggleInitValueChanged(objName, value);
                });
            }
        }
    }

    //按钮初始化时的监听函数 重写后 分支判断名字执行不同逻辑
    protected virtual void OnButtonInitClick(string btnName)
    {

    }

    //多选框初始化时的监听函数 重写后 分支判断名字执行不同逻辑
    protected virtual void OnToggleInitValueChanged(string toggleName, bool value)
    {

    }

    /// <summary>
    /// 得到对应名字的对应控件脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="uIElement"></param>
    /// <returns></returns>
    protected T GetUIElement<T>(string uIElement) where T : UIBehaviour
    {
        if (UIElementDictionary.ContainsKey(uIElement))
        {
            for (int i = 0; i < UIElementDictionary[uIElement].Count; ++i)
            {
                if (UIElementDictionary[uIElement][i] is T)
                    return UIElementDictionary[uIElement][i] as T;
            }
        }

        return null;
    }

    // 动态添加控件
    protected void AddUIElement<T>(string name, T uiElement) where T : UIBehaviour
    {
        if (UIElementDictionary.ContainsKey(name))
        {
            UIElementDictionary[name].Add(uiElement);
        }
        else
        {
            UIElementDictionary.Add(name, new List<UIBehaviour>() { uiElement });
        }
    }

    // 动态移除控件
    protected void RemoveUIElement<T>(string name, T uiElement) where T : UIBehaviour
    {
        if (UIElementDictionary.ContainsKey(name))
        {
            UIElementDictionary[name].Remove(uiElement);
        }
    }


    // Start方法在第一帧之前调用
    protected virtual void Start()
    {
        Init(); // 调用子类的Init方法，用于初始化按钮事件监听等内容
    }

    // 子类必须实现的抽象方法，用于初始化UI面板
    public abstract void Init();

    // 显示UI面板时调用的方法
    public virtual void ShowMe()
    {
        isShow = true;
        canvasGroup.alpha = 0; // 将透明度设置为0，实现淡入效果
    }

    // 隐藏UI面板时调用的方法，接受一个回调函数作为参数
    public virtual void HideMe(UnityAction callBack)
    {
        isShow = false;
        canvasGroup.alpha = 1; // 将透明度设置为1，实现淡出效果
        hideCallBack = callBack; // 记录传入的淡出成功后会执行的函数
    }

    // Update方法在每一帧调用
    void Update()
    {
        // 淡入
        if (isShow && !Mathf.Approximately(canvasGroup.alpha, 1))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1, alphaSpeed * Time.deltaTime);
        }
        // 淡出
        else if (!isShow && !Mathf.Approximately(canvasGroup.alpha, 0))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, alphaSpeed * Time.deltaTime);
            if (Mathf.Approximately(canvasGroup.alpha, 0))
            {
                hideCallBack?.Invoke(); // 在淡出完成后执行回调函数
            }
        }
    }
}
