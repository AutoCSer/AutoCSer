using System;

namespace AutoCSer.DiskBlock
{
    /// <summary>
    /// 磁盘块成员对象
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct MemberValue<valueType> where valueType : class
    {
        /// <summary>
        /// 数据对象
        /// </summary>
        public valueType Value;
        /// <summary>
        /// 成员状态
        /// </summary>
        public MemberState State;
        /// <summary>
        /// 数据对象
        /// </summary>
        /// <param name="value">磁盘块成员对象</param>
        /// <returns>数据对象</returns>
        public static implicit operator valueType(MemberValue<valueType> value)
        {
            switch (value.State)
            {
                case MemberState.Local:
                case MemberState.Remote:
                    return value.Value;
            }
            throw new Exception(value.State.ToString());
        }
    }
}
