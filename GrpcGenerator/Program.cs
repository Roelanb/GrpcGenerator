using GrpcGenerator.Manager;
using System.Data;
using System.Data.SqlClient;

namespace GrpcGenerator
{
    internal class Program
    {
        private static string protofileLocationService = @"D:\beecoders\GrpcGenerator\DemoService\Protos";
        private static string protofileLocationClient = @"D:\beecoders\GrpcGenerator\DemoClient\DemoClient\Protos";

        static void Main(string[] args)
        {
            var mgr = new ProtoBuilder();
            var serviceMgr = new ServiceBuilder();



            mgr.Generate(mgr.Generate("Data Source=(local);Initial Catalog=Test;Integrated Security=True;", "Test"));
            serviceMgr.Generate(serviceMgr.Generate("Data Source=(local);Initial Catalog=Test;Integrated Security=True;", "Test"));

            Console.WriteLine("Please press any key to exit...");
            Console.ReadKey();
        }

    }
}