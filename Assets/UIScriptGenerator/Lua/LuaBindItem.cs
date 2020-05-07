using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LuaBindItem
{
    /// <summary>
    /// 控件变量名
    /// </summary>
    public string variableName;

    /// <summary>
    /// 控件类型
    /// </summary>
    public EUIBindItemType bindType;

    /// <summary>
    /// 控件引用
    /// </summary>
    public GameObject go;
}
