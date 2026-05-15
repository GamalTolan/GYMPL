using GYMDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMDAL.Data.Configurations
{
    public class GymUserConfiguration <T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(50).HasColumnType("varchar").IsRequired();
            builder.Property(x => x.Email).HasMaxLength(100).HasColumnType("varchar");
            builder.Property(x => x.PhoneNumber).HasMaxLength(11).HasColumnType("varchar");

            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.BuildingNumber).HasMaxLength(100).HasColumnName("BuildingNumber");
                address.Property(a => a.City).HasMaxLength(50).HasColumnType("varchar").HasColumnName("City");
                address.Property(a => a.Street).HasMaxLength(100).HasColumnType("varchar").HasColumnName("Street");

            });
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.PhoneNumber).IsUnique();

            builder.ToTable(x =>
            {

                x.HasCheckConstraint("GymUser_CheckEmail", "Email LIKE '%_@__%.__%' ");
                x.HasCheckConstraint("GymUser_CheckPhoneNumber", "PhoneNumber LIKE '01%' and PhoneNumber NOT LIKE '%[^0-9]%'");

            });

        }
    }
}
