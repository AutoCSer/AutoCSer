using System;

namespace AutoCSer.DataSetSerialize
{
    /// <summary>
    /// 数据对象拆包器
    /// </summary>
    internal unsafe class DataReader
    {
        /// <summary>
        /// 数据流
        /// </summary>
        private byte* data;
        /// <summary>
        /// 字符串集合
        /// </summary>
        private string[] strings;
        /// <summary>
        /// 当前字符串索引
        /// </summary>
        private int stringIndex;
        /// <summary>
        /// 字节数组集合
        /// </summary>
        private byte[][] bytes;
        /// <summary>
        /// 当前字节数组索引
        /// </summary>
        private int byteIndex;
        /// <summary>
        /// 数据对象拆包器
        /// </summary>
        /// <param name="data">数据流</param>
        /// <param name="strings">字符串集合</param>
        /// <param name="bytes">字节数组集合</param>
        internal DataReader(byte* data, string[] strings, byte[][] bytes)
        {
            this.data = data;
            this.strings = strings;
            this.bytes = bytes;
        }
        /// <summary>
        /// 获取下一个数据对象
        /// </summary>
        /// <param name="typeIndex">数据类型</param>
        /// <returns>数据对象</returns>
        internal object Get(byte typeIndex)
        {
            object value;
            switch (typeIndex)
            {
                case 0:
                    value = *(int*)data;
                    data += sizeof(int);
                    return value;
                case 1:
                    value = (int?)*(int*)data;
                    data += sizeof(int);
                    return value;
                case 2:
                    return strings[stringIndex++];
                case 3:
                    value = AutoCSer.Extension.DateTime_DataSetSerialize.FromKindTicks(*(ulong*)data);
                    data += sizeof(long);
                    return value;
                case 4:
                    value = (DateTime?)AutoCSer.Extension.DateTime_DataSetSerialize.FromKindTicks(*(ulong*)data);
                    data += sizeof(long);
                    return value;
                case 5:
                    value = *(double*)data;
                    data += sizeof(double);
                    return value;
                case 6:
                    value = (double?)*(double*)data;
                    data += sizeof(double);
                    return value;
                case 7:
                    value = *(float*)data;
                    data += sizeof(float);
                    return value;
                case 8:
                    value = (float?)*(float*)data;
                    data += sizeof(float);
                    return value;
                case 9:
                    value = *(decimal*)data;
                    data += sizeof(decimal);
                    return value;
                case 10:
                    value = (decimal?)*(decimal*)data;
                    data += sizeof(decimal);
                    return value;
                case 11:
                    value = *(Guid*)data;
                    data += sizeof(Guid);
                    return value;
                case 12:
                    value = (Guid?)*(Guid*)data;
                    data += sizeof(Guid);
                    return value;
                case 13:
                    value = *(byte*)data != 0;
                    ++data;
                    return value;
                case 14:
                    value = (bool?)(*(byte*)data != 0);
                    ++data;
                    return value;
                case 15:
                    value = *(byte*)data;
                    ++data;
                    return value;
                case 16:
                    value = (byte?)*(byte*)data;
                    ++data;
                    return value;
                case 17:
                    return bytes[byteIndex++];
                case 18:
                    value = *(sbyte*)data;
                    ++data;
                    return value;
                case 19:
                    value = (sbyte?)*(sbyte*)data;
                    ++data;
                    return value;
                case 20:
                    value = *(short*)data;
                    data += sizeof(short);
                    return value;
                case 21:
                    value = (short?)*(short*)data;
                    data += sizeof(short);
                    return value;
                case 22:
                    value = *(ushort*)data;
                    data += sizeof(ushort);
                    return value;
                case 23:
                    value = (ushort?)*(ushort*)data;
                    data += sizeof(ushort);
                    return value;
                case 24:
                    value = *(uint*)data;
                    data += sizeof(uint);
                    return value;
                case 25:
                    value = (uint?)*(uint*)data;
                    data += sizeof(uint);
                    return value;
                case 26:
                    value = *(long*)data;
                    data += sizeof(long);
                    return value;
                case 27:
                    value = (long?)*(long*)data;
                    data += sizeof(long);
                    return value;
                case 28:
                    value = *(ulong*)data;
                    data += sizeof(ulong);
                    return value;
                case 29:
                    value = (ulong?)*(ulong*)data;
                    data += sizeof(ulong);
                    return value;
            }
            return null;
        }
    }
}
