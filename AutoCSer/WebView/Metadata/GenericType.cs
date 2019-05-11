using System;
using System.Reflection;
using AutoCSer.Threading;

namespace AutoCSer.WebView.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class GenericType
    {
        /// <summary>
        /// HTTP 查询解析器 实例
        /// </summary>
        internal static readonly AutoCSer.Net.Http.HeaderQueryParser HttpHeaderQueryParser = new AutoCSer.Net.Http.HeaderQueryParser();

#if !NOJIT
        /// <summary>
        /// WEB视图成员清理
        /// </summary>
        internal abstract MethodInfo WebViewClearMemberMethod { get; }
#endif
        /// <summary>
        /// 查询类型解析器预编译
        /// </summary>
        internal abstract void HeaderQueryTypeParserCompile();
        /// <summary>
        /// 枚举类型解析
        /// </summary>
        internal abstract MethodInfo HttpHeaderQueryParseEnumMethod { get; }
        /// <summary>
        /// 未知类型解析
        /// </summary>
        internal abstract MethodInfo HttpHeaderQueryParseUnknownMethod { get; }

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
#if !NOJIT
        /// <summary>
        /// WEB视图成员清理
        /// </summary>
        internal override MethodInfo WebViewClearMemberMethod
        {
            get { return ((Action<Type>)AutoCSer.WebView.ClearMember.clear<Type>).Method; }
        }
#endif
        /// <summary>
        /// 查询类型解析器预编译
        /// </summary>
        internal override void HeaderQueryTypeParserCompile()
        {
            AutoCSer.Net.Http.HeaderQueryTypeParser<Type>.Compile();
        }
        /// <summary>
        /// 枚举类型解析
        /// </summary>
        internal override MethodInfo HttpHeaderQueryParseEnumMethod
        {
            get { return ((AutoCSer.Metadata.GenericType<Type>.deSerialize)GenericType.HttpHeaderQueryParser.parseEnum<Type>).Method; }
        }
        /// <summary>
        /// 未知类型解析
        /// </summary>
        internal override MethodInfo HttpHeaderQueryParseUnknownMethod
        {
            get { return ((AutoCSer.Metadata.GenericType<Type>.deSerialize)GenericType.HttpHeaderQueryParser.unknown<Type>).Method; }
        }
    }
}
