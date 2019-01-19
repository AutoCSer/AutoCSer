using System;
using System.Runtime.CompilerServices;
using AutoCSer.Metadata;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 序列化配置参数
    /// </summary>
    public sealed class SerializeConfig
    {
        /// <summary>
        /// 序列化头部数据
        /// </summary>
        internal const uint HeaderMapValue = 0x51031000U;
        /// <summary>
        /// 序列化头部数据
        /// </summary>
        internal const uint HeaderMapAndValue = 0xffffff00U;
        /// <summary>
        /// 是否序列化成员位图
        /// </summary>
        internal const int MemberMapValue = 1;
        /// <summary>
        /// 是否检测引用类型对象的真实类型
        /// </summary>
        internal const int ObjectRealTypeValue = 2;
        /// <summary>
        /// 是否检测全局版本编号
        /// </summary>
        internal const int GlobalVersionValue = 4;

        /// <summary>
        /// 成员位图
        /// </summary>
        public MemberMap MemberMap;
        ///// <summary>
        ///// 是否序列化成员位图
        ///// </summary>
        //public bool IsMemberMap;
        /// <summary>
        /// 是否检测引用类型对象的真实类型
        /// </summary>
        public bool IsRealType;
        /// <summary>
        /// 全局版本编号
        /// </summary>
        public uint GlobalVersion;
        /// <summary>
        /// 全局版本编号
        /// </summary>
        private uint globalVersion;
        /// <summary>
        /// 序列化头部数据
        /// </summary>
        private int headerValue;
        /// <summary>
        /// 序列化头部数据
        /// </summary>
        internal int HeaderValue
        {
            get
            {
                if (headerValue == 0) createHeaderValue();
                return headerValue;
            }
        }
        /// <summary>
        /// 初始化序列化头部数据
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void createHeaderValue()
        {
            globalVersion = GlobalVersion;
            headerValue = (int)HeaderMapValue;
            if (MemberMap != null) headerValue |= MemberMapValue;
            if (IsRealType) headerValue |= ObjectRealTypeValue;
            if (globalVersion != 0) headerValue |= GlobalVersionValue;
        }

        /// <summary>
        /// 写入序列化头部数据
        /// </summary>
        /// <param name="stream"></param>

        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe void WriteHeader(UnmanagedStream stream)
        {
            int headerValue = HeaderValue;
            if (globalVersion == 0) stream.Write(headerValue);
            else
            {
                byte* write = stream.GetPrepSizeCurrent(sizeof(int) + sizeof(uint));
                *(int*)write = headerValue;
                *(uint*)(write + sizeof(int)) = globalVersion;
                stream.ByteSize += sizeof(int) + sizeof(uint);
            }
        }
        /// <summary>
        /// 写入序列化头部数据
        /// </summary>
        /// <param name="stream"></param>

        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe void UnsafeWriteHeader(UnmanagedStream stream)
        {
            int headerValue = HeaderValue;
            if (globalVersion == 0) stream.UnsafeWrite(headerValue);
            else
            {
                byte* write = stream.CurrentData;
                *(int*)write = headerValue;
                *(uint*)(write + sizeof(int)) = globalVersion;
                stream.ByteSize += sizeof(int) + sizeof(uint);
            }
        }

        /// <summary>
        /// 检测序列化头部数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="globalVersion"></param>
        /// <returns></returns>

        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static bool CheckHeaderValue(byte* start, ref uint globalVersion)
        {
            if ((*(int*)start & AutoCSer.BinarySerialize.SerializeConfig.HeaderMapAndValue) == AutoCSer.BinarySerialize.SerializeConfig.HeaderMapValue)
            {
                if ((*(int*)start & GlobalVersionValue) != 0) globalVersion = *(uint*)(start + sizeof(int));
                return true;
            }
            return false;
        }
    }
}
