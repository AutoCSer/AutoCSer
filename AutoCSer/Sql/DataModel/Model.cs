using System;
using System.Threading;
using AutoCSer.Metadata;
using AutoCSer.Extension;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Sql.DataModel
{
    /// <summary>
    /// 数据库表格模型
    /// </summary>
    /// <typeparam name="modelType">数据类型</typeparam>
    internal abstract partial class Model<modelType> : AutoCSer.Data.Model<modelType>
    {
        /// <summary>
        /// 数据库表格模型配置
        /// </summary>
        protected static readonly ModelAttribute attribute;
        /// <summary>
        /// 字段集合
        /// </summary>
        internal static readonly Field[] Fields;
        /// <summary>
        /// 自增字段
        /// </summary>
        internal static readonly Field Identity;
        /// <summary>
        /// 关键字字段集合
        /// </summary>
        internal static readonly Field[] PrimaryKeys;
        /// <summary>
        /// SQL数据成员
        /// </summary>
        internal static readonly MemberMap<modelType> MemberMap;
        /// <summary>
        /// SQL数据成员
        /// </summary>
        internal static MemberMap<modelType> CopyMemberMap
        {
            get { return MemberMap.Copy(); }
        }
        /// <summary>
        /// 自增标识获取器
        /// </summary>
        internal static readonly Func<modelType, long> GetIdentity;
        /// <summary>
        /// 自增标识获取器
        /// </summary>
        internal static readonly Func<modelType, int> GetIdentity32;
        /// <summary>
        /// 设置自增标识
        /// </summary>
        internal static readonly Action<modelType, long> SetIdentity;
        /// <summary>
        /// 分组数据成员位图
        /// </summary>
        private static KeyValue<MemberMap<modelType>, int>[] groupMemberMaps;
        /// <summary>
        /// 分组数据成员位图访问锁
        /// </summary>
        private static readonly object groupMemberMapLock = new object();
        /// <summary>
        /// 获取分组数据成员位图
        /// </summary>
        /// <param name="group">分组</param>
        /// <returns>分组数据成员位图</returns>
        private static MemberMap<modelType> getGroupMemberMap(int group)
        {
            if (groupMemberMaps == null)
            {
                LeftArray<KeyValue<MemberMap<modelType>, int>> memberMaps = new LeftArray<KeyValue<MemberMap<modelType>, int>>();
                memberMaps.Add(new KeyValue<MemberMap<modelType>, int>(MemberMap, 0));
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
                                foreach (KeyValue<MemberMap<modelType>, int> memberMap in memberMaps.Array)
                                {
                                    if (memberMap.Value == field.DataMember.Group || --index == 0) break;
                                }
                                if (index == 0)
                                {
                                    MemberMap<modelType> memberMap = new MemberMap<modelType>();
                                    memberMaps.Add(new KeyValue<MemberMap<modelType>, int>(memberMap, field.DataMember.Group));
                                    memberMap.SetMember(field.MemberMapIndex);
                                }
                                else memberMaps.Array[memberMaps.Length - index].Key.SetMember(field.MemberMapIndex);
                            }
                        }
                        if (memberMaps.Length != 1)
                        {
                            MemberMap<modelType> memberMap = memberMaps.Array[0].Key = new MemberMap<modelType>();
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
            foreach (KeyValue<MemberMap<modelType>, int> memberMap in groupMemberMaps)
            {
                if (memberMap.Value == group) return memberMap.Key;
            }
            AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Error, typeof(modelType).fullName() + " 缺少缓存分组 " + group.toString());
            return null;
        }
        /// <summary>
        /// 获取分组数据成员位图
        /// </summary>
        /// <param name="group">分组</param>
        /// <returns>分组数据成员位图</returns>
        internal static MemberMap<modelType> GetCacheMemberMap(int group)
        {
            MemberMap<modelType> memberMap = getGroupMemberMap(group);
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
        internal static MemberMap<modelType> GetIdentityOrPrimaryKeyMemberMap()
        {
            MemberMap<modelType> memberMap = MemberMap<modelType>.NewEmpty();
            SetIdentityOrPrimaryKeyMemberMap(memberMap);
            return memberMap;
        }
        /// <summary>
        /// 自增标识/关键字成员位图
        /// </summary>
        /// <param name="memberMap"></param>
        internal static void SetIdentityOrPrimaryKeyMemberMap(MemberMap<modelType> memberMap)
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
        internal static Func<modelType, int> IdentityGetter(int baseIdentity)
        {
            if (baseIdentity == 0) return GetIdentity32;
#if NOJIT
            return new baseIdentity32(Identity.Field, baseIdentity).Get();
#else
            DynamicMethod dynamicMethod = new DynamicMethod("GetIdentity32_" + baseIdentity.toString(), typeof(int), new Type[] { typeof(modelType) }, typeof(modelType), true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, Identity.FieldInfo);
            if (Identity.FieldInfo.FieldType != typeof(int) && Identity.FieldInfo.FieldType != typeof(uint)) generator.Emit(OpCodes.Conv_I4);
            generator.int32(baseIdentity);
            generator.Emit(OpCodes.Sub);
            generator.Emit(OpCodes.Ret);
            return (Func<modelType, int>)dynamicMethod.CreateDelegate(typeof(Func<modelType, int>));
#endif
        }
        /// <summary>
        /// 获取以逗号分割的名称集合
        /// </summary>
        /// <param name="sqlStream"></param>
        /// <param name="memberMap"></param>
        /// <param name="constantConverter"></param>
        internal static void GetNames(CharStream sqlStream, MemberMap<modelType> memberMap, ConstantConverter constantConverter)
        {
            int isNext = 0;
            foreach (Field field in Fields)
            {
                if (memberMap.IsMember(field.MemberMapIndex))
                {
                    if (isNext == 0) isNext = 1;
                    else sqlStream.Write(',');
                    if (field.IsSqlColumn) sqlStream.SimpleWriteNotNull(field.GetSqlColumnName());
                    else constantConverter.ConvertNameToSqlStream(sqlStream, field.FieldInfo.Name);
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
            Type type = typeof(modelType);
            attribute = TypeAttribute.GetAttribute<ModelAttribute>(type, true) ?? ModelAttribute.Default;
            Fields = Field.Get(MemberIndexGroup<modelType>.GetFields(attribute.MemberFilters), false).ToArray();
            Identity = Field.GetIdentity(Fields);
            PrimaryKeys = Field.GetPrimaryKeys(Fields).ToArray();
            MemberMap = new MemberMap<modelType>();
            foreach (Field field in Fields) MemberMap.SetMember(field.MemberMapIndex);
            if (Identity != null)
            {
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
                GetIdentity = (Func<modelType, long>)dynamicMethod.CreateDelegate(typeof(Func<modelType, long>));

                dynamicMethod = new DynamicMethod("SetSqlIdentity", null, new Type[] { type, typeof(long) }, type, true);
                generator = dynamicMethod.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                if (Identity.FieldInfo.FieldType != typeof(long) && Identity.FieldInfo.FieldType != typeof(ulong)) generator.Emit(OpCodes.Conv_I4);
                generator.Emit(OpCodes.Stfld, Identity.FieldInfo);
                generator.Emit(OpCodes.Ret);
                SetIdentity = (Action<modelType, long>)dynamicMethod.CreateDelegate(typeof(Action<modelType, long>));

                GetIdentity32 = getIdentityGetter32("GetSqlIdentity32", Identity.FieldInfo);
#endif
            }
        }
    }
}
