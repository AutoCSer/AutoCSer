//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Example.WebView
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
                    string[] names = new string[11];
                    names[0] = "/Ajax";
                    names[1] = "/Call/Add";
                    names[2] = "/CallAsynchronous/Add";
                    names[3] = "/CallAsynchronous/AddBuffer";
                    names[4] = "/CallBoxSerialize/Inc";
                    names[5] = "/CallNameAdd";
                    names[6] = "/File/Download";
                    names[7] = "/";
                    names[8] = "/Location/Add";
                    names[9] = "/Location/Index";
                    names[10] = "/Upload/File";
                    return names;
                }
            }
            private static readonly AutoCSer.WebView.CallMethodInfo _i1 = new AutoCSer.WebView.CallMethodInfo { MethodIndex = 1, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsOnlyPost = false };
            private static readonly AutoCSer.WebView.CallMethodInfo _i2 = new AutoCSer.WebView.CallMethodInfo { MethodIndex = 2, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsOnlyPost = false };
            private static readonly AutoCSer.WebView.CallMethodInfo _i3 = new AutoCSer.WebView.CallMethodInfo { MethodIndex = 3, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsOnlyPost = false };
            private static readonly AutoCSer.WebView.CallMethodInfo _i4 = new AutoCSer.WebView.CallMethodInfo { MethodIndex = 4, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsOnlyPost = false };
            private static readonly AutoCSer.WebView.CallMethodInfo _i5 = new AutoCSer.WebView.CallMethodInfo { MethodIndex = 5, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsOnlyPost = false };
            private static readonly AutoCSer.WebView.CallMethodInfo _i6 = new AutoCSer.WebView.CallMethodInfo { MethodIndex = 6, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsOnlyPost = false };
            private static readonly AutoCSer.WebView.CallMethodInfo _i7 = new AutoCSer.WebView.CallMethodInfo { MethodIndex = 7, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsOnlyPost = false };
            private static readonly AutoCSer.WebView.CallMethodInfo _i8 = new AutoCSer.WebView.CallMethodInfo { MethodIndex = 8, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsOnlyPost = false };
            private static readonly AutoCSer.WebView.CallMethodInfo _i9 = new AutoCSer.WebView.CallMethodInfo { MethodIndex = 9, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsOnlyPost = false };
            private static readonly AutoCSer.WebView.CallMethodInfo _i10 = new AutoCSer.WebView.CallMethodInfo { MethodIndex = 10, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsOnlyPost = false };
            protected override void call(int callIndex, AutoCSer.Net.Http.SocketBase socket)
            {
                switch (callIndex)
                {
                    case 0:
                        AutoCSer.Example.WebView.AjaxLoader loader = AutoCSer.Example.WebView.AjaxLoader/**/.Pop() ?? new AutoCSer.Example.WebView.AjaxLoader();
                        ajaxLoader(loader, socket);
                        loader.Load();
                        return;
                    case 1:
                        load(socket, AutoCSer.Example.WebView.Call/**/.Pop() ?? new AutoCSer.Example.WebView.Call(), _i1);
                        return;
                    case 2:
                        loadAsynchronous(socket, AutoCSer.Example.WebView.CallAsynchronous/**/.Pop() ?? new AutoCSer.Example.WebView.CallAsynchronous(), _i2);
                        return;
                    case 3:
                        loadAsynchronous(socket, AutoCSer.Example.WebView.CallAsynchronous/**/.Pop() ?? new AutoCSer.Example.WebView.CallAsynchronous(), _i3);
                        return;
                    case 4:
                        load(socket, AutoCSer.Example.WebView.CallBoxSerialize/**/.Pop() ?? new AutoCSer.Example.WebView.CallBoxSerialize(), _i4);
                        return;
                    case 5:
                        load(socket, AutoCSer.Example.WebView.CallName/**/.Pop() ?? new AutoCSer.Example.WebView.CallName(), _i5);
                        return;
                    case 6:
                        loadAsynchronous(socket, AutoCSer.Example.WebView.File/**/.Pop() ?? new AutoCSer.Example.WebView.File(), _i6);
                        return;
                    case 7:
                        load(socket, AutoCSer.Example.WebView.Index/**/.Pop() ?? new AutoCSer.Example.WebView.Index(), _i7);
                        return;
                    case 8:
                        load(socket, AutoCSer.Example.WebView.Location/**/.Pop() ?? new AutoCSer.Example.WebView.Location(), _i8);
                        return;
                    case 9:
                        load(socket, AutoCSer.Example.WebView.Location/**/.Pop() ?? new AutoCSer.Example.WebView.Location(), _i9);
                        return;
                    case 10:
                        load(socket, AutoCSer.Example.WebView.Upload/**/.Pop() ?? new AutoCSer.Example.WebView.Upload(), _i10);
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
                                        AutoCSer.Example.WebView.Call value = (AutoCSer.Example.WebView.Call)call;
                                        value.Add(parameter.left, parameter.right);
                                        repsonseCall(value, ref responseStream);
                                        return true;
                                    }
                        }
                        return false;
                    case 4:
                        {
                            _p2 parameter = new _p2();
                            if (call.ParseParameter(ref parameter.value))
                                    {
                                        AutoCSer.Example.WebView.CallBoxSerialize value = (AutoCSer.Example.WebView.CallBoxSerialize)call;
                                        value.Inc(parameter.value);
                                        repsonseCall(value, ref responseStream);
                                        return true;
                                    }
                        }
                        return false;
                    case 5:
                        {
                            _p1 parameter = new _p1();
                                    if (call.ParseParameter(ref parameter))
                                    {
                                        AutoCSer.Example.WebView.CallName value = (AutoCSer.Example.WebView.CallName)call;
                                        value.Add(parameter.left, parameter.right);
                                        repsonseCall(value, ref responseStream);
                                        return true;
                                    }
                        }
                        return false;
                    case 7:
                        {
                                    {
                                        AutoCSer.Example.WebView.Index value = (AutoCSer.Example.WebView.Index)call;
                                        value.Home();
                                        repsonseCall(value, ref responseStream);
                                        return true;
                                    }
                        }
                    case 8:
                        {
                            _p1 parameter = new _p1();
                                    if (call.ParseParameter(ref parameter))
                                    {
                                        AutoCSer.Example.WebView.Location value = (AutoCSer.Example.WebView.Location)call;
                                        value.Add(parameter.left, parameter.right);
                                        repsonseCall(value, ref responseStream);
                                        return true;
                                    }
                        }
                        return false;
                    case 9:
                        {
                                    {
                                        AutoCSer.Example.WebView.Location value = (AutoCSer.Example.WebView.Location)call;
                                        value.Index();
                                        repsonseCall(value, ref responseStream);
                                        return true;
                                    }
                        }
                    case 10:
                        {
                                    {
                                        AutoCSer.Example.WebView.Upload value = (AutoCSer.Example.WebView.Upload)call;
                                        value.File();
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
                    case 2:
                        {
                            _p1 parameter = new _p1();
                                    if (call.ParseParameter(ref parameter))
                                    {
                                        ((AutoCSer.Example.WebView.CallAsynchronous)call).Add(parameter.left, parameter.right);
                                        return true;
                                    }
                        }
                        return false;
                    case 3:
                        {
                            _p1 parameter = new _p1();
                                    if (call.ParseParameter(ref parameter))
                                    {
                                        ((AutoCSer.Example.WebView.CallAsynchronous)call).AddBuffer(parameter.left, parameter.right);
                                        return true;
                                    }
                        }
                        return false;
                    case 6:
                        {
                                    {
                                        ((AutoCSer.Example.WebView.File)call).Download();
                                        return true;
                                    }
                        }
                    default: return false;
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
                public int value;
            }
        }
}namespace AutoCSer.Example.WebView
{
        internal partial class LoadView
        {

            protected override void ajax(CharStream _js_)
            {
                _js_.WriteNotNull(@"{IsView:");
                    {
                        bool _value1_ = IsView;
                                    _js_.WriteJson((bool)_value1_);
                    }
                    _js_.WriteNotNull(@"}");
            }

        }

}namespace AutoCSer.Example.WebView
{
        internal partial class LoadViewQuery
        {

            protected override void ajax(CharStream _js_)
            {
                _js_.WriteNotNull(@"{Sum:");
                    {
                        int _value1_ = Sum;
                                    _js_.WriteJson((int)_value1_);
                    }
                    _js_.WriteNotNull(@",query:");
                    {
                        AutoCSer.Example.WebView.LoadViewQuery.WebViewQuery _value1_ = query;
                            _js_.WriteNotNull(@"{left:");
                    {
                        int _value2_ = _value1_.left;
                                    _js_.WriteJson((int)_value2_);
                    }
                    _js_.WriteNotNull(@",right:");
                    {
                        int _value2_ = _value1_.right;
                                    _js_.WriteJson((int)_value2_);
                    }
                    _js_.WriteNotNull(@"}");
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

}namespace AutoCSer.Example.WebView
{
        internal partial class LoadViewQueryName
        {

            protected override void ajax(CharStream _js_)
            {
                _js_.WriteNotNull(@"{Sum:");
                    {
                        int _value1_ = Sum;
                                    _js_.WriteJson((int)_value1_);
                    }
                    _js_.WriteNotNull(@",parameter:");
                    {
                        AutoCSer.Example.WebView.LoadViewQueryName.WebViewQuery _value1_ = parameter;
                            _js_.WriteNotNull(@"{left:");
                    {
                        int _value2_ = _value1_.left;
                                    _js_.WriteJson((int)_value2_);
                    }
                    _js_.WriteNotNull(@",right:");
                    {
                        int _value2_ = _value1_.right;
                                    _js_.WriteJson((int)_value2_);
                    }
                    _js_.WriteNotNull(@"}");
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
            private WebViewQuery parameter;
            /// <summary>
            /// WEB视图加载
            /// </summary>
            /// <returns>是否成功</returns>
            protected override bool loadView()
            {
                if (base.loadView())
                {
                    
                    parameter= default(WebViewQuery);
                    if (ParseParameter(ref parameter))
                    {
                        return loadView(parameter.left, parameter.right);
                    }
                }
                return false;
            }
        }

}namespace AutoCSer.Example.WebView.Symbol
{
        internal partial class ReView
        {

            protected override void ajax(CharStream _js_)
            {
                _js_.WriteNotNull(@"{IsMobileReView:");
                    {
                        bool _value1_ = IsMobileReView;
                                    _js_.WriteJson((bool)_value1_);
                    }
                    _js_.WriteNotNull(@",IsReView:");
                    {
                        bool _value1_ = IsReView;
                                    _js_.WriteJson((bool)_value1_);
                    }
                    _js_.WriteNotNull(@",OutputData:");
                    {
                        AutoCSer.Example.WebView.Symbol.ViewOnlyData _value1_ = OutputData;
                            _js_.WriteNotNull(@"{Value1:");
                    {
                        int _value2_ = _value1_.Value1;
                                    _js_.WriteJson((int)_value2_);
                    }
                    _js_.WriteNotNull(@",Value2:");
                    {
                        int _value2_ = _value1_.Value2;
                                    _js_.WriteJson((int)_value2_);
                    }
                    _js_.WriteNotNull(@",ViewOnly:");
                    {
                        bool _value2_ = _value1_.ViewOnly;
                                    _js_.WriteJson((bool)_value2_);
                    }
                    _js_.WriteNotNull(@"}");
                    }
                    _js_.WriteNotNull(@",ReViewData:");
                    {
                        AutoCSer.Example.WebView.Symbol.ViewOnlyData _value1_ = ReViewData;
                            _js_.WriteNotNull(@"{Value1:");
                    {
                        int _value2_ = _value1_.Value1;
                                    _js_.WriteJson((int)_value2_);
                    }
                    _js_.WriteNotNull(@",Value2:");
                    {
                        int _value2_ = _value1_.Value2;
                                    _js_.WriteJson((int)_value2_);
                    }
                    _js_.WriteNotNull(@",ViewOnly:");
                    {
                        bool _value2_ = _value1_.ViewOnly;
                                    _js_.WriteJson((bool)_value2_);
                    }
                    _js_.WriteNotNull(@"}");
                    }
                    _js_.WriteNotNull(@",ViewOnlyData:");
                    {
                        AutoCSer.Example.WebView.Symbol.ViewOnlyData _value1_ = ViewOnlyData;
                            _js_.WriteNotNull(@"{Value1:");
                    {
                        int _value2_ = _value1_.Value1;
                                    _js_.WriteJson((int)_value2_);
                    }
                    _js_.WriteNotNull(@",Value2:");
                    {
                        int _value2_ = _value1_.Value2;
                                    _js_.WriteJson((int)_value2_);
                    }
                    _js_.WriteNotNull(@",ViewOnly:");
                    {
                        bool _value2_ = _value1_.ViewOnly;
                                    _js_.WriteJson((bool)_value2_);
                    }
                    _js_.WriteNotNull(@"}");
                    }
                    _js_.WriteNotNull(@"}");
            }

        }

}namespace AutoCSer.Example.WebView.Template
{
        internal partial class Client
        {
            protected override bool page(ref AutoCSer.WebView.Response _html_)
            {
                byte[][] htmls;
                htmls = loadHtml(@"Template\Client.html", 4);
                if (htmls != null)
                {
                    
                    _html_.WriteNotNull(htmls[0]);
                        _html_.Write(ServerData);
                    _html_.WriteNotNull(htmls[1]);
                    _html_.WriteNotNull(htmls[3]);
                    return true;
                }
                return false;
            }

            protected override void ajax(CharStream _js_)
            {
                _js_.WriteNotNull(@"{ServerData:");
                    {
                        int _value1_ = ServerData;
                                    _js_.WriteJson((int)_value1_);
                    }
                    _js_.WriteNotNull(@"}");
            }

        }

}namespace AutoCSer.Example.WebView.Template
{
        internal partial class Expression
        {

            protected override void ajax(CharStream _js_)
            {
                _js_.WriteNotNull(@"{ServerData:");
                    {
                        AutoCSer.Example.WebView.Template.Expression.Data _value1_ = ServerData;
                            _js_.WriteNotNull(@"{Value:");
                    {
                        string _value2_ = _value1_.Value;
                        if (_value2_ == null) _js_.WriteJsonNull();
                        else
                        {
                                    _js_.WriteJson(_value2_);
                        }
                    }
                    _js_.WriteNotNull(@"}");
                    }
                    _js_.WriteNotNull(@",Value:");
                    {
                        string _value1_ = Value;
                        if (_value1_ == null) _js_.WriteJsonNull();
                        else
                        {
                                    _js_.WriteJson(_value1_);
                        }
                    }
                    _js_.WriteNotNull(@"}");
            }

        }

}namespace AutoCSer.Example.WebView.Template
{
        internal partial class If
        {

            protected override void ajax(CharStream _js_)
            {
                _js_.WriteNotNull(@"{False:");
                    {
                        string _value1_ = False;
                        if (_value1_ == null) _js_.WriteJsonNull();
                        else
                        {
                                    _js_.WriteJson(_value1_);
                        }
                    }
                    _js_.WriteNotNull(@",String:");
                    {
                        string _value1_ = String;
                        if (_value1_ == null) _js_.WriteJsonNull();
                        else
                        {
                                    _js_.WriteJson(_value1_);
                        }
                    }
                    _js_.WriteNotNull(@",True:");
                    {
                        bool _value1_ = True;
                                    _js_.WriteJson((bool)_value1_);
                    }
                    _js_.WriteNotNull(@"}");
            }

        }

}namespace AutoCSer.Example.WebView.Template
{
        internal partial class Loop
        {

            protected override void ajax(CharStream _js_)
            {
                _js_.WriteNotNull(@"{LoopData:");
                    {
                        System.Collections.Generic.IEnumerable<AutoCSer.Example.WebView.Template.Loop.TestData> _value1_ = LoopData;
                        if (_value1_ == null) _js_.WriteJsonNull();
                        else
                        {
                            _js_.WriteNotNull(@"[");
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.Example.WebView.Template.Loop.TestData _value2_ in _value1_)
                        {
                            if (_loopIndex_ == 0)
                            {
                                _js_.Write('"');
                                _js_.WriteNotNull("Value");
                                _js_.Write('"');
                            }
                            _js_.Write(',');
                                _js_.WriteNotNull(@"[");
                    {
                        int _value3_ = _value2_.Value;
                                    _js_.WriteJson((int)_value3_);
                    }
                    _js_.WriteNotNull(@"]");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                    _js_.WriteNotNull(@"].FormatView()");
                        }
                    }
                    _js_.WriteNotNull(@"}");
            }

        }

}namespace AutoCSer.Example.WebView.Template
{
        internal partial class NoMark
        {
            protected override bool page(ref AutoCSer.WebView.Response _html_)
            {
                byte[][] htmls;
                htmls = loadHtml(@"Template\NoMark.html", 3);
                if (htmls != null)
                {
                    
                    _html_.WriteNotNull(htmls[0]);
                        _html_.WriteTextArea(Mark);
                    _html_.WriteNotNull(htmls[1]);
                        _html_.WriteTextArea(NoMarkValue);
                    _html_.WriteNotNull(htmls[2]);
                    return true;
                }
                return false;
            }

            protected override void ajax(CharStream _js_)
            {
                _js_.WriteNotNull(@"{Mark:");
                    {
                        string _value1_ = Mark;
                        if (_value1_ == null) _js_.WriteJsonNull();
                        else
                        {
                                    _js_.WriteJson(_value1_);
                        }
                    }
                    _js_.WriteNotNull(@",NoMarkValue:");
                    {
                        string _value1_ = NoMarkValue;
                        if (_value1_ == null) _js_.WriteJsonNull();
                        else
                        {
                                    _js_.WriteJson(_value1_);
                        }
                    }
                    _js_.WriteNotNull(@"}");
            }

        }

}namespace AutoCSer.Example.WebView.Template
{
        internal partial class Value
        {

            protected override void ajax(CharStream _js_)
            {
                _js_.WriteNotNull(@"{Data1:");
                    {
                        AutoCSer.Example.WebView.Template.Value.Test1 _value1_ = Data1;
                            _js_.WriteNotNull(@"{String1:");
                    {
                        string _value2_ = _value1_.String1;
                        if (_value2_ == null) _js_.WriteJsonNull();
                        else
                        {
                                    _js_.WriteJson(_value2_);
                        }
                    }
                    _js_.WriteNotNull(@"}");
                    }
                    _js_.WriteNotNull(@",Data2:");
                    {
                        AutoCSer.Example.WebView.Template.Value.Test2 _value1_ = Data2;
                        if (_value1_ == null) _js_.WriteJsonNull();
                        else
                        {
                            _js_.WriteNotNull(@"{String2:");
                    {
                        string _value2_ = _value1_.String2;
                        if (_value2_ == null) _js_.WriteJsonNull();
                        else
                        {
                                    _js_.WriteJson(_value2_);
                        }
                    }
                    _js_.WriteNotNull(@"}");
                        }
                    }
                    _js_.WriteNotNull(@"}");
            }

        }

}namespace AutoCSer.Example.WebView
{
        internal partial class ViewAsynchronous
        {

            protected override void ajax(CharStream _js_)
            {
                _js_.WriteNotNull(@"{Sum:");
                    {
                        int _value1_ = Sum;
                                    _js_.WriteJson((int)_value1_);
                    }
                    _js_.WriteNotNull(@",query:");
                    {
                        AutoCSer.Example.WebView.ViewAsynchronous.WebViewQuery _value1_ = query;
                            _js_.WriteNotNull(@"{left:");
                    {
                        int _value2_ = _value1_.left;
                                    _js_.WriteJson((int)_value2_);
                    }
                    _js_.WriteNotNull(@",right:");
                    {
                        int _value2_ = _value1_.right;
                                    _js_.WriteJson((int)_value2_);
                    }
                    _js_.WriteNotNull(@"}");
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
namespace AutoCSer.Example.WebView
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
                    int count = 11 + 0 * 2;
                    string[] names = new string[count];
                    string[] views = new string[count];
                    names[--count] = "/LoadView";
                    views[count] = "/LoadView.html";
                    names[--count] = "/LoadViewQuery";
                    views[count] = "/LoadViewQuery.html";
                    names[--count] = "/LoadViewQueryName";
                    views[count] = "/LoadViewQueryName.html";
                    names[--count] = "/Symbol/ReView";
                    views[count] = "/Symbol/ReView.html";
                    names[--count] = "/Template/Client";
                    views[count] = "/Template/Client.html";
                    names[--count] = "/Template/Expression";
                    views[count] = "/Template/Expression.html";
                    names[--count] = "/Template/If";
                    views[count] = "/Template/If.html";
                    names[--count] = "/Template/Loop";
                    views[count] = "/Template/Loop.html";
                    names[--count] = "/Template/NoMark";
                    views[count] = "/Template/NoMark.html";
                    names[--count] = "/Template/Value";
                    views[count] = "/Template/Value.html";
                    names[--count] = "/ViewAsynchronous";
                    views[count] = "/ViewAsynchronous.html";
                    return new KeyValue<string[], string[]>(names, views);
                }
            }
            protected override string[] viewRewrites
            {
                get
                {
                    string[] names = new string[2];
                    names[0] = "/Template/Client";
                    names[1] = "/Template/NoMark";
                    return names;
                }
            }
            protected override string[] views
            {
                get
                {
                    string[] names = new string[2];
                    names[0] = "/Template/Client.html";
                    names[1] = "/Template/NoMark.html";
                    return names;
                }
            }
            protected override void request(int viewIndex, AutoCSer.Net.Http.SocketBase socket)
            {
                switch (viewIndex)
                {
                    case 0:
                        loadPage(socket, AutoCSer.Example.WebView.Template.Client/**/.Pop() ?? new AutoCSer.Example.WebView.Template.Client());
                        return;
                    case 1:
                        loadPage(socket, AutoCSer.Example.WebView.Template.NoMark/**/.Pop() ?? new AutoCSer.Example.WebView.Template.NoMark());
                        return;
                }
            }
            /// <summary>
            /// 网站生成配置
            /// </summary>
            internal new static readonly AutoCSer.Example.WebView.WebConfig WebConfig = new AutoCSer.Example.WebView.WebConfig();
            /// <summary>
            /// 网站生成配置
            /// </summary>
            /// <returns>网站生成配置</returns>
            protected override AutoCSer.WebView.Config getWebConfig() { return WebConfig; }
            static WebServer()
            {
                CompileQueryParse(new System.Type[] { typeof(AutoCSer.Example.WebView.LoadViewQuery/**/.WebViewQuery), typeof(AutoCSer.Example.WebView.LoadViewQueryName/**/.WebViewQuery), typeof(AutoCSer.Example.WebView.ViewAsynchronous/**/.WebViewQuery), null }, new System.Type[] { typeof(_p1), typeof(_p2), null });
            }
        }
}namespace AutoCSer.Example.WebView
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
                    case 8:
                        loadView(AutoCSer.Example.WebView.LoadView/**/.Pop() ?? new AutoCSer.Example.WebView.LoadView(), ajaxInfo);
                        return;
                    case 9:
                        loadView(AutoCSer.Example.WebView.LoadViewQuery/**/.Pop() ?? new AutoCSer.Example.WebView.LoadViewQuery(), ajaxInfo);
                        return;
                    case 10:
                        loadView(AutoCSer.Example.WebView.LoadViewQueryName/**/.Pop() ?? new AutoCSer.Example.WebView.LoadViewQueryName(), ajaxInfo);
                        return;
                    case 11:
                        loadView(AutoCSer.Example.WebView.Symbol.ReView/**/.Pop() ?? new AutoCSer.Example.WebView.Symbol.ReView(), ajaxInfo);
                        return;
                    case 12:
                        loadView(AutoCSer.Example.WebView.Template.Client/**/.Pop() ?? new AutoCSer.Example.WebView.Template.Client(), ajaxInfo);
                        return;
                    case 13:
                        loadView(AutoCSer.Example.WebView.Template.Expression/**/.Pop() ?? new AutoCSer.Example.WebView.Template.Expression(), ajaxInfo);
                        return;
                    case 14:
                        loadView(AutoCSer.Example.WebView.Template.If/**/.Pop() ?? new AutoCSer.Example.WebView.Template.If(), ajaxInfo);
                        return;
                    case 15:
                        loadView(AutoCSer.Example.WebView.Template.Loop/**/.Pop() ?? new AutoCSer.Example.WebView.Template.Loop(), ajaxInfo);
                        return;
                    case 16:
                        loadView(AutoCSer.Example.WebView.Template.NoMark/**/.Pop() ?? new AutoCSer.Example.WebView.Template.NoMark(), ajaxInfo);
                        return;
                    case 17:
                        loadView(AutoCSer.Example.WebView.Template.Value/**/.Pop() ?? new AutoCSer.Example.WebView.Template.Value(), ajaxInfo);
                        return;
                    case 18:
                        AutoCSer.Example.WebView.ViewAsynchronous _p18 = AutoCSer.Example.WebView.ViewAsynchronous/**/.Pop();
                        if (_p18 == null) setPage(_p18 = new AutoCSer.Example.WebView.ViewAsynchronous(), true, false);
                        loadView(_p18, ajaxInfo);
                        return;
                    default: return;
                }
            }
            protected override void loadAjax(AutoCSer.WebView.AjaxMethodInfo ajaxInfo)
            {
                switch (ajaxInfo.MethodIndex)
                {
                    case 0: loadAjax(AutoCSer.Example.WebView.Ajax.Asynchronous/**/.Pop() ?? new AutoCSer.Example.WebView.Ajax.Asynchronous(), ajaxInfo); return;
                    case 1: loadAjax(AutoCSer.Example.WebView.Ajax.BoxSerialize/**/.Pop() ?? new AutoCSer.Example.WebView.Ajax.BoxSerialize(), ajaxInfo); return;
                    case 2: loadAjax(AutoCSer.Example.WebView.Ajax.Get/**/.Pop() ?? new AutoCSer.Example.WebView.Ajax.Get(), ajaxInfo); return;
                    case 3: loadAjax(AutoCSer.Example.WebView.Ajax.Name/**/.Pop() ?? new AutoCSer.Example.WebView.Ajax.Name(), ajaxInfo); return;
                    case 4: loadAjax(AutoCSer.Example.WebView.Ajax.Name/**/.Pop() ?? new AutoCSer.Example.WebView.Ajax.Name(), ajaxInfo); return;
                    case 5: loadAjax(AutoCSer.Example.WebView.Ajax.Post/**/.Pop() ?? new AutoCSer.Example.WebView.Ajax.Post(), ajaxInfo); return;
                    case 6: loadAjax(AutoCSer.Example.WebView.Ajax.Post/**/.Pop() ?? new AutoCSer.Example.WebView.Ajax.Post(), ajaxInfo); return;
                    case 7: loadAjax(AutoCSer.Example.WebView.Ajax.RefOut/**/.Pop() ?? new AutoCSer.Example.WebView.Ajax.RefOut(), ajaxInfo); return;
                    case 20 - 1: pubError(); return;
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
                                AutoCSer.Example.WebView.Ajax.Asynchronous ajax = (AutoCSer.Example.WebView.Ajax.Asynchronous)page;
                                _p2 outputParameter = new _p2 { };
                                _a0 returnCallbak = new _a0 { Ajax = ajax, Parameter = outputParameter };
                                ajax.Add(inputParameter.left, inputParameter.right, returnCallbak.Callback);
                                return true;
                            }
                        }
                        return false;
                    case 1:
                        {
                            _p3 inputParameter = new _p3();
                            if (page.ParseParameter(ref inputParameter))
                            {
                                AutoCSer.Example.WebView.Ajax.BoxSerialize ajax = (AutoCSer.Example.WebView.Ajax.BoxSerialize)page;
                                _p4 outputParameter = new _p4 { };
                                try
                                {
                                    
                                    outputParameter.Return = ajax.Inc(inputParameter.value);
                                }
                                finally { responseAjax(ajax, ref outputParameter); }
                                return true;
                            }
                        }
                        return false;
                    case 2:
                        {
                            _p1 inputParameter = new _p1();
                            if (page.ParseParameter(ref inputParameter))
                            {
                                AutoCSer.Example.WebView.Ajax.Get ajax = (AutoCSer.Example.WebView.Ajax.Get)page;
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
                    case 3:
                        {
                            _p1 inputParameter = new _p1();
                            if (page.ParseParameter(ref inputParameter))
                            {
                                AutoCSer.Example.WebView.Ajax.Name ajax = (AutoCSer.Example.WebView.Ajax.Name)page;
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
                    case 4:
                        {
                            _p1 inputParameter = new _p1();
                            if (page.ParseParameter(ref inputParameter))
                            {
                                AutoCSer.Example.WebView.Ajax.Name ajax = (AutoCSer.Example.WebView.Ajax.Name)page;
                                _p2 outputParameter = new _p2 { };
                                try
                                {
                                    
                                    outputParameter.Return = ajax.Add2(inputParameter.left, inputParameter.right);
                                }
                                finally { responseAjax(ajax, ref outputParameter); }
                                return true;
                            }
                        }
                        return false;
                    case 5:
                        {
                            _p1 inputParameter = new _p1();
                            if (page.ParseParameter(ref inputParameter))
                            {
                                AutoCSer.Example.WebView.Ajax.Post ajax = (AutoCSer.Example.WebView.Ajax.Post)page;
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
                    case 6:
                        {
                            _p1 inputParameter = new _p1();
                            if (page.ParseParameter(ref inputParameter))
                            {
                                AutoCSer.Example.WebView.Ajax.Post ajax = (AutoCSer.Example.WebView.Ajax.Post)page;
                                _p2 outputParameter = new _p2 { };
                                try
                                {
                                    
                                    outputParameter.Return = ajax.Mul(inputParameter.left, inputParameter.right);
                                }
                                finally { responseAjax(ajax, ref outputParameter); }
                                return true;
                            }
                        }
                        return false;
                    case 7:
                        {
                            _p5 inputParameter = new _p5();
                            if (page.ParseParameter(ref inputParameter))
                            {
                                AutoCSer.Example.WebView.Ajax.RefOut ajax = (AutoCSer.Example.WebView.Ajax.RefOut)page;
                                _p6 outputParameter = new _p6 { right = inputParameter.right, };
                                try
                                {
                                    
                                    outputParameter.Return = ajax.Add(inputParameter.left, ref inputParameter.right, out outputParameter.mul);
                                    
                                    outputParameter.right = inputParameter.right;
                                }
                                finally { responseAjax(ajax, ref outputParameter); }
                                return true;
                            }
                        }
                        return false;
                    default: return false;
                }
            }
            sealed class _a0 : AjaxCallbackPool<AutoCSer.Example.WebView.Ajax.Asynchronous, _p2>
            {
                public void Callback(AutoCSer.Net.TcpServer.ReturnValue<int> value)
                {
                    AutoCSer.Example.WebView.Ajax.Asynchronous ajax = System.Threading.Interlocked.Exchange(ref Ajax, null);
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
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            struct _p3
            {
                public int value;
            }
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            struct _p4
            {
                public int Return;
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            struct _p5
            {
                public int left;
                public int mul;
                public int right;
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            struct _p6
            {
                public int mul;
                public int right;
                public int Return;
            }
            static AjaxLoader()
            {
                string[] names = new string[20];
                AutoCSer.WebView.AjaxMethodInfo[] infos = new AutoCSer.WebView.AjaxMethodInfo[20];
                names[0] = "Asynchronous.Add";
                infos[0] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 0, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsPost = true, IsAsynchronous = true };
                names[1] = "BoxSerialize.Inc";
                infos[1] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 1, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsPost = true };
                names[2] = "Get.Add";
                infos[2] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 2, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304 };
                names[3] = "Name.AddName";
                infos[3] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 3, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsPost = true };
                names[4] = "AddFullName";
                infos[4] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 4, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsPost = true };
                names[5] = "Post.Add";
                infos[5] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 5, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsPost = true, IsReferer = true };
                names[6] = "Post.Mul";
                infos[6] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 6, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsPost = true, IsReferer = true };
                names[7] = "RefOut.Add";
                infos[7] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 7, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsPost = true };
                names[8] = "/LoadView.html";
                infos[8] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 8, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsViewPage = true };
                names[9] = "/LoadViewQuery.html";
                infos[9] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 9, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsViewPage = true };
                names[10] = "/LoadViewQueryName.html";
                infos[10] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 10, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsViewPage = true };
                names[11] = "/Symbol/ReView.html";
                infos[11] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 11, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsViewPage = true };
                names[12] = "/Template/Client.html";
                infos[12] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 12, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsViewPage = true };
                names[13] = "/Template/Expression.html";
                infos[13] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 13, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsViewPage = true };
                names[14] = "/Template/If.html";
                infos[14] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 14, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsViewPage = true };
                names[15] = "/Template/Loop.html";
                infos[15] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 15, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsViewPage = true };
                names[16] = "/Template/NoMark.html";
                infos[16] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 16, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsViewPage = true };
                names[17] = "/Template/Value.html";
                infos[17] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 17, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsViewPage = true };
                names[18] = "/ViewAsynchronous.html";
                infos[18] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 18, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsViewPage = true };
                names[20 - 1] = AutoCSer.WebView.AjaxBase.PubErrorCallName;
                infos[20 - 1] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 20 - 1, MaxPostDataSize = 2048, MaxMemoryStreamSize = AutoCSer.SubBuffer.Size.Kilobyte2, IsReferer = true, IsAsynchronous = true, IsPost = true };
                setMethods(names, infos);
                CompileJsonSerialize(new System.Type[] { typeof(_p1), typeof(_p3), typeof(_p5), null }, new System.Type[] { typeof(_p2), typeof(_p4), typeof(_p6), null });
            }
        }
}
#endif