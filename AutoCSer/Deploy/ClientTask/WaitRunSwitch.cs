using System;
using System.IO;
using System.Threading;

namespace AutoCSer.Deploy.ClientTask
{
    /// <summary>
    /// 等待运行程序切换结束 任务信息
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public sealed class WaitRunSwitch : Task
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public override TaskType Type { get { return TaskType.WaitRunSwitch; } }
        /// <summary>
        /// 任务信息索引位置
        /// </summary>
        public int TaskIndex;

        /// <summary>
        /// 等待运行程序切换结束
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        internal override DeployResultData Call(Timer timer)
        {
            FileInfo file = new FileInfo(((UpdateSwitchFile)timer.DeployInfo.Tasks.Array[TaskIndex]).WaitFile);
            if (file.Exists)
            {
                do
                {
                    try
                    {
                        using (FileStream fileStream = file.OpenWrite()) return DeployState.Success;
                    }
                    catch { }
                    Thread.Sleep(1);
                }
                while (true);
            }
            return DeployState.Success;
        }
    }
}
