using System;

namespace AutoCSer.WebView
{
    /// <summary>
    /// 树节点标识类型
    /// </summary>
    internal enum ViewTreeTagType : byte
    {
        /// <summary>
        /// 普通HTML段
        /// </summary>
        Html,
        /// <summary>
        /// 注释子段
        /// </summary>
        Note,
        /// <summary>
        /// =@取值代码
        /// </summary>
        At,
    }
}
