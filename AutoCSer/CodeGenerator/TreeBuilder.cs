using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// CSharp代码建树器
    /// </summary>
    internal sealed class TreeBuilder
    {
        /// <summary>
        /// #region代码段分组名称
        /// </summary>
        public const string RegionGroupName = "region";
        /// <summary>
        /// command代码段分组名称
        /// </summary>
        public const string CommandGroupName = "command";
        /// <summary>
        /// 内容分组名称
        /// </summary>
        public const string ContentGroupName = "content";
        /// <summary>
        /// @取值command
        /// </summary>
        public readonly static string AtCommand = Coder.Command.AT.ToString();
        /// <summary>
        /// 所有command
        /// </summary>
        public readonly static string Commands = EnumAttribute<Coder.Command>.Array().joinString('|', value => value.ToString());
        /// <summary>
        /// command后续取值范围
        /// </summary>
        public const string CommandContent = @"[0-9A-Za-z\.\=]+";
        /// <summary>
        /// #region代码段
        /// </summary>
        private static readonly Regex regionTag = new Regex(@"\r?\n *#(?<" + RegionGroupName + @">" + RegionGroupName + @"|endregion) +(?<" + CommandGroupName + @">" + Commands + @")( +(?<" + ContentGroupName + @">" + CommandContent + @"))? *(?=\r?\n)", RegexOptions.Compiled);
        /// <summary>
        /// /**/注释代码段
        /// </summary>
        private static readonly Regex noteTag = new Regex(@"\/\*(?<" + CommandGroupName + @">" + Commands + @")(\:(?<" + ContentGroupName + @">" + CommandContent + @"))?\*\/", RegexOptions.Compiled);
        /// <summary>
        /// @后续取值范围
        /// </summary>
        public const string AtContent = @"[0-9A-Za-z\.]+";
        /// <summary>
        /// @取值
        /// </summary>
        private static readonly Regex atTag = new Regex(@"@(?<" + ContentGroupName + @">" + AtContent + @")", RegexOptions.Compiled);
        /// <summary>
        /// CSharp代码树节点标识
        /// </summary>
        public sealed class Tag : IEquatable<Tag>
        {
            /// <summary>
            /// 树节点标识类型
            /// </summary>
            public enum Type
            {
                /// <summary>
                /// 普通代码段
                /// </summary>
                Code,
                /// <summary>
                /// #region代码段
                /// </summary>
                Region,
                /// <summary>
                /// /**/注释代码段
                /// </summary>
                Note,
                /// <summary>
                /// @取值代码
                /// </summary>
                At,
            }
            /// <summary>
            /// 树节点标识类型
            /// </summary>
            public Type TagType;
            /// <summary>
            /// 标识command
            /// </summary>
            public string Command;
            /// <summary>
            /// 内容
            /// </summary>
            public SubString Content;
            /// <summary>
            /// 判断树节点标识是否相等
            /// </summary>
            /// <param name="other">树节点标识</param>
            /// <returns>是否相等</returns>
            public bool Equals(Tag other)
            {
                return other != null && other.TagType == TagType && other.Command == Command && other.Content.Equals(ref Content);
            }
            /// <summary>
            /// 转换成字符串
            /// </summary>
            /// <returns>字符串</returns>
            public override string ToString()
            {
                switch (TagType)
                {
                    case Type.Region:
                        return "#region " + Command + " " + Content.ToString();
                    case Type.Note:
                        return "/*" + Command + ":" + Content.ToString() + "*/";
                    case Type.At:
                        return "@" + Content.ToString();
                }
                if (Content.String != null)
                {
                    string code = Content.ToString().Replace('\r', ' ').Replace('\n', ' ');
                    if (code.Length > 64) return code.Substring(0, 32) + " ... " + code.Substring(code.Length - 32);
                    return code;
                }
                return null;
            }
        }
        /// <summary>
        /// CSharp代码树节点
        /// </summary>
        internal sealed class Node : TreeTemplate<Node>.INode, TreeBuilder<Node, Tag>.INode
        {
            /// <summary>
            /// 树节点标识
            /// </summary>
            public Tag Tag { get; set; }
            /// <summary>
            /// 模板命令
            /// </summary>
            public string TemplateCommand
            {
                get { return Tag.Command; }
            }
            /// <summary>
            /// 模板成员名称
            /// </summary>
            public SubString TemplateMemberName
            {
                get { return Tag.Command != null ? Tag.Content : default(SubString); }
            }
            /// <summary>
            /// 模板成员名称
            /// </summary>
            public SubString TemplateMemberNameBeforeAt
            {
                get
                {
                    SubString name = TemplateMemberName;
                    int index = name.IndexOf('@');
                    return index == -1 ? name : name.GetSub(0, index);
                }
            }
            /// <summary>
            /// 模板文本代码
            /// </summary>
            public SubString TemplateCode
            {
                get { return Tag.Command == null ? Tag.Content : default(SubString); }
            }
            /// <summary>
            /// 子节点数量
            /// </summary>
            public int ChildCount
            {
                get { return childs.Length; }
            }
            /// <summary>
            /// 子节点集合
            /// </summary>
            public IEnumerable<Node> Childs
            {
                get { return childs; }
            }
            /// <summary>
            /// 子节点集合
            /// </summary>
            private LeftArray<Node> childs;
            /// <summary>
            /// 设置子节点集合
            /// </summary>
            /// <param name="childs">子节点集合</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void SetChilds(Node[] childs)
            {
                this.childs.Set(childs);
            }
            /// <summary>
            /// 获取第一个匹配的子孙节点
            /// </summary>
            /// <param name="command">模板命令类型</param>
            /// <param name="content">内容</param>
            /// <returns>匹配的CSharp代码树节点</returns>
            public Node GetFirstNodeByTag(Coder.Command command, ref SubString content)
            {
                if (Tag.Command == command.ToString() && content.Equals(ref Tag.Content)) return this;
                foreach (Node son in childs)
                {
                    Node value = son.GetFirstNodeByTag(command, ref content);
                    if (value != null) return value;
                }
                return null;
            }
        }
        /// <summary>
        /// 建树器
        /// </summary>
        private TreeBuilder<Node, Tag> tree = new TreeBuilder<Node, Tag>();
        /// <summary>
        /// 根据代码获取代码树
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns>代码树</returns>
        internal Node Create(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                try
                {
                    tree.Empty();
                    region(code);
                    Node boot = new Node();
                    boot.SetChilds(tree.End());
                    return boot;
                }
                catch (Exception error)
                {
                    Messages.Add(error);
                }
            }
            return null;
        }
        /// <summary>
        /// 解析#region代码段
        /// </summary>
        /// <param name="code">代码</param>
        private void region(string code)
        {
            int index = 0;
            foreach (Match match in regionTag.Matches(code))
            {
                int length = match.Index - index;
                if (length != 0) note(code.Substring(index, length));
                Group name = match.Groups[ContentGroupName];
                Tag tag = new Tag
                {
                    TagType = Tag.Type.Region,
                    Command = match.Groups[CommandGroupName].Value,
                    Content = name != null ? name.Value : null
                };
                if (match.Groups[RegionGroupName].Value == RegionGroupName) tree.Append(new Node { Tag = tag });
                else tree.Round(tag);
                index = match.Index + match.Length;
            }
            if (index != code.Length) note(code.Substring(index));
        }
        /// <summary>
        /// 解析/**/注释代码段
        /// </summary>
        /// <param name="code">代码</param>
        private void note(string code)
        {
            int index = 0;
            foreach (Match match in noteTag.Matches(code))
            {
                int length = match.Index - index;
                if (length != 0) at(code.Substring(index, length));
                Group name = match.Groups[ContentGroupName];
                Tag tag = new Tag
                {
                    TagType = Tag.Type.Note,
                    Command = match.Groups[CommandGroupName].Value,
                    Content = name != null ? name.Value : null
                };
                bool isAt = tag.Command == AtCommand;
                if (isAt || !tree.IsRound(tag, false)) tree.Append(new Node { Tag = tag }, !isAt);
                index = match.Index + match.Length;
            }
            if (index != code.Length) at(code.Substring(index));
        }
        /// <summary>
        /// 解析@取值
        /// </summary>
        /// <param name="code">代码</param>
        private void at(string code)
        {
            int index = 0;
            foreach (Match match in atTag.Matches(code))
            {
                int length = match.Index - index;
                if (length != 0) this.code(new SubString { String = code, Start = index, Length = length });
                tree.Append(new Node
                {
                    Tag = new Tag
                    {
                        TagType = Tag.Type.At,
                        Command = AtCommand,
                        Content = match.Groups[ContentGroupName].Value
                    }
                }, false);
                index = match.Index + match.Length;
            }
            this.code(new SubString { String = code, Start = index, Length = code.Length - index });
        }
        /// <summary>
        /// 普通代码段
        /// </summary>
        /// <param name="code">代码</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void code(SubString code)
        {
            tree.Append(new Node
            {
                Tag = new Tag
                {
                    TagType = Tag.Type.Code,
                    Content = code
                }
            }, false);
        }
    }
    /// <summary>
    /// 建树器
    /// </summary>
    /// <typeparam name="nodeType">树节点类型</typeparam>
    /// <typeparam name="tagType">树节点标识类型</typeparam>
    internal sealed class TreeBuilder<nodeType, tagType>
        where nodeType : TreeBuilder<nodeType, tagType>.INode
        where tagType : IEquatable<tagType>
    {
        /// <summary>
        /// 节点接口
        /// </summary>
        public interface INode
        {
            /// <summary>
            /// 树节点标识
            /// </summary>
            tagType Tag { get; }
            /// <summary>
            /// 设置子节点集合
            /// </summary>
            /// <param name="childs">子节点集合</param>
            void SetChilds(nodeType[] childs);
        }
        /// <summary>
        /// 检测节点回合状态
        /// </summary>
        private enum CheckType
        {
            /// <summary>
            /// 节点回合成功
            /// </summary>
            Ok,
            /// <summary>
            /// 缺少回合节点
            /// </summary>
            LessRound,
            /// <summary>
            /// 未知的回合节点
            /// </summary>
            UnknownRound,
        }
        /// <summary>
        /// 当前节点集合
        /// </summary>
        private LeftArray<KeyValue<nodeType, bool>> nodes = new LeftArray<KeyValue<nodeType, bool>>();
        /// <summary>
        /// 清除节点
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Empty()
        {
            nodes.Length = 0;
        }
        /// <summary>
        /// 追加新节点
        /// </summary>
        /// <param name="node">新节点</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Append(nodeType node)
        {
            nodes.Add(new KeyValue<nodeType, bool>(node, true));
        }
        /// <summary>
        /// 追加新节点
        /// </summary>
        /// <param name="node">新节点</param>
        /// <param name="isRound">是否需要判断回合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Append(nodeType node, bool isRound)
        {
            nodes.Add(new KeyValue<nodeType, bool>(node, isRound));
        }
        /// <summary>
        /// 节点回合
        /// </summary>
        /// <param name="tagName">树节点标识</param>
        /// <param name="isAny">是否匹配任意索引位置,否则只能匹配最后一个索引位置</param>
        /// <returns>节点回合是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool IsRound(tagType tagName, bool isAny)
        {
            return round(tagName, isAny) == CheckType.Ok;
        }
        /// <summary>
        /// 建树结束
        /// </summary>
        /// <returns>根节点集合</returns>
        public nodeType[] End()
        {
            //if (checkRound != null)
            //{
            KeyValue<nodeType, bool>[] array = nodes.Array;
            for (int index = nodes.Length; index != 0; )
            {
                //if (array[--index].Value && checkRound(array[index].Key))
                if (array[--index].Value)
                {
                    throw new OverflowException(@"缺少回合节点
" + nodes.JoinString(@"
", value => value.Key.Tag.ToString()));
                }
            }
            //}
            return nodes.GetArray(value => value.Key);
        }
        /// <summary>
        /// 节点回合
        /// </summary>
        /// <param name="tagName">树节点标识</param>
        /// <param name="isAny"></param>
        /// <returns>节点回合状态</returns>
        private CheckType round(tagType tagName, bool isAny)
        {
            KeyValue<nodeType, bool>[] array = nodes.Array;
            for (int index = nodes.Length; index != 0; )
            {
                if (array[--index].Value)
                {
                    nodeType node = array[index].Key;
                    if (tagName.Equals(node.Tag))
                    {
                        array[index].Set(node, false);
                        node.SetChilds(new SubArray<KeyValue<nodeType, bool>> { Array = array, Start = ++index, Length = nodes.Length - index }.GetArray(value => value.Key));
                        nodes.Length = index;
                        return CheckType.Ok;
                    }
                    else if (!isAny) return CheckType.LessRound;
                    //else if (checkRound != null && checkRound(node)) return checkType.LessRound;
                }
            }
            return CheckType.UnknownRound;
        }
        /// <summary>
        /// 节点回合
        /// </summary>
        /// <param name="tagName">树节点标识</param>
        public void Round(tagType tagName)
        {
            CheckType check = round(tagName, false);
            switch (check)
            {
                case CheckType.LessRound:
                    throw new OverflowException("缺少回合节点 : " + tagName.ToString() + @"
" + nodes.JoinString(@"
", value => value.Key.Tag.ToString()));
                case CheckType.UnknownRound:
                    throw new OverflowException("未知的回合节点 : " + tagName.ToString() + @"
" + nodes.JoinString(@"
", value => value.Key.Tag.ToString()));
            }
        }
    }
}
