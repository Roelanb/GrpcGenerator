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

    public class SqlTable
    {
        public string Catalog { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public List<SqlField> SqlFields { get; set; }

    }
    public class SqlField
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public string MaxLength { get; set; }

        public override string ToString()
        {
            return $"{Name} {Type} {MaxLength}";
        }
    }


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

    }
}
