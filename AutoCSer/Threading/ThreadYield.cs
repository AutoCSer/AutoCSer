using System;
using AutoCSer.Extension;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 线程操作
    /// </summary>
    public unsafe partial class ThreadYield
    {
        /// <summary>
        /// 冲突统计类型
        /// </summary>
        internal enum Type
        {
            /// <summary>
            /// 未知的用户调用
            /// </summary>
            Unknown,
            /// <summary>
            /// 设置备用空闲缓冲区
            /// </summary>
            SubBufferPoolSetBackFree,
            /// <summary>
            /// 获取随机数
            /// </summary>
            RandomNextBit,
            /// <summary>
            /// 获取随机数
            /// </summary>
            RandomNextByte,
            /// <summary>
            /// 获取随机数
            /// </summary>
            RandomNextUShort,
            /// <summary>
            /// 获取随机数
            /// </summary>
            RandomNext64,
            /// <summary>
            /// 字符串名称申请
            /// </summary>
            NamePoolGet,

            /// <summary>
            /// 成员位图内存池空闲地址入池
            /// </summary>
            MemberMapPoolFreePush,
            /// <summary>
            /// 非托管内存池空闲地址入池
            /// </summary>
            UnmanagedPoolFreePush,
            /// <summary>
            /// 添加缓冲区
            /// </summary>
            SubBufferPoolPush,
            /// <summary>
            /// 套接字异步事件对象池添加节点
            /// </summary>
            SocketAsyncEventArgsPush,
            /// <summary>
            /// Sql 连接池添加连接
            /// </summary>
            SqlConnectionPoolPush,
            /// <summary>
            /// 添加日志信息
            /// </summary>
            FileLogPushDebug,
            /// <summary>
            /// Sql 操作链表任务队列添加节点
            /// </summary>
            SqlLinkQueueTaskPush,
            /// <summary>
            /// 定时链表队列添加节点
            /// </summary>
            TimerLinkQueuePush,
            /// <summary>
            /// 链表添加节点
            /// </summary>
            YieldLinkPush,
            /// <summary>
            /// 双向链表添加节点
            /// </summary>
            YieldLinkDoublePush,
            /// <summary>
            /// 队列链表添加节点
            /// </summary>
            YieldQueuePush,
            /// <summary>
            /// TCP 调用客户端回调保持设置命令会话标识
            /// </summary>
            TcpServerKeepCallbackSetCommandIndex,

            /// <summary>
            /// 成员位图内存池
            /// </summary>
            MemberMapPoolMemory,
            /// <summary>
            /// 成员位图内存池获取空闲地址
            /// </summary>
            MemberMapPoolFreePop,
            /// <summary>
            /// 非托管内存池获取空闲地址
            /// </summary>
            UnmanagedPoolFreePop,
            /// <summary>
            /// 获取缓冲区
            /// </summary>
            SubBufferPoolPop,
            /// <summary>
            /// 套接字异步事件对象池弹出节点
            /// </summary>
            SocketAsyncEventArgsPop,
            /// <summary>
            /// Sql 连接池弹出连接
            /// </summary>
            SqlConnectionPoolPop,
            /// <summary>
            /// 交换日志队列信息
            /// </summary>
            FileLogExchangeDebug,
            /// <summary>
            /// Sql 操作链表任务队列弹出节点
            /// </summary>
            SqlLinkQueueTaskPop,
            /// <summary>
            /// 定时链表队列弹出节点
            /// </summary>
            TimerLinkQueuePop,
            /// <summary>
            /// 链表弹出节点
            /// </summary>
            YieldLinkPop,
            /// <summary>
            /// 双向链表弹出节点
            /// </summary>
            YieldLinkDoublePop,
            /// <summary>
            /// 队列链表弹出节点
            /// </summary>
            YieldQueuePop,
            /// <summary>
            /// 释放 TCP 调用客户端回调保持
            /// </summary>
            TcpServerKeepCallbackDispose,

            /// <summary>
            /// 最后关键字缓存字典获取数据
            /// </summary>
            LockLastDictionaryGet,
            /// <summary>
            /// 最后关键字缓存字典设置数据
            /// </summary>
            LockLastDictionarySet,
            /// <summary>
            /// Sql 日志流成员加载检测
            /// </summary>
            SqlLogStreamLoadMember,
            /// <summary>
            /// Sql 时间设置
            /// </summary>
            SqlNowTimeSet,
            /// <summary>
            /// 时间验证服务设置验证时间
            /// </summary>
            TimeVerifyServerSetTicks,
            /// <summary>
            /// 套接字超时取消等待
            /// </summary>
            SocketTimeoutLinkCancelTimeout,
            /// <summary>
            /// 设置 TCP 客户端套接字
            /// </summary>
            TcpCommandClientSetSocket,
            /// <summary>
            /// TCP 服务端保持回调处理
            /// </summary>
            TcpServerKeepCallback,
            /// <summary>
            /// 客户端强制终止 TCP 服务端保持回调
            /// </summary>
            TcpServerKeepCallbackCancel,
            /// <summary>
            /// HTML 标题获取客户端操作
            /// </summary>
            HtmlTitleHttpClient,
            /// <summary>
            /// 缓存服务获取数据缓冲区
            /// </summary>
            CacheServerGetBuffer,
            /// <summary>
            /// 消息队列获取数据缓冲区
            /// </summary>
            MessageQueueGetBuffer,
            /// <summary>
            /// 冲突统计类型数量
            /// </summary>
            Count
        }
        /// <summary>
        /// 冲突统计集合（非线程安全）
        /// </summary>
        private static Pointer yieldCounts = new Pointer { Data = Unmanaged.GetStatic64((int)Type.Count << 3, true) };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">冲突统计类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Yield(Type type)
        {
            ++yieldCounts.ULong[(int)type];
            YieldOnly();
        }
        /// <summary>
        /// 
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void YieldOnly()
        {
#if DOTNET2 || UNITY3D
            System.Threading.Thread.Sleep(0);
#else
            System.Threading.Thread.Yield();
#endif
        }
    }
}
