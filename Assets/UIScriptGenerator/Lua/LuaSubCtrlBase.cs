using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaSubCtrlBase : MonoBehaviour
{
    /// <summary>
    /// Lua绑定GameObject
    /// </summary>
    public List<LuaBindItem> m_LuaBindItems = new List<LuaBindItem>();

    /// <summary>
    /// Lua代码文件
    /// </summary>
    public TextAsset m_LuaFile;
}
