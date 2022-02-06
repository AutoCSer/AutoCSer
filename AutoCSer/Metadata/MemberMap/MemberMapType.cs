using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 成员位图类型信息
    /// </summary>
    internal sealed unsafe class MemberMapType
    {
        /// <summary>
        /// 类型
        /// </summary>
        internal readonly Type Type;
        /// <summary>
        /// 成员位图内存池
        /// </summary>
        internal readonly MemberMapPool Pool;
        /// <summary>
        /// 名称索引查找数据
        /// </summary>
        internal readonly AutoCSer.StateSearcher.CharSearcher NameIndexSearcher;
        /// <summary>
        /// 成员数量
        /// </summary>
        internal readonly int MemberCount;
        /// <summary>
        /// 字段成员数量
        /// </summary>
        internal readonly int FieldCount;
        /// <summary>
        /// 成员位图字节数量
        /// </summary>
        internal readonly int MemberMapSize;
        /// <summary>
        /// 字段成员位图序列化字节数量
        /// </summary>
        internal readonly int BinarySerializeSize;
        /// <summary>
        /// 成员位图类型信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="members">成员索引集合</param>
        /// <param name="fieldCount">字段成员数量</param>
        internal MemberMapType(Type type, AutoCSer.Metadata.MemberIndexInfo[] members, int fieldCount)
        {
            Type = type;
            MemberCount = members.Length;
            FieldCount = fieldCount;
            MemberMapSize = ((MemberCount + 63) >> 6) << 3;
            BinarySerializeSize = ((fieldCount + 31) >> 5) << 2;
            Pool = MemberMapPool.GetPool(MemberMapSize);
            NameIndexSearcher = new AutoCSer.StateSearcher.CharSearcher(AutoCSer.StateSearcher.CharBuilder.Create(members.getArray(value => value.Member.Name), true));
        }
        /// <summary>
        /// 获取成员索引
        /// </summary>
        /// <param name="name">成员名称</param>
        /// <returns>成员索引,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int GetMemberIndex(string name)
        {
            return name != null ? NameIndexSearcher.Search(name) : -1;
        }
        /// <summary>
        /// 获取成员位图
        /// </summary>
        /// <returns>成员位图</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal byte* GetClear()
        {
            if (Pool != null) return Pool.GetClear();
            return MemberMapSize != 0 ? (byte*)AutoCSer.Memory.Unmanaged.Get(MemberMapSize, true) : null;
        }
        /// <summary>
        /// 获取成员位图
        /// </summary>
        /// <returns>成员位图</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal byte* GetMap()
        {
            if (Pool != null) return Pool.Get();
            return MemberMapSize != 0 ? (byte*)AutoCSer.Memory.Unmanaged.Get(MemberMapSize, false) : null;
        }
        /// <summary>
        /// 成员位图入池
        /// </summary>
        /// <param name="map">成员位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Push(ref byte* map)
        {
            if (Pool != null) Pool.Push(ref map);
            else if (MemberMapSize != 0) AutoCSer.Memory.Unmanaged.Free(ref map, MemberMapSize);
        }
    }
}
