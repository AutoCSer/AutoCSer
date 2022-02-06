using System;
using System.Threading;
using AutoCSer.Extensions;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;
#if !DOTNET2
using System.Linq.Expressions;
#endif

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 成员位图
    /// </summary>
    public unsafe abstract partial class MemberMap : IDisposable
    {
        /// <summary>
        /// 是否默认全部成员有效
        /// </summary>
        public bool IsDefault { get { return map == null; } }
        /// <summary>
        /// 添加所有成员
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Full()
        {
            if (map == null)
            {
                if (Type.MemberMapSize == 0) return;
                map = Type.GetMap();
            }
            for (byte* write = map, end = map + Type.MemberMapSize; write != end; write += sizeof(ulong)) *(ulong*)write = ulong.MaxValue;
        }
        /// <summary>
        /// 清除成员索引,忽略默认成员
        /// </summary>
        /// <param name="memberIndex">成员索引</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ClearMember(int memberIndex)
        {
            if (map != null) map[memberIndex >> 3] &= (byte)(byte.MaxValue ^ (1 << (memberIndex & 7)));
        }
        /// <summary>
        /// 清除成员索引,忽略默认成员
        /// </summary>
        /// <param name="memberName">成员名称</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void ClearMember(string memberName)
        {
            int index = Type.GetMemberIndex(memberName);
            if (index >= 0) ClearMember(index);
        }
        /// <summary>
        /// 设置成员索引,忽略默认成员
        /// </summary>
        /// <param name="memberName">成员名称</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool SetMember(string memberName)
        {
            int index = Type.GetMemberIndex(memberName);
            if (index >= 0)
            {
                SetMember(index);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        internal void BinarySerialize(UnmanagedStream stream)
        {
            if (map == null) stream.Write(0);
            else
            {
                byte* data = stream.GetBeforeMove(Type.BinarySerializeSize + sizeof(int)), read = map;
                *(int*)data = Type.FieldCount;
                data += sizeof(int);
                for (byte* end = map + (Type.BinarySerializeSize & (int.MaxValue - sizeof(ulong) + 1)); read != end; read += sizeof(ulong), data += sizeof(ulong)) *(ulong*)data = *(ulong*)read;
                if ((Type.BinarySerializeSize & sizeof(int)) != 0) *(uint*)data = *(uint*)read;
            }
        }
        /// <summary>
        /// 获取字段成员反序列化位图
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal byte* GetFieldDeSerialize()
        {
            if (map == null)
            {
                if (Type.MemberMapSize == 0) return null;
                map = Type.GetMap();
            }
            return map;
        }
        /// <summary>
        /// 比较是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected bool equals(MemberMap other)
        {
            return equals(other.map);
        }
        /// <summary>
        /// 比较是否相等
        /// </summary>
        /// <param name="otherMap"></param>
        /// <returns></returns>
        private bool equals(byte* otherMap)
        {
            if (map == null) return otherMap == null;
            if (otherMap == null) return false;
            if (Type.MemberMapSize == 0) return true;
            byte* write = map, end = map + Type.MemberMapSize, read = otherMap;
            ulong bits = *(ulong*)write ^ *(ulong*)read;
            while ((write += sizeof(ulong)) != end) bits |= *(ulong*)write ^ *(ulong*)(read += sizeof(ulong));
            return bits == 0;
        }
#if !DOTNET2
        /// <summary>
        /// 获取成员名称
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        internal static string GetMemberName(LambdaExpression memberExpression)
        {
            Expression expression = memberExpression.Body;
            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                MemberInfo member = ((MemberExpression)expression).Member;
                FieldInfo field = member as FieldInfo;
                if (field != null) return field.Name;
                PropertyInfo property = member as PropertyInfo;
                if (property != null) return property.Name;
            }
            return null;
        }
#endif
    }
    /// <summary>
    /// 成员位图
    /// </summary>
    public unsafe sealed partial class MemberMap<T> : IEquatable<MemberMap<T>>
    {
#if !DOTNET2
        /// <summary>
        /// 清除成员索引,忽略默认成员
        /// </summary>
        /// <typeparam name="returnType"></typeparam>
        /// <param name="member">成员</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void ClearMember<returnType>(Expression<Func<T, returnType>> member)
        {
            ClearMember(GetMemberName(member));
        }
        /// <summary>
        /// 设置成员索引,忽略默认成员
        /// </summary>
        /// <typeparam name="returnType"></typeparam>
        /// <param name="member">成员</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool SetMember<returnType>(Expression<Func<T, returnType>> member)
        {
            return SetMember(GetMemberName(member));
        }
#endif
        /// <summary>
        /// 比较是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Equals(MemberMap<T> other)
        {
            return equals(other);
        }

        /// <summary>
        /// 所有成员位图
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static MemberMap<T> NewFull()
        {
            MemberMap<T> value = new MemberMap<T>();
            value.Full();
            return value;
        }
        /// <summary>
        /// 空成员位图
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static MemberMap<T> NewEmpty()
        {
            MemberMap<T> value = new MemberMap<T>();
            value.Empty();
            return value;
        }
    }
}
