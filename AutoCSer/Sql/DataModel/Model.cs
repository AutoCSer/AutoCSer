using System;
using System.Threading;
using AutoCSer.Metadata;
using AutoCSer.Extension;
#if NOJIT
#else
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Sql.DataModel
{
    /// <summary>
    /// 数据库表格模型
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal abstract partial class Model<valueType> : AutoCSer.Data.Model<valueType>
    {
        /// <summary>
        /// 数据库表格模型配置
        /// </summary>
        private static readonly ModelAttribute attribute;
        /// <summary>
        /// 字段集合
        /// </summary>
        internal static readonly Field[] Fields;
        /// <summary>
        /// 自增字段
        /// </summary>
        internal static readonly Field Identity;
        /// <summary>
        /// 自增字段名称
        /// </summary>
        internal static readonly string IdentitySqlName;
        /// <summary>
        /// 关键字字段集合
        /// </summary>
        internal static readonly Field[] PrimaryKeys;
        /// <summary>
        /// SQL数据成员
        /// </summary>
        internal static readonly MemberMap<valueType> MemberMap;
        /// <summary>
        /// SQL数据成员
        /// </summary>
        internal static MemberMap<valueType> CopyMemberMap
        {
            get { return MemberMap.Copy(); }
        }
        /// <summary>
        /// 自增标识获取器
        /// </summary>
        internal static readonly Func<valueType, long> GetIdentity;
        /// <summary>
        /// 自增标识获取器
        /// </summary>
        internal static readonly Func<valueType, int> GetIdentity32;
        /// <summary>
        /// 设置自增标识
        /// </summary>
        internal static readonly Action<valueType, long> SetIdentity;
        /// <summary>
        /// 分组数据成员位图
        /// </summary>
        private static KeyValue<MemberMap<valueType>, int>[] groupMemberMaps;
        /// <summary>
        /// 分组数据成员位图访问锁
        /// </summary>
        private static readonly object groupMemberMapLock = new object();
        /// <summary>
        /// 获取分组数据成员位图
        /// </summary>
        /// <param name="group">分组</param>
        /// <returns>分组数据成员位图</returns>
        private static MemberMap<valueType> getGroupMemberMap(int group)
        {
            if (groupMemberMaps == null)
            {
                LeftArray<KeyValue<MemberMap<valueType>, int>> memberMaps = new LeftArray<KeyValue<MemberMap<valueType>, int>>();
                memberMaps.Add(new KeyValue<MemberMap<valueType>, int>(MemberMap, 0));
                Monitor.Enter(groupMemberMapLock);
                if (groupMemberMaps == null)
                {
                    try
                    {
                        foreach (Field field in Fields)
                        {
                            if (field.DataMember.Group != 0)
                            {
                                int index = memberMaps.Length;
                                foreach (KeyValue<MemberMap<valueType>, int> memberMap in memberMaps.Array)
                                {
                                    if (memberMap.Value == field.DataMember.Group || --index == 0) break;
                                }
                                if (index == 0)
                                {
                                    MemberMap<valueType> memberMap = new MemberMap<valueType>();
                                    memberMaps.Add(new KeyValue<MemberMap<valueType>, int>(memberMap, field.DataMember.Group));
                                    memberMap.SetMember(field.MemberMapIndex);
                                }
                                else memberMaps.Array[memberMaps.Length - index].Key.SetMember(field.MemberMapIndex);
                            }
                        }
                        if (memberMaps.Length != 1)
                        {
                            MemberMap<valueType> memberMap = memberMaps.Array[0].Key = new MemberMap<valueType>();
                            foreach (Field field in Fields)
                            {
                                if (field.DataMember.Group == 0) memberMap.SetMember(field.MemberMapIndex);
                            }
                        }
                        groupMemberMaps = memberMaps.ToArray();
                    }
                    finally { Monitor.Exit(groupMemberMapLock); }
                }
                else Monitor.Exit(groupMemberMapLock);
            }
            foreach (KeyValue<MemberMap<valueType>, int> memberMap in groupMemberMaps)
            {
                if (memberMap.Value == group) return memberMap.Key;
            }
            AutoCSer.Log.Pub.Log.add(AutoCSer.Log.LogType.Error, typeof(valueType).fullName() + " 缺少缓存分组 " + group.toString());
            return null;
        }
        /// <summary>
        /// 获取分组数据成员位图
        /// </summary>
        /// <param name="group">分组</param>
        /// <returns>分组数据成员位图</returns>
        internal static MemberMap<valueType> GetCacheMemberMap(int group)
        {
            MemberMap<valueType> memberMap = getGroupMemberMap(group);
            if (memberMap != null)
            {
                SetIdentityOrPrimaryKeyMemberMap(memberMap = memberMap.Copy());
                return memberMap;
            }
            return null;
        }
        /// <summary>
        /// 自增标识/关键字成员位图
        /// </summary>
        /// <returns></returns>
        internal static MemberMap<valueType> GetIdentityOrPrimaryKeyMemberMap()
        {
            MemberMap<valueType> memberMap = MemberMap<valueType>.NewEmpty();
            SetIdentityOrPrimaryKeyMemberMap(memberMap);
            return memberMap;
        }
        /// <summary>
        /// 自增标识/关键字成员位图
        /// </summary>
        /// <param name="memberMap"></param>
        internal static void SetIdentityOrPrimaryKeyMemberMap(MemberMap<valueType> memberMap)
        {
            if (Identity != null) memberMap.SetMember(Identity.MemberMapIndex);
            else if (PrimaryKeys.Length != 0)
            {
                foreach (Field field in PrimaryKeys) memberMap.SetMember(field.MemberMapIndex);
            }
        }
        /// <summary>
        /// 获取自增标识获取器
        /// </summary>
        /// <param name="baseIdentity"></param>
        /// <returns></returns>
        internal static Func<valueType, int> IdentityGetter(int baseIdentity)
        {
            if (baseIdentity == 0) return GetIdentity32;
#if NOJIT
            return new baseIdentity32(Identity.Field, baseIdentity).Get();
#else
            DynamicMethod dynamicMethod = new DynamicMethod("GetIdentity32_" + baseIdentity.toString(), typeof(int), new Type[] { typeof(valueType) }, typeof(valueType), true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, Identity.FieldInfo);
            if (Identity.FieldInfo.FieldType != typeof(int) && Identity.FieldInfo.FieldType != typeof(uint)) generator.Emit(OpCodes.Conv_I4);
            generator.int32(baseIdentity);
            generator.Emit(OpCodes.Sub);
            generator.Emit(OpCodes.Ret);
            return (Func<valueType, int>)dynamicMethod.CreateDelegate(typeof(Func<valueType, int>));
#endif
        }
        /// <summary>
        /// 获取以逗号分割的名称集合
        /// </summary>
        /// <param name="sqlStream"></param>
        /// <param name="memberMap"></param>
        internal static void GetNames(CharStream sqlStream, MemberMap<valueType> memberMap)
        {
            int isNext = 0;
            foreach (Field field in Fields)
            {
                if (memberMap.IsMember(field.MemberMapIndex))
                {
                    if (isNext == 0) isNext = 1;
                    else sqlStream.Write(',');
                    if (field.IsSqlColumn) sqlStream.SimpleWriteNotNull(field.GetSqlColumnName());
                    else sqlStream.SimpleWriteNotNull(field.SqlFieldName);
                }
            }
        }
        /// <summary>
        /// 获取表格信息
        /// </summary>
        /// <param name="client">SQL 客户端操作</param>
        /// <param name="tableName">表格名称</param>
        /// <returns>表格信息</returns>
        internal static TableColumnCollection GetTable(Client client, string tableName)
        {
            TableColumnCollection table = new TableColumnCollection { Columns = new ColumnCollection { Name = tableName } };
            Column[] columns = new Column[Fields.Length];
            Column[] primaryKeyColumns = new Column[PrimaryKeys.Length];
            int index = 0, primaryKeyIndex = 0;
            foreach (Field member in Fields)
            {
                Column column = client.GetColumn(member.FieldInfo.Name, member.FieldInfo.FieldType, member.DataMember);
                columns[index++] = column;
                if (Identity == member) table.Identity = column;
                if (member.DataMember.PrimaryKeyIndex != 0) primaryKeyColumns[primaryKeyIndex++] = column;
            }
            table.Columns.Columns = columns;
            if (primaryKeyColumns.Length != 0)
            {
                table.PrimaryKey = new ColumnCollection
                {
                    Columns = PrimaryKeys.getArray(value => primaryKeyColumns.firstOrDefault(column => column.Name == value.FieldInfo.Name))
                };
            }
            return table;
        }

        static Model()
        {
            Type type = typeof(valueType);
            attribute = TypeAttribute.GetAttribute<ModelAttribute>(type, true) ?? ModelAttribute.Default;
            Fields = Field.Get(MemberIndexGroup<valueType>.GetFields(attribute.MemberFilters), false).ToArray();
            Identity = Field.GetIdentity(Fields);
            PrimaryKeys = Field.GetPrimaryKeys(Fields).ToArray();
            MemberMap = new MemberMap<valueType>();
            foreach (Field field in Fields) MemberMap.SetMember(field.MemberMapIndex);
            if (Identity != null)
            {
                IdentitySqlName = Identity.SqlFieldName;
#if NOJIT
                new identity(Identity.Field).Get(out GetIdentity, out SetIdentity);
                Action<valueType, int> setter32;
                new identity32(Identity.Field).Get(out GetIdentity32, out setter32);
#else
                DynamicMethod dynamicMethod = new DynamicMethod("GetSqlIdentity", typeof(long), new Type[] { type }, type, true);
                ILGenerator generator = dynamicMethod.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, Identity.FieldInfo);
                if (Identity.FieldInfo.FieldType != typeof(long) && Identity.FieldInfo.FieldType != typeof(ulong)) generator.Emit(OpCodes.Conv_I8);
                generator.Emit(OpCodes.Ret);
                GetIdentity = (Func<valueType, long>)dynamicMethod.CreateDelegate(typeof(Func<valueType, long>));

                dynamicMethod = new DynamicMethod("SetSqlIdentity", null, new Type[] { type, typeof(long) }, type, true);
                generator = dynamicMethod.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                if (Identity.FieldInfo.FieldType != typeof(long) && Identity.FieldInfo.FieldType != typeof(ulong)) generator.Emit(OpCodes.Conv_I4);
                generator.Emit(OpCodes.Stfld, Identity.FieldInfo);
                generator.Emit(OpCodes.Ret);
                SetIdentity = (Action<valueType, long>)dynamicMethod.CreateDelegate(typeof(Action<valueType, long>));

                GetIdentity32 = getIdentityGetter32("GetSqlIdentity32", Identity.FieldInfo);
#endif
            }
        }
    }
}
