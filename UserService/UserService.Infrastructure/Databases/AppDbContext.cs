using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using UserService.Domain.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;
namespace UserService.Infrastructure.Databases
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
       
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            //base.OnModelCreating(uilder);
        }
    }
    
}
