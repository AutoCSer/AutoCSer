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
        /// 循环引用设置函数名称
        /// </summary>
        public string SetLoopObject;
        /// <summary>
        /// 循环引用获取函数名称
        /// </summary>
        public string GetLoopObject;
        /// <summary>
        /// 循环引用检测深度，0 表示实时检测，默认为 20
        /// </summary>
        public int CheckLoopDepth = DefaultCheckLoopDepth;
        /// <summary>
        /// 成员位图
        /// </summary>
        public MemberMap MemberMap;
        /// <summary>
        /// 字符 0
        /// </summary>
        public char NullChar = (char)0;
        /// <summary>
        /// 最小时间是否输出为 null，默认为 true
        /// </summary>
        public bool IsDateTimeMinNull = true;
        /// <summary>
        /// 时间是否转换成字符串，默认为 true
        /// </summary>
        public bool IsDateTimeToString = true;
        /// <summary>
        /// 第三方格式 /Date(xxx)/
        /// </summary>
        public bool IsDateTimeOther;
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
        /// 数字是否允许转换为 16 进制字符串
        /// </summary>
        public bool IsNumberToHex;
#if AutoCSer
        /// <summary>
        /// 是否输出客户端视图绑定类型
        /// </summary>
        public bool IsViewClientType;
#endif
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
            return new SerializeConfig { IsDateTimeToString = false, IsNumberToHex = true };
        }
    }
}
