using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Core.Models;

namespace SmartSchool.EF
{
    public class ApplicationDbContext : DbContext
    {
        //This Constarcter is important for connecting to the Database
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
<<<<<<< HEAD
<<<<<<< Updated upstream
=======

        // Here Know what classes are tables in DB
        public DbSet<User> Users { get; set; } 
        public DbSet<Role> Roles { get; set; } 


        //Here write settings of special relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Make default sitting of EF before we edit on it
            base.OnModelCreating(modelBuilder);

            //make the relationship and the foreignKey
=======

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

>>>>>>> 398bc1e9a0a0bd15c61b89ab05abfde1880aae0f
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);
        }

<<<<<<< HEAD
>>>>>>> Stashed changes
=======
>>>>>>> 398bc1e9a0a0bd15c61b89ab05abfde1880aae0f
    }
}
