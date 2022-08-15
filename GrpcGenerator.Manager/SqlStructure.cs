using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcGenerator.Manager
{
    public class SqlStructure
    {
        public List<SqlTable> SqlTables { get; set; }
        public List<SqlProcedure> SqlProcedures { get; set; }
    }
}
