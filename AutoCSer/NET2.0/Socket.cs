using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 套接字扩展
    /// </summary>
    public static class SocketExtension
    {
        /// <summary>
        /// 关闭 System.Net.Sockets.Socket 连接并释放所有关联的资源
        /// </summary>
        /// <param name="socket"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Dispose(this Socket socket)
        {
            //socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
	}
}
