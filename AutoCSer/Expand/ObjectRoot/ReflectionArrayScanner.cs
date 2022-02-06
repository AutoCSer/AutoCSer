using System;
using System.Collections;

namespace AutoCSer.ObjectRoot
{
    /// <summary>
    /// 反射模式数组扫描
    /// </summary>
    internal struct ReflectionArrayScanner
    {
        /// <summary>
        /// 数组枚举器
        /// </summary>
        private IEnumerator enumerator;
        /// <summary>
        /// 对象类型
        /// </summary>
        private ReflectionType objectType;
        /// <summary>
        /// 反射模式数组扫描
        /// </summary>
        /// <param name="value">数组对象</param>
        /// <param name="objectType">对象类型</param>
        internal ReflectionArrayScanner(object value, ReflectionType objectType)
        {
            enumerator = ((Array)value).GetEnumerator();
            this.objectType = objectType;
        }
        /// <summary>
        /// 读取下一个数据
        /// </summary>
        /// <param name="scanner"></param>
        internal void Next(ref ReflectionScanner scanner)
        {
            while (enumerator.MoveNext())
            {
                object elementValue = enumerator.Current;
                if (elementValue != null)
                {
                    Type fieldType = elementValue.GetType();
                    if (fieldType != objectType.Type) objectType = scanner.Scanner.GetObjectType(fieldType);
                    if (objectType.IsScan != 0)
                    {
                        if (fieldType.IsClass)
                        {
                            if (scanner.AddObject(elementValue))
                            {
                                if (objectType.Append(ref scanner, elementValue, true) != 2) return;
                            }
                            else if (scanner.IsLimitExceeded) return;
                        }
                        else if (objectType.Append(ref scanner, elementValue, true) != 2) return;
                    }
                }
            }
            --scanner.Arrays.Length;
        }
    }
}
