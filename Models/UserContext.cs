using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TaskAuthenticationAuthorization.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public UserContext(DbContextOptions<ShoppingContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string buyerRoleName = "byuer";

            Role buyerRole = new Role { Id = 2, Name = buyerRoleName };

            base.OnModelCreating(modelBuilder);
        }
    }
}
