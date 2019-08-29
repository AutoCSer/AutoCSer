using System;
using System.ServiceProcess;

namespace AutoCSer.Web.DeployService
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBase.Run(new ServiceBase[] { new DeployService() });
        }
    }
}
