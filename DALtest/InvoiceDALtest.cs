using System;
using Xunit;
using Persistance;
using DAL;

namespace DALtest
{
    public class InvoiceDALtest
    {
        private InvoicesDAL invoicesDAL = new InvoicesDAL();
        private Invoice invoice = new Invoice();
        [Fact]
        public void LoginTest1()
        {
            int id = 1;
            invoice.Invoice_ID = id;
            int result = invoicesDAL.GetInvoicesId(id).Invoice_ID;
            Assert.True(id == result);
        }
        [Theory]
        [InlineData(1, 1, 0)]
        [InlineData(2, 2, 0)]
        [InlineData(3, 1, 0)]
        [InlineData(4, 2, 0)]
        [InlineData(5, 1, 0)]
        [InlineData(6, 2, 0)]
        [InlineData(7, 1, 0)]
        [InlineData(8, 2, 0)]
        [InlineData(9, 1, 0)]
        [InlineData(10, 2, 0)]
        public void InsertTest1(int tableid, int status, int expected)
        {
            invoice.table.TableId = tableid;
            invoice.Invoices_Status = status;
            int result = invoicesDAL.InsertTest(invoice);
            Assert.True(result <= expected);
        }
    }
}