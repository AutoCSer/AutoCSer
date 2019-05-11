using System;
using System.Reflection;
using AutoCSer.Threading;

namespace AutoCSer.CacheServer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class GenericType
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
        private static readonly AutoCSer.Threading.LockLastDictionary<Type, GenericType> cache = new LockLastDictionary<Type, GenericType>();
        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static GenericType create<Type>()
        {
            return new GenericType<Type>();
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
        public static GenericType Get(Type type)
        {
            GenericType value;
            if (!cache.TryGetValue(type, out value))
            {
                try
                {
                    value = new UnionType { Value = createMethod.MakeGenericMethod(type).Invoke(null, null) }.GenericType;
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
    /// <typeparam name="Type">泛型类型</typeparam>
    internal sealed class GenericType<Type> : GenericType
    {
        /// <summary>
        /// 获取参数数据
        /// </summary>
        internal override Delegate ValueDataGetJsonDelegate
        {
            get { return (AutoCSer.CacheServer.ValueData.GetData<DataStructure.Value.Json<Type>>)AutoCSer.CacheServer.ValueData.Data<Type>.getJson; }
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        internal override Delegate ValueDataGetBinaryDelegate
        {
            get { return (AutoCSer.CacheServer.ValueData.GetData<DataStructure.Value.Binary<Type>>)AutoCSer.CacheServer.ValueData.Data<Type>.getBinary; }
        }
        /// <summary>
        /// 返回数组反序列化
        /// </summary>
        internal override Delegate ReturnArrayDeSerializeJsonDelegate
        {
            get { return (Func<SubArray<byte>, ReturnValue<DataStructure.Value.Json<Type>[]>>)AutoCSer.CacheServer.ReturnArray<Type>.deSerializeJson; }
        }
        /// <summary>
        /// 返回数组反序列化
        /// </summary>
        internal override Delegate ReturnArrayDeSerializeBinaryDelegate
        {
            get { return (Func<SubArray<byte>, ReturnValue<DataStructure.Value.Binary<Type>[]>>)AutoCSer.CacheServer.ReturnArray<Type>.deSerializeBinary; }
        }
    }
}
