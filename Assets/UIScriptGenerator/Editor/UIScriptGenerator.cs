using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

namespace UIScript
{
    public class UIScriptGenerator : Editor
    {
        private static string m_PrefabName;

        private static Dictionary<string, Type> m_UIComponentTypeDict = new Dictionary<string, Type>();

        static UIScriptGenerator()
        {
            m_UIComponentTypeDict.Add("btn", typeof(ButtonComponent));
            m_UIComponentTypeDict.Add("item", typeof(CommonItemScrollViewComponent));
            m_UIComponentTypeDict.Add("drop", typeof(DropDownComponent));
            m_UIComponentTypeDict.Add("go", typeof(GameObjectComponent));
            m_UIComponentTypeDict.Add("img", typeof(ImageComponent));
            m_UIComponentTypeDict.Add("input", typeof(InputComponent));
            m_UIComponentTypeDict.Add("rect", typeof(RectTransformComponent));
            m_UIComponentTypeDict.Add("group", typeof(ReusableLayoutGroupComponent));
            m_UIComponentTypeDict.Add("scroll", typeof(ScrollComponent));
            m_UIComponentTypeDict.Add("slider", typeof(SliderComponent));
            m_UIComponentTypeDict.Add("text", typeof(TextComponent));
            m_UIComponentTypeDict.Add("toggle", typeof(ToggleComponent));
        }

        [MenuItem("GameObject/UIScript/创建CSharpScreen脚本", false, 1)]
        public static void GenerateCSharpUIScript()
        {
            GenerateCSharpScript(Application.dataPath + "/UIScriptGenerator/Template/CSharp.txt", true);
        }

