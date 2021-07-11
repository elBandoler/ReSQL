using ReSQL.Data;

namespace ReSQL.Models
{
    public class Column
    {
        public Column(string name, ColumnType type, object @default, SQLAttribute attributes, bool @null, IndexType indexType, bool autoIncrement)
        {
            Name = name;
            Type = type;
            //Default = @default;
            Attribute = attributes;
            Nullable = @null;
            IndexType = indexType;
            AutoIncrement = autoIncrement;
        }

        public string Name { get; set; }
        public ColumnType Type { get; set; }
        //public object Default { get; set; } // custom, NULL or CURRENT_TIMESTAMP
        public SQLAttribute Attribute { get; set; } 
        public bool Nullable { get; set; }
        public IndexType IndexType { get; set; }
        public bool AutoIncrement { get; set; }
        public bool Signed { get; set; }
    }
}