using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 参数相关操作
    /// </summary>
    internal static partial class ParameterInfoExtension
    {
        /// <summary>
        /// 在文件当前目录启动进程
        /// </summary>
        /// <param name="parameter">参数信息</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static Type elementType(this ParameterInfo parameter)
        {
            Type type = parameter.ParameterType;
            return type.IsByRef ? type.GetElementType() : type;
        }
    }
}
