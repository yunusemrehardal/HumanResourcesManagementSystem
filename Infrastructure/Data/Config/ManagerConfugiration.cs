﻿using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Config
{
    public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Surname).IsRequired().HasMaxLength(50);
            builder.Property(x => x.SecondName).HasMaxLength(50);
            builder.Property(x => x.SecondSurname).HasMaxLength(50);
            builder.Property(x => x.BirthDate).IsRequired().HasColumnType("date");
            builder.Property(x => x.BirthPlace).HasMaxLength(50);
            builder.Property(x => x.StartDateOfWork).IsRequired().HasColumnType("date");
            builder.Property(x => x.EndDateOfWork).HasColumnType("date");
            builder.Property(x => x.Job).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Address).IsRequired().HasMaxLength(100);
            builder.Property(x => x.MailAdress).IsRequired().HasMaxLength(50);
            builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(11);
            builder.Property(x => x.TC).IsRequired().HasMaxLength(12);
        }
    }
}
