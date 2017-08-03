using System;

namespace AutoCSer.CodeGenerator.Template
{
    class WebCall : Pub
    {
        #region PART CLASS
        #region IF Methods.Length
        /// <summary>
        /// WEB服务器
        /// </summary>
        public partial class WebServer : AutoCSer.Net.HttpDomainServer.ViewServer<@SessionType.FullName>
        {
            #region NOTE
            const int MaxPostDataSize = 0;
            const int MaxMemoryStreamSize = 0;
            const int MethodIndex = 0;
            //const bool IsAsynchronous = false;
            FullName[] Methods = null;
            const bool IsOnlyPost = false;
            //const bool IsPool = false;
            #endregion NOTE
            protected override string[] calls
            {
                get
                {
                    string[] names = new string[@Methods.Length];
                    #region LOOP Methods
                    names[@MethodIndex] = "@CallName";
                    #endregion LOOP Methods
                    return names;
                }
            }
            #region LOOP Methods
            #region NOT IsAjaxLoad
            private static readonly AutoCSer.WebView.CallMethodInfo @CallMethodInfo = new AutoCSer.WebView.CallMethodInfo { MethodIndex = @MethodIndex, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)@MaxMemoryStreamSize/*PUSH:Attribute*/, MaxPostDataSize = @MaxPostDataSize, IsOnlyPost = @IsOnlyPost/*PUSH:Attribute*/ };
            #endregion NOT IsAjaxLoad
            #endregion LOOP Methods
            protected override void call(int callIndex, AutoCSer.Net.Http.SocketBase socket)
            {
                switch (callIndex)
                {
                    #region LOOP Methods
                    case @MethodIndex:
                        #region IF IsAjaxLoad
                        @WebCallAsynchronousMethodType.FullName loader = @WebCallAsynchronousMethodType.FullName/**/.Pop() ?? new @WebCallAsynchronousMethodType.FullName();
                        ajaxLoader(loader, socket);
                        loader./*PUSH:Method*/@MethodName/*PUSH:Method*/();
                        #endregion IF IsAjaxLoad
                        #region NOT IsAjaxLoad
                        #region IF IsAsynchronous
                        loadAsynchronous(socket,/*IF:IsPoolType*/ @WebCallAsynchronousMethodType.FullName/**/.Pop() ??/*IF:IsPoolType*/ new @WebCallAsynchronousMethodType.FullName(), @CallMethodInfo);
                        #endregion IF IsAsynchronous
                        #region NOT IsAsynchronous
                        load(socket,/*IF:IsPoolType*/ @WebCallMethodType.FullName/**/.Pop() ??/*IF:IsPoolType*/ new @WebCallMethodType.FullName(), @CallMethodInfo);
                        #endregion NOT IsAsynchronous
                        #endregion NOT IsAjaxLoad
                        return;
                    #endregion LOOP Methods
                }
            }
            protected override bool call(AutoCSer.WebView.CallBase call, ref AutoCSer.UnmanagedStream responseStream)
            {
                switch (call.CallMethodIndex)
                {
                    #region LOOP Methods
                    #region NOT IsAjaxLoad
                    #region NOT IsAsynchronous
                    case @MethodIndex:
                        {
                            #region IF MethodParameters.Length
                            @ParameterTypeName parameter = new @ParameterTypeName();
                            #region IF Attribute.IsFirstParameter
                            #region IF IsFristParameterValueType
                            if (call.ParseParameter(ref parameter/*PUSH:FristParameter*/.@ValueTypeParameterName/*PUSH:FristParameter*/))
                                #endregion IF IsFristParameterValueType
                                #region NOT IsFristParameterValueType
                                if (call.ParseParameterAny(ref parameter/*PUSH:FristParameter*/.@ParameterName/*PUSH:FristParameter*/))
                                    #endregion NOT IsFristParameterValueType
                                    #endregion IF Attribute.IsFirstParameter
                                    #region NOT Attribute.IsFirstParameter
                                    if (call.ParseParameter(ref parameter))
                                    #endregion NOT Attribute.IsFirstParameter
                                    #endregion IF MethodParameters.Length
                                    {
                                        @WebCallMethodType.FullName value = (@WebCallMethodType.FullName)/*NOTE*/(object)/*NOTE*/call;
                                        value./*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:MethodParameters*//*AT:ParameterRef*/parameter.@ParameterName/*AT:ParameterJoin*//*LOOP:MethodParameters*/);
                                        repsonseCall(value, ref responseStream);
                                        return true;
                                    }
                        }
                        #region IF MethodParameters.Length
                        return false;
                    #endregion IF MethodParameters.Length
                    #endregion NOT IsAsynchronous
                    #endregion NOT IsAjaxLoad
                    #endregion LOOP Methods
                    default: return false;
                }
            }
            protected override bool call(AutoCSer.WebView.CallBase call)
            {
                switch (call.CallMethodIndex)
                {
                    #region LOOP Methods
                    #region NOT IsAjaxLoad
                    #region IF IsAsynchronous
                    case @MethodIndex:
                        {
                            #region IF MethodParameters.Length
                            @ParameterTypeName parameter = new @ParameterTypeName();
                            #region IF Attribute.IsFirstParameter
                            #region IF IsFristParameterValueType
                            if (call.ParseParameter(ref parameter/*PUSH:FristParameter*/.@ValueTypeParameterName/*PUSH:FristParameter*/))
                                #endregion IF IsFristParameterValueType
                                #region NOT IsFristParameterValueType
                                if (call.ParseParameterAny(ref parameter/*PUSH:FristParameter*/.@ParameterName/*PUSH:FristParameter*/))
                                    #endregion NOT IsFristParameterValueType
                                    #endregion IF Attribute.IsFirstParameter
                                    #region NOT Attribute.IsFirstParameter
                                    if (call.ParseParameter(ref parameter))
                                    #endregion NOT Attribute.IsFirstParameter
                                    #endregion IF MethodParameters.Length
                                    {
                                        ((@WebCallMethodType.FullName)/*NOTE*/(object)/*NOTE*/call)./*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:MethodParameters*//*AT:ParameterRef*/parameter.@ParameterName/*AT:ParameterJoin*//*LOOP:MethodParameters*/);
                                        return true;
                                    }
                        }
                        #region IF MethodParameters.Length
                        return false;
                    #endregion IF MethodParameters.Length
                    #endregion IF IsAsynchronous
                    #endregion NOT IsAjaxLoad
                    #endregion LOOP Methods
                    default: return false;
                }
            }
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
                #region NOTE
                public int ValueTypeParameterName;
                #endregion NOTE
            }
            #endregion LOOP ParameterTypes
        }
        #endregion IF Methods.Length
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
        public class WebCallMethodType
        {
            /// <summary>
            /// 类型全名
            /// </summary>
            public partial class FullName : AutoCSer.WebView.Call<FullName>
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
                /// <summary>
                /// 
                /// </summary>
                internal override void CancelGetForm()
                {
                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="callIndex"></param>
                /// <param name="page"></param>
                /// <returns></returns>
                protected override bool callAjax(int callIndex, AutoCSer.WebView.AjaxBase page)
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 获取该函数的类型
        /// </summary>
        public class WebCallAsynchronousMethodType
        {
            /// <summary>
            /// 类型全名
            /// </summary>
            public partial class FullName : AutoCSer.WebView.CallAsynchronous<FullName>
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
                /// <summary>
                /// 
                /// </summary>
                internal override void CancelGetForm()
                {
                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="callIndex"></param>
                /// <param name="page"></param>
                /// <returns></returns>
                protected override bool callAjax(int callIndex, AutoCSer.WebView.AjaxBase page)
                {
                    return false;
                }
            }
        }
    }
    #endregion NOTE
}
