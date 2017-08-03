using System;

namespace AutoCSer.Sql.Excel
{
    /// <summary>
    /// 混合数据处理方式
    /// </summary>
    public enum Intermixed : byte
    {
        /// <summary>
        /// 输出模式，此情况下只能用作写入Excel
        /// </summary>
        Write = 0,
        /// <summary>
        /// 输入模式，此情况下只能用作读取Excel，并且始终将Excel数据作为文本类型读取
        /// </summary>
        Read = 1,
        /// <summary>
        /// 连接模式，此情况下既可用作写入、也可用作读取
        /// </summary>
        WriteAndRead = 2
    }
}
