using System;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 动态数组信息
    /// </summary>
    internal static class DynamicArray
    {
        /// <summary>
        /// 是否需要清除数组缓存信息
        /// </summary>
        private static Dictionary<Type, bool> isClearArrayCache = AutoCSer.DictionaryCreator.CreateOnly<Type, bool>();
        /// <summary>
        /// 是否需要清除数组缓存 访问锁
        /// </summary>
        private static readonly object isClearArrayLock = new object();
        /// <summary>
        /// 是否需要清除数组
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>需要清除数组</returns>
        public static bool IsClearArray(Type type)
        {
            if (type.IsPointer) return false;
            if (type.IsClass || type.IsInterface) return true;
            if (type.IsEnum) return false;
            if (type.IsValueType)
            {
                bool isClear;
                Monitor.Enter(isClearArrayLock);
                try
                {
                    if (isClearArrayCache.TryGetValue(type, out isClear)) return isClear;
                    isClearArrayCache.Add(type, isClear = isClearArray(type));
                }
                finally { Monitor.Exit(isClearArrayLock); }
                return isClear;
            }
            return true;
        }
        /// <summary>
        /// 是否需要清除数组
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>需要清除数组</returns>
        private static bool isClearArray(Type type)
        {
            foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                Type fieldType = field.FieldType;
                if (fieldType != type && !fieldType.IsPointer)
                {
                    if (fieldType.IsClass || fieldType.IsInterface) return true;
                    if (!fieldType.IsEnum)
                    {
                        if (fieldType.IsValueType)
                        {
                            bool isClear;
                            if (!isClearArrayCache.TryGetValue(fieldType, out isClear))
                            {
                                isClearArrayCache.Add(fieldType, isClear = isClearArray(fieldType));
                            }
                            if (isClear) return true;
                        }
                        else return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        internal static void ClearCache()
        {
            Monitor.Enter(isClearArrayLock);
            try
            {
                if (isClearArrayCache.Count != 0) isClearArrayCache = AutoCSer.DictionaryCreator.CreateOnly<Type, bool>();
            }
            finally { Monitor.Exit(isClearArrayLock); }
        }
    }
    /// <summary>
    /// 动态数组基类
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public abstract class DynamicArray<valueType>
    {
        /// <summary>
        /// 是否需要清除数组
        /// </summary>
        internal static readonly bool IsClearArray = DynamicArray.IsClearArray(typeof(valueType));
        /// <summary>
        /// 创建新数组
        /// </summary>
        /// <param name="length">数组长度</param>
        /// <returns>数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static valueType[] GetNewArray(int length)
        {
            return new valueType[(uint)length <= ((int.MaxValue >> 1) + 1) ? (int)((uint)length).UpToPower2() : int.MaxValue];
        }

        /// <summary>
        /// 数据数组
        /// </summary>
        protected internal valueType[] Array;
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly { get { return false; } }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="values">数据集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Add(valueType[] values)
        {
            if (values != null) Add(values, 0, values.Length);
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        public abstract void Add(valueType[] values, int index, int count);
    }
}
