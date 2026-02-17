using Domain.Entity.BankModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace persistenceLayer.Configurations
{
    public class BankConfiguration : IEntityTypeConfiguration<CardBank>
    {
        public void Configure(EntityTypeBuilder<CardBank> builder)
        {
            builder.HasOne(x=>x.bank).WithMany().HasForeignKey(x=>x.BankId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(x => x.Amount).HasPrecision(8, 2);
        }
    }
}
