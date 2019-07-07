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
    public unsafe abstract partial class MemberMap : IDisposable
    {
        /// <summary>
        /// 成员位图类型信息
        /// </summary>
        internal sealed class TypeInfo
        {
            /// <summary>
            /// 类型
            /// </summary>
            internal readonly Type Type;
            /// <summary>
            /// 成员位图内存池
            /// </summary>
            internal readonly Pool Pool;
            /// <summary>
            /// 名称索引查找数据
            /// </summary>
            internal readonly StateSearcher.CharSearcher NameIndexSearcher;
            /// <summary>
            /// 成员数量
            /// </summary>
            public int MemberCount { get; private set; }
            /// <summary>
            /// 字段成员数量
            /// </summary>
            public int FieldCount { get; private set; }
            /// <summary>
            /// 成员位图字节数量
            /// </summary>
            public int MemberMapSize { get; private set; }
            /// <summary>
            /// 字段成员位图序列化字节数量
            /// </summary>
            public int BinarySerializeSize { get; private set; }
            /// <summary>
            /// 成员位图类型信息
            /// </summary>
            /// <param name="type">类型</param>
            /// <param name="members">成员索引集合</param>
            /// <param name="fieldCount">字段成员数量</param>
            public TypeInfo(Type type, AutoCSer.Metadata.MemberIndexInfo[] members, int fieldCount)
            {
                Type = type;
                MemberCount = members.Length;
                FieldCount = fieldCount;
                MemberMapSize = ((MemberCount + 63) >> 6) << 3;
                NameIndexSearcher = new StateSearcher.CharSearcher(AutoCSer.StateSearcher.CharBuilder.Create(members.getArray(value => value.Member.Name), true).Pointer);
                Pool = Pool.GetPool(MemberMapSize);
                BinarySerializeSize = ((fieldCount + 31) >> 5) << 2;
            }
            /// <summary>
            /// 获取成员索引
            /// </summary>
            /// <param name="name">成员名称</param>
            /// <returns>成员索引,失败返回-1</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public int GetMemberIndex(string name)
            {
                return name != null ? NameIndexSearcher.Search(name) : -1;
            }
            /// <summary>
            /// 获取成员位图
            /// </summary>
            /// <returns>成员位图</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public byte* GetClear()
            {
                return Pool == null ? (byte*)Unmanaged.Get64(MemberMapSize, true) : Pool.GetClear();
            }
            /// <summary>
            /// 获取成员位图
            /// </summary>
            /// <returns>成员位图</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public byte* GetMap()
            {
                return Pool == null ? (byte*)Unmanaged.Get64(MemberMapSize, false) : Pool.Get();
            }
            /// <summary>
            /// 成员位图入池
            /// </summary>
            /// <param name="map">成员位图</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void Push(ref byte* map)
            {
                if (Pool == null) Unmanaged.FreeStatic(ref map, MemberMapSize);
                else Pool.Push(ref map);
            }
        }
        /// <summary>
        /// 成员位图内存池
        /// </summary>
        internal sealed class Pool
        {
            /// <summary>
            /// 空闲内存地址
            /// </summary>
            private byte* free;
            /// <summary>
            /// 成员位图字节数量
            /// </summary>
            private int size;
            /// <summary>
            /// 空闲内存地址访问锁
            /// </summary>
            private int freeLock;
            /// <summary>
            /// 填充整数数量
            /// </summary>
            private int clearCount;
            /// <summary>
            /// 成员位图内存池
            /// </summary>
            /// <param name="size">成员位图字节数量</param>
            private Pool(int size)
            {
                this.size = size;
                clearCount = size >> 3;
            }
            /// <summary>
            /// 获取成员位图
            /// </summary>
            /// <returns>成员位图</returns>
            public byte* Get()
            {
                if (size == 0) return (byte*)Pub.PuzzleValue;
                byte* value;
                while (System.Threading.Interlocked.CompareExchange(ref freeLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.MemberMapPoolFreePop);
                if (free != null)
                {
                    value = free;
                    free = *(byte**)free;
                    System.Threading.Interlocked.Exchange(ref freeLock, 0);
                    return value;
                }
                System.Threading.Interlocked.Exchange(ref freeLock, 0);
                while (System.Threading.Interlocked.CompareExchange(ref memoryLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.MemberMapPoolMemory);
                value = memoryStart;
                if ((memoryStart += size) <= memoryEnd)
                {
                    System.Threading.Interlocked.Exchange(ref memoryLock, 0);
                    return value;
                }
                //memoryStart -= size;
                System.Threading.Interlocked.Exchange(ref memoryLock, 0);
                Monitor.Enter(createLock);
                while (System.Threading.Interlocked.CompareExchange(ref memoryLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.MemberMapPoolMemory);
                if ((memoryStart += size) <= memoryEnd)
                {
                    value = memoryStart - size;
                    System.Threading.Interlocked.Exchange(ref memoryLock, 0);
                    Monitor.Exit(createLock);
                    return value;
                }
                System.Threading.Interlocked.Exchange(ref memoryLock, 0);
                try
                {
                    value = (byte*)Unmanaged.GetStatic64(memorySize, false);
                    while (System.Threading.Interlocked.CompareExchange(ref memoryLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.MemberMapPoolMemory);
                    memoryStart = value + size;
                    memoryEnd = value + memorySize;
                    System.Threading.Interlocked.Exchange(ref memoryLock, 0);
                }
                finally { Monitor.Exit(createLock); }
                Interlocked.Increment(ref memoryCount);
                return value;
            }
            /// <summary>
            /// 获取成员位图
            /// </summary>
            /// <returns>成员位图</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public byte* GetClear()
            {
                if (size == 0) throw new IndexOutOfRangeException();
                byte* value = Get();
                Memory.ClearUnsafe((ulong*)value, clearCount);
                return value;
            }
            /// <summary>
            /// 成员位图入池
            /// </summary>
            /// <param name="map">成员位图</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void Push(ref byte* map)
            {
                if (map != null && size != 0)
                {
                    while (System.Threading.Interlocked.CompareExchange(ref freeLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.MemberMapPoolFreePush);
                    *(byte**)map = free;
                    free = map;
                    System.Threading.Interlocked.Exchange(ref freeLock, 0);
                    map = null;
                }
            }
            /// <summary>
            /// 获取成员位图内存池
            /// </summary>
            /// <param name="size">成员位图字节数量</param>
            /// <returns></returns>
            public static Pool GetPool(int size)
            {
                int index = size >> 3;
                if (index < pools.Length)
                {
                    Pool pool = pools[index];
                    if (pool != null) return pool;
                    Monitor.Enter(poolLock);
                    if ((pool = pools[index]) == null)
                    {
                        try
                        {
                            pools[index] = pool = new Pool(size);
                        }
                        finally { Monitor.Exit(poolLock); }
                    }
                    else Monitor.Exit(poolLock);
                    return pool;
                }
                return null;
            }

            /// <summary>
            /// 成员位图内存池字节大小
            /// </summary>
            private const int memorySize = 8 << 10;
            /// <summary>
            /// 成员位图内存池支持最大成员数量
            /// </summary>
            private const int maxMemberMapCount = 1 << 10;
            /// <summary>
            /// 成员位图内存池集合
            /// </summary>
            private static readonly Pool[] pools;
            /// <summary>
            /// 成员位图内存池集合访问锁
            /// </summary>
            private static readonly object poolLock = new object();
            /// <summary>
            /// 内存申请数量
            /// </summary>
            private static int memoryCount;
            /// <summary>
            /// 成员位图内存池起始位置
            /// </summary>
            private static byte* memoryStart;
            /// <summary>
            /// 成员位图内存池结束位置
            /// </summary>
            private static byte* memoryEnd;
            /// <summary>
            /// 成员位图内存池访问锁
            /// </summary>
            private static int memoryLock;
            /// <summary>
            /// 成员位图内存池访问锁
            /// </summary>
            private static readonly object createLock = new object();

            static Pool()
            {
                pools = new Pool[maxMemberMapCount >> 6];
            }
        }

        /// <summary>
        /// 成员位图
        /// </summary>
        protected byte* map;
        /// <summary>
        /// 成员位图类型信息
        /// </summary>
        internal TypeInfo Type;
        /// <summary>
        /// 是否默认全部成员有效
        /// </summary>
        public bool IsDefault { get { return map == null; } }
        /// <summary>
        /// 成员位图
        /// </summary>
        /// <param name="type">成员位图类型信息</param>
        internal MemberMap(TypeInfo type)
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
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Empty()
        {
            if (map == null)
            {
                if (Type.MemberMapSize == 0) return;
                map = Type.GetClear();
            }
            else Memory.ClearUnsafe((ulong*)map, Type.MemberMapSize >> 3);
        }
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
        /// 判断成员索引是否有效
        /// </summary>
        /// <param name="memberIndex">成员索引</param>
        /// <returns>成员索引是否有效</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal bool IsMember(int memberIndex)
        {
            return map == null || (map[memberIndex >> 3] & (1 << (memberIndex & 7))) != 0;
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
        /// <param name="memberIndex">成员索引</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
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
                stream.PrepLength(Type.BinarySerializeSize + sizeof(int));
                byte* data = stream.CurrentData, read = map;
                *(int*)data = Type.FieldCount;
                data += sizeof(int);
                for (byte* end = map + (Type.BinarySerializeSize & (int.MaxValue - sizeof(ulong) + 1)); read != end; read += sizeof(ulong), data += sizeof(ulong)) *(ulong*)data = *(ulong*)read;
                if ((Type.BinarySerializeSize & sizeof(int)) != 0) *(uint*)data = *(uint*)read;
                stream.ByteSize += Type.BinarySerializeSize + sizeof(int);
                //stream.PrepLength();
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
    /// <typeparam name="valueType">对象类型</typeparam>
    public unsafe sealed partial class MemberMap<valueType> : MemberMap, IEquatable<MemberMap<valueType>>
    {
        /// <summary>
        /// 成员位图类型信息
        /// </summary>
        internal new static readonly MemberMap.TypeInfo TypeInfo = new MemberMap.TypeInfo(typeof(valueType), MemberIndexGroup<valueType>.GetAllMembers(), MemberIndexGroup<valueType>.FieldCount);
        /// <summary>
        /// 成员位图
        /// </summary>
        internal MemberMap() : base(TypeInfo) { }
#if !DOTNET2
        /// <summary>
        /// 清除成员索引,忽略默认成员
        /// </summary>
        /// <typeparam name="returnType"></typeparam>
        /// <param name="member">成员</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void ClearMember<returnType>(Expression<Func<valueType, returnType>> member)
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
        public bool SetMember<returnType>(Expression<Func<valueType, returnType>> member)
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
        public bool Equals(MemberMap<valueType> other)
        {
            return equals(other);
        }

        /// <summary>
        /// 所有成员位图
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static MemberMap<valueType> NewFull()
        {
            MemberMap<valueType> value = new MemberMap<valueType>();
            value.Full();
            return value;
        }
        /// <summary>
        /// 空成员位图
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static MemberMap<valueType> NewEmpty()
        {
            MemberMap<valueType> value = new MemberMap<valueType>();
            value.Empty();
            return value;
        }
    }
}
