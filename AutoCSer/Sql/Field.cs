using System;
using AutoCSer.Metadata;
using System.Reflection;
using AutoCSer.Data;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 字段信息
    /// </summary>
    internal class Field
    {
        /// <summary>
        /// 字段信息
        /// </summary>
        internal FieldInfo FieldInfo;
        /// <summary>
        /// 数据库成员信息
        /// </summary>
        internal MemberAttribute DataMember;
        /// <summary>
        /// 可空类型数据库数据类型
        /// </summary>
        internal Type NullableDataType;
        /// <summary>
        /// 数据库数据类型
        /// </summary>
        internal Type DataType;
        /// <summary>
        /// 数据读取函数
        /// </summary>
        internal MethodInfo DataReaderMethod;
        /// <summary>
        /// 数据转换SQL字符串函数信息
        /// </summary>
        internal MethodInfo ToSqlMethod;
        /// <summary>
        /// 数据转换SQL字符串之前的类型转换函数信息
        /// </summary>
        internal MethodInfo ToSqlCastMethod;
        /// <summary>
        /// 数据转换成对象之前的类型转换函数信息
        /// </summary>
        internal MethodInfo ToModelCastMethod;
        /// <summary>
        /// 成员位图索引
        /// </summary>
        internal int MemberMapIndex;
        /// <summary>
        /// 是否数据列
        /// </summary>
        internal bool IsSqlColumn;
        /// <summary>
        /// 是否默认JSON
        /// </summary>
        internal bool IsUnknownJson;
        /// <summary>
        /// 是否当前时间
        /// </summary>
        internal bool IsNowTime;
        /// <summary>
        /// 是否需要验证
        /// </summary>
        internal bool IsVerify
        {
            get
            {
                if (IsSqlColumn)
                {
                    bool isVerify;
                    if (verifyTypes.TryGetValue(DataType, out isVerify)) return isVerify;
#if NOJIT
                    isVerify = typeof(ColumnGroup.Column<>.Verifyer).MakeGenericType(DataType).GetField("fields", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) != null
#else
                    isVerify = typeof(ColumnGroup.Column<>.Verifyer).MakeGenericType(DataType).GetField("verifyer", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) != null
#endif
                        || typeof(ColumnGroup.Column<>).MakeGenericType(DataType).GetField("custom", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) != null;
                    verifyTypes.Set(DataType, isVerify);
                    return isVerify;
                }
                if (!DataMember.IsDefaultMember)
                {
                    if (DataType == typeof(string)) return DataMember.MaxStringLength > 0;
                    return DataType.IsClass && !DataMember.IsNull;
                }
                return false;
            }
        }
        /// <summary>
        /// 字段信息
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="attribute">数据库成员信息</param>
        internal Field(FieldIndex field, MemberAttribute attribute)
        {
            FieldInfo = field.Member;
            MemberMapIndex = field.MemberIndex;
            DataMember = format(attribute, FieldInfo.FieldType, ref IsSqlColumn);
            if ((NullableDataType = DataMember.DataType) == null) NullableDataType = FieldInfo.FieldType;
            if ((DataReaderMethod = DataReader.GetMethod(DataType = NullableDataType.nullableType() ?? NullableDataType)) == null)
            {
                if (IsSqlColumn)
                {
                    if (isSqlColumn(DataType)) return;
                    IsSqlColumn = false;
                }
#if NOJIT
                    DataType = NullableDataType = typeof(string);
                    DataReaderMethod = DataReader.GetMethodInfo;
#else
                DataReaderMethod = DataReader.GetMethod(DataType = NullableDataType = typeof(string));
#endif
                IsUnknownJson = true;
                ToSqlCastMethod = jsonSerializeMethod.MakeGenericMethod(FieldInfo.FieldType);
                ToModelCastMethod = jsonParseMethod.MakeGenericMethod(FieldInfo.FieldType);
            }
            else
            {
                ToSqlCastMethod = AutoCSer.Emit.CastType.GetMethod(FieldInfo.FieldType, DataType);
                ToModelCastMethod = AutoCSer.Emit.CastType.GetMethod(DataType, FieldInfo.FieldType);
            }
            if (attribute != null && attribute.IsNowTime && FieldInfo.FieldType == typeof(DateTime)) IsNowTime = true;
            ToSqlMethod = ConstantConverter.GetMethod(DataType);
        }
        /// <summary>
        /// 获取数据列名称
        /// </summary>
        private Func<string, string> getSqlColumnName;
        /// <summary>
        /// 获取数据列名称
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal string GetSqlColumnName()
        {
            if (getSqlColumnName == null) getSqlColumnName = ColumnGroup.Inserter.GetColumnNames(FieldInfo.FieldType);
            return getSqlColumnName(FieldInfo.Name);
        }
        /// <summary>
        /// 格式化数据库成员信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="isSqlColumn"></param>
        /// <returns></returns>
        private static MemberAttribute format(MemberAttribute value, Type type, ref bool isSqlColumn)
        {
            if (type.IsEnum)
            {
                Type enumType = System.Enum.GetUnderlyingType(type);
                if (enumType == typeof(sbyte)) enumType = typeof(byte);
                else if (enumType == typeof(ushort)) enumType = typeof(short);
                else if (enumType == typeof(ulong)) enumType = typeof(long);
                if (value == null) return new MemberAttribute { DataType = enumType };
                if (value.DataType == null) value.DataType = enumType;
                else if (enumType != value.DataType) value.EnumType = enumType;
                return value;
            }
            Type nullableType = null;
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                //if (genericType == typeof(AutoCSer.sql.fileBlockMember<>))
                //{
                //    if (value == null) return new dataMember { DataType = typeof(AutoCSer.io.fileBlockStream.index) };
                //    value.DataType = typeof(AutoCSer.io.fileBlockStream.index);
                //    return value;
                //}
                if (genericType == typeof(Nullable<>)) nullableType = type.GetGenericArguments()[0];
            }
            else if (type.IsDefined(typeof(ColumnAttribute), false))
            {
                isSqlColumn = true;
                return MemberAttribute.DefaultDataMember;
            }
            if (value == null || value.DataType == null)
            {
                MemberAttribute sqlMember = TypeAttribute.GetAttribute<MemberAttribute>(type, false);
                if (sqlMember != null && sqlMember.DataType != null)
                {
                    if (value == null) value = new MemberAttribute();
                    value.DataType = sqlMember.DataType;
                    if (sqlMember.DataType.IsValueType && sqlMember.DataType.IsGenericType && sqlMember.DataType.GetGenericTypeDefinition() == typeof(Nullable<>)) value.IsNull = true;
                }
            }
            if (value == null)
            {
                if (nullableType == null)
                {
                    Type dataType = type.formCSharpType().toCSharpType();
                    if (dataType != type) value = new MemberAttribute { DataType = dataType };
                }
                else
                {
                    value = new MemberAttribute { IsNull = true };
                    Type dataType = nullableType.formCSharpType().toCSharpType();
                    if (dataType != nullableType) value.DataType = dataType.toNullableType();
                }
            }
            return value ?? MemberAttribute.DefaultDataMember;
        }
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <returns>Json字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static string jsonSerialize<valueType>(valueType value)
        {
            return value == null ? string.Empty : AutoCSer.Json.Serializer.Serialize(value);
        }
        /// <summary>
        /// 对象转换JSON字符串函数信息
        /// </summary>
        private static readonly MethodInfo jsonSerializeMethod = typeof(Field).GetMethod("jsonSerialize", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// Json解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="jsonString">Json字符串</param>
        /// <returns>目标数据</returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static valueType jsonParse<valueType>(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString)) return default(valueType);
            valueType value = default(valueType);
            return AutoCSer.Json.Parser.ParseNotEmpty(jsonString, ref value).State == Json.ParseState.Success ? value : default(valueType);
        }
        /// <summary>
        /// Json解析函数信息
        /// </summary>
        private static readonly MethodInfo jsonParseMethod = typeof(Field).GetMethod("jsonParse", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(string) }, null);
        /// <summary>
        /// 数据列类型集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, bool> sqlColumnTypes = new AutoCSer.Threading.LockDictionary<Type, bool>();
        /// <summary>
        /// 是否有效数据列
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool isSqlColumn(Type type)
        {
            bool isType;
            if (sqlColumnTypes.TryGetValue(type, out isType)) return isType;
#if NOJIT
            isType = typeof(ColumnGroup.Column<>.Set).MakeGenericType(type).GetField("fields", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) != null
#else
            isType = typeof(ColumnGroup.Column<>.Setter).MakeGenericType(type).GetField("defaultSetter", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) != null
#endif
                || typeof(ColumnGroup.Column<>).MakeGenericType(type).GetField("custom", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) != null;
            sqlColumnTypes.Set(type, isType);
            return isType;
        }
        /// <summary>  
        /// 数据列验证类型集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, bool> verifyTypes = new AutoCSer.Threading.LockDictionary<Type, bool>();
