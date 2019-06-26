using System;
using System.Data.Common;
using System.Data;
using MySql.Data.MySqlClient;

namespace SpeedTestCollector.util
{
    public class MySQLClass
    {
        const string _connectionString = "SERVER={0};" +
                            "UID={1};" +
                            "PASSWORD={2};" +
                            "DATABASE={3};";
        private string _host { get; set; }
        private string _username { get; set; }
        private string _password { get; set; }
        private string _database { get; set; }

        private MySqlConnection _connection;
        public MySQLClass(string host, string username, string password, string database)
        {
            this._host = host;
            this._username = username;
            this._password = password;
            this._database = database;
        }

        public MySqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new MySqlConnection(String.Format(_connectionString, _host, _username, _password, _database));
                }

                return _connection;
            }
        }

        public void Connect()
        {
            Logger.Print("Verbinde mit MySQL-Datenbank...");
            Connection.Open();
        }

        public void Close()
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
                Logger.Print("Verbindung zur MySQL-Datenbank geschlossen!");
            }
        }

        public void Update(string sql)
        {
            MySqlCommand cmd = Connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        public MySqlDataReader Query(string sql)
        {
            MySqlCommand cmd = Connection.CreateCommand();
            cmd.CommandText = sql;
            return cmd.ExecuteReader();
        }
        public static string GetTableName()
        {
            DateTime now = DateTime.Now;

            string format = Program.Config.Config.mysql.format;
            string year = now.ToString("yyyy");
            string month = now.ToString("MM");

            return String.Format(format, year, month);
        }

        public static string GetPrevTableName()
        {
            DateTime now = DateTime.Now;

            string format = Program.Config.Config.mysql.format;
            string year = now.ToString("yyyy");
            string month = now.ToString("MM");

            int m = Int32.Parse(month) - 1;
            int y = Int32.Parse(year);

            if (m == 0)
            {
                m = 12;
                y--;
            }

            year = String.Format("{0:d4}", y);
            month = String.Format("{0:d2}", m);

            return String.Format(format, year, month);
        }
    }
}