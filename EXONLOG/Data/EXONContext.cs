namespace EXONLOG.Data
{
    using Microsoft.EntityFrameworkCore;
    using EXONLOG.Model.Account;
    using EXONLOG.Model.Inbound;
    using EXONLOG.Model.Outbound;
    using EXONLOG.Model.Shared;
    using EXONLOG.Model.Trans;
    using Microsoft.AspNetCore.Identity;

    public class EXONContext : DbContext
    {
        /// <summary>
        /// Shared Models
        /// </summary>
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialType> MaterialTypes { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }
        

        /// <summary>
        /// Account Models
        /// </summary>
        public DbSet<User> Users { get; set; } // DbSet for User
        public DbSet<Role> Roles { get; set; } // DbSet for User

        /// <summary>
        /// OutBound Models
        /// </summary>
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Importer> Importers { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<OutLading> OutLadings { get; set; } // DbSet for Lading
       
        /// <summary>
        /// Transportation models
        /// </summary>
        public DbSet<TransCompany> TransCompanies { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Driver> Drivers { get; set; }

        /// <summary>
        /// InBound Models
        /// </summary>
        /// 
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<InLading> InLadings { get; set; }
        public DbSet<ShipmentStatus> ShipmentStatuses { get; set; }




        public EXONContext(DbContextOptions<EXONContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var createDateProperty = entityType.FindProperty("CreateDate");
                if (createDateProperty != null && createDateProperty.ClrType == typeof(DateTime))
                {
                    createDateProperty.SetColumnType("datetime2(0)");
                    createDateProperty.SetDefaultValueSql("GETDATE()");
                }
            }
          

            // Example: Configure relationships with User
            modelBuilder.Entity<Material>()
                .HasOne(m => m.User)
                .WithMany() // One User can create many materials
                .HasForeignKey(m => m.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Material>()
                .HasOne(m => m.MaterialType)
                .WithMany() // One User can create many materials
                .HasForeignKey(m => m.MaterialTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.User)
                .WithMany() // One User can create many customers
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Importer>()
                .HasOne(i => i.User)
                .WithMany() // One User can create many importers
                .HasForeignKey(i => i.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Port>()
                .HasOne(p => p.User)
                .WithMany() // One User can create many ports
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // User-Entity Relationships
            modelBuilder.Entity<User>()
                .HasMany(u => u.Materials)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Customers)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Importers)
                .WithOne(i => i.User)
                .HasForeignKey(i => i.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Ports)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Contracts)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Customer-Contract relationship: One Customer can have many Contracts
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Contracts)
                .WithOne(c => c.Customer) // Each Contract is related to one Customer
                .HasForeignKey(c => c.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            // Contract-Order relationship: One Contract can have many Orders
            modelBuilder.Entity<Contract>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Contract)
                .HasForeignKey(o => o.ContractID)
                .OnDelete(DeleteBehavior.Restrict);

          
            modelBuilder.Entity<Truck>()
               .HasIndex(t => new { t.TruckNumber, t.TrailerNumber })
               .IsUnique()
               .HasDatabaseName("IX_Truck_TruckNumber_TrailerNumber"); // Optional: Name the index

            // Configure the relationships for FirstWeigher and SecondWeigher
            modelBuilder.Entity<OutLading>()
                .HasOne(l => l.FirstWeigher)  // First weigher as a user
                .WithMany()  // Assuming each User can be the first weigher in many ladings
                .HasForeignKey(l => l.FirstWeigherID)
                .OnDelete(DeleteBehavior.Restrict);  // Handle delete behavior

            modelBuilder.Entity<OutLading>()
                .HasOne(l => l.SecondWeigher) // Second weigher as a user
                .WithMany()
                .HasForeignKey(l => l.SecondWeigherID)
                .OnDelete(DeleteBehavior.Restrict);
            // Configure other fields, such as constraints and defaults
            modelBuilder.Entity<OutLading>()
                .Property(l => l.WeightStatus)
                .HasMaxLength(50)
                .IsRequired(); // WeightStatus must be defined for each Lading

            modelBuilder.Entity<OutLading>()
                .Property(l => l.ShippingType)
                .HasMaxLength(50); // ShippingType field

            modelBuilder.Entity<OutLading>()
                .Property(l => l.Shrink)
                .HasDefaultValue(0); // Default value for Shrink if not calculated

            // Configure default CreateDate
            modelBuilder.Entity<OutLading>()
                .Property(l => l.CreateDate)
                .HasDefaultValueSql("GETDATE()"); // Set default date to current date

            // Configure the one-to-many relationship between Order and OutLading
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OutLadings) // An Order has many OutLadings
                .WithOne(ol => ol.Order) // Each OutLading is linked to one Order
                .HasForeignKey(ol => ol.OrderID) // Foreign key in OutLading referring to Order
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of Order if OutLading exists


            // Add more relationships as needed

            // Inbound Models Relationships

            modelBuilder.Entity<Supplier>()
                 .HasMany(s => s.Shipments)
                 .WithOne(i => i.Supplier)
                 .HasForeignKey(i => i.SupplierID)
                 .OnDelete(DeleteBehavior.Restrict); // Prevent deleting a supplier with related shipments

            // Shipment -> Batches Relationship
            modelBuilder.Entity<Shipment>()
                .HasMany(s => s.Batches)
                .WithOne(b => b.Shipment)
                .HasForeignKey(b => b.ShipmentID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting shipment with batches

            modelBuilder.Entity<Importer>()
              .HasMany(i => i.Shipments) // An Importer can have many Shipments
              .WithOne(s => s.Importer)  // Each Shipment is linked to one Importer
              .HasForeignKey(s => s.ImporterID)
              .OnDelete(DeleteBehavior.Restrict); // Foreign key in Shipment referring to Importer


            // Shipment -> Batches Relationship
            modelBuilder.Entity<Batch>()
                .HasOne(b => b.Shipment)
                .WithMany(s => s.Batches)
                .HasForeignKey(b => b.ShipmentID)
                .OnDelete(DeleteBehavior.Restrict);

            // Batch -> IncomingLadings Relationship
            modelBuilder.Entity<Batch>()
                .HasMany(b => b.InLadings)
                .WithOne(il => il.Batch)
                .HasForeignKey(il => il.BatchID)
                .OnDelete(DeleteBehavior.Restrict);

            // Batch -> InLadings Relationship
            modelBuilder.Entity<InLading>()
                .HasOne(il => il.Batch)
                .WithMany(b => b.InLadings)
                .HasForeignKey(il => il.BatchID)
                .OnDelete(DeleteBehavior.Restrict);

            // Truck -> InLadings Relationship
            modelBuilder.Entity<InLading>()
                .HasOne(il => il.Truck)
                .WithMany()
                .HasForeignKey(il => il.TruckID)
                .OnDelete(DeleteBehavior.Restrict);

            // Driver -> InLadings Relationship
            modelBuilder.Entity<InLading>()
                .HasOne(il => il.Driver)
                .WithMany()
                .HasForeignKey(il => il.DriverID)
                .OnDelete(DeleteBehavior.Restrict);

            // First Weigher -> InLadings Relationship
            modelBuilder.Entity<InLading>()
                .HasOne(il => il.FirstWeigher)
                .WithMany()
                .HasForeignKey(il => il.FirstWeigherID)
                .OnDelete(DeleteBehavior.Restrict);

            // Second Weigher -> InLadings Relationship
            modelBuilder.Entity<InLading>()
                .HasOne(il => il.SecondWeigher)
                .WithMany()
                .HasForeignKey(il => il.SecondWeigherID)
                .OnDelete(DeleteBehavior.Restrict);

            // OutboundLading -> TransCompany Relationship
            modelBuilder.Entity<OutLading>()
                .HasOne(ol => ol.TransCompany)
                .WithMany()
                .HasForeignKey(ol => ol.TransCompanyID)
                .OnDelete(DeleteBehavior.Restrict);  // Optional: Define delete behavior

            // InLading -> TransCompany Relationship
            modelBuilder.Entity<InLading>()
                .HasOne(il => il.TransCompany)
                .WithMany()
                .HasForeignKey(il => il.TransCompanyID)
                .OnDelete(DeleteBehavior.Restrict);  // Optional: Define delete behavior

        }
    }

}
