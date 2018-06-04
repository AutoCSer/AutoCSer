//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.SqlTableWeb
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
                        AutoCSer.TestCase.SqlTableWeb.AjaxLoader loader = AutoCSer.TestCase.SqlTableWeb.AjaxLoader/**/.Pop() ?? new AutoCSer.TestCase.SqlTableWeb.AjaxLoader();
                        ajaxLoader(loader, socket);
                        loader.Load();
                        return;
                    case 1:
                        load(socket, AutoCSer.TestCase.SqlTableWeb.Index/**/.Pop() ?? new AutoCSer.TestCase.SqlTableWeb.Index(), _i1);
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
                                        AutoCSer.TestCase.SqlTableWeb.Index value = (AutoCSer.TestCase.SqlTableWeb.Index)call;
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
}namespace AutoCSer.TestCase.SqlTableWeb
{
        internal partial class Class
        {
            protected override bool page(ref AutoCSer.WebView.Response _html_)
            {
                byte[][] htmls;
                htmls = loadHtml(@"Class.html", 21);
                if (htmls != null)
                {
                    
                    _html_.WriteNotNull(htmls[0]);
                    _html_.WriteNotNull(htmls[2]);
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Class _value1_ = ClassInfo;
                    if (_value1_ != null)
                    {
                        _html_.WriteHtml(_value1_.Name);
                    }
                }
                    _html_.WriteNotNull(htmls[3]);
                        _html_.Write(ViewMetaKeywords);
                    _html_.WriteNotNull(htmls[4]);
                        _html_.Write(ViewMetaDescription);
                    _html_.WriteNotNull(htmls[5]);
            _if_ = false;
                if (!(bool)FalseFlag)
                {
                    _if_ = true;
                }
            if (_if_)
            {
                    _html_.WriteNotNull(htmls[6]);
                {
                    AutoCSer.TestCase.SqlModel.WebPath.Pub _value1_ = PubPath;
                    {
                        _html_.Write(_value1_.ClassList);
                    }
                }
                    _html_.WriteNotNull(htmls[7]);
            }
                    _html_.WriteNotNull(htmls[8]);
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Class _value1_ = default(AutoCSer.TestCase.SqlTableCacheServer.Class);
                    _value1_ = ClassInfo;
            _if_ = false;
                    if (_value1_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                    _html_.WriteNotNull(htmls[9]);
                        _html_.WriteHtml(_value1_.Name);
                    _html_.WriteNotNull(htmls[10]);
                        _html_.Write(_value1_.Discipline.ToString());
                    _html_.WriteNotNull(htmls[11]);
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Class.RemoteExtension _value2_ = _value1_.Remote;
                    {
                        _html_.Write(_value2_.StudentCount);
                    }
                }
                    _html_.WriteNotNull(htmls[12]);
                    _html_.WriteNotNull(htmls[13]);
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Student[] _value2_ = default(AutoCSer.TestCase.SqlTableCacheServer.Student[]);
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Class.RemoteExtension _value3_ = _value1_.Remote;
                    {
                    _value2_ = _value3_.Students;
                    }
                }
                    if (_value2_ != null)
                    {
                        int _loopIndex2_ = _loopIndex_, _loopCount2_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value2_.Length;
                        foreach (AutoCSer.TestCase.SqlTableCacheServer.Student _value3_ in _value2_)
                        {
                    _html_.WriteNotNull(htmls[14]);
                {
                    AutoCSer.TestCase.SqlModel.WebPath.Student _value4_ = _value3_.Path;
                    {
                        _html_.WriteHtml(_value4_.Index);
                    }
                }
                    _html_.WriteNotNull(htmls[15]);
                        _html_.WriteHtml(_value3_.Name);
                    _html_.WriteNotNull(htmls[16]);
                        _html_.WriteHtml(_value3_.Email);
                    _html_.WriteNotNull(htmls[17]);
                        _html_.WriteHtml(_value3_.Gender.ToString());
                    _html_.WriteNotNull(htmls[18]);
                    _html_.WriteNotNull(htmls[13]);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex2_;
                        _loopCount_ = _loopCount2_;
                    }
                }
                    _html_.WriteNotNull(htmls[19]);
            }
                }
                    _html_.WriteNotNull(htmls[20]);
                    return true;
                }
                return false;
            }

            protected override void ajax(CharStream _js_)
            {
                _js_.WriteNotNull(@"{ClassInfo:");
                    {
                        AutoCSer.TestCase.SqlTableCacheServer.Class _value1_ = ClassInfo;
                        if (_value1_ == null) _js_.WriteJsonNull();
                        else
                        {
                            _js_.WriteNotNull(@"Demo.Class.Get({DateRange:");
                    {
                        AutoCSer.TestCase.SqlModel.Member.DateRange _value2_ = _value1_.DateRange;
                            _js_.WriteNotNull(@"{Start:");
                    {
                        AutoCSer.Sql.Member.IntDate _value3_ = _value2_.Start;
                            _js_.WriteNotNull(@"{DateTime:");
                    {
                        System.DateTime _value4_ = _value3_.DateTime;
                                    _js_.WriteJson((System.DateTime)_value4_);
                    }
                    _js_.WriteNotNull(@",Value:");
                    {
                        int _value4_ = _value3_.Value;
                                    _js_.WriteJson((int)_value4_);
                    }
                    _js_.WriteNotNull(@"}");
                    }
                    _js_.WriteNotNull(@"}");
                    }
                    _js_.WriteNotNull(@",Discipline:");
                    {
                        AutoCSer.TestCase.SqlModel.Member.Discipline _value2_ = _value1_.Discipline;
                                    _js_.CopyJsonNotNull(_value2_.ToString());
                    }
                    _js_.WriteNotNull(@",Id:");
                    {
                        int _value2_ = _value1_.Id;
                                    _js_.WriteJson((int)_value2_);
                    }
                    _js_.WriteNotNull(@",Name:");
                    {
                        string _value2_ = _value1_.Name;
                        if (_value2_ == null) _js_.WriteJsonNull();
                        else
                        {
                                    _js_.WriteJson(_value2_);
                        }
                    }
                    _js_.WriteNotNull(@",Remote:");
                    {
                        AutoCSer.TestCase.SqlTableCacheServer.Class.RemoteExtension _value2_ = _value1_.Remote;
                            _js_.WriteNotNull(@"{StudentCount:");
                    {
                        int _value3_ = _value2_.StudentCount;
                                    _js_.WriteJson((int)_value3_);
                    }
                    _js_.WriteNotNull(@",Students:");
                    {
                        AutoCSer.TestCase.SqlTableCacheServer.Student[] _value3_ = _value2_.Students;
                        if (_value3_ == null) _js_.WriteJsonNull();
                        else
                        {
                            _js_.WriteNotNull(@"[");
                    {
                        int _loopIndex3_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.TestCase.SqlTableCacheServer.Student _value4_ in _value3_)
                        {
                            if (_loopIndex_ == 0)
                            {
                                _js_.Write('"');
                                _js_.WriteNotNull("@.Demo.Student,,Birthday[DateTime,Value]Email,Gender,Id,Name");
                                _js_.Write('"');
                            }
                            _js_.Write(',');
                            if (_value4_ == null) _js_.WriteJsonNull();
                            else
                            {
                                _js_.WriteNotNull(@"[");
                    {
                        AutoCSer.Sql.Member.IntDate _value5_ = _value4_.Birthday;
                                    _js_.WriteNotNull(@"[");
                    {
                        System.DateTime _value6_ = _value5_.DateTime;
                                    _js_.WriteJson((System.DateTime)_value6_);
                    }
                    _js_.WriteNotNull(@",");
                    {
                        int _value6_ = _value5_.Value;
                                    _js_.WriteJson((int)_value6_);
                    }
                    _js_.WriteNotNull(@"]");
                    }
                    _js_.WriteNotNull(@",");
                    {
                        string _value5_ = _value4_.Email;
                                if (_value5_ == null) _js_.WriteJsonNull();
                                else
                                {
                                    _js_.WriteJson(_value5_);
                                }
                    }
                    _js_.WriteNotNull(@",");
                    {
                        AutoCSer.TestCase.SqlModel.Member.Gender _value5_ = _value4_.Gender;
                                    _js_.CopyJsonNotNull(_value5_.ToString());
                    }
                    _js_.WriteNotNull(@",");
                    {
                        int _value5_ = _value4_.Id;
                                    _js_.WriteJson((int)_value5_);
                    }
                    _js_.WriteNotNull(@",");
                    {
                        string _value5_ = _value4_.Name;
                                if (_value5_ == null) _js_.WriteJsonNull();
                                else
                                {
                                    _js_.WriteJson(_value5_);
                                }
                    }
                    _js_.WriteNotNull(@"]");
                            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                    }
                    _js_.WriteNotNull(@"].FormatView()");
                        }
                    }
                    _js_.WriteNotNull(@"}");
                    }
                    _js_.WriteNotNull(@"})");
                        }
                    }
                    _js_.WriteNotNull(@",PubPath:");
                    {
                        AutoCSer.TestCase.SqlModel.WebPath.Pub _value1_ = PubPath;
                                    _js_.WriteNotNull(@"new AutoCSerPath.Pub({})");
                    }
                    _js_.WriteNotNull(@"}");
            }

            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct WebViewQuery
            {
                [AutoCSer.Json.ParseMember(IsDefault = true)]
                /// <summary>
                /// 班级标识
                /// </summary>
                public int ClassId;
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
                        return loadView(query.ClassId);
                    }
                }
                return false;
            }
        }

}namespace AutoCSer.TestCase.SqlTableWeb
{
        internal partial class ClassList
        {
            protected override bool page(ref AutoCSer.WebView.Response _html_)
            {
                byte[][] htmls;
                htmls = loadHtml(@"ClassList.html", 17);
                if (htmls != null)
                {
                    
                    _html_.WriteNotNull(htmls[0]);
                    _html_.WriteNotNull(htmls[2]);
                        _html_.Write(ViewMetaKeywords);
                    _html_.WriteNotNull(htmls[3]);
                        _html_.Write(ViewMetaDescription);
                    _html_.WriteNotNull(htmls[4]);
            _if_ = false;
                if (!(bool)IsClassList)
                {
                    _if_ = true;
                }
            if (_if_)
            {
                    _html_.WriteNotNull(htmls[5]);
                {
                    AutoCSer.TestCase.SqlModel.WebPath.Pub _value1_ = PubPath;
                    {
                        _html_.Write(_value1_.ClassList);
                    }
                }
                    _html_.WriteNotNull(htmls[6]);
            }
                    _html_.WriteNotNull(htmls[7]);
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Class[] _value1_;
                    _value1_ = Classes;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (AutoCSer.TestCase.SqlTableCacheServer.Class _value2_ in _value1_)
                        {
                    _html_.WriteNotNull(htmls[8]);
                {
                    AutoCSer.TestCase.SqlModel.WebPath.Class _value3_ = _value2_.Path;
                    {
                        _html_.WriteHtml(_value3_.Index);
                    }
                }
                    _html_.WriteNotNull(htmls[9]);
                        _html_.WriteHtml(_value2_.Name);
                    _html_.WriteNotNull(htmls[10]);
                        _html_.Write(_value2_.Discipline.ToString());
                    _html_.WriteNotNull(htmls[11]);
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Class.RemoteExtension _value3_ = _value2_.Remote;
                    {
                        _html_.Write(_value3_.StudentCount);
                    }
                }
                    _html_.WriteNotNull(htmls[12]);
                    _html_.WriteNotNull(htmls[13]);
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Student[] _value3_ = default(AutoCSer.TestCase.SqlTableCacheServer.Student[]);
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Class.RemoteExtension _value4_ = _value2_.Remote;
                    {
                    _value3_ = _value4_.Students;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (AutoCSer.TestCase.SqlTableCacheServer.Student _value4_ in _value3_)
                        {
                    _html_.WriteNotNull(htmls[5]);
                {
                    AutoCSer.TestCase.SqlModel.WebPath.Student _value5_ = _value4_.Path;
                    {
                        _html_.WriteHtml(_value5_.Index);
                    }
                }
                    _html_.WriteNotNull(htmls[9]);
                        _html_.WriteHtml(_value4_.Name);
                    _html_.WriteNotNull(htmls[14]);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
                    _html_.WriteNotNull(htmls[15]);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
                    _html_.WriteNotNull(htmls[16]);
                    return true;
                }
                return false;
            }

            protected override void ajax(CharStream _js_)
            {
                _js_.WriteNotNull(@"{Classes:");
                    {
                        AutoCSer.TestCase.SqlTableCacheServer.Class[] _value1_ = Classes;
                        if (_value1_ == null) _js_.WriteJsonNull();
                        else
                        {
                            _js_.WriteNotNull(@"[");
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.TestCase.SqlTableCacheServer.Class _value2_ in _value1_)
                        {
                            if (_loopIndex_ == 0)
                            {
                                _js_.Write('"');
                                _js_.WriteNotNull("@.Demo.Class,,DateRange[Start[DateTime,Value]]Discipline,Id,Name,Remote[StudentCount,Students[[@.Demo.Student,,Id,Name]]]");
                                _js_.Write('"');
                            }
                            _js_.Write(',');
                            if (_value2_ == null) _js_.WriteJsonNull();
                            else
                            {
                                _js_.WriteNotNull(@"[");
                    {
                        AutoCSer.TestCase.SqlModel.Member.DateRange _value3_ = _value2_.DateRange;
                                    _js_.WriteNotNull(@"[");
                    {
                        AutoCSer.Sql.Member.IntDate _value4_ = _value3_.Start;
                                    _js_.WriteNotNull(@"[");
                    {
                        System.DateTime _value5_ = _value4_.DateTime;
                                    _js_.WriteJson((System.DateTime)_value5_);
                    }
                    _js_.WriteNotNull(@",");
                    {
                        int _value5_ = _value4_.Value;
                                    _js_.WriteJson((int)_value5_);
                    }
                    _js_.WriteNotNull(@"]");
                    }
                    _js_.WriteNotNull(@"]");
                    }
                    _js_.WriteNotNull(@",");
                    {
                        AutoCSer.TestCase.SqlModel.Member.Discipline _value3_ = _value2_.Discipline;
                                    _js_.CopyJsonNotNull(_value3_.ToString());
                    }
                    _js_.WriteNotNull(@",");
                    {
                        int _value3_ = _value2_.Id;
                                    _js_.WriteJson((int)_value3_);
                    }
                    _js_.WriteNotNull(@",");
                    {
                        string _value3_ = _value2_.Name;
                                if (_value3_ == null) _js_.WriteJsonNull();
                                else
                                {
                                    _js_.WriteJson(_value3_);
                                }
                    }
                    _js_.WriteNotNull(@",");
                    {
                        AutoCSer.TestCase.SqlTableCacheServer.Class.RemoteExtension _value3_ = _value2_.Remote;
                                    _js_.WriteNotNull(@"[");
                    {
                        int _value4_ = _value3_.StudentCount;
                                    _js_.WriteJson((int)_value4_);
                    }
                    _js_.WriteNotNull(@",");
                    {
                        AutoCSer.TestCase.SqlTableCacheServer.Student[] _value4_ = _value3_.Students;
                                if (_value4_ == null) _js_.WriteJsonNull();
                                else
                                {
                                    _js_.WriteNotNull(@"[[");
                    {
                        int _loopIndex4_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.TestCase.SqlTableCacheServer.Student _value5_ in _value4_)
                        {
                            if (_loopIndex_ != 0) _js_.Write(',');
                            if (_value5_ == null) _js_.WriteJsonNull();
                            else
                            {
                                _js_.Write('[');
                                _js_.WriteNotNull(@"");
                    {
                        int _value6_ = _value5_.Id;
                                    _js_.WriteJson((int)_value6_);
                    }
                    _js_.WriteNotNull(@",");
                    {
                        string _value6_ = _value5_.Name;
                                if (_value6_ == null) _js_.WriteJsonNull();
                                else
                                {
                                    _js_.WriteJson(_value6_);
                                }
                    }
                    _js_.WriteNotNull(@"]");
                            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex4_;
                    }
                    _js_.WriteNotNull(@"]]");
                                }
                    }
                    _js_.WriteNotNull(@"]");
                    }
                    _js_.WriteNotNull(@"]");
                            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                    _js_.WriteNotNull(@"].FormatView()");
                        }
                    }
                    _js_.WriteNotNull(@",IsClassList:");
                    {
                        bool _value1_ = IsClassList;
                                    _js_.WriteJson((bool)_value1_);
                    }
                    _js_.WriteNotNull(@",PubPath:");
                    {
                        AutoCSer.TestCase.SqlModel.WebPath.Pub _value1_ = PubPath;
                                    _js_.WriteNotNull(@"new AutoCSerPath.Pub({})");
                    }
                    _js_.WriteNotNull(@"}");
            }

        }

}namespace AutoCSer.TestCase.SqlTableWeb
{
        internal partial class Student
        {
            protected override bool page(ref AutoCSer.WebView.Response _html_)
            {
                byte[][] htmls;
                htmls = loadHtml(@"Student.html", 22);
                if (htmls != null)
                {
                    
                    _html_.WriteNotNull(htmls[0]);
                    _html_.WriteNotNull(htmls[2]);
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Student _value1_ = StudentInfo;
                    if (_value1_ != null)
                    {
                        _html_.WriteHtml(_value1_.Name);
                    }
                }
                    _html_.WriteNotNull(htmls[3]);
                        _html_.Write(ViewMetaKeywords);
                    _html_.WriteNotNull(htmls[4]);
                        _html_.Write(ViewMetaDescription);
                    _html_.WriteNotNull(htmls[5]);
            _if_ = false;
                if (!(bool)FalseFlag)
                {
                    _if_ = true;
                }
            if (_if_)
            {
                    _html_.WriteNotNull(htmls[6]);
                {
                    AutoCSer.TestCase.SqlModel.WebPath.Pub _value1_ = PubPath;
                    {
                        _html_.Write(_value1_.ClassList);
                    }
                }
                    _html_.WriteNotNull(htmls[7]);
            }
                    _html_.WriteNotNull(htmls[8]);
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Student _value1_ = default(AutoCSer.TestCase.SqlTableCacheServer.Student);
                    _value1_ = StudentInfo;
            _if_ = false;
                    if (_value1_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                    _html_.WriteNotNull(htmls[9]);
                        _html_.WriteHtml(_value1_.Name);
                    _html_.WriteNotNull(htmls[10]);
                        _html_.WriteHtml(_value1_.Email);
                    _html_.WriteNotNull(htmls[11]);
                        _html_.WriteHtml(_value1_.Gender.ToString());
                    _html_.WriteNotNull(htmls[12]);
                    _html_.WriteNotNull(htmls[13]);
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Student.RemoteExtension.JoinClassDate[] _value2_ = default(AutoCSer.TestCase.SqlTableCacheServer.Student.RemoteExtension.JoinClassDate[]);
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Student.RemoteExtension _value3_ = _value1_.Remote;
                    {
                    _value2_ = _value3_.Classes;
                    }
                }
                    if (_value2_ != null)
                    {
                        int _loopIndex2_ = _loopIndex_, _loopCount2_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value2_.Length;
                        foreach (AutoCSer.TestCase.SqlTableCacheServer.Student.RemoteExtension.JoinClassDate _value3_ in _value2_)
                        {
                    _html_.WriteNotNull(htmls[14]);
                    _html_.WriteNotNull(htmls[15]);
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Class _value4_ = default(AutoCSer.TestCase.SqlTableCacheServer.Class);
                    _value4_ = _value3_.Class;
            _if_ = false;
                    if (_value4_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                    _html_.WriteNotNull(htmls[6]);
                {
                    AutoCSer.TestCase.SqlModel.WebPath.Class _value5_ = _value4_.Path;
                    {
                        _html_.WriteHtml(_value5_.Index);
                    }
                }
                    _html_.WriteNotNull(htmls[16]);
                        _html_.WriteHtml(_value4_.Name);
                    _html_.WriteNotNull(htmls[17]);
                        _html_.Write(_value4_.Discipline.ToString());
                    _html_.WriteNotNull(htmls[18]);
            }
                }
                    _html_.WriteNotNull(htmls[19]);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex2_;
                        _loopCount_ = _loopCount2_;
                    }
                }
                    _html_.WriteNotNull(htmls[20]);
            }
                }
                    _html_.WriteNotNull(htmls[21]);
                    return true;
                }
                return false;
            }

            protected override void ajax(CharStream _js_)
            {
                _js_.WriteNotNull(@"{PubPath:");
                    {
                        AutoCSer.TestCase.SqlModel.WebPath.Pub _value1_ = PubPath;
                                    _js_.WriteNotNull(@"new AutoCSerPath.Pub({})");
                    }
                    _js_.WriteNotNull(@",StudentInfo:");
                    {
                        AutoCSer.TestCase.SqlTableCacheServer.Student _value1_ = StudentInfo;
                        if (_value1_ == null) _js_.WriteJsonNull();
                        else
                        {
                            _js_.WriteNotNull(@"Demo.Student.Get({Birthday:");
                    {
                        AutoCSer.Sql.Member.IntDate _value2_ = _value1_.Birthday;
                            _js_.WriteNotNull(@"{DateTime:");
                    {
                        System.DateTime _value3_ = _value2_.DateTime;
                                    _js_.WriteJson((System.DateTime)_value3_);
                    }
                    _js_.WriteNotNull(@",Value:");
                    {
                        int _value3_ = _value2_.Value;
                                    _js_.WriteJson((int)_value3_);
                    }
                    _js_.WriteNotNull(@"}");
                    }
                    _js_.WriteNotNull(@",Email:");
                    {
                        string _value2_ = _value1_.Email;
                        if (_value2_ == null) _js_.WriteJsonNull();
                        else
                        {
                                    _js_.WriteJson(_value2_);
                        }
                    }
                    _js_.WriteNotNull(@",Gender:");
                    {
                        AutoCSer.TestCase.SqlModel.Member.Gender _value2_ = _value1_.Gender;
                                    _js_.CopyJsonNotNull(_value2_.ToString());
                    }
                    _js_.WriteNotNull(@",Id:");
                    {
                        int _value2_ = _value1_.Id;
                                    _js_.WriteJson((int)_value2_);
                    }
                    _js_.WriteNotNull(@",Name:");
                    {
                        string _value2_ = _value1_.Name;
                        if (_value2_ == null) _js_.WriteJsonNull();
                        else
                        {
                                    _js_.WriteJson(_value2_);
                        }
                    }
                    _js_.WriteNotNull(@",Remote:");
                    {
                        AutoCSer.TestCase.SqlTableCacheServer.Student.RemoteExtension _value2_ = _value1_.Remote;
                            _js_.WriteNotNull(@"{Classes:");
                    {
                        AutoCSer.TestCase.SqlTableCacheServer.Student.RemoteExtension.JoinClassDate[] _value3_ = _value2_.Classes;
                        if (_value3_ == null) _js_.WriteJsonNull();
                        else
                        {
                            _js_.WriteNotNull(@"[");
                    {
                        int _loopIndex3_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.TestCase.SqlTableCacheServer.Student.RemoteExtension.JoinClassDate _value4_ in _value3_)
                        {
                            if (_loopIndex_ == 0)
                            {
                                _js_.Write('"');
                                _js_.WriteNotNull("Class[@.Demo.Class,,Discipline,Id,Name]ClassDate[Date[DateTime,Value]]");
                                _js_.Write('"');
                            }
                            _js_.Write(',');
                                _js_.WriteNotNull(@"[");
                    {
                        AutoCSer.TestCase.SqlTableCacheServer.Class _value5_ = _value4_.Class;
                                if (_value5_ == null) _js_.WriteJsonNull();
                                else
                                {
                                    _js_.WriteNotNull(@"[");
                    {
                        AutoCSer.TestCase.SqlModel.Member.Discipline _value6_ = _value5_.Discipline;
                                    _js_.CopyJsonNotNull(_value6_.ToString());
                    }
                    _js_.WriteNotNull(@",");
                    {
                        int _value6_ = _value5_.Id;
                                    _js_.WriteJson((int)_value6_);
                    }
                    _js_.WriteNotNull(@",");
                    {
                        string _value6_ = _value5_.Name;
                                if (_value6_ == null) _js_.WriteJsonNull();
                                else
                                {
                                    _js_.WriteJson(_value6_);
                                }
                    }
                    _js_.WriteNotNull(@"]");
                                }
                    }
                    _js_.WriteNotNull(@",");
                    {
                        AutoCSer.TestCase.SqlModel.Member.ClassDate _value5_ = _value4_.ClassDate;
                                    _js_.WriteNotNull(@"[");
                    {
                        AutoCSer.Sql.Member.IntDate _value6_ = _value5_.Date;
                                    _js_.WriteNotNull(@"[");
                    {
                        System.DateTime _value7_ = _value6_.DateTime;
                                    _js_.WriteJson((System.DateTime)_value7_);
                    }
                    _js_.WriteNotNull(@",");
                    {
                        int _value7_ = _value6_.Value;
                                    _js_.WriteJson((int)_value7_);
                    }
                    _js_.WriteNotNull(@"]");
                    }
                    _js_.WriteNotNull(@"]");
                    }
                    _js_.WriteNotNull(@"]");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                    }
                    _js_.WriteNotNull(@"].FormatView()");
                        }
                    }
                    _js_.WriteNotNull(@"}");
                    }
                    _js_.WriteNotNull(@"})");
                        }
                    }
                    _js_.WriteNotNull(@"}");
            }

            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct WebViewQuery
            {
                [AutoCSer.Json.ParseMember(IsDefault = true)]
                /// <summary>
                /// 学生标识
                /// </summary>
                public int StudentId;
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
                        return loadView(query.StudentId);
                    }
                }
                return false;
            }
        }

}
namespace AutoCSer.TestCase.SqlTableWeb
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
                    int count = 3 + 0 * 2;
                    string[] names = new string[count];
                    string[] views = new string[count];
                    names[--count] = "/Class";
                    views[count] = "/Class.html";
                    names[--count] = "/ClassList";
                    views[count] = "/ClassList.html";
                    names[--count] = "/Student";
                    views[count] = "/Student.html";
                    return new KeyValue<string[], string[]>(names, views);
                }
            }
            protected override string[] viewRewrites
            {
                get
                {
                    string[] names = new string[3];
                    names[0] = "/Class";
                    names[1] = "/ClassList";
                    names[2] = "/Student";
                    return names;
                }
            }
            protected override string[] views
            {
                get
                {
                    string[] names = new string[3];
                    names[0] = "/Class.html";
                    names[1] = "/ClassList.html";
                    names[2] = "/Student.html";
                    return names;
                }
            }
            protected override void request(int viewIndex, AutoCSer.Net.Http.SocketBase socket)
            {
                switch (viewIndex)
                {
                    case 0:
                        loadPage(socket, AutoCSer.TestCase.SqlTableWeb.Class/**/.Pop() ?? new AutoCSer.TestCase.SqlTableWeb.Class());
                        return;
                    case 1:
                        loadPage(socket, AutoCSer.TestCase.SqlTableWeb.ClassList/**/.Pop() ?? new AutoCSer.TestCase.SqlTableWeb.ClassList());
                        return;
                    case 2:
                        loadPage(socket, AutoCSer.TestCase.SqlTableWeb.Student/**/.Pop() ?? new AutoCSer.TestCase.SqlTableWeb.Student());
                        return;
                }
            }
            /// <summary>
            /// 网站生成配置
            /// </summary>
            internal new static readonly AutoCSer.TestCase.SqlTableWeb.WebConfig WebConfig = new AutoCSer.TestCase.SqlTableWeb.WebConfig();
            /// <summary>
            /// 网站生成配置
            /// </summary>
            /// <returns>网站生成配置</returns>
            protected override AutoCSer.WebView.Config getWebConfig() { return WebConfig; }
            static WebServer()
            {
                CompileQueryParse(new System.Type[] { typeof(AutoCSer.TestCase.SqlTableWeb.Class/**/.WebViewQuery), typeof(AutoCSer.TestCase.SqlTableWeb.Student/**/.WebViewQuery), null }, new System.Type[] { null });
            }
        }
}namespace AutoCSer.TestCase.SqlTableWeb
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
                    case 0:
                        loadView(AutoCSer.TestCase.SqlTableWeb.Class/**/.Pop() ?? new AutoCSer.TestCase.SqlTableWeb.Class(), ajaxInfo);
                        return;
                    case 1:
                        loadView(AutoCSer.TestCase.SqlTableWeb.ClassList/**/.Pop() ?? new AutoCSer.TestCase.SqlTableWeb.ClassList(), ajaxInfo);
                        return;
                    case 2:
                        loadView(AutoCSer.TestCase.SqlTableWeb.Student/**/.Pop() ?? new AutoCSer.TestCase.SqlTableWeb.Student(), ajaxInfo);
                        return;
                    default: return;
                }
            }
            protected override void loadAjax(AutoCSer.WebView.AjaxMethodInfo ajaxInfo)
            {
                switch (ajaxInfo.MethodIndex)
                {
                    case 4 - 1: pubError(); return;
                    default: return;
                }
            }
            protected override bool callAjax(int callIndex, AutoCSer.WebView.AjaxBase page)
            {
                switch (callIndex)
                {
                    default: return false;
                }
            }
            static AjaxLoader()
            {
                string[] names = new string[4];
                AutoCSer.WebView.AjaxMethodInfo[] infos = new AutoCSer.WebView.AjaxMethodInfo[4];
                names[0] = "/Class.html";
                infos[0] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 0, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsViewPage = true };
                names[1] = "/ClassList.html";
                infos[1] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 1, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsViewPage = true };
                names[2] = "/Student.html";
                infos[2] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 2, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsViewPage = true };
                names[4 - 1] = AutoCSer.WebView.AjaxBase.PubErrorCallName;
                infos[4 - 1] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = 4 - 1, MaxPostDataSize = 2048, MaxMemoryStreamSize = AutoCSer.SubBuffer.Size.Kilobyte2, IsReferer = true, IsAsynchronous = true, IsPost = true };
                setMethods(names, infos);
                CompileJsonSerialize(new System.Type[] { null }, new System.Type[] { null });
            }
        }
}
#endif