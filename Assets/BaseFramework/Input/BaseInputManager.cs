using UnityEngine;
using UnityEngine.Events;

// 输入管理器
public class BaseInputManager : BaseSingletonInCSharp<BaseInputManager>
{
    //是否开启输入检查
    private bool isCheckInput = false;

    // 构造函数，在 BaseMonoBehaviourManager 的 OnUpdate 中添加输入检测
    public BaseInputManager()
    {
        BaseMonoBehaviourManager.Instance.AddBaseLifeCycleManagerListener<BaseLifeCycleUpdateManager>(OnUpdateCheckInput);
    }

    // 设置是否进行输入检测
    public void SetCheckInput(bool isCheckInput)
    {
        this.isCheckInput = isCheckInput;
    }

    // 在每一帧的 OnUpdate 中检测输入
    private void OnUpdateCheckInput()
    {
        if (!isCheckInput)
            return;


        // 遍历所有 KeyCode 枚举值
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                // 使用事件管理器触发按键按下事件，传递 KeyCode
                BaseEventManager.Instance.EventTrigger<KeyCode>("OnGetKeyDown", keyCode);
            }
            else if (Input.GetKeyUp(keyCode))
            {
                // 使用事件管理器触发按键抬起事件，传递 KeyCode
                BaseEventManager.Instance.EventTrigger<KeyCode>("OnGetKeyUp", keyCode);
            }
        }

    }
}
