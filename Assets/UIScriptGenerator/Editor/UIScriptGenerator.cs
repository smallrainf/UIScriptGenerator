using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UIScript
{
    public class UIScriptGenerator : Editor
    {
        private static Dictionary<string, Type> m_UIComponentTypeDict = new Dictionary<string, Type>();

        static UIScriptGenerator()
        {
            m_UIComponentTypeDict.Add("img", typeof(ImageComponent));
            m_UIComponentTypeDict.Add("btn", typeof(ButtonComponent));
        }

        [MenuItem("GameObject/UIScript/创建CSharp脚本", false, 1)]
        public static void GenerateCSharpUIScript()
        {

        }

        [MenuItem("GameObject/UIScript/创建Lua脚本", false, 2)]
        public static void GenerateLuaUIScript()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null)
            {
                Debug.LogError("请选中UIPrefab");
                return;
            }

            List<UIComponent> uiComponents = CreateUIComponentList(go.transform);

            string luaTemplate = File.ReadAllText(Application.dataPath + "/UIScriptGenerator/Template/Lua.txt");

            string defines = GetUIComponentDefineList(uiComponents);

            string luaInstance = string.Format(luaTemplate, go.name, defines, string.Empty, string.Empty, string.Empty);

            File.WriteAllText(Application.dataPath + "/Examples/Lua/" + go.name + ".lua", luaInstance);

            LuaCtrlBase ctrlBase = AddUIScript(go);
            SetUIScriptData(ctrlBase, uiComponents);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 添加UI脚本
        /// </summary>
        private static LuaCtrlBase AddUIScript(GameObject uiObject)
        {
            LuaCtrlBase ctrlBase = uiObject.GetComponent<LuaCtrlBase>();
            if (ctrlBase == null)
            {
                ctrlBase = uiObject.AddComponent<LuaCtrlBase>();
            }
            return ctrlBase;
        }

        /// <summary>
        /// 设置UI脚本变量
        /// </summary>
        /// <param name="ctrlBase"></param>
        /// <param name="uiComponents"></param>
        private static void SetUIScriptData(LuaCtrlBase ctrlBase, List<UIComponent> uiComponents)
        {
            if (ctrlBase == null)
            {
                return;
            }

            ctrlBase.m_LuaBindItems.Clear();

            foreach (var uiComponent in uiComponents)
            {
                LuaBindItem bindItem = new LuaBindItem()
                {
                    variableName = uiComponent.UIName,
                    bindType = uiComponent.GetBindType(),
                    go = uiComponent.UIObject
                };
                ctrlBase.m_LuaBindItems.Add(bindItem);
            }
        }

        /// <summary>
        /// 获取UI组件集合定义字符串
        /// </summary>
        /// <param name="uiComponents"></param>
        /// <returns></returns>
        private static string GetUIComponentDefineList(List<UIComponent> uiComponents)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var uiComponent in uiComponents)
            {
                stringBuilder.AppendLine(string.Format("{0} = nil;", uiComponent.UIName));
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 创建UI组件集合
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private static List<UIComponent> CreateUIComponentList(Transform root)
        {
            List<UIComponent> uiComponents = new List<UIComponent>();

            CreateUIComponent(root, ref uiComponents);

            return uiComponents;
        }

        /// <summary>
        /// 创建UI组件
        /// </summary>
        /// <param name="root"></param>
        /// <param name="uiComponents"></param>
        private static void CreateUIComponent(Transform root, ref List<UIComponent> uiComponents)
        {
            foreach (Transform child in root.transform)
            {
                CreateUIComponent(child, ref uiComponents);

                if (child.name.Contains("#") == true)
                {
                    string uiName = child.name.Split('#')[0];
                    string uiComponentTypeStr = child.name.Split('#')[1];

                    if (m_UIComponentTypeDict.TryGetValue(uiComponentTypeStr, out Type uiComponentType) == true)
                    {
                        UIComponent uiComponent = System.Activator.CreateInstance(uiComponentType) as UIComponent;
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
