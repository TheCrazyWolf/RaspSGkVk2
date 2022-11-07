using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspSGkVk2.Prop
{
    internal class AppContext : DbContext
    {

        public DbSet<Settings> Settings { get; set; } 
        public DbSet<Tasks> Tasks { get; set; } 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=prop.db");
        }
    }
}
