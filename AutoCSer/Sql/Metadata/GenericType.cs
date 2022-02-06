using AutoCSer.Memory;
using System;
using System.Data.Common;
using System.Reflection;

namespace AutoCSer.Sql.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class GenericType : AutoCSer.Metadata.GenericTypeBase
    {
        /// <summary>
        /// 对象转换JSON字符串函数信息
        /// </summary>
        internal abstract MethodInfo JsonSerializeMethod { get; }
        /// <summary>
        /// Json解析函数信息
        /// </summary>
        internal abstract MethodInfo JsonDeSerializeMethod { get; }
        /// <summary>
        /// 常量转换字符串函数信息
        /// </summary>
        internal abstract MethodInfo ConvertConstantToStringMethod { get; }

        /// <summary>
        /// 获取列名集合函数信息
        /// </summary>
        internal abstract Delegate VerifyerGetColumnNamesMethod { get; }
        /// <summary>
        /// 数据验证函数信息
        /// </summary>
        internal abstract MethodInfo VerifyMethod { get; }

        /// <summary>
        /// 获取列名集合函数信息
        /// </summary>
        internal abstract Delegate UpdaterGetColumnNamesMethod { get; }
        /// <summary>
        /// 获取更新数据SQL表达式函数信息
        /// </summary>
        internal abstract MethodInfo UpdateMethod { get; }

        /// <summary>
        /// 数据列转换数组函数信息
        /// </summary>
        internal abstract MethodInfo ToArrayMethod { get; }

        /// <summary>
        /// 设置字段值函数信息
        /// </summary>
        internal abstract MethodInfo SetMethod { get; }

        /// <summary>
        /// 获取列名集合函数信息
        /// </summary>
        internal abstract Delegate InserterGetColumnNamesMethod { get; }

        /// <summary>
        /// 获取插入数据SQL表达式函数信息
        /// </summary>
        internal abstract MethodInfo InsertMethod { get; }

        /// <summary>
        /// 获取成员名称与类型集合函数信息
        /// </summary>
        internal abstract Delegate GetDataColumnsMethod { get; }

        /// <summary>
        /// 泛型类型元数据缓存
        /// </summary>
        private static readonly AutoCSer.Threading.LockLastDictionary<HashType, GenericType> cache = new AutoCSer.Threading.LockLastDictionary<HashType, GenericType>(getCurrentType);
        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static GenericType create<T>()
        {
            return new GenericType<T>();
        }
        /// <summary>
        /// 创建泛型类型元数据 函数信息
        /// </summary>
        private static readonly MethodInfo createMethod = typeof(GenericType).GetMethod("create", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static GenericType Get(HashType type)
        {
            GenericType value;
            if (!cache.TryGetValue(type, out value))
            {
                try
                {
                    value = new UnionType.GenericType { Object = createMethod.MakeGenericMethod(type).Invoke(null, null) }.Value;
                    cache.Set(type, value);
                }
                finally { cache.Exit(); }
            }
            return value;
        }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    internal sealed partial class GenericType<T> : GenericType
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 对象转换JSON字符串函数信息
        /// </summary>
        internal override MethodInfo JsonSerializeMethod { get { return ((Func<T, string>)Field.JsonSerialize<T>).Method; } }
        /// <summary>
        /// Json解析函数信息
        /// </summary>
        internal override MethodInfo JsonDeSerializeMethod { get { return ((Func<string, T>)Field.JsonDeSerialize<T>).Method; } }
        /// <summary>
        /// 常量转换字符串函数信息
        /// </summary>
        internal override MethodInfo ConvertConstantToStringMethod { get { return ((Action<ConstantConverter, AutoCSer.Memory.CharStream, T>)ConstantConverter.ConvertConstantToString<T>).Method; } }

        /// <summary>
        /// 获取列名集合函数信息
        /// </summary>
        internal override Delegate VerifyerGetColumnNamesMethod { get { return (ColumnGroup.Verifyer.GetName)ColumnGroup.Verifyer.GetColumnNames<T>; } }
        /// <summary>
        /// 数据验证函数信息
        /// </summary>
        internal override MethodInfo VerifyMethod { get { return ((Func<T, Table, string,bool>)ColumnGroup.Column<T>.Verifyer.Verify).Method; } }

        /// <summary>
        /// 获取列名集合函数信息
        /// </summary>
        internal override Delegate UpdaterGetColumnNamesMethod { get { return (ColumnGroup.Verifyer.GetName)ColumnGroup.Updater.GetColumnNames<T>; } }
        /// <summary>
        /// 获取更新数据SQL表达式函数信息
        /// </summary>
        internal override MethodInfo UpdateMethod { get { return ((Action<CharStream, T, ConstantConverter, string>)ColumnGroup.Column<T>.Updater.Update).Method; } }

        /// <summary>
        /// 数据列转换数组
        /// </summary>
        /// <param name="values">目标数组</param>
        /// <param name="value">数据列</param>
        /// <param name="index">当前读取位置</param>
        private delegate void ToArray(T value, object[] values, ref int index);
        /// <summary>
        /// 数据列转换数组函数信息
        /// </summary>
        internal override MethodInfo ToArrayMethod { get { return ((ToArray)ColumnGroup.Column<T>.ToArray.Write).Method; } }

        /// <summary>
        /// 设置字段值
        /// </summary>
        /// <param name="reader">字段读取器物理存储</param>
        /// <param name="value">目标数据</param>
        /// <param name="index">当前读取位置</param>
        internal delegate void Set(DbDataReader reader, ref T value, ref int index);
        /// <summary>
        /// 设置字段值函数信息
        /// </summary>
        internal override MethodInfo SetMethod { get { return ((Set)ColumnGroup.Column<T>.Setter.Set).Method; } }

        /// <summary>
        /// 获取列名集合函数信息
        /// </summary>
        internal override Delegate InserterGetColumnNamesMethod { get { return (Func<string, string>)ColumnGroup.Column<T>.Inserter.GetColumnNames; } }

        /// <summary>
        /// 获取插入数据SQL表达式函数信息
        /// </summary>
        internal override MethodInfo InsertMethod { get { return ((Action<CharStream, T, ConstantConverter>)ColumnGroup.Column<T>.Inserter.Insert).Method; } }

        /// <summary>
        /// 获取成员名称与类型集合函数信息
        /// </summary>
        internal override Delegate GetDataColumnsMethod { get { return (Func<string, KeyValue<string, Type>[]>)ColumnGroup.Column<T>.GetDataColumns; } }
    }
}
