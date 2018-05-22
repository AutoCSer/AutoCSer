using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Parameter
{
    /// <summary>
    /// 查询参数节点
    /// </summary>
    public sealed partial class QueryReturnValue<nodeType>
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValueNode<nodeType>> QueryTask()
        {
            return new ReturnValueNode<nodeType>(await ClientDataStructure.Client.QueryTask(this));
        }
    }
    ///// <summary>
    ///// 查询参数节点
    ///// </summary>
    //public sealed partial class QueryReturnValueNew<valueType>
    //{
    //    /// <summary>
    //    /// 查询数据
    //    /// </summary>
    //    /// <returns></returns>
    //    public async Task<Abstract.ReturnValueNew<valueType>> QueryTask()
    //    {
    //        return new Abstract.ReturnValueNew<valueType>(await ClientDataStructure.Client.QueryTask(this));
    //    }
    //}
}

