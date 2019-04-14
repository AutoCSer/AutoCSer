using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// hosts 文件操作
    /// </summary>
    public sealed class HostsFile
    {
        /// <summary>
        /// 文件的一行数据
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private sealed class FileLine
        {
            /// <summary>
            /// 分隔符
            /// </summary>
            private static readonly char[] splitChars = new char[] { ' ', '	' };
            /// <summary>
            /// 原始文本
            /// </summary>
            private string text;
            /// <summary>
            /// hosts 文件读写
            /// </summary>
            private HostsFile file;
            /// <summary>
            /// 文本中的第几行
            /// </summary>
            private int lineIndex;
            /// <summary>
            /// 注释
            /// </summary>
            private string note;
            /// <summary>
            /// IP + 域名信息集合
            /// </summary>
            private string[] domains;
            /// <summary>
            /// 文件的一行数据
            /// </summary>
            /// <param name="text">原始文本</param>
            /// <param name="file">hosts 文件读写</param>
            /// <param name="lineIndex">文本中的第几行</param>
            internal FileLine(string text, HostsFile file, int lineIndex)
            {
                this.text = text;
                this.file = file;
                this.lineIndex = lineIndex;
                int noteIndex = text.IndexOf('#');
                if (noteIndex != -1)
                {
                    note = text.Substring(noteIndex);
                    text = noteIndex != 0 ? text.Substring(0, noteIndex) : string.Empty;
                }
                if (!string.IsNullOrEmpty(text))
                {
                    domains = text.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                    if (domains.Length > 1)
                    {
                        string ip = null;
                        foreach (string domain in domains)
                        {
                            if (ip == null) ip = domain;
                            else file.domains[domain] = this;
                        }
                    }
                }
                if (note == null && domains == null) file.emptyLines.Add(lineIndex);
            }
            /// <summary>
            /// 文件的一行数据
            /// </summary>
            /// <param name="file"></param>
            /// <param name="ip"></param>
            /// <param name="domain"></param>
            /// <param name="note"></param>
            /// <param name="lineIndex">文本中的第几行</param>
            internal FileLine(HostsFile file, string ip, string domain, string note, int lineIndex)
            {
                this.file = file;
                this.lineIndex = lineIndex;
                Set(ip, domain, note);
            }
            /// <summary>
            /// 判断 IP 地址是否相等
            /// </summary>
            /// <param name="ip"></param>
            /// <returns></returns>
            private bool isIp(string ip)
            {
                return domains != null && domains.Length != 0 && domains[0] == ip;
            }
            /// <summary>
            /// 删除域名信息
            /// </summary>
            /// <param name="domain"></param>
            internal void Remove(string domain)
            {
                int index = Array.IndexOf(domains, domain);
                if (index > 0)
                {
                    domains[index] = null;
                    file.domains.Remove(domain);
                }
            }
            /// <summary>
            /// 尝试删除域名信息
            /// </summary>
            /// <param name="domain"></param>
            /// <param name="ip"></param>
            /// <param name="note"></param>
            /// <returns>操作后是否存在域名</returns>
            internal bool TryRemove(string domain, string ip, string note)
            {
                int index = Array.IndexOf(domains, domain);
                if (this.note != note || !isIp(ip))
                {
                    if (index > 0)
                    {
                        domains[index] = null;
                        file.domains.Remove(domain);
                    }
                    return false;
                }
                return index > 0;
            }
            /// <summary>
            /// 删除 IP 地址
            /// </summary>
            /// <param name="ip"></param>
            internal void RemoveIp(string ip)
            {
                if (isIp(ip))
                {
                    ip = null;
                    foreach (string domain in domains)
                    {
                        if (ip == null) ip = domain;
                        else if (domain != null) file.remove(domain, this);
                    }
                    domains = null;
                    note = null;
                }
            }
            /// <summary>
            /// 尝试添加域名信息
            /// </summary>
            /// <param name="ip"></param>
            /// <param name="domain"></param>
            /// <returns></returns>
            internal bool TrySet(string ip, string domain)
            {
                if (note == null && isIp(ip) && domains.Length <= file.domianCount)
                {
                    domains = domains.getAdd(domain);
                    file.domains[domain] = this;
                    return true;
                }
                return false;
            }
            /// <summary>
            /// 设置 IP 地址与域名信息
            /// </summary>
            /// <param name="ip"></param>
            /// <param name="domain"></param>
            /// <param name="note"></param>
            internal void Set(string ip, string domain, string note)
            {
                domains = new string[] { ip, domain };
                file.domains[domain] = this;
                this.note = note == null ? null : ("#" + note);
            }
            /// <summary>
            /// 转换为文件文本行
            /// </summary>
            /// <returns></returns>
            internal string ToText()
            {
                if (domains != null)
                {
                    string ip = null;
                    foreach (string domain in domains)
                    {
                        if (ip == null) ip = domain;
                        else if(!string.IsNullOrEmpty(domain)) return domains.joinString(' ') + note;
                    }
                }
                return note ?? string.Empty;
            }
        }
        /// <summary>
        /// hosts 文件全名称
        /// </summary>
        private readonly string fileName;
        /// <summary>
        /// 文件编码
        /// </summary>
        private readonly Encoding encoding;
        /// <summary>
        /// 文件行数据集合
        /// </summary>
        private LeftArray<FileLine> lines;
        /// <summary>
        /// 域名集合
        /// </summary>
        private readonly Dictionary<string, FileLine> domains = DictionaryCreator.CreateOnly<string, FileLine>();
        /// <summary>
        /// 空行集合
        /// </summary>
        private LeftArray<int> emptyLines;
        /// <summary>
        /// 域名数量
        /// </summary>
        private int domianCount;
        /// <summary>
        /// hosts 文件读写
        /// </summary>
        /// <param name="fileName">hosts 文件全名称，默认为 Environment.SpecialFolder.System\drivers\etc\hosts</param>
        /// <param name="domianCount">域名数量默认为 1</param>
        /// <param name="encoding">文件编码，默认为 Encoding.Default</param>
        public HostsFile(string fileName = null, int domianCount = 1, Encoding encoding = null)
        {
            this.fileName = fileName ?? (Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\drivers\etc\hosts");
            this.domianCount = Math.Min(Math.Max(domianCount, 6), 1);
            this.encoding = encoding ?? Encoding.Default;
            string[] texts = File.ReadAllLines(this.fileName, this.encoding);
            lines = new LeftArray<FileLine>(texts.Length);
            int lineIndex = 0;
            foreach (string text in texts) lines.Add(new FileLine(text, this, lineIndex++));
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Save()
        {
            File.WriteAllLines(fileName, lines.GetArray(value => value.ToText()), encoding);
        }
        /// <summary>
        /// 域名设置 IP 映射
        /// </summary>
        /// <param name="domain">域名</param>
        /// <param name="ip">IP 地址</param>
        /// <param name="note">注释</param>
        /// <param name="isNewLine"></param>
        public void Set(string domain, string ip, string note = null, bool isNewLine = false)
        {
            if (string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(ip)) throw new ArgumentNullException();
            FileLine fileLine;
            if (domains.TryGetValue(domain, out fileLine) && fileLine.TryRemove(domain, ip, note)) return;
            if (note == null)
            {
                foreach (FileLine line in lines)
                {
                    if (line.TrySet(ip, domain)) return;
                }
            }
            if (!isNewLine && emptyLines.Count != 0) lines.Array[emptyLines.UnsafePop()].Set(ip, domain, note);
            else lines.Add(new FileLine(this, ip, domain, note, lines.Count));
        }
        /// <summary>
        /// 删除域名
        /// </summary>
        /// <param name="domian"></param>
        /// <param name="line"></param>
        private void remove(string domian, FileLine line)
        {
            FileLine cacheLine;
            if (domains.TryGetValue(domian, out cacheLine) && cacheLine == line) domains.Remove(domian);
        }
        /// <summary>
        /// 删除域名映射
        /// </summary>
        /// <param name="domain"></param>
        public void Remove(string domain)
        {
            FileLine fileLine;
            if (domains.TryGetValue(domain, out fileLine)) fileLine.Remove(domain);
        }
        /// <summary>
        /// 删除 IP 地址映射
        /// </summary>
        /// <param name="ip"></param>
        public void RemoveIp(string ip)
        {
            foreach (FileLine line in lines) line.RemoveIp(ip);
        }
    }
}
