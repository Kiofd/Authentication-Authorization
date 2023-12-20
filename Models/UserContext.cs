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

        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminEmail = "admin@gmail.com";
            string adminPassword = "123";

            Role adminRole = new Role { Id = 1, Name = "admin" };
            Role userRole = new Role { Id = 2, Name = "buyer" };
            User adminUser = new User { Id=1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id };
            User userUser = new User { Id = 2, Email = "user@gmail.com", Password = adminPassword, RoleId = userRole.Id };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser, userUser });
            base.OnModelCreating(modelBuilder);        
        }
    }
}
