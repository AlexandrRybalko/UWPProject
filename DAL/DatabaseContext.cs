using DAL.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
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
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

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

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "CamerasDatabase.db");
            if (!File.Exists(dbpath))
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    string createCameras = "CREATE TABLE IF NOT EXISTS Cameras (Id INTEGER, RtspAddress TEXT NOT NULL," +
                        "Country TEXT NOT NULL, City TEXT NOT NULL, Latitude REAL NOT NULL, Longitude REAL NOT NULL, " +
                        "PRIMARY KEY(Id AUTOINCREMENT));";
                    
                    string createCategories = "CREATE TABLE IF NOT EXISTS Categories(Id INTEGER, Title TEXT NOT NULL," +
                        "PRIMARY KEY(Id AUTOINCREMENT));" +
                        "INSERT INTO Categories(Title) VALUES(\"Recent\");" +
                        "INSERT INTO Categories(Title) VALUES(\"Favourites\");";
                    string createCamerasCategories = "CREATE TABLE IF NOT EXISTS CamerasCategories(CameraId INTEGER, CategoryId INTEGER, " +
                        "UpdatedTime TEXT, FOREIGN KEY(CategoryId) REFERENCES Categories(Id), FOREIGN KEY(CameraId) REFERENCES Cameras(Id)," +
                        "PRIMARY KEY(CameraId, CategoryId));";

                    SqliteCommand createCamerasTable = new SqliteCommand(createCameras, db);
                    createCamerasTable.ExecuteReader();
                    createCamerasTable.Dispose();

                    SqliteCommand createCategoriesTable = new SqliteCommand(createCategories, db);
                    createCategoriesTable.ExecuteReader();
                    createCategoriesTable.Dispose();

                    SqliteCommand createCamerasCategoriesTable = new SqliteCommand(createCamerasCategories, db);
                    createCamerasCategoriesTable.ExecuteReader();
                    createCamerasCategoriesTable.Dispose();
                }
            }
        }
    }
}
