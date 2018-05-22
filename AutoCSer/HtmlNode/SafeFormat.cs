using System;
using AutoCSer.Extension;

namespace AutoCSer.HtmlNode
{
    /// <summary>
    /// HTML 安全格式化
    /// </summary>
    public static class SafeFormat
    {
        /// <summary>
        /// 安全格式化
        /// </summary>
        /// <param name="html">HTML</param>
        /// <returns>HTML</returns>
        public static string Format(string html)
        {
            if (!string.IsNullOrEmpty(html))
            {
                Node document = new Node(html);
                document.Remove(removeTagNameHandle);
                foreach (Node node in document.Nodes)
                {
                    if (node.AttributeArray.Length != 0)
                    {
                        KeyValue<SubString, FormatString>[] attributeArray = node.AttributeArray.Array;
                        int removeCount = 0, index = 0;
                        foreach (KeyValue<SubString, FormatString> attribute in node.AttributeArray)
                        {
                            if (!SafeAttributeName.Names.Contains(attribute.Key))
                            {
                                if (attribute.Key.EqualCase("style"))
                                {
                                    string style = formatStyle(attributeArray[index].Value.Text);
                                    if (style == null)
                                    {
                                        attributeArray[index].Key.SetNull();
                                        removeCount = 1;
                                    }
                                    else attributeArray[index].Value.Text = style;
                                }
                                else if (UriAttributeName.Names.Contains(attribute.Key))
                                {
                                    if (!isHttpOrDefalut(attributeArray[index].Value.Text))
                                    {
                                        attributeArray[index].Key.SetNull();
                                        removeCount = 1;
                                    }
                                }
                                else
                                {
                                    attributeArray[index].Key.SetNull();
                                    removeCount = 1;
                                }
                            }
                            ++index;
                        }
                        if (removeCount != 0) node.AttributeArray.Remove(isRemoteAttributeHandle);
                        if (node.AttributeArray.Length != 0 && node.IsTagA)
                        {
                            bool isHrefBlank = false;
                            foreach (KeyValue<SubString, FormatString> attribute in node.AttributeArray)
                            {
                                if (attribute.Key.EqualCase("href"))
                                {
                                    SubString href = attributeArray[index].Value.Text;
                                    if (href.Length != 0 && href[0] != '/') isHrefBlank = true;
                                    break;
                                }
                            }
                            if (isHrefBlank)
                            {
                                index = 0;
                                foreach (KeyValue<SubString, FormatString> attribute in node.AttributeArray)
                                {
                                    if (attribute.Key.EqualCase("target"))
                                    {
                                        attributeArray[index].Value.Text = "_blank";
                                        break;
                                    }
                                    ++index;
                                }
                                if (index == node.AttributeArray.Length)
                                {
                                    node.AttributeArray.Add(new KeyValue<SubString, FormatString>("target", new FormatString { FormatText = "_blank" }));
                                }
                            }
                        }
                    }
                }
                return document.GetHtml(true);
            }
            return html;
        }
        /// <summary>
        /// 格式化样式表
        /// </summary>
        /// <param name="style">样式表</param>
        /// <returns>样式表</returns>
        private static unsafe string formatStyle(SubString style)
        {
            if (style.Length != 0)
            {
                LeftArray<KeyValue<SubString, SubString>> values = new LeftArray<KeyValue<SubString, SubString>>();
                LeftArray<SubString> styles = style.Split(';');
                SubString[] styleArray = styles.Array;
                for (int index = 0; index != styles.Length; ++index)
                {
                    int valueIndex = styleArray[index].IndexOf(':');
                    if (valueIndex > 0)
                    {
                        SubString name = styleArray[index].GetSub(0, valueIndex);
                        if (SafeStyleAttributeName.Names.Contains(name))
                        {
                            styleArray[index].MoveStart(valueIndex + 1);
                            values.Add(new KeyValue<SubString, SubString>(ref name, ref styleArray[index]));
                        }
                    }
                }
                if (values.Length != 0)
                {
                    int length = (values.Count << 1) - 1;
                    foreach (KeyValue<SubString, SubString> value in values) length += value.Key.Length + value.Value.Length;
                    string newStyle = AutoCSer.Extension.StringExtension.FastAllocateString(length);
                    byte* bits = Node.Bits.Byte;
                    fixed (char* newStyleFixed = newStyle, styleFixed = style.String)
                    {
                        char* write = newStyleFixed;
                        foreach (KeyValue<SubString, SubString> value in values)
                        {
                            if (write != newStyleFixed) *write++ = ';';
                            for (char* start = styleFixed + value.Key.Start, end = start + value.Key.Length; start != end; *write++ = *start++) ;
                            *write++ = ':';
                            for (char* start = styleFixed + value.Value.Start, end = start + value.Value.Length; start != end; ++start)
                            {
                                *write++ = ((bits[*(byte*)start] & Node.CssFilterBit) | *(((byte*)start) + 1)) == 0 ? ' ' : *start;
                            }
                        }
                    }
                    return newStyle;
                }
            }
            return null;
        }
        /// <summary>
        /// 判断连接地址是否以 http:// 或者 https:// 或者 // 开头
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private unsafe static bool isHttpOrDefalut(SubString url)
        {
            if (url.Length > 7 && url[6] == '/')
            {
                fixed (char* urlFixed = url.String)
                {
                    char* start = urlFixed + url.Start;
                    if ((*(int*)start | 0x200020) == 'h' + ('t' << 16) && (*(int*)(start + 2) | 0x200020) == 't' + ('p' << 16))
                    {
                        if (*(int*)(start + 4) == ':' + ('/' << 16)) return true;
                        else if ((*(int*)(start + 4) | 0x20) == 's' + (':' << 16) && start[7] == '/') return true;
                    }
                }
            }
            if (url.Length > 2) return url[0] == '/' && url[1] == '/';
            return false;
        }
        /// <summary>
        /// 删除空名称属性
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private static bool isRemoteAttribute(KeyValue<SubString, FormatString> attribute)
        {
            return attribute.Key.String == null;
        }
        /// <summary>
        /// 删除空属性名称匹配委托
        /// </summary>
        private static Func<KeyValue<SubString, FormatString>, bool> isRemoteAttributeHandle = isRemoteAttribute;
        /// <summary>
        /// 默认允许标签名称集合
        /// </summary>
        private static readonly AutoCSer.StateSearcher.AsciiSearcher tagNames = new AutoCSer.StateSearcher.AsciiSearcher(AutoCSer.StateSearcher.AsciiBuilder.Create(new string[] { "a", "b", "big", "blockquote", "br", "center", "code", "dd", "del", "div", "dl", "dt", "em", "font", "h1", "h2", "h3", "h4", "h5", "h6", "hr", "i", "img", "ins", "li", "ol", "p", "pre", "s", "small", "span", "strike", "strong", "sub", "sup", "table", "tbody", "td", "th", "thead", "title", "tr", "u", "ul" }, true).Pointer);
        /// <summary>
        /// 删除标签名称过滤
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static bool removeTagName(Node node)
        {
            return node.Tag.Length != 0 && tagNames.SearchLower(ref node.Tag) < 0;
        }
        /// <summary>
        /// 允许标签名称过滤委托
        /// </summary>
        private static readonly Func<Node, bool> removeTagNameHandle = removeTagName;

        static unsafe SafeFormat()
        {
            byte* bits = Node.Bits.Byte;
            bits['<'] &= Node.CssFilterBit ^ 255;
            bits['>'] &= Node.CssFilterBit ^ 255;
            bits['&'] &= Node.CssFilterBit ^ 255;
            bits['"'] &= Node.CssFilterBit ^ 255;
            bits['\''] &= Node.CssFilterBit ^ 255;
        }
    }
}
