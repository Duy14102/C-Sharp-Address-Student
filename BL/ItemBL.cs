using System;
using System.Collections.Generic;
using Persistance;
using DAL;

namespace BL
{
    public class ItemBL
    {
        private ItemDAL itemDAL = new ItemDAL();

        public List<Item> GetItems()
        {
            return itemDAL.GetItems();
        }
        public Item GetById(int id)
        {
            return itemDAL.GetById(id);
        }
        public int AddItem(Item item)
        {
            return itemDAL.AddItem(item) ?? 0;
        }
        public List<Item> GetByName(string name)
        {
            return itemDAL.GetByName(new Item{ItemName = name});
        }
    }
}