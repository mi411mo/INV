using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context
{
    public class InvoicingContext : DbContext
    {
        public InvoicingContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<InvoiceModel> Invoices{ get; set; }
        public DbSet<Merchant> Merchants{ get; set; }
        public DbSet<Payment> Payments{ get; set; }


    }
}
