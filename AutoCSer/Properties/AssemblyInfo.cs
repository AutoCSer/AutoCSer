using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// 有关程序集的常规信息通过以下
// 特性集控制。更改这些特性值可修改
// 与程序集关联的信息。
#if !DotNetStandard
[assembly: AssemblyTitle("AutoCSer")]
[assembly: AssemblyCopyright("Copyright © 肖进 2017")]
[assembly: AssemblyDescription("AutoCSer 是一个以高效率为目标向导的整体开发框架。主要包括 TCP / RPC 接口服务框架、TCP / RPC 函数服务框架、远程表达式链组件、前后端一体 WEB 视图框架、ORM 内存索引缓存框架、日志流内存数据库缓存组件、二进制 / JSON / XML 数据序列化 等一系列无缝集成的高性能组件。")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("AutoCSer")]
[assembly: AssemblyCompany("")]

// 程序集的版本信息由下面四个值组成:
//
//      主版本
//      次版本 
//      内部版本号
//      修订号
//
// 可以指定所有这些值，也可以使用“内部版本号”和“修订号”的默认值，
// 方法是按如下所示使用“*”:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.1.0.0")]
[assembly: AssemblyFileVersion("1.1.0.0")]
#endif
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 使此程序集中的类型
// 对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型，
// 则将该类型上的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
#if AutoCSer
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e0000")]
#elif AllSerialize
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e0004")]
#elif BinarySerialize
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e0005")]
#elif Json
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e0006")]
#elif Xml
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e0007")]
#endif

[assembly: InternalsVisibleTo("AutoCSer.DynamicAssembly")]
[assembly: InternalsVisibleTo("AutoCSer.TcpServer.Emit")]
[assembly: InternalsVisibleTo("AutoCSer.CodeGenerator")]
[assembly: InternalsVisibleTo("AutoCSer.TcpRegister")]
[assembly: InternalsVisibleTo("AutoCSer.CacheServer")]
[assembly: InternalsVisibleTo("AutoCSer.Sort")]
[assembly: InternalsVisibleTo("AutoCSer.WebView")]
[assembly: InternalsVisibleTo("AutoCSer.Sql")]
[assembly: InternalsVisibleTo("AutoCSer.DiskBlock")]
//[assembly: InternalsVisibleTo("AutoCSer.RemoteDictionaryStreamServer")]
[assembly: InternalsVisibleTo("AutoCSer.Deploy")]
[assembly: InternalsVisibleTo("AutoCSer.ProcessCopy")]
[assembly: InternalsVisibleTo("AutoCSer.TcpStreamServer")]
[assembly: InternalsVisibleTo("AutoCSer.TcpSimpleServer")]
[assembly: InternalsVisibleTo("AutoCSer.Drawing.Gif")]
[assembly: InternalsVisibleTo("AutoCSer.RandomObject")]
[assembly: InternalsVisibleTo("AutoCSer.FieldEquals")]
[assembly: InternalsVisibleTo("AutoCSer.MySql")]
[assembly: InternalsVisibleTo("AutoCSer.Oracle")]
[assembly: InternalsVisibleTo("AutoCSer.Search")]
[assembly: InternalsVisibleTo("AutoCSer.HtmlNode")]
[assembly: InternalsVisibleTo("AutoCSer.HtmlTitle")]
[assembly: InternalsVisibleTo("AutoCSer.RawSocketListener")]
[assembly: InternalsVisibleTo("AutoCSer.DataSetSerialize")]
[assembly: InternalsVisibleTo("AutoCSer.RemoteControl")]
[assembly: InternalsVisibleTo("AutoCSer.RemoteControlClient")]

[assembly: InternalsVisibleTo("AutoCSer.WebClient")]
[assembly: InternalsVisibleTo("AutoCSer.OpenAPI")]
[assembly: InternalsVisibleTo("AutoCSer.OpenAPI.51Nod")]
[assembly: InternalsVisibleTo("AutoCSer.OpenAPI.Weixin")]
[assembly: InternalsVisibleTo("AutoCSer.OpenAPI.QQ")]
[assembly: InternalsVisibleTo("AutoCSer.OpenAPI.Renren")]
[assembly: InternalsVisibleTo("AutoCSer.OpenAPI.Weibo")]

[assembly: InternalsVisibleTo("AutoCSer.Expand")]
[assembly: InternalsVisibleTo("AutoCSer.CodeGenerator.Custom")]//用户自定义代码生成预留程序集名称
[assembly: InternalsVisibleTo("AutoCSer.Custom")]//预留程序集名称，开发者可以自建项目暴露 AutoCSer 的 internal 访问权限
[assembly: InternalsVisibleTo("AutoCSer.TestCase")]
[assembly: InternalsVisibleTo("AutoCSer.Tool.ILTest")]