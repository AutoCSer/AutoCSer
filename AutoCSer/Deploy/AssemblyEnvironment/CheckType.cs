using System;

namespace AutoCSer.Deploy.AssemblyEnvironment
{
    /// <summary>
    /// 程序集环境检测任务类型
    /// </summary>
    public enum CheckType : byte
    {
        /// <summary>
        /// 未知类型
        /// </summary>
        Unknown,
        /// <summary>
        /// 判断类型是否存在
        /// </summary>
        IsType,
        /// <summary>
        /// 获取静态字段值
        /// </summary>
        GetField,
        /// <summary>
        /// 获取静态属性值
        /// </summary>
        GetProperty,
    }
}
