using System;
using System.Collections.Generic;

namespace Persistance
{
    public static class InvoiceStatus
    {
        public const int NEW_INVOICE = 1;
        public const int INVOICE_IN_PROGRESS = 2;
        public const int INVOICE_CANCEL = 3;
    }
    public class Invoice
    {
        public int Invoice_ID { set; get; }
        public DateTime Invoices_Date { set; get; }
        public List<Item> Items { set; get; }
        public int? Invoices_Status { set; get; }
        public TableFood table { set; get; }

        public Invoice()
        {
            table = new TableFood();
            Items = new List<Item>();
        }
        public Item this[int index]
        {
            get
            {
                if (Items == null || Items.Count == 0 || index < 0 || Items.Count < index) return null;
                return Items[index];
            }
            set
            {
                if (Items == null) Items = new List<Item>();
                Items.Add(value);
            }
        }
        public override bool Equals(object obj)
        {
            if (obj is Invoice)
            {
                return ((Invoice)obj).Invoice_ID.Equals(Invoice_ID);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Invoice_ID.GetHashCode();
        }
    }

}