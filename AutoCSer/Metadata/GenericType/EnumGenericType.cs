using AutoCSer.Threading;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract partial class EnumGenericType : GenericTypeBase
    {
        /// <summary>
        /// 枚举转数字委托
        /// </summary>
        internal abstract Delegate ToIntDelegate { get; }
        /// <summary>
        /// 数字转枚举委托
        /// </summary>
        internal abstract Delegate FromIntDelegate { get; }

        /// <summary>
        /// 获取二进制序列化枚举委托
        /// </summary>
        internal abstract Delegate BinarySerializeEnumMemberDelegate { get; }
        /// <summary>
        /// 获取二进制序列化枚举数组委托
        /// </summary>
        internal abstract Delegate BinarySerializeEnumArrayMemberDelegate { get; }
        /// <summary>
        /// 获取二进制序列化枚举数组委托
        /// </summary>
        internal abstract Delegate BinarySerializeEnumArrayDelegate { get; }
        /// <summary>
        /// 获取二进制反序列化枚举委托
        /// </summary>
        internal abstract Delegate BinaryDeSerializeEnumDelegate { get; }
        /// <summary>
        /// 获取二进制反序列化枚举委托
        /// </summary>
        internal abstract Delegate BinaryDeSerializeEnumMemberDelegate { get; }
        /// <summary>
        /// 获取二进制反序列化枚举数组委托
        /// </summary>
        internal abstract Delegate BinaryDeSerializeEnumArrayDelegate { get; }
        /// <summary>
        /// 获取二进制反序列化枚举数组委托
        /// </summary>
        internal abstract Delegate BinaryDeSerializeEnumArrayMemberDelegate { get; }

        /// <summary>
        /// 获取 JSON 反序列化枚举委托
        /// </summary>
        internal abstract Delegate JsonDeSerializeEnumDelegate { get; }
        /// <summary>
        /// 获取 JSON 反序列化枚举委托
        /// </summary>
        internal abstract Delegate JsonDeSerializeEnumFlagsDelegate { get; }

        /// <summary>
        /// 获取 XML 反序列化枚举委托
        /// </summary>
        internal abstract Delegate XmlDeSerializeEnumDelegate { get; }
        /// <summary>
        /// 获取 XML 反序列化枚举委托
        /// </summary>
        internal abstract Delegate XmlDeSerializeEnumFlagsDelegate { get; }

        /// <summary>
        /// 泛型类型元数据缓存
        /// </summary>
        private static readonly AutoCSer.Threading.LockLastDictionary<HashType, EnumGenericType> cache = new LockLastDictionary<HashType, EnumGenericType>(getCurrentType);
        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="UT"></typeparam>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static EnumGenericType create<T, UT>()
            where T : struct, IConvertible
            where UT : struct, IConvertible
        {
            return new EnumGenericType<T, UT>();
        }
        /// <summary>
        /// 创建泛型类型元数据 函数信息
        /// </summary>
        private static readonly MethodInfo createMethod = typeof(EnumGenericType).GetMethod("create", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static EnumGenericType Get(Type type)
        {
            EnumGenericType value;
            if (!cache.TryGetValue(type, out value))
            {
                try
                {
                    Type underlyingType = System.Enum.GetUnderlyingType(type);
                    value = (EnumGenericType)createMethod.MakeGenericMethod(type, underlyingType).Invoke(null, null);
                    cache.Set(type, value);
                }
                finally { cache.Exit(); }
            }
            return value;
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="UT"></typeparam>
    internal sealed partial class EnumGenericType<T, UT> : EnumGenericType
        where T : struct, IConvertible
        where UT : struct, IConvertible
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 获取二进制序列化枚举委托
        /// </summary>
        internal override Delegate BinarySerializeEnumMemberDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingType.Int: return (Action<BinarySerializer, T>)AutoCSer.BinarySerializer.EnumIntMember<T>;
                    case UnderlyingType.UInt: return (Action<BinarySerializer, T>)AutoCSer.BinarySerializer.EnumUIntMember<T>;
                    case UnderlyingType.Byte: return (Action<BinarySerializer, T>)AutoCSer.BinarySerializer.EnumByteMember<T>;
                    case UnderlyingType.ULong: return (Action<BinarySerializer, T>)AutoCSer.BinarySerializer.EnumULongMember<T>;
                    case UnderlyingType.UShort: return (Action<BinarySerializer, T>)AutoCSer.BinarySerializer.EnumUShortMember<T>;
                    case UnderlyingType.Long: return (Action<BinarySerializer, T>)AutoCSer.BinarySerializer.EnumLongMember<T>;
                    case UnderlyingType.Short: return (Action<BinarySerializer, T>)AutoCSer.BinarySerializer.EnumShortMember<T>;
                    case UnderlyingType.SByte: return (Action<BinarySerializer, T>)AutoCSer.BinarySerializer.EnumSByteMember<T>;
                    default: return (Action<BinarySerializer, T>)AutoCSer.BinarySerializer.EnumIntMember<T>;
                }
            }
        }
        /// <summary>
        /// 获取二进制序列化枚举数组委托
        /// </summary>
        internal override Delegate BinarySerializeEnumArrayMemberDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingType.Int: return (Action<BinarySerializer, T[]>)AutoCSer.BinarySerializer.EnumIntArrayMember<T>;
                    case UnderlyingType.UInt: return (Action<BinarySerializer, T[]>)AutoCSer.BinarySerializer.EnumUIntArrayMember<T>;
                    case UnderlyingType.Byte: return (Action<BinarySerializer, T[]>)AutoCSer.BinarySerializer.EnumByteArrayMember<T>;
                    case UnderlyingType.ULong: return (Action<BinarySerializer, T[]>)AutoCSer.BinarySerializer.EnumULongArrayMember<T>;
                    case UnderlyingType.UShort: return (Action<BinarySerializer, T[]>)AutoCSer.BinarySerializer.EnumUShortArrayMember<T>;
                    case UnderlyingType.Long: return (Action<BinarySerializer, T[]>)AutoCSer.BinarySerializer.EnumLongArrayMember<T>;
                    case UnderlyingType.Short: return (Action<BinarySerializer, T[]>)AutoCSer.BinarySerializer.EnumShortArrayMember<T>;
                    case UnderlyingType.SByte: return (Action<BinarySerializer, T[]>)AutoCSer.BinarySerializer.EnumSByteArrayMember<T>;
                    default: return (Action<BinarySerializer, T[]>)AutoCSer.BinarySerializer.EnumIntArrayMember<T>;
                }
            }
        }
        /// <summary>
        /// 获取二进制序列化枚举数组委托
        /// </summary>
        internal override Delegate BinarySerializeEnumArrayDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingType.Int: return (Action<BinarySerializer, T[]>)AutoCSer.BinarySerializer.EnumIntArray<T>;
                    case UnderlyingType.UInt: return (Action<BinarySerializer, T[]>)AutoCSer.BinarySerializer.EnumUIntArray<T>;
                    case UnderlyingType.Byte: return (Action<BinarySerializer, T[]>)AutoCSer.BinarySerializer.EnumByteArray<T>;
                    case UnderlyingType.ULong: return (Action<BinarySerializer, T[]>)AutoCSer.BinarySerializer.EnumULongArray<T>;
                    case UnderlyingType.UShort: return (Action<BinarySerializer, T[]>)AutoCSer.BinarySerializer.EnumUShortArray<T>;
                    case UnderlyingType.Long: return (Action<BinarySerializer, T[]>)AutoCSer.BinarySerializer.EnumLongArray<T>;
                    case UnderlyingType.Short: return (Action<BinarySerializer, T[]>)AutoCSer.BinarySerializer.EnumShortArray<T>;
                    case UnderlyingType.SByte: return (Action<BinarySerializer, T[]>)AutoCSer.BinarySerializer.EnumSByteArray<T>;
                    default: return (Action<BinarySerializer, T[]>)AutoCSer.BinarySerializer.EnumIntArray<T>;
                }
            }
        }
        /// <summary>
        /// 获取二进制反序列化枚举委托
        /// </summary>
        internal override Delegate BinaryDeSerializeEnumDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingType.Int: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T>)AutoCSer.BinaryDeSerializer.EnumIntMember<T>;
                    case UnderlyingType.UInt: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T>)AutoCSer.BinaryDeSerializer.EnumUIntMember<T>;
                    case UnderlyingType.Byte: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T>)AutoCSer.BinaryDeSerializer.EnumByte<T>;
                    case UnderlyingType.ULong: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T>)AutoCSer.BinaryDeSerializer.EnumULongMember<T>;
                    case UnderlyingType.UShort: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T>)AutoCSer.BinaryDeSerializer.EnumUShort<T>;
                    case UnderlyingType.Long: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T>)AutoCSer.BinaryDeSerializer.EnumLongMember<T>;
                    case UnderlyingType.Short: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T>)AutoCSer.BinaryDeSerializer.EnumShort<T>;
                    case UnderlyingType.SByte: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T>)AutoCSer.BinaryDeSerializer.EnumSByte<T>;
                    default: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T>)AutoCSer.BinaryDeSerializer.EnumIntMember<T>;
                }
            }
        }
        /// <summary>
        /// 获取二进制反序列化枚举委托
        /// </summary>
        internal override Delegate BinaryDeSerializeEnumMemberDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingType.Int: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T>)AutoCSer.BinaryDeSerializer.EnumIntMember<T>;
                    case UnderlyingType.UInt: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T>)AutoCSer.BinaryDeSerializer.EnumUIntMember<T>;
                    case UnderlyingType.Byte: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T>)AutoCSer.BinaryDeSerializer.EnumByteMember<T>;
                    case UnderlyingType.ULong: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T>)AutoCSer.BinaryDeSerializer.EnumULongMember<T>;
                    case UnderlyingType.UShort: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T>)AutoCSer.BinaryDeSerializer.EnumUShortMember<T>;
                    case UnderlyingType.Long: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T>)AutoCSer.BinaryDeSerializer.EnumLongMember<T>;
                    case UnderlyingType.Short: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T>)AutoCSer.BinaryDeSerializer.EnumShortMember<T>;
                    case UnderlyingType.SByte: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T>)AutoCSer.BinaryDeSerializer.EnumSByteMember<T>;
                    default: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T>)AutoCSer.BinaryDeSerializer.EnumIntMember<T>;
                }
            }
        }
        /// <summary>
        /// 获取二进制反序列化枚举数组委托
        /// </summary>
        internal override Delegate BinaryDeSerializeEnumArrayDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingType.Int: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)AutoCSer.BinaryDeSerializer.EnumIntArray<T>;
                    case UnderlyingType.UInt: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)AutoCSer.BinaryDeSerializer.EnumUIntArray<T>;
                    case UnderlyingType.Byte: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)AutoCSer.BinaryDeSerializer.EnumByteArray<T>;
                    case UnderlyingType.ULong: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)AutoCSer.BinaryDeSerializer.EnumULongArray<T>;
                    case UnderlyingType.UShort: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)AutoCSer.BinaryDeSerializer.EnumUShortArray<T>;
                    case UnderlyingType.Long: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)AutoCSer.BinaryDeSerializer.EnumLongArray<T>;
                    case UnderlyingType.Short: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)AutoCSer.BinaryDeSerializer.EnumShortArray<T>;
                    case UnderlyingType.SByte: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)AutoCSer.BinaryDeSerializer.EnumSByteArray<T>;
                    default: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)AutoCSer.BinaryDeSerializer.EnumIntArray<T>;
                }
            }
        }
        /// <summary>
        /// 获取二进制反序列化枚举数组委托
        /// </summary>
        internal override Delegate BinaryDeSerializeEnumArrayMemberDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingType.Int: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)AutoCSer.BinaryDeSerializer.EnumIntArrayMember<T>;
                    case UnderlyingType.UInt: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)AutoCSer.BinaryDeSerializer.EnumUIntArrayMember<T>;
                    case UnderlyingType.Byte: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)AutoCSer.BinaryDeSerializer.EnumByteArrayMember<T>;
                    case UnderlyingType.ULong: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)AutoCSer.BinaryDeSerializer.EnumULongArrayMember<T>;
                    case UnderlyingType.UShort: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)AutoCSer.BinaryDeSerializer.EnumUShortArrayMember<T>;
                    case UnderlyingType.Long: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)AutoCSer.BinaryDeSerializer.EnumLongArrayMember<T>;
                    case UnderlyingType.Short: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)AutoCSer.BinaryDeSerializer.EnumShortArrayMember<T>;
                    case UnderlyingType.SByte: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)AutoCSer.BinaryDeSerializer.EnumSByteArrayMember<T>;
                    default: return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)AutoCSer.BinaryDeSerializer.EnumIntArrayMember<T>;
                }
            }
        }

        /// <summary>
        /// 获取 JSON 反序列化枚举委托
        /// </summary>
        internal override Delegate JsonDeSerializeEnumDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingType.Int: return (JsonDeSerializer.DeSerializeDelegate<T>)AutoCSer.Json.EnumIntDeSerialize<T>.DeSerialize;
                    case UnderlyingType.UInt: return (JsonDeSerializer.DeSerializeDelegate<T>)AutoCSer.Json.EnumUIntDeSerialize<T>.DeSerialize;
                    case UnderlyingType.Byte: return (JsonDeSerializer.DeSerializeDelegate<T>)AutoCSer.Json.EnumByteDeSerialize<T>.DeSerialize;
                    case UnderlyingType.ULong: return (JsonDeSerializer.DeSerializeDelegate<T>)AutoCSer.Json.EnumULongDeSerialize<T>.DeSerialize;
                    case UnderlyingType.UShort: return (JsonDeSerializer.DeSerializeDelegate<T>)AutoCSer.Json.EnumUShortDeSerialize<T>.DeSerialize;
                    case UnderlyingType.Long: return (JsonDeSerializer.DeSerializeDelegate<T>)AutoCSer.Json.EnumLongDeSerialize<T>.DeSerialize;
                    case UnderlyingType.Short: return (JsonDeSerializer.DeSerializeDelegate<T>)AutoCSer.Json.EnumShortDeSerialize<T>.DeSerialize;
                    case UnderlyingType.SByte: return (JsonDeSerializer.DeSerializeDelegate<T>)AutoCSer.Json.EnumSByteDeSerialize<T>.DeSerialize;
                    default: return (JsonDeSerializer.DeSerializeDelegate<T>)AutoCSer.Json.EnumIntDeSerialize<T>.DeSerialize;
                }
            }
        }
        /// <summary>
        /// 获取 JSON 反序列化枚举委托
        /// </summary>
        internal override Delegate JsonDeSerializeEnumFlagsDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingType.Int: return (JsonDeSerializer.DeSerializeDelegate<T>)AutoCSer.Json.EnumIntDeSerialize<T>.DeSerializeFlags;
                    case UnderlyingType.UInt: return (JsonDeSerializer.DeSerializeDelegate<T>)AutoCSer.Json.EnumUIntDeSerialize<T>.DeSerializeFlags;
                    case UnderlyingType.Byte: return (JsonDeSerializer.DeSerializeDelegate<T>)AutoCSer.Json.EnumByteDeSerialize<T>.DeSerializeFlags;
                    case UnderlyingType.ULong: return (JsonDeSerializer.DeSerializeDelegate<T>)AutoCSer.Json.EnumULongDeSerialize<T>.DeSerializeFlags;
                    case UnderlyingType.UShort: return (JsonDeSerializer.DeSerializeDelegate<T>)AutoCSer.Json.EnumUShortDeSerialize<T>.DeSerializeFlags;
                    case UnderlyingType.Long: return (JsonDeSerializer.DeSerializeDelegate<T>)AutoCSer.Json.EnumLongDeSerialize<T>.DeSerializeFlags;
                    case UnderlyingType.Short: return (JsonDeSerializer.DeSerializeDelegate<T>)AutoCSer.Json.EnumShortDeSerialize<T>.DeSerializeFlags;
                    case UnderlyingType.SByte: return (JsonDeSerializer.DeSerializeDelegate<T>)AutoCSer.Json.EnumSByteDeSerialize<T>.DeSerializeFlags;
                    default: return (JsonDeSerializer.DeSerializeDelegate<T>)AutoCSer.Json.EnumIntDeSerialize<T>.DeSerializeFlags;
                }
            }
        }

        /// <summary>
        /// 获取 XML 反序列化枚举委托
        /// </summary>
        internal override Delegate XmlDeSerializeEnumDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingType.Int: return (XmlDeSerializer.DeSerializeDelegate<T>)AutoCSer.Xml.EnumIntDeSerialize<T>.DeSerialize;
                    case UnderlyingType.UInt: return (XmlDeSerializer.DeSerializeDelegate<T>)AutoCSer.Xml.EnumUIntDeSerialize<T>.DeSerialize;
                    case UnderlyingType.Byte: return (XmlDeSerializer.DeSerializeDelegate<T>)AutoCSer.Xml.EnumByteDeSerialize<T>.DeSerialize;
                    case UnderlyingType.ULong: return (XmlDeSerializer.DeSerializeDelegate<T>)AutoCSer.Xml.EnumULongDeSerialize<T>.DeSerialize;
                    case UnderlyingType.UShort: return (XmlDeSerializer.DeSerializeDelegate<T>)AutoCSer.Xml.EnumUShortDeSerialize<T>.DeSerialize;
                    case UnderlyingType.Long: return (XmlDeSerializer.DeSerializeDelegate<T>)AutoCSer.Xml.EnumLongDeSerialize<T>.DeSerialize;
                    case UnderlyingType.Short: return (XmlDeSerializer.DeSerializeDelegate<T>)AutoCSer.Xml.EnumShortDeSerialize<T>.DeSerialize;
                    case UnderlyingType.SByte: return (XmlDeSerializer.DeSerializeDelegate<T>)AutoCSer.Xml.EnumSByteDeSerialize<T>.DeSerialize;
                    default: return (XmlDeSerializer.DeSerializeDelegate<T>)AutoCSer.Xml.EnumIntDeSerialize<T>.DeSerialize;
                }
            }
        }
        /// <summary>
        /// 获取 XML 反序列化枚举委托
        /// </summary>
        internal override Delegate XmlDeSerializeEnumFlagsDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingType.Int: return (XmlDeSerializer.DeSerializeDelegate<T>)AutoCSer.Xml.EnumIntDeSerialize<T>.DeSerializeFlags;
                    case UnderlyingType.UInt: return (XmlDeSerializer.DeSerializeDelegate<T>)AutoCSer.Xml.EnumUIntDeSerialize<T>.DeSerializeFlags;
                    case UnderlyingType.Byte: return (XmlDeSerializer.DeSerializeDelegate<T>)AutoCSer.Xml.EnumByteDeSerialize<T>.DeSerializeFlags;
                    case UnderlyingType.ULong: return (XmlDeSerializer.DeSerializeDelegate<T>)AutoCSer.Xml.EnumULongDeSerialize<T>.DeSerializeFlags;
                    case UnderlyingType.UShort: return (XmlDeSerializer.DeSerializeDelegate<T>)AutoCSer.Xml.EnumUShortDeSerialize<T>.DeSerializeFlags;
                    case UnderlyingType.Long: return (XmlDeSerializer.DeSerializeDelegate<T>)AutoCSer.Xml.EnumLongDeSerialize<T>.DeSerializeFlags;
                    case UnderlyingType.Short: return (XmlDeSerializer.DeSerializeDelegate<T>)AutoCSer.Xml.EnumShortDeSerialize<T>.DeSerializeFlags;
                    case UnderlyingType.SByte: return (XmlDeSerializer.DeSerializeDelegate<T>)AutoCSer.Xml.EnumSByteDeSerialize<T>.DeSerializeFlags;
                    default: return (XmlDeSerializer.DeSerializeDelegate<T>)AutoCSer.Xml.EnumIntDeSerialize<T>.DeSerializeFlags;
                }
            }
        }

        /// <summary>
        /// 枚举类型映射基本类型
        /// </summary>
        internal static readonly UnderlyingType UnderlyingType;
