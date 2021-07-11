using ReSQL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReSQL.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ReSQLSaveAttribute : Attribute
    {
        internal string customName { get; set; }
        internal ColumnType columnType { get; set; }
        internal bool isNull { get; set; }
        internal IndexType indexType { get; set; }
        internal SQLAttribute sqlAttribute { get; set; }
        internal bool autoIncrement { get; set; }

        public ReSQLSaveAttribute(string customName = null, ColumnType columnType = ColumnType.NONE, bool isNull = true, IndexType indexType = IndexType.NONE, SQLAttribute sqlAttribute = SQLAttribute.NONE, bool autoIncrement = false, bool Jsonize = false)
        {
            this.customName = customName;
            this.columnType = columnType;
            this.isNull = isNull;
            this.indexType = indexType;
            this.sqlAttribute = sqlAttribute;
            this.autoIncrement = autoIncrement;
        }
    }
}
