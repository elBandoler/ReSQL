using System;
using ReSQL.Models;
using ReSQL.Attributes;
using ReSQL.Data;
using ReSQL;
using System.Linq;
using System.Collections.Generic;

namespace ReSQLTest
{
    class Program
    {
        // hardcoded config to reduce damages when hacking
        private static string database = "bandoleril_altv";
        private static string user = "bandoleril_altv";
        private static string password = "dWOHFrYQ2";
        private static string server = "185.185.135.201";

        static void Main(string[] args)
        {
            Console.WriteLine("Trying to connect to SQL using ReSQL");
            Connection connection = new Connection(server, database, user, password);
            connection.Connect();
            Console.WriteLine("Connected!");

            Console.WriteLine("Creating the structure from the Test class, or updating it if changed.");
            DatabaseStructure db = new DatabaseStructure();
            Table TestTable = new Table(typeof(Test));
            db.AddTable(TestTable);
            Console.WriteLine("Success!");

            Console.WriteLine("Trying to update the structure!");
            connection.UpdateStructure(db, true);
            Console.WriteLine("Success!");

            Console.WriteLine("Trying to insert an object into the structure!");
            Test t = new Test() { dateTime = DateTime.Now.ToString(), Name = "LEL", hehe=1, lul=new Test2() { lel = "LEL" } };
            t.Id = (int)connection.Insert(t ,TestTable); // should NOT do a casting. Use long for Id, but I'm lazy, and nobody cares.
            Console.WriteLine("Success!");

            Console.WriteLine("Trying to UPDATE an object on the DB.");
            t.Name = "LOLLLL";
            connection.Update(t, TestTable);
            Console.WriteLine("Success!");

            Console.WriteLine("Trying to LOAD the object from the DB to an object variable and print the data out of it.");
            Test p = connection.Load<Test>(TestTable, $"`Id`={t.Id}");
            if (p == null) return;
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(p));
            Console.WriteLine("Success!");

            Console.WriteLine("To continue to the removal of the object, press any key.");
            Console.ReadKey();

            Console.WriteLine("Trying to REMOVE the object from the DB.");
            connection.Delete(TestTable, t);
            if (connection.Select(TestTable, $"Id='{t.Id}'").Count > 0)
                Console.WriteLine("Found the ID already in table! Error deleting!");
            else 
                Console.WriteLine("Success!");

            Console.WriteLine("To continue to the removal of the table, press any key.");
            Console.ReadKey();

            Console.WriteLine("Trying to REMOVE the table.");
            connection.Drop(TestTable);
            Console.WriteLine("Success!");

            Console.WriteLine("Trying to disconnect to SQL using ReSQL");
            connection.Disconnect();
            Console.WriteLine("Disconnected!");

            // PART TWO - BENCHMARKING

            
            int amount = 0;
            Console.WriteLine("Ready! To start benchmarking, please insert the amount of objects to create.");
            while(true)
            {
                if (int.TryParse(Console.ReadLine(), out amount))
                {
                    if(amount > 0)
                        break;
                }
                else
                {
                    Console.Beep();
                    Console.WriteLine("Invalid amount, try again.");
                }                  
            }
            Console.WriteLine($"Amount: {amount}");

            List<Account> accounts = new List<Account>();
            DateTime start = DateTime.Now;
            DateTime end;

            // accounts generating
            Console.WriteLine($"Generating started at {start}");
            for (int i = 0; i < amount; i++)
            {
                accounts.Add(new Account()
                { 
                    AdminLevel = 0,
                    Bank = 0, 
                    Clan = 1,
                    LastHardwareIdExHash = new long(),
                    LastHardwareIdHash = new long(),
                    LastIP = "192.168.1.1", 
                    Password = "3bafbf08882a2d10133093a1b8433f50563b93c14acd05b79028eb1d12799027241450980651994501423a66c276ae26c43b739bc65c4e16b10c3af6c202aebb", 
                    Skin=4231504125, 
                    SocialClub=41233412415,
                    Tag="{FF0000}מנהל הקהילה",
                    Username="Bandoler"
                });
            }
            end = DateTime.Now;
            Console.WriteLine($"Generating ended at {end}. It took exactly {new TimeSpan(end.Ticks - start.Ticks).TotalSeconds}s");

            start = DateTime.Now;
            Console.WriteLine($"Connecting, adding table to structure and updating structure started at {start}");
            connection.Connect();
            Table table = new Table(typeof(Account), "Accounts_TEST");
            db.AddTable(table);
            connection.UpdateStructure(db);
            end = DateTime.Now;
            Console.WriteLine($"Connecting, adding table to structure and updating structure ended at {end}. It took exactly {new TimeSpan(end.Ticks - start.Ticks).TotalSeconds}s");

            start = DateTime.Now;
            Console.WriteLine($"Inserting all the objects started at {start}");
            foreach (var acc in accounts)
            {
                connection.Insert(acc, table);
            }
            connection.Disconnect();
            end = DateTime.Now;
            Console.WriteLine($"Inserting all the objects ended at {end}. It took exactly {new TimeSpan(end.Ticks - start.Ticks).TotalSeconds}s");

        }
    }

    class Test
    {
        [ReSQLSave(autoIncrement: true, isNull: false, indexType: IndexType.PRIMARY)]
        public int Id { get; set; }
        [ReSQLSave]
        public string Name { get; set; }
        [ReSQLSave]
        public string dateTime { get; set; }
        [ReSQLSave]
        public uint hehe { get; set; }
        [ReSQLSave]
        public Test2 lul { get; set; }
    }

    class Test2
    {
        public string lel;
    }

    public class Account
    {
        [ReSQLSave(autoIncrement: true, isNull: false, indexType: IndexType.PRIMARY)]
        public int Id { get; set; }

        [ReSQLSave]
        public string Username { get; set; }

        [ReSQLSave]
        public string Password { get; set; } // always hashed ^_^

        [ReSQLSave]
        public string LastIP { get; set; }

        [ReSQLSave]
        public ulong LastHardwareIdHash { get; set; }

        [ReSQLSave]
        public ulong LastHardwareIdExHash { get; set; }

        [ReSQLSave]
        public ulong SocialClub { get; set; }

        [ReSQLSave]
        public uint AdminLevel { get; set; }

        [ReSQLSave]
        public uint Skin { get; set; }

        [ReSQLSave]
        public int Bank { get; set; }

        [ReSQLSave]
        public int Clan { get; set; }

        [ReSQLSave]
        public string Tag { get; set; }

    }
}
