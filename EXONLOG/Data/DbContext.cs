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
        public DbSet<OutboundLading> OutboundLadings { get; set; } // DbSet for Lading
        public DbSet<User> Users { get; set; } // DbSet for User
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<TransCompany> TransCompanies { get; set; }


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

            // Configure the relationships for FirstWeigher and SecondWeigher
            modelBuilder.Entity<OutboundLading>()
                .HasOne(l => l.FirstWeigher)  // First weigher as a user
                .WithMany()  // Assuming each User can be the first weigher in many ladings
                .HasForeignKey(l => l.FirstWeigherID)
                .OnDelete(DeleteBehavior.SetNull);  // Handle delete behavior

            modelBuilder.Entity<OutboundLading>()
                .HasOne(l => l.SecondWeigher) // Second weigher as a user
                .WithMany()
                .HasForeignKey(l => l.SecondWeigherID)
                .OnDelete(DeleteBehavior.SetNull);
            // Configure other fields, such as constraints and defaults
            modelBuilder.Entity<OutboundLading>()
                .Property(l => l.WeightStatus)
                .HasMaxLength(50)
                .IsRequired(); // WeightStatus must be defined for each Lading

            modelBuilder.Entity<OutboundLading>()
                .Property(l => l.ShippingType)
                .HasMaxLength(50); // ShippingType field

            modelBuilder.Entity<OutboundLading>()
                .Property(l => l.Shrink)
                .HasDefaultValue(0); // Default value for Shrink if not calculated

            // Configure default CreateDate
            modelBuilder.Entity<OutboundLading>()
                .Property(l => l.CreateDate)
                .HasDefaultValueSql("GETDATE()"); // Set default date to current date

            // Configure the one-to-many relationship between Order and OutboundLading
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OutboundLadings) // An Order has many OutboundLadings
                .WithOne(ol => ol.Order) // Each OutboundLading is linked to one Order
                .HasForeignKey(ol => ol.OrderID) // Foreign key in OutboundLading referring to Order
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of Order if OutboundLading exists


            // Add more relationships as needed
        }
    }

}
