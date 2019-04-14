using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.DataSetSerialize
{
    /// <summary>
    /// 数据表格
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
    public sealed class DataTable
    {
        /// <summary>
        /// 数据源
        /// </summary>
        private DataSource data;
        /// <summary>
        /// 表格名称
        /// </summary>
        private string name;
        /// <summary>
        /// 表格名称
        /// </summary>
        public string Name { get { return name; } }
        /// <summary>
        /// 列名集合
        /// </summary>
        private string[] columnNames;
        /// <summary>
        /// 列数
        /// </summary>
        public int ColunmCount
        {
            get { return columnNames.length(); }
        }
        /// <summary>
        /// 列类型集合
        /// </summary>
        private byte[] columnTypes;
        /// <summary>
        /// 空数据位图
        /// </summary>
        private byte[] dbNull;
        /// <summary>
        /// 数据行数
        /// </summary>
        private int rowCount;
        /// <summary>
        /// 数据行数
        /// </summary>
        public int RowCount { get { return RowCount; } }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="table"></param>
        public static implicit operator DataTable(System.Data.DataTable table)
        {
            if (table != null)
            {
                DataTable value = new DataTable();
                if ((value.rowCount = table.Rows.Count) != 0)
                {
                    using (DataWriter builder = new DataWriter())
                    {
                        value.from(table, builder);
                        builder.Get(ref value.data);
                    }
                }
                return value;
            }
            return null;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        public unsafe static implicit operator System.Data.DataTable(DataTable value)
        {
            if (value != null)
            {
                System.Data.DataTable table = new System.Data.DataTable(value.name);
                if (value.rowCount == 0) return table;
                bool isTable = false;
                try
                {
                    fixed (byte* dataFixed = value.data.Data)
                    {
                        DataReader builder = new DataReader(dataFixed, value.data.Strings, value.data.Bytes);
                        value.get(table, builder);
                    }
                    isTable = true;
                    return table;
                }
                finally
                {
                    if (!isTable) table.Dispose();
                }
            }
            return null;
        }
        /// <summary>
        /// DataTable 包装
        /// </summary>
        /// <param name="table"></param>
        /// <param name="builder">数据流包装器</param>
        private unsafe void from(System.Data.DataTable table, DataWriter builder)
        {
            int index = 0;
            columnNames = new string[table.Columns.Count];
            fixed (byte* columnFixed = columnTypes = new byte[columnNames.Length])
            {
                byte* columnIndex = columnFixed;
                foreach (System.Data.DataColumn column in table.Columns)
                {
                    if (!typeIndexs.TryGetValue(column.DataType, out *columnIndex)) *columnIndex = 255;
                    ++columnIndex;
                    columnNames[index++] = column.ColumnName;
                }
                fixed (byte* nullFixed = dbNull = new byte[(columnNames.Length * rowCount + 7) >> 3])
                {
                    MemoryMap nullMap = new MemoryMap(nullFixed);
                    index = 0;
                    foreach (System.Data.DataRow row in table.Rows)
                    {
                        columnIndex = columnFixed;
                        foreach (object value in row.ItemArray)
                        {
                            if (value == DBNull.Value) nullMap.Set(index);
                            else builder.Append(value, *columnIndex);
                            ++index;
                            ++columnIndex;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="table"></param>
        /// <returns>序列化数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static byte[] Serialize(System.Data.DataTable table)
        {
            return AutoCSer.BinarySerialize.Serializer.Serialize((DataTable)table);
        }
        /// <summary>
        /// DataTable包装
        /// </summary>
        /// <param name="table"></param>
        /// <param name="builder">数据流包装器</param>
        /// <returns></returns>
        internal static DataTable From(System.Data.DataTable table, DataWriter builder)
        {
            DataTable value = new DataTable();
            if ((value.rowCount = table.Rows.Count) != 0) value.from(table, builder);
            value.name = table.TableName;
            return value;
        }
        /// <summary>
        /// DataTable拆包
        /// </summary>
        /// <param name="builder">数据对象拆包器</param>
        /// <returns></returns>
        internal System.Data.DataTable Get(DataReader builder)
        {
            System.Data.DataTable table = new System.Data.DataTable(name);
            if (rowCount != 0)
            {
                bool isGet = false;
                try
                {
                    get(table, builder);
                    isGet = true;
                }
                finally
                {
                    if (!isGet)
                    {
                        table.Dispose();
                        table = null;
                    }
                }
            }
            return table;
        }
        /// <summary>
        /// DataTable拆包
        /// </summary>
        /// <param name="table"></param>
        /// <param name="builder">数据对象拆包器</param>
        private unsafe void get(System.Data.DataTable table, DataReader builder)
        {
            int index = 0;
            System.Data.DataColumn[] columns = new System.Data.DataColumn[columnNames.Length];
            fixed (byte* columnFixed = columnTypes)
            {
                byte* columnIndex = columnFixed;
                foreach (string columnName in columnNames)
                {
                    columns[index++] = new System.Data.DataColumn(columnName, *columnIndex < types.Length ? types[*columnIndex] : typeof(object));
                    ++columnIndex;
                }
                table.Columns.AddRange(columns);
                fixed (byte* nullFixed = dbNull)
                {
                    MemoryMap nullMap = new MemoryMap(nullFixed);
                    for (int rowCount = index = 0; rowCount != this.rowCount; ++rowCount)
                    {
                        object[] values = new object[columnNames.Length];
                        columnIndex = columnFixed;
                        for (int valueIndex = 0; valueIndex != columnNames.Length; ++valueIndex)
                        {
                            values[valueIndex] = nullMap.Get(index++) == 0 ? builder.Get(*columnIndex) : DBNull.Value;
                            ++columnIndex;
                        }
                        System.Data.DataRow row = table.NewRow();
                        row.ItemArray = values;
                        table.Rows.Add(row);
                    }
                }
            }
        }
        /// <summary>
        /// 自定义读取数据表格
        /// </summary>
        /// <param name="reader"></param>
        public unsafe void CustomRead(IDataTableReader reader)
        {
            fixed (byte* dataFixed = data.Data)
            {
                DataReader builder = new DataReader(dataFixed, data.Strings, data.Bytes);
                int index = 0;
                fixed (byte* columnFixed = columnTypes)
                {
                    byte* columnIndex = columnFixed;
                    foreach (string columnName in columnNames)
                    {
                        reader.SetColumn(index++, columnName, *columnIndex < types.Length ? types[*columnIndex] : typeof(object));
                        ++columnIndex;
                    }
                    fixed (byte* nullFixed = dbNull)
                    {
                        MemoryMap nullMap = new MemoryMap(nullFixed);
                        for (int rowCount = index = 0; rowCount != this.rowCount; ++rowCount)
                        {
                            columnIndex = columnFixed;
                            for (int valueIndex = 0; valueIndex != columnNames.Length; ++valueIndex)
                            {
                                reader.SetValue(rowCount, valueIndex, nullMap.Get(index++) == 0 ? builder.Get(*columnIndex) : DBNull.Value);
                                ++columnIndex;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data">序列化数据</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static System.Data.DataTable DeSerialize(byte[] data)
        {
            return (System.Data.DataTable)AutoCSer.BinarySerialize.DeSerializer.DeSerialize<DataTable>(data);
        }
        /// <summary>
        /// 类型集合
        /// </summary>
        private static readonly Type[] types;
        /// <summary>
        /// 类型索引集合
        /// </summary>
        private static readonly Dictionary<Type, byte> typeIndexs;
        static DataTable()
        {
            int index = 0;
            types = new Type[30];
            typeIndexs = DictionaryCreator.CreateOnly<Type, byte>();
            typeIndexs.Add(types[index] = typeof(int), (byte)index++);
            typeIndexs.Add(types[index] = typeof(int?), (byte)index++);
            typeIndexs.Add(types[index] = typeof(string), (byte)index++);
            typeIndexs.Add(types[index] = typeof(DateTime), (byte)index++);
            typeIndexs.Add(types[index] = typeof(DateTime?), (byte)index++);
            typeIndexs.Add(types[index] = typeof(double), (byte)index++);
            typeIndexs.Add(types[index] = typeof(double?), (byte)index++);
            typeIndexs.Add(types[index] = typeof(float), (byte)index++);
            typeIndexs.Add(types[index] = typeof(float?), (byte)index++);
            typeIndexs.Add(types[index] = typeof(decimal), (byte)index++);
            typeIndexs.Add(types[index] = typeof(decimal?), (byte)index++);
            typeIndexs.Add(types[index] = typeof(Guid), (byte)index++);
            typeIndexs.Add(types[index] = typeof(Guid?), (byte)index++);
            typeIndexs.Add(types[index] = typeof(bool), (byte)index++);
            typeIndexs.Add(types[index] = typeof(bool?), (byte)index++);
            typeIndexs.Add(types[index] = typeof(byte), (byte)index++);
            typeIndexs.Add(types[index] = typeof(byte?), (byte)index++);
            typeIndexs.Add(types[index] = typeof(byte[]), (byte)index++);
            typeIndexs.Add(types[index] = typeof(sbyte), (byte)index++);
            typeIndexs.Add(types[index] = typeof(sbyte?), (byte)index++);
            typeIndexs.Add(types[index] = typeof(short), (byte)index++);
            typeIndexs.Add(types[index] = typeof(short?), (byte)index++);
            typeIndexs.Add(types[index] = typeof(ushort), (byte)index++);
            typeIndexs.Add(types[index] = typeof(ushort?), (byte)index++);
            typeIndexs.Add(types[index] = typeof(uint), (byte)index++);
            typeIndexs.Add(types[index] = typeof(uint?), (byte)index++);
            typeIndexs.Add(types[index] = typeof(long), (byte)index++);
            typeIndexs.Add(types[index] = typeof(long?), (byte)index++);
            typeIndexs.Add(types[index] = typeof(ulong), (byte)index++);
            typeIndexs.Add(types[index] = typeof(ulong?), (byte)index++);
        }
    }
}
