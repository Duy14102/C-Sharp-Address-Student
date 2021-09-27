using System;
using System.Collections.Generic;

namespace Persistance
{
    public class Item
    {
        public int ItemsID { set; get; }
        public string ItemName { set; get; }
        public decimal ItemPrice { set; get; }
        public int Quantity { set; get; }
        public Category CategoryInfo { set; get; }
        public Item()
        {
            CategoryInfo = new Category();
        }
        public override bool Equals(object obj)
        {
            if (obj is Item)
            {
                return ((Item)obj).ItemsID.Equals(ItemsID);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ItemsID.GetHashCode();
        }
    }
}