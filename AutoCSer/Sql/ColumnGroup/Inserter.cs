using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Sql.ColumnGroup
{
    /// <summary>
    /// 数据列添加SQL流动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct Inserter
    {
#if !NOJIT
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
        /// 
        /// </summary>
        private bool isNextMember;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        public Inserter(Type type, ColumnAttribute attribute)
        {
            this.attribute = attribute;
            dynamicMethod = new DynamicMethod("SqlColumnInserter", null, new Type[] { typeof(CharStream), type, typeof(ConstantConverter) }, type, true);
            generator = dynamicMethod.GetILGenerator();
            isNextMember = false;
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(Field field)
        {
            if (isNextMember) generator.charStreamWriteChar(OpCodes.Ldarg_0, ',');
            else isNextMember = true;
            if (field.IsSqlColumn)
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarga_S, 1);
                generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Call, GetTypeInsert(field.DataType));
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarga_S, 1);
                generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                if (field.ToSqlCastMethod != null) generator.Emit(OpCodes.Call, field.ToSqlCastMethod);
                if (attribute.IsNullStringEmpty && field.DataType == typeof(string)) generator.nullStringEmpty();
                generator.Emit(OpCodes.Callvirt, field.ToSqlMethod);
            }
        }
        /// <summary>
        /// 创建web表单委托
        /// </summary>
        /// <returns>web表单委托</returns>
        public Delegate Create<delegateType>()
        {
            generator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(typeof(delegateType));
        }
