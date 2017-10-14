using System;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.OpenAPI._51Nod
{
    /// <summary>
    /// API调用
    /// </summary>
    public sealed class API
    {
        /// <summary>
        /// 当前令牌超时
        /// </summary>
        private DateTime timeout;
        /// <summary>
        /// 当前令牌
        /// </summary>
        private string token;
        /// <summary>
        /// 应用配置
        /// </summary>
        private readonly Config config;
        /// <summary>
        /// 令牌访问锁
        /// </summary>
        private readonly object tokenLock = new object();
        /// <summary>
        /// API调用
        /// </summary>
        /// <param name="config">应用配置</param>
        public API(Config config = null)
        {
            this.config = config ?? Config.Default;
        }
        /// <summary>
        /// 获取访问令牌
        /// </summary>
        /// <returns></returns>
        private string getToken()
        {
            DateTime now = Date.NowTime.UtcNow;
            string value;
            Monitor.Enter(tokenLock);
            if (timeout > now)
            {
                value = token;
                Monitor.Exit(tokenLock);
                return value;
            }
            long timeoutTicks;
            try
            {
                if ((value = config.GetToken(out timeoutTicks)) != null)
                {
                    token = value;
                    timeout = new DateTime(timeoutTicks, DateTimeKind.Utc);
                }
            }
            finally { Monitor.Exit(tokenLock); }
            return value;
        }
        /// <summary>
        /// 添加题目
        /// </summary>
        /// <param name="problem">题目</param>
        /// <returns>题目ID</returns>
        public int AppendProblem(Problem problem)
        {
            if (problem != null)
            {
                string token = getToken();
                if (token != null)
                {
                    ReturnValue<int> value = Config.Client.RequestJson<ReturnValue<int>, ProblemQuery>(config.Domain + "ajax?n=api.problem.Append", new ProblemQuery { token = token, problem = problem });
                    if (value != null) return value.Value;
                }
            }
            return 0;
        }
        /// <summary>
        /// 修改题目
        /// </summary>
        /// <param name="problem">题目</param>
        /// <returns>是否成功</returns>
        public bool ReworkProblem(Problem problem)
        {
            if (problem != null && problem.Id != 0)
            {
                string token = getToken();
                if (token != null)
                {
                    ReturnValue value = Config.Client.RequestJson<ReturnValue, ProblemQuery>(config.Domain + "ajax?n=api.problem.Rework", new ProblemQuery { token = token, problem = problem });
                    return value != null && value.IsReturn;
                }
            }
            return false;
        }
        /// <summary>
        /// Zip文件模式修改或者添加测试数据 参数名称
        /// </summary>
        private static readonly byte[] uploadTestDataParameterName = new byte[] { (byte)'j' };
        /// <summary>
        /// Zip文件模式修改或者添加测试数据
        /// </summary>
        /// <param name="problemId">题目ID</param>
        /// <param name="zipFileData">zip文件内容</param>
        /// <returns>上传后的测试数据数量</returns>
        public int UploadTestData(int problemId, byte[] zipFileData)
        {
            if (problemId != 0 && zipFileData.length() != 0)
            {
                string token = getToken();
                if (token != null)
                {
                    ReturnValue<int> value = Config.Client.RequestJson<ReturnValue<int>>(config.Domain + "upload/ReworkTestData", zipFileData, "file.zip", null, new KeyValue<byte[], byte[]>[] { new KeyValue<byte[], byte[]>(uploadTestDataParameterName, AutoCSer.Json.Serializer.Serialize(new UploadTestDataQuery { token = token, problemId = problemId }).Json.getBytes()) });
                    if (value != null) return value.Value;
                }
            }
            return 0;
        }
        /// <summary>
        /// 删除测试数据
        /// </summary>
        /// <param name="problemId">题目ID</param>
        /// <param name="testId">测试数据ID</param>
        /// <returns></returns>
        public bool DeleteTestData(int problemId, byte testId)
        {
            if (problemId != 0 && testId != 0)
            {
                string token = getToken();
                if (token != null)
                {
                    ReturnValue value = Config.Client.RequestJson<ReturnValue, DeleteTestDataQuery>(config.Domain + "ajax?n=api.problem.DeleteTestData", new DeleteTestDataQuery { token = token, problemId = problemId, testId = testId });
                    return value != null && value.IsReturn;
                }
            }
            return false;
        }
        /// <summary>
        /// 提交测试
        /// </summary>
        /// <param name="judge">提交测试</param>
        /// <returns></returns>
        public bool Judge(Judge judge)
        {
            if (judge != null)
            {
                string token = getToken();
                if (token != null)
                {
                    ReturnValue value = Config.Client.RequestJson<ReturnValue, JudgeQuery>(config.Domain + "ajax?n=api.judge.Open", new JudgeQuery { token = token, judge = judge });
                    return value != null && value.IsReturn;
                }
            }
            return false;
        }
        /// <summary>
        /// 批量提交测试
        /// </summary>
        /// <param name="judges">提交测试</param>
        /// <returns>成功提交的测试ID</returns>
        public int[] BatchJudge(Judge[] judges)
        {
            if (judges.length() != 0)
            {
                string token = getToken();
                if (token != null)
                {
                    ReturnValue<int[]> value = Config.Client.RequestJson<ReturnValue<int[]>, BatchJudgeQuery>(config.Domain + "/ajax?n=api.judge.Batch", new BatchJudgeQuery { token = token, judges = judges });
                    if (value != null) return value.Value;
                }
            }
            return null;
        }
        /// <summary>
        /// Judge判题回调解析
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public JudgeResult[] JudgeCallback(byte[] data)
        {
            return data.length() != 0 ? JudgeCallback(System.Text.Encoding.UTF8.GetString(data)) : null;
        }
        /// <summary>
        /// Judge判题回调解析
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public JudgeResult[] JudgeCallback(string json)
        {
            Callback<JudgeResult[]> value = AutoCSer.Json.Parser.Parse<Callback<JudgeResult[]>>(json);
            return value.Type == CallbackType.OpenJudge ? value.Value : null;
        }
    }
}
