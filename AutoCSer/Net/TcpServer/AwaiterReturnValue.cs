using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// await 返回值包装
    /// </summary>
    /// <typeparam name="returnType">返回值类型</typeparam>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct AwaiterReturnValue<returnType>
#if NOJIT
        : AutoCSer.Net.IReturnParameter
#else
        : AutoCSer.Net.IReturnParameter<returnType>
#endif
    {
        /// <summary>
        /// 返回值
        /// </summary>
        [AutoCSer.Json.IgnoreMember]
        public returnType Ret;
        /// <summary>
        /// 返回值
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public returnType Return
        {
            get { return Ret; }
            set { Ret = value; }
        }
#if NOJIT
        /// <summary>
        /// 返回值
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        public object ReturnObject
        {
            get { return Ret; }
            set { Ret = (returnType)value; }
        }
#endif
    }
    /// <summary>
    /// await 返回值包装
    /// </summary>
    /// <typeparam name="returnType">返回值类型</typeparam>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    [AutoCSer.Metadata.BoxSerialize]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct AwaiterReturnValueBox<returnType>
#if NOJIT
        : AutoCSer.Net.IReturnParameter
#else
        : AutoCSer.Net.IReturnParameter<returnType>
#endif
    {
        /// <summary>
        /// 返回值
        /// </summary>
        [AutoCSer.Json.IgnoreMember]
        public returnType Ret;
        /// <summary>
        /// 返回值
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public returnType Return
        {
            get { return Ret; }
            set { Ret = value; }
        }
#if NOJIT
        /// <summary>
        /// 返回值
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        public object ReturnObject
        {
            get { return Ret; }
            set { Ret = (returnType)value; }
        }
#endif
    }
    /// <summary>
    /// await 返回值包装
    /// </summary>
    /// <typeparam name="returnType">返回值类型</typeparam>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct AwaiterReturnValueReference<returnType>
#if NOJIT
        : AutoCSer.Net.IReturnParameter
#else
        : AutoCSer.Net.IReturnParameter<returnType>
#endif
    {
        /// <summary>
        /// 返回值
        /// </summary>
        [AutoCSer.Json.IgnoreMember]
        public returnType Ret;
        /// <summary>
        /// 返回值
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public returnType Return
        {
            get { return Ret; }
            set { Ret = value; }
        }
#if NOJIT
        /// <summary>
        /// 返回值
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        public object ReturnObject
        {
            get { return Ret; }
            set { Ret = (returnType)value; }
        }
#endif
    }
    /// <summary>
    /// await 返回值包装
    /// </summary>
    /// <typeparam name="returnType">返回值类型</typeparam>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
    [AutoCSer.Metadata.BoxSerialize]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct AwaiterReturnValueBoxReference<returnType>
#if NOJIT
        : AutoCSer.Net.IReturnParameter
#else
        : AutoCSer.Net.IReturnParameter<returnType>
#endif
    {
        /// <summary>
        /// 返回值
        /// </summary>
        [AutoCSer.Json.IgnoreMember]
        public returnType Ret;
        /// <summary>
        /// 返回值
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public returnType Return
        {
            get { return Ret; }
            set { Ret = value; }
        }
#if NOJIT
        /// <summary>
        /// 返回值
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        public object ReturnObject
        {
            get { return Ret; }
            set { Ret = (returnType)value; }
        }
#endif
    }
}
