using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIScript
{
    public abstract class UIComponent
    {
        /// <summary>
        /// 组件名称
        /// </summary>
        public string UIName { get; set; }

        /// <summary>
        /// 组件对象
        /// </summary>
        public GameObject UIObject { get; set; }

        /// <summary>
        /// 获取绑定类型
        /// </summary>
        /// <returns></returns>
        public virtual EUIBindItemType GetBindType()
        {
            return EUIBindItemType.GameObject;
        }

        /// <summary>
        /// 获取get函数
        /// </summary>
        /// <returns></returns>
        public virtual string GetGetterStr()
        {
            return string.Empty;
        }

        /// <summary>
        /// 获取set函数
        /// </summary>
        /// <returns></returns>
        public virtual string GetSetterStr()
        {
            return string.Empty;
        }
    }

}
