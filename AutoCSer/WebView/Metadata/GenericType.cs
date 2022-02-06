using System;
using System.Reflection;
using AutoCSer.Threading;

namespace AutoCSer.WebView.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class GenericType : AutoCSer.Metadata.GenericTypeBase
    {
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

#if !NOJIT
        /// <summary>
        /// WEB视图成员清理
        /// </summary>
        internal override MethodInfo WebViewClearMemberMethod
        {
            get { return ((Action<T>)AutoCSer.WebView.ClearMember.Clear<T>).Method; }
        }
#endif
        /// <summary>
        /// 查询类型解析器预编译
        /// </summary>
        internal override void HeaderQueryTypeParserCompile()
        {
            AutoCSer.Net.Http.HeaderQueryTypeParser<T>.Compile();
        }
        /// <summary>
        /// 反序列化委托
        /// </summary>
        /// <param name="parser">目标数据</param>
        /// <param name="value">目标数据</param>
        private delegate void headerQueryParse(AutoCSer.Net.Http.HeaderQueryParser parser, ref T value);
        /// <summary>
        /// 枚举类型解析
        /// </summary>
        internal override MethodInfo HttpHeaderQueryParseEnumMethod
        {
            get { return ((headerQueryParse)AutoCSer.Net.Http.HeaderQueryParser.ParseEnum<T>).Method; }
        }
        /// <summary>
        /// 未知类型解析
        /// </summary>
        internal override MethodInfo HttpHeaderQueryParseUnknownMethod
        {
            get { return ((headerQueryParse)AutoCSer.Net.Http.HeaderQueryParser.Unknown<T>).Method; }
        }
    }
}
