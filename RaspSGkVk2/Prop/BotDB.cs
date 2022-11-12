using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspSGkVk2.Prop
{
    internal class BotDB : DbContext
    {

        public DbSet<Settings> Settings { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
      
        public DbSet<Prop.Book> Book { get; set; }
        public BotDB()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=prop.db");
            
        }
    }
}
