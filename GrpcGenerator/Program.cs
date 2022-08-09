using GrpcGenerator.Manager;
using System.Data;
using System.Data.SqlClient;

namespace GrpcGenerator
{
    internal class Program
    {

        static void Main(string[] args)
        {
            var def = new SqlDefinition
            {
                Name = "Requirements",
                Package = "requirements",
                ServerName = "bebodbst3",
                DatabaseName = "MES",
                ServiceName = "RequirementsService",
                ServiceNamespace = "DemoService.Services",

                ProtoFileLocationServer = @"C:\github\GrpcGenerator\DemoService\Protos",
                ProtoFileLocationClient = @"C:\github\GrpcGenerator\DemoClient\DemoClient\Protos",
                ServiceFileLocation = @"C:\github\GrpcGenerator\DemoService\Services",

                SqlProcedures = new List<SqlProcedure>
                {
                    new SqlProcedure {Catalog = "MES",Schema="dbo", Name = "csp_req_clone_project"}
                }
            };


            var mgr = new ProtoBuilder();
            var serviceMgr = new ServiceBuilder();


            mgr.Generate(def);
            serviceMgr.Generate(def);

            Console.WriteLine("Please press any key to exit...");
            Console.ReadKey();
        }

    }
}