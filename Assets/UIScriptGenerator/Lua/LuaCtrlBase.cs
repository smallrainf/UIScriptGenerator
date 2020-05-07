using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaCtrlBase : MonoBehaviour
{
    /// <summary>
    /// Lua绑定GameObject
    /// </summary>
    public List<LuaBindItem> m_LuaBindItems;

    /// <summary>
    /// Lua代码文件
    /// </summary>
    public TextAsset m_LuaFile;
}
