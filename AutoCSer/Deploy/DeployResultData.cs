using System;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 部署任务返回结果
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public struct DeployResultData
    {
        /// <summary>
        /// 部署状态
        /// </summary>
        public DeployState State;
        /// <summary>
        /// 返回数据
        /// </summary>
        public byte[] Data;
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="state">部署状态</param>
        public static implicit operator DeployResultData(DeployState state) { return new DeployResultData { State = state }; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="data">返回数据</param>
        public static implicit operator DeployResultData(byte[] data) { return new DeployResultData { State = DeployState.Success, Data = data }; }
    }
}
