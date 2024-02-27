using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//自定义按钮
public class CustomGUIButton : CustomGUIControl
{
    //提供给外部 用于响应 按钮点击的事件 只要在外部给予了响应函数 那就会执行
    public event UnityAction clickEvent;

    //实现Style关和开的绘制抽象方法
    protected override void StyleOffDraw()
    {
        if( GUI.Button(guiPos.Pos, content ) )
        {
            clickEvent?.Invoke();
        }
    }

    protected override void StyleOnDraw()
    {
        if (GUI.Button(guiPos.Pos, content, style))
        {
            clickEvent?.Invoke();
        }
    }
}
