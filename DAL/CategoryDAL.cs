using System;
using System.Collections.Generic;
using Persistance;
using MySql.Data.MySqlClient;
namespace DAL
{
    public class CategoryDAL
    {
        private MySqlConnection connection = DBhelper.GetConnection();
        private string query;
        public List<Category> GetAllCategory()
        {
            List<Category> category = new List<Category>();
            lock (connection)
            {
                try
                {
                    connection.Open();
                    query = @"select * from Category;";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        category.Add(GetCategory(reader));
                    }
                    reader.Close();
                    connection.Close();
                }
                catch { }
            }
            return category;
        }
        public Category GetCategoryById(int id)
        {
            Category category = null;
            lock (connection)
            {
                try
                {
                    connection.Open();
                    query = $"select * from Category where Category_ID = {id};";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        category = GetCategory(reader);
                    }
                    reader.Close();
                    connection.Close();
                }
                catch { }
            }
            return category;
        }
        public Category GetCategory(MySqlDataReader reader)
        {
            Category category = new Category();
            category.CategoryID = reader.GetInt32("Category_ID");
            category.CategoryName = reader.GetString("Category_Name");
            return category;
        }
    }
}