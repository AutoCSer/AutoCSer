using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// Trie 图
    /// </summary>
    public abstract partial class TrieGraph<keyType, valueType>
    {
        /// <summary>
        /// Trie 图创建器
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        internal class ThreadBuilder : Builder
        {
            /// <summary>
            /// 建图线程等待
            /// </summary>
            private AutoCSer.Threading.AutoWaitHandle threadWait;
            /// <summary>
            /// 建图线程任务完成计数
            /// </summary>
            private readonly AutoCSer.Threading.AutoWaitCount waitCount;
            /// <summary>
            /// 建图线程异常
            /// </summary>
            public Exception ThreadException;
            /// <summary>
            /// Trie 图创建器
            /// </summary>
            /// <param name="boot">根节点</param>
            /// <param name="waitCount">建图线程任务完成计数</param>
            public ThreadBuilder(Node boot, AutoCSer.Threading.AutoWaitCount waitCount) : base(boot)
            {
                this.waitCount = waitCount;
                threadWait.Set(0);
                AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(build);
            }
            /// <summary>
            /// 设置当前处理节点集合
            /// </summary>
            /// <param name="reader">当前处理节点集合</param>
            /// <param name="startIndex">处理节点起始索引位置</param>
            /// <param name="count">处理节点节点索引位置</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void Set(Node[] reader, int startIndex, int count)
            {
                Reader = reader;
                StartIndex = startIndex;
                Count = count;
            }
            /// <summary>
            /// 设置当前处理节点集合
            /// </summary>
            /// <param name="reader">当前处理节点集合</param>
            /// <param name="startIndex">处理节点起始索引位置</param>
            /// <param name="count">处理节点节点索引位置</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void SetThread(Node[] reader, int startIndex, int count)
            {
                if (count != 0)
                {
                    Set(reader, startIndex, count);
                    threadWait.Set();
                }
            }
            /// <summary>
            /// 释放建图线程
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void FreeThread()
            {
                Count = 0;
                threadWait.Set();
            }
            /// <summary>
            /// 建图线程
            /// </summary>
            private void build()
            {
                try
                {
                    do
                    {
                        threadWait.Wait();
                        if (Count == 0) return;
                        Build();
                        waitCount.Free();
                    }
                    while (true);
                }
                catch (Exception error)
                {
                    ThreadException = error;
                }
            }
        }
    }
}
