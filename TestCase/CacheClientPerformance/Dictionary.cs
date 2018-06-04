using System;
using AutoCSer.CacheServer.DataStructure;
using AutoCSer.CacheServer.DataStructure.Value;

namespace AutoCSer.TestCase.CacheClientPerformance
{
    /// <summary>
    /// 简单字典测试
    /// </summary>
    sealed class Dictionary : Test
    {
        /// <summary>
        /// 测试回调
        /// </summary>
        private readonly Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> getCallbackReturnParameter;
        /// <summary>
        /// 测试字典节点
        /// </summary>
        private readonly ValueDictionary<int, int> dictionary;
        /// <summary>
        /// 简单数组测试
        /// </summary>
        /// <param name="client">测试客户端</param>
        /// <param name="isFile">服务端是否文件持久化</param>
        internal Dictionary(AutoCSer.CacheServer.Client client, bool isFile) : base(client, isFile)
        {
            getCallbackReturnParameter = AutoCSer.CacheServer.ReturnParameter.GetCallback<int>(getCallback);
            dictionary = client.GetOrCreateDataStructure<ValueDictionary<int, int>>("Dictionary");
        }
        /// <summary>
        /// 测试
        /// </summary>
        internal void Test()
        {
            dictionary.Clear();

            start(CallbackType.Asynchronous, TestType.DictionarySetNodeCache);
            AutoCSer.CacheServer.DataStructure.Parameter.OperationBool setNode = dictionary.GetSetNode(count - 1, int.MaxValue - count);
            for (int index = count; index != 0;)
            {
                --index;
                setNode.Operation(setCallbackReturnParameter);
            }
            wait();

            dictionary.Clear();

            start(CallbackType.Asynchronous, TestType.DictionarySet);
            for (int index = count; index != 0;)
            {
                --index;
                dictionary.Set(index, count - index, setCallbackReturnParameter);
            }
            wait();

            start(CallbackType.Asynchronous, TestType.DictionaryGetNodeCache);
            AutoCSer.CacheServer.DataStructure.Parameter.QueryReturnValue<int> getNode = dictionary.GetNode(count - 1);
            for (int index = count; index != 0;)
            {
                --index;
                getNode.Query(getCallbackReturnParameter);
            }
            wait();

            start(CallbackType.Asynchronous, TestType.DictionaryGet);
            for (int index = count; index != 0; dictionary.Get(--index, getCallbackReturnParameter)) ;
            wait();


            start(CallbackType.Asynchronous, TestType.DictionaryRemove);
            for (int index = count; index != 0; dictionary.Remove(--index, setCallbackReturnParameter)) ;
            wait();

            dictionary.Clear();

            start(CallbackType.Synchronous, TestType.DictionarySet);
            for (int index = count; index != 0;)
            {
                --index;
                setCallback(dictionary.Set(index, count - index));
            }
            wait();

            start(CallbackType.Synchronous, TestType.DictionaryGet);
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
