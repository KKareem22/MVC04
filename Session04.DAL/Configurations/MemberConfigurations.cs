using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Session04.DAL.Models;

namespace Session04.DAL.Configurations
{
    public class MemberConfigurations : GymUserConfigurations<Member>, IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(x => x.CreatedAt)
                .HasColumnName("JoinDate")
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(M => M.HealthRecord)
                .WithOne(HR => HR.Member)
                .HasForeignKey<HealthRecord>(HR => HR.MemberId);

            base.Configure(builder);
        }
    }
}
