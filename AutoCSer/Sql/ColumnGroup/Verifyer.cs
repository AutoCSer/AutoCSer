using System;
using AutoCSer.Extension;
using System.Reflection;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Sql.ColumnGroup
{
    /// <summary>
    /// 数据列验证动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct Verifyer
    {
#if NOJIT
            /// <summary>
            /// 字段信息（反射模式）
            /// </summary>
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct field
            {
                /// <summary>
                /// 字段信息
                /// </summary>
                public AutoCSer.code.cSharp.sqlModel.fieldInfo Field;
                /// <summary>
                /// SQL列验证
                /// </summary>
                public MethodInfo SqlColumnMethod;
                /// <summary>
                /// 设置字段信息
                /// </summary>
                /// <param name="field"></param>
                public void Set(AutoCSer.code.cSharp.sqlModel.fieldInfo field)
                {
                    Field = field;
                    if (field.IsSqlColumn) SqlColumnMethod = sqlColumn.verifyDynamicMethod.GetTypeVerifyer(field.DataType);
                }
            }
#else
        /// <summary>
        /// 动态函数
        /// </summary>
        private readonly DynamicMethod dynamicMethod;
        /// <summary>
        /// 
        /// </summary>
        private readonly ILGenerator generator;
        /// <summary>
        /// 数据列配置
        /// </summary>
        private readonly ColumnAttribute attribute;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        public Verifyer(Type type, ColumnAttribute attribute)
        {
            this.attribute = attribute;
            dynamicMethod = new DynamicMethod("SqlColumnVerifyer", typeof(bool), new Type[] { type, typeof(Table), typeof(string[]) }, type, true);
            generator = dynamicMethod.GetILGenerator();
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="index">名称序号</param>
        public void Push(Field field, int index)
        {
            Label end = generator.DefineLabel();
            if (field.IsSqlColumn)
            {
                generator.Emit(OpCodes.Ldarga_S, 0);
                generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldarg_2);
                generator.int32(index);
                generator.Emit(OpCodes.Ldelem_Ref);
                generator.Emit(OpCodes.Call, GetTypeVerifyer(field.DataType));
                generator.Emit(OpCodes.Brtrue_S, end);
            }
            else if (field.DataType == typeof(string) || field.IsUnknownJson)
            {
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldarg_2);
                generator.int32(index);
                generator.Emit(OpCodes.Ldelem_Ref);
                generator.Emit(OpCodes.Ldarga_S, 0);
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
                generator.Emit(OpCodes.Ldarga_S, 0);
                generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                if (field.ToSqlCastMethod != null) generator.Emit(OpCodes.Call, field.ToSqlCastMethod);
                generator.Emit(OpCodes.Brtrue_S, end);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldarg_2);
                generator.int32(index);
                generator.Emit(OpCodes.Ldelem_Ref);
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
#endif
        /// <summary>
        /// 类型调用函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockLastDictionary<Type, MethodInfo> typeVerifyers = new AutoCSer.Threading.LockLastDictionary<Type, MethodInfo>();
        /// <summary>
        /// 类型委托调用函数信息
        /// </summary>
        /// <param name="type">数组类型</param>
        /// <returns>类型委托调用函数信息</returns>
        public static MethodInfo GetTypeVerifyer(Type type)
        {
            MethodInfo method;
            if (typeVerifyers.TryGetValue(type, out method)) return method;
            typeVerifyers.Set(type, method = verifyMethod.MakeGenericMethod(type));
            return method;
        }
        /// <summary>
        /// 数据验证
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sqlTool"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static bool verify<valueType>(valueType value, Table sqlTool, string columnName)
        {
            return Column<valueType>.Verifyer.Verify(value, sqlTool, columnName);
        }
        /// <summary>
        /// 数据验证函数信息
        /// </summary>
        private static readonly MethodInfo verifyMethod = typeof(Verifyer).GetMethod("verify", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取列名委托
        /// </summary>
        /// <param name="names"></param>
        /// <param name="name"></param>
        internal delegate void GetName(ref LeftArray<string> names, string name);
        /// <summary>
        /// 获取列名委托集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockLastDictionary<Type, GetName> getColumnNameMethods = new AutoCSer.Threading.LockLastDictionary<Type, GetName>();
        /// <summary>
        /// 获取列名委托
        /// </summary>
        /// <param name="type">数据列类型</param>
        /// <returns>获取列名委托</returns>
        public static GetName GetColumnNames(Type type)
        {
            GetName getColumnName;
            if (getColumnNameMethods.TryGetValue(type, out getColumnName)) return getColumnName;
            getColumnName = (GetName)Delegate.CreateDelegate(typeof(GetName), getColumnNamesMethod.MakeGenericMethod(type));
            getColumnNameMethods.Set(type, getColumnName);
            return getColumnName;
        }
        /// <summary>
        /// 获取列名集合
        /// </summary>
        /// <param name="names">列名集合</param>
        /// <param name="name">列名前缀</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void getColumnNames<valueType>(ref LeftArray<string> names, string name)
        {
            names.Add(Column<valueType>.Verifyer.GetColumnNames(name));
        }
        /// <summary>
        /// 获取列名集合函数信息
        /// </summary>
        private static readonly MethodInfo getColumnNamesMethod = typeof(Verifyer).GetMethod("getColumnNames", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void clearCache(int count)
        {
            typeVerifyers.Clear();
            getColumnNameMethods.Clear();
        }
        static Verifyer()
        {
            AutoCSer.Pub.ClearCaches += clearCache;
        }
    }
    /// <summary>
    /// 数据列
    /// </summary>
    internal static partial class Column<valueType>
    {
        /// <summary>
        /// 数据列验证
        /// </summary>
        internal static class Verifyer
        {
            /// <summary>
            /// 数据列名集合
            /// </summary>
            private static readonly AutoCSer.Threading.LockEquatableLastDictionary<HashString, string[]> columnNames;
            /// <summary>
            /// 获取列名集合
            /// </summary>
            /// <param name="name">列名前缀</param>
            /// <returns></returns>
            internal static string[] GetColumnNames(string name)
            {
                string[] names;
                HashString nameKey = name;
                if (columnNames.TryGetValue(ref nameKey, out names)) return names;
                LeftArray<string> nameList = new LeftArray<string>(verifyFields.Length);
                foreach (Field field in verifyFields)
                {
                    if (field.IsSqlColumn) ColumnGroup.Verifyer.GetColumnNames(field.FieldInfo.FieldType)(ref nameList, name + "_" + field.FieldInfo.Name);
                    else nameList.Add(name + "_" + field.FieldInfo.Name);
                }
                columnNames.Set(ref nameKey, names = nameList.ToArray());
                return names;
            }
            /// <summary>
            /// 清除缓存数据
            /// </summary>
            /// <param name="count">保留缓存数据数量</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            private static void clearCache(int count)
            {
                columnNames.Clear();
            }

            /// <summary>
            /// 字段集合
            /// </summary>
            private static readonly Field[] verifyFields;
            /// <summary>
            /// 数据验证
            /// </summary>
            /// <param name="value"></param>
            /// <param name="sqlTool"></param>
            /// <param name="columnName"></param>
            /// <returns></returns>
#if !NOJIT
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
#endif
            public static bool Verify(valueType value, Table sqlTool, string columnName)
            {
#if NOJIT
                if (fields != null)
                {
                    string[] columnNames = GetColumnNames(columnName);
                    object[] sqlColumnParameters = null, castParameters = null;
                    object objectValue = value;
                    int index = 0;
                    foreach (sqlColumn.verifyDynamicMethod.field field in fields)
                    {
                        AutoCSer.code.cSharp.sqlModel.fieldInfo fieldInfo = field.Field;
                        object memberValue = fieldInfo.Field.GetValue(objectValue);
                        if (fieldInfo.IsSqlColumn)
                        {
                            if (sqlColumnParameters == null) sqlColumnParameters = new object[] { null, sqlTool, null };
                            sqlColumnParameters[0] = memberValue;
                            sqlColumnParameters[2] = columnNames[index];
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
                            if (!sqlTool.StringVerify(columnNames[index], (string)memberValue, dataMember.MaxStringLength, dataMember.IsAscii, dataMember.IsNull)) return false;
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
                                sqlTool.NullVerify(columnNames[index]);
                                return false;
                            }
                        }
                        ++index;
                    }
                    return true;
                }
#else
                if (verifyer != null) return verifyer(value, sqlTool, GetColumnNames(columnName));
#endif
                return custom == null || custom.Verify(value, sqlTool, columnName);
            }
#if NOJIT
            /// <summary>
            /// 字段集合
            /// </summary>
            [AutoCSer.IOS.Preserve(Conditional = true)]
            private static readonly sqlColumn.verifyDynamicMethod.field[] fields;
#else
            /// <summary>
            /// 数据验证
            /// </summary>
            [AutoCSer.IOS.Preserve(Conditional = true)]
            private static readonly Func<valueType, Table, string[], bool> verifyer;
#endif
            static Verifyer()
            {
                if (attribute != null && custom == null && Fields != null)
                {
                    LeftArray<Field> verifyFields = Fields.getFind(value => value.IsVerify);
                    if (verifyFields.Length != 0)
                    {
                        columnNames = new AutoCSer.Threading.LockEquatableLastDictionary<HashString, string[]>();
                        int index = 0;
                        Verifyer.verifyFields = verifyFields.ToArray();
#if NOJIT
                        fields = new sqlColumn.verifyDynamicMethod.field[verifyFields.length];
                        foreach (AutoCSer.code.cSharp.sqlModel.fieldInfo member in verify.verifyFields) fields[index++].Set(member);
#else
                        ColumnGroup.Verifyer dynamicMethod = new ColumnGroup.Verifyer(typeof(valueType), attribute);
                        foreach (Field member in Verifyer.verifyFields) dynamicMethod.Push(member, index++);
                        verifyer = (Func<valueType, Table, string[], bool>)dynamicMethod.Create<Func<valueType, Table, string[], bool>>();
#endif
                        AutoCSer.Pub.ClearCaches += clearCache;
                    }
                }
            }
        }
    }
}
