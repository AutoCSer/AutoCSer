using System;
using AutoCSer.WebView;

namespace AutoCSer.CodeGenerator.Template
{
    class WebView : Pub
    {
        #region PART CLASS
        #region NOT IsServer
        /*NOTE*/
        public abstract partial class /*NOTE*/@TypeNameDefinition/*NOTE*/ : AutoCSer.WebView.View<TypeNameDefinition>/*, *//*NOTE*/
        {
            #region NOTE
            public const int HtmlCount = 0;
            #endregion NOTE
            #region IF Attribute.IsPage
            #region NOT IsPoolType
            private static byte[][] @Htmls;
            private static readonly object @HtmlLock = new object();
            #endregion NOT IsPoolType
            #region IF IsAwaitMethod
            protected override /*AT:Async*/ void pageAsync(AutoCSer.WebView.Response _html_)
            {
                try
                {
                    byte[][] htmls;
                    #region NOT IsPoolType
                    htmls = loadHtml(@"@HtmlFile", @HtmlCount, @HtmlLock, ref @Htmls);
                    #endregion NOT IsPoolType
                    #region IF IsPoolType
                    htmls = loadHtml(@"@HtmlFile", @HtmlCount);
                    #endregion IF IsPoolType
                    if (htmls != null)
                    {
                        /*AT:PageCode*/
                        asyncReturn = true;
                    }
                }
                catch (Exception error)
                {
                    addLog(error);
                }
                finally { asyncEnd(); }
            }
            #endregion IF IsAwaitMethod
            #region NOT IsAwaitMethod
            protected override bool page(ref AutoCSer.WebView.Response _html_)
            {
                byte[][] htmls;
                #region NOT IsPoolType
                htmls = loadHtml(@"@HtmlFile", @HtmlCount, @HtmlLock, ref @Htmls);
                #endregion NOT IsPoolType
                #region IF IsPoolType
                htmls = loadHtml(@"@HtmlFile", @HtmlCount);
                #endregion IF IsPoolType
                if (htmls != null)
                {
                    /*AT:PageCode*/
                    return true;
                }
                return false;
            }
            #endregion NOT IsAwaitMethod
            #endregion IF Attribute.IsPage

            #region IF Attribute.IsAjax
            #region IF IsAwaitMethod
            protected override /*AT:Async*/ void ajaxAsync(CharStream _js_)
            {
                try
                {
                    /*AT:AjaxCode*/
                }
                catch (Exception error)
                {
                    addLog(error);
                }
                finally { asyncEnd(); }
            }
            #endregion IF IsAwaitMethod
            #region NOT IsAwaitMethod
            protected override void ajax(CharStream _js_)
            {
                /*AT:AjaxCode*/
            }
            #endregion NOT IsAwaitMethod
            #endregion IF Attribute.IsAjax

