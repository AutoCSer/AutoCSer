using System;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.Web.SearchServer
{
    class Program : AutoCSer.Deploy.SwitchProcess
    {
        private Program(string[] args) : base(args) { }
        protected override void initialize()
        {
            foreach (KeyValue<HashString, AutoCSer.Search.StaticSearcher<DataKey>.QueryResult> result in Searcher.Default.Search("AutoCSer"))
            {
                Console.WriteLine(result.Key.ToString() + " " + result.Value.Count.ToString());
            }
        }
        protected override void onStart()
        {
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(() => AutoCSer.Web.Config.Pub.ConsoleCommand(ExitEvent));
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(Server.CreateSearchServer);
        }
        static void Main(string[] args)
        {
            new Program(args).Start();
        }
    }
}
