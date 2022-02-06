using System;
using System.Runtime.CompilerServices;
using AutoCSer.Extensions;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(因为指针无法静态初始化)
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public unsafe struct Pointer : IEquatable<Pointer>
    {
        /// <summary>
        /// 指针
        /// </summary>
        internal void* Data;
        /// <summary>
        /// 总字节长度
        /// </summary>
        internal int ByteSize;
        /// <summary>
        /// 总字节长度
        /// </summary>
        public int Size { get { return ByteSize; } }
        /// <summary>
        /// 当前数据操作位置
        /// </summary>
        internal int CurrentIndex;
        /// <summary>
        /// 当前数据操作位置
        /// </summary>
        public int Index { get { return CurrentIndex; } }
        /// <summary>
        /// 当前数据操作位置
        /// </summary>
        public byte* Current
        {
            get { return (byte*)Data + CurrentIndex; }
        }
        /// <summary>
        /// 数据结束位置
        /// </summary>
        public byte* End
        {
            get { return (byte*)Data + ByteSize; }
        }
        /// <summary>
        /// 空闲字节数量
        /// </summary>
        internal int FreeSize
        {
            get { return ByteSize - CurrentIndex; }
        }
        /// <summary>
        /// 不为 0 表示存在空闲字节
        /// </summary>
        internal int IsFreeSize { get { return ByteSize ^ CurrentIndex; } }
        /// <summary>
        /// 指针
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        internal Pointer(void* data, int size)
        {
#if DEBUG
            if (size < 0) throw new Exception(size.toString() + " < 0");
#endif
            Data = data;
            ByteSize = size;
            CurrentIndex = 0;
        }
        /// <summary>
        /// 字节指针
        /// </summary>
        public byte* Byte
        {
            get { return (byte*)Data; }
        }
        /// <summary>
        /// 字节指针
        /// </summary>
        public sbyte* SByte
        {
            get { return (sbyte*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public short* Short
        {
            get { return (short*)Data; }
        }
        /// <summary>
        /// 双字节指针
        /// </summary>
        public ushort* UShort
        {
            get { return (ushort*)Data; }
        }
        /// <summary>
        /// 字符指针
        /// </summary>
        public char* Char
        {
            get { return (char*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public int* Int
        {
            get { return (int*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public uint* UInt
        {
            get { return (uint*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public long* Long
        {
            get { return (long*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public ulong* ULong
        {
            get { return (ulong*)Data; }
        }
        /// <summary>
        /// HASH值
        /// </summary>
        /// <returns>HASH值</returns>
        public override int GetHashCode()
        {
            return (int)((uint)((ulong)Data >> 3) ^ (uint)((ulong)Data >> 35));
        }
        /// <summary>
        /// 指针比较
        /// </summary>
        /// <param name="obj">待比较指针</param>
        /// <returns>指针是否相等</returns>
        public override bool Equals(object obj)
        {
            return Equals((Pointer)obj);
        }
        /// <summary>
        /// 指针比较
        /// </summary>
        /// <param name="other">待比较指针</param>
        /// <returns>指针是否相等</returns>
        public bool Equals(Pointer other)
        {
            return Data == other.Data;
        }
        /// <summary>
        /// 清空数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetNull()
        {
            Data = null;
            ByteSize = CurrentIndex = 0;
        }
        /// <summary>
        /// 数据全部设置为 0
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Clear()
        {
            if (ByteSize != 0) AutoCSer.Memory.Common.Clear(Byte, ByteSize);
        }
        /// <summary>
        /// 获取指针并清除
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void* GetDataClearOnly()
        {
            void* data = Data;
            Data = null;
            return data;
        }
        /// <summary>
        /// 转换为 Span
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Span<byte> GetSpan(int index)
        {
#if DEBUG
            if (index < 0) throw new Exception(index.toString() + " < 0");
            if (index > ByteSize) throw new Exception(index.toString() + " > " + ByteSize.toString());
#endif
            return new Span<byte>(Data, CurrentIndex = index);
        }
        /// <summary>
        /// 复制数据到另外一个指针
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CopyTo(ref Pointer data)
        {
#if DEBUG
            DebugCheck();
#endif
            new Span<byte>(Data, CurrentIndex).CopyTo(data.GetSpan(CurrentIndex));
        }
        /// <summary>
        /// 移动当前数据操作位置并返回移动之前的位置
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal byte* GetBeforeMove(int size)
        {
#if DEBUG
            if (size <= 0) throw new Exception(size.toString() + " <= 0");
            if (CurrentIndex + size > ByteSize) throw new Exception(CurrentIndex.toString() + " + " + size.toString() + " > " + ByteSize.toString());
#endif
            byte* data = (byte*)Data + CurrentIndex;
            CurrentIndex += size;
            return data;
        }
        /// <summary>
        /// 设置当前数据操作位置
        /// </summary>
        /// <param name="current"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetCurrent(void* current)
        {
#if DEBUG
            if (current < Data) throw new Exception("current < Data");
            if ((byte*)current - (byte*)Data > ByteSize) throw new Exception(((byte*)current - (byte*)Data).toString() + " > ByteSize");
#endif
            CurrentIndex = (int)((byte*)current - (byte*)Data);
        }
        /// <summary>
        /// 移动当前位置
        /// </summary>
        /// <param name="size"></param>
        public void MoveSize(int size)
        {
            int byteSize = CurrentIndex + size;
            if (byteSize > ByteSize) throw new IndexOutOfRangeException("CurrentIndex[" + CurrentIndex.toString() + "] + size[" + size.toString() + "] > ByteSize[" + ByteSize.toString() + "]");
            if (byteSize < 0) throw new IndexOutOfRangeException("CurrentIndex[" + CurrentIndex.toString() + "] + size[" + size.toString() + "] < 0");
            CurrentIndex = byteSize;
        }
        /// <summary>
        /// 移动当前位置
        /// </summary>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeMoveSize(int size)
        {
#if DEBUG
            if (CurrentIndex + size < 0) throw new Exception("CurrentIndex + size < 0");
            if (CurrentIndex + size > ByteSize) throw new Exception("CurrentIndex + size > ByteSize");
#endif
            CurrentIndex += size;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(bool value)
        {
#if DEBUG
            if (CurrentIndex >= ByteSize) throw new Exception("CurrentIndex >= ByteSize");
#endif
            *((bool*)Data + CurrentIndex++) = value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(byte value)
        {
#if DEBUG
            if (CurrentIndex >= ByteSize) throw new Exception("CurrentIndex >= ByteSize");
#endif
            *((byte*)Data + CurrentIndex++) = value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(sbyte value)
        {
#if DEBUG
            if (CurrentIndex >= ByteSize) throw new Exception("CurrentIndex >= ByteSize");
#endif
            *(sbyte*)((byte*)Data + CurrentIndex++) = value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(short value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(short) > ByteSize) throw new Exception("CurrentIndex + sizeof(short) > ByteSize");
#endif
            *(short*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(short);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(ushort value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(ushort) > ByteSize) throw new Exception("CurrentIndex + sizeof(ushort) > ByteSize");
#endif
            *(ushort*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(ushort);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(int value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(int) > ByteSize) throw new Exception("CurrentIndex + sizeof(int) > ByteSize");
#endif
            *(int*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(int);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(uint value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(uint) > ByteSize) throw new Exception("CurrentIndex + sizeof(uint) > ByteSize");
#endif
            *(uint*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(uint);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(long value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(long) > ByteSize) throw new Exception("CurrentIndex + sizeof(long) > ByteSize");
#endif
            *(long*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(long);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(ulong value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(ulong) > ByteSize) throw new Exception("CurrentIndex + sizeof(ulong) > ByteSize");
#endif
            *(ulong*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(ulong);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(char value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(char) > ByteSize) throw new Exception("CurrentIndex + sizeof(char) > ByteSize");
#endif
            *(char*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(char);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(float value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(float) > ByteSize) throw new Exception("CurrentIndex + sizeof(float) > ByteSize");
#endif
            *(float*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(float);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(double value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(double) > ByteSize) throw new Exception("CurrentIndex + sizeof(double) > ByteSize");
#endif
            *(double*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(double);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(decimal value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(decimal) > ByteSize) throw new Exception("CurrentIndex + sizeof(decimal) > ByteSize");
#endif
            *(decimal*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(decimal);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(DateTime value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(DateTime) > ByteSize) throw new Exception("CurrentIndex + sizeof(DateTime) > ByteSize");
#endif
            *(DateTime*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(DateTime);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(ref Guid value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(Guid) > ByteSize) throw new Exception("CurrentIndex + sizeof(Guid) > ByteSize");
#endif
            *(Guid*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(Guid);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(int value1, uint value2)
        {
#if DEBUG
            if (CurrentIndex + sizeof(int) + sizeof(uint) > ByteSize) throw new Exception("CurrentIndex + sizeof(int) + sizeof(uint) > ByteSize");
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = value1;
            *(uint*)(data + sizeof(int)) = value2;
            CurrentIndex += sizeof(int) + sizeof(uint);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(int value1, int value2, int value3)
        {
#if DEBUG
            if (CurrentIndex + sizeof(int) * 3 > ByteSize) throw new Exception("CurrentIndex + sizeof(int) * 3 > ByteSize");
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = value1;
            *(int*)(data + sizeof(int)) = value2;
            *(int*)(data + sizeof(int) * 2) = value3;
            CurrentIndex += sizeof(int) * 3;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeWriteNullable(int value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(int) * 2 > ByteSize) throw new Exception("CurrentIndex + sizeof(int) * 2 > ByteSize");
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = 0;
            *(int*)(data + sizeof(int)) = value;
            CurrentIndex += sizeof(int) << 1;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeWriteNullable(uint value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(int) + sizeof(uint) > ByteSize) throw new Exception("CurrentIndex + sizeof(int) + sizeof(uint) > ByteSize");
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = 0;
            *(uint*)(data + sizeof(int)) = value;
            CurrentIndex += sizeof(int) + sizeof(uint);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeWriteNullable(long value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(int) + sizeof(long) > ByteSize) throw new Exception("CurrentIndex + sizeof(int) + sizeof(long) > ByteSize");
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = 0;
            *(long*)(data + sizeof(int)) = value;
            CurrentIndex += sizeof(int) + sizeof(long);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeWriteNullable(ulong value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(int) + sizeof(ulong) > ByteSize) throw new Exception("CurrentIndex + sizeof(int) + sizeof(ulong) > ByteSize");
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = 0;
            *(ulong*)(data + sizeof(int)) = value;
            CurrentIndex += sizeof(int) + sizeof(ulong);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeWriteNullable(float value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(int) + sizeof(float) > ByteSize) throw new Exception("CurrentIndex + sizeof(int) + sizeof(float) > ByteSize");
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = 0;
            *(float*)(data + sizeof(int)) = value;
            CurrentIndex += sizeof(int) + sizeof(float);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeWriteNullable(double value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(int) + sizeof(double) > ByteSize) throw new Exception("CurrentIndex + sizeof(int) + sizeof(double) > ByteSize");
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = 0;
            *(double*)(data + sizeof(int)) = value;
            CurrentIndex += sizeof(int) + sizeof(double);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeWriteNullable(decimal value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(int) + sizeof(decimal) > ByteSize) throw new Exception("CurrentIndex + sizeof(int) + sizeof(decimal) > ByteSize");
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = 0;
            *(decimal*)(data + sizeof(int)) = value;
            CurrentIndex += sizeof(int) + sizeof(decimal);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeWriteNullable(DateTime value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(int) + sizeof(DateTime) > ByteSize) throw new Exception("CurrentIndex + sizeof(int) + sizeof(DateTime) > ByteSize");
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = 0;
            *(DateTime*)(data + sizeof(int)) = value;
            CurrentIndex += sizeof(int) + sizeof(DateTime);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeWriteNullable(Guid value)
        {
#if DEBUG
            if (CurrentIndex + sizeof(int) + sizeof(Guid) > ByteSize) throw new Exception("CurrentIndex + sizeof(int) + sizeof(Guid) > ByteSize");
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = 0;
            *(Guid*)(data + sizeof(int)) = value;
            CurrentIndex += sizeof(int) + sizeof(Guid);
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串，长度必须大于0</param>
        internal void Write(string value)
        {
#if DEBUG
            if (CurrentIndex + (value.Length << 1) > ByteSize) throw new Exception("CurrentIndex + value.Length * 2 > ByteSize");
#endif
            value.AsSpan().CopyTo(new Span<char>((byte*)Data + CurrentIndex, value.Length));
            CurrentIndex += value.Length << 1;
        }
        /// <summary>
        /// 写字符串（带双引号）
        /// </summary>
        /// <param name="value"></param>
        internal void WriteQuote(string value)
        {
            Write('"');
            Write(value);
            Write('"');
        }
        /// <summary>
        /// 写 JSON 名称
        /// </summary>
        /// <param name="name"></param>
        internal void WriteJsonCustomNameFirst(string name)
        {
            Write('{' + ('"' << 16));
            SimpleWrite(name);
            Write('"' + (':' << 16));
        }
        /// <summary>
        /// 写 JSON 名称
        /// </summary>
        /// <param name="name"></param>
        internal void WriteJsonCustomNameNext(string name)
        {
            Write(',' + ('"' << 16));
            SimpleWrite(name);
            Write('"' + (':' << 16));
        }
        /// <summary>
        /// 模拟javascript解码函数unescape
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        internal void JavascriptUnescape(byte* start, byte* end)
        {
#if DEBUG
            if (start > end) throw new Exception("start > end");
#endif
            while (start != end && *start != '%')
            {
                Write(*start == 0 ? ' ' : (char)*start);
                ++start;
            }
            if (start != end) javascriptUnescape(start, end);
        }
        /// <summary>
        /// 模拟javascript解码函数unescape
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void javascriptUnescape(byte* start, byte* end)
        {
        NEXT:
            if (*++start == 'u')
            {
                uint code = (uint)(*++start - '0'), number = (uint)(*++start - '0');
                if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
                if (number > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
                code <<= 12;
                code += (number << 8);
                if ((number = (uint)(*++start - '0')) > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
                code += (number << 4);
                number = (uint)(*++start - '0');
                code += (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number);
                Write(code == 0 ? ' ' : (char)code);
            }
            else
            {
                uint code = (uint)(*start - '0'), number = (uint)(*++start - '0');
                if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
                code = (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number) + (code << 4);
                Write(code == 0 ? ' ' : (char)code);
            }
            while (++start < end)
            {
                if (*start == '%') goto NEXT;
                Write(*start == 0 ? ' ' : (char)*start);
            }
        }
        /// <summary>
        /// 复制字符串，适合零碎短小数据(不足8字节按8字节算)
        /// </summary>
        /// <param name="value"></param>
        internal void SimpleWrite(string value)
        {
            if (value.Length != 0)
            {
                fixed (char* valueFixed = value) SimpleWrite((byte*)valueFixed, value.Length << 1);
            }
        }
        /// <summary>
        /// 复制字节数组，适合零碎短小数据(不足8字节按8字节算)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="size">必须大于0</param>
        internal void SimpleWrite(byte* source, int size)
        {
#if DEBUG
            if (source == null) throw new Exception("source == null");
            if (size <= 0) throw new Exception(size.toString() + " <= 0");
#endif
            byte* end = source + ((size + (sizeof(ulong) - 1)) & (int.MaxValue - (sizeof(ulong) - 1))), write = (byte*)Data + CurrentIndex;
#if DEBUG
            if (end - source > ByteSize - CurrentIndex) throw new Exception((end - source).toString() + " >= " + (ByteSize - CurrentIndex).toString());
#endif
            do
            {
                *(ulong*)write = *(ulong*)source;
                source += sizeof(ulong);
                write += sizeof(ulong);
            }
            while (source != end);
            CurrentIndex += size;
        }
        /// <summary>
        /// 增加 1 个空白字节
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void SerializeFillByteSize1()
        {
#if DEBUG
            if (CurrentIndex >= ByteSize) throw new Exception("CurrentIndex >= ByteSize");
#endif
            *((byte*)Data + CurrentIndex++) = 0;
        }
        /// <summary>
        /// 增加 2 个空白字节
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void SerializeFillByteSize2()
        {
#if DEBUG
            if (CurrentIndex + 2 > ByteSize) throw new Exception("CurrentIndex + 2 > ByteSize");
#endif
            *(ushort*)((byte*)Data + CurrentIndex) = 0;
            CurrentIndex += 2;
        }
        /// <summary>
        /// 增加 3 个空白字节
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void SerializeFillByteSize3()
        {
#if DEBUG
            if (CurrentIndex + 3 > ByteSize) throw new Exception("CurrentIndex + 3 > ByteSize");
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *data = 0;
            *(ushort*)(data + 1) = 0;
            CurrentIndex += 3;
        }
        /// <summary>
        /// 填充空白字节
        /// </summary>
        /// <param name="fillSize">字节数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeFill(int fillSize)
        {
            switch (fillSize)
            {
                case 1: SerializeFillByteSize1(); return;
                case 2: SerializeFillByteSize2(); return;
                case 3: SerializeFillByteSize3(); return;
            }
        }
        /// <summary>
        /// 补白对齐 4 字节
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeFillWithStartIndex(int startIndex)
        {
            switch ((CurrentIndex - startIndex) & 3)
            {
                case 1: SerializeFillByteSize3(); return;
                case 2: SerializeFillByteSize2(); return;
                case 3: SerializeFillByteSize1(); return;
            }
        }
        /// <summary>
        /// 左侧补白对齐 4 字节
        /// </summary>
        /// <param name="size">增加数据长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeFillLeftByteSize(int size)
        {
            switch (size & 3)
            {
                case 1:
#if DEBUG
                    if (CurrentIndex < 3) throw new Exception("CurrentIndex < 3");
#endif
                    byte* data = (byte*)Data + (CurrentIndex - 3);
                    *data = 0;
                    *(ushort*)(data + 1) = 0;
                    return;
                case 2:
#if DEBUG
                    if (CurrentIndex < 2) throw new Exception("CurrentIndex < 2");
#endif
                    *(ushort*)((byte*)Data + (CurrentIndex - 2)) = 0;
                    return;
                case 3:
#if DEBUG
                    if (CurrentIndex < 1) throw new Exception("CurrentIndex < 1");
#endif
                    *((byte*)Data + (CurrentIndex - 1)) = 0;
                    return;
            }
        }
        /// <summary>
        /// 左侧补白对齐 4 字节
        /// </summary>
        /// <param name="size">增加数据长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeFillLeftByteSize2(int size)
        {
#if DEBUG
            if (CurrentIndex < 2) throw new Exception("CurrentIndex < 2");
#endif
            if ((size & 2) != 0) *(ushort*)((byte*)Data + (CurrentIndex - 2)) = 0;
        }
        /// <summary>
        /// 增加当前数据长度并且补白对齐 4 字节
        /// </summary>
        /// <param name="current">新的当前位置</param>
        internal void SerializeFillByteSize(void* current)
        {
            int currentIndex = (int)((byte*)current - (byte*)Data);
#if DEBUG
            if (currentIndex < 0) throw new Exception("currentIndex < 0");
            if (currentIndex > ByteSize) throw new Exception("currentIndex > ByteSize");
#endif
            switch ((currentIndex - CurrentIndex) & 3)
            {
                case 1:
                    *(byte*)current = 0;
                    *(ushort*)((byte*)current + 1) = 0;
                    CurrentIndex = currentIndex + 3;
                    return;
                case 2:
                    *(ushort*)current = 0;
                    CurrentIndex = currentIndex + 2;
                    return;
                case 3:
                    *(byte*)current = 0;
                    CurrentIndex = currentIndex + 1;
                    return;
                default: CurrentIndex = currentIndex; return;
            }
        }
        /// <summary>
        /// 增加当前数据长度并且补白对齐 4 字节
        /// </summary>
        /// <param name="current">新的当前位置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeFillByteSize2(void* current)
        {
            int currentIndex = (int)((byte*)current - (byte*)Data);
#if DEBUG
            if (currentIndex < 0) throw new Exception("currentIndex < 0");
#endif
            if (((currentIndex - CurrentIndex) & 2) == 0)
            {
#if DEBUG
                if (currentIndex > ByteSize) throw new Exception("currentIndex > ByteSize");
#endif
                CurrentIndex = currentIndex;
            }
            else
            {
#if DEBUG
                if (currentIndex + 2 > ByteSize) throw new Exception("currentIndex + 2 > ByteSize");
#endif
                *(ushort*)current = 0;
                CurrentIndex = currentIndex + 2;
            }
        }
        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <returns>字节数组</returns>
        public byte[] GetArray()
        {
            if (CurrentIndex == 0) return EmptyArray<byte>.Array;
#if DEBUG
            DebugCheck();
#endif
            byte[] data = new byte[CurrentIndex];
            new Span<byte>(Data, CurrentIndex).CopyTo(data.AsSpan());
            return data;
        }
        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe void GetSubBuffer(ref SubBuffer.PoolBufferFull buffer)
        {
#if DEBUG
            DebugCheck();
#endif
            SubBuffer.Pool.GetBuffer(ref buffer, CurrentIndex);
            new Span<byte>(Byte, CurrentIndex).CopyTo(buffer.Buffer.AsSpan(buffer.StartIndex, CurrentIndex));
        }
        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index">复制起始位置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe void GetSubBuffer(ref SubBuffer.PoolBufferFull buffer, int index)
        {
#if DEBUG
            if (index < 0) throw new Exception("index < 0");
            if (index >= CurrentIndex) throw new Exception("index >= CurrentIndex");
            DebugCheck();
#endif
            int size = CurrentIndex - index;
            SubBuffer.Pool.GetBuffer(ref buffer, CurrentIndex);
            new Span<byte>(Byte + index, size).CopyTo(buffer.Buffer.AsSpan(buffer.StartIndex + index, size));
        }

#if DEBUG
        /// <summary>
        /// 检查数据
        /// </summary>
        internal void DebugCheck()
        {
            if (CurrentIndex < 0) throw new Exception(CurrentIndex.toString() + " < 0");
            if (CurrentIndex > ByteSize) throw new Exception(CurrentIndex.toString() + " > " + ByteSize.toString());
        }
#endif
    }
}
