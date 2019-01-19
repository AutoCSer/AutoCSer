using System;
using AutoCSer.Extension;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 枚举属性获取器
    /// </summary>
    /// <typeparam name="enumType">枚举类型</typeparam>
    public abstract class EnumAttribute<enumType> where enumType : struct, IComparable, IFormattable, IConvertible
    {
        /// <summary>
        /// 获取最大枚举值
        /// </summary>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大枚举值,失败返回默认空值</returns>
        public static int GetMaxValue(int nullValue)
        {
            Type type = typeof(enumType);
            bool isEnum = type.IsEnum;
            if (isEnum)
            {
                enumType[] values = (enumType[])System.Enum.GetValues(type);
                if (values.Length != 0)
                {
                    int maxValue = int.MinValue;
                    foreach (enumType value in values)
                    {
                        int intValue = value.ToInt32(null);
                        if (intValue > maxValue) maxValue = intValue;
                    }
                    return maxValue;
                }
            }
            return nullValue;
        }
        /// <summary>
        /// 获取枚举数组
        /// </summary>
        /// <returns>枚举数组</returns>
        public static enumType[] Array()
        {
            Array array = System.Enum.GetValues(typeof(enumType));
            enumType[] values = new enumType[array.Length];
            int count = 0;
            foreach (enumType value in array) values[count++] = value;
            return values;
        }
    }
    /// <summary>
    /// 枚举属性获取器
    /// </summary>
    /// <typeparam name="enumType">枚举类型</typeparam>
    /// <typeparam name="attributeType">属性类型</typeparam>
    public sealed class EnumAttribute<enumType, attributeType> : EnumAttribute<enumType>
        where enumType : struct, IComparable, IFormattable, IConvertible
        where attributeType : Attribute
    {
        /// <summary>
        /// 属性集合
        /// </summary>
        private static attributeType[] attributeArray;
        /// <summary>
        /// 属性集合
        /// </summary>
        internal static attributeType[] AttributeArray
        {
            get
            {
                if (attributeArray == null) attributeArray = GetAttributes();
                return attributeArray;
            }
        }
        /// <summary>
        /// 获取枚举属性集合
        /// </summary>
        /// <returns>枚举属性集合</returns>
        public static attributeType[] GetAttributes()
        {
            int length = GetMaxValue(-1) + 1;
            if (length != 0)
            {
                if (length >= 1024) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Warn, typeof(enumType).fullName() + " 枚举数组过大 " + length.toString());
                int index;
                attributeType[] names = new attributeType[length];
                //Type enumAttributeType = typeof(attributeType);
                foreach (FieldInfo field in typeof(enumType).GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    foreach(attributeType attribute in field.GetCustomAttributes(typeof(attributeType), false))
                    {
                        if ((index = ((IConvertible)field.GetValue(null)).ToInt32(null)) < length) names[index] = attribute;
                        break;
                    }
                }
                return names;
            }
            return NullValue<attributeType>.Array;
        }
        /// <summary>
        /// 根据索引获取属性
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>属性</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static attributeType Array(int index)
        {
            return (uint)index < (uint)AttributeArray.Length ? AttributeArray[index] : null;
        }
    }
}
