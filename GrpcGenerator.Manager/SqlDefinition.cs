using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcGenerator.Manager
{
    public class SqlDefinition
    {
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }

        public string ServiceNamespace { get; set; }
        public string ServiceName { get; set; }

        public List<SqlTable> SqlTables { get; set; }
        public List<SqlProcedure> SqlProcedures { get; set; }
    }

}