#endif
        /// <summary>
        /// 类型调用函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> typeInserts = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        /// <summary>
        /// 类型委托调用函数信息
        /// </summary>
        /// <param name="type">数组类型</param>
        /// <returns>类型委托调用函数信息</returns>
        public static MethodInfo GetTypeInsert(Type type)
        {
            MethodInfo method;
            if (typeInserts.TryGetValue(type, out method)) return method;
            typeInserts.Set(type, method = insertMethod.MakeGenericMethod(type));
            return method;
        }
        /// <summary>
        /// 获取插入数据SQL表达式
        /// </summary>
        /// <param name="sqlStream">SQL表达式流</param>
        /// <param name="value">数据列</param>
        /// <param name="converter">SQL常量转换</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void insert<valueType>(CharStream sqlStream, valueType value, ConstantConverter converter)
        {
            Column<valueType>.Inserter.Insert(sqlStream, value, converter);
        }
        /// <summary>
        /// 获取插入数据SQL表达式函数信息
        /// </summary>
        private static readonly MethodInfo insertMethod = typeof(Inserter).GetMethod("insert", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取列名委托集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, Func<string, string>> getColumnNameMethods = new AutoCSer.Threading.LockDictionary<Type, Func<string, string>>();
        /// <summary>
        /// 获取列名委托
        /// </summary>
        /// <param name="type">数据列类型</param>
        /// <returns>获取列名委托</returns>
        public static Func<string, string> GetColumnNames(Type type)
        {
            Func<string, string> getColumnName;
            if (getColumnNameMethods.TryGetValue(type, out getColumnName)) return getColumnName;
            getColumnName = (Func<string, string>)Delegate.CreateDelegate(typeof(Func<string, string>), getColumnNamesMethod.MakeGenericMethod(type));
            getColumnNameMethods.Set(type, getColumnName);
            return getColumnName;
        }
        /// <summary>
        /// 获取列名集合
        /// </summary>
        /// <param name="name">列名前缀</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe static string getColumnNames<valueType>(string name)
        {
            return Column<valueType>.Inserter.GetColumnNames(name);
        }
        /// <summary>
        /// 获取列名集合函数信息
        /// </summary>
        private static readonly MethodInfo getColumnNamesMethod = typeof(Inserter).GetMethod("getColumnNames", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void clearCache(int count)
        {
            typeInserts.Clear();
            getColumnNameMethods.Clear();
        }
        static Inserter()
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
        /// 数据列添加SQL流
        /// </summary>
        internal static class Inserter
        {
            /// <summary>
            /// 数据列名集合
            /// </summary>
            private static readonly AutoCSer.Threading.LockEquatableLastDictionary<HashString, string> columnNames;
            /// <summary>
            /// 获取列名集合
            /// </summary>
            /// <param name="name">列名前缀</param>
            /// <returns></returns>
            public unsafe static string GetColumnNames(string name)
            {
                if (custom != null) return custom.GetColumnNames(name);
                if (columnNames != null)
                {
                    string names;
                    HashString nameKey = name;
                    if (columnNames.TryGetValue(ref nameKey, out names)) return names;
                    int isNext = 0;
                    byte* buffer = UnmanagedPool.Default.Get();
                    try
                    {
                        using (CharStream sqlStream = new CharStream((char*)buffer, UnmanagedPool.DefaultSize >> 1))
                        {
                            foreach (Field field in Fields)
                            {
                                if (field.IsSqlColumn)
                                {
                                    if ((names = ColumnGroup.Inserter.GetColumnNames(field.FieldInfo.FieldType)(name + "_" + field.FieldInfo.Name)) != null)
                                    {
                                        if (isNext == 0) isNext = 1;
                                        else sqlStream.Write(',');
                                        sqlStream.Write(names);
                                    }
                                }
                                else
                                {
                                    if (isNext == 0) isNext = 1;
                                    else sqlStream.Write(',');
                                    sqlStream.PrepLength(name.Length + field.FieldInfo.Name.Length + 1);
                                    sqlStream.SimpleWriteNotNull(name);
                                    sqlStream.Write('_');
                                    sqlStream.SimpleWriteNotNull(field.FieldInfo.Name);
                                }
                            }
                            names = sqlStream.Length == 0 ? null : sqlStream.ToString();
                            columnNames.Set(ref nameKey, names);
                        }
                    }
                    finally { UnmanagedPool.Default.Push(buffer); }
                    return names;
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
                columnNames.Clear();
            }

            /// <summary>
            /// 获取插入数据SQL表达式
            /// </summary>
            /// <param name="sqlStream">SQL表达式流</param>
            /// <param name="value">数据列</param>
            /// <param name="converter">SQL常量转换</param>
#if !NOJIT
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
#endif
            public static void Insert(CharStream sqlStream, valueType value, ConstantConverter converter)
            {
#if NOJIT
                if (fields != null)
                {
                    object[] sqlColumnParameters = null, castParameters = null, parameters = null;
                    object objectValue = value;
                    byte isNext = 0;
                    foreach (sqlModel.insertField field in fields)
                    {
                        if (isNext == 0) isNext = 1;
                        else sqlStream.Write(',');
                        AutoCSer.code.cSharp.sqlModel.fieldInfo fieldInfo = field.Field;
                        if (fieldInfo.IsSqlColumn)
                        {
                            if (sqlColumnParameters == null) sqlColumnParameters = new object[] { sqlStream, null, converter };
                            sqlColumnParameters[1] = fieldInfo.Field.GetValue(objectValue);
                            field.SqlColumnMethod.Invoke(null, sqlColumnParameters);
                        }
                        else
                        {
                            object memberValue = fieldInfo.Field.GetValue(objectValue);
                            if (fieldInfo.ToSqlCastMethod != null)
                            {
                                if (castParameters == null) castParameters = new object[1];
                                castParameters[0] = memberValue;
                                memberValue = fieldInfo.ToSqlCastMethod.Invoke(null, castParameters);
                            }
                            if (parameters == null) parameters = new object[] { sqlStream, null };
                            parameters[1] = memberValue;
                            fieldInfo.ToSqlMethod.Invoke(converter, parameters);
                        }
                    }
                }
#else
                if (inserter != null) inserter(sqlStream, value, converter);
#endif
                else if (custom != null) custom.Insert(sqlStream, value, converter);
            }
#if NOJIT
            /// <summary>
            /// 字段集合
            /// </summary>
            private static readonly sqlModel.insertField[] fields;
#else
            /// <summary>
            /// 获取插入数据SQL表达式
            /// </summary>
            private static readonly Action<CharStream, valueType, ConstantConverter> inserter;
#endif

            static Inserter()
            {
                if (attribute != null && custom == null && Fields != null)
                {
                    columnNames = new AutoCSer.Threading.LockEquatableLastDictionary<HashString, string>();
#if NOJIT
                    fields = new sqlModel.insertField[Fields.Length];
                    int index = 0;
                    foreach (AutoCSer.code.cSharp.sqlModel.fieldInfo member in Fields) fields[index++].Set(member);
#else
                    ColumnGroup.Inserter dynamicMethod = new ColumnGroup.Inserter(typeof(valueType), attribute);
                    foreach (Field member in Fields) dynamicMethod.Push(member);
                    inserter = (Action<CharStream, valueType, ConstantConverter>)dynamicMethod.Create<Action<CharStream, valueType, ConstantConverter>>();
#endif
                    AutoCSer.Pub.ClearCaches += clearCache;
                }
            }
        }
    }
}
