using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//自定义单选框
public class CustomGUIToggleGroup : MonoBehaviour
{
    //多选框数组 通过管理多选框实现单选框
    public CustomGUIToggle[] toggles;

    //记录上一次为true的toggle
    private CustomGUIToggle frontTrueToggle;

    void Start()
    {
        //判断多选框数组有没有多选框 没有多选框就返回
        if (toggles.Length == 0)
            return;

        frontTrueToggle = toggles[0];

        for (int j = 0; j < toggles.Length; j++)
        {
            toggles[j].isSel = j == 0;
        }


        //通过遍历多选框数组 来为多个多选框添加监听事件函数
        //在函数中做处理 当一个为true时 另外两个变成false
        for (int i = 0; i < toggles.Length; i++)
        {
            //取出当前多选框进行记录
            CustomGUIToggle toggle = toggles[i];

            //为当前多选框添加监听事件函数
            toggle.changeValue += (value) =>
            {
                //当传入的value是ture时 需要把另外的多选框变成false
                if (value)
                {
                    //遍历多选框 意味着另外的多选框要变成false
                    for (int j = 0; j < toggles.Length; j++)
                    {
                        //判断是否是当前取出的多选框 不是当前取出的多选框就设置为不选择
                        //这里有闭包 toggle就是上一个函数中申明的变量 改变了它的生命周期
                        if (toggles[j] != toggle)
                        {
                            toggles[j].isSel = false;
                        }
                    }
                    //记录上一次为true的toggle
                    frontTrueToggle = toggle;
                }
                //来判断 当前变成false的这个toggle是不是上一次为true
                //如果是 就不应该让它变成false
                //都在有可能全都不选 违反单选框规则
                else if (toggle == frontTrueToggle)
                {
                    //强制改成 true
                    toggle.isSel = true;
                }
            };

        }
    }

}
