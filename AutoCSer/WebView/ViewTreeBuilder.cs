using System;
using System.Text.RegularExpressions;
using AutoCSer.Extension;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.WebView
{
    /// <summary>
    /// HTML 模板建树器
    /// </summary>
    internal unsafe class ViewTreeBuilder
    {
        /// <summary>
        /// =@取值字符位图尺寸
        /// </summary>
        private const int atMapSize = 128;
        /// <summary>
        /// 模板命令HASH匹配数据容器尺寸
        /// </summary>
        private const int commandUniqueHashDataCount = 16;
        /// <summary>
        /// 模板命令HASH匹配数据尺寸
        /// </summary>
        private const int commandUniqueHashDataSize = 16;
        /// <summary>
        /// =@取值字符位图
        /// </summary>
        private static MemoryMap atMap;
        /// <summary>
        /// 模板命令HASH匹配数据
        /// </summary>
        private static Pointer commandUniqueHashData;
        /// <summary>
        /// 客户端命令索引位置
        /// </summary>
        protected static readonly int clientCommandIndex;
        /// <summary>
        /// 图片源
        /// </summary>
        private static readonly Regex imageSrc = new Regex(@" @src=""", RegexOptions.Compiled);
        /// <summary>
        /// HTML片段
        /// </summary>
        internal Dictionary<HashString, int> Htmls = DictionaryCreator.CreateHashString<int>();
        /// <summary>
        /// HTML片段
        /// </summary>
        internal string[] HtmlArray
        {
            get
            {
                string[] values = new string[Htmls.Count];
                foreach (KeyValuePair<HashString, int> value in Htmls) values[value.Value] = value.Key.ToString();
                return values;
            }
        }
        /// <summary>
        /// HTML模板建树器
        /// </summary>
        /// <param name="html">HTML</param>
        /// <param name="isCreate">是否自动创建</param>
        internal ViewTreeBuilder(string html, bool isCreate = true)
        {
            if (isCreate) create(formatHtml(html));
        }
        /// <summary>
        /// 根据HTML获取模板树
        /// </summary>
        /// <param name="html">HTML</param>
        protected unsafe void create(string html)
        {
            if (html != null)
            {
                if (html.Length >= 3)
                {
                    MemoryMap atFixedMap = atMap;
                    byte* commandUniqueHashDataFixed = commandUniqueHashData.Byte;
                    fixed (char* htmlFixed = html)
                    {
                        char* start = htmlFixed, end = htmlFixed + html.Length - 1, current = htmlFixed;
                        char endChar = *end;
                        do
                        {
                            for (*end = '<'; *current != '<'; ++current) ;
                            if (current == end) break;
                            if (*++current == '!' && *(int*)++current == ('-' | ('-' << 16)))
                            {
                                char* tagStart = current += 2;
                                for (*end = '>'; *current != '>'; ++current) ;
                                if (current == end && endChar != '>') break;
                                if (*(int*)(current -= 2) == ('-' | ('-' << 16)))
                                {
                                    char* contentStart = tagStart;
                                    for (*current = ':'; *contentStart != ':'; ++contentStart) ;
                                    *current = '-';
                                    int tagLength = (int)(contentStart - tagStart), tagIndex = (((*tagStart >> 1) ^ (tagStart[tagLength >> 2] >> 2)) & ((1 << 4) - 1));
                                    byte* hashData = commandUniqueHashDataFixed + (tagIndex * commandUniqueHashDataSize);
                                    if (*(int*)hashData == tagLength && Memory.SimpleEqualNotNull((byte*)tagStart, hashData + sizeof(int), tagLength << 1))
                                    {
                                        ViewTreeTag tag = new ViewTreeTag
                                        {
                                            Type = ViewTreeTagType.Note,
                                            Command = new SubString { String = html, Start = (int)(tagStart - htmlFixed), Length = (int)(contentStart - tagStart) }, 
                                            Content = contentStart == current ? new SubString { String = html } : new SubString { String = html, Start = (int)(++contentStart - htmlFixed), Length = (int)(current - contentStart) } 
                                        };
                                        #region =@取值解析
                                        if (start != (tagStart -= 4))
                                        {
                                            contentStart = start;
                                            *tagStart = '@';
                                            do
                                            {
                                                while (*++contentStart != '@') ;
                                                if (contentStart == tagStart) break;
                                                if (*--contentStart == '=' && *(contentStart + 2) != '[')
                                                {
                                                    if (start != contentStart)
                                                    {
                                                        appendHtml(new SubString { String = html, Start = (int)(start - htmlFixed), Length = (int)(contentStart - start) });
                                                    }
                                                    start = (contentStart += 2);
                                                    if (contentStart == tagStart)
                                                    {
                                                        appendAtNode(new SubString { String = html });
                                                        break;
                                                    }
                                                    if (*contentStart == '$')
                                                    {
                                                        appendAtNode(new SubString { String = html, Start = (int)(start - htmlFixed), Length = 1 });
                                                        ++contentStart;
                                                    }
                                                    else
                                                    {
                                                        if (*contentStart == '@' || *contentStart == '*')
                                                        {
                                                            if (++contentStart == tagStart)
                                                            {
                                                                appendAtNode(new SubString { String = html, Start = (int)(start - htmlFixed), Length = 1 });
                                                                ++start;
                                                                break;
                                                            }
                                                        }
                                                        while (*contentStart < atMapSize && atFixedMap.Get(*contentStart) != 0) ++contentStart;
                                                        if (*contentStart == '#' && (*(contentStart + 1) < atMapSize && atFixedMap.Get(*(contentStart + 1)) != 0))
                                                        {
                                                            for (contentStart += 2; *contentStart < atMapSize && atFixedMap.Get(*contentStart) != 0; ++contentStart) ;
                                                        }
                                                        appendAtNode(new SubString { String = html, Start = (int)(start - htmlFixed), Length = (int)(contentStart - start) });
                                                        if (*contentStart == '$') ++contentStart;
                                                    }
                                                    start = contentStart;
                                                }
                                                else contentStart += 2;
                                            }
                                            while (contentStart != tagStart);
                                            *tagStart = '<';
                                            if (start != tagStart)
                                            {
                                                appendHtml(new SubString { String = html, Start = (int)(start - htmlFixed), Length = (int)(tagStart - start) });
                                            }
                                        }
                                        #endregion
                                        appendNode(tagIndex, tag);
                                        start = current + 3;
                                    }
                                }
                                current += 3;
                            }
                        }
                        while (current < end);
                        #region 最后=@取值解析
                        if (current <= end)
                        {
                            current = start;
                            *end = '@';
                            do
                            {
                                while (*current != '@') ++current;
                                if (current == end) break;
                                if (*--current == '=')
                                {
                                    if (start != current)
                                    {
                                        appendHtml(new SubString { String = html, Start = (int)(start - htmlFixed), Length = (int)(current - start) });
                                    }
                                    start = (current += 2);
                                    if (current == end)
                                    {
                                        appendAtNode(new SubString { String = html });
                                        break;
                                    }
                                    if (*current == '@' || *current == '*')
                                    {
                                        if (++current == end)
                                        {
                                            appendAtNode(new SubString { String = html, Start = (int)(start - htmlFixed), Length = 1 });
                                            ++start;
                                            break;
                                        }
                                    }
                                    while (*current < atMapSize && atFixedMap.Get(*current) != 0) ++current;
                                    if (*current == '#' && (*(current + 1) < atMapSize && atFixedMap.Get(*(current + 1)) != 0))
                                    {
                                        for (current += 2; *current < atMapSize && atFixedMap.Get(*current) != 0; ++current) ;
                                    }
                                    appendAtNode(new SubString { String = html, Start = (int)(start - htmlFixed), Length = (int)(current - start) });
                                    start = current;
                                }
                                else current += 2;
                            }
                            while (current != end);
                            *end = endChar;
                            appendHtml(new SubString { String = html, Start = (int)(start - htmlFixed), Length = (int)(end - start) + 1 });
                        }
                        #endregion
                        *end = endChar;
                    }
                }
                else appendHtml(html);
                onCreated();
            }
        }
        /// <summary>
        /// 添加HTML片段
        /// </summary>
        /// <param name="html">HTML片段</param>
        protected virtual void appendHtml(SubString html)
        {
            appendHtml(ref html);
        }
        /// <summary>
        /// 添加HTML片段
        /// </summary>
        /// <param name="html">HTML片段</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void appendHtml(ref SubString html)
        {
            HashString htmlKey = html;
            if (!Htmls.ContainsKey(htmlKey)) Htmls.Add(htmlKey, Htmls.Count);
        }
        /// <summary>
        /// 添加 @ 节点
        /// </summary>
        /// <param name="content"></param>
        protected virtual void appendAtNode(SubString content) { }
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="tagIndex"></param>
        /// <param name="tag"></param>
        protected virtual void appendNode(int tagIndex, ViewTreeTag tag) { }
        /// <summary>
        /// HTML模板树创建完毕
        /// </summary>
        protected virtual void onCreated() { }

        /// <summary>
        /// HTML模板格式化处理
        /// </summary>
        /// <param name="html">HTML模板</param>
        /// <returns>格式化处理后的HTML模板</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected static string formatHtml(string html)
        {
            html = imageSrc.Replace(html, @" src=""");
            //html = html.Replace(ViewBody, string.Empty);
            return html;
        }

        static ViewTreeBuilder()
        {
            atMap = new MemoryMap(Unmanaged.GetStatic64(atMapSize + commandUniqueHashDataCount * commandUniqueHashDataSize, true));
            atMap.Set('0', 10);
            atMap.Set('A', 26);
            atMap.Set('a', 26);
            atMap.Set('.');
            atMap.Set('_');

            commandUniqueHashData = new Pointer { Data = atMap.Map + atMapSize };
            for (byte* start = commandUniqueHashData.Byte, end = start + commandUniqueHashDataCount * commandUniqueHashDataSize; start != end; start += commandUniqueHashDataSize) *(int*)start = int.MinValue;
            foreach (ViewTreeCommand command in System.Enum.GetValues(typeof(ViewTreeCommand)))
            {
                string commandString = command.ToString();
                if (sizeof(int) + (commandString.Length << 1) > commandUniqueHashDataSize)
                {
                    throw new IndexOutOfRangeException();
                }
                fixed (char* commandFixed = commandString)
                {
                    int code = ((*commandFixed >> 1) ^ (commandFixed[commandString.Length >> 2] >> 2)) & ((1 << 4) - 1);
                    if (command == ViewTreeCommand.Client) clientCommandIndex = code;
                    byte* data = commandUniqueHashData.Byte + (code * commandUniqueHashDataSize);
                    if (*(int*)data != int.MinValue) throw new IndexOutOfRangeException();
                    *(int*)data = commandString.Length;
                    Memory.CopyNotNull(commandFixed, data + sizeof(int), commandString.Length << 1);
                }
            } 
        }
    }
}
