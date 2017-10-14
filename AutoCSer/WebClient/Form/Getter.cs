using System;
using System.Collections.Specialized;
using System.Reflection;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.WebClient.Form
{
    /// <summary>
    /// WEB表单生成
    /// </summary>
    /// <typeparam name="valueType">对象类型</typeparam>
    public static class Getter<valueType>
    {
        /// <summary>
        /// web表单生成委托
        /// </summary>
        private static readonly Action<valueType, NameValueCollection> getter;
        /// <summary>
        /// 成员数量
        /// </summary>
        private static readonly int memberCount;
        /// <summary>
        /// 获取POST表单
        /// </summary>
        /// <param name="value">查询对象</param>
        /// <returns>POST表单</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static NameValueCollection Get(valueType value)
        {
            if (getter != null)
            {
                NameValueCollection form = new NameValueCollection(memberCount);
                getter(value, form);
                return form;
            }
            return null;
        }
        /// <summary>
        /// 获取POST表单
        /// </summary>
        /// <param name="value">查询对象</param>
        /// <param name="form">POST表单</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Get(valueType value, NameValueCollection form)
        {
            if (getter != null)
            {
                if (form == null) throw new ArgumentNullException();
                getter(value, form);
            }
        }

        static Getter()
        {
            Type type = typeof(valueType);
            if (type.IsArray || type.IsEnum || type.IsPointer || type.IsInterface || typeof(Delegate).IsAssignableFrom(type)) return;
            foreach (AttributeMethod methodInfo in AttributeMethod.GetStatic(type))
            {
                if (methodInfo.Method.ReturnType == typeof(void))
                {
                    ParameterInfo[] parameters = methodInfo.Method.GetParameters();
                    if (parameters.Length == 2 && parameters[0].ParameterType == type && parameters[1].ParameterType == typeof(NameValueCollection))
                    {
                        if (methodInfo.GetAttribute<Form.CustomAttribute>() != null)
                        {
                            getter = (Action<valueType, NameValueCollection>)Delegate.CreateDelegate(typeof(Action<valueType, NameValueCollection>), methodInfo.Method);
                            return;
                        }
                    }
                }
            }
            Form.FormAttribute attribute = TypeAttribute.GetAttribute<Form.FormAttribute>(type, true) ?? Form.FormAttribute.AllMember;
            LeftArray<FieldIndex> fields = Emit.Pub.GetFields<valueType, MemberAttribute>(attribute.Filter, attribute.IsAllMember);
            if ((memberCount = fields.Length) != 0)
            {
#if NOJIT
                getter = new MemberGetter(ref fields).Get;
#else
                GetterDynamicMethod dynamicMethod = new GetterDynamicMethod(type);
                foreach (FieldIndex member in fields) dynamicMethod.Push(member.Member);
                getter = (Action<valueType, NameValueCollection>)dynamicMethod.Create<Action<valueType, NameValueCollection>>();
#endif
            }
        }
#if NOJIT
        /// <summary>
        /// WEB表单生成
        /// </summary>
        private sealed class MemberGetter
        {
            /// <summary>
            /// 字段信息
            /// </summary>
            [StructLayout(LayoutKind.Auto)]
            private struct field
            {
                /// <summary>
                /// 字段信息
                /// </summary>
                public FieldInfo Field;
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
                /// 设置字段信息
                /// </summary>
                /// <param name="field"></param>
                public void Set(FieldInfo field)
                {
                    Field = field;
                    Type type = field.FieldType;
                    if (type.IsValueType)
                    {
                        IsValueType = true;
                        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            NullableHasValueMethod = AutoCSer.Emit.Pub.GetNullableHasValue(type);
                            NullableValueMethod = AutoCSer.Emit.Pub.GetNullableValue(type);
                        }
                    }
                }
            }
            /// <summary>
            /// 字段集合
            /// </summary>
            private field[] fields;
            /// <summary>
            /// WEB表单生成
            /// </summary>
            /// <param name="fields"></param>
            public MemberGetter(ref LeftArray<FieldIndex> fields)
            {
                this.fields = new field[fields.Count];
                int index = 0;
                foreach (FieldIndex field in fields) this.fields[index++].Set(field.Member);
            }
            /// <summary>
            /// WEB表单生成
            /// </summary>
            /// <param name="value"></param>
            /// <param name="form"></param>
            public void Get(valueType value, NameValueCollection form)
            {
                object objectValuee = value;
                foreach (field field in fields)
                {
                    object fieldValue = field.Field.GetValue(objectValuee);
                    if (field.IsValueType)
                    {
                        if (field.NullableValueMethod == null) form.Add(field.Field.Name, fieldValue.ToString());
                        else if ((bool)field.NullableHasValueMethod.Invoke(fieldValue, null)) form.Add(field.Field.Name, field.NullableValueMethod.Invoke(fieldValue, null).ToString());
                    }
                    else if (fieldValue != null) form.Add(field.Field.Name, fieldValue.ToString());
                }
            }
        }
#endif
    }
}
