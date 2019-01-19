using System;

namespace AutoCSer.WebView
{
    /// <summary>
    /// #!转换URL
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct HashUrl
    {
        /// <summary>
        /// URL路径
        /// </summary>
        public string Path;
        /// <summary>
        /// URL查询
        /// </summary>
        public string Query;
        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Query == null) return Path;
            return Path + "#!" + Query;
        }
    }
}