#if NOJIT
        /// <summary>
        /// 枚举转数字委托
        /// </summary>
        internal override Delegate ToIntDelegate { get { return (Func<T, UT>)EnumCast<T, UT>.ToInt; } }
        /// <summary>
        /// 数字转枚举委托
        /// </summary>
        internal override Delegate FromIntDelegate { get { return (Func<UT, T>)EnumCast<T, UT>.FromInt; } }
#else
        /// <summary>
        /// 枚举转数字委托
        /// </summary>
        internal static readonly Func<T, UT> ToInt;
        /// <summary>
        /// 类型不匹配的未知转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static UT toIntUnknown(T value) { return default(UT); }
        /// <summary>
        /// 数字转枚举委托
        /// </summary>
        internal static readonly Func<UT, T> FromInt;
        /// <summary>
        /// 类型不匹配的未知转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static T fromIntUnknown(UT value) { return default(T); }
        /// <summary>
        /// 枚举转数字委托
        /// </summary>
        internal override Delegate ToIntDelegate { get { return (Func<T, UT>)ToInt; } }
        /// <summary>
        /// 数字转枚举委托
        /// </summary>
        internal override Delegate FromIntDelegate { get { return (Func<UT, T>)FromInt; } }
#endif
        static EnumGenericType()
        {
            Type type = System.Enum.GetUnderlyingType(typeof(T));
            if (type == typeof(UT))
            {
                if (type == typeof(int)) UnderlyingType = UnderlyingType.Int;
                else if (type == typeof(uint)) UnderlyingType = UnderlyingType.UInt;
                else if (type == typeof(byte)) UnderlyingType = UnderlyingType.Byte;
                else if (type == typeof(ulong)) UnderlyingType = UnderlyingType.ULong;
                else if (type == typeof(ushort)) UnderlyingType = UnderlyingType.UShort;
                else if (type == typeof(long)) UnderlyingType = UnderlyingType.Long;
                else if (type == typeof(short)) UnderlyingType = UnderlyingType.Short;
                else if (type == typeof(sbyte)) UnderlyingType = UnderlyingType.SByte;
                else UnderlyingType = UnderlyingType.Int;

#if !NOJIT
                DynamicMethod toIntDynamicMethod = new DynamicMethod("To" + typeof(UT).FullName, typeof(UT), new Type[] { typeof(T) }, typeof(T), true);
                ILGenerator generator = toIntDynamicMethod.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ret);
                ToInt = (Func<T, UT>)toIntDynamicMethod.CreateDelegate(typeof(Func<T, UT>));

                DynamicMethod fromIntDynamicMethod = new DynamicMethod("From" + typeof(UT).FullName, typeof(T), new Type[] { typeof(UT) }, typeof(T), true);
                generator = fromIntDynamicMethod.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ret);
                FromInt = (Func<UT, T>)fromIntDynamicMethod.CreateDelegate(typeof(Func<UT, T>));
#endif
            }
#if !NOJIT
            else
            {
                ToInt = toIntUnknown;
                FromInt = fromIntUnknown;
            }
#endif
        }
    }
}
