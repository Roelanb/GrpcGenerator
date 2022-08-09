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

        public List<RpcCall> RpcCalls { get; set; }
        public List<RpcMessage> RpcMessages { get; set; }

        public ProtoFile(string name, string? serviceName, string package, string namespaces, SqlStructure sql)
        {


            Name = name;
            ServiceName = serviceName;
            Package = package;
            Namespace = namespaces;

            RpcCalls = new List<RpcCall>();
            RpcMessages = new List<RpcMessage>();

            foreach (var sqlProcedure in sql.SqlProcedures)
            {
                var rpcCall = new RpcCall
                {
                    Name = sqlProcedure.Name.FirstCharToUpper(),
                    Description = $"{sqlProcedure.Schema} {sqlProcedure.Name.FirstCharToUpper()}",
                    Request = $"{sqlProcedure.Name.FirstCharToUpper()}_Request",
                    Response = $"{sqlProcedure.Name.FirstCharToUpper()}_Response",
                    SqlProcedure = sqlProcedure,
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
                        Type = SqlTypeToRpcType(sqlField.Type)
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
                        Type = SqlTypeToRpcType(sqlField.Type)
                    };
                    rpcResponseRecordMessage.RpcMessageFields.Add(rpcField);
                }
                RpcMessages.Add(rpcResponseRecordMessage);
            }


        }

        private string SqlTypeToRpcType(string sqlType)
        {
            if (sqlType.Contains("varchar")) return "string";
            if (sqlType.Contains("datetime")) return "google.protobuf.Timestamp";
            if (sqlType.Contains("int")) return "int32";
            if (sqlType.Contains("real")) return "int32";

            return sqlType;
        }
    }

    public class RpcCall
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public SqlProcedure SqlProcedure { get; set; }
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
            return $"   {Type} {Name} = {Index};";
        }
    }
}
