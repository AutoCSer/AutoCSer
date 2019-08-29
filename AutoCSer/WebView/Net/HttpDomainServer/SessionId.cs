using System;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.HttpDomainServer
{
    /// <summary>
    /// 会话标识
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct SessionId : IEquatable<SessionId>
    {
        /// <summary>
        /// 低32位
        /// </summary>
        [FieldOffset(0)]
        private uint bit0;
        /// <summary>
        /// 32-64位
        /// </summary>
        [FieldOffset(sizeof(uint))]
        private uint bit32;
        /// <summary>
        /// 64-96位
        /// </summary>
        [FieldOffset(sizeof(ulong))]
        private uint bit64;
        /// <summary>
        /// 96-128位
        /// </summary>
        [FieldOffset(sizeof(ulong) + sizeof(uint))]
        private uint bit96;
        /// <summary>
        /// 128-160位
        /// </summary>
        [FieldOffset(sizeof(ulong) * 2)]
        private uint bit128;
        /// <summary>
        /// 160-192位
        /// </summary>
        [FieldOffset(sizeof(ulong) * 2 + sizeof(uint))]
        private uint bit160;
        /// <summary>
        /// 192-224位
        /// </summary>
        [FieldOffset(sizeof(ulong) * 3)]
        private uint bit192;
        /// <summary>
        /// 高32位
        /// </summary>
        [FieldOffset(sizeof(ulong) * 3 + sizeof(uint))]
        private uint bit224;
        /// <summary>
        /// 服务器时间戳
        /// </summary>
        [FieldOffset(0)]
        internal ulong Ticks;
        /// <summary>
        /// 服务器自增标识
        /// </summary>
        [FieldOffset(sizeof(ulong))]
        internal ulong Identity;
        /// <summary>
        /// 索引
        /// </summary>
        [FieldOffset(sizeof(ulong))]
        internal int Index;
        /// <summary>
        /// 索引标识
        /// </summary>
        [FieldOffset(sizeof(ulong) + sizeof(uint))]
        internal uint IndexIdentity;
        /// <summary>
        /// 随机数低64位
        /// </summary>
        [FieldOffset(sizeof(ulong) * 2)]
        internal ulong Low;
        /// <summary>
        /// 随机数高64位
        /// </summary>
        [FieldOffset(sizeof(ulong) * 3)]
        internal ulong High;
        /// <summary>
        /// 空值判断参数
        /// </summary>
        internal ulong CookieValue
        {
            get { return Ticks | Low; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(SessionId other)
        {
            return ((Low ^ other.Low) | (High ^ other.High)) == 0 && ((Identity ^ other.Identity) | (Ticks ^ other.Ticks)) == 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns>相等返回0</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ulong Equals(ref SessionId other)
        {
            return (Low ^ other.Low) | (High ^ other.High) | (Identity ^ other.Identity) | (Ticks ^ other.Ticks);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            ulong value = Low ^ High ^ Ticks ^ Identity;
            return (int)((uint)value ^ (uint)(value >> 32));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((SessionId)obj);
        }
        /// <summary>
        /// 重置会话标识
        /// </summary>
        internal void NewNoIndex()
        {
            Low = AutoCSer.Random.Default.SecureNextULongNotZero();
            Ticks = (ulong)Pub.StartTime.Ticks;
            highRandom ^= Low;
            High = (highRandom << 11) | (highRandom >> 53);
        }
        /// <summary>
        /// 清空数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Null()
        {
            Ticks = Low = 0;
        }
        /// <summary>
        /// Cookie 解析
        /// </summary>
        /// <param name="data"></param>
        internal unsafe void FromCookie(ref SubArray<byte> data)
        {
            if (data.Length == 64)
            {
                fixed (byte* dataFixed = data.Array)
                {
                    byte* start = dataFixed + data.StartIndex;
                    bit32 = AutoCSer.Extension.Number.ParseHex32(start);
                    bit0 = AutoCSer.Extension.Number.ParseHex32(start + 8);

                    bit96 = AutoCSer.Extension.Number.ParseHex32(start + 16);
                    bit64 = AutoCSer.Extension.Number.ParseHex32(start + 24);

                    bit160 = AutoCSer.Extension.Number.ParseHex32(start + 32);
                    bit128 = AutoCSer.Extension.Number.ParseHex32(start + 40);

                    bit224 = AutoCSer.Extension.Number.ParseHex32(start + 48);
                    bit192 = AutoCSer.Extension.Number.ParseHex32(start + 56);
                }
                return;
            }
            Low = 1;
            Ticks = 0;
        }
        /// <summary>
        /// 转换成16进制字符串
        /// </summary>
        /// <returns></returns>
        internal unsafe byte[] ToCookie()
        {
            byte[] data = new byte[64];
            fixed (byte* dataFixed = data)
            {
                AutoCSer.Extension.Number.ToHex(Ticks, dataFixed);
                AutoCSer.Extension.Number.ToHex(Identity, dataFixed + 16);
                AutoCSer.Extension.Number.ToHex(Low, dataFixed + 32);
                AutoCSer.Extension.Number.ToHex(High, dataFixed + 48);
            }
            return data;
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer">对象序列化器</param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [AutoCSer.BinarySerialize.SerializeCustom]
        private unsafe void serialize(AutoCSer.BinarySerialize.Serializer serializer)
        {
            UnmanagedStream stream = serializer.Stream;
            stream.PrepLength(sizeof(ulong) * 4);
            byte* write = stream.CurrentData;
            *(ulong*)write = Ticks;
            *(ulong*)(write + sizeof(ulong)) = Identity;
            *(ulong*)(write + sizeof(ulong) * 2) = Low;
            *(ulong*)(write + sizeof(ulong) * 3) = High;
            stream.ByteSize += sizeof(ulong) * 4;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deSerializer">对象反序列化器</param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [AutoCSer.BinarySerialize.SerializeCustom]
        private unsafe void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
        {
            if (deSerializer.MoveReadAny(sizeof(ulong) * 4))
            {
                byte* dataStart = deSerializer.Read;
                Ticks = *(ulong*)(dataStart - sizeof(ulong) * 4);
                Identity = *(ulong*)(dataStart - sizeof(ulong) * 3);
                Low = *(ulong*)(dataStart - sizeof(ulong) * 2);
                High = *(ulong*)(dataStart - sizeof(ulong));
            }
        }

        /// <summary>
        /// 重置会话标识
        /// </summary>
        internal void New()
        {
            Low = AutoCSer.Random.Default.SecureNextULongNotZero();
            Ticks = (ulong)Pub.StartTime.Ticks;
            highRandom ^= Low;
            Identity = (ulong)Interlocked.Increment(ref identityRandom);
            High = (highRandom << 11) | (highRandom >> 53);
        }
        /// <summary>
        /// 判断是否匹配16进制字符串
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        internal unsafe bool CheckHex(string hex)
        {
            if (hex.Length == 64)
            {
                fixed (char* hexFixed = hex) return High.CheckHex(hexFixed + 48) == 0 && (Low.CheckHex(hexFixed + 32) | Identity.CheckHex(hexFixed + 16) | Ticks.CheckHex(hexFixed)) == 0;
            }
            return false;
        }
        /// <summary>
        /// 转换成16进制字符串
        /// </summary>
        /// <returns></returns>
        internal unsafe string ToHex()
        {
            string hex = AutoCSer.Extension.StringExtension.FastAllocateString(64);
            fixed (char* hexFixed = hex)
            {
                Ticks.toHex(hexFixed);
                Identity.toHex(hexFixed + 16);
                Low.toHex(hexFixed + 32);
                High.toHex(hexFixed + 48);
            }
            return hex;
        }

        /// <summary>
        /// 随机数高位
        /// </summary>
        private static ulong highRandom = AutoCSer.Random.Default.SecureNextULong();
        /// <summary>
        /// 随机数自增标识
        /// </summary>
        private static long identityRandom = (long)AutoCSer.Random.Default.SecureNextULong();
    }
}
