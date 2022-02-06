using System;
using System.Collections.Generic;

namespace AutoCSer.TestCase.WebPerformance
{
    /// <summary>
    /// 网站配置
    /// </summary>
    internal sealed class Config : AutoCSer.Configuration.Root
    {
        /// <summary>
        /// 主配置类型集合
        /// </summary>
        public override IEnumerable<Type> MainTypes { get { yield return typeof(Config); } }

        /// <summary>
        /// HTTP 配置
        /// </summary>
        [AutoCSer.Configuration.Member]
        public static AutoCSer.Net.Http.Config HttpConfig
        {
            get
            {
                return new AutoCSer.Net.Http.Config
                {
                    MaxHeaderCount = 0,
                    MaxQueryCount = 4,
                    HeadSize = AutoCSer.Memory.BufferSize.Kilobyte2,
                    BufferSize = AutoCSer.Memory.BufferSize.Kilobyte2,
                    IsResponseServer = false,
                    IsResponseCacheControl = false,
                    IsResponseContentType = false,
                    IsResponseDate = false,
                    IsResponseLastModified = false
                };
            }
        }
    }
}
