using System;
using System.Collections.Generic;
using Persistance;
using MySql.Data.MySqlClient;
namespace DAL
{
    public class InvoicesDAL
    {
        private MySqlConnection connection = DBhelper.GetConnection();
        private string query;
        public bool CreateInvoice(Invoice invoice)
        {
            if (invoice == null || invoice.Items == null || invoice.Items.Count == 0)
            {
                return false;
            }
            bool result = false;
            lock (connection)
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.Connection = connection;
                    // lock all table
                    command.CommandText = "lock tables TableFood write, Invoices write, Items write, Invoice_Details write;";
                    command.ExecuteNonQuery();
                    // transaction data
                    MySqlTransaction transaction = connection.BeginTransaction();
                    command.Transaction = transaction;
                    MySqlDataReader reader = null;
                    try
                    {
                        //Update TableStatus
                        command.CommandText = $"update TableFood set Tables_Status = @Tables_Status where Tables_ID = @Tables_ID;";
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@Tables_Status", TableFood.INUSE_STATUS);
                        command.Parameters.AddWithValue("@Tables_ID", invoice.table.TableId);
                        command.ExecuteNonQuery();
                        //Insert Invoice
                        command.CommandText = "insert into Invoices(TableID_FK, Invoices_Status) values (@TableID_FK, @Invoices_Status);";
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@TableID_FK", invoice.table.TableId);
                        command.Parameters.AddWithValue("@Invoices_Status", InvoiceStatus.NEW_INVOICE);
                        command.ExecuteNonQuery();
                        //New Invoice ID
                        command.CommandText = "select LAST_INSERT_ID() as Invoices_ID";
                        reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            invoice.Invoice_ID = reader.GetInt32("Invoices_ID");
                        }
                        reader.Close();
                        //Insert InvoicesDetail
                        foreach (var item in invoice.Items)
                        {
                            if (item.ItemsID < 0 || item.Quantity < 0)
                            {
                                throw new Exception("Not Exists Item!");
                            }
                            //get Item(price)
                            command.CommandText = "select Items_Price from Items where Items_ID=@Items_ID";
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@Items_ID", item.ItemsID);
                            reader = command.ExecuteReader();
                            if (!reader.Read())
                            {
                                throw new Exception("Not Exists Item");
                            }
                            item.ItemPrice = reader.GetDecimal("Items_Price");
                            reader.Close();
                            //Insert to InvoiceDetail Table
                            command.CommandText = @"insert into Invoice_details(InvoicesID_FK, ItemsID_FK, Items_Price, count) values 
                            (" + invoice.Invoice_ID + ", " + item.ItemsID + ", " + item.ItemPrice + ", " + item.Quantity + ");";
                            command.ExecuteNonQuery();
                        }
                        //commit transaction
                        transaction.Commit();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        try
                        {
                            transaction.Rollback();
                        }
                        catch { }
                    }
                    finally
                    {
                        //unlock all tables;
                        command.CommandText = "unlock tables;";
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception c)
                {
                    Console.WriteLine(c);

                }
                finally { connection.Close(); }
            }
            return result;
        }
        public Invoice GetInvoicesId(int id)
        {
            Invoice invoices = null;
            lock (connection)
            {
                try
                {
                    connection.Open();
                    query = @"select i.Invoices_ID, i.Invoices_Date, c.Tables_Name, i.Invoices_Status from
                    Invoices i inner join TableFood c on i.TableID_FK = c.Tables_ID where i.Invoices_Status = 1 and i.Invoices_ID = " + id + ";";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        invoices = GetInvoice(reader);
                        reader.Close();
                        query = @"select i.InvoicesID_FK, i.ItemsID_FK, c.Items_Name, c.Items_Price, i.count from 
                                Invoice_details i inner join Items c on i.ItemsID_FK = c.Items_ID where i.InvoicesID_FK = " + id + ";";
                        command.CommandText = query;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Item item = new Item();
                            item.ItemsID = reader.GetInt32("ItemsID_FK");
                            item.ItemName = reader.GetString("Items_Name");
                            item.ItemPrice = reader.GetDecimal("Items_Price");
                            item.Quantity = reader.GetInt32("count");
                            invoices.Items.Add(item);
                        }
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.ReadKey();
                }
                finally
                {
                    connection.Close();

                }
            }
            return invoices;
        }

        public List<Invoice> GetAllInvoice()
        {
            List<Invoice> invoices = new List<Invoice>();
            lock (connection)
            {
                try
                {
                    connection.Open();
                    query = @"select i.Invoices_ID, i.Invoices_Date, c.Tables_Name, i.Invoices_Status from
                    Invoices i inner join TableFood c on i.TableID_FK = c.Tables_ID where i.Invoices_Status = 1;";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        invoices.Add(GetInvoice(reader));
                    }
                    reader.Close();
                    connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.ReadKey();
                }
            }
            if (invoices.Count == 0) invoices = null;
            return invoices;
        }
        public List<Invoice> GetHistory()
        {
            List<Invoice> invoices = new List<Invoice>();
            lock (connection)
            {
                try
                {
                    connection.Open();
                    query = @"select i.Invoices_ID, i.Invoices_Date, c.Tables_Name, i.Invoices_Status from
                    Invoices i inner join TableFood c on i.TableID_FK = c.Tables_ID where i.Invoices_Status > 1;";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        invoices.Add(GetInvoice(reader));
                    }
                    reader.Close();
                    connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            if (invoices.Count == 0) invoices = null;
            return invoices;
        }
        public Invoice GetInvoiceHistory(int id)
        {
            Invoice invoices = null;
            lock (connection)
            {
                try
                {
                    connection.Open();
                    query = @"select i.Invoices_ID, i.Invoices_Date, c.Tables_Name, i.Invoices_Status from
                    Invoices i inner join TableFood c on i.TableID_FK = c.Tables_ID where i.Invoices_Status > 1 and i.Invoices_ID = " + id + ";";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        invoices = GetInvoice(reader);
                        reader.Close();
                        query = @"select i.ItemsID_FK, c.Items_Name, c.Items_Price, i.count from 
                                Invoice_details i inner join Items c on i.ItemsID_FK = c.Items_ID where i.InvoicesID_FK = " + id + ";";
                        command.CommandText = query;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Item item = new Item();
                            item.ItemsID = reader.GetInt32("ItemsID_FK");
                            item.ItemName = reader.GetString("Items_Name");
                            item.ItemPrice = reader.GetDecimal("Items_Price");
                            item.Quantity = reader.GetInt32("count");
                            invoices.Items.Add(item);
                        }
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    connection.Close();

                }
            }
            return invoices;
        }
        public bool GetPayment(int id)
        {
            bool result = false;
            lock (connection)
            {
                try
                {
                    connection.Open();
                    query = $"update Invoices set Invoices_Status = 2 where Invoices_ID = " + id + ";";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                    query = $"select TableID_FK from Invoices where Invoices_ID = " + id + ";";
                    command.CommandText = query;
                    MySqlDataReader reader = command.ExecuteReader();
                    int tableId;
                    if (reader.Read())
                    {
                        tableId = reader.GetInt32("TableID_FK");
                        reader.Close();
                        query = $"update TableFood set Tables_Status = 1 where Tables_ID = " + tableId + ";";
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                    result = true;
                }
                catch (Exception e) { Console.WriteLine(e); }
                return result;
            }
        }
        public bool GetCancelInvoice(int id)
        {
            bool result = false;
            lock (connection)
            {
                try
                {
                    connection.Open();
                    query = $"update Invoices set Invoices_Status = 3 where Invoices_ID = " + id + ";";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                    query = $"select TableID_FK from Invoices where Invoices_ID = " + id + ";";
                    command.CommandText = query;
                    MySqlDataReader reader = command.ExecuteReader();
                    int tableId;
                    if (reader.Read())
                    {
                        tableId = reader.GetInt32("TableID_FK");
                        reader.Close();
                        query = $"update TableFood set Tables_Status = 1 where Tables_ID = " + tableId + ";";
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                    result = true;
                }
                catch (Exception e) { Console.WriteLine(e); }
                return result;
            }
        }
        public bool UpdateQuantityItem(int itemId, int id, int count)
        {
            bool result = false;
            lock (connection)
            {
                try
                {
                    connection.Open();
                    query = "update Invoice_details set count = count + " + count + " where InvoicesID_FK = '" + id + "'and ItemsID_FK = '" + itemId + "';";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            return result;
        }
        public bool UpdateItemNew(int id, int itemId, int count)
        {
            bool result = false;
            lock (connection)
            {
                try
                {
                    connection.Open();
                    // lock all table
                    query = "lock tables TableFood write, Invoices write, Items write, Invoice_Details write;";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                    // transaction data
                    MySqlTransaction transaction = connection.BeginTransaction();
                    command.Transaction = transaction;
                    command.ExecuteNonQuery();
                    decimal Price;
                    try
                    {
                        query = "select Items_Price from Items where Items_ID= " + itemId + ";";
                        command.CommandText = query;
                        MySqlDataReader reader = command.ExecuteReader();
                        if (!reader.Read())
                        {
                            throw new Exception("Not Exists Item");
                        }
                        Price = reader.GetDecimal("Items_Price");
                        reader.Close();
                        query = @"insert into Invoice_details(InvoicesID_FK, ItemsID_FK, Items_Price, count) values 
                            (" + id + ", " + itemId + ", " + Price + ", " + count + ");";
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                        transaction.Commit();
                        result = true;
                    }
                    catch
                    {
                        try
                        {
                            transaction.Rollback();
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                    finally
                    {
                        //unlock all tables;
                        command.CommandText = "unlock tables;";
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally { connection.Close(); }
            }
            return result;
        }
        public bool removeitem(int id)
        {
            bool result = false;
            lock (connection)
            {
                try
                {
                    connection.Open();
                    query = "delete from Invoice_details where ItemsID_FK = " + id + ";";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            return result;
        }
        public int InsertTest(Invoice invoice)
        {
            int? result = null;
            MySqlConnection connection = DBhelper.GetConnection();
            string sql = @"insert into Invoices(TableID_FK, Invoice_Status) values 
                      (@tableid, @invoicestatus);";
            lock (connection)
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@tableid", invoice.table.TableId);
                    command.Parameters.AddWithValue("@invoicestatus", invoice.Invoices_Status);
                    result = command.ExecuteNonQuery();
                }
                catch
                {
                    result = -2;
                }
                finally
                {
                    connection.Close();
                }
            }
            return result ?? 0;
        }
        private Invoice GetInvoice(MySqlDataReader reader)
        {
            Invoice invoices = new Invoice();
            invoices.Invoice_ID = reader.GetInt32("Invoices_ID");
            invoices.Invoices_Date = reader.GetDateTime("Invoices_Date");
            invoices.Invoices_Status = reader.GetInt32("Invoices_Status");
            invoices.table.Name = reader.GetString("Tables_Name");
            return invoices;
        }
    }
}