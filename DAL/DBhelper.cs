using System;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class DBhelper
    {
        private static MySqlConnection connection;
        public static MySqlConnection GetConnection()
        {
            if (connection == null)
            {
                connection = new MySqlConnection
                {
                    ConnectionString = "server=localhost;user id=manager;password=manager1234;port=3306;database=ProjectCSDL;"
                };
            }
            return connection;
        }
        private DBhelper() { }
    }
}
