using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class PromoRewardRequestEntityConfiguration : IEntityTypeConfiguration<PromoRewardRequestEntity>
	{
		public void Configure(EntityTypeBuilder<PromoRewardRequestEntity> builder)
		{
			builder.ToTable(PromoRewardRequestEntity.TableName);
			builder.HasKey(t => t.PromoRewardRequestId);
			builder.Property(t => t.PromoRewardRequestTime);
			builder.Property(t => t.EventId);
			builder.Property(t => t.PromoRewardRequestUid);
			builder.Property(t => t.AccountingNumber);

			builder.HasMany(t => t.Images).WithOne().HasForeignKey(t => t.PromoRewardRequestId);
			builder.HasOne(t => t.Event).WithMany(t => t.PromoRewardRequests).HasForeignKey(t => t.EventId);

		}
	}
}
