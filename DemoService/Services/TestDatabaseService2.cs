using Grpc.Core;
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

}
}
