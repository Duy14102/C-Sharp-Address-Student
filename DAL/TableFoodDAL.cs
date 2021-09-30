using System;
using System.Collections.Generic;
using Persistance;
using MySql.Data.MySqlClient;
namespace DAL
{
    public class TableFoodDAL
    {
        private MySqlConnection connection = DBhelper.GetConnection();
        private string query;
        public List<TableFood> GetAllTableFood()
        {
            List<TableFood> tables = new List<TableFood>();
            lock (connection)
            {
                try
                {
                    connection.Open();
                    query = "select * from TableFood;";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        tables.Add(GetTableFood(reader));
                    }
                    reader.Close();
                    connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadKey();
                }
            }
            if (tables.Count == 0) tables = null;
            return tables;
        }
        public TableFood GetById(int id)
        {
            TableFood table = null;
            lock (connection)
            {
                try
                {
                    connection.Open();
                    query = $"select * from TableFood where Tables_ID = {id};";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        table = GetTableFood(reader);
                    }
                    reader.Close();
                    connection.Close();
                }
                catch { }
            }
            return table;
        }
        public TableFood TableStatusCheck(int status)
        {
            TableFood table = null;
            lock (connection)
            {
                try
                {
                    connection.Open();
                    query = $"select * from TableFood where Tables_Status = {status};";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        table = GetTableFood(reader);
                    }
                    reader.Close();
                    connection.Close();
                }
                catch { }
            }
            return table;
        }
        public int? AddTable(TableFood table)
        {
            int? result = 0;
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            MySqlCommand cmd = new MySqlCommand("sp_createTable", connection);
            try
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tableName", table.Name);
                cmd.Parameters["@tableName"].Direction = System.Data.ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@tableId", MySqlDbType.Int32);
                cmd.Parameters["@tableId"].Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                result = (int)cmd.Parameters["@tableId"].Value;
            }
            catch { }
            finally { connection.Close(); }
            return result;
        }
        public bool TableStatusChange(int status, int tableID)
        {
            bool result = false;
            lock (connection)
            {
                try
                {
                    connection.Open();
                    query = $"update TableFood set Tables_Status = {status} where Tables_ID = {tableID};";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    result = true;

                }
                catch { }
            }
            return result;
        }
        public TableFood TableTest(TableFood tableFood)
        {
            TableFood table = null;
            lock (connection)
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "select * from TableFood where Tables_Name ='" + tableFood.Name + "';";
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        table = GetTableFood(reader);
                    }
                    reader.Close();
                    connection.Close();
                }
                catch { }
            }
            return table;
        }
        internal TableFood GetTableFood(MySqlDataReader reader)
        {
            TableFood table = new TableFood();
            table.TableId = reader.GetInt32("Tables_ID");
            table.Name = reader.GetString("Tables_Name");
            table.Status = reader.GetInt32("Tables_Status");
            return table;
        }
    }
}