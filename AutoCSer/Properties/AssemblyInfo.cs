using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// 有关程序集的常规信息通过以下
// 特性集控制。更改这些特性值可修改
// 与程序集关联的信息。
#if NETCOREAPP2_0
#else
[assembly: AssemblyTitle("AutoCSer")]
[assembly: AssemblyCopyright("Copyright © 肖进 2017")]
[assembly: AssemblyDescription("AutoCSer 是一个以高效率为目标向导的整体开发框架。主要包括 TCP 接口服务框架、TCP 函数服务框架、前后端一体 WEB 视图框架、ORM 内存索引缓存框架、二进制 / JSON / XML 数据序列化 等一系列无缝集成的高性能组件。")]
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
#endif
#if CodeGenerator
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e0001")]
#endif
#if CodeGeneratorX64
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e0002")]
#endif
#if CodeGeneratorX86
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e0003")]
#endif
#if AllSerialize
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e0004")]
#endif
#if BinarySerialize
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e0005")]
#endif
#if Json
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e0006")]
#endif
#if Xml
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e0007")]
#endif
#if WebView
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e0008")]
#endif
#if Sort
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e0009")]
#endif
#if Sql
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e000a")]
#endif
#if Deploy
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e000b")]
#endif
#if DiskBlock
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e000c")]
#endif
#if Gif
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e000d")]
#endif
#if RandomObject
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e000e")]
#endif
#if FieldEquals
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e000f")]
#endif
#if MySql
[assembly: Guid("13510310-0414-0c06-0c1f-13530c1e0010")]
#endif

[assembly: InternalsVisibleTo("AutoCSer.DynamicAssembly")]
[assembly: InternalsVisibleTo("AutoCSer.TcpServer.Emit")]
[assembly: InternalsVisibleTo("AutoCSer.CodeGenerator")]
[assembly: InternalsVisibleTo("AutoCSer.Sort")]
[assembly: InternalsVisibleTo("AutoCSer.WebView")]
[assembly: InternalsVisibleTo("AutoCSer.Sql")]
[assembly: InternalsVisibleTo("AutoCSer.DiskBlock")]
[assembly: InternalsVisibleTo("AutoCSer.Deploy")]
[assembly: InternalsVisibleTo("AutoCSer.Drawing.Gif")]
[assembly: InternalsVisibleTo("AutoCSer.RandomObject")]
[assembly: InternalsVisibleTo("AutoCSer.FieldEquals")]
[assembly: InternalsVisibleTo("AutoCSer.MySql")]

[assembly: InternalsVisibleTo("AutoCSer.CodeGenerator.Custom")]//用户自定义代码生成预留程序集名称
[assembly: InternalsVisibleTo("AutoCSer.Custom")]//预留程序集名称，开发者可以自建项目暴露 AutoCSer 的 internal 访问权限
[assembly: InternalsVisibleTo("AutoCSer.TestCase")]
[assembly: InternalsVisibleTo("AutoCSer.Tool.ILTest")]