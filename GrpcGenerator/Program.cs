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
            var def = new SqlDefinition
            {
                ServerName = "bebodbst3",
                DatabaseName = "MES",
                ServiceName = "RequirementsService",
                ServiceNamespace = "DemoService.Services",
                SqlProcedures = new List<SqlProcedure>
                {
                    new SqlProcedure {Catalog = "MES",Schema="dbo", Name = "csp_req_clone_project"}
                }
            };


            var mgr = new ProtoBuilder();
            var serviceMgr = new ServiceBuilder();


            mgr.Generate(def);
         //   serviceMgr.Generate(connstring, database);

            Console.WriteLine("Please press any key to exit...");
            Console.ReadKey();
        }

    }
}