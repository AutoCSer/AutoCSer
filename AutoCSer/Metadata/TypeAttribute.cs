using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 类型自定义属性信息
    /// </summary>
    public static partial class TypeAttribute
    {
        /// <summary>
        /// 自定义属性信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, object[]> attributes = new AutoCSer.Threading.LockDictionary<Type, object[]>();
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ClearCache()
        {
            attributes.Clear();
        }
        /// <summary>
        /// 根据类型获取自定义属性信息集合
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns>自定义属性信息集合</returns>
        private static object[] get(Type type)
        {
            object[] values;
            if (attributes.TryGetValue(type, out values)) return values;
            attributes.Set(type, values = type.GetCustomAttributes(true));
            return values;
        }
        /// <summary>
        /// 获取自定义属性集合
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <typeparam name="attributeType"></typeparam>
        /// <returns></returns>
        internal static IEnumerable<attributeType> GetAttributes<attributeType>(Type type) where attributeType : Attribute
        {
            foreach (object value in get(type))
            {
                attributeType attribute = value as attributeType;
                if (attribute != null) yield return attribute;
            }
        }
        /// <summary>
        /// 根据类型获取自定义属性
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="type">类型</param>
        /// <returns>自定义属性</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static attributeType GetAttribute<attributeType>(Type type)
            where attributeType : Attribute
        {
            foreach (attributeType attribute in GetAttributes<attributeType>(type))
            {
                return (attributeType)attribute;
            }
            return null;
        }
        /// <summary>
        /// 根据类型获取自定义属性
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="type">类型</param>
        /// <param name="isBaseType">是否搜索父类属性</param>
        /// <returns>自定义属性</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static attributeType GetAttribute<attributeType>(Type type, bool isBaseType)
            where attributeType : Attribute
        {
            if (isBaseType) return GetAttribute<attributeType>(type);
            foreach (attributeType attribute in type.GetCustomAttributes(typeof(attributeType), false)) return attribute;
            return null;
        }
    }
}
