using System;
using AutoCSer.Extension;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定结果池的分词搜索器
    /// </summary>
    public unsafe abstract partial class StaticSearcher
    {
        /// <summary>
        /// 绑定静态节点池的字符串 Trie 图
        /// </summary>
        protected readonly StaticStringTrieGraph trieGraph;
        /// <summary>
        /// 结果访问锁
        /// </summary>
        protected readonly object resultLock = new object();
        /// <summary>
        /// 搜索选项
        /// </summary>
        protected readonly SearchFlags flags;
        /// <summary>
        /// 队列等待
        /// </summary>
        internal AutoCSer.Threading.AutoWaitHandle QueueWait;
        /// <summary>
        /// 队列线程等待
        /// </summary>
        internal AutoCSer.Threading.AutoWaitHandle QueueThreadWait;
        /// <summary>
        /// 初始化添加数据数量
        /// </summary>
        protected int initializeCount = 1;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        protected volatile int isDisposed;
        /// <summary>
        /// 绑定结果池的分词搜索器
        /// </summary>
        /// <param name="trieGraph">绑定静态节点池的字符串 Trie 图</param>
        /// <param name="flags">搜索选项</param>
        protected StaticSearcher(StaticStringTrieGraph trieGraph, SearchFlags flags)
        {
            this.trieGraph = trieGraph ?? StaticStringTrieGraph.Null;
            this.flags = flags;
            QueueWait.Set(0);
        }

        /// <summary>
        /// 合并中文与 Trie 图分词类型
        /// </summary>
        /// <param name="wordType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected static byte formatWordType(byte wordType)
        {
            if ((wordType & (byte)(WordType.Chinese | WordType.TrieGraph)) != 0) return wordType |= (byte)(WordType.Chinese | WordType.TrieGraph);
            if ((wordType & (byte)(WordType.Letter | WordType.Number | WordType.Keep)) != 0) return wordType |= (byte)(WordType.Letter | WordType.Number | WordType.Keep);
            return wordType;
        }
    }
    /// <summary>
    /// 绑定结果池的分词搜索器
    /// </summary>
    /// <typeparam name="keyType">数据标识类型</typeparam>
    public sealed partial class StaticSearcher<keyType> : StaticSearcher, IDisposable where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 关键字数据结果集合
        /// </summary>
        private readonly IResult results;
        /// <summary>
        /// 关键字数据结果集合
        /// </summary>
        public IResult Results
        {
            get { return results; }
        }
        /// <summary>
        /// 原始文本信息集合
        /// </summary>
        private readonly Dictionary<keyType, string> texts;
        /// <summary>
        /// 文本更新队列
        /// </summary>
        private SearchData.YieldQueue queue = new SearchData.YieldQueue(new SearchData());
        /// <summary>
        /// 分词查询
        /// </summary>
        private WordQuery query;
        /// <summary>
        /// 绑定结果池的分词搜索器
        /// </summary>
        /// <param name="trieGraph">绑定静态节点池的字符串 Trie 图</param>
        /// <param name="flags">搜索选项</param>
        /// <param name="results">关键字数据结果集合</param>
        public StaticSearcher(StaticStringTrieGraph trieGraph = null, SearchFlags flags = SearchFlags.ResultIndexs, IResult results = null)
            : base(trieGraph, flags)
        {
            if ((flags & SearchFlags.Text) != 0) texts = DictionaryCreator<keyType>.Create<string>();
            this.results = results ?? new DefaultResult(flags);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Monitor.Enter(resultLock);
            try
            {
                if (isDisposed == 0)
                {
                    isDisposed = 1;
                    QueueWait.Set();
                    QueueThreadWait.TryWait();
                    results.Dispose();
                }
            }
            finally { Monitor.Exit(resultLock); }
        }
        /// <summary>
        /// 获取初始化添加数据
        /// </summary>
        /// <returns>初始化添加数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public InitializeAdder GetInitializeAdder()
        {
            InitializeAdder value = new InitializeAdder(this);
            Interlocked.Increment(ref initializeCount);
            return value;
        }
        /// <summary>
        /// 释放初始化添加数据
        /// </summary>
        internal void FreeInitializeAdder()
        {
            if (Interlocked.Decrement(ref initializeCount) == 0)
            {
                Monitor.Enter(resultLock);
                try
                {
                    if (isDisposed == 0)
                    {
                        QueueThreadWait.Set(0);
                        AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(queueThread);
                    }
                }
                finally { Monitor.Exit(resultLock); }
            }
        }
        /// <summary>
        /// 初始化完毕
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Initialized()
        {
            FreeInitializeAdder();
        }
        /// <summary>
        /// 添加新的数据
        /// </summary>
        /// <param name="key">数据标识</param>
        /// <param name="text">原始文本信息</param>
        /// <param name="values">分词结果</param>
        private void initializeAdd(ref keyType key, string text, ReusableDictionary<HashString, ResultIndexLeftArray> values)
        {
            Monitor.Enter(resultLock);
            try
            {
                add(ref key, text, values);
            }
            finally { Monitor.Exit(resultLock); }
        }
        /// <summary>
        /// 添加新的数据
        /// </summary>
        /// <param name="key">数据标识</param>
        /// <param name="text">原始文本信息</param>
        /// <param name="values">分词结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void add(ref keyType key, string text, ReusableDictionary<HashString, ResultIndexLeftArray> values)
        {
            if (isDisposed == 0)
            {
                if ((flags & SearchFlags.Text) != 0) texts[key] = text;
                results.Add(ref key, values);
            }
        }
        /// <summary>
        /// 获取删除原始文本信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool getRemoveText(SearchData data)
        {
            if ((flags & SearchFlags.Text) != 0)
            {
                string text;
                if (texts.TryGetValue(data.Key, out text))
                {
                    texts.Remove(data.Key);
                    if (!string.IsNullOrEmpty(text))
                    {
                        data.Text = text;
                        return true;
                    }
                }
            }
            return !string.IsNullOrEmpty(data.Text);
        }
        /// <summary>
        /// 添加新的数据
        /// </summary>
        /// <param name="key">数据标识</param>
        /// <param name="values">分词结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void remove(ref keyType key, HashSet<HashString> values)
        {
            if ((flags & SearchFlags.Text) != 0) texts.Remove(key);
            if (values.Count != 0) results.Remove(ref key, values);
        }
        /// <summary>
        /// 添加新的数据
        /// </summary>
        /// <param name="key">数据标识</param>
        /// <param name="text">数据文本</param>
        public void Add(keyType key, string text)
        {
            if (isDisposed == 0 && !string.IsNullOrEmpty(text) && queue.IsPushHead(new SearchData { Text = text, Key = key })) QueueWait.Set();
        }
        /// <summary>
        /// 删除历史数据
        /// </summary>
        /// <param name="key">数据标识</param>
        /// <param name="text">数据文本</param>
        public void Remove(keyType key, string text = null)
        {
            if (isDisposed == 0 && queue.IsPushHead(new SearchData { Text = text, Key = key, IsRemove = true })) QueueWait.Set();
        }
        /// <summary>
        /// 删除历史数据
        /// </summary>
        /// <param name="key">数据标识</param>
        /// <param name="text">数据文本</param>
        /// <param name="oldText">数据文本</param>
        public void Update(keyType key, string text, string oldText = null)
        {
            if (isDisposed == 0)
            {
                if (queue.IsPushHead(new SearchData { Text = oldText, Key = key, IsRemove = true })) QueueWait.Set();
                if (!string.IsNullOrEmpty(text) && queue.IsPushHead(new SearchData { Text = text, Key = key })) QueueWait.Set();
            }
        }
        /// <summary>
        /// 队列处理线程
        /// </summary>
        private void queueThread()
        {
            try
            {
                using (Queue queue = new Queue(this))
                {
                    do
                    {
                        QueueWait.Wait();
                        if (isDisposed == 0) queue.Add(this.queue.GetClear());
                    }
                    while (isDisposed == 0);
                }
            }
            finally { QueueThreadWait.Set(); }
        }
        /// <summary>
        /// 获取分词结果
        /// </summary>
        /// <param name="text">分词文本</param>
        /// <returns>分词结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public LeftArray<KeyValue<HashString, QueryResult>> Search(string text)
        {
            LeftArray<KeyValue<HashString, QueryResult>> words = default(LeftArray<KeyValue<HashString, QueryResult>>);
            Search(text, ref words);
            return words;
        }
        /// <summary>
        /// 获取分词结果
        /// </summary>
        /// <param name="text">分词文本</param>
        /// <param name="result">分词结果</param>
        /// <param name="isAllMatch">是否要求关键字全匹配</param>
        public void Search(string text, ref LeftArray<KeyValue<HashString, QueryResult>> result, bool isAllMatch = false)
        {
            WordQuery query = Interlocked.Exchange(ref this.query, null) ?? new WordQuery(this);
            query.Get(text, isAllMatch, ref result);
            if (Interlocked.CompareExchange(ref this.query, query, null) == null)
            {
                if (isDisposed != 0 && (query = Interlocked.Exchange(ref this.query, null)) != null) query.Dispose();
            }
            else query.Dispose();
        }
        /// <summary>
        /// 获取分词结果
        /// </summary>
        /// <param name="text">分词文本</param>
        /// <returns>分词结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public LeftArray<KeyValue<HashString, QueryResult>> Search(ref Simplified text)
        {
            LeftArray<KeyValue<HashString, QueryResult>> words = default(LeftArray<KeyValue<HashString, QueryResult>>);
            Search(ref text, ref words);
            return words;
        }
        /// <summary>
        /// 获取分词结果
        /// </summary>
        /// <param name="text">分词文本</param>
        /// <param name="results">分词结果</param>
        /// <param name="isAllMatch">是否要求关键字全匹配</param>
        public void Search(ref Simplified text, ref LeftArray<KeyValue<HashString, QueryResult>> results, bool isAllMatch = false)
        {
            WordQuery query = Interlocked.Exchange(ref this.query, null) ?? new WordQuery(this);
            query.Get(ref text, isAllMatch, ref results);
            if (Interlocked.CompareExchange(ref this.query, query, null) == null)
            {
                if (isDisposed != 0 && (query = Interlocked.Exchange(ref this.query, null)) != null) query.Dispose();
            }
            else query.Dispose();
        }
        /// <summary>
        /// 获取文本的匹配索引位置
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="indexs">匹配索引位置</param>
        /// <param name="wordLength">分词长度</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns>匹配索引位置集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe KeyValue<int, int>[] FormatTextIndexs(ref SubString text, int[] indexs, int wordLength, int maxLength)
        {
            KeyValue<int, int>[] resultIndexs = null;
            FormatTextIndexs(ref text, indexs, wordLength, maxLength, ref resultIndexs);
            return resultIndexs;
        }
        /// <summary>
        /// 获取文本的匹配索引位置
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="indexs">匹配索引位置</param>
        /// <param name="wordLength">分词长度</param>
        /// <param name="maxLength">最大长度</param>
        /// <param name="resultIndexs">匹配索引位置集合</param>
        /// <returns>匹配文本段数量</returns>
        public unsafe int FormatTextIndexs(ref SubString text, int[] indexs, int wordLength, int maxLength, ref KeyValue<int, int>[] resultIndexs)
        {
            int endIndex = indexs[indexs.Length - 1] + wordLength;
            if (endIndex > maxLength)
            {
                int startIndex = text.Length - maxLength, index0 = indexs[0];
                if (startIndex > index0)
                {
                    fixed (char* textFixed = text.String)
                    {
                        char* textStart = textFixed + text.Start;
                        if ((startIndex = maxLength - (endIndex - index0)) < 0)
                        {
                            if (wordLength < maxLength)
                            {
                                if ((startIndex = index0 - ((maxLength - wordLength) >> 1)) > 0)
                                {
                                    if ((endIndex = startIndex + maxLength - text.Length) > 0) startIndex -= endIndex;
                                    byte* charTypeData = trieGraph.CharTypeData.Byte;
                                    char* wordStart = textStart + index0, start = textStart + startIndex;
                                    byte type = formatWordType(charTypeData[*(start - 1)]);
                                    while (start != wordStart && (type & formatWordType(charTypeData[*start])) != 0) ++start;
                                    if (start == wordStart)
                                    {
                                        if ((type & formatWordType(charTypeData[*wordStart])) == 0) startIndex = index0;
                                    }
                                    else startIndex = (int)(start - textStart);
                                    if ((endIndex = text.Length - startIndex) < maxLength) maxLength = endIndex;
                                }
                                else startIndex = 0;
                                text.Sub(startIndex, maxLength);
                                endIndex = startIndex + maxLength;
                                maxLength = 0;
                                foreach (int value in indexs)
                                {
                                    if (value < endIndex) ++maxLength;
                                    else
                                    {
                                        if (maxLength == 0)
                                        {
                                            if (resultIndexs == null) resultIndexs = NullValue<KeyValue<int, int>>.Array;
                                        }
                                        else
                                        {
                                            if (resultIndexs == null || resultIndexs.Length < maxLength) resultIndexs = new KeyValue<int, int>[maxLength];
                                            endIndex = 0;
                                            foreach (int index in indexs)
                                            {
                                                resultIndexs[endIndex].Set(index - startIndex, wordLength);
                                                if (++endIndex == maxLength) break;
                                            }
                                        }
                                        return maxLength;
                                    }
                                }
                                if (resultIndexs == null || resultIndexs.Length < indexs.Length) resultIndexs = new KeyValue<int, int>[indexs.Length];
                                endIndex = 0;
                                foreach (int index in indexs) resultIndexs[endIndex++].Set(index - startIndex, wordLength);
                                return indexs.Length;
                            }
                            text.Sub(index0, maxLength);
                            if (resultIndexs == null || resultIndexs.Length == 0) resultIndexs = new KeyValue<int, int>[1];
                            resultIndexs[0].Set(0, maxLength);
                            return 1;
                        }
                        else
                        {
                            startIndex = index0 - (startIndex >> 1);
                            if ((endIndex = startIndex + maxLength - text.Length) > 0) startIndex -= endIndex;
                            byte* charTypeData = trieGraph.CharTypeData.Byte;
                            char* wordStart = textStart + index0, start = textStart + startIndex;
                            byte type = formatWordType(charTypeData[*(start - 1)]);
                            while (start != wordStart && (type & formatWordType(charTypeData[*start])) != 0) ++start;
                            if (start == wordStart)
                            {
                                if ((type & formatWordType(charTypeData[*wordStart])) != 0) start = textStart + startIndex;
                                else startIndex = index0;
                            }
                            else startIndex = (int)(start - textStart);
                            text.Sub((int)(start - textStart), Math.Min(maxLength, text.Length - startIndex));
                            if (resultIndexs == null || resultIndexs.Length < indexs.Length) resultIndexs = new KeyValue<int, int>[indexs.Length];
                            endIndex = 0;
                            foreach (int index in indexs) resultIndexs[endIndex++].Set(index - startIndex, wordLength);
                            return indexs.Length;
                        }
                    }
                }
                text.Sub(startIndex);
                if (resultIndexs == null || resultIndexs.Length < indexs.Length) resultIndexs = new KeyValue<int, int>[indexs.Length];
                endIndex = 0;
                foreach (int index in indexs) resultIndexs[endIndex++].Set(index - startIndex, wordLength);
                return indexs.Length;
            }
            if (text.Length > maxLength) text.Sub(0, maxLength);
            if (resultIndexs == null || resultIndexs.Length < indexs.Length) resultIndexs = new KeyValue<int, int>[indexs.Length];
            endIndex = 0;
            foreach (int index in indexs) resultIndexs[endIndex++].Set(index, wordLength);
            return indexs.Length;
        }
        /// <summary>
        /// 数据权重计算
        /// </summary>
        /// <param name="results">分词结果</param>
        /// <param name="isKey">数据标识匹配</param>
        /// <param name="getWeight">权重计算接口</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReusableDictionary<keyType, int> GetWeights(LeftArray<KeyValue<HashString, QueryResult>> results, Func<keyType, bool> isKey = null, IWeight getWeight = null)
        {
            return GetWeights(ref results, isKey, getWeight);
        }
        /// <summary>
        /// 数据权重计算
        /// </summary>
        /// <param name="results">分词结果</param>
        /// <param name="isKey">数据标识匹配</param>
        /// <param name="getWeight">权重计算接口</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReusableDictionary<keyType, int> GetWeights(ref LeftArray<KeyValue<HashString, QueryResult>> results, Func<keyType, bool> isKey = null, IWeight getWeight = null)
        {
            ReusableDictionary<keyType, int> weights = ReusableDictionary<keyType>.Create<int>();
            GetWeights(ref results, weights, isKey, getWeight);
            return weights;
        }
        /// <summary>
        /// 数据权重计算
        /// </summary>
        /// <param name="results">分词结果</param>
        /// <param name="weights">权重计算结果</param>
        /// <param name="isKey">数据标识匹配</param>
        /// <param name="getWeight">权重计算接口</param>
        public void GetWeights(ref LeftArray<KeyValue<HashString, QueryResult>> results, ReusableDictionary<keyType, int> weights, Func<keyType, bool> isKey = null, IWeight getWeight = null)
        {
            int count = results.Length;
            if (count != 0)
            {
                if (getWeight == null) getWeight = Weight.Default;
                foreach (KeyValue<HashString, QueryResult> result in results.Array)
                {
                    int wordCount = result.Value.WordCount, weight = (int)getWeight.GetWordCount(Results.WordCount, wordCount) * (int)getWeight.GetWordType(result.Value.WordType, result.Key.String.Length), oldWeight;
                    foreach (KeyValuePair<keyType, ResultIndexArray> indexs in result.Value.Dictionary)
                    {
                        if (isKey == null || isKey(indexs.Key))
                        {
                            int indexWeight = weight * getWeight.GetIndexCount(wordCount, indexs.Value.Indexs.Length);
                            if (weights.TryGetValue(indexs.Key, out oldWeight)) weights.Set(indexs.Key, oldWeight + indexWeight);
                            else weights.Set(indexs.Key, indexWeight);
                        }
                    }
                    if (--count == 0) break;
                }
            }
        }
        /// <summary>
        /// 获取文本的匹配索引位置
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="textLength">数据文本长度</param>
        /// <param name="results">搜索结果</param>
        /// <returns>匹配索引位置集合</returns>
        public unsafe KeyValue<int, int>[] GetResultIndexs(ref keyType key, int textLength, ref LeftArray<KeyValue<HashString, QueryResult>> results)
        {
            KeyValue<int, int>[] indexs = null;
            Pointer.Size mapBuffer = Unmanaged.GetSize64((textLength + 7) >> 3, true);
            try
            {
                GetResultIndexs(ref key, textLength, ref results, ref indexs, mapBuffer.Data);
            }
            finally { Unmanaged.Free(ref mapBuffer); }
            return indexs;
        }
        /// <summary>
        /// 获取文本的匹配索引位置
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="textLength">数据文本长度</param>
        /// <param name="results">搜索结果</param>
        /// <param name="indexs">匹配索引位置集合</param>
        /// <param name="mapData">文本匹配位图</param>
        /// <returns>匹配文本段数量</returns>
        internal unsafe int GetResultIndexs(ref keyType key, int textLength, ref LeftArray<KeyValue<HashString, QueryResult>> results, ref KeyValue<int, int>[] indexs, void* mapData)
        {
            MemoryMap map = new MemoryMap(mapData);
            int count = results.Length;
            ResultIndexArray indexArray;
            foreach (KeyValue<HashString, QueryResult> result in results.Array)
            {
                int wordLenght = result.Key.String.Length;
                if (result.Value.Dictionary.TryGetValue(key, out indexArray))
                {
                    if (wordLenght > 1)
                    {
                        foreach (int index in indexArray.Indexs)
                        {
                            if (index + wordLenght <= textLength) map.Set(index, wordLenght);
                        }
                    }
                    else
                    {
                        foreach (int index in indexArray.Indexs)
                        {
                            if (index < textLength) map.Set(index);
                        }
                    }
                }
                if (--count == 0) break;
            }
            return getResultIndexs(map, textLength, ref indexs);
        }
        /// <summary>
        /// 获取文本的匹配索引位置
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="text">数据文本</param>
        /// <param name="results">搜索结果</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns>匹配索引位置集合</returns>
        public unsafe KeyValue<int, int>[] FormatTextIndexs(ref keyType key, ref SubString text, ref LeftArray<KeyValue<HashString, QueryResult>> results, int maxLength)
        {
            KeyValue<int, int>[] indexs = null;
            Pointer.Size mapBuffer = Unmanaged.GetSize64((text.Length + 7) >> 3);
            try
            {
                FormatTextIndexs(ref key, ref text, ref results, maxLength, ref indexs, ref mapBuffer);
            }
            finally { Unmanaged.Free(ref mapBuffer); }
            return indexs;
        }
        /// <summary>
        /// 获取文本的匹配索引位置
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="text">数据文本</param>
        /// <param name="results">搜索结果</param>
        /// <param name="maxLength">最大长度</param>
        /// <param name="indexs">匹配索引位置集合</param>
        /// <param name="mapBuffer">文本匹配位图缓冲区</param>
        /// <returns>匹配文本段数量</returns>
        internal unsafe int FormatTextIndexs(ref keyType key, ref SubString text, ref LeftArray<KeyValue<HashString, QueryResult>> results, int maxLength, ref KeyValue<int, int>[] indexs, ref Pointer.Size mapBuffer)
        {
            if (text.Length <= maxLength)
            {
                Memory.ClearUnsafe(mapBuffer.ULong, (text.Length + 63) >> 6);
                return GetResultIndexs(ref key, text.Length, ref results, ref indexs, mapBuffer.Data);
            }
            int count = results.Length, index0 = int.MaxValue, endIndex = 0;
            ResultIndexArray indexArray;
            foreach (KeyValue<HashString, QueryResult> result in results.Array)
            {
                if (result.Value.Dictionary.TryGetValue(key, out indexArray))
                {
                    int index = indexArray.Indexs[0];
                    if (index < index0 || (index == index0 && index + result.Key.String.Length > endIndex))
                    {
                        index0 = index;
                        endIndex = index + result.Key.String.Length;
                    }
                }
                if (--count == 0) break;
            }
            int wordLength = maxLength - (endIndex - index0);
            if (wordLength > 0)
            {
                int startIndex = index0 - (wordLength >> 1);
                if (startIndex <= 0) text.Sub(startIndex = 0, maxLength);
                else if ((endIndex = startIndex + maxLength - text.Length) >= 0) text.Sub(startIndex -= endIndex, maxLength);
                else
                {
                    byte* charTypeData = trieGraph.CharTypeData.Byte;
                    fixed (char* textFixed = text.String)
                    {
                        char* textStart = textFixed, wordStart = textStart + index0, start = textStart + startIndex;
                        byte type = formatWordType(charTypeData[*(start - 1)]);
                        while (start != wordStart && (type & formatWordType(charTypeData[*start])) != 0) ++start;
                        if (start == wordStart)
                        {
                            if ((type & formatWordType(charTypeData[*wordStart])) == 0) startIndex = index0;
                        }
                        else startIndex = (int)(start - textStart);
                    }
                    text.Sub(startIndex, Math.Min(maxLength, text.Length - startIndex));
                }

                Memory.ClearUnsafe(mapBuffer.ULong, (text.Length + 63) >> 6);
                MemoryMap map = new MemoryMap(mapBuffer.Data);
                endIndex = startIndex + maxLength;
                count = results.Length;
                foreach (KeyValue<HashString, QueryResult> result in results.Array)
                {
                    int wordLenght = result.Key.String.Length;
                    if (result.Value.Dictionary.TryGetValue(key, out indexArray))
                    {
                        if (wordLenght > 1)
                        {
                            foreach (int index in indexArray.Indexs)
                            {
                                if (index >= startIndex)
                                {
                                    if (index + wordLenght <= endIndex) map.Set(index - startIndex, wordLenght);
                                    else break;
                                }
                            }
                        }
                        else
                        {
                            foreach (int index in indexArray.Indexs)
                            {
                                if (index >= startIndex)
                                {
                                    if (index < endIndex) map.Set(index - startIndex);
                                    else break;
                                }
                            }
                        }
                    }
                    if (--count == 0) break;
                }
                return getResultIndexs(map, maxLength, ref indexs);
            }
            else
            {
                text.Sub(index0, maxLength);
                if (indexs == null || indexs.Length == 0) indexs = new KeyValue<int, int>[1];
                indexs[0].Set(0, maxLength);
                return 1;
            }
        }
        /// <summary>
        /// 获取文本的匹配索引位置
        /// </summary>
        /// <param name="map">匹配位图</param>
        /// <param name="textLength">文本长度</param>
        /// <param name="indexs">匹配索引位置集合</param>
        /// <returns>匹配文本段数量</returns>
        private static int getResultIndexs(MemoryMap map, int textLength, ref KeyValue<int, int>[] indexs)
        {
            int matchMap = 0, count = 0, startIndex = 0;
            for (int index = 0; index != textLength; ++index)
            {
                if (matchMap == 0)
                {
                    matchMap = map.Get(index);
                    if (matchMap != 0) ++count;
                }
                else matchMap = map.Get(index);
            }
            if (indexs == null || indexs.Length < count) indexs = new KeyValue<int, int>[count];
            int write = matchMap = 0;
            for (int index = 0; index != textLength; ++index)
            {
                if (matchMap == 0)
                {
                    if ((matchMap = map.Get(index)) != 0) startIndex = index;
                }
                else if ((matchMap = map.Get(index)) == 0) indexs[write++].Set(startIndex, index - startIndex);
            }
            if (write != count) indexs[write].Set(startIndex, textLength - startIndex);
            return count;
        }

        /// <summary>
        /// 结果池当前分配数组
        /// </summary>
        private static ArrayPool<WordCounter> counterPool = new ArrayPool<WordCounter>(256);
    }
}
