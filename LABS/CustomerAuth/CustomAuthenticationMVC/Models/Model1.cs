namespace CustomAuthenticationMVC.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<TCustomer> TCustomers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TCustomer>()
                .Property(e => e.cust_name)
                .IsUnicode(false);

            modelBuilder.Entity<TCustomer>()
                .Property(e => e.cust_phone)
                .IsUnicode(false);

            modelBuilder.Entity<TCustomer>()
                .Property(e => e.cust_address)
                .IsUnicode(false);

            modelBuilder.Entity<TCustomer>()
                .Property(e => e.cust_email)
                .IsUnicode(false);
        }
    }
}
