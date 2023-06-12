using Inventory.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Data
{
    public class InventoryContext : IdentityDbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) { }

        public DbSet<Item> Item { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<InventoryCount> InventoryCount { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<ItemsLocations>(itemsLocations =>
            //{
            //    itemsLocations.HasOne(il => il.Item)
            //                  .WithMany(i => i.Location)
            //                  .HasForeignKey(il => il.ItemId);

            //    itemsLocations.HasOne(il => il.Location)
            //                  .WithMany(i => i.Item)
            //                  .HasForeignKey(il => il.LocationId);
            //});

            modelBuilder.Entity<ItemsLocations>()
                        .HasKey(il => new { il.ItemId, il.LocationId });

            //modelBuilder.Entity<IventoriesItems>()
            //            .HasKey(ii => new { ii.ItemId, ii.InventoryCountId });

            modelBuilder.Entity<Stock>()
                        .HasMany(s => s.Locations)
                        .WithOne(s => s.Stock)
                        .OnDelete(DeleteBehavior.Restrict);
        }

    }
}