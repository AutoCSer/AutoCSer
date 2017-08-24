//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Web.WebView
{
        internal partial class Template
        {
            protected override bool page(ref AutoCSer.WebView.Response _html_)
            {
                byte[][] htmls;
                htmls = loadHtml(@"WebView\Template.html", 5);
                if (htmls != null)
                {
                    
                    _html_.WriteNotNull(htmls[0]);
                        _html_.Write(At);
                    _html_.WriteNotNull(htmls[1]);
                        _html_.Write(At);
                    _html_.WriteNotNull(htmls[2]);
                        _html_.Write(At);
                    _html_.WriteNotNull(htmls[3]);
                        _html_.Write(At);
                    _html_.WriteNotNull(htmls[4]);
                    return true;
                }
                return false;
            }

            protected override void ajax(CharStream _js_)
            {
                _js_.WriteNotNull(@"{At:");
                    {
                        string _value1_ = At;
                        if (_value1_ == null) _js_.WriteJsonNull();
                        else
                        {
                                    _js_.WriteJson(_value1_);
                        }
                    }
                    _js_.WriteNotNull(@"}");
            }

        }

}
namespace AutoCSer.Web
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
                    int count = 1 + 0 * 2;
                    string[] names = new string[count];
                    string[] views = new string[count];
                    names[--count] = "/WebView/Template";
                    views[count] = "/WebView/Template.html";
                    return new KeyValue<string[], string[]>(names, views);
                }
            }
            protected override string[] viewRewrites
            {
                get
                {
                    string[] names = new string[1];
                    names[0] = "/WebView/Template";
                    return names;
                }
            }
            protected override string[] views
            {
                get
                {
                    string[] names = new string[1];
                    names[0] = "/WebView/Template.html";
                    return names;
                }
            }
            protected override void request(int viewIndex, AutoCSer.Net.Http.SocketBase socket)
            {
                switch (viewIndex)
                {
                    case 0:
                        loadPage(socket, AutoCSer.Web.WebView.Template/**/.Pop() ?? new AutoCSer.Web.WebView.Template());
                        return;
                }
            }
            /// <summary>
            /// 网站生成配置
            /// </summary>
            internal new static readonly AutoCSer.Web.WebConfig WebConfig = new AutoCSer.Web.WebConfig();
            /// <summary>
            /// 网站生成配置
            /// </summary>
            /// <returns>网站生成配置</returns>
            protected override AutoCSer.WebView.Config getWebConfig() { return WebConfig; }
        }
}namespace AutoCSer.Web
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
                        loadView(AutoCSer.Web.WebView.Template/**/.Pop() ?? new AutoCSer.Web.WebView.Template(), ajaxInfo);
                        return;
                    default: return;
                }
            }
            protected override void loadAjax(AutoCSer.WebView.AjaxMethodInfo ajaxInfo)
            {
                switch (ajaxInfo.MethodIndex)
                {
                    case 0: loadAjax(AutoCSer.Web.Ajax.Example/**/.Pop() ?? new AutoCSer.Web.Ajax.Example(), ajaxInfo); return;
                    case 1: loadAjax(AutoCSer.Web.Ajax.TestCase/**/.Pop() ?? new AutoCSer.Web.Ajax.TestCase(), ajaxInfo); return;
                    case 4 - 1: pubError(); return;
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
                                AutoCSer.Web.Ajax.Example ajax = (AutoCSer.Web.Ajax.Example)page;
                                _p2 outputParameter = new _p2 { };
                                try
                                {
                                    
                                    outputParameter.Return = ajax.GetCode(inputParameter.file);
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
                                AutoCSer.Web.Ajax.TestCase ajax = (AutoCSer.Web.Ajax.TestCase)page;
                                _p2 outputParameter = new _p2 { };
                                try
                                {
                                    
                                    outputParameter.Return = ajax.GetCode(inputParameter.file);
                                }
                                finally { responseAjax(ajax, ref outputParameter); }
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
                public string file;
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            struct _p2
            {
                public string Return;
            }
            static AjaxLoader()
            {
                string[] names = new string[4];
                AutoCSer.WebView.AjaxMethodInfo[] infos = new AutoCSer.WebView.AjaxMethodInfo[4];
                names[0] = "Example.GetCode";
                infos[0] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 0, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)65536, MaxPostDataSize = 4194304 };
                names[1] = "TestCase.GetCode";
                infos[1] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 1, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)65536, MaxPostDataSize = 4194304 };
                names[2] = "/WebView/Template.html";
                infos[2] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 2, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)65536, MaxPostDataSize = 4194304, IsViewPage = true };
                names[4 - 1] = AutoCSer.WebView.AjaxBase.PubErrorCallName;
                infos[4 - 1] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 4 - 1, MaxPostDataSize = 2048, MaxMemoryStreamSize = AutoCSer.SubBuffer.Size.Kilobyte2, IsReferer = true, IsAsynchronous = true, IsPost = true };
                setMethods(names, infos);
            }
        }
}namespace AutoCSer.Web
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
                    string[] names = new string[2];
                    names[0] = "/Ajax";
                    names[1] = "/";
                    return names;
                }
            }
            private static readonly AutoCSer.WebView.CallMethodInfo _i1 = new AutoCSer.WebView.CallMethodInfo { MethodIndex = 1, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)65536, MaxPostDataSize = 4194304, IsOnlyPost = false };
            protected override void call(int callIndex, AutoCSer.Net.Http.SocketBase socket)
            {
                switch (callIndex)
                {
                    case 0:
                        AutoCSer.Web.AjaxLoader loader = AutoCSer.Web.AjaxLoader/**/.Pop() ?? new AutoCSer.Web.AjaxLoader();
                        ajaxLoader(loader, socket);
                        loader.Load();
                        return;
                    case 1:
                        load(socket, AutoCSer.Web.LocationIndex/**/.Pop() ?? new AutoCSer.Web.LocationIndex(), _i1);
                        return;
                }
            }
            protected override bool call(AutoCSer.WebView.CallBase call, ref AutoCSer.UnmanagedStream responseStream)
            {
                switch (call.CallMethodIndex)
                {
                    case 1:
                        {
                                    {
                                        AutoCSer.Web.LocationIndex value = (AutoCSer.Web.LocationIndex)call;
                                        value.Load();
                                        repsonseCall(value, ref responseStream);
                                        return true;
                                    }
                        }
                    default: return false;
                }
            }
            protected override bool call(AutoCSer.WebView.CallBase call)
            {
                switch (call.CallMethodIndex)
                {
                    default: return false;
                }
            }
        }
}
#endif