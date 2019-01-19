using System;

namespace AutoCSer.WebView
{
    /// <summary>
    /// WEB 视图配置
    /// </summary>
    public sealed partial class ViewAttribute : Attribute
    {
        /// <summary>
        /// WEB 视图调用类型名称，用于替换默认的类型名称。
        /// </summary>
        public string TypeCallName;
        /// <summary>
        /// WEB 调用函数名称，用于替换默认的函数名称。
        /// </summary>
        public string MethodName;
        /// <summary>
        /// 如果存在查询参数，则会生成一个 struct 用于整合这些参数，同时生成一个这个类型的字段，这个字段名称默认为 query。
        /// </summary>
        public string QueryName = "query";
        /// <summary>
        /// HTTP Body 接收数据的最大字节数，默认为 4MB。
        /// </summary>
        public int MaxPostDataSize = 4 << 20;
        /// <summary>
        /// 接收 HTTP Body 数据内存缓冲区的最大字节数，默认为 128KB，超出限定则使用文件储存方式。
        /// </summary>
        public SubBuffer.Size MaxMemoryStreamSize = SubBuffer.Size.Kilobyte128;
        /// <summary>
        /// 来源路径重写
        /// </summary>
        public string RewritePath;
        /// <summary>
        /// 来源路径重写是否补全扩展名称
        /// </summary>
        public bool IsRewriteHtml = true;
        /// <summary>
        /// 默认为 true 表示支持服务端 HTML 输出，也就是说支持搜索引擎访问，否则对于搜索引擎返回 404。
        /// </summary>
        public bool IsPage = true;
        /// <summary>
        /// 默认为 true 表示支持服务端输出 WEB 视图 JSON 数据，也就是说支持浏览器的正常访问。
        /// </summary>
        public bool IsAjax = true;
        ///// <summary>
        ///// 默认为 false 表示每次请求都需要 new 一个页面对象；否则在页面对象使用完以后会添加到 WEB 页面对象池中以便重复使用，记得重载 void clear() 处理需要清除的字段数据，或者给需要处理的字段数据添加申明[AutoCSer.WebView.ClearMember]。
        ///// </summary>
        //public bool IsPool;
        /// <summary>
        /// 默认为 false 表示不生成 TypeScript 调用代理，一般只有在嵌入式页面中才需要设置为 true，只有 IsAjax = true 时有效。
        /// </summary>
        public bool IsExportTypeScript;
        /// <summary>
        /// 是否异步请求
        /// </summary>
        public bool IsAsynchronous;
        /// <summary>
        /// 默认为 true 表示在初始化的时候启动 JSON 反序列化预编译任务
        /// </summary>
        public bool IsCompileJsonParser = true;
    }
}
