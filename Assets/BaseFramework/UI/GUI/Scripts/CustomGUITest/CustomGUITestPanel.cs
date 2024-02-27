using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGUITestPanel : MonoBehaviour
{
    //测试按钮
    public CustomGUIButton testCustomGUIButton;
    public CustomGUILabel testCustomGUILabel;
    public CustomGUIToggle testCustomGUIToggle;
    public CustomGUIToggleGroup testCustomGUIToggleGroup;
    public CustomGUIInput testCustomGUIInput;
    public CustomGUISlider testCustomGUISlider;
    public CustomGUITexture testCustomGUITexture;


    void Start()
    {
        testCustomGUIButton.clickEvent += () => { Debug.Log("点击测试按钮"); };

        testCustomGUIToggle.changeValue += (isSel) =>
        {
            Debug.Log($"testCustomGUIToggle isSel:{isSel}");
        };

        for (int i = 0;i < testCustomGUIToggleGroup.toggles.Length; i++)
        {
            int index = i;
            testCustomGUIToggleGroup.toggles[i].changeValue += (isSel) =>
            {
                Debug.Log($"testCustomGUIToggle{index} isSel:{isSel}");
            };
        }

        testCustomGUIInput.textChange += (text) =>
        {
            Debug.Log($"testCustomGUIInput text:{text}");
        };

        testCustomGUISlider.changeValue += (value) =>
        {
            Debug.Log($"testCustomGUISlider value:{value}");
        };
    }
}
