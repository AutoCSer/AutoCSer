using System;
using System.Linq.Expressions;
using AutoCSer.Extension;
using System.Data.Common;

namespace AutoCSer.Sql.Cache.Whole.Event
{
    /// <summary>
    /// 自增ID整表排序树缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="memberCacheType">成员缓存类型</typeparam>
    public sealed unsafe class IdentityTree<valueType, modelType, memberCacheType> : IdentityCache<valueType, modelType, memberCacheType>
        where valueType : class, modelType
        where modelType : class
        where memberCacheType : class
    {
        /// <summary>
        /// 排序树节点数量集合
        /// </summary>
        private Pointer.Size counts;
        /// <summary>
        /// 排序树容器数量
        /// </summary>
        private int size;
        /// <summary>
        /// 自增ID整表数组缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberCache">成员缓存</param>
        /// <param name="group">数据分组</param>
        /// <param name="baseIdentity">基础ID</param>
        public IdentityTree(Sql.Table<valueType, modelType> sqlTool, Expression<Func<valueType, memberCacheType>> memberCache, int group = 0, int baseIdentity = 0)
            : base(sqlTool, memberCache, group, baseIdentity, true)
        {
            sqlTool.OnInserted += onInserted;
            sqlTool.OnDeleted += onDelete;

            reset(null);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            Unmanaged.Free(ref counts);
            size = 0;
        }
        /// <summary>
        /// 重新加载数据
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="query">查询信息</param>
        internal override void Reset(ref DbConnection connection, ref SelectQuery<modelType> query)
        {
            LeftArray<valueType> array = SqlTable.Select(ref connection, ref query);
            int maxIdentity = array.maxKey(value => GetKey(value), 0);
            if (memberGroup == 0) SqlTable.Identity64 = maxIdentity + baseIdentity;
            int length = maxIdentity >= IdentityArray.ArraySize ? 1 << ((uint)maxIdentity).bits() : IdentityArray.ArraySize;
            IdentityArray<valueType> newValues = new IdentityArray<valueType>(length);
            Pointer.Size newCounts = Unmanaged.GetSize64(length * sizeof(int), true);
            try
            {
                int* intCounts = newCounts.Int;
                foreach (valueType value in array)
                {
                    setMemberCacheAndValue(value);
                    int identity = GetKey(value);
                    newValues[identity] = value;
                    intCounts[identity] = 1;
                }
                for (int step = 2; step != length; step <<= 1)
                {
                    for (int index = step, countStep = step >> 1; index != length; index += step)
                    {
                        intCounts[index] += intCounts[index - countStep];
                    }
                }
                Unmanaged.Free(ref counts);
                this.Array = newValues;
                counts = newCounts;
                size = length;
                Count = array.Length;
                newCounts.Null();
            }
            catch (Exception error)
            {
                SqlTable.Log.add(AutoCSer.Log.LogType.Error, error);
            }
            finally { Unmanaged.Free(ref newCounts); }
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        private void onInserted(valueType value)
        {
            int* intCounts = counts.Int;
            int identity = GetKey(value);
            if (identity >= size)
            {
                int newLength = int.MaxValue - 1, oldLength = size;
                if ((identity & 0x40000000) == 0 && oldLength != 0x40000000)
                {
                    for (newLength = oldLength << 1; newLength <= identity; newLength <<= 1) ;
                }
                Array.ToSize(newLength);
                Pointer.Size newCounts = Unmanaged.GetSize64(newLength * sizeof(int), true);
                try
                {
                    Memory.CopyNotNull(intCounts, newCounts.Int, size * sizeof(int));
                    Unmanaged.Free(ref counts);
                    counts = newCounts;
                    size = newLength;
                    newCounts.Null();

                    int index = oldLength, count = (intCounts = counts.Int)[--index];
                    for (int step = 1; (index -= step) != 0; step <<= 1) count += intCounts[index];
                    intCounts[oldLength] = count;
                }
                catch (Exception error)
                {
                    SqlTable.Log.add(AutoCSer.Log.LogType.Error, error);
                }
                finally { Unmanaged.Free(ref newCounts); }
            }
            valueType newValue = AutoCSer.Emit.Constructor<valueType>.New();
            AutoCSer.MemberCopy.Copyer<modelType>.Copy(newValue, value, MemberMap);
            setMemberCacheAndValue(newValue);
            Array[identity] = newValue;
            for (uint index = (uint)identity, countStep = 1, length = (uint)size; index <= length; countStep <<= 1)
            {
                ++intCounts[index];
                while ((index & countStep) == 0) countStep <<= 1;
                index += countStep;
            }
            ++Count;
            callOnInserted(newValue);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        private void onDelete(valueType value)
        {
            int* intCounts = counts.Int;
            int identity = GetKey(value);
            valueType cacheValue = Array[identity];
            --Count;
            for (uint index = (uint)identity, countStep = 1, length = (uint)size; index <= length; countStep <<= 1)
            {
                --intCounts[index];
                while ((index & countStep) == 0) countStep <<= 1;
                index += countStep;
            }
            Array[identity] = null;
            callOnDeleted(cacheValue);
        }
    }
}
