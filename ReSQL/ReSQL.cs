using Exceptions;
using ReSQL.Attributes;
using ReSQL.Data;
using ReSQL.Models;
using ReSQL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReSQL
{
    public static class ReSQL
    {
        public static void UpdateStructure(this Connection connection, DatabaseStructure database, bool alterTables = false)
        {
            if (connection.Conn.State != System.Data.ConnectionState.Open)
                throw new ReSQLQueryConnectionIsNotOpen();

            string query = string.Empty;
            foreach (var table in database.Tables)
            {
                if (!connection.TableExists(table))
                {
                    query = $"CREATE TABLE IF NOT EXISTS {table.Name} (";
                    foreach (var column in table.Columns)
                    {
                        query += $"{column.Name} {Interpret.ColumnTypeName(column.Type)}{" " + Interpret.AttributeName(column.Attribute)}{(column.AutoIncrement ? " AUTO_INCREMENT " : "")}{(column.Nullable ? "" : " NOT NULL ")}{Interpret.IndexTypeName(column.IndexType)},";
                    }
                    query = query.Substring(0, query.Length - 1);
                    query += ");";

#if DEBUG
                    Console.WriteLine(query);
#endif
                    connection.NonQuery(query);
                }
                else if (alterTables) // if we have to and can alter tables, we'll do just that!
                {
                    // if it isn't, we need to: drop columns that are not used, add new ones and alter others

                    // we need to make sure the table exists
                    var results = connection.QueryForResults($"SELECT * FROM `INFORMATION_SCHEMA`.`COLUMNS`  WHERE `TABLE_SCHEMA`= '{connection.Conn.ConnectionString.Split(';')[3].Split('=')[1]}' AND `TABLE_NAME`= '{table.Name}';");

                    if (results.Count == 0)
                        throw new ReSQLTableDoesntExistOnAlter();

                    string add = string.Empty;
                    string drop = string.Empty;
                    string modify = string.Empty;

                    foreach (var line in results)
                    {
                        if (table.Columns.FindAll(column => column.Name == line["COLUMN_NAME"].ToString()).Count != 1) // if the DB's COLUMN was not found in our model, DROP IT
                        {
                            drop += $"DROP COLUMN {line["COLUMN_NAME"]}, ";
                        }
                    }
                    foreach (var column in table.Columns) // go through all of our needed columns
                    {
                        var line = results.Find(line => line["COLUMN_NAME"].ToString().Equals(column.Name)); // find a column in the db, which named like our column
                        if(line == null || line.Count == 0) // no such column. ADD it!
                        {
                            add += $"{column.Name} {Interpret.ColumnTypeName(column.Type)}{" " + Interpret.AttributeName(column.Attribute)}{(column.AutoIncrement ? " AUTO_INCREMENT " : "")}{(column.Nullable ? "" : " NOT NULL ")}{Interpret.IndexTypeName(column.IndexType)}, ";
                            continue;
                        }
                        if (!line["DATA_TYPE"].ToString().Equals(Interpret.ColumnTypeName(column.Type), StringComparison.OrdinalIgnoreCase) || !line["COLUMN_TYPE"].ToString().Contains(Interpret.AttributeName(column.Attribute)) ||  line["IS_NULLABLE"].ToString() != (column.Nullable ? "YES" : "NO") || line["COLUMN_KEY"].ToString() != Interpret.IndexTypeInformationSchemaName(column.IndexType))
                            // the column's properties are different. MODIFY COLUMNS needed!
                        {
                            modify += $"MODIFY COLUMN  {column.Name} {Interpret.ColumnTypeName(column.Type)}{" " + Interpret.AttributeName(column.Attribute)}{(column.AutoIncrement ? " AUTO_INCREMENT " : "")}{(column.Nullable ? "" : " NOT NULL ")}{Interpret.IndexTypeName(column.IndexType)}, ";
                            continue;
                        }
                    }

                    if (add.Length > 2) add = add.Substring(0, add.Length - 2);
                    if (drop.Length > 2) drop = drop.Substring(0, drop.Length - 2) + ";";
                    if (modify.Length > 2) modify = modify.Substring(0, modify.Length - 2);

                    query = (add != string.Empty ? $"ALTER TABLE `{table.Name}` ADD ({add}); " : "") +
                        (drop != string.Empty ? $"ALTER TABLE `{table.Name}` {drop}; " : "") +
                        (modify != string.Empty ? $"ALTER TABLE `{table.Name}` {modify}; " : "");

#if DEBUG
                    Console.WriteLine(query);
#endif

                    if (query.Length > 0)
                        connection.NonQuery(query);
                }
            }
        }

        public static long Insert<T>(this Connection connection, T insertedObject, Table table)
        {
            if (connection.Conn.State != System.Data.ConnectionState.Open)
                throw new ReSQLQueryConnectionIsNotOpen();

            string query = $"INSERT INTO `{table.Name}` (";
            PropertyInfo[] properties = insertedObject.GetType().GetProperties();
            List<string> values = new List<string>();
            foreach (var property in properties)
            {
                ReSQLSaveAttribute attribute = (ReSQLSaveAttribute)property.GetCustomAttribute(typeof(ReSQLSaveAttribute));
                if (attribute != null) // if there is a ReSQLSaveAttribute, only then should go ahead
                {
                    if (!attribute.autoIncrement)
                    {
                        query += $"{property.Name}, ";
                        values.Add(Json.IsJsonNeeded(property) ? 
                            Newtonsoft.Json.JsonConvert.SerializeObject(
                                property.GetValue(insertedObject)
                                ) : 
                            property.GetValue(insertedObject).ToString());
                    }
                }
            }
            query = query.Substring(0, query.Length - 2) + ") VALUES (";
            foreach (var value in values)
                query += $"'{value}',";
            query = query.Substring(0, query.Length - 1) + ");";

#if DEBUG
            Console.WriteLine(query);
#endif

            return connection.NonQuery(query);
        }



        public static T Load<T>(this Connection connection, Table table, string condition)
        {
            var results = connection.Select(table, condition);
            T obj = (T)Activator.CreateInstance(typeof(T));
            foreach (var line in results)
            {
                foreach(var column in line)
                {
                    var property = typeof(T).GetProperty(column.Key);
                    var propertyType = property.PropertyType;
                    if (column.Value.GetType().Equals(typeof(DBNull)))
                    {
                        property.SetValue(obj, Activator.CreateInstance(propertyType));
                    }
                    else if (Json.IsJsonNeeded(property))
                    {
                        property.SetValue(obj, Newtonsoft.Json.JsonConvert.DeserializeObject(column.Value.ToString(), propertyType));
                    }
                    else
                    {
                        property.SetValue(obj, column.Value);
                    }
                }
                break;
            }
            return obj;
        }

        public static List<T> LoadMultiple<T>(this Connection connection, Table table, string condition)
        {
            var results = connection.Select(table, condition);
            List<T> objList = new List<T>();
            foreach (var line in results)
            {
                T obj = (T)Activator.CreateInstance(typeof(T));
                foreach (var column in line)
                {
                    var property = typeof(T).GetProperty(column.Key);
                    var propertyType = property.PropertyType;

                    if (Json.IsJsonNeeded(property))
                    {
                        property.SetValue(obj, Newtonsoft.Json.JsonConvert.DeserializeObject(column.Value.ToString(), propertyType));
                    }
                    else
                    {
                        property.SetValue(obj, column.Value);
                    }
                }
                objList.Add(obj);
            }
            return objList;
        }

        public static List<Dictionary<string, object>> Select(this Connection connection, Table table, string condition = "")
        {
            string query = $"SELECT * FROM `{table.Name}` {(condition.Equals(string.Empty)? "":$"WHERE {condition}")}";
#if DEBUG
            Console.WriteLine(query);
#endif
            var results = connection.QueryForResults(query + ";");
            return results;
        }

        public static void Delete(this Connection connection, Table table, string condition)
        {
            string query = $"DELETE FROM `{table.Name}` WHERE {condition}";
#if DEBUG
            Console.WriteLine(query);
#endif
            connection.NonQuery(query + ";");
            return;
        }

        public static void Delete<T>(this Connection connection, Table table, T obj)
        {
            PropertyInfo idpointer = null;
            foreach (var property in typeof(T).GetProperties())
                if (((ReSQLSaveAttribute)property.GetCustomAttribute(typeof(ReSQLSaveAttribute))).autoIncrement)
                    idpointer = property;

            if (idpointer == null)
                throw new ReSQLIdentifierNotFoundException();

            string query = $"DELETE FROM `{table.Name}` WHERE {idpointer.Name}='{idpointer.GetValue(obj)}';";
#if DEBUG
            Console.WriteLine(query);
#endif
            connection.NonQuery(query + ";");
            return;
        }

        public static void Update(this Connection connection, object o, Table table, string condition = "")
        {
            string query = $"UPDATE `{table.Name}` SET ";
            string where = string.Empty;
            foreach(var property in o.GetType().GetProperties())
            {
                if (((ReSQLSaveAttribute)property.GetCustomAttribute(typeof(ReSQLSaveAttribute))).autoIncrement)
                {
                    if (condition == "")
                        where = $" WHERE {property.Name}={property.GetValue(o)}";
                    continue;
                }
                var value = property.GetValue(o);
                var valueType = value.GetType();
                var propertyName = property.Name;
                if (Json.IsJsonNeeded(property))
                {
                    query += $"{propertyName}='{Newtonsoft.Json.JsonConvert.SerializeObject(value)}', ";
                }
                else
                {
                    query += $"{propertyName}='{value}', ";
                }
            }
            query = query.Substring(0, query.Length - 2);
            if (!where.Equals(string.Empty))
                query += where;
            else
                query += $"WHERE {condition}";

#if DEBUG
            Console.WriteLine(query);
#endif
            connection.NonQuery(query + ";");
            return;
        }

        public static void Drop(this Connection connection, Table table)
        {
            string query = $"DROP TABLE `{table.Name}`";
#if DEBUG
            Console.WriteLine(query);
#endif
            connection.NonQuery(query);
            return;
        }
    }
}
