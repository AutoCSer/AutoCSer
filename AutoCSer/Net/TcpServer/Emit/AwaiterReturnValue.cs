using System;

namespace AutoCSer.Net.TcpServer.Emit
{
    /// <summary>
    /// await 返回值包装
    /// </summary>
    /// <typeparam name="returnType">返回值类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct AwaiterReturnValue<returnType>
    {
        /// <summary>
        /// 返回值
        /// </summary>
        public returnType Return;
    }
}
