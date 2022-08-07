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


          


//            var pf = mgr.Generate("Data Source=(local);Initial Catalog=AdventureWorks2019;Integrated Security=True;", "AdventureWorks2019");
            var pf = mgr.Generate("Data Source=(local);Initial Catalog=Test;Integrated Security=True;", "Test");

            mgr.Generate(pf);

            Console.WriteLine("Please press any key to exit...");
            Console.ReadKey();
        }

    }
}