using System;

namespace AutoCSer.Sql.Member
{
    /// <summary>
    /// 整形日期 映射到 int
    /// </summary>
    [AutoCSer.Metadata.BoxSerialize]
    [AutoCSer.Json.Serialize(Filter = AutoCSer.Metadata.MemberFilters.InstanceField)]
    [AutoCSer.Json.Parse(Filter = AutoCSer.Metadata.MemberFilters.InstanceField)]
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    [AutoCSer.Sql.Member(DataType = typeof(int))]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct IntDate
    {
        /// <summary>
        /// 整形日期值
        /// </summary>
        [AutoCSer.WebView.OutputAjax]
        public int Value;
        /// <summary>
        /// 年份
        /// </summary>
        [AutoCSer.Json.IgnoreMember]
        [AutoCSer.BinarySerialize.IgnoreMember]
        public int Year
        {
            get { return Value >> 9; }
        }
        /// <summary>
        /// 月份
        /// </summary>
        [AutoCSer.Json.IgnoreMember]
        [AutoCSer.BinarySerialize.IgnoreMember]
        public int Month
        {
            get { return (Value >> 5) & 15; }
        }
        /// <summary>
        /// 日期天数
        /// </summary>
        [AutoCSer.Json.IgnoreMember]
        [AutoCSer.BinarySerialize.IgnoreMember]
        public int Day
        {
            get { return Value & 31; }
        }
        /// <summary>
        /// 整形日期
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <param name="day">日期</param>
        public IntDate(int year, int month, int day)
        {
            Value = (year << 9) + (month << 5) + day;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator IntDate(int value) { return new IntDate { Value = value }; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator int(IntDate value) { return value.Value; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator IntDate(DateTime value) { return new IntDate { Value = value != default(DateTime) ? (value.Year << 9) + (value.Month << 5) + value.Day : 0 }; }
        /// <summary>
        /// 日期时间值
        /// </summary>
        public DateTime DateTime
        {
            get 
            {
                if (Value == 0) return default(DateTime);
                return new DateTime(Value >> 9, (Value >> 5) & 15, Value & 31);
            }
        }
        ///// <summary>
        ///// 转换SQL字符串
        ///// </summary>
        ///// <returns></returns>
        //public string ToSqlString()
        //{
        //    return Value.toString();
        //}
    }
}
