using System;
using System.Reflection;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 自定义属性函数信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct AttributeMethod
    {
        /// <summary>
        /// 函数信息
        /// </summary>
        public MethodInfo Method;
        /// <summary>
        /// 自定义属性集合
        /// </summary>
        private object[] attributes;
        /// <summary>
        /// 自定义属性函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, AttributeMethod[]> staticMethods = new AutoCSer.Threading.LockDictionary<Type, AttributeMethod[]>();
        /// <summary>
        /// 自定义属性函数信息集合访问锁
        /// </summary>
        private static readonly object createStaticLock = new object();
        /// <summary>
        /// 根据类型获取自定义属性函数信息集合
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns>自定义属性函数信息集合</returns>
        public static AttributeMethod[] GetStatic(Type type)
        {
            AttributeMethod[] values;
            if (staticMethods.TryGetValue(type, out values)) return values;
            Monitor.Enter(createStaticLock);
            try
            {
                if (staticMethods.TryGetValue(type, out values)) return values;
                LeftArray<AttributeMethod> array = default(LeftArray<AttributeMethod>);
                foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
                {
                    object[] attributes = method.GetCustomAttributes(true);
                    if (attributes.Length != 0) array.Add(new AttributeMethod { Method = method, attributes = attributes });
                }
                staticMethods.Set(type, values = array.ToArray());
            }
            finally { Monitor.Exit(createStaticLock); }
            return values;
        }
        /// <summary>
        /// 自定义属性函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, AttributeMethod[]> methods = new AutoCSer.Threading.LockDictionary<Type, AttributeMethod[]>();
        /// <summary>
        /// 自定义属性函数信息集合访问锁
        /// </summary>
        private static readonly object createLock = new object();
        /// <summary>
        /// 根据类型获取自定义属性函数信息集合
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns>自定义属性函数信息集合</returns>
        public static AttributeMethod[] Get(Type type)
        {
            AttributeMethod[] values;
            if (methods.TryGetValue(type, out values)) return values;
            Monitor.Enter(createLock);
            try
            {
                if (methods.TryGetValue(type, out values)) return values;
                LeftArray<AttributeMethod> array = default(LeftArray<AttributeMethod>);
                foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
                {
                    object[] attributes = method.GetCustomAttributes(true);
                    if (attributes.Length != 0) array.Add(new AttributeMethod { Method = method, attributes = attributes });
                }
                methods.Set(type, values = array.ToArray());
            }
            finally { Monitor.Exit(createLock); }
            return values;
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ClearCache()
        {
            methods.Clear();
            staticMethods.Clear();
        }
        /// <summary>
        /// 获取自定义属性集合
        /// </summary>
        /// <typeparam name="attributeType"></typeparam>
        /// <returns></returns>
        internal IEnumerable<attributeType> Attributes<attributeType>() where attributeType : Attribute
        {
            foreach (object value in attributes)
            {
                attributeType attribute = value as attributeType;
                if (attribute != null) yield return attribute;
            }
        }
        /// <summary>
        /// 根据成员属性获取自定义属性
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <returns>自定义属性</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public attributeType GetAttribute<attributeType>() where attributeType : Attribute
        {
            foreach (attributeType attribute in Attributes<attributeType>()) return attribute;
            return null;
        }
    }
}
