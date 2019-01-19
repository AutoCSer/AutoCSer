using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CsvFile
{
    /// <summary>
    /// CSV 字符串读取器
    /// </summary>
    public sealed class Reader
    {
        /// <summary>
        /// 文本字符串内容
        /// </summary>
        private readonly string content;
        /// <summary>
        /// 列分割字符
        /// </summary>
        private readonly char colSplit;
        /// <summary>
        /// 行分割字符
        /// </summary>
        private readonly char rowSplit;
        /// <summary>
        /// 转义字符
        /// </summary>
        private readonly char escape;
        /// <summary>
        /// 换行忽略字符
        /// </summary>
        private readonly char rowIgnoreSplit;
        /// <summary>
        /// 转义字符串
        /// </summary>
        private string escapeString;
        /// <summary>
        /// 转义字符串
        /// </summary>
        private string escapeString2;
        /// <summary>
        /// CSV 字符串读取器
        /// </summary>
        /// <param name="content">文本字符串内容</param>
        /// <param name="colSplit">列分割字符</param>
        /// <param name="rowSplit">行分割字符</param>
        /// <param name="escape">转义字符</param>
        /// <param name="rowIgnoreSplit">换行忽略字符</param>
        public Reader(string content, char colSplit = ',', char rowSplit = '\n', char escape = '"', char rowIgnoreSplit = '\r')
        {
            this.content = content;
            this.colSplit = colSplit;
            this.rowSplit = rowSplit;
            this.rowIgnoreSplit = rowIgnoreSplit;
            this.escape = escape;
        }
        /// <summary>
        /// 枚举内容
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReaderItem> Read()
        {
            if (!string.IsNullOrEmpty(content))
            {
                ReaderItem item = new ReaderItem { Reader = this };
                int index = 0;
                ReaderStep step = ReaderStep.Start;
                foreach (char code in content)
                {
                    switch (step)
                    {
                        case ReaderStep.Start:
                            if (code == colSplit)
                            {
                                item.End = index;
                                yield return item;
                                item.NextCol(ref step);
                            }
                            else if (code == rowSplit)
                            {
                                item.End = index;
                                yield return item;
                                item.NextRow(ref step);
                            }
                            else if (code == rowIgnoreSplit) step = ReaderStep.RowIgnore;
                            else if (code == escape) step = ReaderStep.Escape;
                            else step = ReaderStep.Next;
                            break;
                        case ReaderStep.Next:
                            if (code == colSplit)
                            {
                                item.End = index;
                                yield return item;
                                item.NextCol(ref step);
                            }
                            else if (code == rowSplit)
                            {
                                item.End = index;
                                yield return item;
                                item.NextRow(ref step);
                            }
                            else if (code == rowIgnoreSplit) step = ReaderStep.RowIgnore;
                            break;
                        case ReaderStep.RowIgnore:
                            if (code == colSplit)
                            {
                                item.End = index;
                                yield return item;
                                item.NextCol(ref step);
                            }
                            else if (code == rowSplit)
                            {
                                item.End = index - 1;
                                yield return item;
                                item.NextIgnoreRow(ref step);
                            }
                            else step = ReaderStep.Next;
                            break;
                        case ReaderStep.Escape:
                            if (code == escape) step = ReaderStep.NextEscape;
                            break;
                        case ReaderStep.NextEscape:
                            if (code == colSplit)
                            {
                                item.End = index;
                                yield return item;
                                item.NextCol(ref step);
                            }
                            else if (code == rowSplit)
                            {
                                item.End = index;
                                yield return item;
                                item.NextRow(ref step);
                            }
                            else if (code == escape) step = ReaderStep.Escape;
                            else if (code == rowIgnoreSplit) step = ReaderStep.EscapeRowIgnore;
                            else
                            {
                                item.Error(index);
                                goto RETURN;
                            }
                            break;
                        case ReaderStep.EscapeRowIgnore:
                            if (code == rowSplit)
                            {
                                item.End = index - 1;
                                yield return item;
                                item.NextIgnoreRow(ref step);
                            }
                            else
                            {
                                item.Error(index);
                                goto RETURN;
                            }
                            break;
                    }
                    ++index;
                }
                item.End = index;
                RETURN:
                yield return item;
            }
        }
        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal string GetString(ref ReaderItem item)
        {
            if (content[item.Start] == escape)
            {
                if (item.Length == 2) return string.Empty;
                if (escapeString == null)
                {
                    escapeString = escape.ToString();
                    escapeString2 = escapeString + escapeString;
                }
                return content.Substring(item.Start + 1, item.Length - 2).Replace(escapeString2, escapeString);
            }
            return content.Substring(item.Start, item.Length);
        }
        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ignorePrefix">忽略前缀</param>
        /// <returns></returns>
        internal string GetString(ref ReaderItem item, char ignorePrefix)
        {
            if (content[item.Start] == ignorePrefix)
            {
                return item.Length != 1 ? content.Substring(item.Start + 1, item.Length - 1) : string.Empty;
            }
            return content.Substring(item.Start, item.Length);
        }
        /// <summary>
        /// 获取原始字符串
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal string GetString(int start, int end)
        {
            return content.Substring(start, end - start);
        }
        /// <summary>
        /// 根据数量检测数据列分隔符
        /// </summary>
        /// <param name="content"></param>
        /// <param name="split"></param>
        /// <param name="otherSplit"></param>
        /// <returns></returns>
        public static char CheckColSplit(string content, char split, char otherSplit)
        {
            int count = 0, otherCount = 0;
            foreach (char code in content)
            {
                if (code == '\n') break;
                if (code == split) ++count;
                else if (code == otherSplit) ++otherCount;
            }
            return count >= otherCount ? split : otherSplit;
        }
    }
}
