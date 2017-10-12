using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.DataSetSerialize
{
    /// <summary>
    /// DataSet 包装
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
    public sealed class DataSet
    {
        /// <summary>
        /// 数据源
        /// </summary>
        private DataSource data;
        /// <summary>
        /// DataSet名称
        /// </summary>
        private string name;
        /// <summary>
        /// 数据表格集合
        /// </summary>
        private DataTable[] tables;
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="set"></param>
        public static implicit operator DataSet(System.Data.DataSet set)
        {
            if (set != null)
            {
                DataSet value = new DataSet();
                value.from(set);
                return value;
            }
            return null;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator System.Data.DataSet(DataSet value)
        {
            if (value != null)
            {
                System.Data.DataSet set = new System.Data.DataSet(value.name);
                bool isSet = false;
                try
                {
                    value.set(set);
                    isSet = true;
                    return set;
                }
                finally
                {
                    if (!isSet) set.Dispose();
                }
            }
            return null;
        }
        /// <summary>
        /// DataSet包装
        /// </summary>
        /// <param name="set"></param>
        private void from(System.Data.DataSet set)
        {
            if (set.Tables.Count != 0)
            {
                using (DataWriter builder = new DataWriter())
                {
                    tables = set.Tables.toGeneric<System.Data.DataTable>().getArray(table => DataTable.From(table, builder));
                    builder.Get(ref data);
                }
            }
            name = set.DataSetName;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="set"></param>
        /// <returns>序列化数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static byte[] Serialize(System.Data.DataSet set)
        {
            return AutoCSer.BinarySerialize.Serializer.Serialize((DataSet)set);
        }
        /// <summary>
        /// DataSet拆包
        /// </summary>
        /// <param name="set"></param>
        private unsafe void set(System.Data.DataSet set)
        {
            if (tables.length() != 0)
            {
                fixed (byte* dataFixed = data.Data)
                {
                    DataReader builder = new DataReader(dataFixed, data.Strings, data.Bytes);
                    foreach (DataTable table in tables) set.Tables.Add(table.Get(builder));
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data">序列化数据</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static System.Data.DataSet DeSerialize(byte[] data)
        {
            return (System.Data.DataSet)AutoCSer.BinarySerialize.DeSerializer.DeSerialize<DataSet>(data);
        }
    }
}
