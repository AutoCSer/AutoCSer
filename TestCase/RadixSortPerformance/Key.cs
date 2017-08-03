using System;

namespace AutoCSer.TestCase.RadixSortPerformance
{
    /// <summary>
    /// 关键字定义
    /// </summary>
    static class Key
    {
        public struct Int32 : IComparable<Int32>
        {
            /// <summary>
            /// 关键字
            /// </summary>
            public int Key;
            /// <summary>
            /// 关键字比较
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(Int32 other)
            {
                if (Key > other.Key) return 1;
                return Key < other.Key ? -1 : 0;
            }
        }
        public struct UInt32 : IComparable<UInt32>
        {
            /// <summary>
            /// 关键字
            /// </summary>
            public uint Key;
            /// <summary>
            /// 关键字比较
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(UInt32 other)
            {
                if (Key > other.Key) return 1;
                return Key < other.Key ? -1 : 0;
            }
        }
        public struct Int64 : IComparable<Int64>
        {
            /// <summary>
            /// 关键字
            /// </summary>
            public long Key;
            /// <summary>
            /// 关键字比较
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(Int64 other)
            {
                if (Key > other.Key) return 1;
                return Key < other.Key ? -1 : 0;
            }
        }
        public struct UInt64 : IComparable<UInt64>
        {
            /// <summary>
            /// 关键字
            /// </summary>
            public ulong Key;
            /// <summary>
            /// 关键字比较
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(UInt64 other)
            {
                if (Key > other.Key) return 1;
                return Key < other.Key ? -1 : 0;
            }
        }
    }
}
