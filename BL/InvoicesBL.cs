using System;
using System.Collections.Generic;
using Persistance;
using DAL;
namespace BL
{
    public class InvoicesBL
    {
        InvoicesDAL invoicesDAL = new InvoicesDAL();
        public Invoice GetInvoicesId(int id)
        {
            return invoicesDAL.GetInvoicesId(id);
        }
        public bool CreateInvoice(Invoice invoice)
        {
            bool result = invoicesDAL.CreateInvoice(invoice);
            return result;
        }
        public List<Invoice> GetAllInvoice()
        {
            return invoicesDAL.GetAllInvoice();
        }
        public bool GetPayment(int id)
        {
            return invoicesDAL.GetPayment(id);
        }
        public Invoice GetInvoiceHistory(int id)
        {
            return invoicesDAL.GetInvoiceHistory(id);
        }
        public List<Invoice> GetHistory()
        {
            return invoicesDAL.GetHistory();
        }
        public bool GetCancelInvoice(int id)
        {
            return invoicesDAL.GetCancelInvoice(id);
        }
        public bool UpdateQuantityItem(int itemId, int id, int count)
        {
            return invoicesDAL.UpdateQuantityItem(itemId, id, count);
        }
        public bool UpdateItemNew(int id, int itemId, int count)
        {
            return invoicesDAL.UpdateItemNew(id, itemId, count);
        }
        public bool removeitem(int id)
        {
            return invoicesDAL.removeitem(id);
        }
    }
}