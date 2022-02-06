using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.ObjectRoot
{
    /// <summary>
    /// 反射模式数组类型
    /// </summary>
    public sealed class ReflectionArrayType : ReflectionType
    {
        /// <summary>
        /// 是否数组（包括 string）
        /// </summary>
        internal override bool IsArray { get { return true; } }
        /// <summary>
        /// 数组元素类型
        /// </summary>
        internal ReflectionType ElementType;
        /// <summary>
        /// 数组元素统计数量
        /// </summary>
        public long ElementCount { get; internal set; }
        /// <summary>
        /// string
        /// </summary>
        internal ReflectionArrayType() : base(typeof(string))
        {
            IsScan = 1;
            ElementType = ReflectionObjectType.NullType;
        }
        /// <summary>
        /// 数组类型
        /// </summary>
        /// <param name="type">类型</param>
        internal ReflectionArrayType(Type type) : base(type)
        {
            IsScan = 1;
            ElementType = ReflectionObjectType.NullType;
        }
        /// <summary>
        /// 获取数组元素类型
        /// </summary>
        /// <param name="scanner"></param>
        internal void GetElementType(ReflectionTypeScanner scanner)
        {
            Type ElementType = Type.GetElementType();
            if (!ElementType.IsPointer && ElementType != typeof(System.Reflection.Pointer))
            {
                this.ElementType = scanner.GetObjectType(ElementType);
                if (this.ElementType.IsScan != 0 || IsScanDerived(ElementType)) IsScan = 2;
            }
            else this.ElementType = ReflectionObjectType.NullType;
        }
        /// <summary>
        /// 添加扫描对象
        /// </summary>
        /// <param name="scanner"></param>
        /// <param name="value"></param>
        /// <param name="isArray">对象是否数组元素，数组元素不统计根静态字段</param>
        /// <returns>添加对象类型</returns>
        internal override byte Append(ref ReflectionScanner scanner, object value, bool isArray)
        {
            if (value.GetType() == typeof(string)) ElementCount += ((string)value).Length;
            else
            {
                ElementCount += ((Array)value).Length;
                if (IsScan == 2)
                {
                    scanner.Append(new ReflectionArrayScanner(value, ElementType));
                    return 1;
                }
            }
            return 2;
        }
    }
}
