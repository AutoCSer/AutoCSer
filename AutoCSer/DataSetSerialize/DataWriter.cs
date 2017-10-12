using System;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.DataSetSerialize
{
    /// <summary>
    /// 数据流包装器
    /// </summary>
    internal class DataWriter : IDisposable
    {
        /// <summary>
        /// 数据流
        /// </summary>
        private UnmanagedStream stream = new UnmanagedStream();
        /// <summary>
        /// 字符串集合
        /// </summary>
        private LeftArray<string> strings;
        /// <summary>
        /// 字节数组集合
        /// </summary>
        private LeftArray<byte[]> bytes;
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        /// <param name="typeIndex">数据类型</param>
        internal void Append(object value, byte typeIndex)
        {
            switch (typeIndex)
            {
                case 0:
                    stream.Write((int)value);
                    break;
                case 1:
                    stream.Write((int)(int?)value);
                    break;
                case 2:
                    strings.Add((string)value);
                    break;
                case 3:
                    stream.Write(((DateTime)value).toKindTicks());
                    break;
                case 4:
                    stream.Write(((DateTime)(DateTime?)value).toKindTicks());
                    break;
                case 5:
                    stream.Write((double)value);
                    break;
                case 6:
                    stream.Write((double)(double?)value);
                    break;
                case 7:
                    stream.Write((float)value);
                    break;
                case 8:
                    stream.Write((float)(float?)value);
                    break;
                case 9:
                    stream.Write((decimal)value);
                    break;
                case 10:
                    stream.Write((decimal)(decimal?)value);
                    break;
                case 11:
                    stream.Write((Guid)value);
                    break;
                case 12:
                    stream.Write((Guid)(Guid?)value);
                    break;
                case 13:
                    stream.Write((bool)value ? (byte)1 : (byte)0);
                    break;
                case 14:
                    stream.Write((bool)(bool?)value ? (byte)1 : (byte)0);
                    break;
                case 15:
                    stream.Write((byte)value);
                    break;
                case 16:
                    stream.Write((byte)(byte?)value);
                    break;
                case 17:
                    bytes.Add((byte[])value);
                    break;
                case 18:
                    stream.Write((sbyte)value);
                    break;
                case 19:
                    stream.Write((sbyte)(sbyte?)value);
                    break;
                case 20:
                    stream.Write((short)value);
                    break;
                case 21:
                    stream.Write((short)(short?)value);
                    break;
                case 22:
                    stream.Write((ushort)value);
                    break;
                case 23:
                    stream.Write((ushort)(ushort?)value);
                    break;
                case 24:
                    stream.Write((uint)value);
                    break;
                case 25:
                    stream.Write((uint)(uint?)value);
                    break;
                case 26:
                    stream.Write((long)value);
                    break;
                case 27:
                    stream.Write((long)(long?)value);
                    break;
                case 28:
                    stream.Write((ulong)value);
                    break;
                case 29:
                    stream.Write((ulong)(ulong?)value);
                    break;
            }
        }
        /// <summary>
        /// 获取数据源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Get(ref DataSource dataSource)
        {
            dataSource.Set(stream.GetArray(), strings.ToArray(), bytes.ToArray());
        }
        /// <summary>
        /// 释放数据流
        /// </summary>
        public void Dispose()
        {
            stream.Dispose();
            stream = null;
        }
    }
}
