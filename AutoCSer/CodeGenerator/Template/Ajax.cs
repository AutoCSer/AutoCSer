using System;
#pragma warning disable 649

namespace AutoCSer.CodeGenerator.Template
{
    class Ajax : Pub
    {
        #region PART CLASS
        /// <summary>
        /// AJAX函数调用
        /// </summary>
        #region NOTE
        [AutoCSer.Metadata.Ignore]
        #endregion NOTE
        [AutoCSer.WebView.Call]
        [AutoCSer.WebView.ClearMember(IsIgnoreCurrent = true)]
        public sealed class AjaxLoader : AutoCSer.WebView.AjaxLoader<AjaxLoader>
        {
            #region NOTE
            //static pub.FullName[] Methods = null;
            //static pub.FullName[] ViewMethods = null;
            const int MaxPostDataSize = 0;
            new const int MaxMemoryStreamSize = 0;
            const int MethodCount = 0;
            const int MethodIndex = 0;
            const bool IsAjaxOnlyPost = false;
            //new const bool IsPool = false;
            const bool IsPubError = false;
            const bool IsOnlyPost = false;
            const bool IsAsynchronous = false;
            const bool IsAwaitMethod = false;
            const bool IsAsynchronousCallback = false;
            const bool IsReferer = false;
            struct InputParameterTypeName
            {
                public ParameterType.FullName ParameterName;
            }
            struct OutputParameterTypeName
            {
                public ParameterType.FullName ParameterName;
                public MethodReturnType.FullName Return;
            }
            #endregion NOTE
            #region NOTE
            [AutoCSer.Metadata.Ignore]
            #endregion NOTE
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
                    #region LOOP ViewMethods
                    case @MethodIndex:
                        #region IF IsSetPage
                        #region IF IsPoolType
                        @WebViewMethodType.FullName @PageName = @WebViewMethodType.FullName/**/.Pop();
                        if (@PageName == null) setPage(@PageName = new @WebViewMethodType.FullName()/*PUSH:Attribute*/, @IsAsynchronous/*PUSH:Attribute*/, @IsAwaitMethod);
                        loadView(@PageName, ajaxInfo);
                        #endregion IF IsPoolType
                        #region NOT IsPoolType
                        loadView(/*NOTE*/(AutoCSer.WebView.View)(object)/*NOTE*/new @WebViewMethodType.FullName(), ajaxInfo/*PUSH:Attribute*/, @IsAsynchronous/*PUSH:Attribute*/, @IsAwaitMethod);
                        #endregion NOT IsPoolType
                        #endregion IF IsSetPage
                        #region NOT IsSetPage
                        loadView(/*IF:IsPoolType*/@WebViewMethodType.FullName/**/.Pop() ?? /*IF:IsPoolType*/new @WebViewMethodType.FullName(), ajaxInfo);
                        #endregion NOT IsSetPage
                        return;
                    #endregion LOOP ViewMethods
                    default: return;
                }
            }
            protected override void loadAjax(AutoCSer.WebView.AjaxMethodInfo ajaxInfo)
            {
                switch (ajaxInfo.MethodIndex)
                {
                    #region LOOP Methods
                    case @MethodIndex: loadAjax(/*IF:IsPoolType*/@WebAjaxMethodType.FullName/**/.Pop() ?? /*IF:IsPoolType*/new @WebAjaxMethodType.FullName(), ajaxInfo); return;
                    #endregion LOOP Methods
                    #region NOT IsPubError
                    case @MethodCount - 1: pubError(); return;
                    #endregion NOT IsPubError
                    default: return;
                }
            }
            protected override bool callAjax(int callIndex, AutoCSer.WebView.AjaxBase page)
            {
                switch (callIndex)
                {
                    #region LOOP Methods
                    case @MethodIndex:
                        {
                            #region IF InputParameterIndex
                            @InputParameterTypeName inputParameter = new @InputParameterTypeName();
                            if (page.ParseParameter(ref inputParameter))
                            #endregion IF InputParameterIndex
                            {
                                @WebAjaxMethodType.FullName ajax = (@WebAjaxMethodType.FullName)page;
                                #region IF OutputParameterIndex
                                @OutputParameterTypeName outputParameter = new @OutputParameterTypeName { /*LOOP:MethodParameters*//*IF:IsRef*/@ParameterName = inputParameter.@ParameterName, /*IF:IsRef*//*LOOP:MethodParameters*/};
                                #endregion IF OutputParameterIndex
                                #region IF IsAsynchronousCallback
                                #region IF MethodIsReturn
                                @AsyncIndexName returnCallbak = new @AsyncIndexName { Ajax = ajax, Parameter = outputParameter };
                                ajax./*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:MethodParameters*//*AT:ParameterRef*/inputParameter.@ParameterName, /*LOOP:MethodParameters*//*NOTE*/(Action<AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName>>)/*NOTE*/returnCallbak.Callback);
                                #endregion IF MethodIsReturn
                                #region NOT MethodIsReturn
                                #region IF IsPoolType
                                AjaxCallbackPool<@WebAjaxMethodType.FullName/*IF:OutputParameterIndex*/, @OutputParameterTypeName/*IF:OutputParameterIndex*/> callbackPool = new AjaxCallbackPool<@WebAjaxMethodType.FullName/*IF:OutputParameterIndex*/, @OutputParameterTypeName/*IF:OutputParameterIndex*/> { Ajax = ajax/*IF:OutputParameterIndex*/, Parameter = outputParameter/*IF:OutputParameterIndex*/ };
                                ajax./*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:MethodParameters*//*AT:ParameterRef*/inputParameter.@ParameterName, /*LOOP:MethodParameters*//*NOTE*/(Action<AutoCSer.Net.TcpServer.ReturnValue>)/*NOTE*/callbackPool.Callback);
                                #endregion IF IsPoolType
                                #region NOT IsPoolType
                                AjaxCallback/*IF:OutputParameterIndex*/<@OutputParameterTypeName>/*IF:OutputParameterIndex*/ callback = new AjaxCallback/*IF:OutputParameterIndex*/<@OutputParameterTypeName>/*IF:OutputParameterIndex*/ { Ajax = /*NOTE*/(AutoCSer.WebView.Ajax)(object)/*NOTE*/ajax/*IF:OutputParameterIndex*/, Parameter = outputParameter/*IF:OutputParameterIndex*/ };
                                ajax./*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:MethodParameters*//*AT:ParameterRef*/inputParameter.@ParameterName, /*LOOP:MethodParameters*//*NOTE*/(Action<AutoCSer.Net.TcpServer.ReturnValue>)/*NOTE*/callback.Callback);
                                #endregion NOT IsPoolType
                                #endregion NOT MethodIsReturn
                                #endregion IF IsAsynchronousCallback
                                #region NOT IsAsynchronousCallback
                                try
                                {
                                    /*IF:MethodIsReturn*/
                                    outputParameter.Return = /*NOTE*/(@MethodReturnType.FullName)/*NOTE*//*IF:MethodIsReturn*/ajax./*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:InputParameters*//*AT:ParameterRef*//*IF:MethodParameter.IsOut*//*PUSH:InputParameter*/outputParameter.@ParameterName/*PUSH:InputParameter*//*IF:MethodParameter.IsOut*//*NOTE*/,/*NOTE*//*NOT:MethodParameter.IsOut*//*PUSH:Parameter*/inputParameter.@ParameterName/*PUSH:Parameter*//*NOT:MethodParameter.IsOut*//*PUSH:Parameter*//*AT:ParameterJoin*//*PUSH:Parameter*//*LOOP:InputParameters*/);
                                    #region LOOP OutputParameters
                                    #region NOT InputMethodParameter.IsOut
                                    /*PUSH:Parameter*/
                                    outputParameter.@ParameterName/*PUSH:Parameter*/ = inputParameter./*PUSH:InputParameter*/@ParameterName/*PUSH:InputParameter*/;
                                    #endregion NOT InputMethodParameter.IsOut
                                    #endregion LOOP OutputParameters
                                }
                                finally { responseAjax(ajax/*IF:OutputParameterIndex*/, ref outputParameter/*IF:OutputParameterIndex*/); }
                                #endregion NOT IsAsynchronousCallback
                                return true;
                            }
                        }
                        return false;
                    #endregion LOOP Methods
                    default: return false;
                }
            }
            #region LOOP Methods
            #region IF IsAsynchronousCallback
            #region IF MethodIsReturn
            sealed class @AsyncIndexName : @AjaxCallbackPool</*IF:IsPoolType*/@WebAjaxMethodType.FullName, /*IF:IsPoolType*/@OutputParameterTypeName>
            {
                public void Callback(AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> value)
                {
                    @WebAjaxMethodType.FullName ajax = System.Threading.Interlocked.Exchange(ref Ajax, null);
                    if (ajax != null)
                    {
                        Parameter.Return = value.Value;
                        response(ajax, value.Type);
                    }
                }
            }
            #endregion IF MethodIsReturn
            #endregion IF IsAsynchronousCallback
            #endregion LOOP Methods
            #region LOOP ParameterTypes
            #region IF IsSerializeBox
            [AutoCSer.Metadata.BoxSerialize]
            #endregion IF IsSerializeBox
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            struct @ParameterTypeName
            {
                #region LOOP Parameters
                public @ParameterType.FullName @ParameterName;
                #endregion LOOP Parameters
                #region IF MethodReturnType.Type
                public @MethodReturnType.FullName Return;
                #endregion IF MethodReturnType.Type
            }
            #endregion LOOP ParameterTypes
            static AjaxLoader()
            {
                string[] names = new string[@MethodCount];
                AutoCSer.WebView.AjaxMethodInfo[] infos = new AutoCSer.WebView.AjaxMethodInfo[@MethodCount];
                #region LOOP Methods
                names[@MethodIndex] = "@CallName";
                infos[@MethodIndex] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = @MethodIndex, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)@MaxMemoryStreamSize/*PUSH:Attribute*/, MaxPostDataSize = @MaxPostDataSize/*IF:IsOnlyPost*/, IsPost = true/*IF:IsOnlyPost*//*IF:IsReferer*/, IsReferer = true/*IF:IsReferer*//*PUSH:Attribute*//*IF:IsAsynchronousCallback*/, IsAsynchronous = true/*IF:IsAsynchronousCallback*/ };
                #endregion LOOP Methods
                #region LOOP ViewMethods
                names[@MethodIndex] = "@CallName";
                infos[@MethodIndex] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = @MethodIndex, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)@MaxMemoryStreamSize/*PUSH:Attribute*/, MaxPostDataSize = @MaxPostDataSize/*PUSH:Attribute*/, IsViewPage = true };
                #endregion LOOP ViewMethods
                #region NOT IsPubError
                names[@MethodCount - 1/*NOTE*/+ 1/*NOTE*/] = AutoCSer.WebView.AjaxBase.PubErrorCallName/*IF:AutoParameter.WebConfig.IgnoreCase*/.ToLower()/*IF:AutoParameter.WebConfig.IgnoreCase*/;
                infos[@MethodCount - 1/*NOTE*/+ 1/*NOTE*/] = new AutoCSer.WebView.AjaxMethodInfo { MethodIndex = @MethodCount - 1, MaxPostDataSize = 2048, MaxMemoryStreamSize = AutoCSer.SubBuffer.Size.Kilobyte2, IsReferer = true, IsAsynchronous = true, IsPost = true };
                #endregion NOT IsPubError
                setMethods(names, infos);
                CompileJsonSerialize(new System.Type[] { /*LOOP:DeSerializeMethods*/typeof(@InputParameterTypeName), /*LOOP:DeSerializeMethods*/null }, new System.Type[] { /*LOOP:SerializeMethods*/typeof(@OutputParameterTypeName), /*LOOP:SerializeMethods*/null });
            }
        }
        #endregion PART CLASS
    }
    #region NOTE
    /// <summary>
    /// CSharp模板公用模糊类型
    /// </summary>
    internal partial class Pub
    {
        /// <summary>
        /// 获取该函数的类型
        /// </summary>
        public class WebAjaxMethodType
        {
            /// <summary>
            /// 类型全名
            /// </summary>
            public partial class FullName : AutoCSer.WebView.Ajax<FullName>
            {
                /// <summary>
                /// web调用
                /// </summary>
                /// <param name="value">调用参数</param>
                /// <returns>返回值</returns>
                public object MethodName(params object[] value)
                {
                    return null;
                }
            }
        }
    }
    #endregion NOTE
}
