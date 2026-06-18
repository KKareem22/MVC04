using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Session04.DAL.Models;

namespace Session04.DAL.Configurations
{
    internal class HelathRecordConfigurations : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.Property(x => x.BloodType)
                .HasMaxLength(5);
            builder.Property(x => x.Note)
                .HasMaxLength(500);
        }
    }
}