        [MenuItem("GameObject/UIScript/创建CSharpSubScreen脚本", false, 1)]
        public static void GenerateCSharpItemScript()
        {
            GenerateCSharpScript(Application.dataPath + "/UIScriptGenerator/Template/CSharpItem.txt", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templatePath"></param>
        /// <param name="isUI"></param>
        private static void GenerateCSharpScript(string templatePath, bool isUI)
        {
            // TODO:实现CSharp脚本自动生成
        }

        [MenuItem("GameObject/UIScript/创建LuaScreen脚本", false, 2)]
        public static void GenerateLuaUIScript()
        {
            GenerateLuaScript(Application.dataPath + "/UIScriptGenerator/Template/Lua.txt", true);
        }

        [MenuItem("GameObject/UIScript/创建LuaSubScreen脚本", false, 2)]
        public static void GenerateLuaItemScript()
        {
            GenerateLuaScript(Application.dataPath + "/UIScriptGenerator/Template/LuaItem.txt", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templatePath"></param>
        /// <param name="isUI"></param>
        private static void GenerateLuaScript(string templatePath, bool isUI)
        {
            GameObject go = Selection.activeGameObject;
            if (go == null)
            {
                Debug.LogError("请选中UIPrefab根结点");
                return;
            }

            m_PrefabName = go.name;

            List<UIComponent> uiComponents = CreateUIComponentList(go.transform);

            string defines = GetVariableDefines(uiComponents);
            string getOrSetFuncs = GetGetOrSetFuncs(uiComponents);
            string registerClickFuncs = GetRegisterClickFuncs(uiComponents);
            string clickFuncs = GetClickFuncs(uiComponents);

            string luaTemplate = File.ReadAllText(templatePath);

            string newLuaInstance = Utils.Format(luaTemplate, m_PrefabName, defines, getOrSetFuncs, registerClickFuncs, clickFuncs);

            string targetLuaPath = Application.dataPath + "/Examples/Lua/" + m_PrefabName + ".lua";
            // 如果目标文件存在，则替代对应字符串；否则使用模板来创建lua文件
            if (File.Exists(targetLuaPath) == true)
            {
                string oldLuaInstance = File.ReadAllText(targetLuaPath);

                int oldStartIndex = oldLuaInstance.IndexOf("--!@#start");
                int oldEndIndex = oldLuaInstance.LastIndexOf("--!@#regclickend");
                string oldSubStr = oldLuaInstance.Substring(oldStartIndex, oldEndIndex - oldStartIndex);

                int newStartIndex = newLuaInstance.IndexOf("--!@#start");
                int newEndIndex = newLuaInstance.LastIndexOf("--!@#regclickend");
                string newSubStr = newLuaInstance.Substring(newStartIndex, newEndIndex - newStartIndex);

                newLuaInstance = oldLuaInstance.Replace(oldSubStr, newSubStr);
            }

            File.WriteAllText(targetLuaPath, newLuaInstance);

            if (isUI == true)
            {
                LuaCtrlBase ctrlBase = AddUIScript<LuaCtrlBase>(go);
                SetUIScriptData(ctrlBase, uiComponents);
            }
            else
            {
                LuaSubCtrlBase ctrlBase = AddUIScript<LuaSubCtrlBase>(go);
                SetItemScriptData(ctrlBase, uiComponents);
            }

            PrefabUtility.SaveAsPrefabAsset(go, "Assets/Examples/Prefabs/" + go.name + ".prefab");
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 添加UI脚本
        /// </summary>
        private static T AddUIScript<T>(GameObject uiObject) where T : MonoBehaviour
        {
            T ctrlBase = uiObject.GetComponent<T>();
            if (ctrlBase == null)
            {
                ctrlBase = uiObject.AddComponent<T>();
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
        /// 设置Item脚本变量
        /// </summary>
        /// <param name="subCtrlBase"></param>
        /// <param name="uiComponents"></param>
        private static void SetItemScriptData(LuaSubCtrlBase subCtrlBase, List<UIComponent> uiComponents)
        {
            if (subCtrlBase == null)
            {
                return;
            }

            subCtrlBase.m_LuaBindItems.Clear();

            foreach (var uiComponent in uiComponents)
            {
                LuaBindItem bindItem = new LuaBindItem()
                {
                    variableName = uiComponent.UIName,
                    bindType = uiComponent.GetBindType(),
                    go = uiComponent.UIObject
                };
                subCtrlBase.m_LuaBindItems.Add(bindItem);
            }
        }

        /// <summary>
        /// 获取UI组件集合定义字符串
        /// </summary>
        /// <param name="uiComponents"></param>
        /// <returns></returns>
        private static string GetVariableDefines(List<UIComponent> uiComponents)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var uiComponent in uiComponents)
            {
                stringBuilder.AppendLine(string.Format("    --{0}", uiComponent.GetBindType().ToString()));
                stringBuilder.AppendLine(string.Format("    {0} = nil;", uiComponent.UIName));
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 获取UI组件集合Get/Set函数字符串
        /// </summary>
        /// <param name="uiComponents"></param>
        /// <returns></returns>
        private static string GetGetOrSetFuncs(List<UIComponent> uiComponents)
        {
            StringBuilder stringBuilder = new StringBuilder();

            // UI组件不一定存在get/set函数
            foreach (var uiComponent in uiComponents)
            {
                string getterStr = uiComponent.GetGetterStr();
                if (string.IsNullOrEmpty(getterStr) == false)
                {
                    stringBuilder.AppendLine(getterStr);
                }
                string setterStr = uiComponent.GetSetterStr();
                if (string.IsNullOrEmpty(setterStr) == false)
                {
                    stringBuilder.AppendLine(setterStr);
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 获取UI组件集合注册Click函数字符串
        /// </summary>
        /// <param name="uiComponents"></param>
        /// <returns></returns>
        private static string GetRegisterClickFuncs(List<UIComponent> uiComponents)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var uiComponent in uiComponents)
            {
                ButtonComponent button = uiComponent as ButtonComponent;
                if (button != null)
                {
                    stringBuilder.AppendLine("    " + button.GetRegisterClickStr());
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 获取UI组件集合Click函数字符串
        /// </summary>
        /// <param name="uiComponents"></param>
        /// <returns></returns>
        private static string GetClickFuncs(List<UIComponent> uiComponents)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var uiComponent in uiComponents)
            {
                ButtonComponent button = uiComponent as ButtonComponent;
                if (button != null)
                {
                    stringBuilder.AppendLine(button.GetClickStr());
                }
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
        /// <param name="parent"></param>
        /// <param name="uiComponents"></param>
        private static void CreateUIComponent(Transform parent, ref List<UIComponent> uiComponents)
        {
            foreach (Transform child in parent.transform)
            {
                CreateUIComponent(child, ref uiComponents);

                if (child.name.Contains("#") == true)
                {
                    string uiName = child.name.Split('#')[0];
                    string uiComponentTypeStr = child.name.Split('#')[1];

                    if (m_UIComponentTypeDict.TryGetValue(uiComponentTypeStr, out Type uiComponentType) == true)
                    {
                        UIComponent uiComponent = System.Activator.CreateInstance(uiComponentType) as UIComponent;
                        uiComponent.PrefabName = m_PrefabName;
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
