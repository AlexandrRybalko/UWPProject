using Microsoft.EntityFrameworkCore;
using UwpBackend.Models;

namespace UwpBackend
{
    public class CamerasDbContext : DbContext
    {
        public CamerasDbContext(DbContextOptions<CamerasDbContext> options) : base(options)
        {
        }

        public DbSet<Camera> Cameras { get; set; }
    }
}
