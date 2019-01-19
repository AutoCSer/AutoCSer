using System;

namespace AutoCSer.WebView
{
    /// <summary>
    /// HTML模板命令类型
    /// </summary>
    internal enum ViewTreeCommand : byte
    {
        /// <summary>
        /// 输出绑定的数据
        /// </summary>
        At,
        /// <summary>
        /// 客户端代码段
        /// </summary>
        Client,
        /// <summary>
        /// 子代码段
        /// </summary>
        Value,
        /// <summary>
        /// 循环
        /// </summary>
        Loop,
        /// <summary>
        /// 循环=LOOP
        /// </summary>
        For,
        /// <summary>
        /// 屏蔽代码段输出
        /// </summary>
        Note,
        /// <summary>
        /// 绑定的数据为true非0非null时输出代码
        /// </summary>
        If,
        /// <summary>
        /// 绑定的数据为false或者0或者null时输出代码
        /// </summary>
        Not,
    }
}
