using System;
using System.Collections.Generic;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Web;

namespace AutoCSer.HtmlNode
{
    /// <summary>
    /// HTML 节点
    /// </summary>
    public unsafe sealed class Node
    {
        /// <summary>
        /// 子节点集合
        /// </summary>
        internal LeftArray<Node> ChildrenArray;
        /// <summary>
        /// 子节点集合
        /// </summary>
        public IEnumerable<Node> Children
        {
            get { return ChildrenArray; }
        }
        /// <summary>
        /// 子节点数量
        /// </summary>
        public int Count
        {
            get { return ChildrenArray.Length; }
        }
        /// <summary>
        /// 子节点索引位置
        /// </summary>
        /// <param name="value">子节点</param>
        /// <returns>索引位置</returns>
        public int this[Node value]
        {
            get
            {
                int index = -1;
                if (value != null && value.Parent == this)
                {
                    index = ChildrenArray.Length;
                    while (--index >= 0 && ChildrenArray.Array[index] != value) ;
                }
                return index;
            }
        }
        /// <summary>
        /// 属性集合
        /// </summary>
        internal LeftArray<KeyValue<SubString, FormatString>> AttributeArray;
        /// <summary>
        /// 属性数量
        /// </summary>
        public int AttributeCount
        {
            get { return AttributeArray.Length; }
        }
        /// <summary>
        /// 获取或设置属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <returns>属性值</returns>
        public string this[string name]
        {
            get
            {
                if (!string.IsNullOrEmpty(name))
                {
                    int index = GetAttirbuteIndex(name);
                    if (index >= 0) return AttributeArray.Array[index].Value.Text;
                }
                return null;
            }
        }
        /// <summary>
        /// 文本节点值
        /// </summary>
        private FormatString nodeText;
        /// <summary>
        /// 父节点
        /// </summary>
        public Node Parent { get; private set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        internal SubString Tag;
        /// <summary>
        /// 标签名称
        /// </summary>
        public SubString TagName { get { return Tag; } }
        /// <summary>
        /// 子孙节点枚举
        /// </summary>
        public IEnumerable<Node> Nodes
        {
            get
            {
                if (ChildrenArray.Length != 0)
                {
                    LeftArray<NodeIndex> indexs = default(LeftArray<NodeIndex>);
                    NodeIndex index = new NodeIndex { Array = ChildrenArray };
                    Node node = ChildrenArray.Array[0];
                    do
                    {
                        yield return node;
                    }
                    while ((node = index.MoveNext(ref node.ChildrenArray, ref indexs)) != null);
                }
            }
        }
        /// <summary>
        /// HTML
        /// </summary>
        public string InnerHTML
        {
            get
            {
                return GetHtml(false);
            }
        }
        /// <summary>
        /// 是否需要文本换行
        /// </summary>
        private bool isTextBr
        {
            get
            {
                switch (Tag.Length)
                {
                    case 1: return (Tag[0] | 0x20) == 'p';
                    case 2: return (Tag[0] | 0x20) == 'b' && (Tag[1] | 0x20) == 'r';
                }
                return false;
            }
        }
        /// <summary>
        /// 是否需要文本换行
        /// </summary>
        private bool isTextDiv
        {
            get
            {
                switch (Tag.Length)
                {
                    case 1: return (Tag[0] | 0x20) == 'p';
                    case 3: return (Tag[0] | 0x20) == 'd' && (Tag[1] | 0x20) == 'i' && (Tag[2] | 0x20) == 'v';
                }
                return false;
            }
        }
        /// <summary>
        /// 是否最后一级 Text 节点
        /// </summary>
        private bool isLastTextNode
        {
            get
            {
                return ChildrenArray.Length == 0 || Tag.Length == 0 || NoTextTagName.TagNames.Contains(Tag);
            }
        }
        /// <summary>
        /// 文本内容
        /// </summary>
        public unsafe string Text
        {
            get
            {
                if (Tag.Length == 0) return nodeText.Text.ToString() ?? string.Empty;
                if (!NoTextTagName.TagNames.Contains(Tag))
                {
                    if (ChildrenArray.Length != 0)
                    {
                        LeftArray<NodeIndex> indexs = default(LeftArray<NodeIndex>);
                        NodeIndex index = new NodeIndex { Array = ChildrenArray };
                        Node node = ChildrenArray.Array[0];
                        bool isSpace = false, isEnter = false;
                        byte* buffer = UnmanagedPool.Default.Get();
                        try
                        {
                            using (CharStream html = new CharStream((char*)buffer, UnmanagedPool.DefaultSize >> 1))
                            {
                                while (true)
                                {
                                    if (node.isTextBr)
                                    {
                                        isSpace = isEnter = false;
                                        html.SimpleWriteNotNull(@"
");
                                    }
                                    if (node.isLastTextNode)
                                    {
                                        if (node.Tag.Length == 0)
                                        {
                                            if (isEnter) html.SimpleWriteNotNull(@"
");
                                            else if (isSpace) html.Write(' ');
                                            html.Write(node.nodeText.Text);
                                            isSpace = true;
                                        }
                                        CHECK:
                                        if (index.NextCount() == 0)
                                        {
                                            if (indexs.Length == 0) break;
                                            index = indexs.UnsafePopOnly();
                                            isEnter = index.Current.isTextDiv;
                                            goto CHECK;
                                        }
                                        node = index.Current;
                                    }
                                    else
                                    {
                                        indexs.Add(index);
                                        node = index.SetNext(ref node.ChildrenArray);
                                    }
                                }
                                return html.ToString();
                            }
                        }
                        finally { UnmanagedPool.Default.Push(buffer); }
                    }
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// 是否 a 标签
        /// </summary>
        internal bool IsTagA
        {
            get { return Tag.Length == 1 && (Tag[0] | 0x20) == 'a'; }
        }
        /// <summary>
        /// HTML 节点
        /// </summary>
        private Node() { }
        /// <summary>
        /// HTML 节点
        /// </summary>
        /// <param name="formatHtml">编码过的 HTML 编码</param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        private Node(string formatHtml, int start, int count)
        {
            nodeText.FormatHtml.Set(formatHtml, start, count);
        }
        /// <summary>
        /// HTML 节点
        /// </summary>
        /// <param name="errorHtml">需要解码的 HTML</param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        private Node(int start, int count, string errorHtml)
        {
            string text = HttpUtility.HtmlDecode(errorHtml.Substring(start, count));
            nodeText.FormatText.Set(text, 0, text.Length);
        }
        /// <summary>
        /// HTML 节点
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="formatHtml">编码过的 HTML 编码</param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        private Node(string tagName, string formatHtml, int start, int count) : this(formatHtml, start, count)
        {
            this.Tag.Set(tagName, 0, tagName.Length);
            nodeText.FormatText = nodeText.FormatHtml;
        }
        /// <summary>
        /// HTML 节点
        /// </summary>
        /// <param name="html">HTML</param>
        public Node(string html) 
        {
            if (!string.IsNullOrEmpty(html))
            {
                fixed (char* htmlFixed = html)
                {
                    char* start = htmlFixed, end = htmlFixed + html.Length, current = end, endTag = null, endTagRound = null, endQuote1 = null, endQuote2 = null;
                    int endCount = 3;
                    while (current != start)
                    {
                        --current;
                        if (endTagRound == null)
                        {
                            if (*current == '>') endTagRound = current;
                        }
                        else
                        {
                            if (*current == '<')
                            {
                                if (endTag == null)
                                {
                                    endTag = current;
                                    if (--endCount == 0) break;
                                }
                            }
                            else if (*current == '\'')
                            {
                                if (endQuote1 == null)
                                {
                                    endQuote1 = current;
                                    if (--endCount == 0) break;
                                }
                            }
                            else if (*current == '"' && endQuote2 == null)
                            {
                                endQuote2 = current;
                                if (--endCount == 0) break;
                            }
                        }
                    }
                    if (endTag != null)
                    {
                        LeftArray<Node> children = new LeftArray<Node>();
                        LeftArray<KeyValue<SubString, FormatString>> attributes = default(LeftArray<KeyValue<SubString, FormatString>>);
                        Node nextNode;
                        SubString name = default(SubString);
                        FormatString value = default(FormatString);
                        char* script;
                        byte* bits = Bits.Byte;
                        bool isErrorHtml = false;
                        current = start;
                        if (endQuote1 == null) endQuote1 = start;
                        if (endQuote2 == null) endQuote2 = start;
                        while (current <= endTag)
                        {
                            while (*current != '<') ++current;
                            if ((*++current & 0xff80) == 0)
                            {
                                if ((bits[*(byte*)current] & tagNameBit) == 0)
                                {
                                    while (((bits[*(byte*)start] & spaceBit) | *(((byte*)start) + 1)) == 0) ++start;
                                    if (start != current - 1)
                                    {
                                        for (script = current - 2; ((bits[*(byte*)script] & spaceBit) | *(((byte*)script) + 1)) == 0; --script) ;
                                        if (isErrorHtml)
                                        {
                                            children.Add(new Node((int)(start - htmlFixed), (int)(script - start) + 1, html));
                                            isErrorHtml = false;
                                        }
                                        else children.Add(new Node(html, (int)(start - htmlFixed), (int)(script - start) + 1));
                                    }
                                    if (*current == '/')
                                    {
                                        #region 标签回合
                                        start = current - 1;
                                        do
                                        {
                                            ++current;
                                        }
                                        while (((bits[*(byte*)current] & spaceBit) | *(((byte*)current) + 1)) == 0);
                                        if ((uint)((*current | 0x20) - 'a') <= 26)
                                        {
                                            while (((bits[*(byte*)current] & tagNameSplitBit) | *(((byte*)current) + 1)) != 0) ++current;
                                            Tag.Set(html, (int)((start += 2) - htmlFixed), (int)(current - start));
                                            int startIndex = children.Length - 1;
                                            while (startIndex >= 0 && (children.Array[startIndex].nodeText.FormatHtml.String != null || !children.Array[startIndex].Tag.EqualCase(ref Tag))) --startIndex;
                                            if (startIndex != -1)
                                            {
                                                int nodeCount;
                                                for (int nextIndex = children.Length - 1; nextIndex != startIndex; --nextIndex)
                                                {
                                                    nextNode = children.Array[nextIndex];
                                                    if (nextNode.nodeText.FormatHtml.String == null)
                                                    {
                                                        if (MustRoundTagName.TagNames.Contains(nextNode.Tag) && (nodeCount = (children.Length - nextIndex - 1)) != 0)
                                                        {
                                                            nextNode.ChildrenArray = new LeftArray<Node>(new SubArray<Node>(nextIndex + 1, nodeCount, children.Array).GetArray());
                                                            children.RemoveRangeOnly(nextIndex + 1, nodeCount);
                                                            foreach (Node sonNode in nextNode.ChildrenArray.Array) sonNode.Parent = nextNode;
                                                        }
                                                    }
                                                    else if (nextNode.nodeText.FormatHtml.Length == 0) nextNode.nodeText.FormatHtml.SetNull();
                                                }
                                                nextNode = children.Array[startIndex];
                                                if ((nodeCount = children.Length - ++startIndex) != 0)
                                                {
                                                    nextNode.ChildrenArray = new LeftArray<Node>(new SubArray<Node>(startIndex, nodeCount, children.Array).GetArray());
                                                    children.RemoveRangeOnly(startIndex, nodeCount);
                                                    foreach (Node sonNode in nextNode.ChildrenArray.Array) sonNode.Parent = nextNode;
                                                }
                                                nextNode.nodeText.FormatHtml.Set(string.Empty, 0, 0);//已回合标识
                                            }
                                            while (*current != '>') ++current;
                                            ++current;
                                        }
                                        else
                                        {
                                            while (*current != '>') ++current;
                                            ++current;
                                            children.Add(new Node("/", html, (int)(start - htmlFixed), (int)(current - start)));
                                        }
                                        start = current;
                                        #endregion
                                    }
                                    else if (*current != '!')
                                    {
                                        #region 标签开始
                                        start = current;
                                        children.Add(nextNode = new Node());
                                        while (((bits[*(byte*)current] & tagNameSplitBit) | *(((byte*)current) + 1)) != 0) ++current;
                                        nextNode.Tag.Set(html, (int)(start - htmlFixed), (int)(current - start));//.toLower();
                                        #region 属性解析
                                        if (*current != '>')
                                        {
                                            start = ++current;
                                            attributes.Length = 0;
                                            do
                                            {
                                                if (current <= endTagRound)
                                                {
                                                    while (((bits[*(byte*)current] & attributeSplitBit) | *(((byte*)current) + 1)) == 0) ++current;
                                                    ROUND:
                                                    if (*current == '>')
                                                    {
                                                        if (*(current - 1) == '/') nextNode.nodeText.FormatHtml.Set(string.Empty, 0, 0);
                                                        start = ++current;
                                                        break;
                                                    }
                                                    for (start = current++; ((bits[*(byte*)current] & tagNameSplitBit) | *(((byte*)current) + 1)) != 0; ++current) ;
                                                    name.Set(html, (int)(start - htmlFixed), (int)(current - start));//.toLower()
                                                    value.FormatHtml.Set(name, name.Start, name.Length);
                                                    if (((bits[*(byte*)current] & attributeNameSplitBit) | *(((byte*)current) + 1)) != 0)
                                                    {
                                                        if (*current != '=')
                                                        {
                                                            while (((bits[*(byte*)current] & spaceBit) | *(((byte*)current) + 1)) == 0) ++current;
                                                        }
                                                        if (*current == '=')
                                                        {
                                                            do
                                                            {
                                                                ++current;
                                                            }
                                                            while (((bits[*(byte*)current] & spaceBit) | *(((byte*)current) + 1)) == 0);
                                                            if (*current != '>')
                                                            {
                                                                if (*current == '"')
                                                                {
                                                                    start = ++current;
                                                                    if (current <= endQuote2) while (*current != '"') ++current;
                                                                    else
                                                                    {
                                                                        value.FormatHtml.Set(string.Empty, 0, 0);
                                                                        attributes.Add(new KeyValue<SubString, FormatString>(name, value));
                                                                        current = start = end;
                                                                        break;
                                                                    }
                                                                }
                                                                else if (*current == '\'')
                                                                {
                                                                    start = ++current;
                                                                    if (current <= endQuote1) while (*current != '\'') ++current;
                                                                    else
                                                                    {
                                                                        value.FormatHtml.Set(string.Empty, 0, 0);
                                                                        attributes.Add(new KeyValue<SubString, FormatString>(name, value));
                                                                        current = start = end;
                                                                        break;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    for (start = current++; ((bits[*(byte*)current] & spaceSplitBit) | *(((byte*)current) + 1)) != 0; ++current) ;
                                                                }
                                                                value.FormatHtml.Set(html, (int)(start - htmlFixed), (int)(current - start));
                                                            }
                                                        }
                                                    }
                                                    attributes.Add(new KeyValue<SubString, FormatString>(name, value));
                                                    if (*current == '>') goto ROUND;
                                                    start = ++current;
                                                }
                                                else
                                                {
                                                    current = start = end;
                                                    break;
                                                }
                                            }
                                            while (true);
                                            if (attributes.Length != 0) nextNode.AttributeArray = new LeftArray<KeyValue<SubString, FormatString>>(attributes.GetArray());
                                        }
                                        else start = ++current;
                                        #endregion

                                        #region 非解析标签
                                        if (current <= endTag && NonanalyticTagName.TagNames.Contains(Tag = nextNode.Tag))
                                        {
                                            script = end;
                                            int tagNameLength = Tag.Length + 2;
                                            fixed (char* tagNameFixed = Tag.String)
                                            {
                                                char* tarNameStart = tagNameFixed + Tag.Start;
                                                while ((int)(endTagRound - current) >= tagNameLength)
                                                {
                                                    for (current += tagNameLength; *current != '>'; ++current) ;
                                                    if (*(int*)(current - tagNameLength) == (('/' << 16) + '<')
                                                        && AutoCSer.Extension.StringExtension.equalCaseNotNull(current - Tag.Length, tarNameStart, Tag.Length))
                                                    {
                                                        script = current++ - tagNameLength;
                                                        break;
                                                    }
                                                }
                                            }
                                            nextNode.nodeText.FormatHtml.Set(html, (int)(start - htmlFixed), (int)(script - start));
                                            nextNode.nodeText.FormatText = nextNode.nodeText.FormatHtml;
                                            if (script == end) current = end;
                                            start = current;
                                        }
                                        #endregion
                                        #endregion
                                    }
                                    else
                                    {
                                        #region 注释
                                        start = current - 1;
                                        int length = (int)(end - ++current);
                                        if (length > 2 && *(int*)current == (('-' << 16) + '-'))
                                        {
                                            current += 2;
                                            CHECKNOTEEND:
                                            while (*current != '>') ++current;
                                            if (*(int*)(current - 2) != (('-' << 16) + '-'))
                                            {
                                                if ((current += 3) <= endTagRound) goto CHECKNOTEEND;
                                                current = start = end;
                                                break;
                                            }
                                        }
                                        else if (length > 9
                                            && (*(long*)current | 0x20002000200000) == '[' + ('c' << 16) + ((long)'d' << 32) + ((long)'a' << 48)
                                            && (*(ulong*)(current + 4) | 0xffff000000200020U) == ('t' | ('a' << 16) | ((ulong)'[' << 32) | 0xffff000000000000U))
                                        {
                                            current += 9;
                                            CHECKNOTEEND:
                                            while (*current != '>') ++current;
                                            if (*(int*)(current - 2) != ((']' << 16) + ']'))
                                            {
                                                if ((current += 3) <= endTagRound) goto CHECKNOTEEND;
                                                current = start = end;
                                                break;
                                            }
                                        }
                                        else while (*current != '>') ++current;
                                        children.Add(new Node("!", html, (int)(start - htmlFixed), (int)(++current - start)));
                                        start = current;
                                        #endregion
                                    }
                                }
                                else isErrorHtml = true;
                            }
                            else
                            {
                                ++current;
                                isErrorHtml = true;
                            }
                        }
                        while (start != end && ((bits[*(byte*)start] & spaceBit) | *(((byte*)start) + 1)) == 0) ++start;
                        if (start != end)
                        {
                            do
                            {
                                --end;
                            }
                            while (((bits[*(byte*)end] & spaceBit) | *(((byte*)end) + 1)) == 0);
                            if (isErrorHtml || AutoCSer.Extension.StringExtension.FindNotNull(start, ++end, '<') != null)
                            {
                                children.Add(new Node((int)(start - htmlFixed), (int)(end - start), html));
                            }
                            else children.Add(new Node(html, (int)(start - htmlFixed), (int)(end - start)));
                        }
                        for (int nodeCount, nextIndex = children.Length - 1; nextIndex != -1; nextIndex--)
                        {
                            nextNode = children.Array[nextIndex];
                            if (nextNode.nodeText.FormatHtml.String == null)
                            {
                                if (MustRoundTagName.TagNames.Contains(nextNode.Tag) && (nodeCount = (children.Length - nextIndex - 1)) != 0)
                                {
                                    nextNode.ChildrenArray = new LeftArray<Node>(new SubArray<Node>(nextIndex + 1, nodeCount, children.Array).GetArray());
                                    children.RemoveRangeOnly(nextIndex + 1, nodeCount);
                                    foreach (Node sonNode in nextNode.ChildrenArray.Array) sonNode.Parent = nextNode;
                                }
                            }
                            else if (nextNode.nodeText.FormatHtml.Length == 0) nextNode.nodeText.FormatHtml.SetNull();
                        }
                        foreach (Node sonNode in children) sonNode.Parent = this;
                        this.ChildrenArray = new LeftArray<Node>(children.ToArray());
                    }
                    else
                    {
                        if (AutoCSer.Extension.StringExtension.FindNotNull(endTagRound == null ? start : endTagRound, end, '<') == null) nodeText.FormatHtml.Set(html, 0, html.Length);
                        else nodeText.FormatText = HttpUtility.HtmlDecode(html);
                    }
                }
            }
            else
            {
                nodeText.FormatHtml.Set(string.Empty, 0, 0);
                nodeText.FormatText.Set(string.Empty, 0, 0);
            }
            Tag.Set(string.Empty, 0, 0);
        }
        /// <summary>
        /// 获取属性索引
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal int GetAttirbuteIndex(string name)
        {
            int index = 0;
            if (NoLowerAttributeName.Names.Contains(name))
            {
                foreach (KeyValue<SubString, FormatString> attribute in AttributeArray)
                {
                    if (attribute.Key.Equals(name)) return index;
                    ++index;
                }
            }
            else
            {
                foreach (KeyValue<SubString, FormatString> attribute in AttributeArray)
                {
                    if (attribute.Key.EqualCase(name)) return index;
                    ++index;
                }
            }
            return -1;
        }
        /// <summary>
        /// 根据筛选路径值匹配 HTML 节点集合
        /// </summary>
        /// <param name="path">筛选路径</param>
        /// <returns>匹配 HTML 节点集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public IEnumerable<Node> Filter(string path)
        {
            return AutoCSer.HtmlNode.Filter.Get(path).Get(this);
        }
        /// <summary>
        /// 子孙节点枚举
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        internal IEnumerable<Node> GetFilterNodes(HashSet<Node> nodes)
        {
            if (ChildrenArray.Length != 0)
            {
                LeftArray<NodeIndex> indexs = default(LeftArray<NodeIndex>);
                NodeIndex index = new NodeIndex { Array = ChildrenArray };
                Node node = ChildrenArray.Array[0];
                do
                {
                    if (!nodes.Contains(node))
                    {
                        nodes.Add(node);
                        yield return node;
                    }
                }
                while ((node = index.MoveNext(ref node.ChildrenArray, ref indexs)) != null);
            }
        }
        /// <summary>
        /// 根据节点名称获取第一个子节点
        /// </summary>
        /// <param name="tagName">节点名称</param>
        /// <returns>第一个匹配子节点</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Node GetFirstChildByTagName(string tagName)
        {
            if (!string.IsNullOrEmpty(tagName))
            {
                foreach (Node value in ChildrenArray)
                {
                    if (value.Tag.EqualCase(tagName)) return value;
                }
            }
            return null;
        }
        /// <summary>
        /// 根据节点名称获取子节点集合
        /// </summary>
        /// <param name="tagName">节点名称</param>
        /// <returns>子节点集合</returns>
        public IEnumerable<Node> GetChildsByTagName(string tagName)
        {
            if (!string.IsNullOrEmpty(tagName))
            {
                foreach (Node value in ChildrenArray)
                {
                    if (value.Tag.EqualCase(tagName)) yield return value;
                }
            }
        }
        /// <summary>
        /// 根据节点名称获取子孙节点集合
        /// </summary>
        /// <param name="tagName">节点名称</param>
        /// <returns>匹配的子孙节点集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public IEnumerable<Node> GetNodesByTagName(string tagName)
        {
            return !string.IsNullOrEmpty(tagName) ? getNodesByTagName(tagName) : null;
        }
        /// <summary>
        /// 根据节点名称获取子孙节点集合
        /// </summary>
        /// <param name="tagName">节点名称</param>
        /// <returns>匹配的子孙节点集合</returns>
        private IEnumerable<Node> getNodesByTagName(string tagName)
        {
            foreach (Node value in ChildrenArray)
            {
                if (value.Tag.EqualCase(tagName)) yield return value;
                foreach (Node node in value.getNodesByTagName(tagName)) yield return node;
            }
        }

        /// <summary>
        /// 是否最后一级 HTML 节点
        /// </summary>
        private bool isLastHtmlNode
        {
            get
            {
                return ChildrenArray.Length == 0 || Tag.Length == 0 || NonanalyticTagName.TagNames.Contains(Tag);
            }
        }
        /// <summary>
        /// 生成 HTML
        /// </summary>
        /// <param name="isTag">是否包含当前标签</param>
        /// <returns>HTML</returns>
        public unsafe string GetHtml(bool isTag)
        {
            if (Tag.Length != 0)
            {
                if (NonanalyticTagName.TagNames.Contains(Tag))
                {
                    if (isTag && Tag.Length != 1)
                    {
                        byte* buffer = UnmanagedPool.Default.Get();
                        try
                        {
                            using (CharStream html = new CharStream((char*)buffer, UnmanagedPool.DefaultSize >> 1))
                            {
                                tagHtml(html);
                                html.Write(nodeText.Html);
                                tagRound(html);
                                return html.ToString();
                            }
                        }
                        finally { UnmanagedPool.Default.Push(buffer); }
                    }
                }
                else
                {
                    byte* buffer = UnmanagedPool.Default.Get();
                    try
                    {
                        using (CharStream html = new CharStream((char*)buffer, UnmanagedPool.DefaultSize >> 1))
                        {
                            if (isTag) tagHtml(html);
                            if (ChildrenArray.Length != 0)
                            {
                                LeftArray<NodeIndex> indexs = default(LeftArray<NodeIndex>);
                                NodeIndex index = new NodeIndex { Array = ChildrenArray };
                                Node node = ChildrenArray.Array[0];
                                do
                                {
                                    if (node.isLastHtmlNode)
                                    {
                                        node.writeHtml(html);
                                        CHECK:
                                        if (index.NextCount() == 0)
                                        {
                                            if (indexs.Length == 0) break;
                                            index = indexs.UnsafePopOnly();
                                            index.Current.tagRound(html);
                                            goto CHECK;
                                        }
                                        node = index.Current;
                                    }
                                    else
                                    {
                                        node.tagHtml(html);
                                        indexs.Add(index);
                                        node = index.SetNext(ref node.ChildrenArray);
                                    }
                                }
                                while (true);
                            }
                            if (isTag) tagRound(html);
                            return html.ToString();
                        }
                    }
                    finally { UnmanagedPool.Default.Push(buffer); }
                }
            }
            return nodeText.Html;
        }
        /// <summary>
        /// 生成 HTML
        /// </summary>
        /// <param name="html"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void writeHtml(CharStream html)
        {
            if (Tag.Length > 1) tagHtml(html);
            html.Write(nodeText.Html);
            if (Tag.Length > 1) tagRound(html);
        }
        /// <summary>
        /// 生成标签html
        /// </summary>
        /// <param name="html">HTML 流</param>
        private void tagHtml(CharStream html)
        {
            if (Tag.Length != 0)
            {
                html.Write('<');
                html.WriteNotEmpty(ref Tag);
                foreach (KeyValue<SubString, FormatString> attribute in AttributeArray)
                {
                    html.Write(' ');
                    html.Write(HttpUtility.HtmlEncode(attribute.Key.ToString()));
                    html.SimpleWriteNotNull(@"=""");
                    html.Write(attribute.Value.Html);
                    html.Write('"');
                }
                if (CanNonRoundTagName.TagNames.Contains(Tag) && ChildrenArray.Array == null && nodeText.Html == null) html.SimpleWriteNotNull(" />");
                else html.Write('>');
            }
        }
        /// <summary>
        /// 生成标签结束
        /// </summary>
        /// <param name="html">HTML 流</param>
        private void tagRound(CharStream html)
        {
            if (Tag.Length != 0
                && (!CanNonRoundTagName.TagNames.Contains(Tag) || ChildrenArray.Array != null || nodeText.Html != null))
            {
                html.SimpleWriteNotNull("</");
                html.WriteNotEmpty(ref Tag);
                html.Write('>');
            }
        }

        /// <summary>
        /// 删除匹配的子孙节点
        /// </summary>
        /// <param name="isRemove">删除节点匹配器</param>
        public void Remove(Func<Node, bool> isRemove)
        {
            if (isRemove != null)
            {
                ChildrenArray.Remove(isRemove);
                if (ChildrenArray.Length != 0)
                {
                    LeftArray<NodeIndex> indexs = default(LeftArray<NodeIndex>);
                    NodeIndex index = new NodeIndex { Array = ChildrenArray };
                    Node node = ChildrenArray.Array[0];
                    do
                    {
                        node.ChildrenArray.Remove(isRemove);
                    }
                    while ((node = index.MoveNext(ref node.ChildrenArray, ref indexs)) != null);
                }
            }
        }

        /// <summary>
        /// 空隔字符
        /// </summary>
        private const int spaceBit = 1;
        /// <summary>
        /// 空隔+结束字符
        /// </summary>
        private const int spaceSplitBit = 2;
        /// <summary>
        /// 标签名称结束字符
        /// </summary>
        private const int tagNameBit = 4;
        /// <summary>
        /// 标签名称开始字符
        /// </summary>
        private const int tagNameSplitBit = 8;
        /// <summary>
        /// 标签属性分隔结束字符
        /// </summary>
        private const int attributeSplitBit = 0x10;
        /// <summary>
        /// 标签属性名称结束字符
        /// </summary>
        private const int attributeNameSplitBit = 0x20;
        /// <summary>
        /// 节点筛选器解析类型
        /// </summary>
        internal const int FilterBit = 0x40;
        /// <summary>
        /// CSS 过滤字符
        /// </summary>
        internal const int CssFilterBit = 0x80;
        /// <summary>
        /// 字符状态位
        /// </summary>
        internal static Pointer Bits;
        static Node()
        {
            byte* bits = (byte*)Unmanaged.GetStatic(256, false);
            AutoCSer.Memory.Fill((ulong*)bits, ulong.MaxValue, 256 >> 3);
            Bits = new Pointer { Data = bits };
            bits['/'] &= (tagNameSplitBit | attributeSplitBit | attributeNameSplitBit | tagNameBit) ^ 255;
            bits['\t'] &= (spaceBit | spaceSplitBit | tagNameSplitBit | attributeSplitBit) ^ 255;
            bits['\r'] &= (spaceBit | spaceSplitBit | tagNameSplitBit | attributeSplitBit) ^ 255;
            bits['\n'] &= (spaceBit | spaceSplitBit | tagNameSplitBit | attributeSplitBit) ^ 255;
            bits[' '] &= (spaceBit | spaceSplitBit | tagNameSplitBit | attributeSplitBit) ^ 255;
            bits['>'] &= (spaceSplitBit | tagNameSplitBit | attributeNameSplitBit) ^ 255;
            bits['"'] &= (tagNameSplitBit | attributeSplitBit | attributeNameSplitBit) ^ 255;
            bits['\''] &= (tagNameSplitBit | attributeSplitBit | attributeNameSplitBit) ^ 255;
            bits['='] &= (tagNameSplitBit | attributeSplitBit) ^ 255;
            bits['!'] &= tagNameBit ^ 255;
            for (int value = 'A'; value <= 'Z'; ++value)
            {
                bits[value] &= tagNameBit ^ 255;
                bits[value | 0x20] &= tagNameBit ^ 255;
            }
        }
    }
}
