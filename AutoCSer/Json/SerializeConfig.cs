using System;
using System.Runtime.CompilerServices;
using AutoCSer.Metadata;

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 序列化配置参数
    /// </summary>
    public class SerializeConfig
    {
        /// <summary>
        /// 循环引用检测深度
        /// </summary>
        public const int DefaultCheckLoopDepth = 20;
        /// <summary>
        /// 成员位图
        /// </summary>
        public MemberMap MemberMap;
        /// <summary>
        /// 自定义 ToString("xxx") 格式
        /// </summary>
        public string DateTimeCustomFormat;
        /// <summary>
        /// 时间输出类型
        /// </summary>
        public DateTimeType DateTimeType;
        /// <summary>
        /// 最小时间是否输出为 null，默认为 true
        /// </summary>
        public bool IsDateTimeMinNull = true;
        /// <summary>
        /// 字符 0
        /// </summary>
        public char NullChar = (char)0;
        /// <summary>
        /// 循环引用检测深度，0 表示实时检测，默认为 20
        /// </summary>
        public int CheckLoopDepth = DefaultCheckLoopDepth;
        /// <summary>
        /// 是否将 object 转换成真实类型输出
        /// </summary>
        public bool IsObject;
        /// <summary>
        /// Dictionary[string,] 是否转换成对象输出，默认为 true
        /// </summary>
        public bool IsStringDictionaryToObject = true;
        /// <summary>
        /// Dictionary 是否转换成对象模式输出
        /// </summary>
        public bool IsDictionaryToObject;
        /// <summary>
        /// 整数是否允许转换为 16 进制字符串
        /// </summary>
        public bool IsIntegerToHex;
        /// <summary>
        /// 超出最大有效精度的 long / ulong 是否转换成字符串
        /// </summary>
        public bool IsMaxNumberToString;
        /// <summary>
        /// 成员位图类型不匹配时是否使用默认输出，默认为 true
        /// </summary>
        public bool IsMemberMapErrorToDefault = true;
        /// <summary>
        /// 默认为 true 表示将 Infinity / -Infinity 转换为 NaN 输出
        /// </summary>
        public bool IsInfinityToNaN = true;
        /// <summary>
        /// 逻辑值是否转换成 1/0 输出
        /// </summary>
        public bool IsBoolToInt;
#if AutoCSer
        /// <summary>
        /// 是否输出客户端视图绑定类型
        /// </summary>
        public bool IsViewClientType;
        /// <summary>
        /// 循环引用设置函数名称（WebView 专用）
        /// </summary>
        public string SetLoopObject;
        /// <summary>
        /// 循环引用获取函数名称（WebView 专用）
        /// </summary>
        public string GetLoopObject;
#endif
        /// <summary>
        /// 获取并设置自定义序列化成员位图
        /// </summary>
        /// <param name="memberMap">序列化成员位图</param>
        /// <returns>序列化成员位图</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public MemberMap SetCustomMemberMap(MemberMap memberMap)
        {
            MemberMap oldMemberMap = MemberMap;
            MemberMap = memberMap;
            return oldMemberMap;
        }
        /// <summary>
        /// 创建内部配置参数
        /// </summary>
        /// <returns></returns>
        internal static SerializeConfig CreateInternal()
        {
            return new SerializeConfig { DateTimeType = DateTimeType.Javascript, IsIntegerToHex = true, IsBoolToInt = true };
        }
    }
}