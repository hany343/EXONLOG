namespace EXONLOG.Data
{
    using EXONLOG.Data.Entities;
    using EXONLOG.Data.Entities.Outbound;
    using EXONLOG.Data.Trans;
    using Microsoft.EntityFrameworkCore;

    public class EXONContext : DbContext
    {
        public DbSet<Material> Materials { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Importer> Importers { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Contract> Contracts { get; set; }

        public EXONContext(DbContextOptions<EXONContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Example: Configure relationships with User
            modelBuilder.Entity<Material>()
                .HasOne(m => m.User)
                .WithMany() // One User can create many materials
                .HasForeignKey(m => m.UserID);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.User)
                .WithMany() // One User can create many customers
                .HasForeignKey(c => c.UserID);

            modelBuilder.Entity<Importer>()
                .HasOne(i => i.User)
                .WithMany() // One User can create many importers
                .HasForeignKey(i => i.UserID);

            modelBuilder.Entity<Port>()
                .HasOne(p => p.User)
                .WithMany() // One User can create many ports
                .HasForeignKey(p => p.UserID);

            // User-Entity Relationships
            modelBuilder.Entity<User>()
                .HasMany(u => u.Materials)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserID);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Customers)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserID);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Importers)
                .WithOne(i => i.User)
                .HasForeignKey(i => i.UserID);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Ports)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserID);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Contracts)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserID);
            // Customer-Contract relationship: One Customer can have many Contracts
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Contracts)
                .WithOne(c => c.Customer) // Each Contract is related to one Customer
                .HasForeignKey(c => c.CustomerID);

            // Contract-Order relationship: One Contract can have many Orders
            modelBuilder.Entity<Contract>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Contract)
                .HasForeignKey(o => o.ContractID);

          
            // Material-Stock relationship: One Material has one Stock
            modelBuilder.Entity<Material>()
                .HasOne(m => m.Stock)
                .WithOne(s => s.Material)
                .HasForeignKey<Stock>(s => s.MaterialID);

            modelBuilder.Entity<Truck>()
               .HasIndex(t => new { t.TruckNumber, t.TrailerNumber })
               .IsUnique()
               .HasDatabaseName("IX_Truck_TruckNumber_TrailerNumber"); // Optional: Name the index

            // Add more relationships as needed
        }
    }

}
