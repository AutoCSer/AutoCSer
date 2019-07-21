//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Web
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
            private static readonly AutoCSer.WebView.CallMethodInfo _i1 = new AutoCSer.WebView.CallMethodInfo { MethodIndex = 1, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsOnlyPost = false };
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
}namespace AutoCSer.Web
{
        internal partial class Search
        {
            protected override bool page(ref AutoCSer.WebView.Response _html_)
            {
                byte[][] htmls;
                htmls = loadHtml(@"Search.html", 15);
                if (htmls != null)
                {
                    
                    _html_.WriteNotNull(htmls[0]);
                        _html_.Write(Key);
                    _html_.WriteNotNull(htmls[1]);
                {
                    AutoCSer.Web.SearchServer.SearchItem[] _value1_;
                    _value1_ = Items;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (AutoCSer.Web.SearchServer.SearchItem _value2_ in _value1_)
                        {
                    _html_.WriteNotNull(htmls[2]);
            _if_ = false;
                {
                    AutoCSer.Web.SearchServer.DataKey _value3_ = _value2_.DataKey;
                    {
                if (_value3_.Type.ToString() == @"HtmlTitle")
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
                    _html_.WriteNotNull(htmls[3]);
                {
                    AutoCSer.Web.SearchServer.Html _value3_ = _value2_.Html;
                    if (_value3_ != null)
                    {
                        _html_.WriteHtml(_value3_.Url);
                    }
                }
                    _html_.WriteNotNull(htmls[4]);
                    _html_.WriteNotNull(htmls[5]);
            }
                    _html_.WriteNotNull(htmls[6]);
            _if_ = false;
                {
                    AutoCSer.Web.SearchServer.DataKey _value3_ = _value2_.DataKey;
                    {
                if (_value3_.Type.ToString() == @"HtmlBodyText")
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
                    _html_.WriteNotNull(htmls[3]);
                {
                    AutoCSer.Web.SearchServer.Html _value3_ = _value2_.Html;
                    if (_value3_ != null)
                    {
                        _html_.WriteHtml(_value3_.Url);
                    }
                }
                    _html_.WriteNotNull(htmls[4]);
                {
                    AutoCSer.Web.SearchServer.Html _value3_ = _value2_.Html;
                    if (_value3_ != null)
                    {
                        _html_.WriteHtml(_value3_.Title);
                    }
                }
                    _html_.WriteNotNull(htmls[7]);
                    _html_.WriteNotNull(htmls[6]);
            }
                    _html_.WriteNotNull(htmls[6]);
            _if_ = false;
                {
                    AutoCSer.Web.SearchServer.DataKey _value3_ = _value2_.DataKey;
                    {
                if (_value3_.Type.ToString() == @"HtmlImage")
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
                    _html_.WriteNotNull(htmls[3]);
                {
                    AutoCSer.Web.SearchServer.Html _value3_ = _value2_.Html;
                    if (_value3_ != null)
                    {
                        _html_.WriteHtml(_value3_.Url);
                    }
                }
                    _html_.WriteNotNull(htmls[4]);
                {
                    AutoCSer.Web.SearchServer.Html _value3_ = _value2_.Html;
                    if (_value3_ != null)
                    {
                        _html_.WriteHtml(_value3_.Title);
                    }
                }
                    _html_.WriteNotNull(htmls[7]);
                    _html_.WriteNotNull(htmls[8]);
                {
                    AutoCSer.Web.SearchServer.SearchItem.RemoteExtension _value3_ = _value2_.Remote;
                    {
                        _html_.WriteHtml(_value3_.ImageUrl);
                    }
                }
                    _html_.WriteNotNull(htmls[9]);
            }
                    _html_.WriteNotNull(htmls[10]);
                {
                    AutoCSer.KeyValue<int,int>[] _value3_;
                    _value3_ = _value2_.Indexs;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (AutoCSer.KeyValue<int,int> _value4_ in _value3_)
                        {
                    _html_.WriteNotNull(htmls[6]);
                    _html_.WriteNotNull(htmls[6]);
                    _html_.WriteNotNull(htmls[6]);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
                    _html_.WriteNotNull(htmls[11]);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
                    _html_.WriteNotNull(htmls[12]);
            _if_ = false;
                {
                    AutoCSer.Web.SearchServer.SearchItem[] _value1_ = Items;
                    if (_value1_ != null)
                    {
                    if (_value1_.Length != 0)
                    {
                        _if_ = true;
                    }
                }
                }
            if (!_if_)
            {
                    _html_.WriteNotNull(htmls[13]);
            }
                    _html_.WriteNotNull(htmls[14]);
                    return true;
                }
                return false;
            }

            protected override void ajax(CharStream _js_)
            {
                _js_.WriteNotNull(@"{Items:");
                    {
                        AutoCSer.Web.SearchServer.SearchItem[] _value1_ = Items;
                        if (_value1_ == null) _js_.WriteJsonNull();
                        else
                        {
                            _js_.WriteNotNull(@"[");
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.Web.SearchServer.SearchItem _value2_ in _value1_)
                        {
                            if (_loopIndex_ == 0)
                            {
                                _js_.Write('"');
                                _js_.WriteNotNull("@.AutoCSerWeb.SearchItem,,DataKey[Id,Type]Html[Title,Url]Indexs[[Key,Value]]Remote[ImageUrl]Text");
                                _js_.Write('"');
                            }
                            _js_.Write(',');
                                _js_.WriteNotNull(@"[");
                    {
                        AutoCSer.Web.SearchServer.DataKey _value3_ = _value2_.DataKey;
                                    _js_.WriteNotNull(@"[");
                    {
                        int _value4_ = _value3_.Id;
                                    _js_.WriteJson((int)_value4_);
                    }
                    _js_.WriteNotNull(@",");
                    {
                        AutoCSer.Web.SearchServer.DataType _value4_ = _value3_.Type;
                                    _js_.CopyJsonNotNull(_value4_.ToString());
                    }
                    _js_.WriteNotNull(@"]");
                    }
                    _js_.WriteNotNull(@",");
                    {
                        AutoCSer.Web.SearchServer.Html _value3_ = _value2_.Html;
                                if (_value3_ == null) _js_.WriteJsonNull();
                                else
                                {
                                    _js_.WriteNotNull(@"[");
                    {
                        string _value4_ = _value3_.Title;
                                if (_value4_ == null) _js_.WriteJsonNull();
                                else
                                {
                                    _js_.WriteJson(_value4_);
                                }
                    }
                    _js_.WriteNotNull(@",");
                    {
                        string _value4_ = _value3_.Url;
                                if (_value4_ == null) _js_.WriteJsonNull();
                                else
                                {
                                    _js_.WriteJson(_value4_);
                                }
                    }
                    _js_.WriteNotNull(@"]");
                                }
                    }
                    _js_.WriteNotNull(@",");
                    {
                        AutoCSer.KeyValue<int,int>[] _value3_ = _value2_.Indexs;
                                if (_value3_ == null) _js_.WriteJsonNull();
                                else
                                {
                                    _js_.WriteNotNull(@"[[");
                    {
                        int _loopIndex3_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.KeyValue<int,int> _value4_ in _value3_)
                        {
                            if (_loopIndex_ != 0) _js_.Write(',');
                                _js_.Write('[');
                                _js_.WriteNotNull(@"");
                    {
                        int _value5_ = _value4_.Key;
                                    _js_.WriteJson((int)_value5_);
                    }
                    _js_.WriteNotNull(@",");
                    {
                        int _value5_ = _value4_.Value;
                                    _js_.WriteJson((int)_value5_);
                    }
                    _js_.WriteNotNull(@"]");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                    }
                    _js_.WriteNotNull(@"]]");
                                }
                    }
                    _js_.WriteNotNull(@",");
                    {
                        AutoCSer.Web.SearchServer.SearchItem.RemoteExtension _value3_ = _value2_.Remote;
                                    _js_.WriteNotNull(@"[");
                    {
                        string _value4_ = _value3_.ImageUrl;
                                if (_value4_ == null) _js_.WriteJsonNull();
                                else
                                {
                                    _js_.WriteJson(_value4_);
                                }
                    }
                    _js_.WriteNotNull(@"]");
                    }
                    _js_.WriteNotNull(@",");
                    {
                        AutoCSer.SubString _value3_ = _value2_.Text;
                                    _js_.WriteJson(_value3_);
                    }
                    _js_.WriteNotNull(@"]");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                    _js_.WriteNotNull(@"].FormatView()");
                        }
                    }
                    _js_.WriteNotNull(@",Key:");
                    {
                        string _value1_ = Key;
                        if (_value1_ == null) _js_.WriteJsonNull();
                        else
                        {
                                    _js_.WriteJson(_value1_);
                        }
                    }
                    _js_.WriteNotNull(@"}");
            }

            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct WebViewQuery
            {
                /// <summary>
                /// 搜索关键字
                /// </summary>
                public string Key;
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
                        Key = query.Key;
                        return true;
                    }
                }
                return false;
            }
        }

}namespace AutoCSer.Web.WebView
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
                    int count = 2 + 0 * 2;
                    string[] names = new string[count];
                    string[] views = new string[count];
                    names[--count] = "/Search";
                    views[count] = "/Search.html";
                    names[--count] = "/WebView/Template";
                    views[count] = "/WebView/Template.html";
                    return new KeyValue<string[], string[]>(names, views);
                }
            }
            protected override string[] viewRewrites
            {
                get
                {
                    string[] names = new string[2];
                    names[0] = "/Search";
                    names[1] = "/WebView/Template";
                    return names;
                }
            }
            protected override string[] views
            {
                get
                {
                    string[] names = new string[2];
                    names[0] = "/Search.html";
                    names[1] = "/WebView/Template.html";
                    return names;
                }
            }
            protected override void request(int viewIndex, AutoCSer.Net.Http.SocketBase socket)
            {
                switch (viewIndex)
                {
                    case 0:
                        loadPage(socket, AutoCSer.Web.Search/**/.Pop() ?? new AutoCSer.Web.Search());
                        return;
                    case 1:
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
            static WebServer()
            {
                CompileQueryParse(new System.Type[] { null }, new System.Type[] { null });
            }
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
                        loadView(AutoCSer.Web.Search/**/.Pop() ?? new AutoCSer.Web.Search(), ajaxInfo);
                        return;
                    case 3:
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
                string[] names = new string[5];
                AutoCSer.WebView.AjaxMethodInfo[] infos = new AutoCSer.WebView.AjaxMethodInfo[5];
                names[0] = "Example.GetCode";
                infos[0] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 0, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304 };
                names[1] = "TestCase.GetCode";
                infos[1] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 1, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304 };
                names[2] = "/Search.html";
                infos[2] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 2, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsViewPage = true };
                names[3] = "/WebView/Template.html";
                infos[3] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 3, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsViewPage = true };
                names[5 - 1] = AutoCSer.WebView.AjaxBase.PubErrorCallName;
                infos[5 - 1] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 5 - 1, MaxPostDataSize = 2048, MaxMemoryStreamSize = AutoCSer.SubBuffer.Size.Kilobyte2, IsReferer = true, IsAsynchronous = true, IsPost = true };
                setMethods(names, infos);
                CompileJsonSerialize(new System.Type[] { typeof(_p1), null }, new System.Type[] { typeof(_p2), null });
            }
        }
}
#endif