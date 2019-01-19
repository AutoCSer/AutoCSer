using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 数组位图
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct SerializeArrayMap
    {
        /// <summary>
        /// 当前位
        /// </summary>
        public uint Bit;
        /// <summary>
        /// 当前位图
        /// </summary>
        public uint Map;
        /// <summary>
        /// 当前写入位置
        /// </summary>
        public byte* Write;
        /// <summary>
        /// 数组位图
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="arrayLength">数组长度</param>
        public SerializeArrayMap(UnmanagedStream stream, int arrayLength)
        {
            int length = ((arrayLength + (31 + 32)) >> 5) << 2;
            Bit = 1U << 31;
            stream.PrepLength(length);
            Write = stream.CurrentData;
            Map = 0;
            *(int*)Write = arrayLength;
            stream.ByteSize += length;
        }
        /// <summary>
        /// 数组位图
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="arrayLength">数组长度</param>
        /// <param name="prepLength">附加长度</param>
        public SerializeArrayMap(UnmanagedStream stream, int arrayLength, int prepLength)
        {
            int length = ((arrayLength + (31 + 32)) >> 5) << 2;
            Bit = 1U << 31;
            stream.PrepLength(length + prepLength);
            Write = stream.CurrentData;
            Map = 0;
            *(int*)Write = arrayLength;
            stream.ByteSize += length;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">是否写位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Next(bool value)
        {
            if (value) Map |= Bit;
            if (Bit == 1)
            {
                *(uint*)(Write += sizeof(int)) = Map;
                Bit = 1U << 31;
                Map = 0;
            }
            else Bit >>= 1;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">是否写位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Next(bool? value)
        {
            if (value.HasValue)
            {
                Map |= Bit;
                if ((bool)value) Map |= (Bit >> 1);
            }
            if (Bit == 2)
            {
                *(uint*)(Write += sizeof(int)) = Map;
                Bit = 1U << 31;
                Map = 0;
            }
            else Bit >>= 2;
        }
        /// <summary>
        /// 位图写入结束
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void End(UnmanagedStream stream)
        {
            if (Bit != 1U << 31) *(uint*)(Write + sizeof(int)) = Map;
            //stream.PrepLength();
        }
    }
}
