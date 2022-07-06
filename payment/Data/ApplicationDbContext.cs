using Microsoft.EntityFrameworkCore;
using payment.Models;

namespace payment.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Login> Login { get; set; }
        public DbSet<Manager> Manager { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Employees> Employees { get; set; }
    }
}
