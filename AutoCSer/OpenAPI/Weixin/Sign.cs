using System;
using System.Reflection;
using System.Security.Cryptography;
using AutoCSer.Extension;
using AutoCSer.Metadata;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 签名计算 https://pay.weixin.qq.com/wiki/tools/signverify/
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    internal static class Sign<valueType> where valueType : class
    {
        /// <summary>
        /// 获取数据到数据缓冲区
        /// </summary>
        private static Action<valueType, string[]> valueGetter;
        /// <summary>
        /// 设置签名
        /// </summary>
        private static Action<valueType, string> setSign;
        /// <summary>
        /// 签名名称集合
        /// </summary>
        private static readonly string[] names;
        /// <summary>
        /// 签名数据是否需要Utf8编码
        /// </summary>
        private static Pointer isUtf8;
        /// <summary>
        /// 数据缓冲区池索引编号
        /// </summary>
        private static readonly int memberCount;
        /// <summary>
        /// 签名计算
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key">签名密钥</param>
        internal unsafe static void Set(valueType value, string key)
        {
            if (valueGetter == null) throw new ArgumentNullException();
            if (value == null) throw new ArgumentNullException("value is null");
            string[] values = new string[memberCount];
            int length = 4 + key.Length + getLength(value, values);
            AutoCSer.SubBuffer.PoolBufferFull buffer = default(AutoCSer.SubBuffer.PoolBufferFull);
            AutoCSer.SubBuffer.Pool.GetBuffer(ref buffer, length);
            try
            {
                concat(values, ref buffer, length, key);
                using (MD5 md5 = new MD5CryptoServiceProvider()) setSign(value, md5.ComputeHash(buffer.Buffer, buffer.StartIndex, length).toUpperHex());
            }
            finally { buffer.Free(); }
        }
        /// <summary>
        /// 签名验证
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        internal unsafe static bool Check(valueType value, string key, string sign)
        {
            if (valueGetter == null) throw new ArgumentNullException();
            if (value == null) throw new ArgumentNullException("value is null");
            if (sign.length() == 32)
            {
                string[] values = new string[memberCount];
                int length = 4 + key.Length + getLength(value, values);
                AutoCSer.SubBuffer.PoolBufferFull buffer = default(AutoCSer.SubBuffer.PoolBufferFull);
                AutoCSer.SubBuffer.Pool.GetBuffer(ref buffer, length);
                try
                {
                    concat(values, ref buffer, length, key);
                    using (MD5 md5 = new MD5CryptoServiceProvider())
                    {
                        if (md5.ComputeHash(buffer.Buffer, buffer.StartIndex, length).checkUpperHexNotNull(sign)) return true;
                    }
                }
                finally { buffer.Free(); }
            }
            return false;
        }
        /// <summary>
        /// 获取签名计算数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="buffer"></param>
        /// <returns>数据长度</returns>
        internal unsafe static int GetData(valueType value, string key, ref AutoCSer.SubBuffer.PoolBufferFull buffer)
        {
            if (valueGetter == null) throw new ArgumentNullException();
            if (value == null) throw new ArgumentNullException("value is null");
            string[] values = new string[memberCount];
            int length = 4 + key.Length + getLength(value, values);
            AutoCSer.SubBuffer.Pool.GetBuffer(ref buffer, length);
            concat(values, ref buffer, length, key);
            using (MD5 md5 = new MD5CryptoServiceProvider()) setSign(value, md5.ComputeHash(buffer.Buffer, buffer.StartIndex, length).toUpperHex());
            return length - key.Length - 5;
        }
        /// <summary>
        /// 计算编码数据长度
        /// </summary>
        /// <param name="value"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private unsafe static int getLength(valueType value, string[] values)
        {
            valueGetter(value, values);
            int length = 0, index = 0;
            MemoryMap utf8Map = new MemoryMap(isUtf8.Byte);
            foreach (string name in names)
            {
                string valueString = values[index];
                if (!string.IsNullOrEmpty(valueString))
                {
                    length += 2 + name.Length + (utf8Map.Get(index) == 0 ? valueString.Length : System.Text.Encoding.UTF8.GetByteCount(valueString));
                }
                ++index;
            }
            return length;
        }
        /// <summary>
        /// 字节拼接
        /// </summary>
        /// <param name="values"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <param name="key"></param>
        private unsafe static void concat(string[] values, ref SubBuffer.PoolBufferFull buffer, int length, string key)
        {
            fixed (byte* bufferFixed = buffer.Buffer)
            {
                byte* write = bufferFixed + buffer.StartIndex, end = write + length;
                int isValue = 0, index = 0;
                MemoryMap utf8Map = new MemoryMap(isUtf8.Byte);
                foreach (string name in names)
                {
                    string valueString = values[index];
                    if (!string.IsNullOrEmpty(valueString))
                    {
                        if (isValue == 0) isValue = 1;
                        else *write++ = (byte)'&';
                        fixed (char* nameFixed = name) AutoCSer.Extension.StringExtension.WriteBytesNotNull(nameFixed, name.Length, write);
                        write += name.Length;
                        *write++ = (byte)'=';
                        fixed (char* valueFixed = valueString)
                        {
                            if (utf8Map.Get(index) == 0)
                            {
                                AutoCSer.Extension.StringExtension.WriteBytesNotNull(valueFixed, valueString.Length, write);
                                write += valueString.Length;
                            }
                            else write += System.Text.Encoding.UTF8.GetBytes(valueFixed, valueString.Length, write, (int)(end - write));
                        }
                    }
                    ++index;
                }
                if (isValue != 0) *write++ = (byte)'&';
                *(int*)write = 'k' + ('e' << 8) + ('y' << 16) + ('=' << 24);
                write += sizeof(int);
                fixed (char* keyFixed = key) AutoCSer.Extension.StringExtension.WriteBytesNotNull(keyFixed, key.Length, write);
            }
        }
        /// <summary>
        /// 没有成员
        /// </summary>
        /// <param name="value"></param>
        /// <param name="values"></param>
        private static void empty(valueType value, string[] values)
        {
        }
        unsafe static Sign()
        {
            Type type = typeof(valueType);
            if (type.IsArray || type.IsEnum || type.IsPointer || type.IsInterface) return;
            SignAttribute attribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<SignAttribute>(type, true) ?? SignAttribute.AllMember;
            LeftArray<FieldIndex> fields = AutoCSer.Net.WebClient.Emit.Pub.GetFields<valueType, SignMemberAttribute>(attribute.Filter, attribute.IsAllMember);
            LeftArray<PropertyIndex> properties = Emit.Pub.GetProperties<valueType, SignMemberAttribute>(attribute.Filter, attribute.IsAllMember, true, false);
            int count = fields.Length + properties.Length - 1;
            if (count < 0) return;
            LeftArray<KeyValue<FieldIndex, PropertyIndex>> members = new LeftArray<KeyValue<FieldIndex, PropertyIndex>>(count);
            FieldInfo signField = null;
            PropertyInfo signProperty = null;
            foreach (FieldIndex field in fields)
            {
                if (field.Member.Name == "sign")
                {
                    if (field.Member.FieldType != typeof(string)) return;
                    signField = field.Member;
                }
                else members.UnsafeAdd(new KeyValue<FieldIndex, PropertyIndex>(field, null));
            }
            foreach (PropertyIndex property in properties)
            {
                if (property.Member.Name == "sign")
                {
                    if (property.Member.PropertyType != typeof(string)) return;
                    signProperty = property.Member;
                }
                else members.UnsafeAdd(new KeyValue<FieldIndex, PropertyIndex>(null, property));
            }
            if ((signField == null) ^ (signProperty == null))
            {
                setSign = signField == null ? Emit.Property.SetProperty<valueType, string>(signProperty) : AutoCSer.Emit.Field.UnsafeSetField<valueType, string>(signField);
                if (count == 0)
                {
                    names = NullValue<string>.Array;
                    valueGetter = empty;
                }
                else
                {
                    members.Array.sort(SignAttribute.NameCompare);
                    names = new string[memberCount = count];
                    isUtf8 = new Pointer { Data = Unmanaged.GetStatic(((count + 31) >> 5) << 2, true) };
#if NOJIT
                    signer signer = new signer(members.Array.Length);
#else
                    SignDynamicMethod dynamicMethod = new SignDynamicMethod(type);
#endif
                    MemoryMap utf8Map = new MemoryMap(isUtf8.Byte);
                    count = 0;
                    foreach (KeyValue<FieldIndex, PropertyIndex> member in members.Array)
                    {
#if NOJIT
                        if (member.Key == null ? signer.Push(member.Value, count) : signer.Push(member.Key, count)) utf8Map.Set(count);
#else
                        if (member.Key == null ? dynamicMethod.Push(member.Value, count) : dynamicMethod.Push(member.Key, count)) utf8Map.Set(count);
#endif
                        names[count++] = member.Key == null ? member.Value.Member.Name : member.Key.Member.Name;
                    }
#if NOJIT
                    valueGetter = signer.Sign;
#else
                    valueGetter = (Action<valueType, string[]>)dynamicMethod.Create<Action<valueType, string[]>>();
#endif
                }
            }
        }
#if NOJIT
        /// <summary>
        /// 签名计算
        /// </summary>
        private sealed class signer
        {
            /// <summary>
            /// 成员
            /// </summary>
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            private struct member
            {
                /// <summary>
                /// 字段信息
                /// </summary>
                public FieldInfo Field;
                /// <summary>
                /// 获取属性函数信息
                /// </summary>
                public MethodInfo GetProperty;
                /// <summary>
                /// 可空类型判断是否存在值
                /// </summary>
                public MethodInfo NullableHasValueMethod;
                /// <summary>
                /// 获取可空类型数据
                /// </summary>
                public MethodInfo NullableValueMethod;
                /// <summary>
                /// 是否值类型
                /// </summary>
                public bool IsValueType;
                /// <summary>
                /// 是否字符串
                /// </summary>
                public bool IsString;
                /// <summary>
                /// 设置字段信息
                /// </summary>
                /// <param name="field"></param>
                /// <returns></returns>
                public bool Set(FieldIndex field)
                {
                    Field = field.Member;
                    bool? isUtf8 = set(Field.FieldType);
                    if (isUtf8 == null) return (field.GetAttribute<SignMemberAttribute>(false) ?? SignMemberAttribute.Default).IsEncodeUtf8;
                    return isUtf8.Value;
                }
                /// <summary>
                /// 设置属性信息
                /// </summary>
                /// <param name="property"></param>
                /// <returns></returns>
                public bool Set(PropertyIndex property)
                {
                    GetProperty = property.Member.GetGetMethod(true);
                    bool? isUtf8 = set(property.Member.PropertyType);
                    if(isUtf8 == null) return (property.GetAttribute<SignMemberAttribute>(false) ?? SignMemberAttribute.Default).IsEncodeUtf8;
                    return isUtf8.Value;
                }
                /// <summary>
                /// 设置类型信息
                /// </summary>
                /// <param name="type"></param>
                /// <returns></returns>
                private bool? set(Type type)
                {
                    if (type.IsValueType)
                    {
                        IsValueType = true;
                        MethodInfo numberToStringMethod;
                        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            NullableHasValueMethod = AutoCSer.Emit.Pub.GetNullableHasValue(type);
                            NullableValueMethod = AutoCSer.Emit.Pub.GetNullableValue(type);
                            numberToStringMethod = AutoCSer.Net.WebClient.Emit.Pub.GetNumberToStringMethod(type.GetGenericArguments()[0]);
                        }
                        else numberToStringMethod = AutoCSer.Net.WebClient.Emit.Pub.GetNumberToStringMethod(type);
                        if (numberToStringMethod != null) return false;
                    }
                    else if (type == typeof(string)) IsString = true;
                    return null;
                }
            }
            /// <summary>
            /// 成员集合
            /// </summary>
            private member[] members;
            /// <summary>
            /// 签名计算
            /// </summary>
            /// <param name="count"></param>
            public signer(int count)
            {
                members = new member[count];
            }
            /// <summary>
            /// 添加字段信息
            /// </summary>
            /// <param name="field"></param>
            /// <param name="index"></param>
            /// <returns></returns>
            public bool Push(FieldIndex field, int index)
            {
                return members[index].Set(field);
            }
            /// <summary>
            /// 添加属性信息
            /// </summary>
            /// <param name="property"></param>
            /// <param name="index"></param>
            /// <returns></returns>
            public bool Push(PropertyIndex property, int index)
            {
                return members[index].Set(property);
            }
            /// <summary>
            /// 签名计算
            /// </summary>
            /// <param name="value"></param>
            /// <param name="values"></param>
            public void Sign(valueType value, string[] values)
            {
                int index = 0;
                foreach (member member in members)
                {
                    object memberValue = member.Field == null ? member.GetProperty.Invoke(value, null) : member.Field.GetValue(value);
                    if (member.IsValueType)
                    {
                        if (member.NullableHasValueMethod == null) values[index] = memberValue.ToString();
                        else if ((bool)member.NullableHasValueMethod.Invoke(value, null)) values[index] = member.NullableValueMethod.Invoke(memberValue, null).ToString();
                    }
                    else
                    {
                        if (member.IsString) values[index] = (string)memberValue;
                        else if (memberValue != null) values[index] = memberValue.ToString();
                    }
                    ++index;
                }
            }
        }
#endif
    }
}
