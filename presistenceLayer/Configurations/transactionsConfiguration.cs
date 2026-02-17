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
    public class transactionsConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasOne(x => x.SenderCardBank).WithMany().HasForeignKey(x => x.SenderCard).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.ReciverCardBank).WithMany().HasForeignKey(x => x.ReciverCard).OnDelete(DeleteBehavior.Restrict);
            builder.Property(x=>x.Amount).HasPrecision(8,2);
            builder.Property(x=>x.Fee).HasPrecision(8,2);

        }
    }
}
