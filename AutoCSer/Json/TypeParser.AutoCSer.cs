using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Json
{
    /// <summary>
    /// 类型解析器
    /// </summary>
    internal unsafe static partial class TypeParser<valueType>
    {
        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="parser">Json解析器</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ParseTcpServer(Parser parser, ref valueType value)
        {
            if (DefaultParser == null)
            {
                if (parser.SearchObject()) ParseMembers(parser, ref value);
                else value = default(valueType);
            }
            else DefaultParser(parser, ref value);
        }
        /// <summary>
        /// 预编译
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Compile() { }
    }
}
