using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//自定义图片绘制
public class CustomGUITexture : CustomGUIControl
{
    //图片绘制的缩放模式
    public ScaleMode scaleMode = ScaleMode.StretchToFill;

    //实现Style关和开的绘制抽象方法

    protected override void StyleOffDraw()
    {
        GUI.DrawTexture(guiPos.Pos, content.image, scaleMode);
    }

    protected override void StyleOnDraw()
    {
        GUI.DrawTexture(guiPos.Pos, content.image, scaleMode);
    }
}
