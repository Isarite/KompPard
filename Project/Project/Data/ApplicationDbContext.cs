using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<ServiceItem> ServiceItems { get; set; }

        public DbSet<InventoryItemCategory> InventoryItemCategories { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<OrderedInventoryItem> OrderedInventoryItems { get; set; }
        public DbSet<OrderedServiceItem> OrderedServiceItems { get; set; }

        public DbSet<MM_InventoryItem_Category> MmInventoryItemCategories { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Feedback> Feedback { get; set; }

        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Feedback>()
                .HasKey(k => new { k.ItemId, k.UserId });
            modelBuilder.Entity<MM_InventoryItem_Category>()
                .HasKey(k => new { k.InventoryItemId, k.UserId });
            modelBuilder.Entity<OrderedInventoryItem>()
                .HasKey(k => new { k.CartId, k.ItemId });
            modelBuilder.Entity<OrderedServiceItem>()
                .HasKey(k => new { k.CartId, k.ServiceId });
            modelBuilder.Entity<Cart>()
                .HasMany(o => o.OrderedServiceItems)
                .WithOne(o => o.Cart);
            modelBuilder.Entity<Cart>()
                .HasMany(o => o.OrderedInventoryItems)
                .WithOne(o => o.Cart);

            base.OnModelCreating(modelBuilder);
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
    }
}
