using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcGenerator.Manager
{
    public static class Helpers
    {
        public static string SqlTypeToRpcType(string sqlType)
        {
            if (sqlType.Contains("char")) return "string";
            if (sqlType.Contains("bit")) return "bool";
            if (sqlType.Contains("datetime")) return "google.protobuf.Timestamp";
            if (sqlType.Contains("int")) return "int32";
            if (sqlType.Contains("real")) return "int32";

            if (sqlType.Contains("xml")) return "string";
            if (sqlType.Contains("uniqueidentifier")) return "string";
            if (sqlType.Contains("geography")) return "string";

            return sqlType;
        }

        public static string StringToSqlDbType(string sqlType)
        {
            if (sqlType == "dattime") return "System.Data.SqlDbType.DateTime";
            if (sqlType == "nvarchar") return "System.Data.SqlDbType.NVarChar";
            if (sqlType == "text") return "System.Data.SqlDbType.Text";
            if (sqlType == "ntext") return "System.Data.SqlDbType.NText";
            if (sqlType == "int") return "System.Data.SqlDbType.Int";
            if (sqlType == "varchar") return "System.Data.SqlDbType.VarChar";

            if (sqlType == "char") return "System.Data.SqlDbType.Char";
            if (sqlType == "nchar") return "System.Data.SqlDbType.NChar";

            return "System.Data.SqlDbType.NVarChar";
        }
    }
}
