using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 客户端
    /// </summary>
    public abstract class MethodClient
    {
        /// <summary>
        /// 序列化预编译
        /// </summary>
        /// <param name="simpleSerializeTypes"></param>
        /// <param name="simpleDeSerializeTypes"></param>
        /// <param name="serializeTypes"></param>
        /// <param name="deSerializeTypes"></param>
        /// <param name="jsonSerializeTypes"></param>
        /// <param name="jsonDeSerializeTypes"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected static void _compileSerialize_(Type[] simpleSerializeTypes, Type[] simpleDeSerializeTypes, Type[] serializeTypes, Type[] deSerializeTypes, Type[] jsonSerializeTypes, Type[] jsonDeSerializeTypes)
        {
            CommandBase.CompileSerialize(simpleDeSerializeTypes, simpleSerializeTypes, deSerializeTypes, serializeTypes, jsonDeSerializeTypes, jsonSerializeTypes);
        }
    }
}
