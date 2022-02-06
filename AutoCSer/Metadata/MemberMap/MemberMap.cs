using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 成员位图
    /// </summary>
    public unsafe abstract partial class MemberMap : IDisposable
    {
        /// <summary>
        /// 成员位图
        /// </summary>
        protected byte* map;
        /// <summary>
        /// 成员位图类型信息
        /// </summary>
        internal readonly MemberMapType Type;
        /// <summary>
        /// 成员位图
        /// </summary>
        /// <param name="type">成员位图类型信息</param>
        internal MemberMap(MemberMapType type)
        {
            Type = type;
        }
        /// <summary>
        ///  析构释放资源
        /// </summary>
        ~MemberMap()
        {
            if (map != null) Type.Push(ref map);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Type.Push(ref map);
        }
        /// <summary>
        /// 清空所有成员
        /// </summary>
        public void Empty()
        {
            if (map == null)
            {
                if (Type.MemberMapSize == 0) return;
                map = Type.GetClear();
            }
            else AutoCSer.Memory.Common.Clear((ulong*)map, Type.MemberMapSize >> 3);
        }
        /// <summary>
        /// 判断成员索引是否有效
        /// </summary>
        /// <param name="memberIndex">成员索引</param>
        /// <returns>成员索引是否有效</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool IsMember(int memberIndex)
        {
            return map == null || (map[memberIndex >> 3] & (1 << (memberIndex & 7))) != 0;
        }
        /// <summary>
        /// 判断成员索引是否有效
        /// </summary>
        /// <param name="memberMap">成员位图</param>
        /// <param name="memberIndex">成员索引</param>
        /// <returns>成员索引是否有效</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool IsMember(MemberMap memberMap, int memberIndex)
        {
            return memberMap.IsMember(memberIndex);
        }
        /// <summary>
        /// 设置成员索引,忽略默认成员
        /// </summary>
        /// <param name="memberIndex">成员索引</param>
        internal void SetMember(int memberIndex)
        {
            if (map == null)
            {
                if (Type.MemberMapSize == 0) return;
                map = Type.GetClear();
            }
            map[memberIndex >> 3] |= (byte)(1 << (memberIndex & 7));
        }
        /// <summary>
        /// 设置成员索引,忽略默认成员
        /// </summary>
        /// <param name="memberMap">成员位图</param>
        /// <param name="memberIndex">成员索引</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void SetMember(MemberMap memberMap, int memberIndex)
        {
            memberMap.SetMember(memberIndex);
        }
    }
    /// <summary>
    /// 成员位图
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public sealed partial class MemberMap<T> : MemberMap
    {
        /// <summary>
        /// 成员位图类型信息
        /// </summary>
        internal static readonly MemberMapType MemberMapType = new MemberMapType(typeof(T), MemberIndexGroup<T>.GetAllMembers(), MemberIndexGroup<T>.Group.FieldCount);
        /// <summary>
        /// 成员位图
        /// </summary>
        internal MemberMap() : base(MemberMapType) { }
    }
}
