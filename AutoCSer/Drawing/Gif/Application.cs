using System;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 应用程序扩展
    /// </summary>
    public sealed class Application
    {
        /// <summary>
        /// 用来鉴别应用程序自身的标识(8个连续ASCII字符)
        /// </summary>
        public SubArray<byte> Identifier { get; private set; }
        /// <summary>
        /// 应用程序定义的特殊标识码(3个连续ASCII字符)
        /// </summary>
        public SubArray<byte> AuthenticationCode { get; private set; }
        /// <summary>
        /// 应用程序自定义数据块集合
        /// </summary>
        private LeftArray<SubArray<byte>> customDatas;
        /// <summary>
        /// 应用程序自定义数据块
        /// </summary>
        public byte[] CustomData
        {
            get { return Decoder.BlocksToByte(ref customDatas); }
        }
        /// <summary>
        /// 应用程序扩展
        /// </summary>
        /// <param name="identifier">用来鉴别应用程序自身的标识(8个连续ASCII字符)</param>
        /// <param name="authenticationCode">应用程序定义的特殊标识码(3个连续ASCII字符)</param>
        /// <param name="customDatas">应用程序自定义数据块集合</param>
        internal Application(SubArray<byte> identifier, SubArray<byte> authenticationCode, LeftArray<SubArray<byte>> customDatas)
        {
            Identifier = identifier;
            AuthenticationCode = authenticationCode;
            this.customDatas = customDatas;
        }
    }
}
