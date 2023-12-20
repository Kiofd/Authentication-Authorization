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
            Role buyerRole = new Role { Id = 2, Name = "byuer" };
            Role adminRole = new Role { Id = 1, Name = "admin" };
            User admin = new User() { Id = 1, Role = adminRole, RoleId = adminRole.Id, Password = "pass123" };

            modelBuilder.Entity<Role>().HasData(buyerRole, adminRole);
            modelBuilder.Entity<User>().HasData(admin);
            base.OnModelCreating(modelBuilder);
        }
    }
}
