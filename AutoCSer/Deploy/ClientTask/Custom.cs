using System;

namespace AutoCSer.Deploy.ClientTask
{
    /// <summary>
    /// 自定义任务处理 任务信息
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public sealed class Custom : Task
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public override TaskType Type { get { return TaskType.Custom; } }
        /// <summary>
        /// 自定义调用名称
        /// </summary>
        public string CallName;
        /// <summary>
        /// 自定义参数数据
        /// </summary>
        public byte[] CustomData;

        /// <summary>
        /// TCP 内部服务套接字数据发送
        /// </summary>
        [AutoCSer.BinarySerialize.IgnoreMember]
        internal AutoCSer.Net.TcpInternalServer.ServerSocketSender Sender;
        /// <summary>
        /// 自定义任务处理
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        internal override DeployResultData Call(Timer timer)
        {
            return timer.Server.CallCustomTask(this);
        }
    }
}
