using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Web.SearchServer
{
    /// <summary>
    /// 搜索数据关键字
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct DataKey : IEquatable<DataKey>
    {
        /// <summary>
        /// 搜索数据类型
        /// </summary>
        public DataType Type;
        /// <summary>
        /// 搜索数据标识
        /// </summary>
        [AutoCSer.WebView.OutputAjax]
        public int Id;
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(DataKey other)
        {
            return ((int)((uint)Type ^ (uint)other.Type) | (Id ^ other.Id)) == 0;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((DataKey)obj);
        }
        /// <summary>
        /// HASH 值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Id ^ (int)(uint)Type;
        }
    }
}
