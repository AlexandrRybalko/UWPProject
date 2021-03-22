using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Windows.Storage;

namespace DAL
{
    public class DatabaseContext : DbContext
    {
        public DbSet<CameraEntity> Cameras { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<CamerasCategories> CamerasCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CamerasCategories>().HasKey(x => new { x.CameraId, x.CategoryId });

            modelBuilder.Entity<CamerasCategories>().HasOne(x => x.Camera)
                .WithMany(x => x.CamerasCategories)
                .HasForeignKey(x => x.CameraId);

            modelBuilder.Entity<CamerasCategories>().HasOne(x => x.Category)
                .WithMany(x => x.CamerasCategories)
                .HasForeignKey(x => x.CategoryId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite("Data Source=CamerasDatabase.db");
        }
    }
}
