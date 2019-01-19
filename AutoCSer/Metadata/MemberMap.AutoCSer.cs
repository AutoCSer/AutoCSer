using System;
using System.Threading;
using AutoCSer.Extension;
using System.Reflection;
using System.Runtime.CompilerServices;
#if !DOTNET2
using System.Linq.Expressions;
#endif

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 成员位图
    /// </summary>
    public unsafe abstract partial class MemberMap
    {
        /// <summary>
        /// 是否存在成员
        /// </summary>
        internal bool IsAnyMember
        {
            get
            {
                if (map == null) return true;
                byte* end = map + Type.MemberMapSize;
                do
                {
                    end -= sizeof(ulong);
                    if (*(ulong*)end != 0) return true;
                }
                while (end != map);
                return false;
            }
        }
        /// <summary>
        /// 复制成员位图
        /// </summary>
        /// <param name="other"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void copyTo(MemberMap other)
        {
            if (map != null) Memory.SimpleCopyNotNull64(map, other.map = Type.GetMap(), Type.MemberMapSize);
        }
        /// <summary>
        /// 成员交集运算
        /// </summary>
        /// <param name="other">成员位图</param>
        protected void and(MemberMap other)
        {
            if (this.map == null) Memory.SimpleCopyNotNull64(other.map, this.map = Type.GetMap(), Type.MemberMapSize);
            else
            {
                byte* write = this.map, end = this.map + Type.MemberMapSize, read = other.map;
                *(ulong*)write &= *(ulong*)read;
                while ((write += sizeof(ulong)) != end) *(ulong*)write &= *(ulong*)(read += sizeof(ulong));
            }
        }
        /// <summary>
        /// 成员异或运算,忽略默认成员
        /// </summary>
        /// <param name="other">成员位图</param>
        protected void xor(MemberMap other)
        {
            if (map != null)
            {
                byte* write = this.map, end = this.map + Type.MemberMapSize, read = other.map;
                *(ulong*)write ^= *(ulong*)read;
                while ((write += sizeof(ulong)) != end) *(ulong*)write ^= *(ulong*)(read += sizeof(ulong));
            }
        }
    }
    /// <summary>
    /// 成员位图
    /// </summary>
    public unsafe sealed partial class MemberMap<valueType>
    {
        /// <summary>
        /// 创建成员位图
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        public struct Builder
        {
            /// <summary>
            /// 成员位图
            /// </summary>
            private MemberMap<valueType> memberMap;
            /// <summary>
            /// 创建成员位图
            /// </summary>
            /// <param name="memberMap">成员位图</param>
            public Builder(MemberMap<valueType> memberMap)
            {
                this.memberMap = memberMap;
            }
            /// <summary>
            /// 创建成员位图
            /// </summary>
            /// <param name="isNew">是否创建成员</param>
            internal Builder(bool isNew)
            {
                memberMap = isNew ? new MemberMap<valueType>() : null;
            }
            /// <summary>
            /// 成员位图
            /// </summary>
            /// <param name="value">创建成员位图</param>
            /// <returns>成员位图</returns>
            public static implicit operator MemberMap<valueType>(Builder value)
            {
                return value.memberMap;
            }
            /// <summary>
            /// 清除成员
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public Builder Clear(MemberIndex index)
            {
                if (memberMap != null) memberMap.ClearMember(index.Index);
                return this;
            }
            /// <summary>
            /// 设置成员
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public Builder Set(MemberIndex index)
            {
                if (memberMap != null) memberMap.SetMember(index.Index);
                return this;
            }
#if DOTNET2
            /// <summary>
            /// 清除成员
            /// </summary>
            /// <param name="memberName"></param>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public Builder Clear(string memberName)
            {
                if (memberMap != null) memberMap.ClearMember(memberName);
                return this;
            }
            /// <summary>
            /// 设置成员
            /// </summary>
            /// <param name="memberName"></param>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public Builder Set(string memberName)
            {
                if (memberMap != null) memberMap.SetMember(memberName);
                return this;
            }
#else
            /// <summary>
            /// 清除成员
            /// </summary>
            /// <typeparam name="returnType"></typeparam>
            /// <param name="member"></param>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public Builder Clear<returnType>(Expression<Func<valueType, returnType>> member)
            {
                if (memberMap != null) memberMap.ClearMember(member);
                return this;
            }
            /// <summary>
            /// 设置成员
            /// </summary>
            /// <typeparam name="returnType"></typeparam>
            /// <param name="member"></param>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public Builder Set<returnType>(Expression<Func<valueType, returnType>> member)
            {
                if (memberMap != null) memberMap.SetMember(member);
                return this;
            }
#endif
        }
        /// <summary>
        /// 成员索引
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        public struct MemberIndex
        {
            /// <summary>
            /// 成员索引
            /// </summary>
            internal int Index;
            /// <summary>
            /// 成员索引
            /// </summary>
            /// <param name="index">成员索引</param>
            private MemberIndex(int index)
            {
                Index = index;
            }
            /// <summary>
            /// 判断成员索引是否有效
            /// </summary>
            /// <param name="memberMap"></param>
            /// <returns>成员索引是否有效</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public bool IsMember(MemberMap<valueType> memberMap)
            {
                return memberMap != null && Index != -1 && memberMap.IsMember(Index);
            }
            /// <summary>
            /// 清除成员索引,忽略默认成员
            /// </summary>
            /// <param name="memberMap"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void ClearMember(MemberMap<valueType> memberMap)
            {
                if (memberMap != null && Index != -1) memberMap.ClearMember(Index);
            }
            /// <summary>
            /// 设置成员索引,忽略默认成员
            /// </summary>
            /// <param name="memberMap"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void SetMember(MemberMap<valueType> memberMap)
            {
                if (memberMap != null && Index != -1) memberMap.SetMember(Index);
            }
#if !DOTNET2
            /// <summary>
            /// 创建成员索引
            /// </summary>
            /// <typeparam name="returnType"></typeparam>
            /// <param name="member"></param>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public static MemberIndex Create<returnType>(Expression<Func<valueType, returnType>> member)
            {
                int index = TypeInfo.GetMemberIndex(GetMemberName(member));
                return new MemberIndex(index >= 0 ? index : -1);
            }
#endif
        }
        /// <summary>
        /// 默认成员位图
        /// </summary>
        internal static readonly MemberMap<valueType> Default = new MemberMap<valueType>();

        /// <summary>
        /// 成员位图
        /// </summary>
        /// <returns>成员位图</returns>
        internal MemberMap<valueType> Copy()
        {
            MemberMap<valueType> value = new MemberMap<valueType>();
            copyTo(value);
            return value;
        }
        /// <summary>
        /// 成员交集运算
        /// </summary>
        /// <param name="other">成员位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void And(MemberMap<valueType> other)
        {
            if (other != null && !other.IsDefault) and(other);
        }
        /// <summary>
        /// 成员异或运算,忽略默认成员
        /// </summary>
        /// <param name="other">成员位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Xor(MemberMap<valueType> other)
        {
            if (other != null && !other.IsDefault) xor(other);
        }
        /// <summary>
        /// 成员并集运算,忽略默认成员
        /// </summary>
        /// <param name="other">成员位图</param>
        internal void Or(MemberMap<valueType> other)
        {
            if (map == null) return;
            if (other == null || other.IsDefault)
            {
                TypeInfo.Push(ref map);
                return;
            }
            byte* write = this.map, end = this.map + TypeInfo.MemberMapSize, read = other.map;
            *(ulong*)write |= *(ulong*)read;
            while ((write += sizeof(ulong)) != end) *(ulong*)write |= *(ulong*)(read += sizeof(ulong));
        }
    }
}
