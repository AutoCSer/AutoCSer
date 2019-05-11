using System;
using System.Reflection;
using AutoCSer.Threading;

namespace AutoCSer.Expand.Metadata
{
    /// <summary>
    /// 引用泛型类型元数据
    /// </summary>
    internal abstract partial class ClassGenericType
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
        private static readonly AutoCSer.Threading.LockLastDictionary<Type, ClassGenericType> cache = new LockLastDictionary<Type, ClassGenericType>();
        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static ClassGenericType create<Type>() where Type : class
        {
            return new ClassGenericType<Type>();
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
        public static ClassGenericType Get(Type type)
        {
            ClassGenericType value;
            if (!cache.TryGetValue(type, out value))
            {
                try
                {
                    value = new UnionType { Value = createMethod.MakeGenericMethod(type).Invoke(null, null) }.ClassGenericType;
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
    /// <typeparam name="Type">泛型类型</typeparam>
    internal sealed partial class ClassGenericType<Type> : ClassGenericType where Type : class
    {
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        internal override Delegate NumberToCharStreamClassJoinCharDelegate
        {
            get { return (Func<Type[], char, string, string>)AutoCSer.NumberToCharStream.JoinMethod.classJoinChar<Type>; }
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        internal override Delegate NumberToCharStreamClassSubArrayJoinCharDelegate
        {
            get { return (Func<SubArray<Type>, char, string, string>)AutoCSer.NumberToCharStream.JoinMethod.classSubArrayJoinChar<Type>; }
        }
    }
}

