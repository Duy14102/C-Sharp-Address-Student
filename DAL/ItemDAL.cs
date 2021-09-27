using System;
using System.Collections.Generic;
using Persistance;
using MySql.Data.MySqlClient;
namespace DAL
{
    public class ItemDAL
    {
        private MySqlConnection connection = DBhelper.GetConnection();
        private string query;
        public List<Item> GetItems()
        {
            List<Item> items = new List<Item>();
            lock (connection)
            {
                try
                {
                    connection.Open();
                    query = @"select i.Items_ID, i.Items_Name, i.Items_Price, c.Category_Name from
                    Items i inner join Category c on i.CategoryID_FK = c.Category_ID;";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        items.Add(GetItem(reader));
                    }
                    reader.Close();
                    connection.Close();
                }
                catch { }
            }
            return items;
        }
        public Item GetById(int id)
        {
            Item item = null;
            lock (connection)
            {
                try
                {
                    connection.Open();
                    query = @"select i.Items_ID, i.Items_Name, i.Items_Price, c.Category_Name from
                    Items i inner join Category c on i.CategoryID_FK = c.Category_ID where i.Items_ID = " + id + ";";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        item = GetItem(reader);
                    }
                    reader.Close();
                    connection.Close();
                }
                catch { }
            }
            return item;
        }
        public List<Item> GetByName(Item item)
        {
            List<Item> itemlist = null;
            lock (connection)
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("", connection);
                    query = @"select i.Items_ID, i.Items_Name, i.Items_Price, c.Category_Name from Items i inner join Category c on i.CategoryID_FK = c.Category_ID
                    where Items_Name like concat('%',@Items_Name,'%');";
                    command.Parameters.AddWithValue("@Items_Name", item.ItemName);
                    command.CommandText = query;
                    MySqlDataReader reader = command.ExecuteReader();
                    itemlist = new List<Item>();
                    while (reader.Read())
                    {
                        itemlist.Add(GetItem(reader));
                    }
                    reader.Close();
                }
                catch { }
                finally { connection.Close(); }
            }
            return itemlist;
        }

        public int? AddItem(Item item)
        {
            int? result = -1;
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            MySqlCommand cmd = new MySqlCommand("sp_createItem", connection);
            try
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@itemName", item.ItemName);
                cmd.Parameters["@itemName"].Direction = System.Data.ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@itemPrice", item.ItemPrice);
                cmd.Parameters["@itemPrice"].Direction = System.Data.ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@categoryId", item.CategoryInfo.CategoryID);
                cmd.Parameters["@categoryId"].Direction = System.Data.ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@itemId", MySqlDbType.Int32);
                cmd.Parameters["@itemId"].Direction = System.Data.ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                result = (int)cmd.Parameters["@itemId"].Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            finally { connection.Close(); }
            return result;
        }
        public Item GetItem(MySqlDataReader reader)
        {
            Item items = new Item();
            items.ItemsID = reader.GetInt32("Items_ID");
            items.ItemName = reader.GetString("Items_Name");
            items.ItemPrice = reader.GetDecimal("Items_Price");
            items.CategoryInfo.CategoryName = reader.GetString("Category_Name");
            return items;
        }

    }
}