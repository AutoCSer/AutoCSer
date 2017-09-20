using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Sql.LogStream
{
    /// <summary>
    /// 自增标识客户端
    /// </summary>
    public sealed partial class IdentityClient<valueType, modelType>
    {
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="identitys"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public async Task<valueType[]> GetAsync(AutoCSer.Net.TcpServer.ReturnValue<int[]> identitys)
        {
            if (identitys.Type == AutoCSer.Net.TcpServer.ReturnType.Success) return await GetAsync(identitys.Value);
            return new AutoCSer.Net.TcpServer.ReturnValue<valueType[]> { Type = identitys.Type };
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="identitys"></param>
        /// <returns></returns>
        public async Task<valueType[]> GetAsync(int[] identitys)
        {
            valueType[] values = new valueType[identitys.Length];
            int index = 0;
            foreach (int identity in identitys)
            {
                AutoCSer.Net.TcpServer.AwaiterBox<valueType> awaiter = Get(identity, ref values[index]);
                if (awaiter != null) values[index] = await awaiter;
                ++index;
            }
            return values;
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="identitysAwaiter"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public async Task<valueType[]> GetAsync(AutoCSer.Net.TcpServer.AwaiterBox<int[]> identitysAwaiter)
        {
            return await GetAsync(await identitysAwaiter);
        }
    }
}
