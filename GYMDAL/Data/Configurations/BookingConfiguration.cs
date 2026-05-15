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
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Ignore(b => b.Id);

            builder.Property(b => b.CreatedAt)
                   .HasColumnName("BookingDate")
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(b => b.Member).WithMany(x => x.MemberSessions).HasForeignKey(x => x.MemberId);
            builder.HasOne(b => b.Session).WithMany(x => x.SessionMembers).HasForeignKey(x => x.SessionId);
            builder.HasKey(x => new { x.MemberId, x.SessionId });
        }
    }
}
