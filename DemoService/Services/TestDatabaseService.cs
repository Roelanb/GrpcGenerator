using DemoService;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;

namespace DemoService.Services
{
    public class TestDatabaseService : TestDatabase.TestDatabaseBase
    {
        private readonly string _connectionString;

        public TestDatabaseService()
        {
            _connectionString = "Data Source=(local);Initial Catalog=AdventureWorks2019;Integrated Security=True;";
        }

        public override Task<Csp_rpc_procedure1_Response> Csp_rpc_procedure1(Csp_rpc_procedure1_Request request, ServerCallContext context)
        {
            var resultRecords = new List<Csp_rpc_procedure1_Record>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("csp_rpc_procedure1", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add("@startdate",System.Data.SqlDbType.DateTime );
                command.Parameters["@startdate"].Value = request.Startdate.ToDateTime();
                command.Parameters.Add("@search1",System.Data.SqlDbType.VarChar);
                command.Parameters["@search1"].Value = request.Search1;

                command.Connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    resultRecords.Add(new Csp_rpc_procedure1_Record
                    {
                        Id = (int)reader["id"]
                    });
                }
            }
            var result = new Csp_rpc_procedure1_Response
            {
                ResultCode = 0,
                ResultText = "OK"
            };
            result.Records.AddRange(resultRecords);
            return Task.FromResult(result) ; 
        }

    }
}