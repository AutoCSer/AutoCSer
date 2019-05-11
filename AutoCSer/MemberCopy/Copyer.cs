using System;
using AutoCSer.Extension;
using AutoCSer.Metadata;
using System.Reflection;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.MemberCopy
{
    /// <summary>
    /// 成员复制
    /// </summary>
    internal sealed class Copyer
    {
        /// <summary>
        /// 浅复制函数信息
        /// </summary>
        internal static readonly MethodInfo MemberwiseCloneMethod = ((Func<object>)new Copyer().MemberwiseClone).Method;
    }
    /// <summary>
    /// 成员复制
    /// </summary>
    /// <typeparam name="valueType">对象类型</typeparam>
    public static partial class Copyer<valueType>
    {
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <param name="value">目标对象</param>
        /// <param name="readValue">被复制对象</param>
        /// <param name="memberMap">成员位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Copy(ref valueType value, valueType readValue, MemberMap<valueType> memberMap = null)
        {
            if (isValueCopy) value = readValue;
            else if (memberMap == null || memberMap.IsDefault) defaultCopyer(ref value, readValue);
            else if (memberMap.Type.Type == typeof(valueType)) defaultMemberCopyer(ref value, readValue, memberMap);
            else throw new InvalidCastException("成员位图类型不匹配 " + typeof(valueType).fullName() + " != " + memberMap.Type.Type.fullName());
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <param name="value">目标对象</param>
        /// <param name="readValue">被复制对象</param>
        /// <param name="memberMap">成员位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Copy(valueType value, valueType readValue, MemberMap<valueType> memberMap = null)
        {
            if (memberMap == null || memberMap.IsDefault) defaultCopyer(ref value, readValue);
            else if (memberMap.Type.Type == typeof(valueType)) defaultMemberCopyer(ref value, readValue, memberMap);
            else throw new InvalidCastException("成员位图类型不匹配 " + typeof(valueType).fullName() + " != " + memberMap.Type.Type.fullName());
        }
        /// <summary>
        /// 成员复制委托
        /// </summary>
        /// <param name="value"></param>
        /// <param name="copyValue"></param>
        internal delegate void copyer(ref valueType value, valueType copyValue);
        /// <summary>
        /// 成员复制委托
        /// </summary>
        /// <param name="value"></param>
        /// <param name="copyValue"></param>
        /// <param name="memberMap">成员位图</param>
        internal delegate void memberMapCopyer(ref valueType value, valueType copyValue, MemberMap memberMap);
        /// <summary>
        /// 是否采用值类型复制模式
        /// </summary>
        private static readonly bool isValueCopy;
        /// <summary>
        /// 默认成员复制委托
        /// </summary>
        private static readonly copyer defaultCopyer;
        /// <summary>
        /// 默认成员复制委托
        /// </summary>
        private static readonly memberMapCopyer defaultMemberCopyer;
        /// <summary>
        /// 数组复制
        /// </summary>
        /// <param name="value"></param>
        /// <param name="readValue"></param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static void copyArray(ref valueType[] value, valueType[] readValue)
        {
            if (readValue != null)
            {
                if (readValue.Length == 0)
                {
                    if (value == null) value = NullValue<valueType>.Array;
                    return;
                }
                if (value == null || value.Length < readValue.Length) System.Array.Resize(ref value, readValue.Length);
                System.Array.Copy(readValue, 0, value, 0, readValue.Length);
            }
        }
        /// <summary>
        /// 数组复制
        /// </summary>
        /// <param name="value"></param>
        /// <param name="readValue"></param>
        /// <param name="memberMap"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static void copyArray(ref valueType[] value, valueType[] readValue, MemberMap memberMap)
        {
            copyArray(ref value, readValue);
        }
        /// <summary>
        /// 自定义复制函数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="readValue"></param>
        private static void customCopy(ref valueType value, valueType readValue)
        {
            defaultMemberCopyer(ref value, readValue, null);
        }
        /// <summary>
        /// 没有复制字段
        /// </summary>
        /// <param name="value"></param>
        /// <param name="readValue"></param>
        private static void noCopy(ref valueType value, valueType readValue)
        {
        }
        /// <summary>
        /// 没有复制字段
        /// </summary>
        /// <param name="value"></param>
        /// <param name="readValue"></param>
        /// <param name="memberMap"></param>
        private static void noCopy(ref valueType value, valueType readValue, MemberMap memberMap)
        {
        }
        /// <summary>
        /// 对象浅复制
        /// </summary>
        private static readonly Func<valueType, object> memberwiseClone;
        /// <summary>
        /// 对象浅复制
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType MemberwiseClone(valueType value)
        {
            return memberwiseClone != null ? (valueType)memberwiseClone(value) : value;
        }

        static Copyer()
        {
            Type type = typeof(valueType), refType = type.MakeByRefType();
            //if (!type.IsValueType) memberwiseClone = (Func<valueType, object>)Delegate.CreateDelegate(typeof(Func<valueType, object>), typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic));
            if (!type.IsValueType) memberwiseClone = (Func<valueType, object>)Delegate.CreateDelegate(typeof(Func<valueType, object>), Copyer.MemberwiseCloneMethod);
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    //Type copyerType = typeof(Copyer<>).MakeGenericType(type.GetElementType());
                    //defaultCopyer = (copyer)Delegate.CreateDelegate(typeof(copyer), copyerType.GetMethod("copyArray", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { refType, type }, null));
                    //defaultMemberCopyer = (memberMapCopyer)Delegate.CreateDelegate(typeof(memberMapCopyer), copyerType.GetMethod("copyArray", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { refType, type, typeof(MemberMap) }, null));
                    AutoCSer.Metadata.GenericType GenericType = AutoCSer.Metadata.GenericType.Get(type.GetElementType());
                    defaultCopyer = (copyer)GenericType.MemberCopyArrayDelegate;
                    defaultMemberCopyer = (memberMapCopyer)GenericType.MemberMapCopyArrayDelegate;
                    return;
                }
                defaultCopyer = noCopy;
                defaultMemberCopyer = noCopy;
                return;
            }
            if (type.IsEnum || type.IsPointer || type.IsInterface || typeof(Delegate).IsAssignableFrom(type))
            {
                isValueCopy = true;
                return;
            }
            foreach (AutoCSer.Metadata.AttributeMethod methodInfo in AutoCSer.Metadata.AttributeMethod.GetStatic(type))
            {
                if (methodInfo.Method.ReturnType == typeof(void))
                {
                    ParameterInfo[] parameters = methodInfo.Method.GetParameters();
                    if (parameters.Length == 3 && parameters[0].ParameterType == refType && parameters[1].ParameterType == type && parameters[2].ParameterType == typeof(MemberMap))
                    {
                        if (methodInfo.GetAttribute<CustomAttribute>() != null)
                        {
                            defaultCopyer = customCopy;
                            defaultMemberCopyer = (memberMapCopyer)Delegate.CreateDelegate(typeof(memberMapCopyer), methodInfo.Method);
                            return;
                        }
                    }
                }
            }
            FieldIndex[] fields = MemberIndexGroup<valueType>.GetFields();
            if (fields.Length == 0)
            {
                defaultCopyer = noCopy;
                defaultMemberCopyer = noCopy;
                return;
            }
#if NOJIT
            defaultCopyer = new FieldCopyer(fields).Copyer();
            defaultMemberCopyer = new MemberMapFieldCopyer(fields).Copyer();
#else
            MemberDynamicMethod dynamicMethod = new MemberDynamicMethod(type, new DynamicMethod("MemberCopyer", null, new Type[] { refType, type }, type, true));
            MemberDynamicMethod memberMapDynamicMethod = new MemberDynamicMethod(type, new DynamicMethod("MemberMapCopyer", null, new Type[] { refType, type, typeof(MemberMap) }, type, true));
            foreach (FieldIndex field in fields)
            {
                dynamicMethod.Push(field);
                memberMapDynamicMethod.PushMemberMap(field);
            }
            defaultCopyer = (copyer)dynamicMethod.Create<copyer>();
            defaultMemberCopyer = (memberMapCopyer)memberMapDynamicMethod.Create<memberMapCopyer>();
#endif
        }
    }
}
