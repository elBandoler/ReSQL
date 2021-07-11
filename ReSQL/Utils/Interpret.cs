using ReSQL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReSQL.Utils
{
    /// <summary>
    /// This class allows to interpret identifiers (enums etc.) to models and objects
    /// </summary>
    class Interpret
    {
        public static ColumnType ColumnType(PropertyInfo field, out bool signed)
        {
            switch(field.PropertyType.Name)
            {
                case "Byte": case "UInt16": case "UInt32": case "UInt64": signed = false; break;
                default: signed = true; break;
            }
            switch (field.PropertyType.Name)
            {
                case "SByte": case "Byte": return Data.ColumnType.TINYINT;
                case "UInt16": case "Int16": return Data.ColumnType.SMALLINT;
                case "UInt32": case "Int32": return Data.ColumnType.INT;
                case "UInt64": case "Int64": return Data.ColumnType.BIGINT;
                case "Decimal": return Data.ColumnType.DECIMAL;
                case "Float": return Data.ColumnType.FLOAT;
                case "Double": return Data.ColumnType.DOUBLE;
                case "Char": return Data.ColumnType.CHAR;
                case "String": return Data.ColumnType.TEXT;
                case "Boolean": return Data.ColumnType.BOOLEAN;
                default: return Data.ColumnType.TEXT;
            }
        }


        internal static string ColumnTypeName(ColumnType type) => type == Data.ColumnType.NONE? "":Enum.GetName(typeof(ColumnType), type);

        internal static string IndexTypeName(IndexType indexType)
        {
            var name = Enum.GetName(typeof(IndexType), indexType);
            switch (indexType)
            {
                case IndexType.NONE: return "";
                case IndexType.PRIMARY: case IndexType.UNIQUE: return name + " KEY";
                default: throw new NotImplementedException();
            }
        }

        internal static string AttributeName(SQLAttribute attribute) 
            => attribute == SQLAttribute.NONE? "":Enum.GetName(typeof(SQLAttribute), attribute);

        internal static string IndexTypeInformationSchemaName(IndexType indexType)
        {
            switch(indexType)
            {
                case IndexType.PRIMARY: return "PRI";
                case IndexType.UNIQUE: return "UNI";
                case IndexType.NONE: return "";
                default: throw new NotImplementedException();
            }
        }
    }
}
