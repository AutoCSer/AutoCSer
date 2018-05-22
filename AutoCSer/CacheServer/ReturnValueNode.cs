using System;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 返回值
    /// </summary>
    /// <typeparam name="nodeType"></typeparam>
    public struct ReturnValueNode<nodeType>
        where nodeType : DataStructure.Abstract.Node, DataStructure.Abstract.IValue
    {
        /// <summary>
        /// 返回值
        /// </summary>
        internal ReturnParameter Value;
        /// <summary>
        /// 返回值类型
        /// </summary>
        public ReturnType Type
        {
            get { return Value.Type; }
        }
        /// <summary>
        /// 返回值
        /// </summary>
        /// <param name="value">返回值</param>
        internal ReturnValueNode(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            Value = value.Value;
            Value.Parameter.TcpReturnType = value.Type;
        }
    }
}
