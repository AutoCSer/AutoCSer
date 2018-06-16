using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.Lock
{
    /// <summary>
    /// 锁节点
    /// </summary>
    internal sealed class Node : Value.Node
    {
        /// <summary>
        /// 缓存管理
        /// </summary>
        internal readonly CacheManager Cache;
        /// <summary>
        /// 随机序号
        /// </summary>
        internal ulong RandomNo;
        /// <summary>
        /// 锁链表首节点
        /// </summary>
        private LinkNode head;
        /// <summary>
        /// 锁链表尾节点
        /// </summary>
        private LinkNode end;
        /// <summary>
        /// 锁 数据节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        private Node(Cache.Node parent, ref OperationParameter.NodeParser parser) : base(parent)
        {
            Cache = parser.Cache;
            RandomNo = AutoCSer.Random.Default.NextULong();
        }
        /// <summary>
        /// 删除节点操作
        /// </summary>
        internal override void OnRemoved()
        {
            base.OnRemoved();
            QueueTaskThread.Thread.Default.Add(new QueueTaskThread.Dispose(this));
        }
        /// <summary>
        /// 释放锁节点
        /// </summary>
        internal void Dispose()
        {
            while (head != null) head = head.DisposeLock();
            end = null;
        }

        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Cache.Node GetOperationNext(ref OperationParameter.NodeParser parser)
        {
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
            return null;
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal override void OperationEnd(ref OperationParameter.NodeParser parser)
        {
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
        }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Cache.Node GetQueryNext(ref OperationParameter.NodeParser parser)
        {
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
            return null;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal override void QueryEnd(ref OperationParameter.NodeParser parser)
        {
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.InsertBefore:
                    if (parser.ValueData.Type == ValueData.DataType.Long)
                    {
                        if (parser.OnReturn != null)
                        {
                            QueueTaskThread.TryEnter enter = new QueueTaskThread.TryEnter(this);
                            enter.LinkNode = new LinkNode(this, ref parser);
                            QueueTaskThread.Thread.Default.Add(enter);
                        }
                    }
                    else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
                    return;
                case OperationParameter.OperationType.SetValue:
                    if (parser.ValueData.Type == ValueData.DataType.Long)
                    {
                        if (parser.OnReturn != null)
                        {
                            QueueTaskThread.Enter enter = new QueueTaskThread.Enter(this);
                            enter.LinkNode = new LinkNode(this, ref parser);
                            QueueTaskThread.Thread.Default.Add(enter);
                        }
                    }
                    else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
                    return;
                case OperationParameter.OperationType.Remove:
                    if (parser.ValueData.Type == ValueData.DataType.ULong)
                    {
                        if (parser.OnReturn != null) QueueTaskThread.Thread.Default.Add(new QueueTaskThread.Exit(this, ref parser));
                    }
                    else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
                    return;
            }
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="exit"></param>
        internal void Exit(QueueTaskThread.Exit exit)
        {
            ReturnParameter returnParameter = new ReturnParameter(ReturnType.NotFoundLock);
            try
            {
                if (head != null && RandomNo == exit.RandomNo)
                {
                    returnParameter.Parameter.ReturnParameterSet(true);
                    do
                    {
                        if ((head = head.Next) == null)
                        {
                            end = null;
                            return;
                        }
                    }
                    while (!head.Enter());
                }
            }
            finally { exit.OnReturn(returnParameter); }
        }
        /// <summary>
        /// 超时处理
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnTimeout(LinkNode node)
        {
            if (Object.ReferenceEquals(node, head))
            {
                while ((node = node.Next) != null && !node.Enter()) ;
            }
        }
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Enter(LinkNode node)
        {
            if (head == null)
            {
                if (node.Enter()) head = end = node;
            }
            else end.Next = node;
        }
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void TryEnter(LinkNode node)
        {
            if (head == null)
            {
                if (node.Enter()) head = end = node;
            }
            else node.OnReturn(new ReturnParameter(ReturnType.Locked));
        }

        /// <summary>
        /// 创建缓存快照
        /// </summary>
        /// <returns></returns>
        internal unsafe override Snapshot.Node CreateSnapshot()
        {
            return Snapshot.NoSerialize.Default;
        }
#if NOJIT
        /// <summary>
        /// 创建锁节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Node create(Cache.Node parent, ref OperationParameter.NodeParser parser)
        {
            return new Node(parent, ref parser);
        }
#endif
        /// <summary>
        /// 节点信息
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly NodeInfo<Node> nodeInfo;
        static Node()
        {
            nodeInfo = new NodeInfo<Node>
            {
#if NOJIT
                Constructor = (Constructor<Node>)Delegate.CreateDelegate(typeof(Constructor<Node>), typeof(Node).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null))
#else
                Constructor = (Constructor<Node>)AutoCSer.Emit.Constructor.CreateCache(typeof(Node), NodeConstructorParameterTypes)
#endif
            };
        }
    }
}
