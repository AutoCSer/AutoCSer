using System;
using AutoCSer.CacheServer.DataStructure;
using AutoCSer.CacheServer.DataStructure.Value;

namespace AutoCSer.TestCase.CacheClientPerformance
{
    /// <summary>
    /// 简单数组测试
    /// </summary>
    sealed class Array : Test
    {
        /// <summary>
        /// 测试回调
        /// </summary>
        private readonly Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> getCallbackReturnParameter;
        /// <summary>
        /// 测试数组节点
        /// </summary>
        private readonly ValueArray<int> array;
        /// <summary>
        /// 简单数组测试
        /// </summary>
        /// <param name="client">测试客户端</param>
        /// <param name="isFile">服务端是否文件持久化</param>
        internal Array(AutoCSer.CacheServer.Client client, bool isFile) : base(client, isFile)
        {
            getCallbackReturnParameter = AutoCSer.CacheServer.ReturnParameter.GetCallback<int>(getCallback);
            array = client.GetOrCreateDataStructure<ValueArray<int>>("Array");
        }
        /// <summary>
        /// 测试
        /// </summary>
        internal void Test()
        {
            array.Clear();

            start(CallbackType.Asynchronous, TestType.ArraySetNodeCache);
            AutoCSer.CacheServer.DataStructure.Parameter.OperationBool setNode = array.GetSetNode(count - 1, int.MaxValue - count);
            for (int index = count; index != 0;)
            {
                --index;
                setNode.Operation(setCallbackReturnParameter);
            }
            wait();

            //start(CallbackType.Asynchronous, TestType.ArraySetValueCache);
            //Value<int> value = int.MaxValue - count;
            //for (int index = count; index != 0; array.Set(--index, value, setCallbackReturnParameter)) ;
            //wait();

            start(CallbackType.Asynchronous, TestType.ArraySet);
            for (int index = count; index != 0;)
            {
                --index;
                array.Set(index, count - index, setCallbackReturnParameter);
            }
            wait();

            start(CallbackType.Asynchronous, TestType.ArrayGetNodeCache);
            AutoCSer.CacheServer.DataStructure.Parameter.QueryReturnValue<int> getNode = array.GetNode(count - 1);
            for (int index = count; index != 0;)
            {
                --index;
                getNode.Query(getCallbackReturnParameter);
            }
            wait();

            start(CallbackType.Asynchronous, TestType.ArrayGet);
            for (int index = count; index != 0; array.Get(--index, getCallbackReturnParameter)) ;
            wait();

            array.Clear();

            start(CallbackType.Synchronous, TestType.ArraySet);
            for (int index = count; index != 0;)
            {
                --index;
                setCallback(array.Set(index, count - index));
            }
            wait();

            start(CallbackType.Synchronous, TestType.ArrayGet);
            for (int index = count; index != 0;)
            {
                --index;
                getCallback(array.Get(index));
            }
            wait();

            array.Clear();
        }
    }
}
