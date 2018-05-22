using System;
using System.Linq.Expressions;
using AutoCSer.Metadata;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Cache.Counter.Event
{
    /// <summary>
    /// 缓存计数器
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="memberCacheType">成员缓存类型</typeparam>
    /// <typeparam name="keyType"></typeparam>
    /// <typeparam name="targetType"></typeparam>
    public abstract class Member<valueType, modelType, memberCacheType, keyType, targetType> : Copy<valueType, modelType>
        where valueType : class, modelType
        where modelType : class
        where memberCacheType : class
        where keyType : struct, IEquatable<keyType>
        where targetType : class
    {
        /// <summary>
        /// 分组字典关键字获取器
        /// </summary>
        internal readonly Func<modelType, keyType> GetKey;
        /// <summary>
        /// 获取缓存目标对象
        /// </summary>
        internal readonly Func<keyType, targetType> GetByKey;
        /// <summary>
        /// 获取缓存目标对象
        /// </summary>
        internal readonly Func<valueType, targetType> GetValue;
        /// <summary>
        /// 获取缓存委托
        /// </summary>
        internal readonly Func<targetType, KeyValue<valueType, int>> GetMember;
        /// <summary>
        /// 设置缓存委托
        /// </summary>
        private readonly Action<targetType, KeyValue<valueType, int>> setMember;
        /// <summary>
        /// 缓存数据数量
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>缓存值</returns>
        public valueType this[keyType key]
        {
            get
            {
                return Get(key);
            }
        }
        /// <summary>
        /// 缓存计数
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="group">数据分组</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="getByKey"></param>
        /// <param name="getValue"></param>
        /// <param name="member"></param>
        protected Member(Whole.Event.Cache<valueType, modelType, memberCacheType> cache, int group, Func<modelType, keyType> getKey
            , Func<keyType, targetType> getByKey, Func<valueType, targetType> getValue, Expression<Func<targetType, KeyValue<valueType, int>>> member)
            : base(cache.SqlTable, group)
        {
            if (getKey == null) throw new ArgumentNullException("getKey is null");
            if (getByKey == null) throw new ArgumentNullException("getByKey is null");
            if (getValue == null) throw new ArgumentNullException("getValue is null");
            if (member == null) throw new ArgumentNullException("member is null");
            MemberExpression<targetType, KeyValue<valueType, int>> expression = new MemberExpression<targetType, KeyValue<valueType, int>>(member);
            if (expression.Field == null) throw new InvalidCastException("member is not MemberExpression");
            GetKey = getKey;
            GetByKey = getByKey;
            GetValue = getValue;
            GetMember = expression.GetMember;
            setMember = expression.SetMember;

            SqlTable.OnUpdated += onUpdated;
            cache.OnDeleted += onDeleted;
        }
        /// <summary>
        /// 更新记录事件 [缓存数据 + 更新后的数据 + 更新前的数据 + 更新数据成员]
        /// </summary>
        public event OnCacheUpdated OnUpdated;
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap">更新成员位图</param>
        private void onUpdated(valueType value, valueType oldValue, MemberMap<modelType> memberMap)
        {
            targetType memberCache = GetByKey(GetKey(value));
            KeyValue<valueType, int> cache = GetMember(memberCache);
            if (cache.Key != null) update(cache.Key, value, oldValue, memberMap);
            if (OnUpdated != null) OnUpdated(cache.Key, value, oldValue, memberMap);
        }
        /// <summary>
        /// 删除记录事件
        /// </summary>
        public event Action<valueType> OnDeleted;
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        private void onDeleted(valueType value)
        {
            targetType cache = GetValue(value);
            KeyValue<valueType, int> cacheValue = GetMember(cache);
            if (cacheValue.Key != null)
            {
                if (OnDeleted != null) OnDeleted(value);
                setMember(cache, new KeyValue<valueType, int>(null, cacheValue.Value));
                --Count;
            }
        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>缓存数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal valueType Get(keyType key)
        {
            targetType value = GetByKey(key);
            return value == null ? null : GetMember(value).Key;
        }
        /// <summary>
        /// 添加缓存数据
        /// </summary>
        /// <param name="value">缓存数据</param>
        /// <returns>缓存数据</returns>
        internal targetType AddGetTarget(valueType value)
        {
            targetType cache = GetByKey(GetKey(value));
            KeyValue<valueType, int> valueCount = GetMember(cache);
            if (valueCount.Key != null)
            {
                ++valueCount.Value;
                setMember(cache, valueCount);
            }
            else
            {
                valueType copyValue = AutoCSer.Emit.Constructor<valueType>.New();
                AutoCSer.MemberCopy.Copyer<modelType>.Copy(copyValue, value, MemberMap);
                setMember(cache, new KeyValue<valueType, int>(copyValue, 0));
                ++Count;
            }
            return cache;
        }
        /// <summary>
        /// 删除缓存数据
        /// </summary>
        /// <param name="cache">缓存数据</param>
        internal void Remove(targetType cache)
        {
            KeyValue<valueType, int> valueCount = GetMember(cache);
            if (valueCount.Key != null)
            {
                if (valueCount.Value == 0)
                {
                    setMember(cache, new KeyValue<valueType, int>());
                    --Count;
                }
                else
                {
                    --valueCount.Value;
                    setMember(cache, valueCount);
                }
            }
            else SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
        }
    }
}
