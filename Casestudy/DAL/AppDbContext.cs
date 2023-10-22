using Microsoft.EntityFrameworkCore;
using Casestudy.DAL.DomainClasses;


namespace Casestudy.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public virtual DbSet<Brands>? Brands{ get; set; }
        public virtual DbSet<Products>? Products { get; set; }
        public virtual DbSet<Customer>? Customers { get; set; }

        public virtual DbSet<Order>? Orders { get; set; }
        public virtual DbSet<OrderLineItem>? OrderLineItems { get; set; }
        public virtual DbSet<Branch>? Branches { get; set; }


    }
}
