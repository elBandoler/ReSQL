using Exceptions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReSQL.Models
{
    public class Connection
    {
        internal string ConnectionString { get; }
        internal MySqlConnection Conn = null;

        public Connection(string server, string database, string user, string password, ushort port = 3306, string charset = "utf8")
        {
            ConnectionString = $"server={server};port={port};user={user};password={password};database={database};CharSet={charset};";
            Conn = new MySqlConnection(ConnectionString);
        }

        public void Connect()
        {
            if (Conn.State == System.Data.ConnectionState.Open)
                throw new ReSQLConnectionAlreadyOpenException();
            Conn.Open();
        }

        /// <summary>
        /// Allows to send orders to the server, which are not SELECT (or other) queries.
        /// </summary>
        /// <param name="query"></param>
        internal long NonQuery(string query)
        {
            if (Conn.State != System.Data.ConnectionState.Open)
                throw new ReSQLQueryConnectionIsNotOpen();

            //query = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(query));

            using (var cmd = new MySqlCommand(query, Conn))
            {
                if (cmd.ExecuteNonQuery() != -1)
                    if (query.StartsWith("INSERT"))
                        return cmd.LastInsertedId;
                    else
                        return -2;
            }
            return -1;
        }

        public System.Data.ConnectionState GetState() => Conn.State;


        internal List<Dictionary<string, object>> QueryForResults(string query)
        {
            var results = new List<Dictionary<string, object>>();
            if (Conn.State != System.Data.ConnectionState.Open)
                throw new ReSQLQueryConnectionIsNotOpen();


            using (var cmd = new MySqlCommand(query, Conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        var line = new Dictionary<string, object>();
                        for (var i = 0; i < reader.FieldCount; i++)
                            line.Add(reader.GetName(i), reader.GetValue(i));
                        results.Add(line);
                    }
                }
            }
            return results;
        }
        
        internal bool TableExists(Table table)
        {
            string query = $"SELECT * FROM `information_schema`.`TABLES` WHERE TABLE_NAME='{table.Name}'";
            using (var cmd = new MySqlCommand(query, Conn))
            {
                cmd.Prepare();
                using (var reader = cmd.ExecuteReader())
                    return reader.HasRows;
            }
        }

        public void Disconnect()
        {
            if (Conn.State == System.Data.ConnectionState.Closed)
                throw new ReSQLConnectionAlreadyClosedException();
            Conn.Close();
        }


    }
}
