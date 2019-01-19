using System;

namespace AutoCSer.CsvFile
{
    /// <summary>
    /// 解析步骤状态
    /// </summary>
    internal enum ReaderStep : byte
    {
        /// <summary>
        /// 起始字符
        /// </summary>
        Start,
        /// <summary>
        /// 下一个字符
        /// </summary>
        Next,
        /// <summary>
        /// 换行忽略
        /// </summary>
        RowIgnore,
        /// <summary>
        /// 转义中
        /// </summary>
        Escape,
        /// <summary>
        /// 下一个转义符
        /// </summary>
        NextEscape,
        /// <summary>
        /// 下一个转义符换行忽略
        /// </summary>
        EscapeRowIgnore
    }
}
