using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Oracle
{
    /// <summary>
    /// 常量转换
    /// </summary>
    internal sealed class ConstantConverter: AutoCSer.Sql.ConstantConverter
    {
        /// <summary>
        /// 常量转换
        /// </summary>
        internal ConstantConverter()
        {
            converters[typeof(DateTime)] = convertConstantDateTime;
            converters[typeof(DateTime?)] = convertConstantDateTimeNullable;
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        protected override void convertConstant(CharStream sqlStream, DateTime value)
        {
            sqlStream.PrepLength(AutoCSer.Date.MillisecondStringSize + 9 + 26);
            sqlStream.UnsafeWrite("to_date('");
            AutoCSer.Date.ToString((DateTime)value, sqlStream);
            sqlStream.UnsafeWrite("','yyyy/mm/dd hh24:mi:ss')");
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantDateTime(CharStream sqlStream, object value)
        {
            convertConstant(sqlStream, (DateTime)value);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, DateTime? value)
        {
            if (value == null) sqlStream.WriteJsonNull();
            else convertConstant(sqlStream, (DateTime)value);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantDateTimeNullable(CharStream sqlStream, object value)
        {
            convertConstant(sqlStream, (DateTime?)value);
        }
        /// <summary>
        /// SQL 关键字搜索器
        /// </summary>
        private static AutoCSer.StateSearcher.AsciiSearcher keywordSearcher = new AutoCSer.StateSearcher.AsciiSearcher(AutoCSer.StateSearcher.AsciiBuilder.Create(new string[] { "add", "all", "alter", "and", "any", "as", "asc", "authorization", "backup", "begin", "between", "break", "browse", "bulk", "by", "cascade", "case", "check", "checkpoint", "close", "clustered", "coalesce", "collate", "column", "commit", "compute", "constraint", "contains", "containstable", "continue", "convert", "create", "cross", "current", "current_date", "current_time", "current_timestamp", "current_user", "cursor", "database", "dbcc", "deallocate", "declare", "default", "delete", "deny", "desc", "disk", "distinct", "distributed", "double", "drop", "dump", "else", "end", "errlvl", "escape", "except", "exec", "execute", "exists", "exit", "external", "fetch", "file", "fillfactor", "for", "foreign", "freetext", "freetexttable", "full", "function", "goto", "grant", "group", "having", "holdlock", "identity", "identity_insert", "identitycol", "if", "in", "index", "inner", "insert", "intersect", "into", "is", "join", "key", "kill", "left", "like", "lineno", "load", "merge", "national", "nocheck", "nonclustered", "not", "null", "nullif", "of", "off", "offsets", "on", "open", "opendatasource", "openquery", "openrowset", "openxml", "option", "or", "order", "outer", "over", "percent", "pivot", "plan", "precision", "primary", "print", "proc", "procedure", "public", "raiserror", "read", "readtext", "reconfigure", "references", "replication", "restore", "restrict", "return", "revert", "revoke", "right", "rollback", "rowcount", "rowguidcol", "rule", "save", "schema", "securityaudit", "select", "semantickeyphrasetable", "semanticsimilaritydetailstable", "semanticsimilaritytable", "session_user", "set", "setuser", "shutdown", "some", "statistics", "system_user", "table", "tablesample", "textsize", "then", "top", "tran", "transaction", "trigger", "truncate", "try_convert", "tsequal", "union", "unique", "unpivot", "update", "updatetext", "uid", "use", "user", "values", "varying", "view", "waitfor", "when", "where", "while", "with", "within", "writetext" }, true).Pointer);
        /// <summary>
        /// SQL名称关键字处理
        /// </summary>
        /// <param name="name"></param>
        /// <returns>SQL名称</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal override string ConvertName(string name)
        {
            return keywordSearcher.SearchLower(name) < 0 ? name : (@"""" + name + @"""");
        }
        /// <summary>
        /// SQL名称关键字处理
        /// </summary>
        /// <param name="sqlStream"></param>
        /// <param name="name"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal override void ConvertNameToSqlStream(CharStream sqlStream, string name)
        {
            if (keywordSearcher.SearchLower(name) < 0) sqlStream.SimpleWriteNotNull(name);
            else
            {
                sqlStream.Write('"');
                sqlStream.SimpleWriteNotNull(name);
                sqlStream.Write('"');
            }
        }

        /// <summary>
        /// 常量转换
        /// </summary>
        internal static new readonly ConstantConverter Default = new ConstantConverter();
    }
}
