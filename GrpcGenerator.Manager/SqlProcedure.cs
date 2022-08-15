using System;

namespace GrpcGenerator.Manager
{
    public class SqlProcedure
    {
        public string Catalog { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public List<SqlField> SqlInputParameters { get; set; }
        public List<SqlField> SqlFields { get; set; }

        public override string ToString()
        {
            return $"{Catalog} {Schema} {Name} {Type}";
        }

        public List<string> Procedure_ServiceCallFromTemplate(string[] template, string indent, RpcCall rpcCall)
        {
            if (rpcCall == null) return new List<string>();

            var protoFileLines = new List<string>();

            var setRecordFieldList_Result = new List<string>();

            foreach (var sqlField in rpcCall.SqlProcedure.SqlFields)
            {
                setRecordFieldList_Result.AddRange(GenerateResult(indent, sqlField));
            }


            var procedureParameterList_Result = new List<string>();

            foreach (var sqlField in rpcCall.SqlProcedure.SqlInputParameters)
            {
                procedureParameterList_Result.AddRange(GenerateProcedureInputParameter(indent, sqlField));
            }



            var sqlCmd = $"{rpcCall.SqlProcedure.Name}";

            foreach (var templateLine in template)
            {
                var line = templateLine;

                line = line.Replace("{Response}", $"{rpcCall.Response}");
                line = line.Replace("{Request}", $"{rpcCall.Request}");
                line = line.Replace("{Record}", $"{rpcCall.Name}_Record");
                line = line.Replace("{Name}", $"{rpcCall.Name}");
                line = line.Replace("{SqlCommand}", $"{sqlCmd}");
                line = line.Replace("{SetRecordFieldList}", $"{string.Join(Environment.NewLine, setRecordFieldList_Result)}");
                line = line.Replace("{InputParameterList}", $"{string.Join(Environment.NewLine, procedureParameterList_Result)}");

                protoFileLines.Add(line);
            }

            return protoFileLines;


        }

        public List<string> GenerateResult(string indent, SqlField sqlField)
        {
            var protoFileLines = new List<string>();

            if (sqlField.Type.ToLower() == "int")
                protoFileLines.Add($"{indent}{indent}{indent}{sqlField.Name} = (int)reader[\"{sqlField.Name}\"],");
            if (sqlField.Type.ToLower().Contains("varchar"))
                protoFileLines.Add($"{indent}{indent}{indent}{sqlField.Name} = reader[\"{sqlField.Name}\"].ToString(),");


            return protoFileLines;
        }

        public List<string> GenerateProcedureInputParameter(string indent, SqlField sqlField)
        {
            var protoFileLines = new List<string>();

            protoFileLines.Add($"{indent}{indent}command.Parameters.Add(\"{sqlField.Name}\",{Helpers.StringToSqlDbType(sqlField.Type)} );");
            protoFileLines.Add($"{indent}{indent}command.Parameters[\"{sqlField.Name}\"].Value = request.{sqlField.Name.Replace("@", "")};");
            return protoFileLines;
        }

    }
}
