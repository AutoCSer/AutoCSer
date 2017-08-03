using System;

namespace AutoCSer.TestCase.TcpInternalClientPerformance
{
    /// <summary>
    /// TCP 客户端操作
    /// </summary>
    internal unsafe static partial class Client
    {
        /// <summary>
        /// 测试回调
        /// </summary>
        /// <param name="value"></param>
        internal static void OnAdd(AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpInternalServerPerformance.ServerCustomSerialize> value)
        {
            SubArray<byte> buffer = value.Value.Buffer;
            fixed (byte* bufferFixed = buffer.BufferArray)
            {
                for (byte* start = bufferFixed + buffer.StartIndex, end = start + buffer.Count; start != end; start += sizeof(int) * 3)
                {
                    if (((Left ^ *(int*)start) | ((Left + *(int*)(start + sizeof(int))) ^ *(int*)(start + sizeof(int) * 2))) != 0 || !addMap.SetWhenNullUnsafe(*(int*)(start + sizeof(int))))
                    {
                        ++ErrorCount;
                    }
                }
            }
            if ((waitCount -= buffer.Count / (sizeof(int) * 3)) <= 0)
            {
                Time.Stop();
                WaitHandle.Set();
            }
        }
        /// <summary>
        /// 测试回调
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static bool OnAddEmit(AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpInternalServerPerformance.ServerCustomSerialize> value)
        {
            SubArray<byte> buffer = value.Value.Buffer;
            fixed (byte* bufferFixed = buffer.BufferArray)
            {
                for (byte* start = bufferFixed + buffer.StartIndex, end = start + buffer.Count; start != end; start += sizeof(int) * 3)
                {
                    if (((Left ^ *(int*)start) | ((Left + *(int*)(start + sizeof(int))) ^ *(int*)(start + sizeof(int) * 2))) != 0 || !addMap.SetWhenNullUnsafe(*(int*)(start + sizeof(int))))
                    {
                        ++ErrorCount;
                    }
                }
            }
            if ((waitCount -= buffer.Count / (sizeof(int) * 3)) <= 0)
            {
                Time.Stop();
                WaitHandle.Set();
            }
            return true;
        }
    }
}
