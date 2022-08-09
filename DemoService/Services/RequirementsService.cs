using Grpc.Core;
using System.Data.SqlClient;

namespace DemoService.Services.Services
{
public class RequirementsService : Requirements.RequirementsBase    
{
private readonly string _connectionString;

public RequirementsService()
{
_connectionString = "Data Source=(local);Initial Catalog=AdventureWorks2019;Integrated Security=True;";
}

public override Task<Csp_req_clone_project_Response> Csp_req_clone_project(Csp_req_clone_project_Request request, ServerCallContext context)
{
var resultRecords = new List<Csp_req_clone_project_Record>();

using (SqlConnection connection = new SqlConnection(_connectionString))
{
      var command = new SqlCommand("csp_req_clone_project", connection);
      command.CommandType = System.Data.CommandType.StoredProcedure;

      command.Parameters.Add("@new_project_code",System.Data.SqlDbType.DateTime );
      command.Parameters.Add("@old_project_code",System.Data.SqlDbType.DateTime );

      command.Connection.Open();
      var reader = command.ExecuteReader();
      while (reader.Read())
{
         resultRecords.Add(new Csp_req_clone_project_Record
{
         ResultCode = reader["ResultCode"].ToString(),
         ResultText = reader["ResultText"].ToString(),
});
}
}

var result = new Csp_req_clone_project_Response
{
   ResultCode = 0,
   ResultText = "OK"
};
result.Records.AddRange(resultRecords);
return Task.FromResult(result);
}
}
}
