using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data.Config
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Address).IsRequired().HasMaxLength(100);
            builder.Property(x => x.MailAdress).IsRequired().HasMaxLength(50);
            builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(11);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(50);
            builder.Property(x => x.MersisNo).IsRequired().HasMaxLength(100);
            builder.Property(x => x.TaxNo).IsRequired().HasMaxLength(100);
            builder.Property(x => x.TaxDepartment).IsRequired().HasMaxLength(100);
            builder.Property(x => x.FoundationYear).HasColumnType("date");
            builder.Property(x => x.ContractStartYear).HasColumnType("date");
            builder.Property(x => x.ContractEndYear).HasColumnType("date");
            builder
               .Property(s => s.RemainingPackageTime)
               .HasConversion(new TimeSpanToStringConverter());
        }
    }
}
