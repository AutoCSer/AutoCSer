using System;

namespace AutoCSer.Net.TcpServer.ServerOutput
{
    /// <summary>
    /// TCP 服务端套接字输出信息
    /// </summary>
    internal unsafe sealed class CustomDataOutput : OutputLink
    {
        /// <summary>
        /// 输出参数
        /// </summary>
        internal SubArray<byte> Data;
        /// <summary>
        /// 创建输出信息
        /// </summary>
        /// <param name="sender">TCP 服务套接字数据发送</param>
        /// <param name="buildInfo">输出创建参数</param>
        internal override OutputLink Build(ServerSocketSenderBase sender, ref SenderBuildInfo buildInfo)
        {
            UnmanagedStream stream = sender.OutputSerializer.Stream;
            int dataSize = (Data.Length + (sizeof(int) + 3)) & (int.MaxValue), prepLength = dataSize + (sizeof(uint) + sizeof(int));
            if (buildInfo.Count == 0 || (buildInfo.SendBufferSize - stream.ByteSize) >= prepLength
                || (stream.FreeSize >= prepLength && prepLength > buildInfo.SendBufferSize - TcpServer.ServerOutput.OutputLink.StreamStartIndex))
            {
                OutputLink nextBuild = LinkNext;
                stream.PrepLength(prepLength);
                byte* write = stream.CurrentData;
                *(uint*)write = ClientCommand.KeepCommand.CustomDataIndex;
                *(int*)(write + sizeof(uint)) = dataSize;
                *(int*)(write + (sizeof(uint) + sizeof(int))) = Data.Length;
                if (Data.Length != 0)
                {
                    fixed (byte* dataFixed = Data.Array) Memory.CopyNotNull(dataFixed + Data.Start, write + (sizeof(uint) + sizeof(int) * 2), Data.Length);
                }
                stream.ByteSize += dataSize + (sizeof(uint) + sizeof(int));
                ++buildInfo.Count;
                LinkNext = null;
                Data.Array = null;
                AutoCSer.Threading.RingPool<CustomDataOutput>.Default.PushNotNull(this);
                return nextBuild;
            }
            buildInfo.isFullSend = 1;
            return this;
        }
        /// <summary>
        /// 释放 TCP 服务端套接字输出信息
        /// </summary>
        /// <returns></returns>
        protected override OutputLink free()
        {
            OutputLink next = LinkNext;
            Data.Array = null;
            LinkNext = null;
            AutoCSer.Threading.RingPool<CustomDataOutput>.Default.PushNotNull(this);
            return next;
        }
    }
}
