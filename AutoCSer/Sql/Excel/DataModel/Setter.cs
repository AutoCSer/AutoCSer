using System;
using System.Data.Common;
using AutoCSer.Metadata;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Sql.Excel.DataModel
{
    /// <summary>
    /// 数据模型
    /// </summary>
    internal abstract partial class Model<modelType>
    {
        /// <summary>
        /// 数据库模型设置
        /// </summary>
        internal static class Setter
        {
            /// <summary>
            /// 设置字段值
            /// </summary>
            /// <param name="reader">字段读取器物理存储</param>
            /// <param name="value">目标数据</param>
            /// <param name="memberMap">成员位图</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public static void Set(DbDataReader reader, modelType value, MemberMap memberMap)
            {
                if (setter != null) setter(reader, value, memberMap);
            }
            /// <summary>
            /// 默认数据列设置
            /// </summary>
            private static readonly Action<DbDataReader, modelType, MemberMap> setter;

            static Setter()
            {
                if (AutoCSer.Sql.DataModel.Model<modelType>.Attribute != null)
                {
                    AutoCSer.Sql.DataModel.Setter dynamicMethod = new AutoCSer.Sql.DataModel.Setter(typeof(modelType), ClientKind.Excel);
                    foreach (Field member in AutoCSer.Sql.DataModel.Model<modelType>.Fields) dynamicMethod.Push(member);
                    setter = (Action<DbDataReader, modelType, MemberMap>)dynamicMethod.Create<Action<DbDataReader, modelType, MemberMap>>();
                }
            }
        }
    }
}
