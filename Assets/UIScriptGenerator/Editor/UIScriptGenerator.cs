using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using UnityEditor;

namespace UIScript
{
    public class UIScriptGenerator : Editor
    {
        private static Dictionary<string, Type> m_UIComponentTypeDict = new Dictionary<string, Type>();

        static UIScriptGenerator()
        {
            // 切换不同DLL环境的时候会重新调用构造函数
            // 例如Play游戏时候会加载Assembly-CSharp.DLL，停止Play的时候会加载Assembly-CSharp-Editor.DLL
            foreach (var type in Assembly.Load("Assembly-CSharp-Editor").GetTypes())
            {
                var attribute = type.GetCustomAttribute<UIComponentAttirbute>();
                if (attribute != null)
                {
                    m_UIComponentTypeDict.Add(attribute.UIName, type);
                }
            }
        }

        [MenuItem("GameObject/UIScript/创建CSharpScreen脚本", false, 1)]
        public static void GenerateCSharpUIScript()
        {
            CSharpUIScriptGenerator.GenerateUIScript(Application.dataPath + "/UIScriptGenerator/Editor/Template/CSharp.txt", true);
        }

        [MenuItem("GameObject/UIScript/创建CSharpSubScreen脚本", false, 1)]
        public static void GenerateCSharpItemScript()
        {
            CSharpUIScriptGenerator.GenerateUIScript(Application.dataPath + "/UIScriptGenerator/Editor/Template/CSharpItem.txt", false);
        }

        [MenuItem("GameObject/UIScript/创建LuaScreen脚本", false, 2)]
        public static void GenerateLuaUIScript()
        {
            LuaUIScriptGenerator.GenerateUIScript(Application.dataPath + "/UIScriptGenerator/Editor/Template/Lua.txt", true);
        }

        [MenuItem("GameObject/UIScript/创建LuaSubScreen脚本", false, 2)]
        public static void GenerateLuaItemScript()
        {
            LuaUIScriptGenerator.GenerateUIScript(Application.dataPath + "/UIScriptGenerator/Editor/Template/LuaItem.txt", false);
        }

        /// <summary>
        /// 创建UI组件集合
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public static List<UIComponent> CreateUIComponentList(Transform root)
        {
            List<UIComponent> uiComponents = new List<UIComponent>();

            CreateUIComponent(root, root.gameObject.name, ref uiComponents);

            return uiComponents;
        }

        /// <summary>
        /// 创建UI组件
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="prefabName"></param>
        /// <param name="uiComponents"></param>
        private static void CreateUIComponent(Transform parent, string prefabName, ref List<UIComponent> uiComponents)
        {
            foreach (Transform child in parent.transform)
            {
                CreateUIComponent(child, prefabName, ref uiComponents);

                if (child.name.Contains("#") == true)
                {
                    string uiName = child.name.Split('#')[0];
                    string uiComponentTypeStr = child.name.Split('#')[1];

                    if (m_UIComponentTypeDict.TryGetValue(uiComponentTypeStr, out Type uiComponentType) == true)
                    {
                        UIComponent uiComponent = System.Activator.CreateInstance(uiComponentType) as UIComponent;
                        uiComponent.PrefabName = prefabName;
                        uiComponent.UIName = uiName;
                        uiComponent.UIObject = child.gameObject;
                        uiComponents.Add(uiComponent);
                    }
                    else
                    {
                        Debug.LogError("该类型目前不支持，uiComponentTypeStr:" + uiComponentTypeStr);
                    }
                }
            }
        }
    }
}
