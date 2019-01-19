using System;
using AutoCSer.Extension;

namespace AutoCSer.Net.TcpServer.ClientCommand
{
    /// <summary>
    /// 客户端命令
    /// </summary>
    internal abstract class KeepCommand : Command
    {
        /// <summary>
        /// 非法回调命令索引
        /// </summary>
        internal const int NullIndex = -1;
        /// <summary>
        /// 合并回调命令索引
        /// </summary>
        internal const int MergeIndex = 0;
        /// <summary>
        /// 服务端自定义数据包回调命令索引
        /// </summary>
        internal const int CustomDataIndex = MergeIndex + 1;
        /// <summary>
        /// 命令索引起始位置
        /// </summary>
        internal const int CommandPoolIndex = CustomDataIndex + 1;
        /// <summary>
        /// 命令信息
        /// </summary>
        internal static readonly CommandInfo KeepCallbackCommandInfo = new CommandInfo { IsKeepCallback = 1 };

        /// <summary>
        /// 保持回调
        /// </summary>
        internal KeepCallback KeepCallback;
        ///// <summary>
        ///// 保持回调序号
        ///// </summary>
        //protected int keepCallbackIdentity;
        ///// <summary>
        ///// 保持回调
        ///// </summary>
        //internal bool SetKeepCallback()
        //{
        //    int index = (int)(CommandIndex & Server.CommandIndexAnd);
        //    try
        //    {
        //        KeepCallback = new KeepCallback(this, ++keepCallbackIdentity);
        //        return true;
        //    }
        //    catch (Exception error)
        //    {
        //        Socket.Log.add(Log.LogType.Error, error);
        //    }
        //    Socket.FreeIndex(index);
        //    return false;
        //}
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>下一个命令</returns>
        internal unsafe override CommandBase Build(ref SenderBuildInfo buildInfo)
        {
            UnmanagedStream stream = Socket.OutputSerializer.Stream;
            if ((buildInfo.SendBufferSize - stream.ByteSize) >= sizeof(int) + sizeof(uint))
            {
                CommandBase nextBuild = LinkNext;
                int commandIndex = Socket.CommandPool.Push(this);
                if (commandIndex != 0)
                {
                    if (KeepCallback.SetCommandIndex(commandIndex))
                    {
                        byte* write = stream.CurrentData;
                        *(int*)write = CommandInfo.Command;
                        *(uint*)(write + sizeof(int)) = (uint)commandIndex | (uint)(CommandInfo.CommandFlags | CommandFlags.NullData);
                        ++buildInfo.Count;
                        LinkNext = null;
                        stream.ByteSize += sizeof(int) + sizeof(uint);
                        return nextBuild;
                    }
                    Socket.CommandPool.Cancel(commandIndex);
                }
                else KeepCallback.BuildCancel();
                LinkNext = null;
                return nextBuild;
            }
            buildInfo.isFullSend = 1;
            return this;
        }
    }
}
