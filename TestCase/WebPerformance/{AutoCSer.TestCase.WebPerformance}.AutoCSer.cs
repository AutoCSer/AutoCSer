//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.WebPerformance
{

        /// <summary>
        /// WEB服务器
        /// </summary>
        public partial class WebServer : AutoCSer.Net.HttpDomainServer.ViewServer<int>
        {
            protected override string[] calls
            {
                get
                {
                    string[] names = new string[3];
                    names[0] = "/Ajax";
                    names[1] = "/WebCall/Add";
                    names[2] = "/WebCallAsynchronous/Xor";
                    return names;
                }
            }
            private static readonly AutoCSer.WebView.CallMethodInfo _i1 = new AutoCSer.WebView.CallMethodInfo { MethodIndex = 1, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsOnlyPost = false };
            private static readonly AutoCSer.WebView.CallMethodInfo _i2 = new AutoCSer.WebView.CallMethodInfo { MethodIndex = 2, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsOnlyPost = false };
            protected override void call(int callIndex, AutoCSer.Net.Http.SocketBase socket)
            {
                switch (callIndex)
                {
                    case 0:
                        AutoCSer.TestCase.WebPerformance.AjaxLoader loader = AutoCSer.TestCase.WebPerformance.AjaxLoader/**/.Pop() ?? new AutoCSer.TestCase.WebPerformance.AjaxLoader();
                        ajaxLoader(loader, socket);
                        loader.Load();
                        return;
                    case 1:
                        load(socket, AutoCSer.TestCase.WebPerformance.WebCall/**/.Pop() ?? new AutoCSer.TestCase.WebPerformance.WebCall(), _i1);
                        return;
                    case 2:
                        loadAsynchronous(socket, AutoCSer.TestCase.WebPerformance.WebCallAsynchronous/**/.Pop() ?? new AutoCSer.TestCase.WebPerformance.WebCallAsynchronous(), _i2);
                        return;
                }
            }
            protected override bool call(AutoCSer.WebView.CallBase call, ref AutoCSer.UnmanagedStream responseStream)
            {
                switch (call.CallMethodIndex)
                {
                    case 1:
                        {
                            _p1 parameter = new _p1();
                                    if (call.ParseParameter(ref parameter))
                                    {
                                        AutoCSer.TestCase.WebPerformance.WebCall value = (AutoCSer.TestCase.WebPerformance.WebCall)call;
                                        value.Add(parameter.left, parameter.right);
                                        repsonseCall(value, ref responseStream);
                                        return true;
                                    }
                        }
                        return false;
                    default: return false;
                }
            }
            protected override bool call(AutoCSer.WebView.CallBase call)
            {
                switch (call.CallMethodIndex)
                {
                    case 2:
                        {
                            _p1 parameter = new _p1();
                                    if (call.ParseParameter(ref parameter))
                                    {
                                        ((AutoCSer.TestCase.WebPerformance.WebCallAsynchronous)call).Xor(parameter.left, parameter.right);
                                        return true;
                                    }
                        }
                        return false;
                    default: return false;
                }
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            struct _p1
            {
                public int left;
                public int right;
            }
        }
}namespace AutoCSer.TestCase.WebPerformance
{
        internal partial class WebView
        {
            protected override bool page(ref AutoCSer.WebView.Response _html_)
            {
                byte[][] htmls;
                htmls = loadHtml(@"WebView.html", 2);
                if (htmls != null)
                {
                    
                    _html_.WriteNotNull(htmls[0]);
                        _html_.Write(Return);
                    _html_.WriteNotNull(htmls[1]);
                    return true;
                }
                return false;
            }

            protected override void ajax(CharStream _js_)
            {
                _js_.WriteNotNull(@"{Return:");
                    {
                        int _value1_ = Return;
                                    _js_.WriteJson((int)_value1_);
                    }
                    _js_.WriteNotNull(@"}");
            }

            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct WebViewQuery
            {
                [AutoCSer.Json.ParseMember(IsDefault = true)]
                public int left;
                public int right;
            }
            /// <summary>
            /// 查询参数
            /// </summary>
            private WebViewQuery query;
            /// <summary>
            /// WEB视图加载
            /// </summary>
            /// <returns>是否成功</returns>
            protected override bool loadView()
            {
                if (base.loadView())
                {
                    
                    query= default(WebViewQuery);
                    if (ParseParameter(ref query))
                    {
                        return loadView(query.left, query.right);
                    }
                }
                return false;
            }
        }

}namespace AutoCSer.TestCase.WebPerformance
{
        internal partial class WebViewAsynchronous
        {
            protected override bool page(ref AutoCSer.WebView.Response _html_)
            {
                byte[][] htmls;
                htmls = loadHtml(@"WebViewAsynchronous.html", 2);
                if (htmls != null)
                {
                    
                    _html_.WriteNotNull(htmls[0]);
                        _html_.Write(Return);
                    _html_.WriteNotNull(htmls[1]);
                    return true;
                }
                return false;
            }

            protected override void ajax(CharStream _js_)
            {
                _js_.WriteNotNull(@"{Return:");
                    {
                        int _value1_ = Return;
                                    _js_.WriteJson((int)_value1_);
                    }
                    _js_.WriteNotNull(@"}");
            }

            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct WebViewQuery
            {
                [AutoCSer.Json.ParseMember(IsDefault = true)]
                public int left;
                public int right;
            }
            /// <summary>
            /// 查询参数
            /// </summary>
            private WebViewQuery query;
            /// <summary>
            /// WEB视图加载
            /// </summary>
            /// <returns>是否成功</returns>
            protected override bool loadView()
            {
                if (base.loadView())
                {
                    
                    query= default(WebViewQuery);
                    if (ParseParameter(ref query))
                    {
                        return loadView(query.left, query.right);
                    }
                }
                return false;
            }
        }

}
namespace AutoCSer.TestCase.WebPerformance
{


        /// <summary>
        /// WEB服务器
        /// </summary>
        public partial class WebServer : AutoCSer.Net.HttpDomainServer.ViewServer<int>
        {

            protected override KeyValue<string[], string[]> rewrites
            {
                get
                {
                    int count = 2 + 0 * 2;
                    string[] names = new string[count];
                    string[] views = new string[count];
                    names[--count] = "/WebView";
                    views[count] = "/WebView.html";
                    names[--count] = "/WebViewAsynchronous";
                    views[count] = "/WebViewAsynchronous.html";
                    return new KeyValue<string[], string[]>(names, views);
                }
            }
            protected override string[] viewRewrites
            {
                get
                {
                    string[] names = new string[2];
                    names[0] = "/WebView";
                    names[1] = "/WebViewAsynchronous";
                    return names;
                }
            }
            protected override string[] views
            {
                get
                {
                    string[] names = new string[2];
                    names[0] = "/WebView.html";
                    names[1] = "/WebViewAsynchronous.html";
                    return names;
                }
            }
            protected override void request(int viewIndex, AutoCSer.Net.Http.SocketBase socket)
            {
                switch (viewIndex)
                {
                    case 0:
                        loadPage(socket, AutoCSer.TestCase.WebPerformance.WebView/**/.Pop() ?? new AutoCSer.TestCase.WebPerformance.WebView());
                        return;
                    case 1:
                        AutoCSer.TestCase.WebPerformance.WebViewAsynchronous _p1 = AutoCSer.TestCase.WebPerformance.WebViewAsynchronous/**/.Pop();
                        if (_p1 == null) setPage(_p1 = new AutoCSer.TestCase.WebPerformance.WebViewAsynchronous(), true, false);
                        loadPage(socket, _p1);
                        return;
                }
            }
            /// <summary>
            /// 网站生成配置
            /// </summary>
            internal new static readonly AutoCSer.TestCase.WebPerformance.WebConfig WebConfig = new AutoCSer.TestCase.WebPerformance.WebConfig();
            /// <summary>
            /// 网站生成配置
            /// </summary>
            /// <returns>网站生成配置</returns>
            protected override AutoCSer.WebView.Config getWebConfig() { return WebConfig; }
            static WebServer()
            {
                CompileQueryParse(new System.Type[] { typeof(AutoCSer.TestCase.WebPerformance.WebView/**/.WebViewQuery), typeof(AutoCSer.TestCase.WebPerformance.WebViewAsynchronous/**/.WebViewQuery), null }, new System.Type[] { typeof(_p1), null });
            }
        }
}namespace AutoCSer.TestCase.WebPerformance
{

        /// <summary>
        /// AJAX函数调用
        /// </summary>
        [AutoCSer.WebView.Call]
        [AutoCSer.WebView.ClearMember(IsIgnoreCurrent = true)]
        public sealed class AjaxLoader : AutoCSer.WebView.AjaxLoader<AjaxLoader>
        {
            [AutoCSer.WebView.CallMethod(FullName = "/Ajax")]
            [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void Load()
            {
                load();
            }
            protected override void loadView(AutoCSer.WebView.AjaxMethodInfo ajaxInfo)
            {
                switch (ajaxInfo.MethodIndex)
                {
                    case 2:
                        loadView(AutoCSer.TestCase.WebPerformance.WebView/**/.Pop() ?? new AutoCSer.TestCase.WebPerformance.WebView(), ajaxInfo);
                        return;
                    case 3:
                        AutoCSer.TestCase.WebPerformance.WebViewAsynchronous _p3 = AutoCSer.TestCase.WebPerformance.WebViewAsynchronous/**/.Pop();
                        if (_p3 == null) setPage(_p3 = new AutoCSer.TestCase.WebPerformance.WebViewAsynchronous(), true, false);
                        loadView(_p3, ajaxInfo);
                        return;
                    default: return;
                }
            }
            protected override void loadAjax(AutoCSer.WebView.AjaxMethodInfo ajaxInfo)
            {
                switch (ajaxInfo.MethodIndex)
                {
                    case 0: loadAjax(AutoCSer.TestCase.WebPerformance.Ajax.Call/**/.Pop() ?? new AutoCSer.TestCase.WebPerformance.Ajax.Call(), ajaxInfo); return;
                    case 1: loadAjax(AutoCSer.TestCase.WebPerformance.Ajax.Call/**/.Pop() ?? new AutoCSer.TestCase.WebPerformance.Ajax.Call(), ajaxInfo); return;
                    case 5 - 1: pubError(); return;
                    default: return;
                }
            }
            protected override bool callAjax(int callIndex, AutoCSer.WebView.AjaxBase page)
            {
                switch (callIndex)
                {
                    case 0:
                        {
                            _p1 inputParameter = new _p1();
                            if (page.ParseParameter(ref inputParameter))
                            {
                                AutoCSer.TestCase.WebPerformance.Ajax.Call ajax = (AutoCSer.TestCase.WebPerformance.Ajax.Call)page;
                                _p2 outputParameter = new _p2 { };
                                try
                                {
                                    
                                    outputParameter.Return = ajax.Add(inputParameter.left, inputParameter.right);
                                }
                                finally { responseAjax(ajax, ref outputParameter); }
                                return true;
                            }
                        }
                        return false;
                    case 1:
                        {
                            _p1 inputParameter = new _p1();
                            if (page.ParseParameter(ref inputParameter))
                            {
                                AutoCSer.TestCase.WebPerformance.Ajax.Call ajax = (AutoCSer.TestCase.WebPerformance.Ajax.Call)page;
                                _p2 outputParameter = new _p2 { };
                                _a1 returnCallbak = new _a1 { Ajax = ajax, Parameter = outputParameter };
                                ajax.Xor(inputParameter.left, inputParameter.right, returnCallbak.Callback);
                                return true;
                            }
                        }
                        return false;
                    default: return false;
                }
            }
            sealed class _a1 : AjaxCallbackPool<AutoCSer.TestCase.WebPerformance.Ajax.Call, _p2>
            {
                public void Callback(AutoCSer.Net.TcpServer.ReturnValue<int> value)
                {
                    AutoCSer.TestCase.WebPerformance.Ajax.Call ajax = System.Threading.Interlocked.Exchange(ref Ajax, null);
                    if (ajax != null)
                    {
                        Parameter.Return = value.Value;
                        response(ajax, value.Type);
                    }
                }
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            struct _p1
            {
                public int left;
                public int right;
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            struct _p2
            {
                public int Return;
            }
            static AjaxLoader()
            {
                string[] names = new string[5];
                AutoCSer.WebView.AjaxMethodInfo[] infos = new AutoCSer.WebView.AjaxMethodInfo[5];
                names[0] = "Call.Add";
                infos[0] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 0, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304 };
                names[1] = "Call.Xor";
                infos[1] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 1, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsAsynchronous = true };
                names[2] = "/WebView.html";
                infos[2] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 2, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsViewPage = true };
                names[3] = "/WebViewAsynchronous.html";
                infos[3] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 3, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsViewPage = true };
                names[5 - 1] = AutoCSer.WebView.AjaxBase.PubErrorCallName;
                infos[5 - 1] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 5 - 1, MaxPostDataSize = 2048, MaxMemoryStreamSize = AutoCSer.SubBuffer.Size.Kilobyte2, IsReferer = true, IsAsynchronous = true, IsPost = true };
                setMethods(names, infos);
                CompileJsonSerialize(new System.Type[] { typeof(_p1), null }, new System.Type[] { typeof(_p2), null });
            }
        }
}
#endif