using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEditor;

namespace UIScript
{
    public class LuaUIScriptGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templatePath"></param>
        /// <param name="isUI"></param>
        public static void GenerateUIScript(string templatePath, bool isUI)
        {
            GameObject go = Selection.activeGameObject;
            // TODO:判断是否为Prefab根结点，目前Prefab编辑模式下会自动创建Canvas作为父节点
            if (go == null)
            {
                EditorUtility.DisplayDialog("Error", "请选中UIPrefab根结点", "ok");
                return;
            }

            List<UIComponent> uiComponents = UIScriptGenerator.CreateUIComponentList(go.transform);

            string defines = GetVariableDefines(uiComponents);
            string getOrSetFuncs = GetGetOrSetFuncs(uiComponents);
            string registerClickFuncs = GetRegisterClickFuncs(uiComponents);
            string clickFuncs = GetClickFuncs(uiComponents);

            string luaTemplate = File.ReadAllText(templatePath);

            string newLuaInstance = luaTemplate.CustomFormat(go.name, defines, getOrSetFuncs, registerClickFuncs, clickFuncs);

            // Unity识别.txt和.bytes文件后缀来获取TextAsset，因此这里需要采用.lua.txt文件后缀名
            string targetLuaPath = Application.dataPath + "/Examples/Lua/" + go.name + ".lua.txt";
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

                int startIndex = oldLuaInstance.IndexOf("--!@#clickstart");
                int endIndex = oldLuaInstance.LastIndexOf("--!@#clickend");
                string oldClickSubStr = oldLuaInstance.Substring(startIndex + 16, endIndex - (startIndex + 16));
                MatchCollection mc = Regex.Matches(oldClickSubStr, @"function(.|\n)*?end");
                startIndex = newLuaInstance.IndexOf("--!@#clickstart");
                endIndex = newLuaInstance.LastIndexOf("--!@#clickend");
                string newClickSubStr = newLuaInstance.Substring(startIndex + 16, endIndex - (startIndex + 16));
                MatchCollection mc2 = Regex.Matches(newClickSubStr, @"function(.|\n)*?end");
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var m2 in mc2)
                {
                    string pendingStr = m2.ToString();
                    foreach (var m1 in mc)
                    {
                        if (m1.ToString().Split('(')[0] == m2.ToString().Split('(')[0])
                        {
                            pendingStr = m1.ToString();
                            break;
                        }
                    }
                    stringBuilder.AppendLine(pendingStr);
                }

                newLuaInstance = oldLuaInstance.Replace(oldSubStr, newSubStr);
                newLuaInstance = newLuaInstance.Replace(oldClickSubStr, stringBuilder.ToString());
            }

            File.WriteAllText(targetLuaPath, newLuaInstance);
            AssetDatabase.Refresh();

            if (isUI == true)
            {
                LuaCtrlBase ctrlBase = AddUIScript<LuaCtrlBase>(go);
                SetUIScriptData(ctrlBase, uiComponents, targetLuaPath);
            }
            else
            {
                LuaSubCtrlBase ctrlBase = AddUIScript<LuaSubCtrlBase>(go);
                SetItemScriptData(ctrlBase, uiComponents, targetLuaPath);
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
        /// <param name="luaFilePath"></param>
        private static void SetUIScriptData(LuaCtrlBase ctrlBase, List<UIComponent> uiComponents, string luaFilePath)
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

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(luaFilePath.RemoveAssetsPrefix());
            ctrlBase.m_LuaFile = textAsset;
        }

        /// <summary>
        /// 设置Item脚本变量
        /// </summary>
        /// <param name="subCtrlBase"></param>
        /// <param name="uiComponents"></param>
        /// <param name="luaFilePath"></param>
        private static void SetItemScriptData(LuaSubCtrlBase subCtrlBase, List<UIComponent> uiComponents, string luaFilePath)
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

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(luaFilePath.RemoveAssetsPrefix());
            subCtrlBase.m_LuaFile = textAsset;
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
    }
}
