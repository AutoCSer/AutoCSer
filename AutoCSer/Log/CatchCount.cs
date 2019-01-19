using System;
using AutoCSer.Extension;

namespace AutoCSer.Log
{
    /// <summary>
    /// 未处理异常计数
    /// </summary>
    internal unsafe static class CatchCount
    {
        /// <summary>
        /// 未处理异常类型
        /// </summary>
        internal enum Type : byte
        {
            /// <summary>
            /// 
            /// </summary>
            Log_Output,
            /// <summary>
            /// 
            /// </summary>
            SocketTimeoutLink_Dispose,
            /// <summary>
            /// 
            /// </summary>
            TcpServerSocket_Dispose,
            /// <summary>
            /// 
            /// </summary>
            TcpServerClientSocket_Dispose,
            /// <summary>
            /// 
            /// </summary>
            TcpClientSocket_Dispose,
            /// <summary>
            /// 
            /// </summary>
            Count,
        }
        /// <summary>
        /// 未处理异常类型计数
        /// </summary>
        private static Pointer counts;
        /// <summary>
        /// 添加未处理异常
        /// </summary>
        /// <param name="type"></param>
        internal static void Add(Type type)
        {
            if (counts.Data != null)
            {
                ulong value = ++counts.ULong[(byte)type];
                if (((uint)value & 1023) == 0) Log.Pub.Log.Add(Log.LogType.Debug | Log.LogType.Info, type.ToString() + "[" + value.toHex() + "]", null, null, CacheType.None);
            }
        }

        static CatchCount()
        {
            if (Log.Pub.Log.IsAnyType(Log.LogType.Debug | Log.LogType.Info)) counts = new Pointer { Data = Unmanaged.GetStatic64((byte)Type.Count * sizeof(long), true) };
        }
    }
}
