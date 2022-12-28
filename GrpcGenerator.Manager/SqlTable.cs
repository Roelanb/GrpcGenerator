using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace GrpcGenerator.Manager
{
    [ObservableObject]
    public partial class SqlTable
    {
        [ObservableProperty]
        private string _catalog;
        [ObservableProperty]
        private string _schema;

        [ObservableProperty]
        private string? _name;
        [ObservableProperty]
        private string _type;
        [ObservableProperty]
        public ObservableCollection<SqlField> _sqlFields;


        public (RpcCall rpcCall, RpcMessage rpcResult, RpcMessage rpcQuery, RpcMessage rpcRecord) GetData_ProtoStructure()
        {
            var simpleSchema = Schema;
            if (simpleSchema == "dbo") simpleSchema = "";

            var rpcCall = new RpcCall
            {
                Name = $"{simpleSchema}{Name}_GetData",
                Description = $"{Schema}.{Name} simple data query",
                Request = $"{simpleSchema}{Name}_GetData_Query",
                Response = $"{simpleSchema}{Name}_GetData_Result",
                SqlProcedure = null,
                SqlTable = this
            };

            var rpcRecord = new RpcMessage
            {
                Name = $"{simpleSchema}{Name}_GetData_Record",
                Description = $"Record data",
                RpcMessageFields = new List<RpcMessageField>()
            };
            var paramCount = 1;
            foreach (var sqlField in SqlFields)
            {
                var rpcField = new RpcMessageField
                {
                    Name = sqlField.Name.Replace("@", ""),
                    Description = $"{sqlField.Type}",
                    Index = paramCount++,
                    Type = Helpers.SqlTypeToRpcType(sqlField.Type)
                };
                rpcRecord.RpcMessageFields.Add(rpcField);
            }

            var rpcResult = new RpcMessage
            {
                Name = $"{simpleSchema}{Name}_GetData_Result",
                Description = $"Result data",
                RpcMessageFields = new List<RpcMessageField>()
            };

            rpcResult.RpcMessageFields.Add(new RpcMessageField
            {
                Name = "resultText",
                Description = "Result Text",
                Index = 1,
                Type = "string"
            });

            rpcResult.RpcMessageFields.Add(new RpcMessageField
            {
                Name = "resultCode",
                Description = "Result Code",
                Index = 2,
                Type = "int32"
            });

            rpcResult.RpcMessageFields.Add(new RpcMessageField
            {
                Name = "records",
                Description = "Result Code",
                Index = 3,
                Type = $"repeated {simpleSchema}{Name}_GetData_Record"
            });

            var rpcQuery = new RpcMessage
            {
                Name = $"{simpleSchema}{Name}_GetData_Query",
                Description = $"Result data",
                RpcMessageFields = new List<RpcMessageField>()
            };

            rpcQuery.RpcMessageFields.Add(new RpcMessageField
            {
                Name = "WhereClause",
                Index = 1,
                Type = "string"
            });
            return (rpcCall, rpcResult, rpcQuery, rpcRecord);
        }

        public List<string> GetData_ServiceCall(string indent, RpcCall rpcCall)
        {
            var simpleSchema = Schema;
            if (simpleSchema == "dbo") simpleSchema = "";

            var protoFileLines = new List<string>();

            var sqlCmd = $"select * from {rpcCall.SqlTable.Schema}.{rpcCall.SqlTable.Name}";

            protoFileLines.Add($"public override Task<{rpcCall.Response}> {rpcCall.Name}({rpcCall.Request} request, ServerCallContext context)");
            protoFileLines.Add("{");
            protoFileLines.Add($"var resultRecords = new List<{rpcCall.Name}_Record>();");

            protoFileLines.Add("");
            protoFileLines.Add($"using (SqlConnection connection = new SqlConnection(_connectionString))");
            protoFileLines.Add("{");
            protoFileLines.Add($"{indent}{indent}var command = new SqlCommand(\"{sqlCmd}\", connection);");
            protoFileLines.Add($"{indent}{indent}command.CommandType = System.Data.CommandType.Text;");
            protoFileLines.Add("");


            protoFileLines.Add($"{indent}{indent}command.Connection.Open();");
            protoFileLines.Add($"{indent}{indent}var reader = command.ExecuteReader();");
            protoFileLines.Add($"{indent}{indent}while (reader.Read())");
            protoFileLines.Add("{");
            protoFileLines.Add($"{indent}{indent}{indent}resultRecords.Add(new {rpcCall.Name}_Record");
            protoFileLines.Add("{");

            foreach (var sqlField in rpcCall.SqlTable.SqlFields)
            {
                protoFileLines.AddRange(GenerateResult(indent, sqlField));
            }

            protoFileLines.Add("});");

            protoFileLines.Add("}");

            protoFileLines.Add("}");

            protoFileLines.Add("");

            protoFileLines.Add($"var result = new {rpcCall.Response}");
            protoFileLines.Add("{");
            protoFileLines.Add($"{indent}ResultCode = 0,");
            protoFileLines.Add($"{indent}ResultText = \"OK\"");
            protoFileLines.Add("};");

            protoFileLines.Add($"result.Records.AddRange(resultRecords);");
            protoFileLines.Add($"return Task.FromResult(result);");


            protoFileLines.Add("}");

            return protoFileLines;
        }

        public List<string> GetData_ServiceCallFromTemplate(string[] template, string indent, RpcCall rpcCall)
        {
            if (rpcCall == null) return new List<string>();

            var protoFileLines = new List<string>();

            var setRecordFieldList_Result = new List<string>();

            foreach (var sqlField in rpcCall.SqlTable.SqlFields)
            {
                setRecordFieldList_Result.AddRange(GenerateResult(indent, sqlField));
            }

          


            var sqlCmd = $"select * from {rpcCall.SqlTable.Schema}.{rpcCall.SqlTable.Name}";

            foreach (var templateLine in template)
            {
                var line = templateLine;

                line = line.Replace("{Response}", $"{rpcCall.Response}");
                line = line.Replace("{Request}", $"{rpcCall.Request}");
                line = line.Replace("{Record}", $"{rpcCall.Name}_Record");
                line = line.Replace("{Name}", $"{rpcCall.Name}");
                line = line.Replace("{SqlCommand}", $"{sqlCmd}");
                line = line.Replace("{SetRecordFieldList}", $"{string.Join(Environment.NewLine, setRecordFieldList_Result)}");

                protoFileLines.Add(line);
            }

            return protoFileLines;


        }

        public List<string> GenerateResult(string indent, SqlField sqlField)
        {
            var protoFileLines = new List<string>();

            if (sqlField.Type.ToLower() == "int")
                protoFileLines.Add($"{indent}{indent}{indent}if (reader[\"{sqlField.Name}\"]!=System.DBNull.Value)  rec.{sqlField.Name.FirstCharToUpper()} = (int)reader[\"{sqlField.Name}\"];");
            if (sqlField.Type.ToLower().Contains("varchar"))
                protoFileLines.Add($"{indent}{indent}{indent}rec.{sqlField.Name.FirstCharToUpper()} = reader[\"{sqlField.Name}\"].ToString();");


            return protoFileLines;
        }
    }
}
