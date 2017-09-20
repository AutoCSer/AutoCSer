using System;
using AutoCSer.Net;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.DiskBlock
{
    /// <summary>
    /// 磁盘块服务
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Name = Server.ServerName, Host = "127.0.0.1", Port = (int)ServerPort.DiskBlock, IsMarkData = true)]
    public partial class Server : AutoCSer.Net.TcpInternalServer.TimeVerifyServer
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public const string ServerName = "DiskBlock";
        /// <summary>
        /// 磁盘块编号位移数量
        /// </summary>
        internal const int IndexShift = 48;
        /// <summary>
        /// 磁盘块最大索引位置
        /// </summary>
        internal const ulong MaxIndex = (1UL << IndexShift) - 1;
        /// <summary>
        /// 磁盘块编号
        /// </summary>
        private int index;
        /// <summary>
        /// 磁盘块处理接口
        /// </summary>
        private IBlock block;
        /// <summary>
        /// 磁盘块服务
        /// </summary>
        /// <param name="block">磁盘块处理接口</param>
        public Server(IBlock block = null)
        {
            index = block.Index;
            this.block = block;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="buffer">缓冲区，Start 指定字节数量</param>
        /// <param name="index">索引位置</param>
        /// <param name="onRead">获取数据回调委托</param>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox | AutoCSer.Net.TcpServer.ParameterFlags.ClientAsynchronousReturnInput)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void read(ClientBuffer buffer, ulong index, Func<AutoCSer.Net.TcpServer.ReturnValue<ClientBuffer>, bool> onRead)
        {
            if ((int)(index >> IndexShift) == this.index) block.Read((long)(index & MaxIndex), buffer.Buffer.Start, onRead);
            else onRead(new ClientBuffer { State = MemberState.BlockIndexError });
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="buffer">数据</param>
        /// <param name="onWrite">添加数据回调委托</param>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void append(AppendBuffer buffer, Func<AutoCSer.Net.TcpServer.ReturnValue<ulong>, bool> onWrite)
        {
            if (buffer.CheckBlockIndex(index)) block.Append(ref buffer, onWrite);
            else onWrite(0);
        }
        /// <summary>
        /// 创建磁盘块服务
        /// </summary>
        /// <param name="block">磁盘块处理接口</param>
        /// <returns>磁盘块服务</returns>
        public static TcpInternalServer Create(IBlock block)
        {
            AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig(ServerName, typeof(AutoCSer.DiskBlock.Server));
            int index = block.Index;
            if (index != 0)
            {
                attribute = AutoCSer.MemberCopy.Copyer<Net.TcpInternalServer.ServerAttribute>.MemberwiseClone(attribute);
                attribute.Name += index.toString();
            }
            return new TcpInternalServer(attribute, null, new Server(block));
        }
    }
}
