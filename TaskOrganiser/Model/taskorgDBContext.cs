using System.Data.Entity;

namespace TaskOrganiser.Model
{
    internal class taskorgDBContext : DbContext
    {
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Task> Tasks { get; set; }
    }
}
