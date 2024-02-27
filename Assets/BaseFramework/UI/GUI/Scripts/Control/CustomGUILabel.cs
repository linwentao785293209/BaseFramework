using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//自定义文本
public class CustomGUILabel : CustomGUIControl
{
    //实现Style关和开的绘制抽象方法
    protected override void StyleOffDraw()
    {
        GUI.Label(guiPos.Pos, content);
    }

    protected override void StyleOnDraw()
    {
        GUI.Label(guiPos.Pos, content, style);
    }
}
