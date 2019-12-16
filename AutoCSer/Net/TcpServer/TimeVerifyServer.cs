using System;
using AutoCSer.Extension;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 时间验证服务
    /// </summary>
    public static unsafe class TimeVerifyServer
    {
        /// <summary>
        /// 时间验证函数 命令序号
        /// </summary>
        public const int CommandIdentity = 1;
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="value"></param>
        /// <param name="randomPrefix"></param>
        /// <param name="ticks"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static byte[] Md5(string value, ulong randomPrefix, long ticks)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider()) return Md5(md5, value, randomPrefix, ticks);
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="md5"></param>
        /// <param name="value"></param>
        /// <param name="randomPrefix"></param>
        /// <param name="ticks"></param>
        /// <returns></returns>
        internal static byte[] Md5(MD5CryptoServiceProvider md5, string value, ulong randomPrefix, long ticks)
        {
            SubBuffer.PoolBufferFull buffer = default(SubBuffer.PoolBufferFull);
            SubBuffer.Pool.GetBuffer(ref buffer, (value.Length << 1) + (sizeof(ulong) + sizeof(long)));
            try
            {
                fixed (char* valueFixed = value)
                fixed (byte* dataFixed = buffer.Buffer)
                {
                    byte* start = dataFixed + buffer.StartIndex;
                    *(ulong*)start = randomPrefix;
                    *(long*)(start + sizeof(ulong)) = ticks;
                    AutoCSer.Extension.StringExtension.SimpleCopyNotNull64(valueFixed, (char*)(start + (sizeof(ulong) + sizeof(long))), value.Length);
                }
                return md5.ComputeHash(buffer.Buffer, buffer.StartIndex, (value.Length << 1) + (sizeof(ulong) + sizeof(long)));
            }
            finally { buffer.PoolBuffer.Free(); }
        }
        /// <summary>
        /// 判断 MD5 值是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ulong IsMd5(byte[] left, byte[] right)
        {
            fixed (byte* leftFixed = left, rightRixed = right)
            {
                return (*(ulong*)leftFixed ^ *(ulong*)rightRixed) | (*(ulong*)(leftFixed + sizeof(ulong)) ^ *(ulong*)(rightRixed + sizeof(ulong)));
            }
        }
    }
    /// <summary>
    /// 时间验证服务
    /// </summary>
    public abstract unsafe class TimeVerifyServer<serverType, serverSocketType, serverSocketSenderType> : IDisposable
#if !NOJIT
        , ISetTcpServer<serverType>
#endif
        where serverType : Server<serverType, serverSocketSenderType>
        where serverSocketType : ServerSocket<serverType, serverSocketType, serverSocketSenderType>
        where serverSocketSenderType : ServerSocketSender<serverType, serverSocketType, serverSocketSenderType>
    {
        /// <summary>
        /// TCP 服务端
        /// </summary>
        protected serverType server;
        /// <summary>
        /// TCP 服务端
        /// </summary>
        internal serverType TcpServer
        {
            get { return server; }
        }
        /// <summary>
        /// 设置TCP服务端
        /// </summary>
        /// <param name="server">TCP服务端</param>
        public virtual void SetTcpServer(serverType server)
        {
            this.server = server;
        }
        /// <summary>
        /// MD5 加密
        /// </summary>
        private MD5CryptoServiceProvider md5;
        /// <summary>
        /// 验证时间戳
        /// </summary>
        private TimeVerifyTick timeVerifyTick = new TimeVerifyTick(Date.NowTime.UtcNow.Ticks - 1);
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            if (md5 != null)
            {
                md5.Dispose();
                md5 = null;
            }
        }
        /// <summary>
        /// 时间验证函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="userID">用户ID</param>
        /// <param name="randomPrefix">随机前缀</param>
        /// <param name="md5Data">MD5 数据</param>
        /// <param name="ticks">验证时钟周期</param>
        /// <returns>是否验证成功</returns>
        [AutoCSer.Net.TcpOpenServer.Method(IsVerifyMethod = true, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, CommandIdentity = TimeVerifyServer.CommandIdentity)]
        [Method(IsVerifyMethod = true, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, CommandIdentity = TimeVerifyServer.CommandIdentity)]
        protected virtual bool verify(serverSocketSenderType sender, string userID, ulong randomPrefix, byte[] md5Data, ref long ticks)
        {
            return server.CheckVerifyString() || verify(sender, randomPrefix, server.Attribute.VerifyString, md5Data, ref ticks);
        }
        /// <summary>
        /// 时间验证函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="randomPrefix">随机前缀</param>
        /// <param name="verifyString">验证字符串</param>
        /// <param name="md5Data">MD5 数据</param>
        /// <param name="ticks">验证时钟周期</param>
        /// <returns>是否验证成功</returns>
        protected bool verify(serverSocketSenderType sender, ulong randomPrefix, string verifyString, byte[] md5Data, ref long ticks)
        {
            if (md5Data != null && md5Data.Length == 16)
            {
                if (!timeVerifyTick.Check(ref ticks, ref sender.TimeVerifyTicks)) return false;
                if (md5 == null) md5 = new MD5CryptoServiceProvider();
                if (TimeVerifyServer.IsMd5(TimeVerifyServer.Md5(md5, verifyString, randomPrefix, ticks), md5Data) == 0)
                {
                    timeVerifyTick.Set(ticks);
                    if (!server.Attribute.IsMarkData || sender.SetMarkData(server.ServerAttribute.VerifyHashCode ^ randomPrefix)) return true;
                }
            }
            ticks = 0;
            return false;
        }
    }
}
