using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public abstract class BaseNGUIPanel<T> : BaseSingletonInMonoBehaviour<T> where T : MonoBehaviour
    {
        protected virtual void Start()
        {
            //初始化 主要用于 监听按钮等事件
            Init();
        }

        /// <summary>
        /// 方便子类继承时 进行初始化操作
        /// 只要子类在其中写一些初始化逻辑 
        /// 最终都会在Start里面执行
        /// 子类就不太需要去写Start函数了
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// 显示自己
        /// 写成虚函数的目的 是方便拓展 可能子类面板 需要在显示 或者隐藏自己时
        /// 做一些额外操作 那么你可以在子类去重写 这两个函数 达到目的
        /// </summary>
        public virtual void ShowMe()
        {
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// 隐藏自己
        /// </summary>
        public virtual void HideMe()
        {
            this.gameObject.SetActive(false);
        }
    }
}