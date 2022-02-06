using System;
using AutoCSer.Metadata;
using AutoCSer.Extensions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.ColumnGroup
{
    /// <summary>
    /// 数据列
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal static partial class Column<valueType>
    {
        /// <summary>
        /// SQL列配置
        /// </summary>
        private static readonly ColumnAttribute attribute;
        /// <summary>
        /// 自定义类型处理接口
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
#if NOJIT
        private static readonly ICustom custom;
#else
        private static readonly ICustom<valueType> custom;
#endif
        /// <summary>
        /// 字段集合
        /// </summary>
        internal static readonly Field[] Fields;

        /// <summary>
        /// 数据列名与类型集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<HashString, KeyValue<string, Type>[]> dataColumns;
        /// <summary>
        /// 获取成员名称与类型集合
        /// </summary>
        /// <param name="name">列名前缀</param>
        /// <returns></returns>
        internal static KeyValue<string, Type>[] GetDataColumns(string name)
        {
            if (custom != null) return custom.GetDataColumns(name);
            if (Fields != null)
            {
                KeyValue<string, Type>[] values;
                HashString nameKey = name;
                if (dataColumns.TryGetValue(nameKey, out values)) return values;
                LeftArray<KeyValue<string, Type>> columns = new LeftArray<KeyValue<string, Type>>(Fields.Length);
                foreach (Field field in Fields)
                {
                    if (field.IsSqlColumn) ((Func<string, KeyValue<string, Type>[]>)AutoCSer.Sql.Metadata.GenericType.Get(field.DataType).GetDataColumnsMethod)(name + "_" + field.FieldInfo.Name);
                    else columns.Add(new KeyValue<string, Type>(name + "_" + field.FieldInfo.Name, field.DataType));
                }
                values = columns.ToArray();
                dataColumns.Set(nameKey, values);
                return values;
            }
            return null;
        }

        static Column()
        {
            Type type = typeof(valueType);
            if (type.IsEnum || !type.IsValueType)
            {
                AutoCSer.LogHelper.Error(type.fullName() + " 非值类型，不能用作数据列", LogLevel.Error | LogLevel.AutoCSer);
                return;
            }
            attribute = TypeAttribute.GetAttribute<ColumnAttribute>(type, true) ?? ColumnAttribute.Default;
            foreach (MethodInfo method in type.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
            {
#if NOJIT
                if (typeof(ICustom).IsAssignableFrom(method.ReturnType)
#else
                if (typeof(ICustom<valueType>).IsAssignableFrom(method.ReturnType)
#endif
                    && method.GetParameters().Length == 0 && method.IsDefined(typeof(ColumnAttribute), false))
                {
                    object customValue = method.Invoke(null, null);
                    if (customValue != null)
                    {
#if NOJIT
                        custom = (ICustom)customValue;
#else
                        custom = (ICustom<valueType>)customValue;
#endif
                        return;
                    }
                }
            }
            Fields = Field.Get(MemberIndexGroup<valueType>.GetFields(attribute.MemberFilters), true).ToArray();
            dataColumns = new AutoCSer.Threading.LockDictionary<HashString, KeyValue<string, Type>[]>();
            AutoCSer.Memory.Common.AddClearCache(dataColumns.Clear, typeof(Column<valueType>), 60 * 60);
        }
    }
}
