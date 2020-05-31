using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace UIScript
{
    public static class Utils
    {
        /// <summary>
        /// 格式化字符串，替代{数字}内容
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string CustomFormat(this string str, params string[] args)
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

        /// <summary>
        /// 移除路径中Assets前缀，保证Unity API直接调用
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string RemoveAssetsPrefix(this string path)
        {
            return path.Replace(Application.dataPath, "Assets");
        }
    }
}