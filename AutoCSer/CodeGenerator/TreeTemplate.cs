using System;
using System.Collections.Generic;
using AutoCSer.Metadata;
using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extension;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 树节点模板
    /// </summary>
    internal abstract class TreeTemplate
    {
        /// <summary>
        /// 成员信息缓存集合
        /// </summary>
        private static Dictionary<Type, Dictionary<HashString, AutoCSer.Metadata.MemberIndexInfo>> memberCache = DictionaryCreator.CreateOnly<Type, Dictionary<HashString, AutoCSer.Metadata.MemberIndexInfo>>();
        /// <summary>
        /// 成员树节点
        /// </summary>
        public sealed class MemberNode
        {
            /// <summary>
            /// 树节点模板
            /// </summary>
            private TreeTemplate template;
            /// <summary>
            /// 成员类型
            /// </summary>
            internal ExtensionType Type { get; private set; }
            /// <summary>
            /// 当前节点成员名称
            /// </summary>
            private SubString name;
            /// <summary>
            /// 父节点成员
            /// </summary>
            internal MemberNode Parent;
            /// <summary>
            /// 节点路径
            /// </summary>
            internal string Path;
            /// <summary>
            /// 节点路径全称
            /// </summary>
            internal string FullPath
            {
                get
                {
                    if (Parent != null)
                    {
                        StringArray path = new StringArray();
                        for (MemberNode member = this; member.Parent != null; member = member.Parent) path.Add(member.name);
                        path.Reverse();
                        return path.Join('.');
                    }
                    return null;
                }
            }
            /// <summary>
            /// 节点路径上是否有下级路径
            /// </summary>
            internal bool IsNextPath
            {
                get
                {
                    Dictionary<HashString, MemberNode> paths;
                    return template.memberPaths.TryGetValue(this, out paths) && paths.Count != 0;
                }
            }
            /// <summary>
            /// Ajax视图输出参数
            /// </summary>
            internal AutoCSer.WebView.OutputAjaxAttribute OutputAjax;
            /// <summary>
            /// 是否忽略null值输出
            /// </summary>
            internal bool IsIgnoreNull
            {
                get { return OutputAjax.IsIgnoreNull; }
            }
            /// <summary>
            /// 是否 await 函数
            /// </summary>
            internal bool IsAwaitMethod;
            /// <summary>
            /// 节点路径
            /// </summary>
            internal string AwaitPath
            {
                get
                {
                    return IsAwaitMethod ? "(" + Type.FullName + ")(await " + Path + "())" : Path;
                }
            }
            /// <summary>
            /// 成员名称+成员信息集合
            /// </summary>
            internal Dictionary<HashString, AutoCSer.Metadata.MemberIndexInfo> Members
            {
                get
                {
                    Dictionary<HashString, AutoCSer.Metadata.MemberIndexInfo> values;
                    Type type = Type.Type;
                    if (!memberCache.TryGetValue(type, out values))
                    {
                        try
                        {
                            memberCache[type] = values = MemberIndexGroup.Get(Type).Find(Path == "this" ? MemberFilters.Instance : MemberFilters.PublicInstance)
                                .getDictionary(value => new HashString(value.Member.Name));
                            foreach (MethodInfo method in type.GetMethods(Path == "this" ? BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic : (BindingFlags.Instance | BindingFlags.Public)))
                            {
                                Type returnType = method.ReturnType;
                                if (returnType.IsGenericType && !method.IsGenericMethod && (returnType = returnType.BaseType) != null
                                    && returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(AutoCSer.Net.TcpServer.Awaiter<,>) && method.GetParameters().Length == 0)
                                {
                                    HashString name = method.Name;
                                    if (!values.ContainsKey(name)) values.Add(name, new AutoCSer.CodeGenerator.Metadata.MethodIndex(method, returnType.GetGenericArguments()[0]));
                                }
                            }
                        }
                        catch (Exception error)
                        {
                            string output = string.Join(",", MemberIndexGroup.Get(Type).Find(Path == "this" ? MemberFilters.Instance : MemberFilters.PublicInstance)
                                .groupCount(value => value.Member.Name)
                                .getFind(value => value.Value != 1)
                                .GetArray(value => value.Key));
                            AutoCSer.Log.Pub.Log.WaitThrow(AutoCSer.Log.LogType.All, error, Type.FullName + " : " + output, true);
                        }
                    }
                    return values;
                }
            }
            /// <summary>
            /// 成员树节点
            /// </summary>
            /// <param name="template">树节点模板</param>
            /// <param name="type">成员类型</param>
            /// <param name="name">当前节点成员名称</param>
            /// <param name="path">当前节点成员名称</param>
            /// <param name="outputAjax">Ajax视图输出参数</param>
            /// <param name="isAwaitMethod">是否 await 函数</param>
            internal MemberNode(TreeTemplate template, ExtensionType type, ref SubString name, string path, AutoCSer.WebView.OutputAjaxAttribute outputAjax, bool isAwaitMethod)
            {
                this.template = template;
                this.Type = type;
                this.name = name;
                template.IsAwaitMethod |= (this.IsAwaitMethod = isAwaitMethod);
                Path = path;
                OutputAjax = outputAjax ?? AutoCSer.WebView.OutputAjaxAttribute.Null;
                if (OutputAjax.IsAllMember)
                {
                    foreach (MemberIndexInfo member in Members.Values)
                    {
                        if (!member.IsIgnore && ((outputAjax = member.GetAttribute<AutoCSer.WebView.OutputAjaxAttribute>(true)) == null || (outputAjax.IsSetup && outputAjax.BindingName == null)))
                        {
                            SubString memberName = member.Member.Name;
                            Get(ref memberName, false);
                        }
                    }
                }
                else
                {
                    foreach (MemberIndexInfo member in Members.Values)
                    {
                        //if (member.Member.customAttribute<AutoCSer.code.ignore>(true) == null)
                        if (!member.IsIgnore && (outputAjax = member.GetAttribute<AutoCSer.WebView.OutputAjaxAttribute>(true)) != null && outputAjax.IsSetup && outputAjax.BindingName == null)
                        {
                            SubString memberName = member.Member.Name;
                            Get(ref memberName, false);
                        }
                    }
                }
            }
            /// <summary>
            /// 根据成员名称获取子节点成员
            /// </summary>
            /// <param name="name">成员名称</param>
            /// <param name="isLast">是否最后层级</param>
            /// <returns>子节点成员</returns>
            internal MemberNode Get(ref SubString name, bool isLast)
            {
                Dictionary<HashString, MemberNode> paths;
                if (!template.memberPaths.TryGetValue(this, out paths))
                {
                    template.memberPaths[this] = paths = DictionaryCreator.CreateHashString<MemberNode>();
                }
                MemberNode value;
                if (isLast && template.isCollectionLength && name.Equals("length"))
                {
                    if (Type.Type.IsArray) name = "Length";
                    else if (typeof(ICollection).IsAssignableFrom(Type.Type)) name = "Count";
                }
                HashString hashName = name;
                if (paths.TryGetValue(hashName, out value))
                {
                    template.IsAwaitMethod |= value.IsAwaitMethod;
                    return value;
                }
                bool isPath = true;
                if (name.Length != 0)
                {
                    AutoCSer.Metadata.MemberIndexInfo member;
                    if (Members.TryGetValue(name, out member))
                    {
                        //if (member.Member.customAttribute<AutoCSer.code.ignore>(true) != null) isPath = false;
                        if (member.IsIgnore) isPath = false;
                        AutoCSer.WebView.OutputAjaxAttribute outputAjax = member.GetAttribute<AutoCSer.WebView.OutputAjaxAttribute>(true);
                        if (outputAjax != null)
                        {
                            if (outputAjax.BindingName != null)
                            {
                                SubString outputName = outputAjax.BindingName;
                                value = Get(ref outputName, false);
                            }
                            if (!outputAjax.IsSetup) isPath = false;
                        }
                        value = new MemberNode(template, member.TemplateMemberType, ref name, null, outputAjax, member.AwaiterReturnType != null);
                    }
                }
                else
                {
                    SubString nullName = default(SubString);
                    value = new MemberNode(template, Type.EnumerableArgumentType, ref nullName, null, null, false);
                }
                if (value != null)
                {
                    value.Parent = this;
                    //value.template = template;
                    if (isPath) paths[hashName] = value;
                }
                return value;
            }
            /// <summary>
            /// 根据成员名称获取子节点成员
            /// </summary>
            /// <param name="name">成员名称</param>
            /// <returns>子节点成员</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal MemberNode Get(string name)
            {
                SubString subName = new SubString { String = name, Start = 0, Length = name.Length };
                return Get(ref subName, false);
            }
        }
        /// <summary>
        /// 模板数据视图类型
        /// </summary>
        protected Type viewType;
        /// <summary>
        /// 当前代码字符串
        /// </summary>
        internal StringArray Code = new StringArray();
        /// <summary>
        /// 当前代码字符串常量
        /// </summary>
        protected StringArray pushCodes = new StringArray();
        /// <summary>
        /// 当前代码字符串
        /// </summary>
        internal string CodeString
        {
            get
            {
                pushCode();
                return Code.ToString();
            }
        }
        /// <summary>
        /// 子段程序代码集合
        /// </summary>
        internal Dictionary<string, string> PartCodes = DictionaryCreator.CreateOnly<string, string>();
        /// <summary>
        /// 成员树
        /// </summary>
        protected Dictionary<MemberNode, Dictionary<HashString, MemberNode>> memberPaths = DictionaryCreator.CreateOnly<MemberNode, Dictionary<HashString, MemberNode>>();
        /// <summary>
        /// 当前成员节点集合
        /// </summary>
        protected LeftArray<MemberNode> currentMembers;
        /// <summary>
        /// 错误处理委托
        /// </summary>
        protected Action<string> onError;
        /// <summary>
        /// 信息处理委托
        /// </summary>
        protected Action<string> onMessage;
        /// <summary>
        /// 忽略代码
        /// </summary>
        protected int ignoreCode;
        /// <summary>
        /// 忽略成员错误
        /// </summary>
        protected int ignoreMemberError;
        /// <summary>
        /// 集合是否支持length属性
        /// </summary>
        protected virtual bool isCollectionLength { get { return false; } }
        /// <summary>
        /// 临时逻辑变量名称
        /// </summary>
        protected string ifName = "_if_";
        /// <summary>
        /// 是否 await 函数
        /// </summary>
        internal bool IsAwaitMethod;
        /// <summary>
        /// 检测错误成员名称
        /// </summary>
        /// <param name="memberName">成员名称</param>
        /// <returns>是否忽略错误</returns>
        protected virtual bool checkErrorMemberName(ref SubString memberName)
        {
            return false;
        }
        /// <summary>
        /// 获取临时变量名称
        /// </summary>
        /// <param name="index">临时变量层次</param>
        /// <returns>变量名称</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected string path(int index)
        {
            return "_value" + (index == 0 ? (currentMembers.Length - 1) : index).ToString() + "_";
        }
        /// <summary>
        /// 获取循环索引临时变量名称
        /// </summary>
        /// <param name="index">临时变量层次</param>
        /// <returns>循环索引变量名称</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected string loopIndex(int index)
        {
            return "_loopIndex" + (index == 0 ? (currentMembers.Length - 1) : index).ToString() + "_";
        }
        /// <summary>
        /// 获取循环数量临时变量名称
        /// </summary>
        /// <param name="index">临时变量层次</param>
        /// <returns>循环数量变量名称</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected string loopCount(int index)
        {
            return "_loopCount" + (index == 0 ? (currentMembers.Length - 1) : index).ToString() + "_";
        }
        /// <summary>
        /// 添加代码
        /// </summary>
        /// <param name="code">代码</param>
        protected virtual void pushCode(SubString code)
        {
            if (ignoreCode == 0) pushCodes.Add(code);
        }
        /// <summary>
        /// 截断代码字符串
        /// </summary>
        protected virtual void pushCode()
        {
            if (ignoreCode == 0)
            {
                string code = pushCodes.ToString();
                if (code.Length != 0)
                {
                    Code.Append(@"
            _code_.Add(@""", code.Replace(@"""", @""""""), @""");");
                }
                pushCodes.Length = 0;
            }
        }
        /// <summary>
        /// 根据成员名称获取成员树节点
        /// </summary>
        /// <param name="memberName">成员名称</param>
        /// <param name="isDepth">是否深度搜索,false表示当前节点子节点</param>
        /// <returns>成员树节点</returns>
        protected MemberNode getMember(ref SubString memberName, out bool isDepth)
        {
            int memberIndex = 0;
            while (memberIndex != memberName.Length && memberName[memberIndex] == '.') ++memberIndex;
            memberName.MoveStart(memberIndex);
            memberIndex = currentMembers.Length - memberIndex - 1;
            if (memberIndex < 0) memberIndex = 0;
            MemberNode value = currentMembers[memberIndex];
            isDepth = false;
            if (memberName.Length != 0)
            {
                LeftArray<SubString> names = memberName.Split('.');
                SubString[] nameArray = names.Array;
                for (int lastIndex = names.Length - 1; memberIndex >= 0; --memberIndex)
                {
                    if ((value = currentMembers[memberIndex].Get(ref nameArray[0], lastIndex == 0)) != null)
                    {
                        if (memberIndex == 0)
                        {
                            //keyValue<memberIndex, string> propertyIndex;
                            //if (!propertyNames.TryGetValue(names[0], out propertyIndex)) propertyIndex.Value = names[0];
                            //value.Path = propertyIndex.Value;
                            value.Path = nameArray[0].ToString();
                        }
                        else value.Path = path(memberIndex) + "." + nameArray[0].ToString();
                        if (names.Length != 1) isDepth = true;
                        for (int nameIndex = 1; nameIndex != names.Length; ++nameIndex)
                        {
                            if ((value = value.Get(ref nameArray[nameIndex], nameIndex == lastIndex)) == null) break;
                            value.Path = value.Parent.AwaitPath + "." + nameArray[nameIndex].ToString();
                        }
                        if (value == null) break;
                        else return value;
                    }
                }
                string message = viewType.fullName() + " 未找到属性 " + currentMembers.UnsafeLast.FullPath + " . " + memberName.ToString() + " [" + memberName.String + @"]
" + new System.Diagnostics.StackTrace().ToString();
                if (checkErrorMemberName(ref memberName))
                {
                    if (ignoreMemberError == 0) onMessage(message);
                }
                else if (ignoreMemberError == 0) onError(message);
                return null;
            }
            return value;
        }
        /// <summary>
        /// 添加当前成员节点
        /// </summary>
        /// <param name="member">成员节点</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void pushMember(MemberNode member)
        {
            currentMembers.Add(member);
        }
        /// <summary>
        /// 输出绑定的数据
        /// </summary>
        /// <param name="member">成员节点</param>
        protected void at(MemberNode member)
        {
            pushCode();
            if (ignoreCode == 0)
            {
                if (member.Type.IsString)
                {
                    Code.Append(@"
            _code_.Add(", member.AwaitPath, ");");
                }
                else if (member.Type.IsBool && member.Type.IsStruct)
                {
                    Code.Append(@"
            _code_.Add(", member.AwaitPath, @" ? ""true"" : ""false"");");
                }
                else
                {
                    Code.Append(@"
            _code_.Add(", member.AwaitPath, ".ToString());");
                }
            }
        }
        /// <summary>
        /// if代码段处理
        /// </summary>
        /// <param name="member">成员节点</param>
        /// <param name="memberName">成员名称</param>
        /// <param name="isDepth">是否深度搜索</param>
        /// <param name="doMember">成员处理函数</param>
        protected void ifThen(MemberNode member, ref SubString memberName, bool isDepth, Action<MemberNode> doMember)
        {
            if (isDepth)
            {
                pushCode();
                LeftArray<SubString> names = splitMemberName(ref memberName);
                for (int index = 0; index != names.Length - 1; ++index) ifStart(ref names.Array[index], false);
                doMember(getMember(ref names.Array[names.Length - 1], out isDepth));
                pushCode();
                for (int index = 0; index != names.Length - 1; ++index) ifEnd(true);
            }
            else doMember(member);
        }
        /// <summary>
        /// if开始代码段
        /// </summary>
        /// <param name="memberName">成员名称</param>
        /// <param name="isSkip">是否跳跃层次</param>
        protected void ifStart(ref SubString memberName, bool isSkip)
        {
            bool isDepth;
            MemberNode member = getMember(ref memberName, out isDepth);
            pushMember(member);
            if (isSkip) pushMember(member);
            string name = path(0);
            if (ignoreCode == 0)
            {
                Code.Append(@"
                {
                    ", member.Type.FullName, " ", name, " = ", member.AwaitPath, ";");
            }
            ifStart(member.Type, name, null);
        }
        /// <summary>
        /// if开始代码段
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <param name="name">成员路径名称</param>
        /// <param name="ifName">if临时变量名称</param>
        internal void ifStart(ExtensionType type, string name, string ifName)
        {
            if (ignoreCode == 0)
            {
                if (type.IsStruct || type.Type.IsEnum)
                {
                    if (type.IsBool)
                    {
                        Code.Append(@"
                    if (", name, ")");
                    }
                    else if (type.IsAjaxToString)
                    {
                        Code.Append(@"
                    if (", name, " != 0)");
                    }
                }
                else
                {
                    Code.Append(@"
                    if (", name, " != null)");
                }
                Code.Append(@"
                    {");
                if (ifName != null)
                {
                    Code.Append(@"
                        ", ifName, " = true;");
                }
            }
        }
        /// <summary>
        /// if结束代码段
        /// </summary>
        /// <param name="isMember">是否删除成员节点</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void ifEnd(bool isMember)
        {
            if (isMember) --currentMembers.Length;
            if (ignoreCode == 0)
            {
                Code.Append(@"
                    }
                }");
            }
        }

        /// <summary>
        /// 分解成员名称
        /// </summary>
        /// <param name="memberName">成员名称</param>
        /// <returns>成员名称集合</returns>
        protected static LeftArray<SubString> splitMemberName(ref SubString memberName)
        {
            int memberIndex = 0;
            while (memberIndex != memberName.Length && memberName[memberIndex] == '.') ++memberIndex;
            LeftArray<SubString> names = memberName.GetSub(memberIndex).Split('.');
            names.Array[0].MoveStart(-memberIndex);
            return names;
        }
    }
    /// <summary>
    /// 树节点模板
    /// </summary>
    /// <typeparam name="nodeType">树节点类型</typeparam>
    internal abstract class TreeTemplate<nodeType> : TreeTemplate where nodeType : TreeTemplate<nodeType>.INode
    {
        /// <summary>
        /// 模板代码节点接口
        /// </summary>
        public interface INode
        {
            /// <summary>
            /// 模板命令
            /// </summary>
            string TemplateCommand { get; }
            /// <summary>
            /// 模板成员名称
            /// </summary>
            SubString TemplateMemberName { get; }
            /// <summary>
            /// 模板成员名称
            /// </summary>
            SubString TemplateMemberNameBeforeAt { get; }
            /// <summary>
            /// 模板文本代码
            /// </summary>
            SubString TemplateCode { get; }
            /// <summary>
            /// 子节点数量
            /// </summary>
            int ChildCount { get; }
            /// <summary>
            /// 子节点集合
            /// </summary>
            IEnumerable<nodeType> Childs { get; }
        }
        /// <summary>
        /// 模板command+解析器
        /// </summary>
        protected Dictionary<string, Action<nodeType>> creators = DictionaryCreator.CreateOnly<string, Action<nodeType>>();
        /// <summary>
        /// 引用代码树节点
        /// </summary>
        protected Dictionary<HashString, nodeType> nameNodes = DictionaryCreator.CreateHashString<nodeType>();
        /// <summary>
        /// 树节点模板
        /// </summary>
        /// <param name="type">模板数据视图</param>
        /// <param name="onError">错误处理委托</param>
        /// <param name="onMessage">消息处理委托</param>
        protected TreeTemplate(Type type, Action<string> onError, Action<string> onMessage)
        {
            this.onError = onError;
            this.onMessage = onMessage;
            SubString name = default(SubString);
            currentMembers.Add(new MemberNode(this, viewType = type ?? GetType(), ref name, "this", null, false));
        }
        /// <summary>
        /// 检测成员名称
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="isClient"></param>
        protected virtual void checkMemberName(ref SubString memberName, ref bool isClient) { }
        /// <summary>
        /// 添加代码树节点
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void skin(nodeType node)
        {
            Action<nodeType> creator;
            foreach (nodeType son in node.Childs)
            {
                string command = son.TemplateCommand;
                if (command == null) pushCode(son.TemplateCode);
                else if (creators.TryGetValue(command, out creator)) creator(son);
                else onError(viewType.fullName() + " 未找到命名处理函数 " + command + " : " + son.TemplateMemberName.ToString());
            }
        }
        /// <summary>
        /// 注释处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void note(nodeType node)
        {
        }
        /// <summary>
        /// 输出绑定的数据
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected virtual void at(nodeType node)
        {
            bool isDepth;
            SubString memberName = node.TemplateMemberName;
            MemberNode member = getMember(ref memberName, out isDepth);
            if (member != null) ifThen(member, ref memberName, isDepth, value => at(value));
        }
        /// <summary>
        /// 子代码段处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void push(nodeType node)
        {
            bool isDepth = false, isClient = false;
            MemberNode member = null;
            SubString memberName = node.TemplateMemberNameBeforeAt;
            checkMemberName(ref memberName, ref isClient);
            if (isClient) ++ignoreMemberError;
            if ((member = getMember(ref memberName, out isDepth)) != null && node.ChildCount != 0)
            {
                if (isClient) ++ignoreCode;
                pushCode();
                string name = path(currentMembers.Length);
                if (ignoreCode == 0)
                {
                    Code.Append(@"
                {
                    ", member.Type.FullName, " ", name, " = default(", member.Type.FullName, ");");
                }
                if (isDepth)
                {
                    LeftArray<SubString> names = splitMemberName(ref memberName);
                    ifStart(ref names.Array[0], true);
                    for (int index = 1; index != names.Length - 1; ++index) ifStart(ref names.Array[index], false);
                    push(getMember(ref names.Array[names.Length - 1], out isDepth), node, name, names.Length - 1);
                }
                else push(member, node, name, 0);
                if (ignoreCode == 0)
                {
                    Code.Append(@"
                }");
                }
                if (isClient) --ignoreCode;
            }
            if (isClient) --ignoreMemberError;
        }
        /// <summary>
        /// 子代码段处理
        /// </summary>
        /// <param name="member">成员节点</param>
        /// <param name="node">代码树节点</param>
        /// <param name="name">成员路径名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        protected void push(MemberNode member, nodeType node, string name, int popCount)
        {
            if (ignoreCode == 0)
            {
                Code.Append(@"
                    ", name, " = ", member.AwaitPath, ";");
            }
            if (popCount != 0) --currentMembers.Length;
            while (popCount != 0)
            {
                ifEnd(true);
                --popCount;
            }
            pushMember(member);
            if (ignoreCode == 0)
            {
                Code.Append(@"
            ", ifName, " = false;");
            }
            ifThen(node, member.Type, name, ifName, false, 0, false);
            --currentMembers.Length;
        }
        /// <summary>
        /// if代码段处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        /// <param name="type">成员类型</param>
        /// <param name="name">成员路径名称</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="isMember">是否删除当前成员节点</param>
        /// <param name="popCount">删除成员节点数量</param>
        /// <param name="isNot"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ifThen(nodeType node, ExtensionType type, string name, string ifName, bool isMember, int popCount, bool isNot)
        {
            ifOr(type, name, ifName, isMember, popCount);
            ifEnd(node, isNot);
        }
        /// <summary>
        /// if代码段处理
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <param name="name">成员路径名称</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="isMember">是否删除当前成员节点</param>
        /// <param name="popCount">删除成员节点数量</param>
        private void ifOr(ExtensionType type, string name, string ifName, bool isMember, int popCount)
        {
            ifStart(type, name, ifName);
            while (popCount != 0)
            {
                ifEnd(true);
                --popCount;
            }
            if (isMember) --currentMembers.Length;
            if (ignoreCode == 0)
            {
                Code.Append(@"
                }");
            }
        }
        /// <summary>
        /// if条件判断结束
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isNot"></param>
        private void ifEnd(nodeType node, bool isNot)
        {
            if (ignoreCode == 0)
            {
                Code.Append(@"
            if (", isNot ? "!" : null, ifName, @")
            {");
            }
            skin(node);
            pushCode();
            if (ignoreCode == 0)
            {
                Code.Append(@"
            }");
            }
        }
        /// <summary>
        /// 绑定的数据为true非0非null时输出代码
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void ifThen(nodeType node)
        {
            SubString memberName = node.TemplateMemberNameBeforeAt;
            if (memberName.IndexOf('|') == -1)
            {
                if (memberName.IndexOf('&') == -1)
                {
                    string value = null;
                    int valueIndex = memberName.IndexOf('=');
                    if (valueIndex != -1)
                    {
                        value = memberName.GetSub(valueIndex + 1);
                        memberName.Sub(0, valueIndex);
                    }
                    MemberNode member = null;
                    bool isDepth = false, isClient = false, isNot = false;
                    if (memberName.Length != 0 && memberName[0] == '!')
                    {
                        isNot = true;
                        memberName.MoveStart(1);
                    }
                    checkMemberName(ref memberName, ref isClient);
                    //if (isClient) ++ignoreMemberError;
                    if ((member = getMember(ref memberName, out isDepth)) == null)
                    {
                        //if (isClient) --ignoreMemberError;
                        return;
                    }
                    if (isClient) ++ignoreCode;
                    pushCode();
                    if (ignoreCode == 0)
                    {
                        Code.Append(@"
            ", ifName, " = false;");
                    }
                    if (isDepth)
                    {
                        LeftArray<SubString> names = splitMemberName(ref memberName);
                        for (int index = 0; index != names.Length - 1; ++index) ifStart(ref names.Array[index], false);
                        ifThen(getMember(ref names.Array[names.Length - 1], out isDepth), node, value, ifName, names.Length - 1, isNot);
                    }
                    else ifThen(member, node, value, ifName, 0, isNot);
                    if (isClient)
                    {
                        --ignoreCode;
                        //--ignoreMemberError;
                    }
                }
                else ifThen(node, ref memberName, true);
            }
            else ifThen(node, ref memberName, false);
        }
        /// <summary>
        /// if代码段处理
        /// </summary>
        /// <param name="member">成员节点</param>
        /// <param name="node">代码树节点</param>
        /// <param name="value">匹配值</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        /// <param name="isNot"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void ifThen(MemberNode member, nodeType node, string value, string ifName, int popCount, bool isNot)
        {
            if (value == null) ifThen(node, member.Type, member.AwaitPath, ifName, false, popCount, isNot);
            else ifThen(node, member.Type, member.AwaitPath, value, ifName, popCount, isNot);
        }
        /// <summary>
        /// if代码段处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        /// <param name="type">成员类型</param>
        /// <param name="name">成员路径名称</param>
        /// <param name="value">匹配值</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        /// <param name="isNot"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ifThen(nodeType node, ExtensionType type, string name, string value, string ifName, int popCount, bool isNot)
        {
            ifOr(type, name, value, ifName, popCount);
            ifEnd(node, isNot);
        }
        /// <summary>
        /// if代码段处理
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <param name="name">成员路径名称</param>
        /// <param name="value">匹配值</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        private void ifOr(ExtensionType type, string name, string value, string ifName, int popCount)
        {
            if (ignoreCode == 0)
            {
                if (type.IsStruct || type.Type.IsEnum)
                {
                    Code.Append(@"
                if (", name, @".ToString() == @""", value.Replace(@"""", @""""""), @""")");
                }
                else
                {
                    Code.Append(@"
                if (", name, @" != null && ", name, @".ToString() == @""", value.Replace(@"""", @""""""), @""")");
                }
                Code.Append(@"
                {
                    ", ifName, @" = true;
                }");
            }
            while (popCount != 0)
            {
                ifEnd(true);
                --popCount;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="memberName"></param>
        /// <param name="isAnd"></param>
        private void ifThen(nodeType node, ref SubString memberName, bool isAnd)
        {
            pushCode();
            if (ignoreCode == 0)
            {
                Code.Append(@"
                ", ifName, " = false;");
            }
            byte isNext = 0;
            bool isNot = false;
            foreach (SubString subMemberName in memberName.Split(isAnd ? '&' : '|'))
            {
                if (isNext == 0)
                {
                    isNot = ifOr(subMemberName);
                    isNext = 1;
                }
                else
                {
                    if (ignoreCode == 0)
                    {
                        if (isAnd ^ isNot)
                        {
                            Code.Append(@"
            if (", ifName, @")
            {
                ", ifName, " = false;");
                        }
                        else
                        {
                            Code.Append(@"
            if (!", ifName, @")
            {");
                        }
                    }
                    isNot = ifOr(subMemberName);
                    if (ignoreCode == 0)
                    {
                        Code.Append(@"
            }");
                    }
                }
            }
            ifEnd(node, isNot);
        }
        /// <summary>
        /// if多条件OR
        /// </summary>
        /// <param name="subMemberName"></param>
        /// <returns>是否需要取反</returns>
        private bool ifOr(SubString subMemberName)
        {
            string value = null;
            SubString memberName;
            bool isDepth = false, isClient = false, isNot = false;
            if (subMemberName.Length != 0 && subMemberName[0] == '!')
            {
                subMemberName.MoveStart(1);
                isNot = true;
            }
            int valueIndex = subMemberName.IndexOf('=');
            if (valueIndex != -1)
            {
                value = subMemberName.GetSub(valueIndex + 1);
                memberName = subMemberName.GetSub(0, valueIndex);
            }
            else memberName = subMemberName;
            MemberNode member = null;
            checkMemberName(ref memberName, ref isClient);
            if (isClient) ++ignoreMemberError;
            if ((member = getMember(ref memberName, out isDepth)) == null)
            {
                if (isClient) --ignoreMemberError;
                return isNot;
            }
            if (isClient) ++ignoreCode;
            if (isDepth)
            {
                LeftArray<SubString> names = splitMemberName(ref memberName);
                for (int index = 0; index != names.Length - 1; ++index) ifStart(ref names.Array[index], false);
                ifOr(getMember(ref names.Array[names.Length - 1], out isDepth), value, ifName, names.Length - 1);
            }
            else ifOr(member, value, ifName, 0);
            if (isClient)
            {
                --ignoreCode;
                --ignoreMemberError;
            }
            return isNot;
        }
        /// <summary>
        /// if代码段处理
        /// </summary>
        /// <param name="member">成员节点</param>
        /// <param name="value">匹配值</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void ifOr(MemberNode member, string value, string ifName, int popCount)
        {
            if (value == null) ifOr(member.Type, member.AwaitPath, ifName, false, popCount);
            else ifOr(member.Type, member.AwaitPath, value, ifName, popCount);
        }
        /// <summary>
        /// 绑定的数据为false或者0或者null时输出代码
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void not(nodeType node)
        {
            SubString memberName = node.TemplateMemberNameBeforeAt;
            if (memberName.IndexOf('|') == -1)
            {
                if (memberName.IndexOf('&') == -1)
                {
                    string value = null;
                    int valueIndex = memberName.IndexOf('=');
                    if (valueIndex != -1)
                    {
                        value = memberName.GetSub(valueIndex + 1);
                        memberName.Sub(0, valueIndex);
                    }
                    MemberNode member = null;
                    bool isDepth = false, isClient = false, isNot = false;
                    if (memberName.Length != 0 && memberName[0] == '!')
                    {
                        isNot = true;
                        memberName.MoveStart(1);
                    }
                    checkMemberName(ref memberName, ref isClient);
                    //if (isClient) ++ignoreMemberError;
                    if ((member = getMember(ref memberName, out isDepth)) == null)
                    {
                        //if (isClient) --ignoreMemberError;
                        return;
                    }
                    if (isClient) ++ignoreCode;
                    pushCode();
                    if (ignoreCode == 0)
                    {
                        Code.Append(@"
            ", ifName, " = false;");

                    }
                    if (isDepth)
                    {
                        LeftArray<SubString> names = splitMemberName(ref memberName);
                        for (int index = 0; index != names.Length - 1; ++index) ifStart(ref names.Array[index], false);
                        not(getMember(ref names.Array[names.Length - 1], out isDepth), node, value, ifName, names.Length - 1, isNot);
                    }
                    else not(member, node, value, ifName, 0, isNot);
                    if (isClient)
                    {
                        --ignoreCode;
                        //--ignoreMemberError;
                    }
                }
                else not(node, ref memberName, true);
            }
            else not(node, ref memberName, false);
        }
        /// <summary>
        /// not代码段处理
        /// </summary>
        /// <param name="member">成员节点</param>
        /// <param name="node">代码树节点</param>
        /// <param name="value">匹配值</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        /// <param name="isNot"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void not(MemberNode member, nodeType node, string value, string ifName, int popCount, bool isNot)
        {
            notOr(member, value, ifName, popCount);
            ifEnd(node, isNot);
        }
        /// <summary>
        /// not代码段处理
        /// </summary>
        /// <param name="member">成员节点</param>
        /// <param name="value">匹配值</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        private void notOr(MemberNode member, string value, string ifName, int popCount)
        {
            if (ignoreCode == 0)
            {
                if (member.Type.IsStruct || member.Type.Type.IsEnum)
                {
                    if (value != null)
                    {
                        Code.Append(@"
                if (", member.AwaitPath, @".ToString() != @""", value.Replace(@"""", @""""""), @""")");
                    }
                    else if (member.Type.IsBool)
                    {
                        Code.Append(@"
                if (!(bool)", member.AwaitPath, ")");
                    }
                    else if (member.Type.IsAjaxToString)
                    {
                        Code.Append(@"
                if (", member.AwaitPath, " == 0)");
                    }
                }
                else if (value != null)
                {
                    string memberName = path(0);
                    Code.Append(@"
                ", member.Type.FullName, " ", memberName, " = ", member.AwaitPath, @";
                if (", memberName, @" == null || ", memberName, @".ToString() != @""", value.Replace(@"""", @""""""), @""")");
                }
                else
                {
                    Code.Append(@"
                if (", member.AwaitPath, " == null)");
                }
                Code.Append(@"
                {
                    ", ifName, @" = true;
                }");
            }
            while (popCount != 0)
            {
                ifEnd(true);
                --popCount;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="memberName"></param>
        /// <param name="isAnd"></param>
        private void not(nodeType node, ref SubString memberName, bool isAnd)
        {
            pushCode();
            if (ignoreCode == 0)
            {
                Code.Append(@"
                ", ifName, " = false;");
            }
            byte isNext = 0;
            bool isNot = false;
            foreach (SubString subMemberName in memberName.Split(isAnd ? '&' : '|'))
            {
                if (isNext == 0)
                {
                    isNot = notOr(subMemberName);
                    isNext = 1;
                }
                else
                {
                    if (ignoreCode == 0)
                    {
                        if (isAnd ^ isNot)
                        {
                            Code.Append(@"
            if (", ifName, @")
            {
                ", ifName, " = false;");
                        }
                        else
                        {
                            Code.Append(@"
            if (!", ifName, @")
            {");
                        }
                    }
                    isNot = notOr(subMemberName);
                    if (ignoreCode == 0)
                    {
                        Code.Append(@"
            }");
                    }
                }
            }
            ifEnd(node, isNot);
        }
        /// <summary>
        /// not多条件OR
        /// </summary>
        /// <param name="subMemberName"></param>
        private bool notOr(SubString subMemberName)
        {
            string value = null;
            SubString memberName;
            bool isDepth = false, isClient = false, isNot = false;
            if (subMemberName.Length != 0 && subMemberName[0] == '!')
            {
                subMemberName.MoveStart(1);
                isNot = true;
            }
            int valueIndex = subMemberName.IndexOf('=');
            if (valueIndex != -1)
            {
                value = subMemberName.GetSub(valueIndex + 1);
                memberName = subMemberName.GetSub(0, valueIndex);
            }
            else memberName = subMemberName;
            MemberNode member = null;
            checkMemberName(ref memberName, ref isClient);
            if (isClient) ++ignoreMemberError;
            if ((member = getMember(ref memberName, out isDepth)) == null)
            {
                if (isClient) --ignoreMemberError;
                return isNot;
            }
            if (isClient) ++ignoreCode;
            if (isDepth)
            {
                LeftArray<SubString> names = splitMemberName(ref memberName);
                for (int index = 0; index != names.Length - 1; ++index) ifStart(ref names.Array[index], false);
                notOr(getMember(ref names.Array[names.Length - 1], out isDepth), value, ifName, names.Length - 1);
            }
            else notOr(member, value, ifName, 0);
            if (isClient)
            {
                --ignoreCode;
                --ignoreMemberError;
            }
            return isNot;
        }
        /// <summary>
        /// 循环处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void loop(nodeType node)
        {
            bool isDepth = false, isClient = false;
            MemberNode member = null;
            SubString memberName = node.TemplateMemberNameBeforeAt;
            checkMemberName(ref memberName, ref isClient);
            if (isClient) ++ignoreMemberError;
            member = getMember(ref memberName, out isDepth);
            if (member == null)
            {
                if (isClient) --ignoreMemberError;
                return;
            }
            pushCode();
            string name = path(currentMembers.Length);
            int codeCount = Code.Length;
            bool isLoop;
            if (isDepth)
            {
                if (ignoreCode == 0)
                {
                    Code.Append(@"
                {
                    ", member.Type.FullName, " ", name, " = default(", member.Type.FullName, ");");
                }
                LeftArray<SubString> names = splitMemberName(ref memberName);
                ifStart(ref names.Array[0], true);
                for (int index = 1; index != names.Length - 1; ++index) ifStart(ref names.Array[index], false);
                isLoop = loop(getMember(ref names.Array[names.Length - 1], out isDepth), node, name, names.Length - 1);
            }
            else
            {
                if (ignoreCode == 0)
                {
                    Code.Append(@"
                {
                    ", member.Type.FullName, " ", name, ";");
                }
                isLoop = loop(member, node, name, 0);
            }
            if (ignoreCode == 0)
            {
                if (isLoop)
                {
                    Code.Append(@"
                }");
                }
                else
                {
                    Code.Length = codeCount;
                    pushCodes.Length = 0;
                }
            }
            if (isClient) --ignoreMemberError;
        }
        /// <summary>
        /// 循环处理
        /// </summary>
        /// <param name="member">成员节点</param>
        /// <param name="node">代码树节点</param>
        /// <param name="name">成员路径名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        /// <returns>是否正常生成代码</returns>
        protected bool loop(MemberNode member, nodeType node, string name, int popCount)
        {
            ExtensionType enumerableType = member.Type.EnumerableType;
            if (enumerableType != null && ignoreCode == 0)
            {
                Code.Append(@"
                    ", name, " = ", member.AwaitPath, ";");
            }
            if (popCount != 0) --currentMembers.Length;
            while (popCount != 0)
            {
                ifEnd(true);
                --popCount;
            }
            if (enumerableType == null)
            {
                if (ignoreMemberError == 0) onError(viewType.fullName() + " 属性不可枚举 " + currentMembers.UnsafeLast.FullPath);
                return false;
            }
            pushMember(member);
            string valueName = path(currentMembers.Length);
            if (ignoreCode == 0)
            {
                if (!member.Type.Type.IsValueType)
                {
                    Code.Append(@"
                    if (", name, @" != null)");
                }
                Code.Append(@"
                    {
                        int ", loopIndex(0), @" = _loopIndex_, ", loopCount(0), @" = _loopCount_;");
//                if (isLoopValue)
//                {
//                    Code.Append(@"
//                        var ", loopValues(0), @" = _loopValues_, ", loopValue(0), @" = _loopValue_;
//                        _loopValues_ = ", name, ";");
//                }
                Code.Append(@"
                        _loopIndex_ = 0;
                        _loopCount_ = ", member.Type.Type.IsArray ? null : "_getCount_(", name, member.Type.Type.IsArray ? ".Length" : ")", @";
                        foreach (", member.Type.EnumerableArgumentType.FullName, " " + valueName + " in ", name, @")
                        {");
//                if (isLoopValue)
//                {
//                    Code.Append(@"
//                            _loopValue_ = ", valueName, ";");
//                }
            }
            MemberNode loopMember = member.Get(string.Empty);
            loopMember.Path = valueName;
            pushMember(loopMember);
            skin(node);
            --currentMembers.Length;
            pushCode();
            if (ignoreCode == 0)
            {
                Code.Append(@"
                            ++_loopIndex_;
                        }
                        _loopIndex_ = ", loopIndex(0), @";
                        _loopCount_ = ", loopCount(0), @";");
//                if (isLoopValue)
//                {
//                    Code.Append(@"
//                        _loopValue_ = ", loopValue(0), @";
//                        _loopValues_ = ", loopValues(0), ";");
//                }
                Code.Append(@"
                    }");
            }
            --currentMembers.Length;
            return true;
        }
        /// <summary>
        /// 子段模板处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void name(nodeType node)
        {
            HashString nameKey = node.TemplateMemberName;
            if (nameNodes.ContainsKey(nameKey)) onError(viewType.fullName() + " NAME " + nameKey.ToString() + " 重复定义");
            nameNodes[nameKey] = node;
            if (node.ChildCount != 0) skin(node);
        }
        /// <summary>
        /// 引用子段模板处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void fromName(nodeType node)
        {
            SubString memberName = node.TemplateMemberName;
            int typeIndex = memberName.IndexOf('.');
            if (typeIndex == -1)
            {
                if (!nameNodes.TryGetValue(memberName, out node)) onError(viewType.fullName() + " NAME " + memberName.ToString() + " 未定义");
            }
            else
            {
                SubString name = memberName.GetSub(++typeIndex);
                node = fromNameNode(memberName.GetSub(0, typeIndex), ref name);
            }
            if (node != null && node.ChildCount != 0) skin(node);
        }
        /// <summary>
        /// 根据类型名称获取子段模板
        /// </summary>
        /// <param name="fileName">模板文件名称</param>
        /// <param name="name">子段模板名称</param>
        /// <returns>子段模板</returns>
        protected virtual nodeType fromNameNode(string fileName, ref SubString name)
        {
            return default(nodeType);
        }
        /// <summary>
        /// 子段程序代码处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void part(nodeType node)
        {
            string memberName = node.TemplateMemberName;
            pushCode();
            Code.Add(@"
            StringArray _PART_" + memberName + @"_ = _code_;
            _code_ = new StringArray();");
            StringArray historyCode = Code;
            Code = new StringArray();
            skin(node);
            pushCode();
            string partCode = Code.ToString();
            PartCodes[memberName] = partCode;
            Code = historyCode;
            Code.Add(partCode);
            Code.Add(@"
            _partCodes_[""" + memberName + @"""] = _code_.ToString();
            _code_ = _PART_" + memberName + @"_;
            _code_.Add(_partCodes_[""" + memberName + @"""]);");
        }
    }
}
