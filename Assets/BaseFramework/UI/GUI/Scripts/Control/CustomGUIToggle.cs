using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//自定义多选框
public class CustomGUIToggle : CustomGUIControl
{
    //更改之前是否选中
    private bool isOldSel;

    //是否选中
    public bool isSel;

    //更改选中状态后的响应事件
    public event UnityAction<bool> changeValue;

    //实现Style关和开的绘制抽象方法

    protected override void StyleOffDraw()
    {
        //每次实时赋值是否选中
        isSel = GUI.Toggle(guiPos.Pos, isSel, content);

        //判断选中状态是否变化
        //选择状态只有变化时 才告诉外部执行函数 否则没有必要一直告诉别人同一个值
        if(isOldSel != isSel)
        {
            //执行更改选中状态后的响应事件
            changeValue?.Invoke(isSel);
            //赋值更改之前是否选中的为最新的
            isOldSel = isSel;
        }
    }

    protected override void StyleOnDraw()
    {
        //每次实时赋值是否选中
        isSel = GUI.Toggle(guiPos.Pos, isSel, content, style);

        //判断选中状态是否变化
        //选择状态只有变化时 才告诉外部执行函数 否则没有必要一直告诉别人同一个值
        if (isOldSel != isSel)
        {
            //执行更改选中状态后的响应事件
            changeValue?.Invoke(isSel);
            //赋值更改之前是否选中的为最新的
            isOldSel = isSel;
        }
    }
}
