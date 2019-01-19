using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 解析状态
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
        /// JSON 字符串参数为空
        /// </summary>
        NullJson,
        /// <summary>
        /// 解析目标对象参数为空
        /// </summary>
        NullValue,
        /// <summary>
        /// 非正常意外结束
        /// </summary>
        CrashEnd,
        /// <summary>
        /// 未能识别的注释
        /// </summary>
        UnknownNote,
        /// <summary>
        /// /**/ 注释缺少回合
        /// </summary>
        NoteNotRound,
        /// <summary>
        /// null 值解析失败
        /// </summary>
        NotNull,
        /// <summary>
        /// 逻辑值解析错误
        /// </summary>
        NotBool,
        /// <summary>
        /// 非数字解析错误
        /// </summary>
        NotNumber,
        /// <summary>
        /// 16 进制数字解析错误
        /// </summary>
        NotHex,
        /// <summary>
        /// 字符解析错误
        /// </summary>
        NotChar,
        /// <summary>
        /// 字符串解析失败
        /// </summary>
        NotString,
        /// <summary>
        /// 字符串被换行截断
        /// </summary>
        StringEnter,
        /// <summary>
        /// 时间解析错误
        /// </summary>
        NotDateTime,
        /// <summary>
        /// Guid解析错误
        /// </summary>
        NotGuid,
        /// <summary>
        /// 不支持多维数组
        /// </summary>
        ArrayManyRank,
        /// <summary>
        /// 数组解析错误
        /// </summary>
        NotArray,
        /// <summary>
        /// 数组数据解析错误
        /// </summary>
        NotArrayValue,
        ///// <summary>
        ///// 不支持指针
        ///// </summary>
        //Pointer,
        ///// <summary>
        ///// 找不到构造函数
        ///// </summary>
        //NoConstructor,
        /// <summary>
        /// 非枚举字符
        /// </summary>
        NotEnumChar,
        /// <summary>
        /// 没有找到匹配的枚举值
        /// </summary>
        NoFoundEnumValue,
        /// <summary>
        /// 对象解析错误
        /// </summary>
        NotObject,
        /// <summary>
        /// 没有找到成员名称
        /// </summary>
        NotFoundName,
        /// <summary>
        /// 没有找到冒号
        /// </summary>
        NotFoundColon,
        /// <summary>
        /// 忽略值解析错误
        /// </summary>
        UnknownValue,
        /// <summary>
        /// 字典解析错误
        /// </summary>
        NotDictionary,
        /// <summary>
        /// 类型解析错误
        /// </summary>
        ErrorType,
        /// <summary>
        /// 自定义序列化失败
        /// </summary>
        Custom,
    }
}
