using System;
using AutoCSer.Metadata;
using AutoCSer.Extension;
using System.Reflection;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Sql.DataModel
{
    /// <summary>
    /// 数据列验证动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct Verifyer
    {
        /// <summary>
        /// 动态函数
        /// </summary>
        private readonly DynamicMethod dynamicMethod;
        /// <summary>
        /// 
        /// </summary>
        private readonly ILGenerator generator;
        /// <summary>
        /// 数据库表格模型配置
        /// </summary>
        private readonly ModelAttribute attribute;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        public Verifyer(Type type, ModelAttribute attribute)
        {
            this.attribute = attribute;
            dynamicMethod = new DynamicMethod("SqlModelVerifyer", typeof(bool), new Type[] { type, typeof(MemberMap), typeof(Table) }, type, true);
            generator = dynamicMethod.GetILGenerator();
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(Field field)
        {
            Label end = generator.DefineLabel();
            generator.memberMapIsMember(OpCodes.Ldarg_1, field.MemberMapIndex);
            generator.Emit(OpCodes.Brfalse_S, end);
            MethodInfo sqlVerifyMethod = getVerifyMethod(field.FieldInfo.FieldType);
            if (sqlVerifyMethod != null)
            {
                generator.Emit(OpCodes.Ldarg_0);
                if (field.FieldInfo.FieldType.IsValueType)
                {
                    generator.Emit(OpCodes.Ldflda, field.FieldInfo);
                    generator.Emit(OpCodes.Call, sqlVerifyMethod);
                }
                else
                {
                    generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                    generator.Emit(OpCodes.Brfalse_S, end);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                    generator.Emit(OpCodes.Callvirt, sqlVerifyMethod);
                }
                generator.Emit(OpCodes.Brtrue_S, end);
            }
            else if (field.IsSqlColumn)
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldstr, field.FieldInfo.Name);
                generator.Emit(OpCodes.Call, ColumnGroup.Verifyer.GetTypeVerifyer(field.DataType));
                generator.Emit(OpCodes.Brtrue_S, end);
            }
            else if (field.DataType == typeof(string) || field.IsUnknownJson)
            {
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldstr, field.FieldInfo.Name);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                if (field.ToSqlCastMethod != null) generator.Emit(OpCodes.Call, field.ToSqlCastMethod);
                generator.int32(field.DataMember.MaxStringLength);
                generator.Emit(field.DataMember.IsAscii ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
                generator.Emit(field.DataMember.IsNull || attribute.IsNullStringEmpty ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
                generator.Emit(OpCodes.Callvirt, Table.StringVerifyMethod);
                generator.Emit(OpCodes.Brtrue_S, end);
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                if (field.ToSqlCastMethod != null) generator.Emit(OpCodes.Call, field.ToSqlCastMethod);
                generator.Emit(OpCodes.Brtrue_S, end);
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldstr, field.FieldInfo.Name);
                generator.Emit(OpCodes.Callvirt, Table.NullVerifyMethod);
            }
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Ret);
            generator.MarkLabel(end);
        }
        /// <summary>
        /// 创建web表单委托
        /// </summary>
        /// <returns>web表单委托</returns>
        public Delegate Create<delegateType>()
        {
            generator.Emit(OpCodes.Ldc_I4_1);
            generator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(typeof(delegateType));
        }

        /// <summary>
        /// SQL数据验证函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> verifyMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        /// <summary>
        /// SQL数据验证调用函数信息
        /// </summary>
        /// <param name="type">数组类型</param>
        /// <returns>SQL数据验证调用函数</returns>
        private static MethodInfo getVerifyMethod(Type type)
        {
            if (typeof(IVerify).IsAssignableFrom(type))
            {
                MethodInfo method;
                if (verifyMethods.TryGetValue(type, out method)) return method;
                verifyMethods.Set(type, method = type.GetMethod("IsSqlVeify", BindingFlags.Instance | BindingFlags.Public, null, NullValue<Type>.Array, null));
                return method;
            }
            return null;
        }

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void clearCache(int count)
        {
            verifyMethods.Clear();
        }
        static Verifyer()
        {
            AutoCSer.Pub.ClearCaches += clearCache;
        }
    }
    /// <summary>
    /// 数据模型
    /// </summary>
    internal abstract partial class Model<modelType>
    {
        /// <summary>
        /// 数据列验证
        /// </summary>
        internal static class Verifyer
        {
            /// <summary>
            /// 数据验证
            /// </summary>
            /// <param name="value"></param>
            /// <param name="memberMap"></param>
            /// <param name="sqlTool"></param>
            /// <returns></returns>
#if !NOJIT
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
#endif
            public static bool Verify(modelType value, MemberMap memberMap, Table sqlTool)
            {
#if NOJIT
                if (fields != null)
                {
                    object[] sqlColumnParameters = null, castParameters = null;
                    foreach (sqlModel.verifyField field in fields)
                    {
                        AutoCSer.code.cSharp.sqlModel.fieldInfo fieldInfo = field.Field;
                        if (memberMap.IsMember(fieldInfo.MemberMapIndex))
                        {
                            object memberValue = fieldInfo.Field.GetValue(value);
                            if (field.IsSqlVerify)
                            {
                                if (!fieldInfo.Field.FieldType.IsValueType && memberValue == null) return false;
                                if (!(bool)((AutoCSer.emit.sqlTable.ISqlVerify)memberValue).IsSqlVeify()) return false;
                            }
                            else if (fieldInfo.IsSqlColumn)
                            {
                                if (sqlColumnParameters == null) sqlColumnParameters = new object[] { null, sqlTool, null };
                                sqlColumnParameters[0] = memberValue;
                                sqlColumnParameters[2] = fieldInfo.Field.Name;
                                if (!(bool)field.SqlColumnMethod.Invoke(null, sqlColumnParameters)) return false;
                            }
                            else if (fieldInfo.DataType == typeof(string))
                            {
                                if (fieldInfo.ToSqlCastMethod != null)
                                {
                                    if (castParameters == null) castParameters = new object[1];
                                    castParameters[0] = memberValue;
                                    memberValue = fieldInfo.ToSqlCastMethod.Invoke(null, castParameters);
                                }
                                dataMember dataMember = fieldInfo.DataMember;
                                if (!sqlTool.StringVerify(fieldInfo.Field.Name, (string)memberValue, dataMember.MaxStringLength, dataMember.IsAscii, dataMember.IsNull)) return false;
                            }
                            else
                            {
                                if (fieldInfo.ToSqlCastMethod != null && !fieldInfo.IsUnknownJson)
                                {
                                    if (castParameters == null) castParameters = new object[1];
                                    castParameters[0] = memberValue;
                                    memberValue = fieldInfo.ToSqlCastMethod.Invoke(null, castParameters);
                                }
                                if (memberValue == null)
                                {
                                    sqlTool.NullVerify(fieldInfo.Field.Name);
                                    return false;
                                }
                            }
                        }
                    }
                }
                return true;
#else
                return verifyer == null || verifyer(value, memberMap, sqlTool);
#endif
            }
#if NOJIT
            /// <summary>
            /// 字段集合
            /// </summary>
            private static readonly sqlModel.verifyField[] fields;
            /// <summary>
            /// 是否存在验证数据
            /// </summary>
            internal static bool IsVerifyer
            {
                get { return fields != null; }
            }
#else
            /// <summary>
            /// 数据验证
            /// </summary>
            private static readonly Func<modelType, MemberMap, Table, bool> verifyer;
            /// <summary>
            /// 是否存在验证数据
            /// </summary>
            internal static bool IsVerifyer
            {
                get { return verifyer != null; }
            }
#endif

            static Verifyer()
            {
                if (attribute != null)
                {
                    LeftArray<Field> verifyFields = Fields.getFind(value => value.IsVerify);
                    if (verifyFields.Length != 0)
                    {
#if NOJIT
                        fields = new sqlModel.verifyField[verifyFields.length];
                        int index = 0;
                        foreach (Field member in verifyFields) fields[index++].Set(member);
#else
                        DataModel.Verifyer dynamicMethod = new DataModel.Verifyer(typeof(modelType), attribute);
                        foreach (Field member in verifyFields) dynamicMethod.Push(member);
                        verifyer = (Func<modelType, MemberMap, Table, bool>)dynamicMethod.Create<Func<modelType, MemberMap, Table, bool>>();
#endif
                    }
                }
            }
        }
    }
}
