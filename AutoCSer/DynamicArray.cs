using System;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 动态数组信息
    /// </summary>
    internal static class DynamicArray
    {
        /// <summary>
        /// 默认数组容器长度
        /// </summary>
        internal const int DefalutArrayCapacity = sizeof(int);
        /// <summary>
        /// 是否需要清除数组缓存信息
        /// </summary>
        private static Dictionary<HashType, bool> isClearArrayCache;
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
                    if (isClearArrayCache != null)
                    {
                        if (isClearArrayCache.TryGetValue(type, out isClear)) return isClear;
                    }
                    else isClearArrayCache = DictionaryCreator<HashType>.Create<bool>();
                    isClearArrayCache.Add(type, true);
                    isClearArrayCache[type] = isClear = isClearArray(type, isClearArrayCache);
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
        /// <param name="isClearArrayCache"></param>
        /// <returns>需要清除数组</returns>
        private static bool isClearArray(Type type, Dictionary<HashType, bool> isClearArrayCache)
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
                                isClearArrayCache.Add(fieldType, true);
                                isClearArrayCache[fieldType] = isClear = isClearArray(fieldType, isClearArrayCache);
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
        private static void clearCache()
        {
            Monitor.Enter(isClearArrayLock);
            isClearArrayCache = null;
            Monitor.Exit(isClearArrayLock);
        }
        static DynamicArray()
        {
            AutoCSer.Memory.Common.AddClearCache(clearCache, typeof(DynamicArray), 60 * 60);
        }
    }
    /// <summary>
    /// 动态数组基类
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public static class DynamicArray<T>
    {
        /// <summary>
        /// 是否需要清除数组
        /// </summary>
        internal static readonly bool IsClearArray = DynamicArray.IsClearArray(typeof(T));
        /// <summary>
        /// 创建新数组
        /// </summary>
        /// <param name="length">数组长度</param>
        /// <returns>数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static T[] GetNewArray(int length)
        {
            return new T[(uint)length <= ((int.MaxValue >> 1) + 1) ? (int)((uint)length).UpToPower2() : int.MaxValue];
        }
    }
}
