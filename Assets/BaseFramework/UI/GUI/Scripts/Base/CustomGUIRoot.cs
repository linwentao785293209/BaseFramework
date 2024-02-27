using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//在类名添加ExecuteAlways特性 可以让编辑模式下让指定代码运行
[ExecuteAlways]
//所有自定义GUI的根部类
public class CustomGUIRoot : MonoBehaviour
{
    //用于存储 子对象 所有GUI控件的容器
    private CustomGUIControl[] allControls;

    void Start()
    {
        //通过每一次绘制之前 得到所有子对象控件的 父类脚本
        allControls = this.GetComponentsInChildren<CustomGUIControl>();
    }

    //在这同一绘制子对象控件的内容
    private void OnGUI()
    {
        //编辑状态下 才会一直执行
        //if (!Application.isPlaying)
        //{
        //这句代码 浪费性能 因为每次 gui都会来获取所有的 控件对应的脚本 所以要在编辑状态下才会一直执行
        //在编辑状态下运行是因为想一直看到控件的移动

        //最后没开启是因为只得了一次子对象的CustomGUIControl 就算子对象被隐藏了也不会重写得CustomGUIControl 会有问题
        allControls = this.GetComponentsInChildren<CustomGUIControl>();

        //}

        //遍历每一个控件 让其 执行绘制
        for (int i = 0; i < allControls.Length; i++)
        {
            allControls[i].DrawGUI();
        }
    }
}
