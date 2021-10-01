using System;
using System.Collections.Generic;
using Persistance;
using DAL;

namespace BL
{
    public class TableFoodBL
    {
        TableFoodDAL tableDAL = new TableFoodDAL();
        public List<TableFood> GetAllTableFood()
        {
            return tableDAL.GetAllTableFood();
        }
        public TableFood GetById(int id)
        {
            return tableDAL.GetById(id);
        }
        public int AddTable(TableFood table)
        {
            return tableDAL.AddTable(table) ?? 0;
        }
        public bool TableStatusChange(int status, int tableID)
        {
            return tableDAL.TableStatusChange(status, tableID);
        }
        public TableFood TableStatusCheck(int status)
        {
            return tableDAL.TableStatusCheck(status);
        }
        public bool RemoveItem(int id)
        {
            return tableDAL.removeitem(id);
        }
        public List<TableFood> GetByName(string name)
        {
            return tableDAL.GetByName(new TableFood { Name = name });
        }
    }
}