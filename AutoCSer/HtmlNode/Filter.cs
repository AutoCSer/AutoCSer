using System;
using AutoCSer.Extension;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.HtmlNode
{
    /// <summary>
    /// 节点筛选器
    /// </summary>
    public unsafe sealed class Filter : IDisposable
    {
        /// <summary>
        /// 下级筛选器
        /// </summary>
        private Filter nextFilter;
        /// <summary>
        /// 当前筛选节点匹配名称
        /// </summary>
        private string name;
        /// <summary>
        /// 当前筛选节点匹配值
        /// </summary>
        private string value;
        /// <summary>
        /// 当前筛选节点匹配多值集合
        /// </summary>
        private Pointer.Size valueData;
        /// <summary>
        /// 当前筛选节点匹配多值集合
        /// </summary>
        private bool isValueData
        {
            get { return valueData.Data != null; }
        }
        /// <summary>
        /// 当前筛选节点匹配多值集合
        /// </summary>
        private StateSearcher.CharSearcher values;
        /// <summary>
        /// 当前筛选节点匹配多位置集合
        /// </summary>
        private int[] indexs;
        /// <summary>
        /// 当前筛选节点匹配位置
        /// </summary>
        private int index = -1;
        /// <summary>
        /// 当前结点深度
        /// </summary>
        private int depth;
        /// <summary>
        /// 去重数组大小
        /// </summary>
        private int distinctArraySize;
        /// <summary>
        /// 节点筛选器类型
        /// </summary>
        private FilterType type;
        /// <summary>
        /// 当前筛选节点匹配值是否小写
        /// </summary>
        private bool isValueLower;
        /// <summary>
        /// 子孙节点筛选是否需要去重
        /// </summary>
        private bool isDistinctNode;
        /// <summary>
        /// 节点筛选器解析
        /// </summary>
        /// <param name="start">起始字符位置</param>
        /// <param name="end">结束字符位置</param>
        /// <param name="depth">当前结点深度</param>
        private Filter(char* start, char* end, int depth)
        {
            this.depth = depth;
            byte* bits = Node.Bits.Byte;
            if (((bits[*(byte*)start] & Node.FilterBit) | *(((byte*)start) + 1)) == 0)
            {
                switch (*start & 7)
                {
                    case '/' & 7:
                        type = FilterType.Child;
                        if (++start != end && start != (end = next(start, end)))
                        {
                            char* index = end;
                            if (*--index == ']' && (index = AutoCSer.Extension.StringExtension.FindNotNull(start, index, '[')) != null)
                            {
                                getValue(start, index);
                                getIndex(++index, --end);
                            }
                            else getLowerValues(start, end);
                        }
                        break;
                    case '.' & 7:
                        type = FilterType.Class;
                        if (++start != end) getLowerValues(start, end = next(start, end));
                        break;
                    case '#' & 7:
                        name = "id";
                        type = FilterType.Value;
                        if (++start != end) getValues(start, end = next(start, end));
                        break;
                    case '*' & 7:
                        //case ':' & 7:
                        if (*start == '*')
                        {
                            name = "name";
                            type = FilterType.Value;
                            if (++start != end) getLowerValues(start, next(start, end));
                        }
                        else
                        {
                            name = "type";
                            type = FilterType.Value;
                            if (++start != end) getLowerValues(start, end = next(start, end));
                        }
                        break;
                    case '@' & 7:
                        type = FilterType.Value;
                        if (++start != end)
                        {
                            end = next(start, end);
                            char* value = AutoCSer.Extension.StringExtension.FindNotNull(start, end, '=');
                            if (value != null)
                            {
                                getName(start, value);
                                if (++value == end) this.value = string.Empty;
                                else getLowerValues(value, end);
                            }
                            else getName(start, end);
                        }
                        break;
                }
            }
            else
            {
                type = FilterType.Node;
                if (*start == '\\') ++start;
                if (start != end) getLowerValues(start, next(start, end));
            }
            if (depth == 0)
            {
                int nodeCount = type == FilterType.Node ? 1 : 0;
                distinctArraySize = 1;
                for (Filter filter = nextFilter; filter != null; filter = filter.nextFilter)
                {
                    if (filter.type == FilterType.Node && ++nodeCount >= 2) filter.isDistinctNode = true;
                    ++distinctArraySize;
                }
                if (nodeCount >= 2) isDistinctNode = true;
            }
        }
        /// <summary>
        ///  析构释放资源
        /// </summary>
        ~Filter()
        {
            Unmanaged.Free(ref valueData);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Unmanaged.Free(ref valueData);
            if (nextFilter != null) nextFilter.Dispose();
        }
        /// <summary>
        /// 解析下一个筛选功能
        /// </summary>
        /// <param name="start">起始字符位置</param>
        /// <param name="end">结束字符位置</param>
        /// <returns>字符位置</returns>
        private char* next(char* start, char* end)
        {
            while (((Node.Bits.Byte[*(byte*)start] & Node.FilterBit) | *(((byte*)start) + 1)) != 0)
            {
                if (++start == end) return end;
            }
            nextFilter = new Filter(start, end, depth + 1);
            return start;
        }
        /// <summary>
        /// 解析多值集合
        /// </summary>
        /// <param name="start">起始字符位置</param>
        /// <param name="end">结束字符位置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void getLowerValues(char* start, char* end)
        {
            if (start != end)
            {
                isValueLower = true;
                (this.value = new string(start, 0, (int)(end - start))).toLower();
                if (AutoCSer.Extension.StringExtension.FindNotNull(start, end, ',') != null) getValues();
            }
        }
        /// <summary>
        /// 解析多值集合
        /// </summary>
        /// <param name="start">起始字符位置</param>
        /// <param name="end">结束字符位置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void getValues(char* start, char* end)
        {
            if (start != end)
            {
                this.value = new string(start, 0, (int)(end - start));
                if (AutoCSer.Extension.StringExtension.FindNotNull(start, end, ',') != null) getValues();
            }
        }
        /// <summary>
        /// 解析多值集合
        /// </summary>
        private void getValues()
        {
            HashSet<SubString> hashSet = HashSetCreator.CreateSubString();
            foreach (SubString value in this.value.split(','))
            {
                if (value.Length != 0) hashSet.Add(value);
            }
            this.value = null;
            if (hashSet.Count > 1)
            {
                valueData = AutoCSer.StateSearcher.CharBuilder.Create(hashSet.getArray(value => (string)value), false);
                values = new StateSearcher.CharSearcher(valueData.Pointer);
            }
            else
            {
                foreach (string value in hashSet) this.value = value;
            }
        }
        /// <summary>
        /// 解析值
        /// </summary>
        /// <param name="start">起始字符位置</param>
        /// <param name="end">结束字符位置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void getValue(char* start, char* end)
        {
            if (start != end) value = new string(start, 0, (int)(end - start));
        }
        /// <summary>
        /// 解析名称
        /// </summary>
        /// <param name="start">起始字符位置</param>
        /// <param name="end">结束字符位置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void getName(char* start, char* end)
        {
            if (start != end) name = new string(start, 0, (int)(end - start));
        }
        /// <summary>
        /// 解析索引位置
        /// </summary>
        /// <param name="start">起始字符位置</param>
        /// <param name="end">结束字符位置</param>
        private void getIndex(char* start, char* end)
        {
            if (start != end)
            {
                string value = new string(start, 0, (int)(end - start));
                if (AutoCSer.Extension.StringExtension.FindNotNull(start, end, ',') == null)
                {
                    if (!int.TryParse(value, out index)) index = -1;
                }
                else
                {
                    HashSet<int> hashSet = HashSetCreator.CreateInt();
                    int[] indexs = value.splitIntNoCheckNotEmpty(',');
                    foreach (int index in indexs) hashSet.Add(index);
                    if (hashSet.Count != indexs.Length)
                    {
                        if (hashSet.Count == 1)
                        {
                            foreach (int index in hashSet) this.index = index;
                        }
                        else this.indexs = hashSet.getArray();
                    }
                    else
                    {
                        if (indexs.Length != 1) Array.Sort(this.indexs = indexs);
                        else index = indexs[0];
                    }
                }
            }
        }
        /// <summary>
        /// 子孙节点筛选
        /// </summary>
        /// <param name="node">筛选节点</param>
        /// <returns>匹配的 HTML 节点集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private IEnumerable<Node> getTagNode(Node node)
        {
            foreach (Node nextNode in node.Nodes)
            {
                if (nextNode.Tag.EqualCase(value)) yield return nextNode;
            }
        }
        /// <summary>
        /// 子孙节点筛选
        /// </summary>
        /// <param name="node">筛选节点</param>
        /// <returns>匹配的 HTML 节点集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private IEnumerable<Node> getSearchNode(Node node)
        {
            foreach (Node nextNode in node.Nodes)
            {
                if (values.SearchLower(nextNode.Tag) >= 0) yield return nextNode;
            }
        }
        /// <summary>
        /// 子孙节点筛选
        /// </summary>
        /// <param name="node">筛选节点</param>
        /// <returns>匹配的 HTML 节点集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private IEnumerable<Node> getNode(Node node)
        {
            if (valueData.Data == null)
            {
                return value != null ? getTagNode(node) : node.Nodes;
            }
            return getSearchNode(node);
        }
        /// <summary>
        /// class 样式匹配
        /// </summary>
        /// <param name="node">筛选节点</param>
        /// <returns></returns>
        private bool isClass(Node node)
        {
            string className = node["class"];
            if (className != null)
            {
                if (valueData.Data == null)
                {
                    if (!string.IsNullOrEmpty(value) && className.Length >= value.Length)
                    {
                        fixed (char* classFixed = className, valueFixed = value)
                        {
                            char* start = classFixed, end = classFixed + className.Length;
                            start += value.Length;
                            do
                            {
                                do
                                {
                                    if (start == end) return AutoCSer.Extension.StringExtension.equalCaseNotNull(start - value.Length, valueFixed, value.Length);
                                    if (*start == ' ') break;
                                    ++start;
                                }
                                while (true);
                                if (AutoCSer.Extension.StringExtension.equalCaseNotNull(start - value.Length, valueFixed, value.Length)) return true;
                                start += value.Length + 1;
                            }
                            while (start <= end);
                        }
                    }
                }
                else
                {
                    fixed (char* classFixed = className)
                    {
                        char* start = classFixed, end = classFixed + className.Length;
                        for (char* current = start; current != end; ++current)
                        {
                            if (*current == ' ')
                            {
                                if (start != current && values.SearchLower(start, current) >= 0) return true;
                                start = ++current;
                                if (current == end) break;
                            }
                        }
                        if (start != end) return values.SearchLower(start, end) >= 0;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 子节点筛选
        /// </summary>
        /// <param name="node">筛选节点</param>
        /// <returns>匹配的 HTML 节点集合</returns>
        private IEnumerable<Node> getChild(Node node)
        {
            if (this.index < 0)
            {
                if (indexs == null)
                {
                    if (isValueData)
                    {
                        foreach (Node nextNode in node.ChildrenArray)
                        {
                            if (values.SearchLower(nextNode.Tag) >= 0) yield return nextNode;
                        }
                    }
                    else
                    {
                        if (value != null)
                        {
                            foreach (Node nextNode in node.ChildrenArray)
                            {
                                if (nextNode.Tag.EqualCase(value)) yield return nextNode;
                            }
                        }
                        else
                        {
                            foreach (Node nextNode in node.ChildrenArray) yield return nextNode;
                        }
                    }
                }
                else
                {
                    if (value != null)
                    {
                        int indexIndex = 0, index = indexs[0], count = 0;
                        foreach (Node nextNode in node.ChildrenArray)
                        {
                            if (nextNode.Tag.EqualCase(value))
                            {
                                if (count == index)
                                {
                                    yield return nextNode;
                                    if (++indexIndex == indexs.Length) break;
                                    index = indexs[indexIndex];
                                }
                                ++count;
                            }
                        }
                    }
                    else
                    {
                        foreach (int index in indexs)
                        {
                            if (index < node.ChildrenArray.Length)
                            {
                                yield return node.ChildrenArray.Array[index];
                            }
                            else break;
                        }
                    }
                }
            }
            else
            {
                if (value != null)
                {
                    int errorCount = node.ChildrenArray.Length - index;
                    if (errorCount > 0)
                    {
                        int count = index;
                        foreach (Node nextNode in node.ChildrenArray)
                        {
                            if (nextNode.Tag.EqualCase(value))
                            {
                                if (count == 0)
                                {
                                    yield return nextNode;
                                    break;
                                }
                                --count;
                            }
                            else if (--errorCount == 0) break;
                        }
                    }
                }
                else if (index < node.ChildrenArray.Length) yield return node.ChildrenArray.Array[index];
            }
        }
        /// <summary>
        /// 属性值配置
        /// </summary>
        /// <param name="node">筛选节点</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool isValue(Node node)
        {
            if (valueData.Data == null)
            {
                return value == null ? node.GetAttirbuteIndex(name) >= 0 : (isValueLower ? value.equalCase(node[name]) : value == node[name]);
            }
            return (isValueLower ? values.SearchLower(node[name]) : values.Search(node[name])) >= 0;
        }
        /// <summary>
        /// 根据筛选路径值匹配HTML节点集合
        /// </summary>
        /// <param name="node">筛选节点</param>
        /// <returns>匹配的 HTML 节点集合</returns>
        public IEnumerable<Node> Get(Node node)
        {
            if (node != null)
            {
                switch (type)
                {
                    case FilterType.Node:
                        if (nextFilter == null)
                        {
                            foreach (Node nextNode in getNode(node)) yield return nextNode;
                        }
                        else
                        {
                            foreach (Node nextNode in getNode(node))
                            {
                                foreach (Node returnNode in nextFilter.Get(nextNode)) yield return returnNode;
                            }
                        }
                        break;
                    case FilterType.Class:
                        if (isClass(node))
                        {
                            if (nextFilter == null) yield return node;
                            else
                            {
                                foreach (Node returnNode in nextFilter.Get(node)) yield return returnNode;
                            }
                        }
                        break;
                    case FilterType.Child:
                        if (nextFilter == null)
                        {
                            foreach (Node nextNode in getChild(node)) yield return nextNode;
                        }
                        else
                        {
                            foreach (Node nextNode in getChild(node))
                            {
                                foreach (Node returnNode in nextFilter.Get(nextNode)) yield return returnNode;
                            }
                        }
                        break;
                    case FilterType.Value:
                        if (isValue(node))
                        {
                            if (nextFilter == null) yield return node;
                            else
                            {
                                foreach (Node returnNode in nextFilter.Get(node)) yield return returnNode;
                            }
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// 子孙节点筛选
        /// </summary>
        /// <param name="node">筛选节点</param>
        /// <param name="nodes"></param>
        /// <returns>匹配的 HTML 节点集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private IEnumerable<Node> getTagNode(Node node, HashSet<Node> nodes)
        {
            foreach (Node nextNode in node.GetFilterNodes(nodes))
            {
                if (nextNode.Tag.EqualCase(value)) yield return nextNode;
            }
        }
        /// <summary>
        /// 子孙节点筛选
        /// </summary>
        /// <param name="node">筛选节点</param>
        /// <param name="nodes"></param>
        /// <returns>匹配的 HTML 节点集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private IEnumerable<Node> getSearchNode(Node node, HashSet<Node> nodes)
        {
            foreach (Node nextNode in node.GetFilterNodes(nodes))
            {
                if (values.SearchLower(nextNode.Tag) >= 0) yield return nextNode;
            }
        }
        /// <summary>
        /// 子孙节点筛选
        /// </summary>
        /// <param name="node">筛选节点</param>
        /// <param name="nodes"></param>
        /// <returns>匹配的 HTML 节点集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private IEnumerable<Node> getNode(Node node, HashSet<Node> nodes)
        {
            if (valueData.Data == null)
            {
                return value != null ? getTagNode(node, nodes) : node.GetFilterNodes(nodes);
            }
            return getSearchNode(node, nodes);
        }
        /// <summary>
        /// 根据筛选路径值匹配HTML节点集合
        /// </summary>
        /// <param name="node">筛选节点</param>
        /// <param name="nodes"></param>
        /// <returns>匹配的 HTML 节点集合</returns>
        private IEnumerable<Node> get(Node node, HashSet<Node>[] nodes)
        {
            if (node != null)
            {
                switch (type)
                {
                    case FilterType.Node:
                        if (isDistinctNode)
                        {
                            HashSet<Node> hashSet = nodes[depth];
                            if (hashSet == null || !hashSet.Contains(node))
                            {
                                if (hashSet == null) nodes[depth] = hashSet = HashSetCreator.CreateOnly<Node>();
                                hashSet.Add(node);
                                if (nextFilter == null)
                                {
                                    foreach (Node nextNode in getNode(node, hashSet)) yield return nextNode;
                                }
                                else
                                {
                                    foreach (Node nextNode in getNode(node, hashSet))
                                    {
                                        foreach (Node returnNode in nextFilter.get(nextNode, nodes)) yield return returnNode;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (nextFilter == null)
                            {
                                foreach (Node nextNode in getNode(node)) yield return nextNode;
                            }
                            else
                            {
                                foreach (Node nextNode in getNode(node))
                                {
                                    foreach (Node returnNode in nextFilter.get(nextNode, nodes)) yield return returnNode;
                                }
                            }
                        }
                        break;
                    case FilterType.Class:
                        if (isClass(node))
                        {
                            if (nextFilter == null) yield return node;
                            else
                            {
                                foreach (Node returnNode in nextFilter.get(node, nodes)) yield return returnNode;
                            }
                        }
                        break;
                    case FilterType.Child:
                        if (nextFilter == null)
                        {
                            foreach (Node nextNode in getChild(node)) yield return nextNode;
                        }
                        else
                        {
                            foreach (Node nextNode in getChild(node))
                            {
                                foreach (Node returnNode in nextFilter.get(nextNode, nodes)) yield return returnNode;
                            }
                        }
                        break;
                    case FilterType.Value:
                        if (isValue(node))
                        {
                            if (nextFilter == null) yield return node;
                            else
                            {
                                foreach (Node returnNode in nextFilter.get(node, nodes)) yield return returnNode;
                            }
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// 根据筛选路径值匹配HTML节点集合（去重操作）
        /// </summary>
        /// <param name="node">筛选节点</param>
        /// <returns>匹配的 HTML 节点集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public IEnumerable<Node> GetDistinct(Node node)
        {
            if (node != null)
            {
                if (isDistinctNode)
                {
                    HashSet<Node>[] nodes = new HashSet<Node>[distinctArraySize];
                    return get(node, nodes);
                }
                Get(node);
            }
            return null;
        }

        /// <summary>
        /// 节点筛选器解析缓存
        /// </summary>
        private static Dictionary<HashString, Filter> cache = DictionaryCreator.CreateHashString<Filter>();
        /// <summary>
        /// 根据筛选路径解析筛选器
        /// </summary>
        /// <param name="path">筛选路径</param>
        /// <returns>筛选器</returns>
        public static Filter Get(string path)
        {
            Filter value;
            HashString key = path;
            if (!cache.TryGetValue(key, out value))
            {
                fixed (char* pathFixed = path)
                {
                    cache[key] = value = new Filter(pathFixed, pathFixed + path.Length, 0);
                }
            }
            return value;
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private static void clearCache(int count)
        {
            if (cache.Count != 0) cache = DictionaryCreator.CreateHashString<Filter>();
        }
        static Filter()
        {
            byte* bits = Node.Bits.Byte;
            bits['/'] &= Node.FilterBit ^ 255;
            bits['.'] &= Node.FilterBit ^ 255;
            bits['#'] &= Node.FilterBit ^ 255;
            bits['*'] &= Node.FilterBit ^ 255;
            bits[':'] &= Node.FilterBit ^ 255;
            bits['@'] &= Node.FilterBit ^ 255;

            Pub.ClearCaches += clearCache;
        }
    }
}
