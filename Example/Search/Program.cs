using System;
using AutoCSer.Extension;
using System.Collections.Generic;

namespace AutoCSer.Example.Search
{
    class Program
    {
        static void Main(string[] args)
        {
            User[] users = new User[]
            {
                new User { Id = 1, Name = @"AutoCSer", Remark = @"现在的努力是为了将来能够吹牛B" }
                , new User { Id = 2, Name = @"张三", Remark = @"现在的努力是为了曾经吹过的牛B" }
                , new User { Id = 3, Name = @"李四", Remark = @"现在吹下的牛b是将来努力的动力" }
            };

            using (AutoCSer.Search.StaticSearcher<SearchKey> searcher = new AutoCSer.Search.StaticSearcher<SearchKey>())//没有词库
            {
                using (AutoCSer.Search.StaticSearcher<SearchKey>.InitializeAdder initializeAdder = searcher.GetInitializeAdder())
                {//支持多个 InitializeAdder，可用于多线程并行初始化数据
                    initializeAdder.Add(users.getArray(value => new KeyValue<SearchKey, string>(new SearchKey { Type = SearchType.UserName, Id = value.Id }, value.Name)));
                    initializeAdder.Add(users.getArray(value => new KeyValue<SearchKey, string>(new SearchKey { Type = SearchType.UserRemark, Id = value.Id }, value.Remark)));
                }
                searcher.Initialized();

                foreach (KeyValue<HashString, AutoCSer.Search.StaticSearcher<SearchKey>.QueryResult> result in searcher.Search(@"张三丰偷学AutoCSer以后不再吹牛B了"))
                {
                    SubString SubString = result.Key;
                    Console.WriteLine(result.Value.WordType.ToString() + " " + SubString.ToString() + "[" + SubString.StartIndex.toString() + "]");
                    foreach (KeyValuePair<SearchKey, AutoCSer.Search.ResultIndexArray> indexs in result.Value.Result)
                    {
                        Console.WriteLine(" " + indexs.Key.Type.ToString() + "[Id = " + indexs.Key.Id.toString() + "] " + indexs.Value.Indexs.joinString(',', index => index.toString()));
                    }
                }
            }
            Console.WriteLine();

            using (AutoCSer.Search.StringTrieGraph trieGraph = new AutoCSer.Search.StringTrieGraph(new string[] { "牛B" }.getArray(value => AutoCSer.Search.Simplified.Format(value))))
            using (AutoCSer.Search.StaticStringTrieGraph staticTrieGraph = trieGraph.CreateStaticGraph(false))
            using (AutoCSer.Search.StaticSearcher<SearchKey> searcher = new AutoCSer.Search.StaticSearcher<SearchKey>(staticTrieGraph))
            {
                using (AutoCSer.Search.StaticSearcher<SearchKey>.InitializeAdder initializeAdder = searcher.GetInitializeAdder())
                {
                    initializeAdder.Add(users.getArray(value => new KeyValue<SearchKey, string>(new SearchKey { Type = SearchType.UserName, Id = value.Id }, value.Name)));
                    initializeAdder.Add(users.getArray(value => new KeyValue<SearchKey, string>(new SearchKey { Type = SearchType.UserRemark, Id = value.Id }, value.Remark)));
                }
                searcher.Initialized();

                foreach (KeyValue<HashString, AutoCSer.Search.StaticSearcher<SearchKey>.QueryResult> result in searcher.Search(@"张三丰偷学AutoCSer以后不再吹牛B了"))
                {
                    SubString SubString = result.Key;
                    Console.WriteLine(result.Value.WordType.ToString() + " " + SubString.ToString() + "[" + SubString.StartIndex.toString() + "]");
                    foreach (KeyValuePair<SearchKey, AutoCSer.Search.ResultIndexArray> indexs in result.Value.Result)
                    {
                        Console.WriteLine(" " + indexs.Key.Type.ToString() + "[Id = " + indexs.Key.Id.toString() + "] " + indexs.Value.Indexs.joinString(',', index => index.toString()));
                    }
                }
            }
            Console.WriteLine();

            using (AutoCSer.Search.StringTrieGraph trieGraph = new AutoCSer.Search.StringTrieGraph(new string[] { "牛b", "张三丰" }.getArray(value => AutoCSer.Search.Simplified.Format(value))))
            using (AutoCSer.Search.StaticStringTrieGraph staticTrieGraph = trieGraph.CreateStaticGraph(false))
            using (AutoCSer.Search.StaticSearcher<SearchKey> searcher = new AutoCSer.Search.StaticSearcher<SearchKey>(staticTrieGraph))
            {
                using (AutoCSer.Search.StaticSearcher<SearchKey>.InitializeAdder initializeAdder = searcher.GetInitializeAdder())
                {
                    initializeAdder.Add(users.getArray(value => new KeyValue<SearchKey, string>(new SearchKey { Type = SearchType.UserName, Id = value.Id }, value.Name)));
                    initializeAdder.Add(users.getArray(value => new KeyValue<SearchKey, string>(new SearchKey { Type = SearchType.UserRemark, Id = value.Id }, value.Remark)));
                }
                searcher.Initialized();

                foreach (KeyValue<HashString, AutoCSer.Search.StaticSearcher<SearchKey>.QueryResult> result in searcher.Search(@"张三丰偷学AutoCSer以后不再吹牛B了"))
                {
                    SubString SubString = result.Key;
                    Console.WriteLine(result.Value.WordType.ToString() + " " + SubString.ToString() + "[" + SubString.StartIndex.toString() + "]");
                    foreach (KeyValuePair<SearchKey, AutoCSer.Search.ResultIndexArray> indexs in result.Value.Result)
                    {
                        Console.WriteLine(" " + indexs.Key.Type.ToString() + "[Id = " + indexs.Key.Id.toString() + "] " + indexs.Value.Indexs.joinString(',', index => index.toString()));
                    }
                }
            }
            Console.WriteLine();

            Console.WriteLine("Over");
            Console.ReadKey();
        }
    }
}
