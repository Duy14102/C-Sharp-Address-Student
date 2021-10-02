using System;
using System.Collections.Generic;
using Persistance;
using BL;

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
            staff = new Staff() { Role = 1 };
            staff.Role = 1;
            staff = new Staff() { Role = 2 };
            staff.Role = 2;
            switch (staff.Role)
            {
                case Staff.STAFF_ROLE:
                    string[] menuItemsStaff = { "CREATE ORDER\t\t\t   │", "TABLE MANAGEMENT\t\t\t   │", "MENU MANAGEMENT\t\t\t   │", "INVOICE\t\t\t\t   │", "HISTORY\t\t\t\t   │", "EXIT\t\t\t\t   │" };
                    do
                    {
                        choice = Menu("POPULAR RICE SYSTEM", menuItemsStaff);
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
                    } while (choice != menuItemsStaff.Length);
                    break;
                case Staff.ADMIN_ROLE:
                    string[] menuItemsAdmin = { "CREATE ORDER\t\t\t   │", "TABLE MANAGEMENT\t\t\t   │", "MENU MANAGEMENT\t\t\t   │", "INVOICE\t\t\t\t   │", "HISTORY\t\t\t\t   │", "EXIT\t\t\t\t   │" };
                    List<TableFood> tablesAdmin = new List<TableFood>();
                    do
                    {
                        choice = Menu("POPULAR RICE SYSTEM", menuItemsAdmin);
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
                Console.Clear();
                invoices = invoicesBL.GetHistory();
                if (invoices == null)
                {
                    Console.Clear();
                    Console.WriteLine("┼─────────────────────────────────┼");
                    Console.WriteLine("│                                 │");
                    Console.WriteLine("│        Nothing to show!         │");
                    Console.WriteLine("│                                 │");
                    Console.WriteLine("┼─────────────────────────────────┼");
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
                    string lined = "┼──────────────────────────────────────────────────────────────────────────┼";
                    Console.Clear();
                    Console.WriteLine(lined);
                    Console.WriteLine("│\t\t\t\tInvoice {0} History\t\t\t   │", choosehistory);
                    Console.WriteLine(lined);
                    Console.WriteLine("│ {0,-5} │ {1,-23} │ {2,-10} │ {3,-10} │ {4,-12} │", "Id", "Name", "Price", "Amount", "TotalPrice");
                    Console.WriteLine(lined);
                    for (int i = 0; i < invoice.Items.Count; i++)
                    {
                        Console.WriteLine("│ {0,-5} │ {1,-23} │ {2,-10} │ {3,-10} │ {4,-12} │", invoice.Items[i].ItemsID, invoice.Items[i].ItemName, invoice.Items[i].ItemPrice, invoice.Items[i].Quantity, invoice.Items[i].ItemPrice * (decimal)invoice.Items[i].Quantity);
                        Console.WriteLine(lined);
                    }
                    Console.WriteLine("Press any key to back to menu...");
                    Console.ReadKey();
                }
            } while (choosehistory != 0);
        }
        static void MenuManagement(Staff staff)
        {
            int chooseit;
            Console.Clear();
            string line = "┼──────────────────────────────────────────────────────────┼";
            string title = "MENU MANAGEMENT";
            int position = line.Length / 2 - title.Length / 2;
            Console.WriteLine(line);
            Console.WriteLine("│\t\t\t\t\t\t\t   │");
            Console.WriteLine("│{0," + position + "}\b{1}\t\t\t   │", "", title);
            Console.WriteLine("│\t\t\t\t\t\t\t   │");
            Console.WriteLine(line);
            Console.WriteLine("│  1. Add Item\t\t\t\t\t\t   │\n│  2. Remove Item\t\t\t\t\t   │\n│  3. Get item by id\t\t\t\t\t   │\n│  4. Get item by name\t\t\t\t\t   │\n│  5. Get all item\t\t\t\t\t   │\n│  6. Exit to main menu\t\t\t\t\t   │");
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
                    Console.Write("Are you sure you want to remove item ? (yes/no) : ");
                    string takeit = Console.ReadLine();
                    do
                    {
                        if (takeit == "yes")
                        {
                            Console.Clear();
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
                        string linet = "┼───────────────────────────────────────────────────────────────────────┼";
                        string titlet = "MENU";
                        int positiont = linet.Length / 2 - titlet.Length / 2;
                        Console.WriteLine(linet);
                        Console.WriteLine("│{0," + positiont + "}\b{1}\t\t\t\t\t│", "", titlet);
                        Console.WriteLine(linet);
                        Console.WriteLine("│ {0,-10} │ {1,-20} │ {2,-15} │ {3,-15} │", "ID", "Name", "Price", "Category");
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
                    if (items == null)
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
                        DisplayItem(items);
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
            string line = "┼──────────────────────────────────────────────────────────┼";
            string title = "TABLE MANAGEMENT";
            int position = line.Length / 2 - title.Length / 2;
            Console.WriteLine(line);
            Console.WriteLine("│\t\t\t\t\t\t\t   │");
            Console.WriteLine("│{0," + position + "}\b{1}\t\t\t   │", "", title);
            Console.WriteLine("│\t\t\t\t\t\t\t   │");
            Console.WriteLine(line);
            Console.WriteLine("│  1. Add Table\t\t\t\t\t\t   │\n│  2. Remove Table\t\t\t\t\t   │\n│  3. Get table by id\t\t\t\t\t   │\n│  4. Get table by name\t\t\t\t\t   │\n│  5. Get all table\t\t\t\t\t   │\n│  6. Exit to main menu\t\t\t\t\t   │");
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
                    Console.Write("Are you sure you want to create new table ? (yes/no) : ");
                    string tookit = Console.ReadLine();
                    do
                    {
                        if (tookit == "yes")
                        {
                            Console.Clear();
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
                    Console.Write("Are you sure you want to remove table ? (yes/no) : ");
                    string takeit = Console.ReadLine();
                    do
                    {
                        if (takeit == "yes")
                        {
                            Console.Clear();
                            Console.Write("Input Id of table to remove : ");
                            int getit = GetID();
                            TableFood tableFood = tableBL.GetById(getit);
                            if (tableFood == null)
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
                        string linek = "┼────────────────────────────────────────────────┼";
                        string titlek = "TABLES";
                        int positionk = linek.Length / 2 - titlek.Length / 2;
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
                        DisplayTables(tableFoods);
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
            do
            {
                Console.Clear();
                tablesStaff = tableBL.GetAllTableFood();
                if (tablesStaff == null)
                {
                    Console.WriteLine("┼──────────────────────────────────────────────┼");
                    Console.WriteLine("│                                              │");
                    Console.WriteLine("│               Nothing to show!               │");
                    Console.WriteLine("│                                              │");
                    Console.WriteLine("┼──────────────────────────────────────────────┼");
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                    break;
                }
                else
                {
                    DisplayTables(tablesStaff);
                }
                Console.Write("Select a table to see details or input 0 to back to menu: ");
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
                        DisplayItem(items);
                        while (true)
                        {
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
            string title = "INVOICE SYSTEM";
            int position = line.Length / 2 - title.Length / 2;
            Console.WriteLine(line);
            Console.WriteLine("│\t\t\t\t\t\t\t   │");
            Console.WriteLine("│{0," + position + "}\b{1}\t\t\t   │", "", title);
            Console.WriteLine("│\t\t\t\t\t\t\t   │");
            Console.WriteLine(line);
            Console.WriteLine("│  1. Show List Order\t\t\t\t\t   │\n│  2. Show Order By ID\t\t\t\t\t   │\n│  3. Back to menu\t\t\t\t\t   │");
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
                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                            Console.WriteLine("│                                              │");
                            Console.WriteLine("│               Nothing to show!               │");
                            Console.WriteLine("│                                              │");
                            Console.WriteLine("┼──────────────────────────────────────────────┼");
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
                            Console.WriteLine("│\t\t\t          Order {0}            \t\t\t   │", chooseinvoice);
                            Console.WriteLine(lined);
                            Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", "Id", "Name", "Price", "Quantity", "Amount");
                            Console.WriteLine(lined);
                            for (int i = 0; i < invoice.Items.Count; i++)
                            {
                                Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", invoice.Items[i].ItemsID, invoice.Items[i].ItemName, invoice.Items[i].ItemPrice, invoice.Items[i].Quantity, invoice.Items[i].ItemPrice * (decimal)invoice.Items[i].Quantity);
                                Console.WriteLine(lined);
                                a += invoice.Items[i].ItemPrice * invoice.Items[i].Quantity;
                            }
                            Console.WriteLine("│\t\t\t\t\t       Total Price : {0,-14}│", a);
                            Console.WriteLine(lined);
                            Console.WriteLine("│ 1. Payment\t\t\t\t\t\t\t\t   │\n│ 2. Cancel Invoice\t\t\t\t\t\t\t   │\n│ 3. Add dishes\t\t\t\t\t\t\t\t   │\n│ 4. Remove dishes\t\t\t\t\t\t\t   │\n│ 5. Back to menu\t\t\t\t\t\t\t   │");
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
                                    Console.WriteLine("│\t\t\t      POPULAR RICE SYSTEM      \t\t\t   │");
                                    Console.WriteLine(lined);
                                    Console.WriteLine("│ Order Id : {0, -62}│\n│ Date : {1, -66}│\n│ Table : {2, -65}│", invoice.Invoice_ID, invoice.Invoices_Date, invoice.table.Name);
                                    Console.WriteLine(lined);
                                    Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", "Id", "Name", "Price", "Quantity", "Amount");
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
                                                Console.WriteLine("┼──────────────────────────────────────────────┼");
                                                Console.WriteLine("│                                              │");
                                                Console.Write("│             ");
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.Write("Payment Complete!");
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
                                    Console.WriteLine("│\t\t\t      POPULAR RICE SYSTEM      \t\t\t   │");
                                    Console.WriteLine(lined);
                                    Console.WriteLine("│ Order Id : {0, -62}│\n│ Date : {1, -66}│\n│ Table : {2, -65}│", invoice.Invoice_ID, invoice.Invoices_Date, invoice.table.Name);
                                    Console.WriteLine(lined);
                                    Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", "Id", "Name", "Price", "Quantity", "Amount");
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
                                    DisplayItem(items);
                                    while (true)
                                    {
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

                                                }
                                                else
                                                {

                                                }
                                            }
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.Write("Added Successfully!");
                                            Console.ResetColor();
                                        }
                                    }
                                    break;
                                case 4://Remove dishes
                                    Console.Clear();
                                    Console.Write("Input Dishes Id to remove or input 0 to back to menu : ");
                                    int removeid = GetID();
                                    if (removeid == 0) break;
                                    Item item2 = itemBL.GetById(removeid);
                                    if (item2 == null)
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
                                    }
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
                        Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", "Id", "Name", "Price", "Quantity", "Amount");
                        Console.WriteLine(lined);
                        for (int i = 0; i < invoice2.Items.Count; i++)
                        {
                            Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", invoice2.Items[i].ItemsID, invoice2.Items[i].ItemName, invoice2.Items[i].ItemPrice, invoice2.Items[i].Quantity, invoice2.Items[i].ItemPrice * (decimal)invoice2.Items[i].Quantity);
                            Console.WriteLine(lined);
                            a += invoice2.Items[i].ItemPrice * (decimal)invoice2.Items[i].Quantity;
                        }
                        Console.WriteLine("│\t\t\t\t\t       Total Price : {0,-14}│", a);
                        Console.WriteLine(lined);
                        Console.WriteLine("│ 1. Payment\t\t\t\t\t\t\t\t   │\n│ 2. Cancel Invoice\t\t\t\t\t\t\t   │\n│ 3. Add dishes\t\t\t\t\t\t\t\t   │\n│ 4. Remove dishes\t\t\t\t\t\t\t   │\n│ 5. Back to menu\t\t\t\t\t\t\t   │");
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
                                Console.WriteLine("│\t\t\t      Order {0} Payment      \t\t\t   │", chooseidinvoice);
                                Console.WriteLine(lined);
                                Console.WriteLine("│ Order Id : {0, -62}│\n│ Date : {1, -66}│\n│ Table : {2, -65}│", invoice2.Invoice_ID, invoice2.Invoices_Date, invoice2.table.Name);
                                Console.WriteLine(lined);
                                Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", "Id", "Name", "Price", "Quantity", "Amount");
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
                                            Console.WriteLine("┼──────────────────────────────────────────────┼");
                                            Console.WriteLine("│                                              │");
                                            Console.Write("│             ");
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.Write("Payment Complete!");
                                            Console.ResetColor();
                                            Console.WriteLine("                │");
                                            Console.WriteLine("│                                              │");
                                            Console.WriteLine("┼──────────────────────────────────────────────┼");
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
                                Console.WriteLine("│\t\t\t      Order {0} Cancel      \t\t\t   │", chooseidinvoice);
                                Console.WriteLine(lined);
                                Console.WriteLine("│ Order Id : {0, -62}│\n│ Date : {1, -66}│\n│ Table : {2, -65}│", invoice2.Invoice_ID, invoice2.Invoices_Date, invoice2.table.Name);
                                Console.WriteLine(lined);
                                Console.WriteLine("│ {0,-5} │ {1,-22} │ {2,-10} │ {3,-10} │ {4,-13} │", "Id", "Name", "Price", "Quantity", "Amount");
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

                                        }
                                        else
                                        {

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
                                DisplayItem(items);
                                while (true)
                                {
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

                                            }
                                            else
                                            {

                                            }
                                        }
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.Write("Added Successfully!");
                                        Console.ResetColor();
                                    }
                                }
                                break;
                            case 4://Remove dishes
                                Console.Clear();
                                Console.Write("Input Dishes Id to remove or input 0 to back to menu : ");
                                int removeid = GetID();
                                if (removeid == 0) break;
                                Item item2 = itemBL.GetById(removeid);
                                if (item2 == null)
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
                                }
                                break;
                            case 5:
                                break;
                        }
                    }
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
            string title = "TABLES";
            int position = line.Length / 2 - title.Length / 2;
            Console.WriteLine(line);
            Console.WriteLine("│{0," + position + "}\b{1}\t\t\t │", "", title);
            Console.WriteLine(line);
            Console.WriteLine("│ {0,-10} │ {1,-15} │ {2,-15} │", "ID", "Name", "Status");
            Console.WriteLine(line);
            for (int i = 0; i < tables.Count; i++)
            {
                status = tables[i].Status == TableFood.READY_STATUS ? "Ready" : "In Use";
                Console.WriteLine("│ {0,-10} │ {1,-15} │ {2,-15} │", tables[i].TableId, tables[i].Name, status);
                Console.WriteLine(line);
            }
        }
        static void DisplayInvoice(List<Invoice> invoices)
        {
            Console.Clear();
            string status;
            string line = "┼──────────────────────────────────────────────────────────────────┼";
            string title = "LIST ORDER";
            int position = line.Length / 2 - title.Length / 2;
            Console.WriteLine(line);
            Console.WriteLine("│{0," + position + "}\b{1}\t\t\t\t   │", "", title);
            Console.WriteLine(line);
            Console.WriteLine("│ {0,-10} │ {1,-21} │ {2,-15} │ {3,-9} │", "ID", "Date", "Table", "Status");
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
        static int Menu(string title, string[] menuItems)
        {
            int choose = 0;
            string line = "┼──────────────────────────────────────────┼";
            int position = line.Length / 2 - title.Length / 2;
            Console.Clear();
            Console.WriteLine(line);
            Console.WriteLine("│                                          │");
            Console.WriteLine("│{0," + position + "}\b{1}\t   │", "", title);
            Console.WriteLine("│                                          │");
            Console.WriteLine(line);
            for (int i = 0; i < menuItems.Length; i++)
            {
                Console.WriteLine("│  {0}. {1}", i + 1, menuItems[i]);
            }
            Console.WriteLine(line);
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
            string title = "MENU";
            int position = line.Length / 2 - title.Length / 2;
            Console.WriteLine(line);
            Console.WriteLine("│{0," + position + "}\b{1}\t\t\t\t\t│", "", title);
            Console.WriteLine(line);
            Console.WriteLine("│ {0,-10} │ {1,-20} │ {2,-15} │ {3,-15} │", "ID", "Name", "Price", "Category");
            Console.WriteLine(line);
            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine("│ {0,-10} │ {1,-20} │ {2,-15} │ {3, -15} │", items[i].ItemsID, items[i].ItemName, items[i].ItemPrice, items[i].CategoryInfo.CategoryName);
                Console.WriteLine(line);
            }
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
