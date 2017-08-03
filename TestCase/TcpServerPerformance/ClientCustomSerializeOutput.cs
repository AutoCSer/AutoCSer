using System;
using System.Threading;

namespace AutoCSer.TestCase.TcpInternalServerPerformance
{
    /// <summary>
    /// 自定义序列化计算回调输出
    /// </summary>
    public unsafe sealed class ClientCustomSerializeOutput
    {
        /// <summary>
        /// 
        /// </summary>
        internal int left;
        /// <summary>
        /// 
        /// </summary>
        internal int right;
        /// <summary>
        /// 添加客户端自定义序列化
        /// </summary>
        private Action<ClientCustomSerialize> addCustomSerialize;
        /// <summary>
        /// 
        /// </summary>
        private readonly EventWaitHandle wait = new EventWaitHandle(false, EventResetMode.ManualReset);
        /// <summary>
        /// 自定义序列化计算回调输出
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="addCustomSerialize"></param>
        public ClientCustomSerializeOutput(int left, int right, Action<ClientCustomSerialize> addCustomSerialize)
        {
            this.left = left;
            this.right = right;
            this.addCustomSerialize = addCustomSerialize;
            addCustomSerialize(new ClientCustomSerialize { Output = this });
        }
        /// <summary>
        /// 自定义序列化输出
        /// </summary>
        /// <param name="serializer"></param>
        internal void Serialize(AutoCSer.BinarySerialize.Serializer serializer)
        {
            UnmanagedStream stream = serializer.Stream;
            int count = Math.Min((stream.FreeSize - sizeof(int) * 2) / (sizeof(int) * 2), right), endRight = right - count;
            if (endRight > 0)
            {
                //Thread.Sleep(0);
                addCustomSerialize(new ClientCustomSerialize { Output = this });
            }
            else
            {
                wait.Set();
                endRight = 0;
            }
            int size = (count * (sizeof(int) * 2)) + sizeof(int);
            byte* write = stream.CurrentData;
            *(int*)write = size;
            write += sizeof(int);
            while (right != endRight)
            {
                *(int*)write = left;
                *(int*)(write + sizeof(int)) = --right;
                write += sizeof(int) * 2;
            }
            stream.MoveSize(size);
        }
        /// <summary>
        /// 等待输出结束
        /// </summary>
        public void Wait()
        {
            wait.WaitOne();
        }
    }
}
