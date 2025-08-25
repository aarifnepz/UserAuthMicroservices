using Microsoft.EntityFrameworkCore;

namespace RoleService
{
    public class RoleContext : DbContext
    {
        public RoleContext(DbContextOptions<RoleContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
    }
}