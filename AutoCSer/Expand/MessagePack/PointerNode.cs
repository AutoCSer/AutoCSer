using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Collections.Generic;

namespace AutoCSer.MessagePack
{
    /// <summary>
    /// 数据节点 https://github.com/msgpack/msgpack/blob/master/spec.md
    /// </summary>
    public unsafe struct PointerNode
    {
        /// <summary>
        /// 节点数据起始位置
        /// </summary>
        private byte* start;
        /// <summary>
        /// 数据结束位置
        /// </summary>
        private byte* end;
        /// <summary>
        /// 是否存在数据节点
        /// </summary>
        public bool IsNode
        {
            get { return end > start; }
        }
        /// <summary>
        /// 数据节点
        /// </summary>
        /// <param name="start">节点数据起始位置</param>
        /// <param name="end">数据最大结束位置</param>
        public PointerNode(byte* start, byte* end)
        {
            this.start = start;
            this.end = end;
            //end = ignore(start);
            //this.end = end != null && end <= this.end ? end : null;
        }
        /// <summary>
        /// 设置数据节点
        /// </summary>
        /// <param name="start">节点数据起始位置</param>
        /// <param name="end">数据最大结束位置</param>
        private void set(byte* start, byte* end)
        {
            this.start = start;
            this.end = end;
        }
        /// <summary>
        /// 节点数据类型
        /// </summary>
        public DataType DataType
        {
            get
            {
                if (start == null) return DataType.Error;
                switch (*start - 0xC0)
                {
                    case 0xC0 - 0xC0: return DataType.Null;
                    case 0xC1 - 0xC0: return DataType.Reserved;
                    case 0xC2 - 0xC0:
                    case 0xC3 - 0xC0: 
                        return DataType.Bool;
                    case 0xC4 - 0xC0:
                    case 0xC5 - 0xC0:
                    case 0xC6 - 0xC0:
                    case 0xD9 - 0xC0:
                    case 0xDA - 0xC0:
                    case 0xDB - 0xC0:
                        return DataType.Memory;
                    case 0xC7 - 0xC0:
                    case 0xC8 - 0xC0:
                    case 0xC9 - 0xC0:
                    case 0xD4 - 0xC0:
                    case 0xD5 - 0xC0:
                    case 0xD6 - 0xC0:
                    case 0xD7 - 0xC0:
                    case 0xD8 - 0xC0:
                        return DataType.Extension;
                    case 0xCA - 0xC0: return DataType.Float;
                    case 0xCB - 0xC0: return DataType.Double;
                    case 0xCC - 0xC0:
                    case 0xCD - 0xC0:
                    case 0xCE - 0xC0:
                    case 0xCF - 0xC0:
                    case 0xD0 - 0xC0: 
                    case 0xD1 - 0xC0:
                    case 0xD2 - 0xC0:
                    case 0xD3 - 0xC0:
                        return DataType.Integer;
                    case 0xDC - 0xC0:
                    case 0xDD - 0xC0:
                        return DataType.Array;
                    case 0xDE - 0xC0:
                    case 0xDF - 0xC0:
                        return DataType.Map;
                }
                switch ((*start >> 4) - 0x8)
                {
                    case 0x8 - 0x8: return DataType.Map;
                    case 0x9 - 0x8: return DataType.Array;
                    case 0xA - 0x8:
                    case 0xB - 0x8:
                        return DataType.Memory;
                }
                return DataType.Integer;
            }
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        public PointerNode[] Array
        {
            get
            {
                if ((*start & 0xf0) == 0x90) return getArray(start + 1, (uint)*start & 0xf);
                switch (*start - 0xC0)
                {
                    case 0xC0 - 0xC0: return null;
                    case 0xDC - 0xC0: return getArray(start + (1 + sizeof(ushort)), AutoCSer.Extension.Memory_Expand.GetUShortBigEndian(start + 1));
                    case 0xDD - 0xC0: return getArray(start + (1 + sizeof(uint)), AutoCSer.Extension.Memory_Expand.GetUIntBigEndian(start + 1));
                }
                throw new InvalidCastException();
            }
        }
        /// <summary>
        /// 获取没有错误的数组
        /// </summary>
        public IEnumerable<PointerNode> NoErrorArray
        {
            get
            {
                PointerNode[] array = Array;
                if (array != null)
                {
                    foreach (PointerNode node in array)
                    {
                        if (node.IsNode) yield return node;
                        else break;
                    }
                }
            }
        }
        /// <summary>
        /// 获取 Map
        /// </summary>
        public KeyValue<PointerNode, PointerNode>[] Map
        {
            get
            {
                if ((*start & 0xf0) == 0x80) return getMap(start + 1, (uint)*start & 0xf);
                switch (*start - 0xC0)
                {
                    case 0xC0 - 0xC0: return null;
                    case 0xDE - 0xC0: return getMap(start + (1 + sizeof(ushort)), AutoCSer.Extension.Memory_Expand.GetUShortBigEndian(start + 1));
                    case 0xDF - 0xC0: return getMap(start + (1 + sizeof(uint)), AutoCSer.Extension.Memory_Expand.GetUIntBigEndian(start + 1));
                }
                throw new InvalidCastException();
            }
        }
        /// <summary>
        /// 获取没有错误的 Map
        /// </summary>
        public IEnumerable<KeyValue<PointerNode, PointerNode>> NoErrorMap
        {
            get
            {
                KeyValue<PointerNode, PointerNode>[] map = Map;
                if (map != null)
                {
                    foreach (KeyValue<PointerNode, PointerNode> node in map)
                    {
                        if (node.Value.IsNode) yield return node;
                        else break;
                    }
                }
            }
        }
        /// <summary>
        /// 获取第一个键值对
        /// </summary>
        public KeyValue<PointerNode, PointerNode> Map0
        {
            get
            {
                if ((*start & 0xf0) == 0x80) return getMap0(start + 1, (uint)*start & 0xf);
                switch (*start - 0xC0)
                {
                    case 0xC0 - 0xC0: return default(KeyValue<PointerNode, PointerNode>);
                    case 0xDE - 0xC0: return getMap0(start + (1 + sizeof(ushort)), AutoCSer.Extension.Memory_Expand.GetUShortBigEndian(start + 1));
                    case 0xDF - 0xC0: return getMap0(start + (1 + sizeof(uint)), AutoCSer.Extension.Memory_Expand.GetUIntBigEndian(start + 1));
                }
                throw new InvalidCastException();
            }
        }
        /// <summary>
        /// 内存数据块
        /// </summary>
        public Pointer.Size Memory
        {
            get
            {
                switch (*start - 0xC0)
                {
                    case 0xC0 - 0xC0: return default(Pointer.Size);
                    case 0xC4 - 0xC0:
                    case 0xD9 - 0xC0:
                        return getMemory(start + 2, start[1]);
                    case 0xC5 - 0xC0:
                    case 0xDA - 0xC0:
                        return getMemory(start + (1 + sizeof(ushort)), AutoCSer.Extension.Memory_Expand.GetUShortBigEndian(start + 1));
                    case 0xC6 - 0xC0:
                    case 0xDB - 0xC0:
                        return getMemory(start + (1 + sizeof(uint)), (int)AutoCSer.Extension.Memory_Expand.GetUIntBigEndian(start + 1));
                }
                switch ((*start >> 4) - 0xA)
                {
                    case 0xA - 0xA:
                    case 0xB - 0xA:
                        return getMemory(start + 1, *start & 0x1f);
                }
                throw new InvalidCastException();
            }
        }
        /// <summary>
        /// 内存数据块
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Pointer.Size getMemory(byte* data, int size)
        {
            if (size >= 0 && data + size <= end) return new Pointer.Size { Data = data, ByteSize = size };
            return default(Pointer.Size);
        }
        /// <summary>
        /// 字节数组
        /// </summary>
        public byte[] ByteArray
        {
            get
            {
                Pointer.Size memory = Memory;
                if (memory.ByteSize > 0)
                {
                    byte[] data = new byte[memory.ByteSize];
                    AutoCSer.Memory.CopyNotNull(memory.Byte, data, memory.ByteSize);
                    return data;
                }
                return memory.Data == null ? null : NullValue<byte>.Array;
            }
        }
        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string GetString(Encoding encoding)
        {
            Pointer.Size memory = Memory;
            if (memory.ByteSize > 0)
            {
                int size = encoding.GetCharCount(memory.Byte, memory.ByteSize);
                string value = AutoCSer.Extension.StringExtension.FastAllocateString(size);
                fixed (char* valueFixed = value) encoding.GetChars(memory.Byte, memory.ByteSize, valueFixed, size);
                return value;
            }
            return memory.Data == null ? null : string.Empty;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="node">数据节点</param>
        /// <returns>整数</returns>
        public static implicit operator long(PointerNode node) { return node.getLong(); }
        /// <summary>
        /// 整数
        /// </summary>
        private long getLong()
        {
            switch (*start - 0xCC)
            {
                case 0xCC - 0xCC: return *(byte*)(start + 1);
                case 0xCD - 0xCC: return AutoCSer.Extension.Memory_Expand.GetUShortBigEndian(start + 1);
                case 0xCE - 0xCC: return AutoCSer.Extension.Memory_Expand.GetUIntBigEndian(start + 1);
                case 0xCF - 0xCC: return (long)AutoCSer.Extension.Memory_Expand.GetULongBigEndian(start + 1);
                case 0xD0 - 0xCC: return *(sbyte*)(start + 1);
                case 0xD1 - 0xCC: return AutoCSer.Extension.Memory_Expand.GetShortBigEndian(start + 1);
                case 0xD2 - 0xCC: return AutoCSer.Extension.Memory_Expand.GetIntBigEndian(start + 1);
                case 0xD3 - 0xCC: return AutoCSer.Extension.Memory_Expand.GetLongBigEndian(start + 1);
            }
            if ((*start & 0x80) == 0) return *start;
            if ((*start & 0xE0) == 0xE0) return *(sbyte*)start;
            throw new InvalidCastException();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="node">数据节点</param>
        /// <returns>无符号整数</returns>
        public static implicit operator ulong(PointerNode node) { return node.getULong(); }
        /// <summary>
        /// 无符号整数
        /// </summary>
        private ulong getULong()
        {
            switch (*start - 0xCC)
            {
                case 0xCC - 0xCC: return *(byte*)(start + 1);
                case 0xCD - 0xCC: return AutoCSer.Extension.Memory_Expand.GetUShortBigEndian(start + 1);
                case 0xCE - 0xCC: return AutoCSer.Extension.Memory_Expand.GetUIntBigEndian(start + 1);
                case 0xCF - 0xCC: return AutoCSer.Extension.Memory_Expand.GetULongBigEndian(start + 1);
                case 0xD0 - 0xCC: return (ulong)(long)*(sbyte*)(start + 1);
                case 0xD1 - 0xCC: return (ulong)(long)AutoCSer.Extension.Memory_Expand.GetShortBigEndian(start + 1);
                case 0xD2 - 0xCC: return (ulong)(long)AutoCSer.Extension.Memory_Expand.GetIntBigEndian(start + 1);
                case 0xD3 - 0xCC: return (ulong)AutoCSer.Extension.Memory_Expand.GetLongBigEndian(start + 1);
            }
            if ((*start & 0x80) == 0) return *start;
            if ((*start & 0xE0) == 0xE0) return (ulong)(long)*(sbyte*)start;
            throw new InvalidCastException();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="node">数据节点</param>
        /// <returns>逻辑值</returns>
        public static implicit operator bool(PointerNode node) { return node.getBool(); }
        /// <summary>
        /// 逻辑值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool getBool()
        {
            switch (*start - 0xC2)
            {
                case 0xC2 - 0xC2: return false;
                case 0xC3 - 0xC2: return true;
            }
            throw new InvalidCastException();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="node">数据节点</param>
        /// <returns>浮点数</returns>
        public static implicit operator float(PointerNode node) { return node.getFloat(); }
        /// <summary>
        /// 浮点数
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private float getFloat()
        {
            if (*start == 0xCA) return *(float*)(start + 1);
            throw new InvalidCastException();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="node">数据节点</param>
        /// <returns>双精度浮点数</returns>
        public static implicit operator double(PointerNode node) { return node.getDouble(); }
        /// <summary>
        /// 双精度浮点数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private double getDouble()
        {
            switch (*start - 0xCA)
            {
                case 0xCA - 0xCA: return *(float*)(start + 1);
                case 0xCB - 0xCA: return *(double*)(start + 1);
            }
            throw new InvalidCastException();
        }
        /// <summary>
        /// 获取数据字节数组
        /// </summary>
        public byte[] GetData()
        {
            byte[] data = new byte[(int)(end - start)];
            fixed (byte* dataFixed = data) AutoCSer.Memory.CopyNotNull(start, dataFixed, data.Length);
            return data;
        }

        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private PointerNode[] getArray(byte* start, uint size)
        {
            if (start > this.end) return null;
            if (size == 0) return NullValue<PointerNode>.Array;
            PointerNode[] nodeArray = new PointerNode[size];
            for (uint index = 0; index != size; ++index)
            {
                byte* end = ignore(start);
                if (end > this.end || end == null) break;
                nodeArray[index].set(start, end);
                start = end;
            }
            return nodeArray;
        }
        /// <summary>
        /// 获取 Map
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private KeyValue<PointerNode, PointerNode>[] getMap(byte* start, uint size)
        {
            if (start > this.end) return null;
            if (size == 0) return NullValue<KeyValue<PointerNode, PointerNode>>.Array;
            KeyValue<PointerNode, PointerNode>[] nodeArray = new KeyValue<PointerNode, PointerNode>[size];
            for (uint index = 0; index != size; ++index)
            {
                byte* keyEnd = ignore(start);
                if (keyEnd > end || keyEnd == null) break;
                byte* valueEnd = ignore(keyEnd);
                if (valueEnd > end || valueEnd == null) break;
                nodeArray[index].Key.set(start, keyEnd);
                nodeArray[index].Value.set(keyEnd, valueEnd);
                start = valueEnd;
            }
            return nodeArray;
        }
        /// <summary>
        /// 获取第一个键值对
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private KeyValue<PointerNode, PointerNode> getMap0(byte* start, uint size)
        {
            KeyValue<PointerNode, PointerNode> keyValue = default(KeyValue<PointerNode, PointerNode>);
            if (size != 0 && start < this.end)
            {
                byte* keyEnd = ignore(start);
                if (keyEnd < end && keyEnd != null)
                {
                    byte* valueEnd = ignore(keyEnd);
                    if (valueEnd <= end && valueEnd != null)
                    {
                        keyValue.Key.set(start, keyEnd);
                        keyValue.Value.set(keyEnd, valueEnd);
                    }
                }
            }
            return keyValue;
        }
        /// <summary>
        /// 获取节点结束位置
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public byte* GetEnd()
        {
            return ignore(start);
        }
        /// <summary>
        /// 忽略数据并返回数据结束位置
        /// </summary>
        /// <param name="start">节点数据起始位置</param>
        /// <returns>数据结束位置</returns>
        private byte* ignore(byte* start)
        {
            switch (*start - 0xC0)
            {
                case 0xC4 - 0xC0:
                case 0xD9 - 0xC0:
                    return start + (start[1] + (1 + 1));
                case 0xC5 - 0xC0:
                case 0xDA - 0xC0:
                    return movePointer(start, (AutoCSer.Extension.Memory_Expand.GetUShortBigEndian(start + 1) + (1 + sizeof(ushort))));
                case 0xC6 - 0xC0:
                case 0xDB - 0xC0:
                    return movePointer(start, AutoCSer.Extension.Memory_Expand.GetUIntBigEndian(start + 1) + (1 + sizeof(uint)));
                case 0xC7 - 0xC0: return start + (start[1] + (2 + 1));
                case 0xC8 - 0xC0: return movePointer(start, (AutoCSer.Extension.Memory_Expand.GetUShortBigEndian(start + 1) + (2 + sizeof(ushort))));
                case 0xC9 - 0xC0: return movePointer(start, (AutoCSer.Extension.Memory_Expand.GetUIntBigEndian(start + 1) + (2 + sizeof(uint))));
                case 0xCA - 0xC0: return start + (1 + sizeof(float));
                case 0xCB - 0xC0: return start + (1 + sizeof(double));
                case 0xCC - 0xC0: return start + (1 + sizeof(byte));
                case 0xCD - 0xC0: return start + (1 + sizeof(ushort));
                case 0xCE - 0xC0: return start + (1 + sizeof(uint));
                case 0xCF - 0xC0: return start + (1 + sizeof(ulong));
                case 0xD0 - 0xC0: return start + (1 + sizeof(sbyte));
                case 0xD1 - 0xC0: return start + (1 + sizeof(short));
                case 0xD2 - 0xC0: return start + (1 + sizeof(int));
                case 0xD3 - 0xC0: return start + (1 + sizeof(long));
                case 0xD4 - 0xC0: return start + (1 + 1 + 1);
                case 0xD5 - 0xC0: return start + (1 + 1 + 2);
                case 0xD6 - 0xC0: return start + (1 + 1 + 4);
                case 0xD7 - 0xC0: return start + (1 + 1 + 8);
                case 0xD8 - 0xC0: return start + (1 + 1 + 16);
                case 0xDC - 0xC0: return ignoreArray(start + (1 + sizeof(ushort)), AutoCSer.Extension.Memory_Expand.GetUShortBigEndian(start + 1));
                case 0xDD - 0xC0: return ignoreArray(start + (1 + sizeof(uint)), AutoCSer.Extension.Memory_Expand.GetUIntBigEndian(start + 1));
                case 0xDE - 0xC0: return ignoreMap(start + (1 + sizeof(ushort)), AutoCSer.Extension.Memory_Expand.GetUShortBigEndian(start + 1));
                case 0xDF - 0xC0: return ignoreMap(start + (1 + sizeof(uint)), AutoCSer.Extension.Memory_Expand.GetUIntBigEndian(start + 1));
            }
            switch ((*start >> 4) - 0x8)
            {
                case 0x8 - 0x8: return ignoreMap(start + 1, (uint)*start & 0xf);
                case 0x9 - 0x8: return ignoreArray(start + 1, (uint)*start & 0xf);
                case 0xA - 0x8:
                case 0xB - 0x8:
                    return start + (1 + (*start & 0x1f));
            }
            return start + 1;
        }
        /// <summary>
        /// 移动指针
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private byte* movePointer(byte* start, long size)
        {
            if (size < 0) return null;
            start += size;
            if (start > end) return null;
            return start;
        }
        /// <summary>
        /// 忽略数组
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private byte* ignoreArray(byte* start, uint size)
        {
            while (size != 0)
            {
                start = ignore(start);
                if (start > end || start == null) return null;
                --size;
            }
            return start;
        }
        /// <summary>
        /// 忽略键值对
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private byte* ignoreMap(byte* start, uint size)
        {
            while (size != 0)
            {
                start = ignore(start);
                if (start > end || start == null) return null;
                start = ignore(start);
                if (start > end || start == null) return null;
                --size;
            }
            return start;
        }
    }
}
