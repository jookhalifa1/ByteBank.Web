using Domain.Entity.BankModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace persistenceLayer.Data
{
     public class StoreDbContext:DbContext
    {
        public StoreDbContext( DbContextOptions<StoreDbContext> options):base(options)
        {
            

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        
        }
        public DbSet<Bank> banks { get; set; }
        public DbSet< CardBank> Cardbanks { get; set; }
        public DbSet<Transactions> transactions { get; set; }
         


    }
}
