using System;

namespace AutoCSer.ObjectRoot
{
    /// <summary>
    /// 反射模式对象类型
    /// </summary>
    public abstract class ReflectionType : ObjectType
    {
        /// <summary>
        /// 反射模式对象类型
        /// </summary>
        /// <param name="type">类型</param>
        internal ReflectionType(Type type) : base(type) { }
        /// <summary>
        /// 添加扫描对象
        /// </summary>
        /// <param name="scanner"></param>
        /// <param name="value"></param>
        /// <param name="isArray">对象是否数组元素，数组元素不统计根静态字段</param>
        /// <returns>添加对象类型</returns>
        internal abstract byte Append(ref ReflectionScanner scanner, object value, bool isArray);
    }
}
