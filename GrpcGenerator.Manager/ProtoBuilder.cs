using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace GrpcGenerator.Manager
{
    public class ProtoBuilder
    {
   //     private static string protofileLocationService = @"D:\beecoders\GrpcGenerator\DemoService\Protos";
    //    private static string protofileLocationClient = @"D:\beecoders\GrpcGenerator\DemoClient\DemoClient\Protos";
        private static string indent = "   ";

        private List<string> protoFileLines;

        public List<string> Generate(ProtoFile pf, string protoFileLocationServer,string protoFileLocationClient)
        {
            protoFileLines = new List<string>();

            protoFileLines.Add($"// Auto generated proto file: {DateTime.Now:MMM-dd-yyyy HH:mm}");

            protoFileLines.Add("syntax = \"proto3\";");
            protoFileLines.Add("");

            // imports
            protoFileLines.Add("import \"google/protobuf/duration.proto\";");
            protoFileLines.Add("import \"google/protobuf/timestamp.proto\";");
            protoFileLines.Add("");

            protoFileLines.Add($"option csharp_namespace = \"{pf.Namespace}\";");
            protoFileLines.Add("");
            protoFileLines.Add($"package {pf.Package};");
            protoFileLines.Add("");

            protoFileLines.Add($"// The {pf.Name} service definition.");
            protoFileLines.Add($"service {pf.Name}");
            protoFileLines.Add("{");

            // list of rpcCalls
            if (pf.RpcCalls != null)
            {
                foreach (var rpcCall in pf.RpcCalls)
                {
                    protoFileLines.Add($"// Sends a {rpcCall.Description}");

                    protoFileLines.Add($"rpc {rpcCall.Name} ({rpcCall.Request}) returns ({rpcCall.Response});");
                    protoFileLines.Add("");

                }
            }

            protoFileLines.Add("}");

            protoFileLines.Add("");

            if (pf.RpcMessages != null)
            {
                foreach (var rpcMessage in pf.RpcMessages)
                {
                    protoFileLines.Add($"// {rpcMessage.Description}");
                    protoFileLines.Add($"message {rpcMessage.Name}");

                    protoFileLines.Add("{");

                    foreach (var field in rpcMessage.RpcMessageFields)
                    {
                        protoFileLines.Add(field.ToString());
                    }
                    protoFileLines.Add("}");

                }
            }

            // store the file

            var filename = $"{pf.Name.ToLower()}.proto";

            if (!string.IsNullOrEmpty(protoFileLocationServer))
            {
                var protofileService = $"{protoFileLocationServer}\\{filename}";
                File.WriteAllLines(protofileService, protoFileLines);
            }
            if (!string.IsNullOrEmpty(protoFileLocationClient))
            {
                var protofileClient = $"{protoFileLocationClient}\\{filename}";
                File.WriteAllLines(protofileClient, protoFileLines);
            }

            return protoFileLines;
        }
        public void GenerateFromTemplate(ProtoFile pf, string protoFileLocationServer, string protoFileLocationClient)
        {
            
            protoFileLines = new List<string>();

            protoFileLines.Add("syntax = \"proto3\";");
            protoFileLines.Add("");

            // imports
            protoFileLines.Add("import \"google/protobuf/duration.proto\";");
            protoFileLines.Add("import \"google/protobuf/timestamp.proto\";");
            protoFileLines.Add("");

            protoFileLines.Add($"option csharp_namespace = \"{pf.Namespace}\";");
            protoFileLines.Add("");
            protoFileLines.Add($"package {pf.Package};");
            protoFileLines.Add("");

            protoFileLines.Add($"// The {pf.Name} service definition.");
            protoFileLines.Add($"service {pf.Name}");
            protoFileLines.Add("{");

            // list of rpcCalls
            if (pf.RpcCalls != null)
            {
                foreach (var rpcCall in pf.RpcCalls)
                {
                    protoFileLines.Add($"// Sends a {rpcCall.Description}");

                    protoFileLines.Add($"rpc {rpcCall.Name} ({rpcCall.Request}) returns ({rpcCall.Response});");
                    protoFileLines.Add("");

                }
            }

            protoFileLines.Add("}");

            protoFileLines.Add("");

            if (pf.RpcMessages != null)
            {
                foreach (var rpcMessage in pf.RpcMessages)
                {
                    protoFileLines.Add($"// {rpcMessage.Description}");
                    protoFileLines.Add($"message {rpcMessage.Name}");

                    protoFileLines.Add("{");

                    foreach (var field in rpcMessage.RpcMessageFields)
                    {
                        protoFileLines.Add(field.ToString());
                    }
                    protoFileLines.Add("}");

                }
            }

            // store the file

            var filename = $"{pf.Name.ToLower()}.proto";

            if (!string.IsNullOrEmpty(protoFileLocationServer))
            {
                var protofileService = $"{protoFileLocationServer}\\{filename}";
                File.WriteAllLines(protofileService, protoFileLines);
            }
            if (!string.IsNullOrEmpty(protoFileLocationClient))
            {
                var protofileClient = $"{protoFileLocationClient}\\{filename}";
                File.WriteAllLines(protofileClient, protoFileLines);
            }
        }

        public ProtoFile Generate(SqlDefinition definition)
        {

           
            var sqlStructure = new SqlStructure();

            using (SqlConnection conn = new SqlConnection(definition.ConnectionString))
            {
                conn.Open();

                sqlStructure.SqlTables = GetSqlTables(conn.GetSchema("Tables"), conn.GetSchema("Columns"), definition.SqlTables);

                sqlStructure.SqlProcedures = GetSqlProcedures(conn, definition.SqlProcedures);

            }

            var pf = new ProtoFile(definition.Name,
                            definition.ServiceName,
                            definition.Package,
                            definition.ServiceNamespace,
                            definition.ConnectionString,
                            sqlStructure);

            pf.GeneratedProtoFile = Generate(pf,definition.ProtoFileLocationServer,definition.ProtoFileLocationClient);

            
            return pf;
        }


        public List<SqlTable> GetSqlTables(DataTable tables, DataTable columns, List<SqlTable>? filter)
        {
            var result = new List<SqlTable>();

            foreach (DataRow row in tables.Rows)
            {
                var sqlTable = new SqlTable();
                sqlTable.SqlFields = new ObservableCollection<SqlField>();

                foreach (DataColumn col in tables.Columns)
                {
                    if (col.ColumnName == "TABLE_CATALOG") sqlTable.Catalog = row[col].ToString();
                    if (col.ColumnName == "TABLE_SCHEMA") sqlTable.Schema = row[col].ToString();
                    if (col.ColumnName == "TABLE_NAME") sqlTable.Name = row[col].ToString();
                    if (col.ColumnName == "TABLE_TYPE") sqlTable.Type = row[col].ToString();
                }
                if (filter == null)
                {
                    result.Add(sqlTable);
                }
                else
                {

                    if (filter.Any(p => p.Name == sqlTable.Name && p.Schema == sqlTable.Schema))
                    {
                        Console.WriteLine($"Adding table {sqlTable.Schema}.{sqlTable.Name}");
                        result.Add(sqlTable);
                    }
                }
            }

            foreach (DataRow row in columns.Rows)
            {
                var sqlField = new SqlField();

                sqlField.Name = row["COLUMN_NAME"].ToString();
                sqlField.Type = row["DATA_TYPE"].ToString();

                var table = row["TABLE_NAME"].ToString();

                var sqlTable = result.FirstOrDefault(p => p.Name == table);

                if (sqlTable != null) sqlTable.SqlFields.Add(sqlField);


            }
            return result;
        }



        public static List<SqlProcedure> GetSqlProcedures(SqlConnection conn,List<SqlProcedure> filter)
        {
            if (filter == null) return new List<SqlProcedure>();

            DataTable procedures = conn.GetSchema("Procedures");
            DataTable procedureParameters = conn.GetSchema("ProcedureParameters");

            var result = new List<SqlProcedure>();

            foreach (DataRow row in procedures.Rows)
            {
                var sqlProcedure = new SqlProcedure();
                sqlProcedure.SqlInputParameters = new List<SqlField>();
                sqlProcedure.SqlFields = new List<SqlField>();

                sqlProcedure.Catalog = row["ROUTINE_CATALOG"].ToString();
                sqlProcedure.Schema = row["ROUTINE_SCHEMA"].ToString();
                sqlProcedure.Name = row["ROUTINE_NAME"].ToString();
                sqlProcedure.Type = row["ROUTINE_TYPE"].ToString();

                if (filter.Any(p => p.Name == sqlProcedure.Name && p.Schema == sqlProcedure.Schema)) result.Add(sqlProcedure);
            }

            foreach (DataRow row in procedureParameters.Rows)
            {
                var sqlField = new SqlField();

                sqlField.Name = row["PARAMETER_NAME"].ToString();
                sqlField.Type = row["DATA_TYPE"].ToString();
                sqlField.MaxLength = row["CHARACTER_MAXIMUM_LENGTH"].ToString();

                var procedure = row["SPECIFIC_NAME"].ToString();

                var sqlProcedure = result.FirstOrDefault(p => p.Name == procedure);

                if (sqlProcedure != null) sqlProcedure.SqlInputParameters.Add(sqlField);

            }

            foreach (var proc in result.Where(p => p.Type == "PROCEDURE"))
            {
             
                var checkCommand = $"sp_describe_first_result_set N'{proc.Schema}.{proc.Name}';";
                SqlCommand command = new SqlCommand(checkCommand, conn);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {

                    var sqlField = new SqlField();


                    sqlField.Name = reader["name"].ToString();
                    sqlField.Type = reader["system_type_name"].ToString();

                    proc.SqlFields.Add(sqlField);
                }
                reader.Close();

            }

            //DataColumn procedureDataColumn = procedureDataTable.Columns["ROUTINE_NAME"];

            //if (procedureDataColumn != null)
            //{
            //    foreach (DataRow row in procedureDataTable.Rows)
            //    {
            //        String procedureName = row[procedureDataColumn].ToString();

            //        DataTable parmsDataTable = conn.GetSchema("ProcedureParameters", new string[] { null, null, procedureName });

            //        DataColumn parmNameDataColumn = parmsDataTable.Columns["PARAMETER_NAME"];
            //        DataColumn parmTypeDataColumn = parmsDataTable.Columns["DATA_TYPE"];

            //        foreach (DataRow parmRow in parmsDataTable.Rows)
            //        {
            //            string parmName = parmRow[parmNameDataColumn].ToString();
            //            string parmType = parmRow[parmTypeDataColumn].ToString();

            //            Console.WriteLine($"{parmName} - {parmType}");
            //        }
            //    }

            //    SqlCommand cmd = new SqlCommand("csp_rpc_procedure1", conn);
            //    IDataReader rdr = cmd.ExecuteReader(CommandBehavior.KeyInfo);
            //    DataTable schema = rdr.GetSchemaTable();


            //}


            return result;
        }



        private static void ShowDataTable(DataTable table, Int32 length)
        {
            foreach (DataColumn col in table.Columns)
            {
                Console.Write("{0,-" + length + "}", col.ColumnName);
            }
            Console.WriteLine();

            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn col in table.Columns)
                {
                    if (col.DataType.Equals(typeof(DateTime)))
                        Console.Write("{0,-" + length + ":d}", row[col]);
                    else if (col.DataType.Equals(typeof(Decimal)))
                        Console.Write("{0,-" + length + ":C}", row[col]);
                    else
                        Console.Write("{0,-" + length + "}", row[col]);
                }
                Console.WriteLine();
            }
        }



        private static void ShowDataTable(DataTable table)
        {
            ShowDataTable(table, 14);
        }

        private static void ShowColumns(DataTable columnsTable)
        {
            var selectedRows = from info in columnsTable.AsEnumerable()
                               select new
                               {
                                   TableCatalog = info["TABLE_CATALOG"],
                                   TableSchema = info["TABLE_SCHEMA"],
                                   TableName = info["TABLE_NAME"],
                                   ColumnName = info["COLUMN_NAME"],
                                   DataType = info["DATA_TYPE"]
                               };

            Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}{4,-15}", "TableCatalog", "TABLE_SCHEMA",
                "TABLE_NAME", "COLUMN_NAME", "DATA_TYPE");
            foreach (var row in selectedRows)
            {
                Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}{4,-15}", row.TableCatalog,
                    row.TableSchema, row.TableName, row.ColumnName, row.DataType);
            }
        }

        private static void ShowIndexColumns(DataTable indexColumnsTable)
        {
            var selectedRows = from info in indexColumnsTable.AsEnumerable()
                               select new
                               {
                                   TableSchema = info["table_schema"],
                                   TableName = info["table_name"],
                                   ColumnName = info["column_name"],
                                   ConstraintSchema = info["constraint_schema"],
                                   ConstraintName = info["constraint_name"],
                                   KeyType = info["KeyType"]
                               };

            Console.WriteLine("{0,-14}{1,-11}{2,-14}{3,-18}{4,-16}{5,-8}", "table_schema", "table_name", "column_name", "constraint_schema", "constraint_name", "KeyType");
            foreach (var row in selectedRows)
            {
                Console.WriteLine("{0,-14}{1,-11}{2,-14}{3,-18}{4,-16}{5,-8}", row.TableSchema,
                    row.TableName, row.ColumnName, row.ConstraintSchema, row.ConstraintName, row.KeyType);
            }
        }
    }
}


