using System;
using System.Threading;
using AutoCSer.CacheServer.DataStructure;
using AutoCSer.CacheServer.DataStructure.Value;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.CacheClientPerformance
{
    /// <summary>
    /// Json 测试
    /// </summary>
    sealed class Json : Test
    {
        /// <summary>
        /// 测试回调
        /// </summary>
        private readonly Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> getCallbackReturnParameter;
        /// <summary>
        /// 测试字典节点
        /// </summary>
        private readonly ValueDictionary<int, Json<int>> dictionary;
        /// <summary>
        /// 简单数组测试
        /// </summary>
        /// <param name="client">测试客户端</param>
        /// <param name="isFile">服务端是否文件持久化</param>
        internal Json(AutoCSer.CacheServer.Client client, bool isFile) : base(client, isFile)
        {
            getCallbackReturnParameter = AutoCSer.CacheServer.ReturnParameter.GetCallback<Json<int>>(getCallback);
            dictionary = client.GetOrCreateDataStructure<ValueDictionary<int, Json<int>>>("Json");
        }
        /// <summary>
        /// 测试回调
        /// </summary>
        /// <param name="value"></param>
        private void getCallback(AutoCSer.CacheServer.ReturnValue<Json<int>> value)
        {
            if (value.Get().Type != AutoCSer.CacheServer.ReturnType.Success) ++errorCount;
            getCallback();
        }
        /// <summary>
        /// 测试
        /// </summary>
        internal void Test()
        {
            dictionary.Clear();

            start(CallbackType.Asynchronous, TestType.JsonSetNodeCache);
            AutoCSer.CacheServer.DataStructure.Parameter.OperationBool setNode = dictionary.GetSetNode(count - 1, int.MaxValue - count);
            for (int index = count; index != 0;)
            {
                --index;
                setNode.Operation(setCallbackReturnParameter);
            }
            wait();

            start(CallbackType.Asynchronous, TestType.JsonSetValueCache);
            Json<int> value = int.MaxValue - count;
            for (int index = count; index != 0; dictionary.Set(--index, value, setCallbackReturnParameter)) ;
            wait();

            dictionary.Clear();

            start(CallbackType.Asynchronous, TestType.JsonSet);
            for (int index = count; index != 0;)
            {
                --index;
                dictionary.Set(index, count - index, setCallbackReturnParameter);
            }
            wait();

            start(CallbackType.Asynchronous, TestType.JsonGetNodeCache);
            AutoCSer.CacheServer.DataStructure.Parameter.QueryReturnValue<Json<int>> getNode = dictionary.GetNode(count - 1);
            for (int index = count; index != 0;)
            {
                --index;
                getNode.Query(getCallbackReturnParameter);
            }
            wait();

            start(CallbackType.Asynchronous, TestType.JsonGet);
            for (int index = count; index != 0; dictionary.Get(--index, getCallbackReturnParameter)) ;
            wait();

            dictionary.Clear();

            start(CallbackType.Synchronous, TestType.JsonSet);
            for (int index = count; index != 0;)
            {
                --index;
                setCallback(dictionary.Set(index, count - index));
            }
            wait();

            start(CallbackType.Synchronous, TestType.JsonGet);
            for (int index = count; index != 0;)
            {
                --index;
                getCallback(dictionary.Get(index));
            }
            wait();

            dictionary.Clear();
        }
    }
}
