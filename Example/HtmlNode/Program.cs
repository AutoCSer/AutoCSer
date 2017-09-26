using System;
using System.Threading;

namespace AutoCSer.Example.HtmlNode
{
    class Program
    {
        static void Main(string[] args)
        {
            using (System.Net.WebClient webClient = new System.Net.WebClient())
            {
                string html = System.Text.Encoding.UTF8.GetString(webClient.DownloadData(@"http://www.AutoCSer.com/Index.html"));
                AutoCSer.HtmlNode.Node htmlNode = new AutoCSer.HtmlNode.Node(html);

                string childPath = @"/html/head/title";//子节点标签名称筛选，多个标签名称使用逗号 , 分割，比如 /TAG1,TAG2
                Console.WriteLine(@"Filter / => " + childPath);
                foreach (AutoCSer.HtmlNode.Node node in AutoCSer.HtmlNode.Filter.Get(childPath).Get(htmlNode)) Console.WriteLine(node.Text);
                Console.WriteLine();

                string childIndexPath = @"/html/body/div[1]/h1";//带索引子节点标签名称筛选仅支持一个标签名称，多个索引使用逗号 , 分割，比如 /TAG[1,3]
                Console.WriteLine(@"Filter / => " + childIndexPath);
                foreach (AutoCSer.HtmlNode.Node node in AutoCSer.HtmlNode.Filter.Get(childIndexPath).Get(htmlNode)) Console.WriteLine(node.Text);
                Console.WriteLine();

                string classPath = @"/html/body/div[0]/p/b.AutoCSerClass";//class 样式筛选，多个样式使用逗号 , 分割，比如 .CLASS1,CLASS2
                Console.WriteLine(@"Filter . => " + classPath);
                foreach (AutoCSer.HtmlNode.Node node in AutoCSer.HtmlNode.Filter.Get(classPath).Get(htmlNode)) Console.WriteLine(node.Text);
                Console.WriteLine();

                string idPath = @"/html/body/div[0]/p/b#AutoCSerId";//id 判定，多个 id 使用逗号 , 分割，比如 #ID1,ID2
                Console.WriteLine(@"Filter # => " + idPath);
                foreach (AutoCSer.HtmlNode.Node node in AutoCSer.HtmlNode.Filter.Get(idPath).Get(htmlNode)) Console.WriteLine(node.Text);
                Console.WriteLine();

                string namePath = @"/html/body/div[0]/p/b*AutoCSerName";//name 判定，多个 name 使用逗号 , 分割，比如 *NAME1,NAME2
                Console.WriteLine(@"Filter * => " + namePath);
                foreach (AutoCSer.HtmlNode.Node node in AutoCSer.HtmlNode.Filter.Get(namePath).Get(htmlNode)) Console.WriteLine(node.Text);
                Console.WriteLine();

                string typePath = @"/html/body/div[0]/p/b:AutoCSerType";//type 判定，多个 type 使用逗号 , 分割，比如 :TYPE1,TYPE2
                Console.WriteLine(@"Filter : => " + typePath);
                foreach (AutoCSer.HtmlNode.Node node in AutoCSer.HtmlNode.Filter.Get(typePath).Get(htmlNode)) Console.WriteLine(node.Text);
                Console.WriteLine();

                string attributePath = @"/html/body/div[0]/p/b@id";//属性判定，比如 #NAME
                Console.WriteLine(@"Filter @ => " + attributePath);
                foreach (AutoCSer.HtmlNode.Node node in AutoCSer.HtmlNode.Filter.Get(attributePath).Get(htmlNode)) Console.WriteLine(node.Text);
                Console.WriteLine();

                string attributeValuePath = @"/html/body/div[0]/p/b@id=AutoCSerId";//属性值判定，多个 value 使用逗号 , 分割，比如 #NAME=VALUE1,VALUE1
                Console.WriteLine(@"Filter @ => " + attributeValuePath);
                foreach (AutoCSer.HtmlNode.Node node in AutoCSer.HtmlNode.Filter.Get(attributeValuePath).Get(htmlNode)) Console.WriteLine(node.Text);
                Console.WriteLine();

                string nodePath = @"\title";//所有子孙节点标签名称筛选，多个标签名称使用逗号 , 分割，比如 \TAG1,TAG2
                Console.WriteLine(@"Filter \ => " + nodePath);
                foreach (AutoCSer.HtmlNode.Node node in AutoCSer.HtmlNode.Filter.Get(nodePath).Get(htmlNode)) Console.WriteLine(node.Text);
                Console.WriteLine();

                string defaultNodePath = @"title";//所有子孙节点标签名称筛选，第一个字符 \ 可省略，比如 TAG1,TAG2
                Console.WriteLine(@"Filter => " + defaultNodePath);
                foreach (AutoCSer.HtmlNode.Node node in AutoCSer.HtmlNode.Filter.Get(defaultNodePath).Get(htmlNode)) Console.WriteLine(node.Text);
                Console.WriteLine();

                Console.WriteLine("Over");
                Console.ReadKey();
            }
        }
    }
}
