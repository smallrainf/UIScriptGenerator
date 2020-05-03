using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

namespace UIScript
{
    public class UIScriptGenerator : Editor
    {
        private static Dictionary<string, Graph> m_GraphTypeDict = new Dictionary<string, Graph>();

        static UIScriptGenerator()
        {
            m_GraphTypeDict.Add("img", new Image());
            m_GraphTypeDict.Add("btn", new Button());
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

            List<Graph> graphs = CreateGraphList(go.transform);

            string luaTemplate = File.ReadAllText(Application.dataPath + "/UIScriptGenerator/Template/Lua.txt");

            string luaInstance = Format(luaTemplate, go.name, string.Empty, string.Empty, string.Empty, string.Empty);

            File.WriteAllText(Application.dataPath + "/Examples/Lua/" + go.name + ".lua", luaInstance);

            //AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private static List<Graph> CreateGraphList(Transform root)
        {
            List<Graph> graphs = new List<Graph>();

            CreateGraph(root, ref graphs);

            return graphs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="graphs"></param>
        private static void CreateGraph(Transform root, ref List<Graph> graphs)
        {
            foreach (Transform child in root.transform)
            {
                CreateGraph(child, ref graphs);

                if (child.name.Contains("#") == true)
                {
                    string graphTypeStr = child.name.Split('#')[1];
                    if (m_GraphTypeDict.TryGetValue(graphTypeStr, out Graph graph) == true)
                    {
                        graphs.Add(graph);
                    }
                    else
                    {
                        Debug.LogError("该类型目前不支持，graphTypeStr:" + graphTypeStr);
                    }
                }
            }
        }

        /// <summary>
        /// 格式化字符串，替代{数字}内容
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static string Format(string str, params string[] args)
        {
            string newStr = str;
            string rexStr = @"\{[0-9]+\}";
            MatchCollection mc = Regex.Matches(str, rexStr);
            if (mc.Count > 0)
            {
                for (int i = 0; i < mc.Count; i++)
                {
                    if (i < args.Length)
                    {
                        string subStr = "{" + i + "}";
                        newStr = newStr.Replace(subStr, args[i]);
                    }
                }
            }
            return newStr;
        }
    }
}
