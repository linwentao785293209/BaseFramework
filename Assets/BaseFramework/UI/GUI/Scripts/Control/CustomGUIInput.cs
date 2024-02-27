using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//自定义输入框
public class CustomGUIInput : CustomGUIControl
{
    //输入框文本更改事件
    public event UnityAction<string> textChange;

    //记录上一次文本信息
    private string oldStr = "";

    //实现Style关和开的绘制抽象方法

    protected override void StyleOffDraw()
    {
        content.text = GUI.TextField(guiPos.Pos, content.text);
        if(oldStr != content.text)
        {
            textChange?.Invoke(oldStr);
            oldStr = content.text;
        }
    }

    protected override void StyleOnDraw()
    {
        content.text = GUI.TextField(guiPos.Pos, content.text, style);
        if (oldStr != content.text)
        {
            textChange?.Invoke(oldStr);
            oldStr = content.text;
        }
    }
}
