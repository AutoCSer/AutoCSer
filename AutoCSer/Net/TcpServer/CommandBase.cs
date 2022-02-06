using System;
using System.Net;
using AutoCSer.Log;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
using System.Net.Sockets;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 组件基类
    /// </summary>
    public abstract class CommandBase : IDisposable
    {
        /// <summary>
        /// TCP 服务调用配置
        /// </summary>
        internal readonly ServerBaseAttribute Attribute;
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServerName { get { return Attribute.ServerName; } }
        /// <summary>
        /// 日志接口
        /// </summary>
        internal readonly ILog Log;
        /// <summary>
        /// 发送数据缓存区最大字节大小
        /// </summary>
        internal readonly int SendBufferMaxSize;
        /// <summary>
        /// 压缩启用最低字节数量
        /// </summary>
        internal readonly int MinCompressSize;

        /// <summary>
        /// 发送数据 new 缓冲区次数
        /// </summary>
        internal int SendNewBufferCount;
        /// <summary>
        /// 是否已经关闭
        /// </summary>
        internal volatile int IsDisposed;
        /// <summary>
        /// 接收数据 new 缓冲区次数
        /// </summary>
        internal int ReceiveNewBufferCount;
#if !NOJIT
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        internal CommandBase() { }
#endif
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        /// <param name="attribute">TCP 服务调用配置</param>
        /// <param name="sendBufferMaxSize">发送数据缓存区最大字节大小</param>
        /// <param name="log">日志接口</param>
        internal CommandBase(ServerBaseAttribute attribute, int sendBufferMaxSize, ILog log)
        {
            Attribute = attribute;
            MinCompressSize = attribute.GetMinCompressSize;
            Log = log ?? AutoCSer.LogHelper.Default;
            SendBufferMaxSize = sendBufferMaxSize;
            if (MinCompressSize <= 0) MinCompressSize = int.MaxValue;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            IsDisposed = 1;
            Log.Debug("释放TCP客户端", LogLevel.Debug | LogLevel.AutoCSer);
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="error"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void AddLog(Exception error)
        {
            Log.Exception(error);
        }

        /// <summary>
        /// 关闭套接字
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ShutdownClient(Socket socket)
        {
            shutdown(socket, true);
        }
        /// <summary>
        /// 关闭套接字
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="isLog">正常关闭不写日志</param>
        private static void shutdown(Socket socket, bool isLog)
        {
            try
            {
                if(socket.Connected) socket.Shutdown(SocketShutdown.Both);
            }
            catch(Exception error)
            {
                if (isLog) AutoCSer.LogHelper.Exception(error, null, LogLevel.Debug | LogLevel.Info | LogLevel.AutoCSer);
            }
            finally { socket.Dispose(); }
        }
        /// <summary>
        /// 关闭套接字
        /// </summary>
        /// <param name="socket"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ShutdownServer(Socket socket)
        {
            shutdown(socket, false);
        }
        /// <summary>
        /// 关闭套接字
        /// </summary>
        /// <param name="socket"></param>
#if !DotNetStandard
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
#endif
        internal static void CloseServer(Socket socket)
        {
#if DotNetStandard
            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch { }
            finally { socket.Dispose(); }
#else
            socket.Dispose();
#endif
        }
        /// <summary>
        /// 变换数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="markData"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void Mark(ref SubArray<byte> data, ulong markData)
        {
            fixed (byte* dataFixed = data.GetFixedBuffer()) Mark(dataFixed + data.Start, markData, data.Length);
        }
        /// <summary>
        /// 变换数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="markData"></param>
        /// <param name="startIndex"></param>
        /// <param name="dataLength"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void Mark(byte[] data, ulong markData, int startIndex, int dataLength)
        {
            fixed (byte* dataFixed = data) Mark(dataFixed + startIndex, markData, dataLength);
        }
        /// <summary>
        /// 变换数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="markData"></param>
        /// <param name="dataLength"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void Mark(byte* data, ulong markData, int dataLength)
        {
            if (((int)data & 7) == 4) Mark32(data, markData, dataLength);
            else Mark64(data, markData, dataLength);
        }
        /// <summary>
        /// 变换数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="markData"></param>
        /// <param name="dataLength"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void Mark32(byte* data, ulong markData, int dataLength)
        {
            *(uint*)data ^= (uint)markData;
            Mark64(data + sizeof(int), ((ulong)(uint)markData << 32) | markData >> 32, dataLength - sizeof(uint));
        }
        /// <summary>
        /// 变换数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="markData"></param>
        /// <param name="dataLength"></param>
        internal unsafe static void Mark64(byte* data, ulong markData, int dataLength)
        {
            for (byte* end = data + (dataLength & (int.MaxValue - (sizeof(ulong) * 4 - 1))); data != end; data += sizeof(ulong) * 4)
            {
                *(ulong*)data ^= markData;
                *(ulong*)(data + sizeof(ulong)) ^= markData;
                *(ulong*)(data + sizeof(ulong) * 2) ^= markData;
                *(ulong*)(data + sizeof(ulong) * 3) ^= markData;
            }
            if ((dataLength & (sizeof(ulong) * 2)) != 0)
            {
                *(ulong*)data ^= markData;
                *(ulong*)(data + sizeof(ulong)) ^= markData;
                data += sizeof(ulong) * 2;
            }
            if ((dataLength & sizeof(ulong)) != 0)
            {
                *(ulong*)data ^= markData;
                data += sizeof(ulong);
            }
            if ((dataLength &= (sizeof(ulong) - 1)) != 0)
            {
                if ((dataLength & sizeof(uint)) != 0)
                {
                    *(uint*)data ^= (uint)markData;
                    data += sizeof(uint);
                    markData >>= 32;
                }
                if ((dataLength & sizeof(ushort)) != 0)
                {
                    *(ushort*)data ^= (ushort)markData;
                    data += sizeof(ushort);
                    markData >>= 16;
                }
                if ((dataLength & 1) != 0) *(byte*)data ^= (byte)markData;
            }
        }
        /// <summary>
        /// 序列化预编译
        /// </summary>
        /// <param name="simpleDeSerializeTypes"></param>
        /// <param name="simpleSerializeTypes"></param>
        /// <param name="deSerializeTypes"></param>
        /// <param name="serializeTypes"></param>
        /// <param name="jsonDeSerializeTypes"></param>
        /// <param name="jsonSerializeTypes"></param>
        internal protected static void CompileSerialize(Type[] simpleDeSerializeTypes, Type[] simpleSerializeTypes, Type[] deSerializeTypes, Type[] serializeTypes, Type[] jsonDeSerializeTypes, Type[] jsonSerializeTypes)
        {
            if (simpleDeSerializeTypes.Length > 1) AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(simpleSerializeTypes, AutoCSer.Threading.ThreadTaskType.CompileSimpleDeSerialize);
            if (simpleSerializeTypes.Length > 1) AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(simpleSerializeTypes, AutoCSer.Threading.ThreadTaskType.CompileSimpleSerialize);
            if (deSerializeTypes.Length > 1) AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(simpleSerializeTypes, AutoCSer.Threading.ThreadTaskType.CompileBinaryDeSerialize);
            if (serializeTypes.Length > 1) AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(simpleSerializeTypes, AutoCSer.Threading.ThreadTaskType.CompileBinarySerialize);
            if (jsonDeSerializeTypes.Length > 1) AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(simpleSerializeTypes, AutoCSer.Threading.ThreadTaskType.CompileJsonDeSerialize);
            if (jsonSerializeTypes.Length > 1) AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(simpleSerializeTypes, AutoCSer.Threading.ThreadTaskType.CompileJsonSerialize);
        }
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
        public void ClientCompileSerialize(Type[] simpleSerializeTypes, Type[] simpleDeSerializeTypes, Type[] serializeTypes, Type[] deSerializeTypes, Type[] jsonSerializeTypes, Type[] jsonDeSerializeTypes)
        {
            CompileSerialize(simpleDeSerializeTypes, simpleSerializeTypes, deSerializeTypes, serializeTypes, jsonDeSerializeTypes, jsonSerializeTypes);
        }
    }
}
