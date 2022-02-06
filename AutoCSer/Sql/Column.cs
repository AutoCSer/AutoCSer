using System;
using System.Data;
using AutoCSer.Extensions;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 数据列
    /// </summary>
    internal sealed class Column
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string Name;
        /// <summary>
        /// 表格列类型
        /// </summary>
        public Type SqlColumnType;
        /// <summary>
        /// 备注说明
        /// </summary>
        public string Remark;
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue;
        /// <summary>
        /// 新增字段时的计算子查询
        /// </summary>
        public string UpdateValue;
        /// <summary>
        /// 列长
        /// </summary>
        public int Size;
        /// <summary>
        /// 数据库类型
        /// </summary>
        public SqlDbType DbType;
        /// <summary>
        /// 是否允许为空
        /// </summary>
        public bool IsNull;

        /// <summary>
        /// 判断是否匹配数据列
        /// </summary>
        /// <param name="table"></param>
        /// <param name="value">数据列</param>
        /// <param name="isIgnoreCase">是否忽略大小写</param>
        /// <param name="isIgnoreMatchDateTime">是否忽略 DateTime 类型不匹配错误</param>
        /// <returns>是否匹配</returns>
        internal bool IsMatch(Table table, Column value, bool isIgnoreCase, bool isIgnoreMatchDateTime)
        {
            if (value != null)
            {
                if (DbType != value.DbType)
                {
                    switch (DbType)
                    {
                        case SqlDbType.DateTime:
                        case SqlDbType.DateTime2:
                        case SqlDbType.SmallDateTime:
                            if (isIgnoreMatchDateTime)
                            {
                                switch (value.DbType)
                                {
                                    case SqlDbType.DateTime:
                                    case SqlDbType.DateTime2:
                                    case SqlDbType.SmallDateTime:
                                        table.Log.Debug("表格 " + table.TableName + " 字段 " + Name + " 数据库类型不匹配 : " + DbType.ToString() + " <> " + value.DbType.ToString());
                                        break;
                                    default: return false;
                                }
                            }
                            else return false;
                            break;
                        default: return false;
                    }
                }
                return Size == value.Size && IsNull == value.IsNull && (isIgnoreCase ? Name.equalCase(value.Name) : Name == value.Name);
            }
            return false;
        }
    }
}
