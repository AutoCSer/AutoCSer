using System;
using System.Reflection;

namespace AutoCSer.ObjectRoot
{
    /// <summary>
    /// 反射模式对象扫描
    /// </summary>
    internal struct ReflectionObjectScanner
    {
        /// <summary>
        /// 扫描的对象
        /// </summary>
        private readonly object value;
        /// <summary>
        /// 对象类型
        /// </summary>
        private readonly ReflectionObjectType objectType;
        /// <summary>
        /// 当前读取字段索引
        /// </summary>
        private int index;
        /// <summary>
        /// 对象是否数组元素，数组元素不统计根静态字段
        /// </summary>
        private readonly bool isArray;
        /// <summary>
        /// 反射模式对象扫描
        /// </summary>
        /// <param name="value">扫描的对象</param>
        /// <param name="objectType">对象类型</param>
        /// <param name="isArray">对象是否数组元素，数组元素不统计根静态字段</param>
        internal ReflectionObjectScanner(object value, ReflectionObjectType objectType, bool isArray)
        {
            this.value = value;
            this.objectType = objectType;
            this.isArray = isArray;
            index = 0;
        }
        /// <summary>
        /// 读取下一个数据
        /// </summary>
        /// <param name="scanner"></param>
        internal void Next(ref ReflectionScanner scanner)
        {
            object value = this.value;
            bool isArray = this.isArray;
            int isEndIndex = 0;
            do
            {
                KeyValue<FieldInfo, ReflectionType> field = this.objectType.ScanFields[index++];
                if (index == this.objectType.ScanFields.Length)
                {
                    --scanner.Objects.Length;
                    if (this.objectType.BaseType.IsScan != 0) this.objectType.BaseType.Append(ref scanner, value, isArray);
                    isEndIndex = 1;
                }
                object fieldValue = field.Key.GetValue(value);
                if (fieldValue != null)
                {
                    ReflectionType objectType = field.Value;
                    Type fieldType = fieldValue.GetType();
                    if (fieldType != objectType.Type) objectType = scanner.Scanner.GetObjectType(fieldType);
                    if (objectType.IsScan != 0)
                    {
                        if (fieldType.IsClass)
                        {
                            if (scanner.AddObject(fieldValue))
                            {
                                if (!isArray) objectType.Add(scanner.FieldInfo, scanner.Scanner);
                                if (objectType.Append(ref scanner, fieldValue, false) == 0) return;
                            }
                            else if (scanner.IsLimitExceeded) return;
                        }
                        else if (objectType.Append(ref scanner, fieldValue, false) == 0) return;
                    }
                }
            }
            while (isEndIndex == 0);
        }
    }
}
