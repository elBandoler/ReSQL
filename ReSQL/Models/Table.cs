using ReSQL.Attributes;
using ReSQL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReSQL.Models
{
    public class Table
    {
        public string Name { get; set; }
        public List<Column> Columns { get; set; }

        /// <summary>
        /// Creates the structure of a table based upon a type or class
        /// </summary>
        /// <param name="T">the type or class to use as a paradigm</param>
        public Table(Type T, string name = "")
        {
            Name = name.Equals("")? T.Name:name;
            Columns = new List<Column>();

            if (T.GetCustomAttribute(typeof(ReSQLSaveAttribute)) != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                foreach (var property in T.GetProperties())
                {
                    ReSQLSaveAttribute attribute = (ReSQLSaveAttribute)property.GetCustomAttribute(typeof(ReSQLSaveAttribute));
                    if (attribute != null) // if there is a ReSQLSaveAttribute, only then should go ahead
                    {
                        string customName = attribute.customName != null ? attribute.customName : property.Name;
                        bool signed = new();
                        ColumnType columnType = attribute.columnType != ColumnType.NONE ? attribute.columnType : Utils.Interpret.ColumnType(property, out signed);
                        SQLAttribute sqlAttribute = attribute.sqlAttribute == SQLAttribute.NONE && !signed ? SQLAttribute.UNSIGNED : SQLAttribute.NONE;
                        Column column = new Column(customName, columnType, attribute.isNull, sqlAttribute, attribute.isNull, attribute.indexType, attribute.autoIncrement);
                        Columns.Add(column);
                    }
                }
            }

            if (Columns.Count < 1) throw new Exceptions.ReSQLEmptyTableException();
        }
    }
}