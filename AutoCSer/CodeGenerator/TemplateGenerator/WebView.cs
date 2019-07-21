using System;
using AutoCSer.Extension;
using System.IO;
using System.Reflection;
using AutoCSer.CodeGenerator.Metadata;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// WEB 视图配置
    /// </summary>
    internal sealed partial class WebView
    {
        /// <summary>
        /// 获取视图加载函数+视图加载函数配置
        /// </summary>
        /// <param name="type">视图类型</param>
        /// <param name="attribute">视图加载函数配置</param>
        /// <returns>视图加载函数</returns>
        private static AutoCSer.CodeGenerator.Metadata.MethodIndex getLoadMethod(Type type, out AutoCSer.WebView.ViewAttribute attribute)
        {
            AutoCSer.CodeGenerator.Metadata.MethodIndex loadMethod = null;
            foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (method.ReturnType == typeof(bool))
                {
                    foreach (AutoCSer.WebView.ViewAttribute loadWebView in method.GetCustomAttributes(typeof(AutoCSer.WebView.ViewAttribute), true))
                    {
                        attribute = loadWebView;
                        return new AutoCSer.CodeGenerator.Metadata.MethodIndex(method, AutoCSer.Metadata.MemberFilters.Instance, 0);
                    }
                    if (loadMethod == null && method.Name == "loadView" && method.GetParameters().Length != 0)
                    {
                        loadMethod = new AutoCSer.CodeGenerator.Metadata.MethodIndex(method, AutoCSer.Metadata.MemberFilters.Instance, 0);
                    }
                }
            }
            attribute = loadMethod == null ? null : AutoCSer.WebView.View.DefaultAttribute;
            return loadMethod;
        }
        /// <summary>
        /// HTML 模板建树器
        /// </summary>
        internal sealed class ViewTreeBuilder : AutoCSer.WebView.ViewTreeBuilder
        {
            /// <summary>
            /// @取值command
            /// </summary>
            private readonly static string atCommand = AutoCSer.WebView.ViewTreeCommand.At.ToString();
            /// <summary>
            /// 建树器
            /// </summary>
            private TreeBuilder<ViewTreeNode, AutoCSer.WebView.ViewTreeTag> tree;
            /// <summary>
            /// 树节点
            /// </summary>
            public ViewTreeNode Boot { get; private set; }
            /// <summary>
            /// HTML模板建树器
            /// </summary>
            /// <param name="html">HTML</param>
            public ViewTreeBuilder(string html)
                : base(html, false)
            {
                tree = new TreeBuilder<ViewTreeNode, AutoCSer.WebView.ViewTreeTag>();
                create(formatHtml(html));
            }
            /// <summary>
            /// 添加HTML片段
            /// </summary>
            /// <param name="html">HTML片段</param>
            protected override void appendHtml(SubString html)
            {
                tree.Append(new ViewTreeNode { Tag = new AutoCSer.WebView.ViewTreeTag { Type = AutoCSer.WebView.ViewTreeTagType.Html, Content = html } }, false);
                base.appendHtml(ref html);
            }
            /// <summary>
            /// 添加 @ 节点
            /// </summary>
            /// <param name="content"></param>
            protected override void appendAtNode(SubString content)
            {
                tree.Append(new ViewTreeNode { Tag = new AutoCSer.WebView.ViewTreeTag { Type = AutoCSer.WebView.ViewTreeTagType.At, Command = atCommand, Content = content } }, false);
            }
            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="tagIndex"></param>
            /// <param name="tag"></param>
            protected override void appendNode(int tagIndex, AutoCSer.WebView.ViewTreeTag tag)
            {
                if (!tree.IsRound(tag, tagIndex == clientCommandIndex)) tree.Append(new ViewTreeNode { Tag = tag });
            }
            /// <summary>
            /// HTML模板树创建完毕
            /// </summary>
            protected override void onCreated()
            {
                (Boot = new ViewTreeNode()).SetChilds(tree.End());
            }
            /// <summary>
            /// 获取HTML片段索引号
            /// </summary>
            /// <param name="html">HTML片段</param>
            /// <returns>索引号</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public int GetHtmlIndex(ref SubString html)
            {
                //if (!htmls.ContainsKey(html)) log.Default.Add("->" + html + "<-", false, false);
                return Htmls[html];
            }
        }
        /// <summary>
        /// HTML模板树节点
        /// </summary>
        internal sealed class ViewTreeNode : AutoCSer.CodeGenerator.TreeTemplate<ViewTreeNode>.INode, TreeBuilder<ViewTreeNode, AutoCSer.WebView.ViewTreeTag>.INode
        {
            /// <summary>
            /// 树节点标识
            /// </summary>
            public AutoCSer.WebView.ViewTreeTag Tag { get; internal set; }
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
                get { return Tag.Command.String != null ? Tag.Content : default(SubString); }
            }
            /// <summary>
            /// 模板文本代码
            /// </summary>
            public SubString TemplateCode
            {
                get { return Tag.Command.String == null ? Tag.Content : default(SubString); }
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
            /// 子节点集合
            /// </summary>
            private LeftArray<ViewTreeNode> childs;
            /// <summary>
            /// 子节点集合
            /// </summary>
            public IEnumerable<ViewTreeNode> Childs
            {
                get { return childs; }
            }
            /// <summary>
            /// 子节点数量
            /// </summary>
            public int ChildCount
            {
                get { return childs.Length; }
            }
            /// <summary>
            /// 设置子节点集合
            /// </summary>
            /// <param name="childs">子节点集合</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void SetChilds(ViewTreeNode[] childs)
            {
                if (Tag != null) Tag.IsRound = true;
                this.childs = new LeftArray<ViewTreeNode>(childs);
            }
        }
        /// <summary>
        /// HTML模板解析
        /// </summary>
        internal class TreeTemplate : TreeTemplate<ViewTreeNode>
        {
            /// <summary>
            /// AJAX引号替换
            /// </summary>
            private static readonly Regex ajaxQuoteRegex = new Regex(@"(js\.WriteNotNull\(@"".?""\);)", RegexOptions.Compiled);
            /// <summary>
            /// 视图AJAX成员节点有序名称排序
            /// </summary>
            private static readonly Func<KeyValue<SubString, MemberNode>, KeyValue<SubString, MemberNode>, int> ajaxNameSortHandle = ajaxNameSort;
            /// <summary>
            /// HTML模板建树器
            /// </summary>
            private ViewTreeBuilder treeBuilder;
            /// <summary>
            /// HTML片段数量
            /// </summary>
            internal int HtmlCount
            {
                get { return treeBuilder.Htmls.Count; }
            }
            /// <summary>
            /// 视图查询参数名称
            /// </summary>
            private string viewQueryName;
            /// <summary>
            /// 集合是否支持length属性
            /// </summary>
            protected override bool isCollectionLength { get { return true; } }
            /// <summary>
            /// HTML模板解析
            /// </summary>
            /// <param name="type">模板关联视图类型</param>
            /// <param name="html">HTML模板</param>
            public TreeTemplate(Type type, string html)
                : base(type, Messages.Add, Messages.Message)
            {
                creators[AutoCSer.WebView.ViewTreeCommand.Note.ToString()] = creators[AutoCSer.WebView.ViewTreeCommand.Client.ToString()] = note;
                creators[AutoCSer.WebView.ViewTreeCommand.Loop.ToString()] = creators[AutoCSer.WebView.ViewTreeCommand.For.ToString()] = loop;
                creators[AutoCSer.WebView.ViewTreeCommand.At.ToString()] = at;
                creators[AutoCSer.WebView.ViewTreeCommand.Value.ToString()] = push;
                creators[AutoCSer.WebView.ViewTreeCommand.If.ToString()] = ifThen;
                creators[AutoCSer.WebView.ViewTreeCommand.Not.ToString()] = not;
                AutoCSer.WebView.ViewAttribute attribute;
                AutoCSer.CodeGenerator.Metadata.MethodIndex loadMethod = getLoadMethod(type, out attribute);
                if (attribute != null && type.GetField(attribute.QueryName, BindingFlags.Instance | BindingFlags.NonPublic) == null)
                {
                    viewQueryName = attribute.QueryName + ".";
                }
                skin((treeBuilder = new ViewTreeBuilder(html)).Boot);
            }
            /// <summary>
            /// 检测错误成员名称
            /// </summary>
            /// <param name="memberName">成员名称</param>
            /// <returns>是否忽略错误</returns>
            protected override bool checkErrorMemberName(ref SubString memberName)
            {
                return viewQueryName != null && memberName.Length >= viewQueryName.Length && memberName.GetSub(0, viewQueryName.Length).Equals(viewQueryName);
            }
            /// <summary>
            /// 添加代码
            /// </summary>
            /// <param name="code">代码</param>
            protected override void pushCode(SubString code)
            {
                if (ignoreCode == 0)
                {
                    Code.Append(@"
                    _html_.WriteNotNull(htmls[", treeBuilder.GetHtmlIndex(ref code).toString(), "]);");
                }
            }
            /// <summary>
            /// 截断代码字符串
            /// </summary>
            protected override void pushCode() { }
            /// <summary>
            /// 检测成员名称
            /// </summary>
            /// <param name="memberName"></param>
            /// <param name="isClient"></param>
            protected override void checkMemberName(ref SubString memberName, ref bool isClient)
            {
                int index = memberName.IndexOf('#');
                if (index != -1)
                {
                    isClient = true;
                    memberName.Sub(0, index);
                }
            }
            /// <summary>
            /// 输出绑定的数据
            /// </summary>
            /// <param name="node">代码树节点</param>
            protected override void at(ViewTreeNode node)
            {
                bool isToHtml = false, isToTextArea = false, isClient = false, isDepth;
                SubString memberName = node.TemplateMemberName;
                if (memberName.Length != 0)
                {
                    if (memberName[0] == '$') return;
                    if (memberName[0] == '@')
                    {
                        memberName.MoveStart(1);
                        isToHtml = true;
                    }
                    else if (memberName[0] == '*')
                    {
                        memberName.MoveStart(1);
                        isToTextArea = true;
                    }
                    int clientIndex = memberName.IndexOf('#');
                    if (clientIndex != -1)
                    {
                        memberName.Sub(0, clientIndex);
                        isClient = true;
                    }
                }
                MemberNode member = null;
                if (memberName.Length == 0) isDepth = false;
                else member = getMember(ref memberName, out isDepth);
                if (member != null && !isClient) ifThen(member, ref memberName, isDepth, value => at(value, isToHtml, isToTextArea));
            }
            /// <summary>
            /// 输出绑定的数据
            /// </summary>
            /// <param name="member">成员节点</param>
            /// <param name="isToHtml"></param>
            /// <param name="isToTextArea"></param>
            protected void at(MemberNode member, bool isToHtml, bool isToTextArea)
            {
                if (ignoreCode == 0)
                {
                    string call = "Write";
                    if (isToHtml) call = "WriteHtml";
                    if (isToTextArea) call = "WriteTextArea";
                    if (member.Type.IsString || member.Type.IsSubString || member.Type.IsHashUrl || member.Type.IsChar)
                    {
                        Code.Append(@"
                        _html_.", call, "(", member.AwaitPath, ");");
                    }
                    else if (member.Type.IsNumberToString)
                    {
                        Code.Append(@"
                        _html_.Write(", member.AwaitPath, ");");
                    }
                    else
                    {
                        Code.Append(@"
                        _html_.", call, "(", member.AwaitPath, ".ToString());");
                    }
                }
            }
            /// <summary>
            /// 视图AJAX代码
            /// </summary>
            private CharStream ajaxCode;
            /// <summary>
            /// 视图AJAX输出
            /// </summary>
            /// <returns>AJAX字符串</returns>
            public string Ajax()
            {
                using (ajaxCode = new CharStream())
                {
                    ajaxCode.WriteNotNull(@"_js_.WriteNotNull(@""");
                    ajax(currentMembers[0], null);
                    ajaxCode.WriteNotNull(@""");");
                    return ajaxQuoteRegex.Replace(ajaxCode.ToString(), ajaxQuote);
                }
            }
            /// <summary>
            /// 视图AJAX成员节点代码
            /// </summary>
            /// <param name="node">成员节点</param>
            /// <param name="parentPath">父节点路径</param>
            private void ajax(MemberNode node, string parentPath)
            {
                KeyValue<SubString, MemberNode>[] members = ajaxName(node);
                KeyValue<SubString, MemberNode> loopMember = default(KeyValue<SubString, MemberNode>);
                foreach (KeyValue<SubString, MemberNode> member in members)
                {
                    if (member.Key.Length == 0)
                    {
                        loopMember = member;
                        break;
                    }
                }
                if (loopMember.Key.String != null)
                {
                    bool isDepth;
                    MemberNode member = getMember(ref loopMember.Key, out isDepth);
                    ExtensionType type = member.Type.EnumerableArgumentType;
                    ajaxCode.Write(@"["");
                    {
                        int ", loopIndex(0), @" = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (", type.FullName, " ", path(currentMembers.Length) + " in ", (parentPath ?? member.AwaitPath), @")
                        {");
                    if (loopMember.Value.IsNextPath)
                    {
                        ajaxCode.WriteNotNull(@"
                            if (_loopIndex_ == 0)
                            {
                                _js_.Write('""');
                                _js_.WriteNotNull(""");
                        ajaxLoopNames(loopMember.Value);
                        ajaxCode.WriteNotNull(@""");
                                _js_.Write('""');
                            }
                            _js_.Write(',');");
                        if (type.IsNull)
                        {
                            ajaxCode.Write(@"
                            if (", path(currentMembers.Length), @" == null) _js_.WriteJsonNull();
                            else
                            {");
                        }
                        ajaxCode.WriteNotNull(@"
                                _js_.WriteNotNull(@""[");
                        pushMember(member.Get(string.Empty));
                        ajaxLoop(loopMember.Value);
                        --currentMembers.Length;
                        ajaxCode.WriteNotNull(@"]"");");
                        if (type.IsNull)
                        {
                            ajaxCode.WriteNotNull(@"
                            }");
                        }
                        ajaxCode.Write(@"
                            ++_loopIndex_;
                        }
                        _loopIndex_ = ", loopIndex(0), @";
                    }
                    _js_.WriteNotNull(@""]", AutoCSer.WebView.AjaxBase.FormatView);
                    }
                    else
                    {
                        ajaxCode.WriteNotNull(@"
                            if (_loopIndex_ != 0) _js_.Write(',');");
                        if (type.IsNull)
                        {
                            ajaxCode.Write(@"
                                if (", path(currentMembers.Length), @" == null) _js_.WriteJsonNull();
                                else
                                {");
                        }
                        ajaxValue(type, path(currentMembers.Length));
                        if (type.IsNull)
                        {
                            ajaxCode.WriteNotNull(@"
                                }");
                        }
                        ajaxCode.Write(@"
                            ++_loopIndex_;
                        }
                        _loopIndex_ = ", loopIndex(0), @";
                    }
                    _js_.WriteNotNull(@""]");
                    }
                }
                else
                {
                    int memberIndex = 0;
                    bool isDepth;
                    string clientTypeName = node.Type.ClientViewTypeName, clientMemberName = null;
                    if (clientTypeName != null)
                    {
                        clientMemberName = node.Type.ClientViewMemberName;
                        if (clientMemberName == null)
                        {
                            ajaxCode.WriteNotNull("new ");
                            ajaxCode.WriteNotNull(clientTypeName);
                            ajaxCode.Write('(');
                        }
                        else
                        {
                            ajaxCode.WriteNotNull(clientTypeName);
                            ajaxCode.WriteNotNull(".Get(");
                        }
                    }
                    ajaxCode.Write('{');
                    foreach (KeyValue<SubString, MemberNode> name in members)
                    {
                        if (!name.Value.IsIgnoreNull)
                        {
                            SubString nameKey = name.Key;
                            MemberNode member = getMember(ref nameKey, out isDepth);
                            if (memberIndex++ != 0) ajaxCode.Write(',');
                            ajaxCode.Write(ref nameKey);
                            ajaxCode.Write(':');
                            pushMember(member);
                            ajaxCode.Write(@""");
                    {
                        ", member.Type.FullName, " ", path(0), " = ", member.AwaitPath, ";");
                            if (member.Type.IsNull)
                            {
                                ajaxCode.Write(@"
                        if (", path(0), @" == null) _js_.WriteJsonNull();
                        else
                        {");
                            }
                            if (name.Value.IsNextPath)
                            {
                                ajaxCode.WriteNotNull(@"
                            _js_.WriteNotNull(@""");
                                ajax(name.Value, path(0));
                                ajaxCode.WriteNotNull(@""");");
                            }
                            else ajaxValue(member.Type, path(0));
                            if (member.Type.IsNull)
                            {
                                ajaxCode.WriteNotNull(@"
                        }");
                            }
                            ajaxCode.WriteNotNull(@"
                    }
                    _js_.WriteNotNull(@""");
                            --currentMembers.Length;
                        }
                    }
                    if (memberIndex == 0)
                    {
                        string memberIgnoreName = memberIgnore(0);
                        ajaxCode.Write(@""");
                    bool ", memberIgnoreName, @" = false;");
                        foreach (KeyValue<SubString, MemberNode> name in members)
                        {
                            if (name.Value.IsIgnoreNull)
                            {
                                SubString nameKey = name.Key;
                                MemberNode member = getMember(ref nameKey, out isDepth);
                                pushMember(member);
                                ajaxCode.Write(@"
                    {
                        ", member.Type.FullName, " ", path(0), " = ", member.AwaitPath, ";");
                                if (member.Type.IsNull)
                                {
                                    ajaxCode.Write(@"
                        if (", path(0), @" != null)
                        {");
                                }
                                else if (member.Type.IsNumber)
                                {
                                    ajaxCode.Write(@"
                        if (", path(0), @" != 0)
                        {");
                                }
                                else if (member.Type.IsBool)
                                {
                                    ajaxCode.Write(@"
                        if (", path(0), @")
                        {");
                                }
                                ajaxCode.Write(@"
                            if (", memberIgnoreName, @") _js_.Write(',');
                            else ", memberIgnoreName, @" = true;
                            _js_.WriteNotNull(@""");
                                ajaxCode.Write(ref nameKey);
                                ajaxCode.Write(':');
                                if (name.Value.IsNextPath)
                                {
                                    ajax(name.Value, path(0));
                                    ajaxCode.WriteNotNull(@""");");
                                }
                                else
                                {
                                    ajaxCode.WriteNotNull(@""");");
                                    ajaxValue(member.Type, path(0));
                                }
                                if (member.Type.IsNull || member.Type.IsNumber || member.Type.IsBool)
                                {
                                    ajaxCode.WriteNotNull(@"
                        }");
                                }
                                ajaxCode.WriteNotNull(@"
                    }");
                                --currentMembers.Length;
                            }
                        }
                        ajaxCode.WriteNotNull(@"
                    _js_.WriteNotNull(@""");
                    }
                    else
                    {
                        foreach (KeyValue<SubString, MemberNode> name in members)
                        {
                            if (name.Value.IsIgnoreNull)
                            {
                                SubString nameKey = name.Key;
                                MemberNode member = getMember(ref nameKey, out isDepth);
                                pushMember(member);
                                ajaxCode.Write(@""");
                    {
                        ", member.Type.FullName, " ", path(0), " = ", member.AwaitPath, ";");
                                if (member.Type.IsNull)
                                {
                                    ajaxCode.Write(@"
                        if (", path(0), @" != null)
                        {");
                                }
                                else if (member.Type.IsNumber)
                                {
                                    ajaxCode.Write(@"
                        if (", path(0), @" != 0)
                        {");
                                }
                                else if (member.Type.IsBool)
                                {
                                    ajaxCode.Write(@"
                        if (", path(0), @")
                        {");
                                }
                                ajaxCode.WriteNotNull(@"
                            _js_.WriteNotNull(@"",");
                                ajaxCode.Write(ref nameKey);
                                ajaxCode.Write(':');
                                if (name.Value.IsNextPath)
                                {
                                    ajax(name.Value, path(0));
                                    ajaxCode.WriteNotNull(@""");");
                                }
                                else
                                {
                                    ajaxCode.WriteNotNull(@""");");
                                    ajaxValue(member.Type, path(0));
                                }
                                if (member.Type.IsNull || member.Type.IsNumber || member.Type.IsBool)
                                {
                                    ajaxCode.WriteNotNull(@"
                        }");
                                }
                                ajaxCode.WriteNotNull(@"
                    }
                    _js_.WriteNotNull(@""");
                                --currentMembers.Length;
                            }
                        }
                    }
                    ajaxCode.Write('}');
                    if (clientTypeName != null) ajaxCode.Write(')');
                }
            }
            /// <summary>
            /// 视图AJAX成员节点有序名称集合
            /// </summary>
            /// <param name="node">成员节点</param>
            /// <returns>视图AJAX成员节点有序名称集合</returns>
            private KeyValue<SubString, MemberNode>[] ajaxName(MemberNode node)
            {
                Dictionary<HashString, MemberNode> members;
                return memberPaths.TryGetValue(node, out members) ? members.getArray(value => new KeyValue<SubString, MemberNode>(value.Key.String, value.Value)).sort(ajaxNameSortHandle) : NullValue<KeyValue<SubString, MemberNode>>.Array;
            }
            /// <summary>
            /// 视图AJAX循环成员节点名称
            /// </summary>
            /// <param name="node">成员节点</param>
            private void ajaxLoopNames(MemberNode node)
            {
                KeyValue<SubString, MemberNode>[] members = ajaxName(node);
                foreach (KeyValue<SubString, MemberNode> member in members)
                {
                    if (member.Key.Length == 0)
                    {
                        ajaxCode.Write('[');
                        if (member.Value.IsNextPath) ajaxLoopNames(member.Value);
                        ajaxCode.Write(']');
                        return;
                    }
                }
                int memberIndex = 0;
                string clientTypeName = node.Type.ClientViewTypeName;
                if (clientTypeName != null)
                {
                    memberIndex = 1;
                    ajaxCode.Write(AutoCSer.WebView.AjaxBase.ViewClientType);
                    if (node.Type.ClientViewMemberName != null) ajaxCode.Write(AutoCSer.WebView.AjaxBase.ViewClientMember);
                    ajaxCode.WriteNotNull(clientTypeName);
                    ajaxCode.Write(',');
                }
                foreach (KeyValue<SubString, MemberNode> name in members)
                {
                    if (memberIndex != 0) ajaxCode.Write(',');
                    ajaxCode.Write(name.Key);
                    if (name.Value.IsNextPath)
                    {
                        ajaxCode.Write('[');
                        ajaxLoopNames(name.Value);
                        ajaxCode.Write(']');
                        memberIndex = 0;
                    }
                    else memberIndex = 1;
                }
            }
            /// <summary>
            /// 视图AJAX循环成员节点数据
            /// </summary>
            /// <param name="node">成员节点</param>
            private void ajaxLoop(MemberNode node)
            {
                KeyValue<SubString, MemberNode>[] members = ajaxName(node);
                KeyValue<SubString, MemberNode> loopMember = default(KeyValue<SubString, MemberNode>);
                foreach (KeyValue<SubString, MemberNode> member in members)
                {
                    if (member.Key.Length == 0)
                    {
                        loopMember = member;
                        break;
                    }
                }
                if (loopMember.Key.String != null)
                {
                    string parentPath = path(0);
                    bool isDepth;
                    MemberNode member = getMember(ref loopMember.Key, out isDepth);
                    System.Type type = member.Type.EnumerableArgumentType;
                    //if (memberIndex != 0) ajaxCode.Write(',');
                    ajaxCode.Write(@"["");
                    {
                        int ", loopIndex(0).ToString(), @" = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (", type.fullName(), " " + path(currentMembers.Length) + " in ", (parentPath ?? member.AwaitPath), @")
                        {
                            if (_loopIndex_ != 0) _js_.Write(',');");
                    if (type.isNull())
                    {
                        ajaxCode.Write(@"
                            if (", path(currentMembers.Length), @" == null) _js_.WriteJsonNull();
                            else
                            {");
                    }
                    if (loopMember.Value.IsNextPath)
                    {
                        ajaxCode.WriteNotNull(@"
                                _js_.Write('[');");
                        pushMember(member.Get(string.Empty));
                        ajaxCode.WriteNotNull(@"
                                _js_.WriteNotNull(@""");
                        ajaxLoop(loopMember.Value);
                        --currentMembers.Length;
                        ajaxCode.WriteNotNull(@"]"");");
                    }
                    else ajaxValue(type, path(currentMembers.Length));
                    if (type.isNull())
                    {
                        ajaxCode.WriteNotNull(@"
                            }");
                    }
                    ajaxCode.Write(@"
                            ++_loopIndex_;
                        }
                        _loopIndex_ = ", loopIndex(0), @";
                    }
                    _js_.WriteNotNull(@""]");
                }
                else
                {
                    int memberIndex = 0;
                    bool isDepth;
                    foreach (KeyValue<SubString, MemberNode> name in members)
                    {
                        if (memberIndex++ != 0) ajaxCode.Write(',');
                        SubString nameKey = name.Key;
                        MemberNode member = getMember(ref nameKey, out isDepth);
                        pushMember(member);
                        ajaxCode.Write(@""");
                    {
                        ", member.Type.FullName, " ", path(0), " = ", member.AwaitPath, ";");
                        if (member.Type.IsNull)
                        {
                            ajaxCode.Write(@"
                                if (", path(0), @" == null) _js_.WriteJsonNull();
                                else
                                {");
                        }
                        if (name.Value.IsNextPath)
                        {
                            ajaxCode.WriteNotNull(@"
                                    _js_.WriteNotNull(@""[");
                            ajaxLoop(name.Value);
                            ajaxCode.WriteNotNull(@"]"");");
                        }
                        else ajaxValue(member.Type, path(0));
                        if (member.Type.IsNull)
                        {
                            ajaxCode.WriteNotNull(@"
                                }");
                        }
                        ajaxCode.WriteNotNull(@"
                    }
                    _js_.WriteNotNull(@""");
                        --currentMembers.Length;
                    }
                }
            }
            /// <summary>
            /// 视图AJAX叶子成员节点代码
            /// </summary>
            /// <param name="type">成员类型</param>
            /// <param name="name">成员名称</param>
            private void ajaxValue(ExtensionType type, string name)
            {
                if (type.IsAjaxToString || type.IsDateTime)
                {
                    ajaxCode.Write(@"
                                    _js_.WriteJson((", type.NotNullType.FullName, ")", name, ");");
                }
                else if (type.IsString || type.IsSubString || type.IsHashUrl)
                {
                    ajaxCode.Write(@"
                                    _js_.WriteJson(", name, ");");
                }
                else if (type.Type.IsEnum)
                {
                    ajaxCode.Write(@"
                                    _js_.CopyJsonNotNull(", name, @".ToString());");
                }
                else
                {
                    string clientTypeName = type.ClientViewTypeName, clientMemberName = null;
                    if (clientTypeName != null)
                    {
                        ajaxCode.WriteNotNull(@"
                                    _js_.WriteNotNull(@""");
                        clientMemberName = type.ClientViewMemberName;
                        if (clientMemberName == null)
                        {
                            ajaxCode.WriteNotNull("new ");
                            ajaxCode.WriteNotNull(clientTypeName);
                            ajaxCode.Write('(');
                        }
                        else
                        {
                            ajaxCode.WriteNotNull(clientTypeName);
                            ajaxCode.WriteNotNull(".Get(");
                        }
                        ajaxCode.Write(@"{})"");");
                    }
                    else
                    {
                        ajaxCode.WriteNotNull(@"
                                    _js_.WriteJsonObject();");
                    }
                }
            }
            /// <summary>
            /// 获取忽略输出变量名称
            /// </summary>
            /// <param name="index">忽略输出变量层次</param>
            /// <returns>忽略输出变量名称</returns>
            private string memberIgnore(int index)
            {
                return "_memberIgnore" + (index == 0 ? (currentMembers.Length - 1) : index).ToString() + "_";
            }

            /// <summary>
            /// 视图AJAX成员节点有序名称排序
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            private static int ajaxNameSort(KeyValue<SubString, MemberNode> left, KeyValue<SubString, MemberNode> right)
            {
                return left.Key.CompareTo(ref right.Key);
            }
            /// <summary>
            /// AJAX引号替换
            /// </summary>
            private static string ajaxQuote(Match match)
            {
                return match.Length == 22 ? "_js_.Write('" + match.Value[18] + "');" : string.Empty;
            }
        }
        /// <summary>
        /// WEB 代码生成
        /// </summary>
        /// <typeparam name="attributeType"></typeparam>
        internal abstract class Generator<attributeType> : TemplateGenerator.Generator<attributeType>
            where attributeType : Attribute
        {
            /// <summary>
            /// 视图加载函数
            /// </summary>
            public MethodIndex LoadMethod;
            /// <summary>
            /// 视图加载函数配置
            /// </summary>
            public AutoCSer.WebView.ViewAttribute LoadAttribute;
            /// <summary>
            /// Session类型
            /// </summary>
            public ExtensionType SessionType;
            /// <summary>
            /// 是否WEB视图服务代码生成
            /// </summary>
            public bool IsServer;
            /// <summary>
            /// 安装入口
            /// </summary>
            /// <param name="parameter">安装参数</param>
            /// <returns>是否安装成功</returns>
            public override bool Run(ProjectParameter parameter)
            {
                if (parameter.WebConfig == null) return true;
                SessionType = parameter.WebConfig.SessionType;
                return base.Run(parameter);
            }
        }
        /// <summary>
        /// WEB视图代码生成
        /// </summary>
        [Generator(Name = "WEB 视图", DependType = typeof(WebCall.Generator), IsAuto = true)]
        internal partial class Generator : Generator<AutoCSer.WebView.ViewAttribute>
        {
            /// <summary>
            /// WEB 视图 API代码生成
            /// </summary>
            private static readonly TypeScript typeScript = new TypeScript();
            /// <summary>
            /// ajax 函数存在 await 函数的类型集合
            /// </summary>
            internal static HashSet<Type> AjaxAwaitMethodTypes = new HashSet<Type>();
            /// <summary>
            /// WEB视图类型信息
            /// </summary>
            internal sealed class ViewType
            {
                /// <summary>
                /// WEB视图类型
                /// </summary>
                public ExtensionType Type;
                /// <summary>
                /// WEB视图类型
                /// </summary>
                public ExtensionType WebViewMethodType
                {
                    get { return Type; }
                }
                /// <summary>
                /// WEB视图配置
                /// </summary>
                public AutoCSer.WebView.ViewAttribute Attribute;
                /// <summary>
                /// 视图加载函数
                /// </summary>
                public MethodIndex LoadMethod;
                /// <summary>
                /// 默认程序集名称
                /// </summary>
                public string DefaultNamespace;
                /// <summary>
                /// 序号
                /// </summary>
                public int Index;
                /// <summary>
                /// 页面序号
                /// </summary>
                public int PageIndex;
                /// <summary>
                /// 页面对象名称
                /// </summary>
                public string PageName
                {
                    get { return "_p" + PageIndex.toString(); }
                }
                /// <summary>
                /// 是否忽略大小写
                /// </summary>
                public bool IgnoreCase;
                /// <summary>
                /// WEB视图调用类型名称
                /// </summary>
                private string callTypeName
                {
                    get
                    {
                        string callName = Type.FullName;
                        if (callName.StartsWith(DefaultNamespace, StringComparison.Ordinal)) callName = callName.Substring(DefaultNamespace.Length - 1);
                        else callName = "/" + callName;
                        return callName.replaceNotNull('.', '/');
                    }
                }
                /// <summary>
                /// WEB视图调用名称
                /// </summary>
                private string callName;
                /// <summary>
                /// WEB视图调用名称
                /// </summary>
                public string CallName
                {
                    get
                    {
                        if (callName == null)
                        {
                            if ((callName = Attribute.TypeCallName) == null) callName = callTypeName;
                            else if (callName.Length == 0 || callName[0] != '/') callName = "/" + callName;
                            if (Attribute.MethodName != null) callName += "/" + Attribute.MethodName.replaceNotNull('.', '/');
                            callName += ".html";
                            if (IgnoreCase) callName = callName.toLowerNotEmpty();
                        }
                        return callName;
                    }
                }
                /// <summary>
                /// 来源重写js文件重定向
                /// </summary>
                public string RewriteJs
                {
                    get
                    {
                        string path = CallName;
                        return (path.EndsWith(".html", StringComparison.OrdinalIgnoreCase) ? path.Substring(0, path.Length - 5) : path) + ".js";
                    }
                }
                /// <summary>
                /// WEB视图重写路径
                /// </summary>
                public string RewritePath
                {
                    get
                    {
                        //if (Attribute.RewritePath != null) return Attribute.RewritePath;
                        string path = callTypeName;
                        if (path.EndsWith("/index")) path = path.Substring(0, path.Length - 5);
                        return IgnoreCase ? path.toLower() : path;
                    }
                }
                /// <summary>
                /// 是否 await 函数
                /// </summary>
                public bool IsAwaitMethod;
                /// <summary>
                /// 是否使用对象池
                /// </summary>
                public bool IsPoolType;
                /// <summary>
                /// 页面对象是否需要初始化
                /// </summary>
                public bool IsSetPage
                {
                    get { return IsAwaitMethod || Attribute.IsAsynchronous; }
                }
            }
            /// <summary>
            /// WEB视图类型集合
            /// </summary>
            private LeftArray<ViewType> views = new LeftArray<ViewType>();
            /// <summary>
            /// WEB视图类型集合
            /// </summary>
            public ViewType[] Views;
            /// <summary>
            /// WEB 调用函数查询参数名称集合
            /// </summary>
            public WebCall.Generator.CallMethod[] CallMethods
            {
                get
                {
                    Dictionary<int, WebCall.Generator.CallMethod> typeIndexs = DictionaryCreator.CreateInt<WebCall.Generator.CallMethod>();
                    foreach (WebCall.Generator.CallMethod method in WebCall.Generator.CallMethods)
                    {
                        if (method.MethodParameters.Length != 0 && method.Attribute.IsCompileQueryParser && !typeIndexs.ContainsKey(method.ParameterIndex))
                        {
                            typeIndexs.Add(method.ParameterIndex, method);
                        }
                    }
                    return typeIndexs.getArray(value => value.Value);
                }
            }
            /// <summary>
            /// WEB视图页面数量
            /// </summary>
            public int ViewPageCount;
            /// <summary>
            /// 来源路径重写数量
            /// </summary>
            public int RewriteViewCount;
            /// <summary>
            /// HTML文件名
            /// </summary>
            public string HtmlFile
            {
                get
                {
                    string value = Type.FullName;
                    if (value.StartsWith(AutoParameter.DefaultNamespace, StringComparison.Ordinal) && value[AutoParameter.DefaultNamespace.Length] == '.')
                    {
                        value = value.Substring(AutoParameter.DefaultNamespace.Length + 1).replaceNotNull('.', Path.DirectorySeparatorChar);
                    }
                    else value = value.Replace('.', Path.DirectorySeparatorChar);
                    return value + ".html";
                }
            }
            /// <summary>
            /// 查询成员信息集合
            /// </summary>
            public AutoCSer.CodeGenerator.Metadata.MemberIndex[] QueryMembers;
            /// <summary>
            /// 是否存在查询参数
            /// </summary>
            public bool IsQuery
            {
                get
                {
                    return QueryMembers.Length != 0 || LoadMethod != null;
                }
            }
            /// <summary>
            /// HTML片段数量
            /// </summary>
            public int HtmlCount;
            /// <summary>
            /// 服务器端页面代码
            /// </summary>
            public string PageCode;
            /// <summary>
            /// AJAX输出
            /// </summary>
            public string AjaxCode;
            /// <summary>
            /// 是否使用对象池
            /// </summary>
            public bool IsPoolType;
            /// <summary>
            /// 是否 await 函数
            /// </summary>
            public bool IsAwaitMethod;
            /// <summary>
            /// 页面对象是否需要初始化
            /// </summary>
            public bool IsSetPage
            {
                get { return IsAwaitMethod || Attribute.IsAsynchronous; }
            }
            /// <summary>
            /// HTML 数据字段名称
            /// </summary>
            public string Htmls;
            /// <summary>
            /// HTML 数据访问锁字段名称
            /// </summary>
            public string HtmlLock;
            /// <summary>
            /// 是否必须配置自定义属性
            /// </summary>
            public override bool IsAttribute
            {
                get { return false; }
            }
            /// <summary>
            /// 安装下一个类型
            /// </summary>
            protected override void nextCreate()
            {
                IsPoolType = typeof(AutoCSer.WebView.View<>).isAssignableFromGenericDefinition(Type);
                if (IsPoolType)
                {
                    if (Attribute == null) Attribute = AutoCSer.WebView.View.DefaultAttribute;
                }
                else
                {
                    if (Attribute == null) return;
                    if (!typeof(AutoCSer.WebView.View).IsAssignableFrom(Type))
                    {
                        Messages.Add(Type.FullName + " 必须继承自 AutoCSer.WebView.View 或者 AutoCSer.WebView.View<" + Type.FullName + ">");
                        return;
                    }
                }
                
                string fileName = AutoParameter.ProjectPath + HtmlFile;
                if (!File.Exists(fileName)) Messages.Add("未找到HTML页面文件 " + fileName);
                else
                {
                    LoadMethod = getLoadMethod(Type, out LoadAttribute);
                    if (LoadAttribute == AutoCSer.WebView.View.DefaultAttribute) LoadAttribute = Attribute;
                     QueryMembers = MemberIndex.GetMembers<AutoCSer.WebView.ViewQueryAttribute>(Type, AutoCSer.Metadata.MemberFilters.Instance, true, true);
                    if (QueryMembers.Length != 0 && LoadAttribute == null) LoadAttribute = AutoCSer.WebView.View.DefaultAttribute;
                    if (QueryMembers.Length != 0 && LoadMethod != null)
                    {
                        foreach (MethodParameter parameter in LoadMethod.Parameters)
                        {
                            if (QueryMembers.any(member => member.MemberName == parameter.ParameterName))
                            {
                                Messages.Add(Type.FullName + " 查询名称冲突 " + parameter.ParameterName);
                                return;
                            }
                        }
                    }
                    try
                    {
                        TreeTemplate template = new TreeTemplate(Type, File.ReadAllText(fileName, AutoParameter.WebConfig.Encoding));
                        IsAwaitMethod = template.IsAwaitMethod;
                        if (Attribute.IsPage) PageCode = template.CodeString;
                        if (Attribute.IsAjax)
                        {
                             AjaxCode = template.Ajax();
                            if (IsAwaitMethod) AjaxAwaitMethodTypes.Add(Type);
                        }
                        HtmlCount = template.HtmlCount;
                        int index = views.Length;
                        ViewType view = new ViewType { Type = Type, Attribute = Attribute, LoadMethod = LoadMethod, DefaultNamespace = AutoParameter.DefaultNamespace + ".", Index = index, IgnoreCase = AutoParameter.WebConfig.IgnoreCase, IsAwaitMethod = IsAwaitMethod, IsPoolType = IsPoolType };
                        views.Add(view);
                        Htmls = "_h" + index.toString();
                        HtmlLock = "_l" + index.toString();
                        IsServer = false;
                        create(true);
                        if (Attribute.IsAjax && Attribute.IsExportTypeScript) typeScript.Create(AutoParameter, Type, view, LoadMethod);
                    }
                    catch (Exception exception)
                    {
                        Messages.Add(exception);
                        throw new Exception(fileName);
                    }
                }
            }
            /// <summary>
            /// 安装完成处理
            /// </summary>
            protected override void onCreated()
            {
                if (views.Length != 0)
                {
                    Views = views.ToArray();
                    HashSet<string> rewritePaths = new HashSet<string>();
                    ViewPageCount = RewriteViewCount = 0;
                    foreach (ViewType view in Views)
                    {
                        if (view.Attribute.IsPage) view.PageIndex = ViewPageCount++;
                        string rewritePath = view.RewritePath;
                        if (rewritePaths.Contains(rewritePath))
                        {
                            Messages.Add("URL 重写冲突 " + rewritePath);
                            throw new Exception("URL 重写冲突" + rewritePath);
                        }
                        rewritePaths.Add(rewritePath);
                        if (view.Attribute.RewritePath != null)
                        {
                            string path = view.IgnoreCase ? view.Attribute.RewritePath.toLower() : view.Attribute.RewritePath;
                            if (view.Attribute.IsRewriteHtml) rewritePath = path + ".html";
                            //{
                            //    if (path.EndsWith(".html", view.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)) path = path.Substring(0, path.Length - 5);
                            //    rewritePath = path + ".html";
                            //}
                            else rewritePath = path;
                            if (rewritePaths.Contains(rewritePath = path))
                            {
                                Messages.Add("URL 重写冲突 " + rewritePath);
                                throw new Exception("URL 重写冲突" + rewritePath);
                            }
                            rewritePaths.Add(rewritePath);
                            if (rewritePaths.Contains(rewritePath = path + ".js"))
                            {
                                Messages.Add("URL 重写冲突 " + rewritePath);
                                throw new Exception("URL 重写冲突" + rewritePath);
                            }
                            rewritePaths.Add(rewritePath);
                            ++RewriteViewCount;
                        }
                    }
                    IsServer = true;
                    _code_.Length = 0;
                    create(false);
                    Coder.Add(@"
namespace " + AutoParameter.DefaultNamespace + @"
{
" + _code_.ToString() + @"
}");
                }
            }
        }
        /// <summary>
        /// AJAX API代码生成
        /// </summary>
        [Generator(Name = "WEB 视图 API", Language = CodeLanguage.TypeScript)]
        internal sealed partial class TypeScript : Generator
        {
            /// <summary>
            /// 类型
            /// </summary>
            private struct TypeKey : IEquatable<TypeKey>
            {
                /// <summary>
                /// 命名空间
                /// </summary>
                public string Namespace;
                /// <summary>
                /// 类型名称
                /// </summary>
                public string Name;
                /// <summary>
                /// 哈希值
                /// </summary>
                public int HashCode;
                /// <summary>
                /// 类型
                /// </summary>
                /// <param name="parameter"></param>
                /// <param name="type"></param>
                public TypeKey(ProjectParameter parameter, ExtensionType type)
                {
                    Namespace = type.Type.Namespace;
                    if (Namespace == parameter.DefaultNamespace)
                    {
                        Namespace = Ajax.TypeScript.AutoCSerAPI;
                        Name = "webView";
                    }
                    else
                    {
                        if (Namespace.StartsWith(parameter.DefaultNamespace, StringComparison.Ordinal) && Namespace[parameter.DefaultNamespace.Length] == '.') Namespace = Namespace.Substring(parameter.DefaultNamespace.Length);
                        int index = Namespace.IndexOf('.');
                        if (index == -1)
                        {
                            Name = Namespace;
                            Namespace = Ajax.TypeScript.AutoCSerAPI;
                        }
                        else
                        {
                            Name = Namespace.Substring(index + 1);
                            Namespace = Ajax.TypeScript.AutoCSerAPI + Namespace.Substring(0, index);
                        }
                    }
                    HashCode = Namespace.GetHashCode() ^ Name.GetHashCode();
                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="other"></param>
                /// <returns></returns>
                public bool Equals(TypeKey other)
                {
                    return HashCode == other.HashCode && Namespace == other.Namespace && Name == other.Name;
                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="obj"></param>
                /// <returns></returns>
                public override bool Equals(object obj)
                {
                    return Equals((TypeKey)obj);
                }
                /// <summary>
                /// 
                /// </summary>
                /// <returns></returns>
                public override int GetHashCode()
                {
                    return HashCode;
                }
            }
            /// <summary>
            /// 代码集合
            /// </summary>
            private static readonly ReusableDictionary<TypeKey, StringArray> codes = ReusableDictionary<TypeKey>.Create<StringArray>();
            /// <summary>
            /// 类型集合
            /// </summary>
            private static readonly Dictionary<string, TypeKey> types = DictionaryCreator.CreateOnly<string, TypeKey>();
            /// <summary>
            /// API生成
            /// </summary>
            internal static string Code
            {
                get
                {
                    if (codes.Count != 0)
                    {
                        StringArray code = new StringArray();
                        foreach (KeyValue<TypeKey, StringArray> typeCode in codes.KeyValues)
                        {
                            code.Append(@"module ", typeCode.Key.Namespace, @" {
	export class ", typeCode.Key.Name, @" {");
                            code.Add(typeCode.Value);
                            code.Append(@"
	}
}
");
                        }
                        codes.Empty();
                        return code.ToString();
                    }
                    return null;
                }
            }
            /// <summary>
            /// WEB视图类型信息
            /// </summary>
            public ViewType View;
            /// <summary>
            /// 创建代码
            /// </summary>
            /// <param name="parameter"></param>
            /// <param name="type"></param>
            /// <param name="view"></param>
            /// <param name="loadMethod"></param>
            public void Create(ProjectParameter parameter, ExtensionType type, ViewType view, MethodIndex loadMethod)
            {
                AutoParameter = parameter;
                View = view;
                Type = type;
                LoadMethod = loadMethod;

                TypeKey typeKey;
                if (!types.TryGetValue(type.Type.Namespace, out typeKey)) types.Add(type.Type.Namespace, typeKey = new TypeKey(parameter, type));

                _code_.Length = 0;
                create(false);
                StringArray code;
                if (!codes.TryGetValue(ref typeKey, out code)) codes.Set(ref typeKey, code = new StringArray());
                code.Add(_code_);
            }
        }
    }
}