            #region IF IsQuery
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct WebViewQuery
            {
                #region IF LoadMethod.Parameters.Length
                [AutoCSer.Json.ParseMember(IsDefault = true)]
                #endregion IF LoadMethod.Parameters.Length
                #region LOOP LoadMethod.Parameters
                #region IF XmlDocument
                /// <summary>
                /// @XmlDocument
                /// </summary>
                #endregion IF XmlDocument
                public @ParameterType.FullName @ParameterName;
                #endregion LOOP LoadMethod.Parameters
                #region LOOP QueryMembers
                #region IF XmlDocument
                /// <summary>
                /// @XmlDocument
                /// </summary>
                #endregion IF XmlDocument
                public @MemberType.FullName @MemberName;
                #endregion LOOP QueryMembers
                #region NOTE
                public object ParameterJoinName;
                #endregion NOTE
            }
            #region IF LoadAttribute.QueryName
            /// <summary>
            /// 查询参数
            /// </summary>
            private WebViewQuery /*PUSH:LoadAttribute*/@QueryName/*PUSH:LoadAttribute*/;
            #endregion IF LoadAttribute.QueryName
            /// <summary>
            /// WEB视图加载
            /// </summary>
            /// <returns>是否成功</returns>
            protected override bool loadView()
            {
                if (base.loadView())
                {
                    #region NOTE
                    MemberType.FullName MemberName;
                    #endregion NOTE
                    #region IF LoadAttribute.QueryName
                    /*PUSH:LoadAttribute*/
                    @QueryName/*PUSH:LoadAttribute*/= default(WebViewQuery);
                    if (ParseParameter(ref /*PUSH:LoadAttribute*/@QueryName/*PUSH:LoadAttribute*/))
                    {
                        #region LOOP QueryMembers
                        @MemberName = /*PUSH:LoadAttribute*/@QueryName/*PUSH:LoadAttribute*/.@MemberName;
                        #endregion LOOP QueryMembers
                        #region IF LoadMethod
                        return loadView(/*LOOP:LoadMethod.Parameters*//*PUSH:LoadAttribute*/@QueryName/*PUSH:LoadAttribute*/.@ParameterJoinName/*LOOP:LoadMethod.Parameters*/);
                        #endregion IF LoadMethod
                        #region NOT LoadMethod
                        return true;
                        #endregion NOT LoadMethod
                    }
                    #endregion IF LoadAttribute.QueryName
                    #region NOT LoadAttribute.QueryName
                    WebViewQuery query = new WebViewQuery();
                    if (ParseParameter(ref query))
                    {
                        #region LOOP QueryMembers
                        @MemberName = query.@MemberName;
                        #endregion LOOP QueryMembers
                        #region IF LoadMethod
                        return loadView(/*LOOP:LoadMethod.Parameters*/query.@ParameterJoinName/*LOOP:LoadMethod.Parameters*/);
                        #endregion IF LoadMethod
                        #region NOT LoadMethod
                        return true;
                        #endregion NOT LoadMethod
                    }
                    #endregion NOT LoadAttribute.QueryName
                }
                return false;
            }
            #region NOTE
            bool loadView(object _) { return false; }
            #endregion NOTE
            #endregion IF IsQuery
        }
        #endregion NOT IsServer

        #region IF IsServer
        /// <summary>
        /// WEB服务器
        /// </summary>
        public partial class WebServer : AutoCSer.Net.HttpDomainServer.ViewServer<@SessionType.FullName>
        {
            #region NOTE
            static readonly FullName[] Views = null;
            const int ViewPageCount = 0;
            const int PageIndex = 0;
            const int RewriteViewCount = 0;
            //const bool IsPool = false;
            const bool IsAsynchronous = false;
            const bool IsAwaitMethod = false;
            #endregion NOTE

