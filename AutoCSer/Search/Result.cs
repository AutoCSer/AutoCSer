using System;
using System.Collections.Generic;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定结果池的分词搜索器
    /// </summary>
    public sealed partial class StaticSearcher<keyType>
    {
        /// <summary>
        /// 关键字数据结果集合
        /// </summary>
        public interface IResult : IDisposable
        {
            /// <summary>
            /// 当前更新版本
            /// </summary>
            long Version { get; }
            /// <summary>
            /// 总词频
            /// </summary>
            long WordCount { get; }
            /// <summary>
            /// 添加新的数据
            /// </summary>
            /// <param name="key">数据标识</param>
            /// <param name="values">分词结果</param>
            /// <param name="text">原始文本信息</param>
            void Add(ref keyType key, ReusableDictionary<HashString, ResultIndexLeftArray> values, string text);
            /// <summary>
            /// 添加新的数据
            /// </summary>
            /// <param name="key">数据标识</param>
            /// <param name="values">分词结果</param>
            /// <param name="text">原始文本信息</param>
            void Remove(ref keyType key, HashSet<HashString> values, string text);
            /// <summary>
            /// 添加分词
            /// </summary>
            /// <param name="word">分词</param>
            /// <param name="result">结果</param>
            bool GetResult(ref HashString word, ref QueryResult result);
            /// <summary>
            /// 获取未匹配单个汉字结果集合
            /// </summary>
            /// <param name="lessWords"></param>
            /// <param name="lessChineseCharacters"></param>
            /// <returns></returns>
            bool GetLessChineseCharacter(ref LeftArray<SubString> lessWords, ref LeftArray<HashSet<keyType>> lessChineseCharacters);
            /// <summary>
            /// 获取单个汉字结果
            /// </summary>
            /// <param name="chineseCharacter"></param>
            /// <returns></returns>
            HashSet<keyType> GetChineseCharacter(char chineseCharacter);
        }
        /// <summary>
        /// 关键字数据结果集合
        /// </summary>
        private unsafe sealed class DefaultResult : IResult
        {
            /// <summary>
            /// 关键字数据结果集合
            /// </summary>
            private readonly ReusableDictionary<HashString, WordCounterIndex> results = ReusableDictionary.CreateHashString<WordCounterIndex>();
            /// <summary>
            /// 分词字符类型集合
            /// </summary>
            private readonly byte* charTypeData;
            /// <summary>
            /// 数据结果
            /// </summary>
            private readonly HashSet<keyType>[] chineseCharacters;
            /// <summary>
            /// 当前更新版本
            /// </summary>
            public long Version { get; private set; }
            /// <summary>
            /// 总词频
            /// </summary>
            public long WordCount { get; private set; }
            /// <summary>
            /// 搜索选项
            /// </summary>
            private readonly SearchFlags flags;
            /// <summary>
            /// 关键字数据结果集合
            /// </summary>
            /// <param name="flags">搜索选项</param>
            /// <param name="trieGraph"></param>
            internal DefaultResult(SearchFlags flags, StaticStringTrieGraph trieGraph)
            {
                this.flags = flags;
                if ((flags & SearchFlags.ChineseCharacter) != 0)
                {
                    charTypeData = trieGraph.CharTypeData.Byte;
                    chineseCharacters = new HashSet<keyType>[1 << 16];
                }
            }
            /// <summary>
            /// 释放结果
            /// </summary>
            public void Dispose()
            {
                counterPool.Free(results.Values, results.Count);
                results.Empty();
            }
            /// <summary>
            /// 添加新的数据
            /// </summary>
            /// <param name="key">数据标识</param>
            /// <param name="values">分词结果</param>
            /// <param name="text">原始文本信息</param>
            public void Add(ref keyType key, ReusableDictionary<HashString, ResultIndexLeftArray> values, string text)
            {
                WordCounterIndex counterIndex;
                ResultIndexArray resultIndex = default(ResultIndexArray);
                foreach (KeyValue<HashString, ResultIndexLeftArray> result in values.KeyValues)
                {
                    if (!results.TryGetValue(result.Key, out counterIndex))
                    {
                        counterIndex.Index = counterPool.Get(out counterIndex.Array);
                        results.Set(result.Key, counterIndex);
                        counterIndex.CreateResult();
                    }
                    if ((flags & SearchFlags.ResultIndexs) == 0)
                    {
                        resultIndex.Set(EmptyArray<int>.Array, result.Value.TextLength);
                        counterIndex.Add(key, result.Value.WordType, ref resultIndex);
                        ++WordCount;
                    }
                    else
                    {
                        resultIndex.Set(result.Value.Indexs.GetArray(), result.Value.TextLength);
                        counterIndex.Add(key, result.Value.WordType, ref resultIndex);
                        WordCount += result.Value.Indexs.Length;
                    }
                }
                if (chineseCharacters != null)
                {
                    foreach (char chineseCharacter in text)
                    {
                        if ((charTypeData[chineseCharacter] & (byte)WordType.Chinese) != 0)
                        {
                            HashSet<keyType> keyTypes = chineseCharacters[chineseCharacter];
                            if (keyTypes == null) chineseCharacters[chineseCharacter] = keyTypes = HashSetCreator<keyType>.Create();
                            keyTypes.Add(key);
                        }
                    }
                }
                ++Version;
            }
            /// <summary>
            /// 添加新的数据
            /// </summary>
            /// <param name="key">数据标识</param>
            /// <param name="values">分词结果</param>
            /// <param name="text">原始文本信息</param>
            public void Remove(ref keyType key, HashSet<HashString> values, string text)
            {
                WordCounterIndex counterIndex;
                foreach (HashString result in values)
                {
                    if (results.TryGetValue(result, out counterIndex)) WordCount -= counterIndex.Remove(key);
                }
                if (chineseCharacters != null)
                {
                    foreach (char chineseCharacter in text)
                    {
                        HashSet<keyType> keyTypes = chineseCharacters[chineseCharacter];
                        if (keyTypes != null) keyTypes.Remove(key);
                    }
                }
                ++Version;
            }
            /// <summary>
            /// 添加分词
            /// </summary>
            /// <param name="word">分词</param>
            /// <param name="result">结果</param>
            public bool GetResult(ref HashString word, ref QueryResult result)
            {
                WordCounterIndex counterIndex;
                if (results.TryGetValue(ref word, out counterIndex))
                {
                    counterIndex.SetResult(ref result);
                    return true;
                }
                return false;
            }
            /// <summary>
            /// 获取未匹配单个汉字结果集合
            /// </summary>
            /// <param name="lessWords"></param>
            /// <param name="lessChineseCharacters"></param>
            /// <returns></returns>
            public bool GetLessChineseCharacter(ref LeftArray<SubString> lessWords, ref LeftArray<HashSet<keyType>> lessChineseCharacters)
            {
                lessChineseCharacters.Length = 0;
                foreach (SubString Word in lessWords)
                {
                    if (Word.Count != 1) return false;
                    HashSet<keyType> result = chineseCharacters[Word[0]];
                    if (result == null) return false;
                    lessChineseCharacters.Add(result);
                }
                return true;
            }
            /// <summary>
            /// 获取单个汉字结果
            /// </summary>
            /// <param name="chineseCharacter"></param>
            /// <returns></returns>
            public HashSet<keyType> GetChineseCharacter(char chineseCharacter)
            {
                return chineseCharacters[chineseCharacter];
            }
        }
    }
}
