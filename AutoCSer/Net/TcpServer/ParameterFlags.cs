using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 参数设置
    /// </summary>
    [Flags]
    public enum ParameterFlags : byte
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 默认为 true 表示输入参数二进制序列化需要检测循环引用，如果可以保证参数没有循环引用而且对象无需重用则应该设置为 false 减少 CPU 开销。
        /// </summary>
        InputSerializeReferenceMember = 1,
        /// <summary>
        /// 默认为 true 表示输出参数（包括 ref / out）二进制序列化需要检测循环引用，如果可以保证参数没有循环引用而且对象无需重用则应该设置为 false 减少 CPU 开销。
        /// </summary>
        OutputSerializeReferenceMember = 2,
        /// <summary>
        /// 输入参数是否添加包装处理申明 AutoCSer.emit.boxSerialize，用于只有一个输入参数的类型忽略外壳类型的处理以减少序列化开销。
        /// </summary>
        InputSerializeBox = 4,
        /// <summary>
        /// 输出参数是否添加包装处理申明 AutoCSer.emit.boxSerialize，用于只有一个输出参数的类型忽略外壳类型的处理以减少序列化开销。
        /// </summary>
        OutputSerializeBox = 8,
        /// <summary>
        /// InputSerializeBox | OutputSerializeBox
        /// </summary>
        SerializeBox = InputSerializeBox | OutputSerializeBox,
        /// <summary>
        /// InputSerializeReferenceMember | OutputSerializeReferenceMember | SerializeBox
        /// </summary>
        Default = InputSerializeReferenceMember | OutputSerializeReferenceMember | SerializeBox,
        /// <summary>
        /// 客户端异步回调的返回值是否和第一个相同类型的输入参数公用同一个对象，类似于 ref 的作用。
        /// </summary>
        ClientAsynchronousReturnInput = 0x10
        ///// <summary>
        ///// 默认为 false 表示对输入参数生成 struct 以减少 new 开销，但是会增加参数赋值的开销，否则使用 class 包装输入参数。
        ///// </summary>
        //InputClass = 0x40,
        ///// <summary>
        ///// 默认为 false 表示对输出参数生成 struct 以减少 new 开销，但是会增加参数赋值的开销，否则使用 class 包装输出参数。
        ///// </summary>
        //OutputClass = 0x80,
    }
}