#if !NOJIT
        /// <summary>
        /// int引用参数类型
        /// </summary>
        internal static readonly Type RefIntType = typeof(int).MakeByRefType();
#endif

        /// <summary>
        /// 获取字段成员集合
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="isColumn"></param>
        /// <returns>字段成员集合</returns>
        internal static LeftArray<Field> Get(FieldIndex[] fields, bool isColumn)
        {
            LeftArray<Field> values = new LeftArray<Field>(fields.Length);
            foreach (FieldIndex field in fields)
            {
                Type type = field.Member.FieldType;
                if (!type.IsPointer && (!type.IsArray || type.GetArrayRank() == 1) && !field.IsIgnore)
                {
                    MemberAttribute attribute = field.GetAttribute<MemberAttribute>(false);
                    if (attribute == null || attribute.IsSetup)
                    {
                        LogAttribute logAttribute = isColumn ? null : field.GetAttribute<LogAttribute>(false);
                        if (logAttribute == null || logAttribute.IsMember)
                        {
                            if (attribute != null && attribute.IsNowTime && type != typeof(DateTime)) attribute.IsNowTime = false;
                            values.Add(new Field(field, attribute));
                        }
                    }
                }
            }
            return values;
        }
        /// <summary>
        /// 默认自增ID列名称
        /// </summary>
        internal const string IdentityName = "Id";
        /// <summary>
        /// 获取自增标识
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        internal static Field GetIdentity(Field[] fields)
        {
            Field identity = null;
            int isIdentityCase = 0;
            foreach (Field field in fields)
            {
                if (field.DataMember.IsIdentity) return field;
                if (isIdentityCase == 0 && field.FieldInfo.Name == IdentityName)
                {
                    identity = field;
                    isIdentityCase = 1;
                }
                else if (identity == null && field.FieldInfo.Name.Length == IdentityName.Length && field.FieldInfo.Name.ToLower() == IdentityName) identity = field;
            }
            return identity;
        }
        /// <summary>
        /// 获取关键字集合
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        internal static LeftArray<Field> GetPrimaryKeys(Field[] fields)
        {
            return fields.getFind(value => value.DataMember.PrimaryKeyIndex != 0)
                .Sort((left, right) =>
                {
                    int value = left.DataMember.PrimaryKeyIndex - right.DataMember.PrimaryKeyIndex;
                    return value == 0 ? AutoCSer.Metadata.MemberIndexGroup.Compare(left.FieldInfo, right.FieldInfo) : value;
                });
        }
        /// <summary>
        /// 获取数据库成员信息集合 
        /// </summary>
        /// <param name="type">数据库绑定类型</param>
        /// <param name="database">数据库配置</param>
        /// <returns>数据库成员信息集合</returns>
        internal static KeyValue<MemberIndexInfo, MemberAttribute>[] GetMemberIndexs<attributeType>(Type type, attributeType database)
             where attributeType : Metadata.MemberFilterAttribute
        {
            return GetMembers(MemberIndexGroup.Get(type).Find<MemberAttribute>(database));
        }
        /// <summary>
        /// 获取数据库成员信息集合
        /// </summary>
        /// <param name="members">成员集合</param>
        /// <returns>数据库成员信息集合</returns>
        internal static KeyValue<MemberIndexInfo, MemberAttribute>[] GetMembers(MemberIndexInfo[] members)
        {
            return members.getFind(value => value.CanSet && value.CanGet)
                .GetArray(value => new KeyValue<MemberIndexInfo, MemberAttribute>(value, MemberAttribute.Get(value)));
        }

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void clearCache(int count)
        {
            sqlColumnTypes.Clear();
            verifyTypes.Clear();
        }
        static Field()
        {
            AutoCSer.Pub.ClearCaches += clearCache;
        }
    }
}
