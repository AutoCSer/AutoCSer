using System;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 自定义简单组合模板链
    /// </summary>
    internal sealed class CombinationTemplateLink
    {
        /// <summary>
        /// 下一个模板节点
        /// </summary>
        private CombinationTemplateLink next;
        /// <summary>
        /// 下一个模板节点
        /// </summary>
        internal CombinationTemplateLink Next
        {
            get { return object.ReferenceEquals(next, this) ? null : next; }
        }
        /// <summary>
        /// 模板代码片段
        /// </summary>
        internal string Code;
        /// <summary>
        /// 替换行号
        /// </summary>
        internal readonly int Row;
        /// <summary>
        /// 替换列号
        /// </summary>
        internal readonly int Col;
        /// <summary>
        /// 代码索引位置
        /// </summary>
        internal int Index;
        /// <summary>
        /// 自定义简单组合模板链
        /// </summary>
        /// <param name="code">模板代码片段</param>
        internal CombinationTemplateLink(string code)
        {
            this.Code = code;
            Row = Col = -1;
            next = this;
        }
        /// <summary>
        /// 替换节点
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private CombinationTemplateLink(int row, int col)
        {
            Code = string.Empty;
            Row = row;
            Col = col;
            next = this;
        }
        /// <summary>
        /// 代码分片
        /// </summary>
        /// <param name="value"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="isSplit"></param>
        /// <returns>下一个节点</returns>
        internal CombinationTemplateLink Split(string value, int row, int col, ref bool isSplit)
        {
            CombinationTemplateLink next = this.next;
            if (value.Length != 0 && Code.Length >= value.Length)
            {
                string[] codeArray = Code.Split(new string[] { value }, StringSplitOptions.None);
                if (codeArray.Length > 1)
                {
                    CombinationTemplateLink link = this;
                    int index = 0;
                    foreach (string code in codeArray)
                    {
                        if (index == 0) this.Code = code;
                        else
                        {
                            link.next = new CombinationTemplateLink(row, col);
                            link = link.next;
                            link.next = new CombinationTemplateLink(code);
                            link = link.next;
                        }
                        ++index;
                    }
                    if (!object.ReferenceEquals(this, next)) link.next = next;
                    isSplit = true;
                }
            }
            return object.ReferenceEquals(this, next) ? null : next;
        }
    }
}
