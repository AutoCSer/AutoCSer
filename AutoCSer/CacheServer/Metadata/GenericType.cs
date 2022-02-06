using System;
using System.Reflection;
using AutoCSer.Threading;

namespace AutoCSer.CacheServer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class GenericType : AutoCSer.Metadata.GenericTypeBase
    {
        /// <summary>
        /// 获取参数数据
        /// </summary>
        internal abstract Delegate ValueDataGetJsonDelegate { get; }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        internal abstract Delegate ValueDataGetBinaryDelegate { get; }
        /// <summary>
        /// 返回数组反序列化
        /// </summary>
        internal abstract Delegate ReturnArrayDeSerializeJsonDelegate { get; }
        /// <summary>
        /// 返回数组反序列化
        /// </summary>
        internal abstract Delegate ReturnArrayDeSerializeBinaryDelegate { get; }

        /// <summary>
        /// 泛型类型元数据缓存
        /// </summary>
        private static readonly AutoCSer.Threading.LockLastDictionary<HashType, GenericType> cache = new LockLastDictionary<HashType, GenericType>(getCurrentType);
        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static GenericType create<T>()
        {
            return new GenericType<T>();
        }
        /// <summary>
        /// 创建泛型类型元数据 函数信息
        /// </summary>
        private static readonly MethodInfo createMethod = typeof(GenericType).GetMethod("create", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static GenericType Get(HashType type)
        {
            GenericType value;
            if (!cache.TryGetValue(type, out value))
            {
                try
                {
                    value = new UnionType.GenericType { Object = createMethod.MakeGenericMethod(type).Invoke(null, null) }.Value;
                    cache.Set(type, value);
                }
                finally { cache.Exit(); }
            }
            return value;
        }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    internal sealed class GenericType<T> : GenericType
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 获取参数数据
        /// </summary>
        internal override Delegate ValueDataGetJsonDelegate
        {
            get { return (AutoCSer.CacheServer.ValueData.GetData<DataStructure.Value.Json<T>>)AutoCSer.CacheServer.ValueData.Data<T>.getJson; }
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        internal override Delegate ValueDataGetBinaryDelegate
        {
            get { return (AutoCSer.CacheServer.ValueData.GetData<DataStructure.Value.Binary<T>>)AutoCSer.CacheServer.ValueData.Data<T>.getBinary; }
        }
        /// <summary>
        /// 返回数组反序列化
        /// </summary>
        internal override Delegate ReturnArrayDeSerializeJsonDelegate
        {
            get { return (Func<SubArray<byte>, ReturnValue<DataStructure.Value.Json<T>[]>>)AutoCSer.CacheServer.ReturnArray<T>.deSerializeJson; }
        }
        /// <summary>
        /// 返回数组反序列化
        /// </summary>
        internal override Delegate ReturnArrayDeSerializeBinaryDelegate
        {
            get { return (Func<SubArray<byte>, ReturnValue<DataStructure.Value.Binary<T>[]>>)AutoCSer.CacheServer.ReturnArray<T>.deSerializeBinary; }
        }
    }
}
