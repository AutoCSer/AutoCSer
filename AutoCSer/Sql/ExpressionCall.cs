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
        public static int Sum<valueType>(valueType value)
        {
            return 0;
        }
        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns>当前时间</returns>
        public static DateTime GetDate()
        {
            return AutoCSer.Date.NowTime.Now;
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
        /// LIKE 表达式
        /// </summary>
        /// <param name="value"></param>
        /// <param name="matchValue"></param>
        /// <returns></returns>
        public static bool Like(string value, string matchValue)
        {
            return false;
        }
    }
}
