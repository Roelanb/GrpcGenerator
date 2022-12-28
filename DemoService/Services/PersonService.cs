using Grpc.Core;
using System.Data.SqlClient;

// Service Auto generated: Dec-22-2022 13:51

namespace DemoService.Services
{
    public class PersonService : Person.PersonBase
    {
        private readonly string _connectionString;

        public PersonService()
        {
            _connectionString = "Data Source=(local);Initial Catalog=AdventureWorks2019;Integrated Security=True;";
        }

        public override Task<PersonPerson_GetData_Result> PersonPerson_GetData(PersonPerson_GetData_Query request, ServerCallContext context)
{
    var resultRecords = new List<PersonPerson_GetData_Record>();

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        var command = new SqlCommand("select * from Person.Person", connection);
        command.CommandType = System.Data.CommandType.Text;

        command.Connection.Open();
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var rec = new PersonPerson_GetData_Record();

                     if (reader["BusinessEntityID"]!=System.DBNull.Value)  rec.BusinessEntityID = (int)reader["BusinessEntityID"];
         rec.FirstName = reader["FirstName"].ToString();
         rec.LastName = reader["LastName"].ToString();
         if (reader["EmailPromotion"]!=System.DBNull.Value)  rec.EmailPromotion = (int)reader["EmailPromotion"];
         rec.Suffix = reader["Suffix"].ToString();
         rec.MiddleName = reader["MiddleName"].ToString();
         rec.Title = reader["Title"].ToString();

            
            resultRecords.Add(rec);
        }
    }

    var result = new PersonPerson_GetData_Result
    {
        ResultCode = 0,
        ResultText = "OK"
    };
    result.Records.AddRange(resultRecords);
    return Task.FromResult(result);
}
public override Task<PersonAddress_GetData_Result> PersonAddress_GetData(PersonAddress_GetData_Query request, ServerCallContext context)
{
    var resultRecords = new List<PersonAddress_GetData_Record>();

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        var command = new SqlCommand("select * from Person.Address", connection);
        command.CommandType = System.Data.CommandType.Text;

        command.Connection.Open();
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var rec = new PersonAddress_GetData_Record();

                     if (reader["AddressID"]!=System.DBNull.Value)  rec.AddressID = (int)reader["AddressID"];
         rec.AddressLine1 = reader["AddressLine1"].ToString();
         rec.City = reader["City"].ToString();
         if (reader["StateProvinceID"]!=System.DBNull.Value)  rec.StateProvinceID = (int)reader["StateProvinceID"];
         rec.PostalCode = reader["PostalCode"].ToString();
         rec.AddressLine2 = reader["AddressLine2"].ToString();

            
            resultRecords.Add(rec);
        }
    }

    var result = new PersonAddress_GetData_Result
    {
        ResultCode = 0,
        ResultText = "OK"
    };
    result.Records.AddRange(resultRecords);
    return Task.FromResult(result);
}

         public override Task<UspGetManagerEmployees_Response> UspGetManagerEmployees(UspGetManagerEmployees_Request request, ServerCallContext context)
        {
            var resultRecords = new List<UspGetManagerEmployees_Record>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("uspGetManagerEmployees", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                      command.Parameters.Add("@BusinessEntityID",System.Data.SqlDbType.Int );
      command.Parameters["@BusinessEntityID"].Value = request.BusinessEntityID;

                command.Connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    resultRecords.Add(new UspGetManagerEmployees_Record
                    {
                        RecursionLevel = (int)reader["RecursionLevel"],
                        OrganizationNode = reader["OrganizationNode"].ToString(),
                        ManagerFirstName = reader["ManagerFirstName"].ToString(),
                        ManagerLastName = reader["ManagerLastName"].ToString(),
                        BusinessEntityID = (int)reader["BusinessEntityID"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                    });
                }
            }

            var result = new UspGetManagerEmployees_Response
            {
                ResultCode = 0,
                ResultText = "OK"
            };
            result.Records.AddRange(resultRecords);
            return Task.FromResult(result);
        }
       
    }
}
