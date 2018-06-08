using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 队列消费节点
    /// </summary>
    internal abstract class QueueNode : Node
    {
        /// <summary>
        /// 队列数据 写文件
        /// </summary>
        internal File.QueueWriter Writer;
        /// <summary>
        /// 队列消费节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        protected QueueNode(Cache.Node parent, ref OperationParameter.NodeParser parser) : base(parent, ref parser) { }
        /// <summary>
        /// 初始化
        /// </summary>
        protected override void start()
        {
            AutoCSer.Threading.ThreadPool.Tiny.Start((Writer = new File.QueueWriter(this)).Start);
        }
        /// <summary>
        /// 删除节点操作
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onRemoved()
        {
            isRemoved = true;
            if (Writer != null) Writer.TryDispose();
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="dataType"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void enqueue(ref OperationParameter.NodeParser parser, AutoCSer.CacheServer.ValueData.DataType dataType)
        {
            if (parser.ValueData.Type == dataType)
            {
                if (parser.OnReturn != null) Writer.Append(new Buffer(this, ref parser));
            }
            else parser.ReturnParameter.Type = ReturnType.ValueDataLoadError;
        }
        /// <summary>
        /// 消息队列设置当前读取数据标识
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void setDequeueIdentity(ref OperationParameter.NodeParser parser, File.QueueReader reader)
        {
            if (reader != null && !reader.IsDisposed && parser.ValueData.Type == ValueData.DataType.ULong) reader.TrySetIdentity(parser.ValueData.Int64.ULong);
        }
        /// <summary>
        /// 获取队列数据 读取配置
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected Config.QueueReader getReaderConfig(ref OperationParameter.NodeParser parser)
        {
            if (parser.OnReturn != null)
            {
                if (parser.ValueData.Type == ValueData.DataType.Json)
                {
                    Config.QueueReader config = null;
                    if (parser.ValueData.GetJson(ref config) && config != null)
                    {
                        if (Writer != null)
                        {
                            if (!Writer.IsDisposed) return config;
                            else parser.ReturnParameter.Type = ReturnType.MessageQueueDisposed;
                        }
                        else parser.ReturnParameter.Type = ReturnType.MessageQueueNotFoundWriter;
                        return null;
                    }
                }
                parser.ReturnParameter.Type = ReturnType.ValueDataLoadError;
            }
            return null;
        }
    }
}
