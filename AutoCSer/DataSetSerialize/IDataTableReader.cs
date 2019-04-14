using System;

namespace AutoCSer.DataSetSerialize
{
    /// <summary>
    /// 自定义数据表格读取
    /// </summary>
    public interface IDataTableReader
    {
        /// <summary>
        /// 设置列信息
        /// </summary>
        /// <param name="columnIndex">列号</param>
        /// <param name="columnName">列名</param>
        /// <param name="dataType">数据类型</param>
        void SetColumn(int columnIndex, string columnName, Type dataType);
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        /// <param name="value">数据</param>
        void SetValue(int row, int column, object value);
    }
}
