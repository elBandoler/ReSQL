using AltV.Net;
using deathmatch.Models_and_Factories;
using ReSQL.Attributes;
using ReSQL.Data;
using System.Diagnostics.CodeAnalysis;
using static OsSql.OsSqlTypes;

namespace deathmatch
{
    public class Account
    {
        [ReSQLSave(AutoInc: true)]
        [NotNull]
        public int Id { get; set; }

        [ReSQLSave]
        [NotNull]
        public string Username { get; set; }

        [ReSQLSave]
        [NotNull]
        public string Password { get; set; } // always hashed ^_^

        [ReSQLSave]
        [NotNull]
        public string LastIP { get; set; }

        [ReSQLSave]
        [NotNull]
        public ulong LastHardwareIdHash { get; set; }

        [ReSQLSave]
        [NotNull]
        public ulong LastHardwareIdExHash { get; set; }

        [ReSQLSave]
        [NotNull]
        public ulong SocialClub { get; set; }

        [ReSQLSave]
        [NotNull]
        public uint AdminLevel { get; set; }

        [ReSQLSave]
        [NotNull]
        public uint Skin { get; set; }
        
        [ReSQLSave]
        [NotNull]
        public int Bank { get; set; }

        [ReSQLSave]
        [NotNull]
        public int Clan { get; set; }

        [ReSQLSave]
        public string Tag { get; set; }

        [ReSQLSave(columnType: ColumnType.Object)]
        public Punishment Punishment { get; set; }

    }

    public static class AccountExtensions
    {
        public static Account LoadAccount(Player player)
        {
            Account account;
            try
            {
                Database.conn.Connect();
                account = Database.conn.AutoLoad<Account>(Database.Table_Accounts, $"`Username`='{player.Name}'", out _);
            }
            catch
            {
                account = null;
            }
            if (Database.conn.Connection.State == System.Data.ConnectionState.Open)
                Database.conn.Disconnect();
            return account;
        }

        public static Account LoadAccount(string playername)
        {
            Account account;
            try
            {
                Database.conn.Connect();
                account = Database.conn.AutoLoad<Account>(Database.Table_Accounts, $"`Username`='{playername}'", out _);
            }
            catch
            {
                account = null;
            }
            if (Database.conn.Connection.State == System.Data.ConnectionState.Open)
                Database.conn.Disconnect();
            return account;
        }

        public static bool Save(this Account Account)
        {
            Database.conn.Connect();
            if(Database.conn.Select(Database.Table_Accounts, $"Id={Account.Id}").Count == 0)
            {
                Alt.Log($"{{FF0000}}Database system tried to save DBID {Account.Id}, but there is no such account!");
            }
            else
            {
                Database.conn.AutoUpdate(Account, Database.Table_Accounts, $"Id={Account.Id}");
            }
            Database.conn.Disconnect();
            return true;
        }
    }
}