using System;
using System.Collections.Generic;

namespace AutoCSer.Sql
{
    /// <summary>
    /// SQL函数调用
    /// </summary>
    public static class ExpressionCall
    {
        /// <summary>
        /// 自定义表达式异常
        /// </summary>
        public const string CustomExpressionExceptionMessage = "Custom Expression Exception";
        /// <summary>
        /// 自定义表达式异常类型名称
        /// </summary>
        internal const string CustomExpressionExceptionName = "SqlCustomExpressionException";

        /// <summary>
        /// SQL 函数调用
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="FucntionName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static valueType Call<valueType>(string FucntionName, params object[] parameters) { return default(valueType); }

        /// <summary>
        /// 计数
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        /// <returns>计数</returns>
        public static int Count<valueType>(valueType value)
        {
            return 0;
        }
        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        /// <returns>求和</returns>
        public static valueType Sum<valueType>(valueType value)
        {
            return default(valueType);
        }
        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns>当前时间</returns>
        public static DateTime GetDate()
        {
            return AutoCSer.Threading.SecondTimer.Now;
        }
        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns>当前时间</returns>
        public static DateTime SysDateTime()
        {
            return AutoCSer.Threading.SecondTimer.Now;
        }
        /// <summary>
        /// IN 表达式
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value">数据</param>
        /// <param name="values">数值集合</param>
        /// <returns>是否包含数据</returns>
        public static bool In<valueType>(valueType value, params valueType[] values)
        {
            return false;
        }
        /// <summary>
        /// IN 表达式
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value">数据</param>
        /// <param name="values">数值集合</param>
        /// <returns>是否包含数据</returns>
        public static bool In<valueType>(valueType value, IEnumerable<valueType> values)
        {
            return false;
        }
        /// <summary>
        /// NOT IN 表达式
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value">数据</param>
        /// <param name="values">数值集合</param>
        /// <returns>是否不包含数据</returns>
        public static bool NotIn<valueType>(valueType value, params valueType[] values)
        {
            return false;
        }
        /// <summary>
        /// NOT IN 表达式
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value">数据</param>
        /// <param name="values">数值集合</param>
        /// <returns>是否不包含数据</returns>
        public static bool NotIn<valueType>(valueType value, IEnumerable<valueType> values)
        {
            return false;
        }
        /// <summary>
        /// 获取字符串长度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Len(string value)
        {
            return -1;
        }
        /// <summary>
        /// LIKE 表达式
        /// </summary>
        /// <param name="value"></param>
        /// <param name="matchValue"></param>
        /// <returns></returns>
        public static bool Like(string value, string matchValue)
        {
            return false;
        }
        /// <summary>
        /// NOT LIKE 表达式
        /// </summary>
        /// <param name="value"></param>
        /// <param name="matchValue"></param>
        /// <returns></returns>
        public static bool NotLike(string value, string matchValue)
        {
            return false;
        }
        /// <summary>
        /// LIKE 表达式
        /// </summary>
        /// <param name="value"></param>
        /// <param name="matchValue"></param>
        /// <returns></returns>
        public static bool StartsWith(string value, string matchValue)
        {
            return false;
        }
        /// <summary>
        /// LIKE 表达式
        /// </summary>
        /// <param name="value"></param>
        /// <param name="matchValue"></param>
        /// <returns></returns>
        public static bool NotStartsWith(string value, string matchValue)
        {
            return false;
        }
        /// <summary>
        /// LIKE 表达式
        /// </summary>
        /// <param name="value"></param>
        /// <param name="matchValue"></param>
        /// <returns></returns>
        public static bool EndsWith(string value, string matchValue)
        {
            return false;
        }
        /// <summary>
        /// LIKE 表达式
        /// </summary>
        /// <param name="value"></param>
        /// <param name="matchValue"></param>
        /// <returns></returns>
        public static bool NotEndsWith(string value, string matchValue)
        {
            return false;
        }
        /// <summary>
        /// REPLACE 表达式
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string Replace(string value, string oldValue, string newValue)
        {
            return null;
        }
        /// <summary>
        /// 空值判断函数支持
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        public static valueType IsNull<valueType>(valueType value, valueType nullValue)
        {
            return value;
        }
        /// <summary>
        /// 计算时间差
        /// </summary>
        /// <param name="DateDiffType"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public static int DateDiff(ExpressionCallDateDiffType DateDiffType, DateTime StartTime, DateTime EndTime)
        {
            return 0;
        }
    }
}
