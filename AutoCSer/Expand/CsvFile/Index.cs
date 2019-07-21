using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CsvFile
{
    /// <summary>
    /// CSV 字符串读取器索引
    /// </summary>
    public sealed class Index
    {
        /// <summary>
        /// 索引名称集合，不指定为所有数据
        /// </summary>
        private readonly HashSet<string> nameHash;
        /// <summary>
        /// 索引名称集合
        /// </summary>
        private readonly Dictionary<string, int> names = DictionaryCreator.CreateOnly<string, int>();
        /// <summary>
        /// 索引集合
        /// </summary>
        private Dictionary<int, int> indexs = DictionaryCreator.CreateInt<int>();
        /// <summary>
        /// 索引映射数组
        /// </summary>
        private int[] indexArray;
        /// <summary>
        /// CSV 解析数据对象集合
        /// </summary>
        private LeftArray<ObjectReader> objectReaders;
        /// <summary>
        /// CSV 解析数据对象集合
        /// </summary>
        public LeftArray<ObjectReader> ObjectReaders
        {
            get { return objectReaders; }
        }
        /// <summary>
        /// 数据列数量
        /// </summary>
        internal int ColCount = 0;
        /// <summary>
        /// 最大的数据列索引号
        /// </summary>
        private int maxCol = -1;
        /// <summary>
        /// 当前解析 CSV 解析数据对象
        /// </summary>
        private ObjectReader currentObject;
        /// <summary>
        /// CSV 字符串读取器索引
        /// </summary>
        /// <param name="names">索引名称集合，不指定为所有数据</param>
        public Index(string[] names = null)
        {
            if (names != null) nameHash = new HashSet<string>(names);
        }
        /// <summary>
        /// 添加索引名称
        /// </summary>
        /// <param name="item"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void AppendName(ReaderItem item)
        {
            appendName(item.String, item.Col);
        }
        /// <summary>
        /// 添加索引名称
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ignorePrefix">忽略前缀</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void AppendName(ReaderItem item, char ignorePrefix)
        {
            appendName(item.GetString(ignorePrefix), item.Col);
        }
        /// <summary>
        /// 添加索引名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="col"></param>
        private void appendName(string name, int col)
        {
            if (nameHash == null || nameHash.Contains(name))
            {
                indexs[col] = ColCount;
                names[name] = ColCount;
                maxCol = col;
                ++ColCount;
            }
        }
        /// <summary>
        /// 创建索引映射数组
        /// </summary>
        private void createIndexArray()
        {
            indexArray = new int[maxCol + 1];
            for (int index = 0; index != indexArray.Length; indexArray[index++] = -1) ;
            foreach (KeyValuePair<int, int> index in indexs) indexArray[index.Key] = index.Value;
            indexs = null;
        }
        /// <summary>
        /// 当列号为 0 时创建下一个待解析 CSV 数据对象
        /// </summary>
        /// <param name="item"></param>
        private void tryCreateNextObject(ref ReaderItem item)
        {
            if (currentObject != null && item.Row == currentObject.Row) currentObject.SetEnd(ref item);
            else
            {
                if (indexArray == null) createIndexArray();
                objectReaders.Add(currentObject = new ObjectReader(this, ref item));
            }
        }
        /// <summary>
        /// 根据数据列号获取数据索引位置
        /// </summary>
        /// <param name="col">数据列号</param>
        /// <returns>失败返回 -1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private int getIndex(int col)
        {
            return col < indexArray.Length ? indexArray[col] : -1;
        }
        /// <summary>
        /// 根据名称获取数据索引位置
        /// </summary>
        /// <param name="name"></param>
        /// <returns>失败返回 -1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int GetIndex(string name)
        {
            int index;
            return names.TryGetValue(name, out index) ? index : -1;
        }
        /// <summary>
        /// 根据内容获取数据索引位置
        /// </summary>
        /// <param name="item"></param>
        /// <returns>失败返回 -1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int GetIndex(ReaderItem item)
        {
            return getIndex(item.Col);
        }
        /// <summary>
        /// 设置 CSV 数据对象数据
        /// </summary>
        /// <param name="item"></param>
        /// <returns>是否设置成功，数据列不存在时返回 false</returns>
        public bool SetObject(ReaderItem item)
        {
            tryCreateNextObject(ref item);
            int index = getIndex(item.Col);
            if (index >= 0)
            {
                currentObject.ValueArray[index] = item.String;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置 CSV 数据对象数据
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ignorePrefix">数据忽略前缀</param>
        /// <returns>是否设置成功，数据列不存在时返回 false</returns>
        public bool SetObject(ReaderItem item, char ignorePrefix)
        {
            tryCreateNextObject(ref item);
            int index = getIndex(item.Col);
            if (index >= 0)
            {
                currentObject.ValueArray[index] = item.GetString(ignorePrefix);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置 CSV 数据对象数据
        /// </summary>
        /// <param name="item"></param>
        /// <param name="value"></param>
        /// <returns>是否设置成功，数据列不存在时返回 false</returns>
        public bool SetObject(ReaderItem item, string value)
        {
            tryCreateNextObject(ref item);
            int index = getIndex(item.Col);
            if (index >= 0)
            {
                currentObject.ValueArray[index] = value;
                return true;
            }
            return false;
        }
    }
}
