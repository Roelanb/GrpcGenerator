using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcGenerator.Manager
{
    public class ProtoFile
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string Package { get; set; }
        public string ServiceName { get; set; }
        public string ConnectionString { get; set; }

        public List<RpcCall> RpcCalls { get; set; }
        public List<RpcMessage> RpcMessages { get; set; }

        public List<string> GeneratedProtoFile { get; set; }
        public List<string> GeneratedServiceFile { get; set; }
        public ProtoFile(string name, string? serviceName, string package, string namespaces, string connectionString, SqlStructure sql)
        {


            Name = name;
            ServiceName = serviceName;
            Package = package;
            Namespace = namespaces;
            ConnectionString = connectionString;
            RpcCalls = new List<RpcCall>();
            RpcMessages = new List<RpcMessage>();

            MapSqlTables(sql.SqlTables);
            MapSqlProcedures(sql.SqlProcedures);

        }

        public void MapSqlTables(List<SqlTable> sqlTables)
        {
            foreach (var sqlTable in sqlTables)
            {
                var rpc = sqlTable.GetData_ProtoStructure();

                RpcCalls.Add(rpc.rpcCall);
                RpcMessages.Add(rpc.rpcResult);
                RpcMessages.Add(rpc.rpcRecord);
                RpcMessages.Add(rpc.rpcQuery);

            }
        }

        public void MapSqlProcedures(List<SqlProcedure> sqlProcedures)
        {
            foreach (var sqlProcedure in sqlProcedures)
            {
                var rpcCall = new RpcCall
                {
                    Name = sqlProcedure.Name.FirstCharToUpper(),
                    Description = $"{sqlProcedure.Schema} {sqlProcedure.Name.FirstCharToUpper()}",
                    Request = $"{sqlProcedure.Name.FirstCharToUpper()}_Request",
                    Response = $"{sqlProcedure.Name.FirstCharToUpper()}_Response",
                    SqlProcedure = sqlProcedure,
                    SqlTable = null
                };

                RpcCalls.Add(rpcCall);

                // requestMessage

                var rpcRequestMessage = new RpcMessage
                {
                    Name = $"{sqlProcedure.Name.FirstCharToUpper()}_Request",
                    Description = $"{sqlProcedure.Name.FirstCharToUpper()}_Request",
                    RpcMessageFields = new List<RpcMessageField>()
                };
                var paramCount = 1;
                foreach (var sqlField in sqlProcedure.SqlInputParameters)
                {
                    var rpcField = new RpcMessageField
                    {
                        Name = sqlField.Name.Replace("@", ""),
                        Description = "",
                        Index = paramCount++,
                        Type = Helpers.SqlTypeToRpcType(sqlField.Type)
                    };
                    rpcRequestMessage.RpcMessageFields.Add(rpcField);
                }
                RpcMessages.Add(rpcRequestMessage);
                // requestMessage

                var rpcResponseMessage = new RpcMessage
                {
                    Name = $"{sqlProcedure.Name.FirstCharToUpper()}_Response",
                    Description = $"{sqlProcedure.Name.FirstCharToUpper()}_Response",
                    RpcMessageFields = new List<RpcMessageField>
                    {
                        new RpcMessageField { Index = 1, Name = "resultText", Type = "string" },
                        new RpcMessageField { Index = 2, Name = "resultCode", Type = "int32" },
                        new RpcMessageField { Index = 3, Name = "records", Type = $"repeated {sqlProcedure.Name.FirstCharToUpper()}_Record" },
                    }
                };

                RpcMessages.Add(rpcResponseMessage);

                var rpcResponseRecordMessage = new RpcMessage
                {
                    Name = $"{sqlProcedure.Name.FirstCharToUpper()}_Record",
                    Description = $"{sqlProcedure.Name.FirstCharToUpper()}_Record",
                    RpcMessageFields = new List<RpcMessageField>()
                };
                paramCount = 1;
                foreach (var sqlField in sqlProcedure.SqlFields)
                {
                    var rpcField = new RpcMessageField
                    {
                        Name = sqlField.Name.Replace("@", ""),
                        Description = "",
                        Index = paramCount++,
                        Type = Helpers.SqlTypeToRpcType(sqlField.Type)
                    };
                    rpcResponseRecordMessage.RpcMessageFields.Add(rpcField);
                }
                RpcMessages.Add(rpcResponseRecordMessage);
            }
        }

       
    }

    public class RpcCall
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public SqlProcedure? SqlProcedure { get; set; }
        public SqlTable? SqlTable { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
    }

    public class RpcMessage
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public List<RpcMessageField> RpcMessageFields { get; set; }

    }

    public class RpcMessageField
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
        public string Type { get; set; }

        public override string ToString()
        {
            return $"   {Type} {Name} = {Index};        // {Description}";
        }
    }
}
