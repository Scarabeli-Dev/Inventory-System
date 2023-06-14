using Inventory.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Data
{
    public class InventoryContext : IdentityDbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) { }

        public DbSet<Addressing> Addressing { get; set; }
        public DbSet<AddressingsStockTaking> AddressingsStockTaking { get; set; }
        public DbSet<InventoryMovement> InventoryMovement { get; set; }
        public DbSet<InventoryStart> InventoryStart { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<ItemsAddressings> ItemsAddressing { get; set; }
        public DbSet<StockTaking> StockTaking { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<ItemsAddressings>(itemsAddressings =>
            //{
            //    itemsAddressings.HasOne(il => il.Item)
            //                  .WithMany(i => i.Addressing)
            //                  .HasForeignKey(il => il.ItemId);

            //    itemsAddressings.HasOne(il => il.Addressing)
            //                  .WithMany(i => i.Item)
            //                  .HasForeignKey(il => il.AddressingId);
            //});


            //modelBuilder.Entity<IventoriesItems>()
            //            .HasKey(ii => new { ii.ItemId, ii.StockTakingId });

            modelBuilder.Entity<Warehouse>()
                        .HasMany(s => s.Addressings)
                        .WithOne(s => s.Warehouse)
                        .OnDelete(DeleteBehavior.Cascade);
        }

    }
}