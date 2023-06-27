using Inventory.Models;
using Inventory.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Data
{
    public class InventoryContext : IdentityDbContext<User, Role, int>
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) { }

        public DbSet<Addressing> Addressing { get; set; }
        public DbSet<AddressingsInventoryStart> AddressingsInventoryStart { get; set; }
        public DbSet<InventoryMovement> InventoryMovement { get; set; }
        public DbSet<InventoryStart> InventoryStart { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<ItemsAddressings> ItemsAddressing { get; set; }
        public DbSet<StockTaking> StockTaking { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.HasOne(ur => ur.Role)
                        .WithMany(r => r.UserRole)
                        .HasForeignKey(ur => ur.RoleId)
                        .IsRequired();

                userRole.HasOne(ur => ur.User)
                        .WithMany(r => r.UserRole)
                        .HasForeignKey(ur => ur.UserId)
                        .IsRequired();
            });

            modelBuilder.Entity<Warehouse>()
                        .HasMany(s => s.Addressings)
                        .WithOne(s => s.Warehouse)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InventoryStart>()
                        .HasMany(s => s.Addressings)
                        .WithOne(s => s.InventoryStart)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AddressingsInventoryStart>()
                        .HasMany(s => s.StockTaking)
                        .WithOne(s => s.AddressingsInventoryStart)
                        .OnDelete(DeleteBehavior.Cascade);
        }

    }
}