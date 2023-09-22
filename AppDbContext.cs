using ManagementTaskAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagementTaskAPI
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TaskList> TaskLists { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

    }

}
