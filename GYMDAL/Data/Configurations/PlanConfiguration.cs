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
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {

            builder.Property(x => x.Name).HasMaxLength(50).HasColumnType("varchar").IsRequired();
            builder.Property(x => x.Description).HasMaxLength(200).HasColumnType("varchar").IsRequired();
            builder.Property(x => x.Price).HasPrecision(10, 2);
            builder.ToTable(x => { x.HasCheckConstraint("Plan_DurationDaysChick", "DurationDays between 1 and 365"); });
        }
    }
}
