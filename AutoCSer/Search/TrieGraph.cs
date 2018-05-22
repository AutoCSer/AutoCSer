using System;
using AutoCSer.Extension;

namespace AutoCSer.Search
{
    /// <summary>
    /// Trie 图
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="valueType">节点值类型</typeparam>
    public abstract partial class TrieGraph<keyType, valueType>
        where keyType : struct, IEquatable<keyType>
        where valueType : class
    {
        /// <summary>
        /// 根节点
        /// </summary>
        internal readonly Node Boot = new Node();
        /// <summary>
        /// 建图
        /// </summary>
        /// <param name="threadCount">并行线程数量</param>
        /// <param name="log">日志处理</param>
        internal void BuildGraph(int threadCount, AutoCSer.Log.ILog log)
        {
            if (Boot.Nodes != null)
            {
                if (threadCount > AutoCSer.Threading.Pub.CpuCount) threadCount = AutoCSer.Threading.Pub.CpuCount;
                if (threadCount > 1) buildGraph(threadCount, log ?? AutoCSer.Log.Pub.Log);
                else buildGraph();
            }
        }
        /// <summary>
        /// 单线程建图
        /// </summary>
        private unsafe void buildGraph()
        {
            Builder builder = new Builder(Boot);
            for (LeftArray<Node> reader = new LeftArray<Node>(Boot.Nodes.Values.getArray()); reader.Length != 0; reader.Exchange(ref builder.Writer))
            {
                builder.Set(ref reader);
                builder.Build();
            }
        }
        /// <summary>
        /// 多线程并行建图
        /// </summary>
        /// <param name="threadCount">并行线程数量</param>
        /// <param name="log">日志处理</param>
        private void buildGraph(int threadCount, AutoCSer.Log.ILog log)
        {
            LeftArray<Node> reader = new LeftArray<Node>(Boot.Nodes.Values.getArray());
            int taskCount = threadCount - 1;
            bool isError = false;
            AutoCSer.Threading.AutoWaitCount waitCount = new AutoCSer.Threading.AutoWaitCount(taskCount);
            ThreadBuilder[] builders = new ThreadBuilder[threadCount];
            try
            {
                for (int builderIndex = 0; builderIndex != builders.Length; builders[builderIndex++] = new ThreadBuilder(Boot, waitCount)) ;
                do
                {
                    Node[] readerArray = reader.Array;
                    int count = reader.Length / threadCount, index = 0;
                    for (int builderIndex = 0; builderIndex != taskCount; ++builderIndex)
                    {
                        builders[builderIndex].SetThread(readerArray, index, count);
                        index += count;
                    }
                    builders[taskCount].Set(readerArray, index, reader.Length);
                    builders[taskCount].Build();
                    waitCount.WaitSet(taskCount);
                    reader.Length = 0;
                    foreach (ThreadBuilder builder in builders)
                    {
                        if (builder.ThreadException == null) reader.Add(ref builder.Writer);
                        else
                        {
                            log.Add(Log.LogType.Error, builder.ThreadException);
                            isError = true;
                        }
                    }
                }
                while (reader.Length != 0 && !isError);
            }
            finally
            {
                foreach (ThreadBuilder builder in builders)
                {
                    if (builder != null && builder.ThreadException  == null) builder.FreeThread();
                }
            }
        }
    }
}
