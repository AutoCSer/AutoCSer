using System;
using System.Reflection;
using AutoCSer.Threading;

namespace AutoCSer.Expand.Metadata
{
    /// <summary>
    /// 引用泛型类型元数据
    /// </summary>
    internal abstract partial class ClassGenericType : AutoCSer.Metadata.GenericTypeBase
    {
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        internal abstract Delegate NumberToCharStreamClassJoinCharDelegate { get; }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        internal abstract Delegate NumberToCharStreamClassSubArrayJoinCharDelegate { get; }

        /// <summary>
        /// 泛型类型元数据缓存
        /// </summary>
        private static readonly AutoCSer.Threading.LockLastDictionary<HashType, ClassGenericType> cache = new LockLastDictionary<HashType, ClassGenericType>(getCurrentType);
        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static ClassGenericType create<T>() where T : class
        {
            return new ClassGenericType<T>();
        }
        /// <summary>
        /// 创建泛型类型元数据 函数信息
        /// </summary>
        private static readonly MethodInfo createMethod = typeof(ClassGenericType).GetMethod("create", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ClassGenericType Get(HashType type)
        {
            ClassGenericType value;
            if (!cache.TryGetValue(type, out value))
            {
                try
                {
                    value = new UnionType.ClassGenericType { Object = createMethod.MakeGenericMethod(type).Invoke(null, null) }.Value;
                    cache.Set(type, value);
                }
                finally { cache.Exit(); }
            }
            return value;
        }
    }
    /// <summary>
    /// 结构体泛型类型元数据
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    internal sealed partial class ClassGenericType<T> : ClassGenericType where T : class
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 连接字符串集合
        /// </summary>
        internal override Delegate NumberToCharStreamClassJoinCharDelegate
        {
            get { return (Func<T[], char, string, string>)AutoCSer.NumberToCharStream.JoinMethod.classJoinChar<T>; }
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        internal override Delegate NumberToCharStreamClassSubArrayJoinCharDelegate
        {
            get { return (Func<SubArray<T>, char, string, string>)AutoCSer.NumberToCharStream.JoinMethod.classSubArrayJoinChar<T>; }
        }
    }
}

