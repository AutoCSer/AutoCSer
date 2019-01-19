using System;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 解析状态
    /// </summary>
    public enum ParseState : byte
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 成员位图类型错误
        /// </summary>
        MemberMap,
        /// <summary>
        /// XML字符串参数为空
        /// </summary>
        NullXml,
        /// <summary>
        /// xml头部解析错误
        /// </summary>
        HeaderError,
        /// <summary>
        /// 没有找到根节点结束标签
        /// </summary>
        NotFoundBootNodeEnd,
        /// <summary>
        /// 没有找到根节点开始标签
        /// </summary>
        NotFoundBootNodeStart,
        /// <summary>
        /// 没有找到名称标签开始符号
        /// </summary>
        NotFoundTagStart,
        /// <summary>
        /// 没有找到匹配的结束标签
        /// </summary>
        NotFoundTagEnd,
        /// <summary>
        /// 属性名称解析失败
        /// </summary>
        NotFoundAttributeName,
        /// <summary>
        /// 属性值解析失败
        /// </summary>
        NotFoundAttributeValue,
        /// <summary>
        /// 没有找到预期数据
        /// </summary>
        NotFoundValue,
        /// <summary>
        /// 没有找到预期数据结束
        /// </summary>
        NotFoundValueEnd,
        /// <summary>
        /// 没有找到预期的CDATA开始
        /// </summary>
        NotFoundCDATAStart,
        /// <summary>
        /// 注释错误
        /// </summary>
        NoteError,
        /// <summary>
        /// 非正常意外结束
        /// </summary>
        CrashEnd,
        /// <summary>
        /// 不支持直接解析 基元类型/可空类型/数组/枚举/指针/字典
        /// </summary>
        NotSupport,
        /// <summary>
        /// 找不到构造函数
        /// </summary>
        NoConstructor,
        /// <summary>
        /// 字符解码失败
        /// </summary>
        DecodeError,
        /// <summary>
        /// 逻辑值解析错误
        /// </summary>
        NotBool,
        /// <summary>
        /// 非数字解析错误
        /// </summary>
        NotNumber,
        /// <summary>
        /// 16进制数字解析错误
        /// </summary>
        NotHex,
        /// <summary>
        /// 字符解析错误
        /// </summary>
        NotChar,
        /// <summary>
        /// 时间解析错误
        /// </summary>
        NotDateTime,
        /// <summary>
        /// Guid解析错误
        /// </summary>
        NotGuid,
        /// <summary>
        /// 数组节点解析错误
        /// </summary>
        NotArrayItem,
        /// <summary>
        /// 没有找到匹配的枚举值
        /// </summary>
        NoFoundEnumValue,
        /// <summary>
        /// 非枚举字符
        /// </summary>
        NotEnumChar,
        /// <summary>
        /// 未知名称节点自定义解析失败
        /// </summary>
        UnknownNameError,
        /// <summary>
        /// 自定义序列化失败
        /// </summary>
        Custom,
    }
}
