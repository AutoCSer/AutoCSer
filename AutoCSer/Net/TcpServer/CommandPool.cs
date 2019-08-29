using System;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using AutoCSer.Log;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 客户端命令池
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    internal unsafe class CommandPool
    {
        /// <summary>
        /// 命令数组最小二进制长度
        /// </summary>
        private const int minArrayBitSize = 2;
        /// <summary>
        /// 命令数组最大二进制长度
        /// </summary>
        private const int maxArrayBitSize = 16;
        /// <summary>
        /// 客户端命令
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        internal struct CommandLink
        {
            /// <summary>
            /// 客户端命令
            /// </summary>
            internal ClientCommand.Command Command;
            /// <summary>
            /// 下一个命令序号
            /// </summary>
            internal int Next;
            /// <summary>
            /// 设置客户端命令
            /// </summary>
            /// <param name="command">客户端命令</param>
            /// <returns>下一个命令序号</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal int Set(ClientCommand.Command command)
            {
                Command = command;
                return Next;
            }
            /// <summary>
            /// 获取客户端命令
            /// </summary>
            /// <param name="nextIndex"></param>
            /// <param name="command"></param>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal int Get(int nextIndex, out ClientCommand.Command command)
            {
                command = Command;
                if (Command != null)
                {
                    if (Command.CommandInfo.IsKeepCallback == 0)
                    {
                        Command = null;
                        Next = nextIndex;
                        return 0;
                    }
                    return 1;
                }
                return 2;
            }
            /// <summary>
            /// 释放客户端命令
            /// </summary>
            /// <param name="nextIndex">下一个命令序号</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Cancel(int nextIndex)
            {
                Command = null;
                Next = nextIndex;
            }
            /// <summary>
            /// 释放客户端命令
            /// </summary>
            /// <param name="command"></param>
            /// <param name="nextIndex"></param>
            /// <returns></returns>
            internal bool Cancel(ClientCommand.Command command, int nextIndex)
            {
                if (Command == command)
                {
                    Command = null;
                    Next = nextIndex;
                    return true;
                }
                return false;
            }
        }
        private readonly ulong pad0, pad1, pad2, pad3, pad4, pad5, pad6;
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILog log;
        /// <summary>
        /// 客户端命令池
        /// </summary>
        private CommandLink[][] arrays;
        /// <summary>
        /// 获取客户端命令
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal ClientCommand.Command this[int index]
        {
            get
            {
                return arrays[index >> bitSize][index & arraySizeAnd].Command;
            }
        }
        /// <summary>
        /// 第一个客户端命令池数组
        /// </summary>
        internal CommandLink[] Array;
        /// <summary>
        /// 数组长度
        /// </summary>
        private int arraySizeAnd;
        /// <summary>
        /// 数组二进制长度
        /// </summary>
        private int bitSize;
        /// <summary>
        /// 当前数组数量
        /// </summary>
        private int arrayCount;
        /// <summary>
        /// 命令地址数量
        /// </summary>
        private int commandCount;
        private readonly ulong pad10, pad11, pad12, pad13, pad14, pad15, pad16;
        /// <summary>
        /// 客户端命令池数组
        /// </summary>
        private CommandLink[] pushArray;
        /// <summary>
        /// 空闲命令位置
        /// </summary>
        private int freeIndex;
        /// <summary>
        /// 客户端命令池数组索引
        /// </summary>
        private int pushArrayIndex = int.MinValue;
        private readonly ulong pad20, pad21, pad22, pad23, pad24, pad25, pad26;
        /// <summary>
        /// 保持回调命令
        /// </summary>
        private ClientCommand.Command keepCallbackCommand;
        /// <summary>
        /// 客户端命令池数组
        /// </summary>
        private CommandLink[] getArray;
        /// <summary>
        /// 保持回调命令会话标识
        /// </summary>
        private int keepCallbackCommandIndex = int.MinValue;
        /// <summary>
        /// 客户端命令池数组索引
        /// </summary>
        private int getArrayIndex = int.MinValue;
        /// <summary>
        /// 空闲命令结束位置
        /// </summary>
        private int freeEndIndex;
        /// <summary>
        /// 空闲命令结束位置访问锁
        /// </summary>
        private int freeEndIndexLock;
        /// <summary>
        /// 是否输出过错误日志 活动会话数量过多
        /// </summary>
        private int isErrorLog;
        private readonly int pad37;
        private readonly ulong pad30, pad31, pad32, pad33, pad34, pad35, pad36;
        /// <summary>
        /// 客户端命令池
        /// </summary>
        /// <param name="bitSize"></param>
        /// <param name="freeIndex"></param>
        /// <param name="log"></param>
        internal CommandPool(int bitSize, int freeIndex, ILog log)
        {
            this.log = log;
            this.bitSize = bitSize <= maxArrayBitSize ? (bitSize >= minArrayBitSize ? bitSize : minArrayBitSize) : maxArrayBitSize;
            commandCount = 1 << this.bitSize;
            if ((uint)freeIndex >= commandCount) throw new IndexOutOfRangeException();
            Array = new CommandLink[commandCount];
            arrays = new CommandLink[4][];
            this.freeIndex = freeIndex;
            arrays[0] = Array;
            arrayCount = 1;
            for (int index = freeIndex; index != commandCount; ++index) Array[index].Next = index + 1;
            freeEndIndex = arraySizeAnd = commandCount - 1;
        }
        /// <summary>
        /// 添加客户端命令
        /// </summary>
        /// <param name="command">客户端命令</param>
        /// <returns>客户端命令索引位置</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int Push(ClientCommand.Command command)
        {
            int index = freeIndex, arrayIndex = freeIndex >> bitSize;
            if (arrayIndex == 0) freeIndex = Array[index].Set(command);
            else
            {
                if (arrayIndex != pushArrayIndex) pushArray = arrays[pushArrayIndex = arrayIndex];
                freeIndex = pushArray[index & arraySizeAnd].Set(command);
            }
            return freeIndex == commandCount ? create(index) : index;
        }
        /// <summary>
        /// 新建客户端命令池
        /// </summary>
        /// <param name="currentIndex">当前空闲命令位置</param>
        /// <returns></returns>
        private int create(int currentIndex)
        {
            if (bitSize == maxArrayBitSize)
            {
                if (arrayCount == arrays.Length)
                {
                    if (arrayCount == 1 << (Server.CommandIndexBits - maxArrayBitSize))
                    {
                        freeIndex = currentIndex;
                        if (isErrorLog == 0)
                        {
                            isErrorLog = 1;
                            log.Add(LogType.Error, "TCP 客户端活动会话数量过多");
                        }
                        return 0;
                    }
                    arrays = arrays.copyNew(arrayCount << 1);
                }
                int index = 1 << maxArrayBitSize;
                CommandLink[] array = new CommandLink[index];
                do
                {
                    array[index - 1].Next = commandCount + index;
                }
                while (--index != 0);
                arrays[arrayCount++] = array;
                while (System.Threading.Interlocked.CompareExchange(ref freeEndIndexLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
                arrays[freeEndIndex >> bitSize][freeEndIndex & arraySizeAnd].Next = commandCount;
                freeEndIndex = (commandCount += 1 << maxArrayBitSize) - 1;
                System.Threading.Interlocked.Exchange(ref freeEndIndexLock, 0);
            }
            else
            {
                CommandLink[] array = new CommandLink[1 << ++bitSize];
                for (int index = commandCount, endIndex = commandCount << 1; index != endIndex; ++index) array[index].Next = index + 1;
                while (System.Threading.Interlocked.CompareExchange(ref freeEndIndexLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
                Array.CopyTo(array, 0);
                arrays[0] = Array = array;
                array[freeEndIndex].Next = commandCount;
                freeEndIndex = arraySizeAnd = (commandCount <<= 1) - 1;
                System.Threading.Interlocked.Exchange(ref freeEndIndexLock, 0);
            }
            freeIndex = (currentIndex < (1 << maxArrayBitSize) ? Array : pushArray)[currentIndex & arraySizeAnd].Next;
            return currentIndex;
        }
        /// <summary>
        /// 获取客户端命令
        /// </summary>
        /// <param name="index">客户端命令索引位置</param>
        /// <returns>客户端命令</returns>
        internal ClientCommand.Command GetCommand(int index)
        {
            if (keepCallbackCommandIndex == index) return keepCallbackCommand;
            ClientCommand.Command command;
            int arrayIndex = index >> bitSize;
            if (arrayIndex == 0)
            {
                while (System.Threading.Interlocked.CompareExchange(ref freeEndIndexLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
                switch (Array[index].Get(commandCount, out command))
                {
                    case 0:
                        arrays[freeEndIndex >> bitSize][freeEndIndex & arraySizeAnd].Next = index;
                        freeEndIndex = index;
                        System.Threading.Interlocked.Exchange(ref freeEndIndexLock, 0);
                        return command;
                    case 1:
                        System.Threading.Interlocked.Exchange(ref freeEndIndexLock, 0);
                        keepCallbackCommand = command;
                        keepCallbackCommandIndex = index;
                        return command;
                    default: System.Threading.Interlocked.Exchange(ref freeEndIndexLock, 0); return null;
                }
            }
            if (arrayIndex != getArrayIndex) getArray = arrays[getArrayIndex = arrayIndex];
            int commandIndex = index & arraySizeAnd;
            while (System.Threading.Interlocked.CompareExchange(ref freeEndIndexLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
            switch (getArray[commandIndex].Get(commandCount, out command))
            {
                case 0:
                    arrays[freeEndIndex >> bitSize][freeEndIndex & arraySizeAnd].Next = index;
                    freeEndIndex = index;
                    System.Threading.Interlocked.Exchange(ref freeEndIndexLock, 0);
                    return command;
                case 1:
                    System.Threading.Interlocked.Exchange(ref freeEndIndexLock, 0);
                    keepCallbackCommand = command;
                    keepCallbackCommandIndex = index;
                    return command;
                default: System.Threading.Interlocked.Exchange(ref freeEndIndexLock, 0); return null;
            }
        }
        /// <summary>
        /// 取消客户端命令
        /// </summary>
        /// <param name="index">客户端命令索引位置</param>
        internal void Cancel(int index)
        {
            int arrayIndex = index >> bitSize, commandIndex = index & arraySizeAnd;
            while (System.Threading.Interlocked.CompareExchange(ref freeEndIndexLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
            arrays[arrayIndex][commandIndex].Cancel(commandCount);
            arrays[freeEndIndex >> bitSize][freeEndIndex & arraySizeAnd].Next = index;
            freeEndIndex = index;
            System.Threading.Interlocked.Exchange(ref freeEndIndexLock, 0);
        }
        /// <summary>
        /// 取消客户端命令
        /// </summary>
        /// <param name="index"></param>
        /// <param name="command"></param>
        internal void Cancel(int index, ClientCommand.Command command)
        {
            int arrayIndex = index >> bitSize, commandIndex = index & arraySizeAnd;
            while (System.Threading.Interlocked.CompareExchange(ref freeEndIndexLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
            if (arrays[arrayIndex][commandIndex].Cancel(command, commandCount))
            {
                arrays[freeEndIndex >> bitSize][freeEndIndex & arraySizeAnd].Next = index;
                freeEndIndex = index;
            }
            System.Threading.Interlocked.Exchange(ref freeEndIndexLock, 0);
        }
        /// <summary>
        /// 释放所有命令
        /// </summary>
        /// <param name="head"></param>
        /// <param name="end"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        internal ClientCommand.CommandBase Free(ClientCommand.CommandBase head, ClientCommand.CommandBase end, int startIndex)
        {
            bool isNext = false;
            while (System.Threading.Interlocked.CompareExchange(ref freeEndIndexLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
            try
            {
                foreach (CommandLink[] array in arrays)
                {
                    if (isNext)
                    {
                        if (array == null) break;
                        for (startIndex = array.Length; startIndex != 0; )
                        {
                            ClientCommand.Command command = array[--startIndex].Command;
                            if (command != null)
                            {
                                array[startIndex].Command = null;
                                if (head == null) head = end = command;
                                else
                                {
                                    end.LinkNext = command;
                                    end = command;
                                }
                            }
                        }
                    }
                    else
                    {
                        isNext = true;
                        do
                        {
                            ClientCommand.Command command = array[startIndex].Command;
                            if (command != null)
                            {
                                array[startIndex].Command = null;
                                if (head == null) head = end = command;
                                else
                                {
                                    end.LinkNext = command;
                                    end = command;
                                }
                            }
                        }
                        while (++startIndex != array.Length);
                    }
                }
            }
            finally { System.Threading.Interlocked.Exchange(ref freeEndIndexLock, 0); }
            return head;
        }
    }
    ///// <summary>
    ///// 命令索引信息
    ///// </summary>
    //[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    //internal struct CommandPoolIndex
    //{
    //    /// <summary>
    //    /// 客户端命令
    //    /// </summary>
    //    public ClientCommand.Command Command;
    //    /// <summary>
    //    /// 取消接收数据
    //    /// </summary>
    //    /// <returns>接收数据回调+回调是否使用任务池</returns>
    //    [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
    //    public ClientCommand.Command Cancel()
    //    {
    //        ClientCommand.Command command = Command;
    //        Command = null;
    //        return command;
    //    }
    //    /// <summary>
    //    /// 取消接收数据
    //    /// </summary>
    //    /// <param name="command"></param>
    //    [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
    //    public void Cancel(ClientCommand.Command command)
    //    {
    //        if (Command == command) Command = null;
    //    }
    //    /// <summary>
    //    /// 取消接收数据
    //    /// </summary>
    //    /// <param name="command"></param>
    //    /// <returns>客户端命令相同返回 0</returns>
    //    public byte IsCancel(ClientCommand.Command command)
    //    {
    //        if (Command == command)
    //        {
    //            Command = null;
    //            return 0;
    //        }
    //        return 1;
    //    }
    //    /// <summary>
    //    /// 获取接收数据回调
    //    /// </summary>
    //    /// <param name="isKeepCallback">是否保持异步回调</param>
    //    /// <returns>客户端命令</returns>
    //    [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
    //    public ClientCommand.Command Get(ref int isKeepCallback)
    //    {
    //        ClientCommand.Command command = Command;
    //        if ((isKeepCallback = Command.CommandInfo.IsKeepCallback) == 0) Command = null;
    //        return command;
    //    }
    //}
}
