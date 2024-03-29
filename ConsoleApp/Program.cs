﻿using System;
using System.Collections.Generic;
using Persistance;
using BL;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        private static StaffBL staffBL = new StaffBL();
        private static TableFoodBL tableBL = new TableFoodBL();

        private static ItemBL itemBL = new ItemBL();
        private static InvoicesBL invoicesBL = new InvoicesBL();
        private static CategoryBL categoryBL = new CategoryBL();
        static void Main(string[] args)
        {
            Staff staff = null;
            int choice = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("┼────────────────────────────────────────────────────────┼");
                Console.WriteLine("│                                                        │");
                Console.WriteLine("│        _        _____    _____    __    __  ___        │");
                Console.WriteLine("│       │ │      │     │  │ ___ │  │  │  │  │/   │       │");
                Console.WriteLine("│       │ │      │ ___ │  ││___││  │  │  │   /│  │       │");
                Console.WriteLine("│       │ │      ││___││  │_____│  │  │  │  │ │  │       │");
                Console.WriteLine("│       │ │___   │     │   _____│  │  │  │  │ │  │       │");
                Console.WriteLine("│       │_____│  │_____│  │_____│  │__│  │__│ │__│       │");
                Console.WriteLine("│                                                        │");
                Console.WriteLine("┼────────────────────────────────────────────────────────┼");
                Console.Write("                    User Name : ");
                string userName = Console.ReadLine();
                Console.WriteLine("         ┼────────────────────────────────────┼");
                Console.Write("                    Password : ");
                string pass = GetPassword();
                Console.WriteLine();
                //valid username password
                staff = staffBL.Login(new Staff { Username = userName, Userpass = pass });
                if (staff == null)
                {
                    Console.Clear();
                    Console.WriteLine("┼──────────────────────────────────────────────┼");
                    Console.WriteLine("│                                              │");
                    Console.Write("│       ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Incorrect Username or Password!");
                    Console.ResetColor();
                    Console.WriteLine("        │");
                    Console.WriteLine("│                                              │");
                    Console.WriteLine("┼──────────────────────────────────────────────┼");
                    Console.Write("Press any key to login again....");
                    Console.ReadKey(true);
                }
            } while (staff == null);
            switch (staff.Role)
            {
                case Staff.STAFF_ROLE:
                    staff = new Staff() { Role = 1 };
                    staff.Role = 1;
                    string[] menuItemsStaff = { "CREATE ORDER\t\t\t\t\t       │", "INVOICE\t\t\t\t\t\t       │", "HISTORY\t\t\t\t\t\t       │", "EXIT\t\t\t\t\t\t       │" };
                    do
                    {
                        choice = Menu("POPULAR RICE SYSTEM", "Staff Menu", menuItemsStaff);
                        switch (choice)
                        {
                            case 1:
                                CreateOrder(staff);
                                break;
                            case 2:
                                MenuInvoice(staff);
                                break;
                            case 3:
                                History(staff);
                                break;
                        }
                    } while (choice != menuItemsStaff.Length);
                    break;
                case Staff.ADMIN_ROLE:
                    staff = new Staff() { Role = 2 };
                    staff.Role = 2;
                    string[] menuItemsAdmin = { "CREATE ORDER\t\t\t\t\t       │", "TABLE MANAGEMENT\t\t\t\t       │", "MENU MANAGEMENT\t\t\t\t\t       │", "INVOICE\t\t\t\t\t\t       │", "HISTORY\t\t\t\t\t\t       │", "EXIT\t\t\t\t\t\t       │" };
                    List<TableFood> tablesAdmin = new List<TableFood>();
                    do
                    {
                        choice = Menu("POPULAR RICE SYSTEM", "Admin Menu", menuItemsAdmin);
                        switch (choice)
                        {
                            case 1:
                                CreateOrder(staff);
                                break;
                            case 2:
                                CreateTable(staff);
                                break;
                            case 3:
                                MenuManagement(staff);
                                break;
                            case 4:
                                MenuInvoice(staff);
                                break;
                            case 5:
                                History(staff);
                                break;

                        }
                    } while (choice != menuItemsAdmin.Length);
                    break;
            }
        }
        static void History(Staff staff)
        {
            List<Invoice> invoices = new List<Invoice>();
            int choosehistory;
            do
            {
                string lined = "┼──────────────────────────────────────────────────────────────────────────┼";
                string len = "┼──────────────────────────────────────────────────────────────────┼";
                string blen = "│                                                                  │";
                Console.Clear();
                invoices = invoicesBL.GetHistory();
                if (invoices == null)
                {
                    Console.Clear();
                    Console.WriteLine(len);
                    Console.WriteLine("│\t\t\t POPULAR RICE SYSTEM\t\t\t   │");
                    Console.WriteLine("│\t\t    ┼───────────────────────────┼\t\t   │");
                    Console.WriteLine("│\t\t\t\tHistory\t\t\t\t   │");
                    Console.WriteLine(len);
                    Console.WriteLine(blen);
                    Console.WriteLine("│\t\t\t   Nothing to Show!\t\t\t   │");
                    Console.WriteLine(blen);
                    Console.WriteLine(len);
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                    break;
                }
                else
                {
                    DisplayHistory(invoices);
                }
                Console.Write("Select Id to view detail or input 0 to back to menu : ");
                choosehistory = GetID();
                if (choosehistory == 0) break;
                Invoice invoice = invoicesBL.GetInvoiceHistory(choosehistory);
                if (invoice == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Invalid input, re-enter : ");
                    Console.ResetColor();
                    choosehistory = GetID();
                }
                else
                {
                    decimal a = 0;
                    Console.Clear();
                    Console.WriteLine(lined);
                    Console.WriteLine("│\t\t\t\tInvoice {0} History\t\t\t   │", choosehistory);
                    Console.WriteLine(lined);
                    Console.WriteLine("│ {0,-5} │ {1,-23} │ {2,-10} │ {3,-10} │ {4,-12} │", "Id", "Name", "Price(Vnd)", "Quantity", "Amount");
                    Console.WriteLine(lined);
                    for (int i = 0; i < invoice.Items.Count; i++)
                    {
                        Console.WriteLine("│ {0,-5} │ {1,-23} │ {2,-10} │ {3,-10} │ {4,-12} │", invoice.Items[i].ItemsID, invoice.Items[i].ItemName, invoice.Items[i].ItemPrice, invoice.Items[i].Quantity, invoice.Items[i].ItemPrice * (decimal)invoice.Items[i].Quantity);
                        Console.WriteLine(lined);
                        a += invoice.Items[i].ItemPrice * (decimal)invoice.Items[i].Quantity;
                    }
                    Console.WriteLine("│\t\t\t\t\t        Total Price : {0,-13}│", a);
                    Console.WriteLine(lined);
                    Console.WriteLine("Press any key to back to menu...");
                    Console.ReadKey();
                }
            } while (choosehistory != 0);
        }
        static void MenuManagement(Staff staff)
        {
            int chooseit;
            Console.Clear();
            string linet = "┼───────────────────────────────────────────────────────────────────────┼";
            string titlet = "MENU";
            int positiont = linet.Length / 2 - titlet.Length / 2;
            string line = "┼──────────────────────────────────────────────────────────┼";
            string len = "┼──────────────────────────────────────────────────────────────────┼";
            string blen = "│                                                                  │";
            string title = "MENU MANAGEMENT";
            int position = line.Length / 2 - title.Length / 2;
            Console.WriteLine(line);
            Console.WriteLine("│\t\t     POPULAR RICE SYSTEM\t\t   │");
            Console.WriteLine("│\t\t┼───────────────────────────┼\t\t   │");
            Console.WriteLine("│{0," + position + "}\b{1}\t\t\t   │", "", title);
            Console.WriteLine(line);
            Console.WriteLine("│  1  │ Add Item\t\t\t\t\t   │");
            Console.WriteLine(line);
            Console.WriteLine("│  2  │ Remove Item\t\t\t\t\t   │");
            Console.WriteLine(line);
            Console.WriteLine("│  3  │ Get item by id\t\t\t\t\t   │");
            Console.WriteLine(line);
            Console.WriteLine("│  4  │ Get item by name\t\t\t\t   │");
            Console.WriteLine(line);
            Console.WriteLine("│  5  │ Get all item\t\t\t\t\t   │");
            Console.WriteLine(line);
            Console.WriteLine("│  6  │ Exit to main menu\t\t\t\t   │");
            Console.WriteLine(line);
            Console.WriteLine();
            Console.Write("  ───> Your choice : ");
            chooseit = GetID();
            while (chooseit < 1 || chooseit > 6)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Invalid Input, re-enter : ");
                Console.ResetColor();
                chooseit = GetID();
            }
            switch (chooseit)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine(len);
                    Console.WriteLine(blen);
                    Console.WriteLine("│\t\t\t POPULAR RICE SYSTEM\t\t\t   │");
                    Console.WriteLine("│\t\t    ┼───────────────────────────┼\t\t   │");
                    Console.WriteLine("│\t\t\t      Create Item\t\t\t   │");
                    Console.WriteLine(blen);
                    Console.WriteLine(len);
                    Console.Write("Are you sure you want to add item ? (yes/no) : ");
                    string tookit = Console.ReadLine();
                    do
                    {
                        if (tookit == "yes")
                        {
                            List<Category> categories = new List<Category>();
                            Category category = new Category();
                            Item item = new Item();
                            Console.Clear();
                            Console.WriteLine(len);
                            Console.WriteLine(blen);
                            Console.WriteLine("│\t\t\t POPULAR RICE SYSTEM\t\t\t   │");
                            Console.WriteLine("│\t\t    ┼───────────────────────────┼\t\t   │");
                            Console.WriteLine("│\t\t\t      Create Item\t\t\t   │");
                            Console.WriteLine(blen);
                            Console.WriteLine(len);
                            Console.Write("Name of new item : ");
                            item.ItemName = Console.ReadLine();
                            Console.Write("Price of new item : ");
                            item.ItemPrice = decimal.Parse(Console.ReadLine());
                            categories = categoryBL.GetAllCategory();
                            DisplayCategory(categories);
                            Console.Write("Your choice : ");
                            int categoryitem = Getidover0();
                            category = categoryBL.GetCategoryById(categoryitem);
                            while (category == null)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Category Not found!");
                                Console.ResetColor();
                                Console.Write("Try Again : ");
                                categoryitem = Getidover0();
                                category = categoryBL.GetCategoryById(categoryitem);
                            }
                            item.CategoryInfo = category;
                            itemBL.AddItem(item);
                            Console.Clear();
                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                            Console.WriteLine("│                                              │");
                            Console.Write("│           ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Add Item Successfully!");
                            Console.ResetColor();
                            Console.WriteLine("             │");
                            Console.WriteLine("│                                              │");
                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                            Console.Write("Press any key to continue...");
                            Console.ReadKey();
                            break;
                        }
                        else if (tookit == "no")
                        {
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Invalid input, re-enter : ");
                            Console.ResetColor();
                            tookit = Console.ReadLine();
                        }
                    } while (tookit != "no");
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine(len);
                    Console.WriteLine(blen);
                    Console.WriteLine("│\t\t\t POPULAR RICE SYSTEM\t\t\t   │");
                    Console.WriteLine("│\t\t    ┼───────────────────────────┼\t\t   │");
                    Console.WriteLine("│\t\t\t      Remove Item\t\t\t   │");
                    Console.WriteLine(blen);
                    Console.WriteLine(len);
                    Console.Write("Are you sure you want to remove item ? (yes/no) : ");
                    string takeit = Console.ReadLine();
                    do
                    {
                        if (takeit == "yes")
                        {
                            Console.Clear();
                            Console.WriteLine(len);
                            Console.WriteLine(blen);
                            Console.WriteLine("│\t\t\t POPULAR RICE SYSTEM\t\t\t   │");
                            Console.WriteLine("│\t\t    ┼───────────────────────────┼\t\t   │");
                            Console.WriteLine("│\t\t\t      Remove Item\t\t\t   │");
                            Console.WriteLine(blen);
                            Console.WriteLine(len);
                            Console.Write("Input Id of item to remove : ");
                            int getit = GetID();
                            Item item = itemBL.GetById(getit);
                            if (item == null)
                            {
                                Console.Clear();
                                Console.WriteLine("┼──────────────────────────────────────────────┼");
                                Console.WriteLine("│                                              │");
                                Console.Write("│              ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("Item Id invalid!");
                                Console.ResetColor();
                                Console.WriteLine("                │");
                                Console.WriteLine("│                                              │");
                                Console.WriteLine("┼──────────────────────────────────────────────┼");
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                            }
                            else
                            {
                                Console.WriteLine(linet);
                                Console.WriteLine("│ {0,-10} │ {1,-20} │ {2,-15} │ {3,-15} │", "ID", "Name", "Price(Vnd)", "Category");
                                Console.WriteLine(linet);
                                Console.WriteLine("│ {0,-10} │ {1,-20} │ {2,-15} │ {3, -15} │", item.ItemsID, item.ItemName, item.ItemPrice, item.CategoryInfo.CategoryName);
                                Console.WriteLine(linet);
                                Console.Write("Are you sure you want to remove this item ? (yes/no) : ");
                                string yesno = Console.ReadLine();
                                do
                                {
                                    if (yesno == "yes")
                                    {
                                        bool wow = itemBL.RemoveItem(getit);
                                        if (wow)
                                        {
                                            Console.Clear();
                                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                                            Console.WriteLine("│                                              │");
                                            Console.Write("│           ");
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.Write("Remove Item Complete!");
                                            Console.ResetColor();
                                            Console.WriteLine("              │");
                                            Console.WriteLine("│                                              │");
                                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                                            Console.Write("Press any key to continue...");
                                            Console.ReadKey();
                                        }
                                        else
                                        {
                                            Console.Clear();
                                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                                            Console.WriteLine("│                                              │");
                                            Console.Write("│              ");
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.Write("Remove Item Fail!");
                                            Console.ResetColor();
                                            Console.WriteLine("               │");
                                            Console.WriteLine("│                                              │");
                                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                                            Console.Write("Press any key to continue...");
                                            Console.ReadKey();
                                        }
                                        break;
                                    }
                                    else if (yesno == "no")
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.Write("Invalid input, re-enter : ");
                                        Console.ResetColor();
                                        yesno = Console.ReadLine();
                                    }
                                } while (yesno != "no");
                            }
                            break;
                        }
                        else if (takeit == "no")
                        {
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Invalid input, re-enter : ");
                            Console.ResetColor();
                            takeit = Console.ReadLine();
                        }
                    } while (takeit != "no");
                    break;
                case 3:
                    Console.Clear();
                    Console.Write("Input Item ID to search or input 0 to exit to menu : ");
                    int id = GetID();
                    if (id == 0) break;
                    Item itemId = itemBL.GetById(id);
                    if (itemId == null)
                    {
                        Console.Clear();
                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                        Console.WriteLine("│                                              │");
                        Console.Write("│              ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Item Not Found!");
                        Console.ResetColor();
                        Console.WriteLine("                 │");
                        Console.WriteLine("│                                              │");
                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine(linet);
                        Console.WriteLine("│{0," + positiont + "}\b{1}\t\t\t\t\t│", "", titlet);
                        Console.WriteLine(linet);
                        Console.WriteLine("│ {0,-10} │ {1,-20} │ {2,-15} │ {3,-15} │", "ID", "Name", "Price(Vnd)", "Category");
                        Console.WriteLine(linet);
                        Console.WriteLine("│ {0,-10} │ {1,-20} │ {2,-15} │ {3, -15} │", itemId.ItemsID, itemId.ItemName, itemId.ItemPrice, itemId.CategoryInfo.CategoryName);
                        Console.WriteLine(linet);
                        Console.Write("Press any key to back to main menu...");
                        Console.ReadKey();
                    }
                    break;
                case 4:
                    List<Item> items1;
                    Console.Clear();
                    Console.Write("Input ItemName to search or input 0 to back to main menu : ");
                    string name = Console.ReadLine();
                    if (name == "0") break;
                    items1 = itemBL.GetByName(name);
                    if (items1 == null)
                    {
                        Console.Clear();
                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                        Console.WriteLine("│                                              │");
                        Console.Write("│              ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Item Not Found!");
                        Console.ResetColor();
                        Console.WriteLine("                 │");
                        Console.WriteLine("│                                              │");
                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Clear();
                        DisplayItem(items1);
                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                    break;
                case 5:
                    List<Item> items = new List<Item>();
                    Console.Clear();
                    items = itemBL.GetItems();
                    if (items.Count == 0)
                    {
                        Console.Clear();
                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                        Console.WriteLine("│                                              │");
                        Console.WriteLine("│               Nothing to show!               │");
                        Console.WriteLine("│                                              │");
                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        ShowPageItem(items);
                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                    break;
                case 6:
                    break;
            }
        }
        static void CreateTable(Staff staff)
        {
            int chooseit;
            Console.Clear();
            string linek = "┼────────────────────────────────────────────────┼";
            string titlek = "TABLES";
            int positionk = linek.Length / 2 - titlek.Length / 2;
            string line = "┼──────────────────────────────────────────────────────────┼";
            string len = "┼──────────────────────────────────────────────────────────────────┼";
            string blen = "│                                                                  │";
            string title = "TABLE MANAGEMENT";
            int position = line.Length / 2 - title.Length / 2;
            Console.WriteLine(line);
            Console.WriteLine("│\t\t     POPULAR RICE SYSTEM\t\t   │");
            Console.WriteLine("│\t\t┼───────────────────────────┼\t\t   │");
            Console.WriteLine("│{0," + position + "}\b{1}\t\t\t   │", "", title);
            Console.WriteLine(line);
            Console.WriteLine("│  1  │ Add Table\t\t\t\t\t   │");
            Console.WriteLine(line);
            Console.WriteLine("│  2  │ Remove Table\t\t\t\t\t   │");
            Console.WriteLine(line);
            Console.WriteLine("│  3  │ Get table by id\t\t\t\t\t   │");
            Console.WriteLine(line);
            Console.WriteLine("│  4  │ Get table by name\t\t\t\t   │");
            Console.WriteLine(line);
            Console.WriteLine("│  5  │ Get all table\t\t\t\t\t   │");
            Console.WriteLine(line);
            Console.WriteLine("│  6  │ Exit to main menu\t\t\t\t   │");
            Console.WriteLine(line);
            Console.WriteLine();
            Console.Write("  ───> Your choice : ");
            chooseit = GetID();
            while (chooseit < 1 || chooseit > 6)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Invalid Input, re-enter : ");
                Console.ResetColor();
                chooseit = GetID();
            }
            switch (chooseit)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine(len);
                    Console.WriteLine(blen);
                    Console.WriteLine("│\t\t\t POPULAR RICE SYSTEM\t\t\t   │");
                    Console.WriteLine("│\t\t    ┼───────────────────────────┼\t\t   │");
                    Console.WriteLine("│\t\t\t      Create Table\t\t\t   │");
                    Console.WriteLine(blen);
                    Console.WriteLine(len);
                    Console.Write("Are you sure you want to create new table ? (yes/no) : ");
                    string tookit = Console.ReadLine();
                    do
                    {
                        if (tookit == "yes")
                        {
                            Console.Clear();
                            Console.WriteLine(len);
                            Console.WriteLine(blen);
                            Console.WriteLine("│\t\t\t POPULAR RICE SYSTEM\t\t\t   │");
                            Console.WriteLine("│\t\t    ┼───────────────────────────┼\t\t   │");
                            Console.WriteLine("│\t\t\t      Create Table\t\t\t   │");
                            Console.WriteLine(blen);
                            Console.WriteLine(len);
                            Console.Write("Name of new table : ");
                            string nametable = Console.ReadLine();
                            TableFood table = new TableFood { Name = nametable };
                            tableBL.AddTable(table);
                            Console.Clear();
                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                            Console.WriteLine("│                                              │");
                            Console.Write("│          ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Create Table Successfully!");
                            Console.ResetColor();
                            Console.WriteLine("          │");
                            Console.WriteLine("│                                              │");
                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                            Console.Write("Press any key to continue...");
                            Console.ReadKey();
                            break;
                        }
                        else if (tookit == "no")
                        {
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Invalid input, re-enter : ");
                            Console.ResetColor();
                            tookit = Console.ReadLine();

                        }
                    } while (tookit != "no");
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine(len);
                    Console.WriteLine(blen);
                    Console.WriteLine("│\t\t\t POPULAR RICE SYSTEM\t\t\t   │");
                    Console.WriteLine("│\t\t    ┼───────────────────────────┼\t\t   │");
                    Console.WriteLine("│\t\t\t      Remove Table\t\t\t   │");
                    Console.WriteLine(blen);
                    Console.WriteLine(len);
                    Console.Write("Are you sure you want to remove table ? (yes/no) : ");
                    string takeit = Console.ReadLine();
                    do
                    {
                        if (takeit == "yes")
                        {
                            Console.Clear();
                            Console.WriteLine(len);
                            Console.WriteLine(blen);
                            Console.WriteLine("│\t\t\t POPULAR RICE SYSTEM\t\t\t   │");
                            Console.WriteLine("│\t\t    ┼───────────────────────────┼\t\t   │");
                            Console.WriteLine("│\t\t\t      Remove Table\t\t\t   │");
                            Console.WriteLine(blen);
                            Console.WriteLine(len);
                            Console.Write("Input Id of table to remove : ");
                            int getit = GetID();
                            TableFood tableFood = tableBL.GetById(getit);
                            if (tableFood == null)
                            {
                                Console.Clear();
                                Console.WriteLine("┼──────────────────────────────────────────────┼");
                                Console.WriteLine("│                                              │");
                                Console.Write("│             ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("Table Id invalid!");
                                Console.ResetColor();
                                Console.WriteLine("                │");
                                Console.WriteLine("│                                              │");
                                Console.WriteLine("┼──────────────────────────────────────────────┼");
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                            }
                            else if (tableFood.Status == 2)
                            {
                                string status;
                                Console.WriteLine(linek);
                                Console.WriteLine("│{0," + positionk + "}\b{1}\t\t\t │", "", titlek);
                                Console.WriteLine(linek);
                                Console.WriteLine("│ {0,-10} │ {1,-15} │ {2,-15} │", "ID", "Name", "Status");
                                Console.WriteLine(linek);
                                status = tableFood.Status == TableFood.READY_STATUS ? "Ready" : "In Use";
                                Console.WriteLine("│ {0,-10} │ {1,-15} │ {2,-15} │", tableFood.TableId, tableFood.Name, status);
                                Console.WriteLine(linek);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("This table is inuse, cannot remove !");
                                Console.ResetColor();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                            }
                            else
                            {
                                string status;
                                Console.WriteLine(linek);
                                Console.WriteLine("│{0," + positionk + "}\b{1}\t\t\t │", "", titlek);
                                Console.WriteLine(linek);
                                Console.WriteLine("│ {0,-10} │ {1,-15} │ {2,-15} │", "ID", "Name", "Status");
                                Console.WriteLine(linek);
                                status = tableFood.Status == TableFood.READY_STATUS ? "Ready" : "In Use";
                                Console.WriteLine("│ {0,-10} │ {1,-15} │ {2,-15} │", tableFood.TableId, tableFood.Name, status);
                                Console.WriteLine(linek);
                                Console.Write("Are you sure you want to remove this table ? (yes/no) : ");
                                string yesno = Console.ReadLine();
                                do
                                {
                                    if (yesno == "yes")
                                    {
                                        bool wow = tableBL.RemoveItem(getit);
                                        if (wow)
                                        {
                                            Console.Clear();
                                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                                            Console.WriteLine("│                                              │");
                                            Console.Write("│           ");
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.Write("Remove Table Complete!");
                                            Console.ResetColor();
                                            Console.WriteLine("             │");
                                            Console.WriteLine("│                                              │");
                                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                                            Console.Write("Press any key to continue...");
                                            Console.ReadKey();
                                        }
                                        else
                                        {
                                            Console.Clear();
                                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                                            Console.WriteLine("│                                              │");
                                            Console.Write("│             ");
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.Write("Remove Table Fail!");
                                            Console.ResetColor();
                                            Console.WriteLine("               │");
                                            Console.WriteLine("│                                              │");
                                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                                            Console.Write("Press any key to continue...");
                                            Console.ReadKey();
                                        }
                                        break;
                                    }
                                    else if (yesno == "no")
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.Write("Invalid input, re-enter : ");
                                        Console.ResetColor();
                                        yesno = Console.ReadLine();
                                    }
                                } while (yesno != "no");
                            }
                            break;
                        }
                        else if (takeit == "no")
                        {
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Invalid input, re-enter : ");
                            Console.ResetColor();
                            takeit = Console.ReadLine();
                        }
                    } while (takeit != "no");
                    break;
                case 3:
                    Console.Clear();
                    Console.Write("Input Table ID to search or input 0 to exit to menu : ");
                    int id = GetID();
                    if (id == 0) break;
                    TableFood tableFood2 = tableBL.GetById(id);
                    if (tableFood2 == null)
                    {
                        Console.Clear();
                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                        Console.WriteLine("│                                              │");
                        Console.Write("│              ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Table Not Found!");
                        Console.ResetColor();
                        Console.WriteLine("                │");
                        Console.WriteLine("│                                              │");
                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Clear();
                        string status;
                        Console.WriteLine(linek);
                        Console.WriteLine("│{0," + positionk + "}\b{1}\t\t\t │", "", titlek);
                        Console.WriteLine(linek);
                        Console.WriteLine("│ {0,-10} │ {1,-15} │ {2,-15} │", "ID", "Name", "Status");
                        Console.WriteLine(linek);
                        status = tableFood2.Status == TableFood.READY_STATUS ? "Ready" : "In Use";
                        Console.WriteLine("│ {0,-10} │ {1,-15} │ {2,-15} │", tableFood2.TableId, tableFood2.Name, status);
                        Console.WriteLine(linek);
                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                    break;
                case 4:
                    List<TableFood> tableFoods1;
                    Console.Clear();
                    Console.Write("Input table name to search or input 0 to back to main menu : ");
                    string name = Console.ReadLine();
                    if (name == "0") break;
                    tableFoods1 = tableBL.GetByName(name);
                    if (tableFoods1 == null)
                    {
                        Console.Clear();
                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                        Console.WriteLine("│                                              │");
                        Console.Write("│              ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Table Not Found!");
                        Console.ResetColor();
                        Console.WriteLine("                │");
                        Console.WriteLine("│                                              │");
                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Clear();
                        DisplayTables(tableFoods1);
                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                    break;
                case 5:
                    List<TableFood> tableFoods = new List<TableFood>();
                    Console.Clear();
                    tableFoods = tableBL.GetAllTableFood();
                    if (tableFoods == null)
                    {
                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                        Console.WriteLine("│                                              │");
                        Console.WriteLine("│               Nothing to show!               │");
                        Console.WriteLine("│                                              │");
                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        ShowPage(tableFoods);
                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                    break;
                case 6:
                    break;
            }
        }
        static void CreateOrder(Staff staff)
        {
            List<Item> items = new List<Item>();
            List<TableFood> tablesStaff = new List<TableFood>();
            Invoice invoice = new Invoice();
            int choosetable, choosedishes;
            string len = "┼──────────────────────────────────────────────────────────────────┼";
            string blen = "│                                                                  │";
            do
            {
                Console.Clear();
                tablesStaff = tableBL.GetAllTableFood();
                if (tablesStaff == null)
                {
                    Console.Clear();
                    Console.WriteLine(len);
                    Console.WriteLine("│\t\t\t POPULAR RICE SYSTEM\t\t\t   │");
                    Console.WriteLine("│\t\t    ┼───────────────────────────┼\t\t   │");
                    Console.WriteLine("│\t\t\t       Tables\t\t\t\t   │");
                    Console.WriteLine(len);
                    Console.WriteLine(blen);
                    Console.WriteLine("│\t\t None table exists, Create a new one!\t\t   │");
                    Console.WriteLine(blen);
                    Console.WriteLine(len);
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                    break;
                }
                else
                {
                    ShowPage(tablesStaff);
                }
                Console.Write("Select a table to create order or input 0 to back to menu: ");
                choosetable = GetID();
                if (choosetable == 0) break;
                TableFood table = tableBL.GetById(choosetable);
                if (table == null)
                {
                    Console.Clear();
                    Console.WriteLine("┼──────────────────────────────────────────────┼");
                    Console.WriteLine("│                                              │");
                    Console.Write("│              ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Table Id Invalid!");
                    Console.ResetColor();
                    Console.WriteLine("               │");
                    Console.WriteLine("│                                              │");
                    Console.WriteLine("┼──────────────────────────────────────────────┼");
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
                else
                {
                    if (table.Status == 2)
                    {
                        Console.Clear();
                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                        Console.WriteLine("│                                              │");
                        Console.Write("│    ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Table is IN USE, cannot create order!");
                        Console.ResetColor();
                        Console.WriteLine("     │");
                        Console.WriteLine("│                                              │");
                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Clear();
                        invoice.table = table;
                        items = itemBL.GetItems();
                        if (items.Count == 0)
                        {
                            Console.Clear();
                            Console.WriteLine(len);
                            Console.WriteLine("│\t\t\t POPULAR RICE SYSTEM\t\t\t   │");
                            Console.WriteLine("│\t\t    ┼───────────────────────────┼\t\t   │");
                            Console.WriteLine("│\t\t\t       Menu\t\t\t\t   │");
                            Console.WriteLine(len);
                            Console.WriteLine(blen);
                            Console.WriteLine("│\t\t None item exists, Create a new one!\t\t   │");
                            Console.WriteLine(blen);
                            Console.WriteLine(len);
                            Console.Write("Press any key to continue...");
                            Console.ReadKey();
                            break;
                        }
                        while (true)
                        {
                            ShowPageItem(items);
                            Console.Write("Input ID to add dishes and input 0 to finish order : ");
                            choosedishes = GetID();
                            if (choosedishes == 0) break;
                            Item item = itemBL.GetById(choosedishes);
                            if (item == null)
                            {
                                Console.Clear();
                                Console.WriteLine("┼──────────────────────────────────────────────┼");
                                Console.WriteLine("│                                              │");
                                Console.Write("│             ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("Dishes Id Invalid!");
                                Console.ResetColor();
                                Console.WriteLine("               │");
                                Console.WriteLine("│                                              │");
                                Console.WriteLine("┼──────────────────────────────────────────────┼");
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                            }
                            else
                            {
                                Console.WriteLine("┼────────────────────────────────────────────────────────────────┼");
                                Console.WriteLine("│ {0,-10} │ {1,-20} │ {2,-10} │ {3,-13} │", "Id", "Name", "Price(Vnd)", "Category");
                                Console.WriteLine("┼────────────────────────────────────────────────────────────────┼");
                                Console.WriteLine("│ {0,-10} │ {1,-20} │ {2,-10} │ {3,-13} │", item.ItemsID, item.ItemName, item.ItemPrice, item.CategoryInfo.CategoryName);
                                Console.WriteLine("┼────────────────────────────────────────────────────────────────┼");
                                Console.Write("How many (maximum 100): ");
                                int quantitydishes = int.Parse(Console.ReadLine());
                                while (quantitydishes < 1 || quantitydishes > 100)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write("Invalid input, re-enter : ");
                                    Console.ResetColor();
                                    quantitydishes = int.Parse(Console.ReadLine());
                                }
                                bool result2 = false;
                                for (int i = 0; i < invoice.Items.Count; i++)
                                {

                                    if (choosedishes == invoice.Items[i].ItemsID)
                                    {
                                        invoice.Items[i].Quantity += quantitydishes;
                                        result2 = true;
                                        break;
                                    }
                                }
                                if (!result2)
                                {
                                    item.Quantity = quantitydishes;
                                    invoice.Items.Add(item);
                                }
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Added Successfully");
                                Console.ResetColor();
                            }
                        }
                        bool result = invoicesBL.CreateInvoice(invoice);
                        if (result)
                        {
                            Console.Clear();
                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                            Console.WriteLine("│                                              │");
                            Console.Write("│         ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Create Order Successfully!");
                            Console.ResetColor();
                            Console.WriteLine("           │");
                            Console.WriteLine("│                                              │");
                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                            Console.Write("Press any key to continue...");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                            Console.WriteLine("│                                              │");
                            Console.Write("│             ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Create Order Fail!");
                            Console.ResetColor();
                            Console.WriteLine("               │");
                            Console.WriteLine("│                                              │");
                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                            Console.Write("Press any key to continue...");
                            Console.ReadKey();
                        }
                    }
                }
            } while (choosetable != 0);
        }
        static void MenuInvoice(Staff staff)
        {
            List<Item> items = new List<Item>();
            List<Invoice> invoices = new List<Invoice>();
            int chooseit, chooseinvoice, chooseidinvoice, choosefunction, choosefunctionid;
            string answer1, answer2, answer3, answer4;
            Console.Clear();
            string line = "┼──────────────────────────────────────────────────────────┼";
            string len = "┼──────────────────────────────────────────────────────────────────┼";
            string blen = "│                                                                  │";
            string title = "INVOICE SYSTEM";
            int position = line.Length / 2 - title.Length / 2;
            Console.WriteLine(line);
            Console.WriteLine("│\t\t\t\t\t\t\t   │");
            Console.WriteLine("│{0," + position + "}\b{1}\t\t\t   │", "", title);
            Console.WriteLine("│\t\t\t\t\t\t\t   │");
            Console.WriteLine(line);
            Console.WriteLine("│  1  │ Show List Order\t\t\t\t\t   │");
            Console.WriteLine(line);
            Console.WriteLine("│  2  │ Show Order By ID\t\t\t\t   │");
            Console.WriteLine(line);
            Console.WriteLine("│  3  │ Back to menu\t\t\t\t\t   │");
            Console.WriteLine(line);
            Console.WriteLine();
            Console.Write("  ───> Your choice : ");
            chooseit = GetID();
            while (chooseit < 1 || chooseit > 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Invalid input, re-enter : ");
                Console.ResetColor();
                chooseit = GetID();
            }
            switch (chooseit)
            {
                case 1:
                    do
                    {
                        Console.Clear();
                        invoices = invoicesBL.GetAllInvoice();
                        if (invoices == null)
                        {
                            Console.Clear();
                            Console.WriteLine(len);
                            Console.WriteLine("│\t\t\t POPULAR RICE SYSTEM\t\t\t   │");
                            Console.WriteLine("│\t\t    ┼───────────────────────────┼\t\t   │");
                            Console.WriteLine("│\t\t\t     List Order\t\t\t\t   │");
                            Console.WriteLine(len);
                            Console.WriteLine(blen);
                            Console.WriteLine("│\t\t\t   Nothing to Show!\t\t\t   │");
                            Console.WriteLine(blen);
                            Console.WriteLine(len);
                            Console.Write("Press any key to continue...");
                            Console.ReadKey();
                            break;
                        }
                        else
                        {
                            DisplayInvoice(invoices);
                        }
                        Console.Write("Select a Invoice to see details or input 0 to back to menu: ");
                        chooseinvoice = GetID();
                        if (chooseinvoice == 0) break;
                        Invoice invoice = invoicesBL.GetInvoicesId(chooseinvoice);
                        if (invoice == null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Invalid Invoice, re-enter : ");
                            Console.ResetColor();
                            chooseinvoice = GetID();
                        }
                        else
                        {
                            decimal a = 0;
                            string lined = "┼──────────────────────────────────────────────────────────────────────────┼";
                            Console.Clear();
                            Console.WriteLine(lined);
                            Console.WriteLine("│\t\t\t          ORDER {0}            \t\t\t   │", chooseinvoice);
                            Console.WriteLine(lined);
                            Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", "Id", "Name", "Price(Vnd)", "Quantity", "Amount");
                            Console.WriteLine(lined);
                            for (int i = 0; i < invoice.Items.Count; i++)
                            {
                                Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", invoice.Items[i].ItemsID, invoice.Items[i].ItemName, invoice.Items[i].ItemPrice, invoice.Items[i].Quantity, invoice.Items[i].ItemPrice * (decimal)invoice.Items[i].Quantity);
                                Console.WriteLine(lined);
                                a += invoice.Items[i].ItemPrice * invoice.Items[i].Quantity;
                            }
                            Console.WriteLine("│\t\t\t\t\t       Total Price : {0,-14}│", a);
                            Console.WriteLine(lined);
                            Console.WriteLine("│\t\t\t\t\t\t\t\t\t   │");
                            Console.WriteLine("│\t\t\t*****       OPTIONS       *****\t\t\t   │");
                            Console.WriteLine("│\t\t\t\t\t\t\t\t\t   │");
                            Console.WriteLine(lined);
                            Console.WriteLine("│  1  │ Payment\t\t\t\t\t\t\t\t   │");
                            Console.WriteLine(lined);
                            Console.WriteLine("│  2  │ Cancel Invoice\t\t\t\t\t\t\t   │");
                            Console.WriteLine(lined);
                            Console.WriteLine("│  3  │ Add dishes\t\t\t\t\t\t\t   │");
                            Console.WriteLine(lined);
                            Console.WriteLine("│  4  │ Remove dishes\t\t\t\t\t\t\t   │");
                            Console.WriteLine(lined);
                            Console.WriteLine("│  5  │ Back to menu\t\t\t\t\t\t\t   │");
                            Console.WriteLine(lined);
                            Console.WriteLine();
                            Console.Write("  ───> Your choice : ");
                            choosefunction = GetID();
                            while (choosefunction < 1 || choosefunction > 5)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("Invalid input, re-enter : ");
                                Console.ResetColor();
                                choosefunction = GetID();
                            }
                            switch (choosefunction)
                            {
                                case 1://payment
                                    Console.Clear();
                                    Console.WriteLine(lined);
                                    Console.WriteLine("│\t\t\t          PAYMENT      \t\t\t\t   │");
                                    Console.WriteLine(lined);
                                    Console.WriteLine("│ Order Id : {0, -62}│\n│ Date : {1, -66}│\n│ Table : {2, -65}│", invoice.Invoice_ID, invoice.Invoices_Date, invoice.table.Name);
                                    Console.WriteLine(lined);
                                    Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", "Id", "Name", "Price(Vnd)", "Quantity", "Amount");
                                    Console.WriteLine(lined);
                                    for (int i = 0; i < invoice.Items.Count; i++)
                                    {
                                        Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", invoice.Items[i].ItemsID, invoice.Items[i].ItemName, invoice.Items[i].ItemPrice, invoice.Items[i].Quantity, invoice.Items[i].ItemPrice * (decimal)invoice.Items[i].Quantity);
                                        Console.WriteLine(lined);
                                    }
                                    Console.WriteLine("│\t\t\t\t\t       Total Price : {0,-14}│", a);
                                    Console.WriteLine(lined);
                                    Console.Write("Are you sure you want to payment? (yes/no) : ");
                                    answer1 = Console.ReadLine();
                                    do
                                    {
                                        if (answer1 == "yes")
                                        {
                                            bool resultpayment = invoicesBL.GetPayment(chooseinvoice);
                                            if (resultpayment)
                                            {
                                                Console.Clear();
                                                Console.WriteLine(lined);
                                                Console.WriteLine("│\t\t\t    POPULAR RICE SYSTEM      \t\t\t   │");
                                                Console.WriteLine("│\t\t\t┼─────────────────────────┼\t\t\t   │");
                                                Console.WriteLine("│\t\t\t          INVOICE      \t\t\t\t   │");
                                                Console.WriteLine(lined);
                                                Console.WriteLine("│ Order Id : {0, -62}│\n│ Date : {1, -66}│\n│ Table : {2, -65}│", invoice.Invoice_ID, invoice.Invoices_Date, invoice.table.Name);
                                                Console.WriteLine(lined);
                                                Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", "Id", "Name", "Price(Vnd)", "Quantity", "Amount");
                                                Console.WriteLine(lined);
                                                for (int i = 0; i < invoice.Items.Count; i++)
                                                {
                                                    Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", invoice.Items[i].ItemsID, invoice.Items[i].ItemName, invoice.Items[i].ItemPrice, invoice.Items[i].Quantity, invoice.Items[i].ItemPrice * (decimal)invoice.Items[i].Quantity);
                                                    Console.WriteLine(lined);
                                                }
                                                Console.WriteLine("│\t\t\t\t\t       Total Price : {0,-14}│", a);
                                                Console.WriteLine(lined);
                                                Console.WriteLine("│\t\t\t Thanks, Have a nice day!\t\t\t   │");
                                                Console.WriteLine(lined);
                                                Console.Write("Press any key to continue...");
                                                Console.ReadKey();
                                            }
                                            else
                                            {
                                                Console.Clear();
                                                Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                Console.WriteLine("│                                              │");
                                                Console.Write("│           ");
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.Write("Payment Not Complete!");
                                                Console.ResetColor();
                                                Console.WriteLine("              │");
                                                Console.WriteLine("│                                              │");
                                                Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                Console.Write("Press any key to continue...");
                                                Console.ReadKey();
                                            }
                                            break;
                                        }
                                        else if (answer1 == "no")
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.Write("Invalid input, re-enter : ");
                                            Console.ResetColor();
                                            answer1 = Console.ReadLine();
                                        }
                                    } while (answer1 != "no");
                                    break;
                                case 2://Cancel
                                    Console.Clear();
                                    Console.WriteLine(lined);
                                    Console.WriteLine("│\t\t\t\t   CANCEL      \t\t\t\t   │");
                                    Console.WriteLine(lined);
                                    Console.WriteLine("│ Order Id : {0, -62}│\n│ Date : {1, -66}│\n│ Table : {2, -65}│", invoice.Invoice_ID, invoice.Invoices_Date, invoice.table.Name);
                                    Console.WriteLine(lined);
                                    Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", "Id", "Name", "Price(Vnd)", "Quantity", "Amount");
                                    Console.WriteLine(lined);
                                    for (int i = 0; i < invoice.Items.Count; i++)
                                    {
                                        Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", invoice.Items[i].ItemsID, invoice.Items[i].ItemName, invoice.Items[i].ItemPrice, invoice.Items[i].Quantity, invoice.Items[i].ItemPrice * (decimal)invoice.Items[i].Quantity);
                                        Console.WriteLine(lined);
                                    }
                                    Console.Write("Are you sure you want to cancel ? (yes/no) : ");
                                    answer2 = Console.ReadLine();
                                    do
                                    {
                                        if (answer2 == "yes")
                                        {
                                            bool resultcancel = invoicesBL.GetCancelInvoice(chooseinvoice);
                                            if (resultcancel)
                                            {
                                                Console.Clear();
                                                Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                Console.WriteLine("│                                              │");
                                                Console.Write("│          ");
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.Write("Cancel Invoice Complete!");
                                                Console.ResetColor();
                                                Console.WriteLine("            │");
                                                Console.WriteLine("│                                              │");
                                                Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                Console.Write("Press any key to continue...");
                                                Console.ReadKey();
                                            }
                                            else
                                            {
                                                Console.Clear();
                                                Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                Console.WriteLine("│                                              │");
                                                Console.Write("│            ");
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.Write("Cancel Invoice Fail!");
                                                Console.ResetColor();
                                                Console.WriteLine("              │");
                                                Console.WriteLine("│                                              │");
                                                Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                Console.Write("Press any key to continue...");
                                                Console.ReadKey();
                                            }
                                            break;
                                        }
                                        else if (answer2 == "no")
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.Write("Invalid input, re-enter : ");
                                            Console.ResetColor();
                                            answer2 = Console.ReadLine();
                                        }
                                    } while (answer2 != "no");
                                    break;
                                case 3://add dishes
                                    Console.Clear();
                                    items = itemBL.GetItems();
                                    // DisplayItem(items);
                                    while (true)
                                    {
                                        DisplayItem(items);
                                        Console.Write("Input ID to add dishes or input 0 to back to menu : ");
                                        int choosedishes = GetID();
                                        if (choosedishes == 0) break;
                                        Item item = itemBL.GetById(choosedishes);
                                        if (item == null)
                                        {
                                            Console.Clear();
                                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                                            Console.WriteLine("│                                              │");
                                            Console.Write("│             ");
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.Write("Dishes Id Invalid!");
                                            Console.ResetColor();
                                            Console.WriteLine("               │");
                                            Console.WriteLine("│                                              │");
                                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                                            Console.Write("Press any key to continue...");
                                            Console.ReadKey();
                                        }
                                        else
                                        {
                                            Console.WriteLine("┼────────────────────────────────────────────────────────────────┼");
                                            Console.WriteLine("│ {0,-10} │ {1,-20} │ {2,-10} │ {3,-13} │", "Id", "Name", "Price(Vnd)", "Category");
                                            Console.WriteLine("┼────────────────────────────────────────────────────────────────┼");
                                            Console.WriteLine("│ {0,-10} │ {1,-20} │ {2,-10} │ {3,-13} │", item.ItemsID, item.ItemName, item.ItemPrice, item.CategoryInfo.CategoryName);
                                            Console.WriteLine("┼────────────────────────────────────────────────────────────────┼");
                                            Console.Write("How many (maximum 100): ");
                                            int quantitydishes = int.Parse(Console.ReadLine());
                                            while (quantitydishes < 0 || quantitydishes > 100)
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.Write("Invalid Input, re-enter : ");
                                                Console.ResetColor();
                                                quantitydishes = int.Parse(Console.ReadLine());
                                            }
                                            bool result2 = false;
                                            for (int i = 0; i < invoice.Items.Count; i++)
                                            {

                                                if (choosedishes == invoice.Items[i].ItemsID)
                                                {
                                                    result2 = true;
                                                }
                                            }
                                            if (result2)
                                            {
                                                bool move = invoicesBL.UpdateQuantityItem(choosedishes, chooseinvoice, quantitydishes);
                                                if (move)
                                                {
                                                    Console.Clear();
                                                    Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                    Console.WriteLine("│                                              │");
                                                    Console.Write("│             ");
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.Write("Added Successfully!");
                                                    Console.ResetColor();
                                                    Console.WriteLine("              │");
                                                    Console.WriteLine("│                                              │");
                                                    Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                    Console.Write("Press any key to continue...");
                                                    Console.ReadKey();
                                                }
                                                else
                                                {

                                                }
                                            }
                                            else
                                            {
                                                bool mark = invoicesBL.UpdateItemNew(chooseinvoice, choosedishes, quantitydishes);
                                                if (mark)
                                                {
                                                    Console.Clear();
                                                    Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                    Console.WriteLine("│                                              │");
                                                    Console.Write("│             ");
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.Write("Added Successfully!");
                                                    Console.ResetColor();
                                                    Console.WriteLine("              │");
                                                    Console.WriteLine("│                                              │");
                                                    Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                    Console.Write("Press any key to continue...");
                                                    Console.ReadKey();
                                                }
                                                else
                                                {

                                                }
                                            }
                                        }
                                    }
                                    break;
                                case 4://Remove dishes
                                    int removeid;
                                    do
                                    {
                                        Console.Clear();
                                        Console.Write("Input Dishes Id to remove or input 0 to back to menu : ");
                                        removeid = GetID();
                                        if (removeid == 0) break;
                                        invoice = invoicesBL.GetRemoveID(removeid, chooseinvoice);
                                        if (invoice.Items.Count == 0)
                                        {
                                            Console.Clear();
                                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                                            Console.WriteLine("│                                              │");
                                            Console.Write("│             ");
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.Write("Dishes Id Invalid!");
                                            Console.ResetColor();
                                            Console.WriteLine("               │");
                                            Console.WriteLine("│                                              │");
                                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                                            Console.Write("Press any key to continue...");
                                            Console.ReadKey();
                                        }
                                        else
                                        {
                                            Console.WriteLine(lined);
                                            Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", "Id", "Name", "Price(Vnd)", "Quantity", "Amount");
                                            Console.WriteLine(lined);
                                            for (int i = 0; i < invoice.Items.Count; i++)
                                            {
                                                Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", invoice.Items[i].ItemsID, invoice.Items[i].ItemName, invoice.Items[i].ItemPrice, invoice.Items[i].Quantity, invoice.Items[i].ItemPrice * (decimal)invoice.Items[i].Quantity);
                                                Console.WriteLine(lined);
                                            }
                                            Console.Write("Are you sure you want to remove this item ? (yes/no) : ");
                                            string yesno = Console.ReadLine();
                                            do
                                            {
                                                if (yesno == "yes")
                                                {
                                                    bool make = invoicesBL.removeitem(removeid);
                                                    if (make)
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                        Console.WriteLine("│                                              │");
                                                        Console.Write("│              ");
                                                        Console.ForegroundColor = ConsoleColor.Green;
                                                        Console.Write("Remove Complete!");
                                                        Console.ResetColor();
                                                        Console.WriteLine("                │");
                                                        Console.WriteLine("│                                              │");
                                                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                        Console.Write("Press any key to continue...");
                                                        Console.ReadKey();
                                                    }
                                                    else
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                        Console.WriteLine("│                                              │");
                                                        Console.Write("│                ");
                                                        Console.ForegroundColor = ConsoleColor.Red;
                                                        Console.Write("Remove Fail!");
                                                        Console.ResetColor();
                                                        Console.WriteLine("                  │");
                                                        Console.WriteLine("│                                              │");
                                                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                        Console.Write("Press any key to continue...");
                                                        Console.ReadKey();
                                                    }
                                                    break;
                                                }
                                                else if (yesno == "no")
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.Write("Invalid input, re-enter : ");
                                                    Console.ResetColor();
                                                    yesno = Console.ReadLine();
                                                }
                                            } while (yesno != "no");
                                        }
                                    } while (removeid != 0);
                                    break;
                                case 5:
                                    break;
                            }
                        }
                    } while (chooseinvoice != 0);
                    break;
                case 2:
                    Console.Clear();
                    Console.Write("Input ID of invoices to view detail or input 0 to back to menu: ");
                    chooseidinvoice = GetID();
                    do
                    {
                        if (chooseidinvoice == 0) break;
                        Invoice invoice2 = invoicesBL.GetInvoicesId(chooseidinvoice);
                        if (invoice2 == null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Nothing to show!, re-enter : ");
                            Console.ResetColor();
                            chooseidinvoice = GetID();
                        }
                        else
                        {
                            decimal a = 0;
                            string lined = "┼──────────────────────────────────────────────────────────────────────────┼";
                            Console.Clear();
                            Console.WriteLine(lined);
                            Console.WriteLine("│\t\t\t         ORDER {0}           \t\t\t   │", chooseidinvoice);
                            Console.WriteLine(lined);
                            Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", "Id", "Name", "Price(Vnd)", "Quantity", "Amount");
                            Console.WriteLine(lined);
                            for (int i = 0; i < invoice2.Items.Count; i++)
                            {
                                Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", invoice2.Items[i].ItemsID, invoice2.Items[i].ItemName, invoice2.Items[i].ItemPrice, invoice2.Items[i].Quantity, invoice2.Items[i].ItemPrice * (decimal)invoice2.Items[i].Quantity);
                                Console.WriteLine(lined);
                                a += invoice2.Items[i].ItemPrice * (decimal)invoice2.Items[i].Quantity;
                            }
                            Console.WriteLine("│\t\t\t\t\t       Total Price : {0,-14}│", a);
                            Console.WriteLine(lined);
                            Console.WriteLine("│\t\t\t\t\t\t\t\t\t   │");
                            Console.WriteLine("│\t\t\t*****       OPTIONS       *****\t\t\t   │");
                            Console.WriteLine("│\t\t\t\t\t\t\t\t\t   │");
                            Console.WriteLine(lined);
                            Console.WriteLine("│  1  │ Payment\t\t\t\t\t\t\t\t   │");
                            Console.WriteLine(lined);
                            Console.WriteLine("│  2  │ Cancel Invoice\t\t\t\t\t\t\t   │");
                            Console.WriteLine(lined);
                            Console.WriteLine("│  3  │ Add dishes\t\t\t\t\t\t\t   │");
                            Console.WriteLine(lined);
                            Console.WriteLine("│  4  │ Remove dishes\t\t\t\t\t\t\t   │");
                            Console.WriteLine(lined);
                            Console.WriteLine("│  5  │ Back to menu\t\t\t\t\t\t\t   │");
                            Console.WriteLine(lined);
                            Console.WriteLine();
                            Console.Write("  ───> Your Choice : ");
                            choosefunctionid = GetID();
                            while (choosefunctionid < 1 || choosefunctionid > 5)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("Invalid input, re-enter : ");
                                Console.ResetColor();
                                choosefunctionid = GetID();
                            }
                            switch (choosefunctionid)
                            {
                                case 1://payment
                                    Console.Clear();
                                    Console.WriteLine(lined);
                                    Console.WriteLine("│\t\t\t          PAYMENT      \t\t\t\t   │");
                                    Console.WriteLine(lined);
                                    Console.WriteLine("│ Order Id : {0, -62}│\n│ Date : {1, -66}│\n│ Table : {2, -65}│", invoice2.Invoice_ID, invoice2.Invoices_Date, invoice2.table.Name);
                                    Console.WriteLine(lined);
                                    Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", "Id", "Name", "Price(Vnd)", "Quantity", "Amount");
                                    Console.WriteLine(lined);
                                    for (int i = 0; i < invoice2.Items.Count; i++)
                                    {
                                        Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", invoice2.Items[i].ItemsID, invoice2.Items[i].ItemName, invoice2.Items[i].ItemPrice, invoice2.Items[i].Quantity, invoice2.Items[i].ItemPrice * (decimal)invoice2.Items[i].Quantity);
                                        Console.WriteLine(lined);
                                    }
                                    Console.WriteLine("│\t\t\t\t\t       Total Price : {0,-14}│", a);
                                    Console.WriteLine(lined);
                                    Console.Write("Are you sure you want to payment ? (yes/no) : ");
                                    answer3 = Console.ReadLine();
                                    do
                                    {
                                        if (answer3 == "yes")
                                        {
                                            bool result = invoicesBL.GetPayment(chooseidinvoice);
                                            if (result)
                                            {
                                                Console.Clear();
                                                Console.WriteLine(lined);
                                                Console.WriteLine("│\t\t\t    POPULAR RICE SYSTEM      \t\t\t   │");
                                                Console.WriteLine("│\t\t\t┼─────────────────────────┼\t\t\t   │");
                                                Console.WriteLine("│\t\t\t          INVOICE      \t\t\t\t   │");
                                                Console.WriteLine(lined);
                                                Console.WriteLine("│ Order Id : {0, -62}│\n│ Date : {1, -66}│\n│ Table : {2, -65}│", invoice2.Invoice_ID, invoice2.Invoices_Date, invoice2.table.Name);
                                                Console.WriteLine(lined);
                                                Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", "Id", "Name", "Price(Vnd)", "Quantity", "Amount");
                                                Console.WriteLine(lined);
                                                for (int i = 0; i < invoice2.Items.Count; i++)
                                                {
                                                    Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", invoice2.Items[i].ItemsID, invoice2.Items[i].ItemName, invoice2.Items[i].ItemPrice, invoice2.Items[i].Quantity, invoice2.Items[i].ItemPrice * (decimal)invoice2.Items[i].Quantity);
                                                    Console.WriteLine(lined);
                                                }
                                                Console.WriteLine("│\t\t\t\t\t       Total Price : {0,-14}│", a);
                                                Console.WriteLine(lined);
                                                Console.WriteLine("│\t\t\t Thanks, Have a nice day!\t\t\t   │");
                                                Console.WriteLine(lined);
                                                Console.Write("Press any key to continue...");
                                                Console.ReadKey();
                                            }
                                            else
                                            {
                                                Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                Console.WriteLine("│                                              │");
                                                Console.Write("│            ");
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.Write("Payment Not Complete!");
                                                Console.ResetColor();
                                                Console.WriteLine("             │");
                                                Console.WriteLine("│                                              │");
                                                Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                Console.Write("Press any key to continue...");
                                                Console.ReadKey();
                                            }
                                            break;
                                        }
                                        else if (answer3 == "no")
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.Write("Invalid input, re-enter : ");
                                            Console.ResetColor();
                                            answer3 = Console.ReadLine();
                                        }
                                    } while (answer3 != "no");
                                    break;
                                case 2://Cancel
                                    Console.Clear();
                                    Console.WriteLine(lined);
                                    Console.WriteLine("│\t\t\t\t   CANCEL      \t\t\t\t   │");
                                    Console.WriteLine(lined);
                                    Console.WriteLine("│ Order Id : {0, -62}│\n│ Date : {1, -66}│\n│ Table : {2, -65}│", invoice2.Invoice_ID, invoice2.Invoices_Date, invoice2.table.Name);
                                    Console.WriteLine(lined);
                                    Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", "Id", "Name", "Price(Vnd)", "Quantity", "Amount");
                                    Console.WriteLine(lined);
                                    for (int i = 0; i < invoice2.Items.Count; i++)
                                    {
                                        Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", invoice2.Items[i].ItemsID, invoice2.Items[i].ItemName, invoice2.Items[i].ItemPrice, invoice2.Items[i].Quantity, invoice2.Items[i].ItemPrice * (decimal)invoice2.Items[i].Quantity);
                                        Console.WriteLine(lined);
                                    }
                                    Console.Write("Are you sure you want to cancel ? (yes/no) : ");
                                    answer4 = Console.ReadLine();
                                    do
                                    {
                                        if (answer4 == "yes")
                                        {
                                            bool result = invoicesBL.GetCancelInvoice(chooseidinvoice);
                                            if (result)
                                            {
                                                Console.Clear();
                                                Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                Console.WriteLine("│                                              │");
                                                Console.Write("│          ");
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.Write("Cancel Invoice Complete!");
                                                Console.ResetColor();
                                                Console.WriteLine("            │");
                                                Console.WriteLine("│                                              │");
                                                Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                Console.Write("Press any key to continue...");
                                                Console.ReadKey();
                                            }
                                            else
                                            {
                                                Console.Clear();
                                                Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                Console.WriteLine("│                                              │");
                                                Console.Write("│            ");
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.Write("Cancel Invoice Fail!");
                                                Console.ResetColor();
                                                Console.WriteLine("              │");
                                                Console.WriteLine("│                                              │");
                                                Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                Console.Write("Press any key to continue...");
                                                Console.ReadKey();
                                            }
                                            break;
                                        }
                                        else if (answer4 == "no")
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.Write("Invalid input, re-enter : ");
                                            Console.ResetColor();
                                            answer4 = Console.ReadLine();
                                        }
                                    } while (answer4 != "no");
                                    break;
                                case 3://Add dishes
                                    Console.Clear();
                                    items = itemBL.GetItems();
                                    // DisplayItem(items);
                                    while (true)
                                    {
                                        DisplayItem(items);
                                        Console.Write("Input ID to add dishes or input 0 to back to menu : ");
                                        int choosedishes = GetID();
                                        if (choosedishes == 0) break;
                                        Item item = itemBL.GetById(choosedishes);
                                        if (item == null)
                                        {
                                            Console.Clear();
                                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                                            Console.WriteLine("│                                              │");
                                            Console.Write("│             ");
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.Write("Dishes Id Invalid!");
                                            Console.ResetColor();
                                            Console.WriteLine("               │");
                                            Console.WriteLine("│                                              │");
                                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                                            Console.Write("Press any key to continue...");
                                            Console.ReadKey();
                                        }
                                        else
                                        {
                                            Console.WriteLine("┼────────────────────────────────────────────────────────────────┼");
                                            Console.WriteLine("│ {0,-10} │ {1,-20} │ {2,-10} │ {3,-13} │", "Id", "Name", "Price(Vnd)", "Category");
                                            Console.WriteLine("┼────────────────────────────────────────────────────────────────┼");
                                            Console.WriteLine("│ {0,-10} │ {1,-20} │ {2,-10} │ {3,-13} │", item.ItemsID, item.ItemName, item.ItemPrice, item.CategoryInfo.CategoryName);
                                            Console.WriteLine("┼────────────────────────────────────────────────────────────────┼");
                                            Console.Write("How many (maximum 100): ");
                                            int quantitydishes = int.Parse(Console.ReadLine());
                                            while (quantitydishes < 0 || quantitydishes > 100)
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.Write("Invalid input, re-enter : ");
                                                Console.ResetColor();
                                                quantitydishes = int.Parse(Console.ReadLine());
                                            }
                                            bool result2 = false;
                                            for (int i = 0; i < invoice2.Items.Count; i++)
                                            {

                                                if (choosedishes == invoice2.Items[i].ItemsID)
                                                {
                                                    result2 = true;
                                                }
                                            }
                                            if (result2)
                                            {
                                                bool move = invoicesBL.UpdateQuantityItem(choosedishes, chooseidinvoice, quantitydishes);
                                                if (move)
                                                {
                                                    Console.Clear();
                                                    Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                    Console.WriteLine("│                                              │");
                                                    Console.Write("│             ");
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.Write("Added Successfully!");
                                                    Console.ResetColor();
                                                    Console.WriteLine("              │");
                                                    Console.WriteLine("│                                              │");
                                                    Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                    Console.Write("Press any key to continue...");
                                                    Console.ReadKey();
                                                }
                                                else
                                                {

                                                }
                                            }
                                            else
                                            {
                                                bool mark = invoicesBL.UpdateItemNew(chooseidinvoice, choosedishes, quantitydishes);
                                                if (mark)
                                                {
                                                    Console.Clear();
                                                    Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                    Console.WriteLine("│                                              │");
                                                    Console.Write("│             ");
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.Write("Added Successfully!");
                                                    Console.ResetColor();
                                                    Console.WriteLine("              │");
                                                    Console.WriteLine("│                                              │");
                                                    Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                    Console.Write("Press any key to continue...");
                                                    Console.ReadKey();
                                                }
                                                else
                                                {

                                                }
                                            }
                                        }
                                    }
                                    break;
                                case 4://Remove dishes
                                    int removeid;
                                    do
                                    {
                                        Console.Clear();
                                        Console.Write("Input Dishes Id to remove or input 0 to back to menu : ");
                                        removeid = GetID();
                                        if (removeid == 0) break;
                                        invoice2 = invoicesBL.GetRemoveID(removeid, chooseidinvoice);
                                        if (invoice2.Items.Count == 0)
                                        {
                                            Console.Clear();
                                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                                            Console.WriteLine("│                                              │");
                                            Console.Write("│             ");
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.Write("Dishes Id Invalid!");
                                            Console.ResetColor();
                                            Console.WriteLine("               │");
                                            Console.WriteLine("│                                              │");
                                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                                            Console.Write("Press any key to continue...");
                                            Console.ReadKey();
                                        }
                                        else
                                        {
                                            Console.WriteLine(lined);
                                            Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", "Id", "Name", "Price(Vnd)", "Quantity", "Amount");
                                            Console.WriteLine(lined);
                                            for (int i = 0; i < invoice2.Items.Count; i++)
                                            {
                                                Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", invoice2.Items[i].ItemsID, invoice2.Items[i].ItemName, invoice2.Items[i].ItemPrice, invoice2.Items[i].Quantity, invoice2.Items[i].ItemPrice * (decimal)invoice2.Items[i].Quantity);
                                                Console.WriteLine(lined);
                                            }
                                            Console.Write("Are you sure you want to remove this item ? (yes/no) : ");
                                            string yesno = Console.ReadLine();
                                            do
                                            {
                                                if (yesno == "yes")
                                                {
                                                    bool make = invoicesBL.removeitem(removeid);
                                                    if (make)
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                        Console.WriteLine("│                                              │");
                                                        Console.Write("│             ");
                                                        Console.ForegroundColor = ConsoleColor.Green;
                                                        Console.Write("Remove Complete!");
                                                        Console.ResetColor();
                                                        Console.WriteLine("                 │");
                                                        Console.WriteLine("│                                              │");
                                                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                        Console.Write("Press any key to continue...");
                                                        Console.ReadKey();
                                                    }
                                                    else
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                        Console.WriteLine("│                                              │");
                                                        Console.Write("│               ");
                                                        Console.ForegroundColor = ConsoleColor.Red;
                                                        Console.Write("Remove Fail!");
                                                        Console.ResetColor();
                                                        Console.WriteLine("                   │");
                                                        Console.WriteLine("│                                              │");
                                                        Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                        Console.Write("Press any key to continue...");
                                                        Console.ReadKey();
                                                    }
                                                    break;
                                                }
                                                else if (yesno == "no")
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.Write("Invalid input, re-enter : ");
                                                    Console.ResetColor();
                                                    yesno = Console.ReadLine();
                                                }
                                            } while (yesno != "no");
                                        }
                                    } while (removeid != 0);
                                    break;
                                case 5:
                                    break;
                            }
                        }
                    } while (chooseidinvoice != 0);
                    break;
                case 3:
                    break;
            }

        }
        static void DisplayTables(List<TableFood> tables)
        {
            Console.Clear();
            string status;
            string line = "┼────────────────────────────────────────────────┼";
            string lined = "┼──────────────────────────┼";
            string blank = "│                                                │";
            string title = "TABLES";
            int position = line.Length / 2 - title.Length / 2;
            Console.WriteLine(line);
            Console.WriteLine(blank);
            Console.WriteLine("│\t\tPOPULAR RICE SYSTEM\t\t │");
            Console.WriteLine("│{0," + position + "}\b{1}\t\t\t │", "", title);
            Console.WriteLine(blank);
            Console.WriteLine(line);
            Console.WriteLine("│ {0,-10} │ {1,-15} │ {2,-15} │", "ID", "Name", "Status");
            Console.WriteLine(line);
            for (int i = 0; i < tables.Count; i++)
            {
                status = tables[i].Status == TableFood.READY_STATUS ? "Ready" : "In Use";
                Console.WriteLine("│ {0,-10} │ {1,-15} │ {2,-15} │", tables[i].TableId, tables[i].Name, status);
                Console.WriteLine(line);
            }
            Console.WriteLine("\t\t<───     ───>");
            Console.WriteLine(lined);
            Console.WriteLine("│Press '<───' to turn left │\n│Press '───>' to turn right│\n│Press 'esc' to escape     │");
            Console.WriteLine(lined);
        }
        static void DisplayInvoice(List<Invoice> invoices)
        {
            Console.Clear();
            string status;
            string line = "┼───────────────────────────────────────────────────────────────────┼";
            string title = "LIST ORDER";
            int position = line.Length / 2 - title.Length / 2;
            Console.WriteLine(line);
            Console.WriteLine("│{0," + position + "}\b{1}\t\t\t\t    │", "", title);
            Console.WriteLine(line);
            Console.WriteLine("│ {0,-10} │ {1,-22} │ {2,-15} │ {3,-9} │", "ID", "Date", "Table", "Status");
            Console.WriteLine(line);
            for (int i = 0; i < invoices.Count; i++)
            {
                status = invoices[i].Invoices_Status == InvoiceStatus.NEW_INVOICE ? "Waiting" : "Paid";
                Console.WriteLine("│ {0,-10} │ {1,-21} │ {2,-15} │ {3,-9} │", invoices[i].Invoice_ID, invoices[i].Invoices_Date, invoices[i].table.Name, status);
                Console.WriteLine(line);
            }
        }
        static void DisplayHistory(List<Invoice> invoices)
        {
            Console.Clear();
            string status;
            string line = "┼──────────────────────────────────────────────────────────────────┼";
            string title = "History";
            int position = line.Length / 2 - title.Length / 2;
            Console.WriteLine(line);
            Console.WriteLine("│\t\t\t POPULAR RICE SYSTEM\t\t\t   │");
            Console.WriteLine("│\t\t    ┼──────────────────────────┼\t\t   │");
            Console.WriteLine("│{0," + position + "}\b{1}\t\t\t\t   │", "", title);
            Console.WriteLine(line);
            Console.WriteLine("│ {0,-10} │ {1,-21} │ {2,-15} │ {3,-9} │", "ID", "Date", "Table", "Status");
            Console.WriteLine(line);
            for (int i = 0; i < invoices.Count; i++)
            {
                status = invoices[i].Invoices_Status == InvoiceStatus.INVOICE_IN_PROGRESS ? "Paid" : "Cancel";
                Console.WriteLine("│ {0,-10} │ {1,-21} │ {2,-15} │ {3,-9} │", invoices[i].Invoice_ID, invoices[i].Invoices_Date, invoices[i].table.Name, status);
                Console.WriteLine(line);
            }
        }
        static void DisplayCategory(List<Category> category)
        {
            Console.WriteLine("┼───────────────────────────────┼");
            Console.WriteLine("│ {0,-10} │ {1,-15} │", "Category ID", "Category name");
            Console.WriteLine("┼───────────────────────────────┼");
            for (int i = 0; i < category.Count; i++)
            {
                Console.WriteLine("│ {0,-11} │ {1,-15} │", category[i].CategoryID, category[i].CategoryName);
                Console.WriteLine("┼───────────────────────────────┼");
            }
        }
        static int Menu(string title, string role, string[] menuItems)
        {
            int choose = 0;
            string line = "┼──────────────────────────────────────────────────────────────┼";
            string blank = "│\t\t\t\t\t\t\t       │";
            int position = line.Length / 2 - title.Length / 2;
            Console.Clear();
            Console.WriteLine(line);
            Console.WriteLine(blank);
            Console.WriteLine("│{0," + position + "}\b{1}\t\t       │", "", title);
            Console.WriteLine("│    {0," + position + "}\b{1}\t\t\t       │", "", role);
            Console.WriteLine(blank);
            Console.WriteLine(line);
            for (int i = 0; i < menuItems.Length; i++)
            {
                Console.WriteLine("│  {0}  │ {1}", i + 1, menuItems[i]);
                Console.WriteLine(line);
            }
            Console.Write("\n  ───> Your choice: ");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out choose) && choose >= 1 && choose <= menuItems.Length) return choose;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Entered incorrectly!");
                Console.ResetColor();
                Console.Write(" → re-enter: ");
            }

        }
        static void DisplayItem(List<Item> items)
        {
            Console.Clear();
            string line = "┼───────────────────────────────────────────────────────────────────────┼";
            string lined = "┼──────────────────────────┼";
            string title = "MENU";
            int position = line.Length / 2 - title.Length / 2;
            Console.WriteLine(line);
            Console.WriteLine("│{0," + position + "}\b{1}\t\t\t\t\t│", "", title);
            Console.WriteLine(line);
            Console.WriteLine("│ {0,-10} │ {1,-20} │ {2,-15} │ {3,-15} │", "ID", "Name", "Price(Vnd)", "Category");
            Console.WriteLine(line);
            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine("│ {0,-10} │ {1,-20} │ {2,-15} │ {3, -15} │", items[i].ItemsID, items[i].ItemName, items[i].ItemPrice, items[i].CategoryInfo.CategoryName);
                Console.WriteLine(line);
            }
            Console.WriteLine("\t\t<───     ───>");
            Console.WriteLine(lined);
            Console.WriteLine("│Press '<───' to turn left │\n│Press '───>' to turn right│\n│Press 'esc' to escape     │");
            Console.WriteLine(lined);
        }
        public static void ShowPage(List<TableFood> tableFoods)
        {
            int count = tableFoods.Count;
            int max_page = (count % 10 == 0) ? (count % 10) : (count % 10 + 1); // lấy max page nếu count % 10 --> show / count thừa sẽ + dần
            int[] index = new int[max_page];
            string[] keywords = new string[] { "Escape", "LeftArrow", "RightArrow" }; // lấy keyword từ hàm GetChoice
            index[0] = 0;
            int current_page = 1;
            string choice = string.Empty;
            List<TableFood> foods = new List<TableFood>();
            for (int i = 1; i < max_page; i++)
            {
                index[i] = 10 * i;
            }
            do
            {
                foods = new List<TableFood>();
                for (int i = 0; i < 10; i++)
                {
                    if (current_page == max_page)
                    {
                        if (i == count - 10 * (max_page - 1))
                        {
                            break;
                        }
                    }
                    foods.Add(tableFoods[index[current_page - 1] + i]);
                }
                DisplayTables(foods);
                choice = GetChoice(keywords);
                switch (choice)
                {
                    case "LeftArrow":
                        // current_page--;
                        if (current_page != 1)
                        {
                            current_page--;
                        }
                        break;
                    case "RightArrow":
                        // current_page++;
                        if (current_page != max_page)
                        {
                            current_page++;
                        }
                        break;
                    case "Escape":
                        break;
                    default:
                        int id;
                        //convert + check choice
                        try
                        {
                            id = int.Parse(choice);
                        }
                        catch
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nInvalid input!");
                            Console.ResetColor();
                            Console.Write("Press any key to continue...");
                            Console.ReadKey();
                            break;
                        }
                        // -> details
                        break;

                }
            } while (choice != "Escape");
        }
        public static void ShowPageItem(List<Item> items)
        {
            int count = items.Count;
            int max_page = (count % 10 == 0) ? (count % 10) : (count % 10 + 1); // lấy max page nếu page % 10 hoặc page lẻ (vd : 11, 12) sẽ cộng thêm
            int[] index = new int[max_page];
            string[] keywords = new string[] { "Escape", "LeftArrow", "RightArrow" }; // lấy keyword từ hàm GetChoice
            index[0] = 0;
            int current_page = 1;
            string choice = string.Empty;
            List<Item> items1 = new List<Item>();
            for (int i = 1; i < max_page; i++)
            {
                index[i] = 10 * i;
            }
            do
            {
                items1 = new List<Item>();
                for (int i = 0; i < 10; i++)
                {
                    if (current_page == max_page)
                    {
                        if (i == count - 10 * (max_page - 1))
                        {
                            break;
                        }
                    }
                    items1.Add(items[index[current_page - 1] + i]);
                }
                DisplayItem(items1);
                choice = GetChoice(keywords);
                switch (choice)
                {
                    case "LeftArrow":
                        // current_page--;
                        if (current_page != 1)
                        {
                            current_page--;
                        }
                        break;
                    case "RightArrow":
                        // current_page++;
                        if (current_page != max_page)
                        {
                            current_page++;
                        }
                        break;
                    case "Escape":
                        break;
                    default:
                        int id;
                        //convert + check choice
                        try
                        {
                            id = int.Parse(choice);
                        }
                        catch
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nInvalid input!");
                            Console.ResetColor();
                            Console.Write("Press any key to continue...");
                            Console.ReadKey();
                            break;
                        }
                        // -> details
                        break;

                }
            } while (choice != "Escape");
        }
        public static string GetChoice(string[] keyword)
        { // hiển thị yêu cầu lựa chọn, trả về key vừa nhập nếu nó thuộc một trong các keyword truyền vào
            var result = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;
                if (key == ConsoleKey.Enter)
                {
                    break;
                }
                if (key == ConsoleKey.Backspace && result.Length > 0)
                {
                    Console.Write("\b \b");
                    result = result[0..^1];
                }
                else if (keyword.Contains(key.ToString()))
                {
                    return key.ToString();
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    result += keyInfo.KeyChar;
                    Console.Write(keyInfo.KeyChar);
                }
            } while (key != ConsoleKey.Enter);
            return result;
        }
        static int GetID()
        {
            int id;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out id) && id >= 0) return id;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Invalid Input, re-enter : ");
                Console.ResetColor();
            }
        }
        static int Getidover0()
        {
            int id;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out id) && id > 0) return id;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Invalid Input, re-enter : ");
                Console.ResetColor();
            }
        }

        static string GetPassword()
        {
            var pass = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);
            return pass;
        }
    }
}
