using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class BaseFGUIManager : BaseSingletonInCSharp<BaseFGUIManager>
    {
        //用于存储已经显示的 UI面板
        private Dictionary<string, GComponent> panelDic = new Dictionary<string, GComponent>();

        //用于存储已经显示的 窗口
        private Dictionary<string, Window> windowDic = new Dictionary<string, Window>();

        public BaseFGUIManager()
        {
            //默认字体
            UIConfig.defaultFont = "UI/STHUPO";
            //默认音效
            UIPackage.AddPackage("UI/Public");
            UIConfig.buttonSound = (NAudioClip)UIPackage.GetItemAssetByURL("ui://Public/btnMusic");

            //适配相关的设置
            GRoot.inst.SetContentScaleFactor(1365, 768, UIContentScaler.ScreenMatchMode.MatchHeight);

            //设置模态半透明程度
            UIConfig.modalLayerColor = new Color(0, 0, 0, 0.5f);

            //注册相关的代码
        }

        //组件名和面板类名 是一致的 
        public T ShowPanel<T>(string packageName) where T : GComponent
        {
            Type panelType = typeof(T);
            string panelName = panelType.Name;
            //如果字典中有该面板的名字 证明已经创建过了 直接返回即可
            if (panelDic.ContainsKey(panelName))
            {
                panelDic[panelName].visible = true;
                return panelDic[panelName] as T;
            }

            //加载包和依赖包 
            //由于从Resources文件夹中加载包 会帮助我们判断重复没有 所以 这里既是重复执行也没什么问题
            UIPackage package = UIPackage.AddPackage("UI/" + packageName);
            foreach (var item in package.dependencies)
            {
                UIPackage.AddPackage("UI/" + item["name"]);
            }

            //创建组件面板
            GComponent panel = UIPackage.CreateObject(packageName, panelName).asCom;
            //把组件的尺寸设置的和逻辑分辨率一致
            panel.MakeFullScreen();
            GRoot.inst.AddChild(panel);
            //和父对象建立 宽高关联 这样 分辨率变化时 面板也不会出问题
            panel.AddRelation(GRoot.inst, RelationType.Size);

            //进行批处理 DC优化 开关开启
            panel.fairyBatching = true;
            //把当前显示的面板存起来 用于之后的隐藏
            panelDic.Add(panelName, panel);

            //把父类转换成对应的 子类
            return panel as T;
        }

        //显示窗口方法
        public T ShowWindow<T>() where T : Window, new()
        {
            Type type = typeof(T);
            string windowName = type.Name;

            //判断有没有面板
            if (windowDic.ContainsKey(windowName))
            {
                windowDic[windowName].Show();
                return windowDic[windowName] as T;
            }

            //创建并显示面板
            T win = new T();
            //记录字典中
            windowDic.Add(windowName, win);

            //当存储了再去显示 避免显示时调用隐藏不执行
            win.Show();


            return win;
        }

        public void HidePanel<T>(bool isDispose = false) where T : GComponent
        {
            Type panelType = typeof(T);
            string panelName = panelType.Name;
            //如果没有面板显示着  就直接返回
            if (!panelDic.ContainsKey(panelName))
                return;
            //希望移除面板
            if (isDispose)
            {
                //移除面板 并且从字典中移除
                panelDic[panelName].Dispose();
                panelDic.Remove(panelName);
            }
            //希望只是失活
            else
            {
                panelDic[panelName].visible = false;
            }
        }

        //隐藏窗口方法
        public void HideWindow<T>(bool isDispose = false)
        {
            Type type = typeof(T);
            string windowName = type.Name;

            if (windowDic.ContainsKey(windowName))
            {
                if (isDispose)
                {
                    windowDic[windowName].Dispose();
                    windowDic.Remove(windowName);
                }
                else
                {
                    windowDic[windowName].Hide();
                }
            }
        }

        public T GetPanel<T>() where T : GComponent
        {
            Type panelType = typeof(T);
            string panelName = panelType.Name;
            //如果有这个面板 直接返回
            if (panelDic.ContainsKey(panelName))
                return panelDic[panelName] as T;

            return null;
        }

        //得到窗口
        public T GetWindow<T>() where T : Window
        {
            Type type = typeof(T);
            string windowName = type.Name;

            if (windowDic.ContainsKey(windowName))
            {
                return windowDic[windowName] as T;
            }

            return null;
        }

        //主要用于销毁所有面板 和 资源垃圾回收的方法
        public void ClearPanel(bool isGC = false)
        {
            //销毁所有面板 并且清空字典
            foreach (var item in panelDic.Values)
            {
                item.Dispose();
            }

            panelDic.Clear();

            if (isGC)
            {
                //释放所有包资源
                UIPackage.RemoveAllPackages();
                //垃圾回收
                GC.Collect();
            }
        }

        //清除所有窗口
        public void ClearWindow(bool isGC = false)
        {
            //销毁所有面板 并且清空字典
            foreach (var item in windowDic.Values)
            {
                item.Dispose();
            }

            windowDic.Clear();
            if (isGC)
            {
                //释放所有包资源
                UIPackage.RemoveAllPackages();
                //垃圾回收
                GC.Collect();
            }
        }


        /// <summary>
        /// 加载组件 
        /// </summary>
        /// <param name="packageName">包名</param>
        /// <param name="componentName">组件名</param>
        /// <returns></returns>
        public GComponent LoadComponent(string packageName, string componentName)
        {
            //加载包
            UIPackage package = UIPackage.AddPackage("UI/" + packageName);
            //加载依赖包
            foreach (var item in package.dependencies)
            {
                UIPackage.AddPackage("UI/" + item["name"]);
            }

            GComponent component = UIPackage.CreateObject(packageName, componentName).asCom;
            //component.MakeFullScreen();

            //优化dc，只需要把面板组件fairyBatching设置为true
            component.fairyBatching = true;
            return component;
        }
    }
}