            protected override KeyValue<string[], string[]> rewrites
            {
                get
                {
                    int count = @Views.Length + @RewriteViewCount * 2;
                    string[] names = new string[count];
                    string[] views = new string[count];
                    #region LOOP Views
                    names[--count] = "@RewritePath";
                    views[count] = "@CallName";
                    #region IF Attribute.RewritePath
                    names[--count] = @"@Attribute.RewritePath/*IF:Attribute.IsRewriteHtml*/.html/*IF:Attribute.IsRewriteHtml*/";
                    views[count] = "@CallName";
                    names[--count] = @"@Attribute.RewritePath/*NOTE*//*NOTE*/.js";
                    views[count] = "@RewriteJs";
                    #endregion IF Attribute.RewritePath
                    #endregion LOOP Views
                    return new KeyValue<string[], string[]>(names, views);
                }
            }
            #region IF ViewPageCount
            protected override string[] viewRewrites
            {
                get
                {
                    string[] names = new string[@ViewPageCount];
                    #region LOOP Views
                    #region IF Attribute.IsPage
                    names[@PageIndex] = "@RewritePath";
                    #endregion IF Attribute.IsPage
                    #endregion LOOP Views
                    return names;
                }
            }
            protected override string[] views
            {
                get
                {
                    string[] names = new string[@ViewPageCount];
                    #region LOOP Views
                    #region IF Attribute.IsPage
                    names[@PageIndex] = "@CallName";
                    #endregion IF Attribute.IsPage
                    #endregion LOOP Views
                    return names;
                }
            }
            protected override void request(int viewIndex, AutoCSer.Net.Http.SocketBase socket)
            {
                switch (viewIndex)
                {
                    #region LOOP Views
                    #region IF Attribute.IsPage
                    case @PageIndex:
                        #region IF IsPoolType
                        #region IF IsSetPage
                        @WebViewMethodType.FullName @PageName = @WebViewMethodType.FullName/**/.Pop();
                        if (@PageName == null) setPage(@PageName = new @WebViewMethodType.FullName()/*PUSH:Attribute*/, @IsAsynchronous/*PUSH:Attribute*/, @IsAwaitMethod);
                        loadPage(socket, @PageName);
                        #endregion IF IsSetPage
                        #region NOT IsSetPage
                        loadPage(socket, /*IF:IsPoolType*/@WebViewMethodType.FullName/**/.Pop() ?? /*IF:IsPoolType*/new @WebViewMethodType.FullName());
                        #endregion NOT IsSetPage
                        #endregion IF IsPoolType
                        #region NOT IsPoolType
                        loadPage(socket, /*NOTE*/(AutoCSer.WebView.View)(object)/*NOTE*/new @WebViewMethodType.FullName()/*IF:IsSetPage*//*PUSH:Attribute*/, @IsAsynchronous/*PUSH:Attribute*/, @IsAwaitMethod/*IF:IsSetPage*/);
                        #endregion NOT IsPoolType
                        return;
                    #endregion IF Attribute.IsPage
                    #endregion LOOP Views
                }
            }
            #endregion IF ViewPageCount
            /// <summary>
            /// 网站生成配置
            /// </summary>
            #region PUSH AutoParameter
            internal new static readonly @WebConfigType.FullName WebConfig = new @WebConfigType.FullName();
            #endregion PUSH AutoParameter
            /// <summary>
            /// 网站生成配置
            /// </summary>
            /// <returns>网站生成配置</returns>
            protected override AutoCSer.WebView.Config getWebConfig() { return WebConfig; }
            static WebServer()
            {
                CompileQueryParse(new System.Type[] { /*LOOP:Views*//*IF:LoadMethod*//*IF:Attribute.IsCompileJsonParser*/typeof(@Type.FullName/**/.WebViewQuery), /*IF:Attribute.IsCompileJsonParser*//*IF:LoadMethod*//*LOOP:Views*/null }, new System.Type[] { /*LOOP:CallMethods*/typeof(@ParameterTypeName), /*LOOP:CallMethods*/null });
            }
            #region NOTE
            class ParameterTypeName { }
            #endregion NOTE
        }
        #endregion IF IsServer
        #endregion PART CLASS
    }
    #region NOTE
    /// <summary>
    /// CSharp模板公用模糊类型
    /// </summary>
    internal partial class Pub
    {
        /// <summary>
        /// Session类型
        /// </summary>
        public class SessionType : Pub { }
        /// <summary>
        /// 获取该函数的类型
        /// </summary>
        public class WebViewMethodType
        {
            /// <summary>
            /// 类型全名
            /// </summary>
            public partial class FullName : AutoCSer.WebView.View<FullName>
            {
                /// <summary>
                /// 
                /// </summary>
                internal override void CancelGetForm()
                {
                }
            }
        }
        /// <summary>
        /// 网站生成配置
        /// </summary>
        public class WebConfigType
        {
            /// <summary>
            /// 类型全名
            /// </summary>
            public partial class FullName : AutoCSer.WebView.Config
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public partial class FullName
        {
            /// <summary>
            /// 查询参数类型
            /// </summary>
            public class WebViewQuery { }
        }
    }
    #endregion NOTE
}
