using System;

namespace AutoCSer.WebView
{
    /// <summary>
    /// WEB 调用函数配置
    /// </summary>
    public sealed partial class CallMethodAttribute : MethodAttribute
    {
        /// <summary>
        /// 默认为 false 表示支持 GET 请求，否则仅支持 POST 请求。
        /// </summary>
        public bool IsOnlyPost;
        ///// <summary>
        ///// 正常情况下输入参数会包装成一个 struct，设置为 true 表示只有一个输入参数时序列化操作忽略外壳类型的处理。
        ///// </summary>
        //public bool IsSerializeBox;
        /// <summary>
        /// 是否仅仅序列化第一个参数，设置为 true 表示忽略生成的包装 struct
        /// </summary>
        public bool IsFirstParameter;
        ///// <summary>
        ///// 是否异步请求
        ///// </summary>
        //public bool IsAsynchronous;
        /// <summary>
        /// 默认为 true 表示在初始化的时候启动 Query 反序列化预编译任务
        /// </summary>
        public bool IsCompileQueryParser = true;
    }
}
