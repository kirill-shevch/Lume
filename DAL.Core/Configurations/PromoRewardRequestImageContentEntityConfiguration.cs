using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class PromoRewardRequestImageContentEntityConfiguration : IEntityTypeConfiguration<PromoRewardRequestImageContentEntity>
	{
		public void Configure(EntityTypeBuilder<PromoRewardRequestImageContentEntity> builder)
		{
			builder.ToTable(PromoRewardRequestImageContentEntity.TableName);
			builder.HasKey(t => t.PromoRewardRequestImageContentId);
			builder.Property(t => t.PromoRewardRequestImageContentUid);
			builder.Property(t => t.PromoRewardRequestId);
		}
	}
}