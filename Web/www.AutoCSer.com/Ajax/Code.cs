using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace AutoCSer.Web.Ajax
{
    /// <summary>
    /// 获取代码
    /// </summary>
    abstract class Code<codeType> : AutoCSer.WebView.Ajax<codeType>
        where codeType: Code<codeType>
    {
        /// <summary>
        /// 示例代码集合
        /// </summary>
        protected static readonly Dictionary<string, string> codes = AutoCSer.DictionaryCreator.CreateOnly<string, string>();
        /// <summary>
        /// 示例代码访问锁
        /// </summary>
        protected static readonly object codeLock = new object();
        /// <summary>
        /// 获取示例代码
        /// </summary>
        /// <param name="path">代码路径</param>
        /// <param name="file">文件名称</param>
        /// <returns>示例代码</returns>
        protected static string getCode(string path, string file)
        {
            if (file != null)
            {
                string code;
                file = file.FileNameToLower();
                Monitor.Enter(codeLock);
                if (codes.TryGetValue(file, out code)) Monitor.Exit(codeLock);
                else
                {
                    try
                    {
                        FileInfo fileInfo = new FileInfo(path + file);
                        if (fileInfo.Exists)
                        {
                            string fileName = fileInfo.FullName;
                            if (path.PathCompare(fileName) == 0) codes.Add(file, code = File.ReadAllText(fileName));
                        }
                    }
                    finally { Monitor.Exit(codeLock); }
                }
                return code;
            }
            return null;
        }
    }
}
