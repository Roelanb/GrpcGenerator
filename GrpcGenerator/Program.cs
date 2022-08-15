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
                Name = "Person",
                Package = "adventureWorks2019",
                ServerName = "localhost",
                DatabaseName = "AdventureWorks2019",
                ServiceName = "PersonService",
                ServiceNamespace = "DemoService.Services",
                ConnectionString = "Data Source=(local);Initial Catalog=AdventureWorks2019;Integrated Security=True;",
                ProtoFileLocationServer = @"d:\beecoders\GrpcGenerator\DemoService\Protos",
                ProtoFileLocationClient = @"d:\beecoders\GrpcGenerator\DemoClient\DemoClient\Protos",
                ServiceFileLocation = @"d:\beecoders\GrpcGenerator\DemoService\Services",

                SqlProcedures = new List<SqlProcedure>
                {
                    new SqlProcedure {Catalog = "AdventureWorks2019",Schema="dbo", Name = "uspGetManagerEmployees"}
                },

                SqlTables = new List<SqlTable>
                {
                    new SqlTable {Catalog = "AdventureWorks2019", Schema = "Person", Name = "Person" },
                    new SqlTable {Catalog = "AdventureWorks2019", Schema = "Person", Name = "Address" },
                }

            };


            var protoMgr = new ProtoBuilder();
            var serviceMgr = new ServiceBuilder();


            protoMgr.Generate(def);
            serviceMgr.Generate(def);

            Console.WriteLine("Please press any key to exit...");
            Console.ReadKey();
        }

    }
}