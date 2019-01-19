using System;
using AutoCSer.Metadata;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 调用函数配置
    /// </summary>
    public abstract class MethodBaseAttribute : IgnoreMemberAttribute
    {
        /// <summary>
        /// 自定义命令序号，不能重复，默认小于 0  表示不指定。存在自定义需求时不要使用巨大的数据，建议从 0 开始，因为它会是某个数组的大小。
        /// </summary>
        public int CommandIdentity = int.MinValue;
        /// <summary>
        /// 申明验证方法，客户端只有通过了验证才能调用其它函数。一个 TCP 服务只能指定一个验证方法（对于跨类型单例服务只能定义在 AutoCSer.Net.TcpStaticServer.ServerAttribute.IsServer = true 的 class 中），且返回值类型必须为 bool。从安全的角度考虑，实际项目中的服务都应该定义验证方法，除非你能保证该服务绝对不会被其它人建立非法连接。比如参考 AutoCSer.net.tcp.timeVerifyServer。
        /// </summary>
        public bool IsVerifyMethod;
        /// <summary>
        /// 服务端任务类型
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal abstract ServerTaskType ServerTaskType { get; set; }
        /// <summary>
        /// 客户端异步任务类型
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal abstract ClientTaskType ClientTaskType { get; }
        /// <summary>
        /// 默认为 true 表示与服务配置使用相同的序列化方式
        /// </summary>
        public bool IsServerSerialize = true;
        /// <summary>
        /// 是否使用 JSON 序列化
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal bool IsJsonSerialize;
        /// <summary>
        /// 是否生成同步调用代理函数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal virtual bool GetIsClientSynchronous { get { return true; } }
        /// <summary>
        /// 是否生成异步调用代理函数。
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal virtual bool GetIsClientAsynchronous
        {
            get { return false; }
        }
        /// <summary>
        /// 保持异步回调
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal virtual bool GetIsKeepCallback
        {
            get { return false; }
        }
        /// <summary>
        /// 参数设置默认为 InputSerializeReferenceMember | ParameterFlags.OutputSerializeReferenceMember | InputSerializeBox | OutputSerializeBox
        /// </summary>
        public ParameterFlags ParameterFlags = ParameterFlags.Default;
        /// <summary>
        /// 参数设置
        /// </summary>
        internal virtual ParameterFlags GetParameterFlags
        {
            get { return ParameterFlags; }
        }
        /// <summary>
        /// 默认为 true 表示输入参数二进制序列化需要检测循环引用，如果可以保证参数没有循环引用而且对象无需重用则应该设置为 false 减少 CPU 开销。
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal bool IsInputSerializeReferenceMember
        {
            get { return (GetParameterFlags & ParameterFlags.InputSerializeReferenceMember) != 0; }
        }
        /// <summary>
        /// 默认为 true 表示输出参数（包括 ref / out）二进制序列化需要检测循环引用，如果可以保证参数没有循环引用而且对象无需重用则应该设置为 false 减少 CPU 开销。
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal bool IsOutputSerializeReferenceMember
        {
            get { return (GetParameterFlags & ParameterFlags.OutputSerializeReferenceMember) != 0; }
        }
        /// <summary>
        /// 输入参数是否添加包装处理申明 AutoCSer.emit.boxSerialize，用于只有一个输入参数的类型忽略外壳类型的处理以减少序列化开销。
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal bool IsInputSerializeBox
        {
            get { return (GetParameterFlags & ParameterFlags.InputSerializeBox) != 0; }
        }
        /// <summary>
        /// 输出参数是否添加包装处理申明 AutoCSer.emit.boxSerialize，用于只有一个输出参数的类型忽略外壳类型的处理以减少序列化开销。
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal bool IsOutputSerializeBox
        {
            get { return (GetParameterFlags & ParameterFlags.OutputSerializeBox) != 0; }
        }
        /// <summary>
        /// 客户端是否仅发送数据，无需服务端应答
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal virtual bool GetIsClientSendOnly { get { return false; } }
        /// <summary>
        /// 是否支持 async Task
        /// </summary>
        internal virtual bool GetIsClientTaskAsync { get { return false; } }
        /// <summary>
        /// 是否支持 async Task
        /// </summary>
        internal virtual bool GetIsClientAwaiter { get { return false; } }
        /// <summary>
        /// 默认为 true 表示对于 属性 / 字段 仅仅生成获取数据的代理，否则生成设置数据的代理（如果属性可写）。
        /// </summary>
        public bool IsOnlyGetMember = true;
        /// <summary>
        /// 是否过期
        /// </summary>
        public bool IsExpired;
#if !NOJIT
        /// <summary>
        /// 默认为 false 表示可能不会对输入输出参数进行初始化操作从而导致错误时出现随机数据，仅 Emit 模式有效
        /// </summary>
        public bool IsInitobj;
#endif
    }
}
