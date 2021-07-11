using System;
using System.Collections.Generic;

namespace ReSQL.Models
{
    /// <summary>
    /// Contains the structure of the database, and ways to add new tables and/or types
    /// </summary>
    public class DatabaseStructure
    {
        public List<Table> Tables { get; set; }
        public DatabaseStructure(List<Table> tables) => Tables = tables;
        public DatabaseStructure() => Tables = new List<Table>();
        public void AddTable(Table table) => Tables.Add(table);
        public void AddType(Type T) => Tables.Add(new Table(T, T.Name+"s"));
    }
}
