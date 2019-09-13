using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 解析配置参数
    /// </summary>
    public class ParseConfig
    {
        /// <summary>
        /// 自定义构造函数
        /// </summary>
        public Func<Type, object> Constructor;
        ///// <summary>
        ///// 成员选择
        ///// </summary>
        //public AutoCSer.Metadata.MemberFilters MemberFilters = Metadata.MemberFilters.Instance;
        /// <summary>
        /// 对象解析结束后是否检测最后的空格符，默认为 true
        /// </summary>
        public bool IsEndSpace = true;
        /// <summary>
        /// 是否临时字符串(可修改)
        /// </summary>
        public bool IsTempString;
        /// <summary>
        /// 是否强制匹配枚举值
        /// </summary>
        public bool IsMatchEnum;
        /// <summary>
        /// 默认数组大小
        /// </summary>
        public int NewArraySize = 10;
        ///// <summary>
        ///// 是否紧凑解析模式
        ///// </summary>
        //public bool IsCompact;
        /// <summary>
        /// 指针模式 JSON 解析失败时是否 new string
        /// </summary>
        public bool IsErrorJsonNewString;
        /// <summary>
        /// 清空数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Null()
        {
            Constructor = null;
        }
    }
}
