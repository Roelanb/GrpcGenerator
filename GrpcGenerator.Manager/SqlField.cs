namespace GrpcGenerator.Manager
{
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
}
