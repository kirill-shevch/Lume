using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class BadgeEntityConfiguration : IEntityTypeConfiguration<BadgeEntity>
	{
		public void Configure(EntityTypeBuilder<BadgeEntity> builder)
		{
			builder.ToTable(BadgeEntity.TableName);
			builder.HasKey(t => t.BadgeId);
			builder.Property(t => t.BadgeUid);
			builder.Property(t => t.Name);
		}
	}
}
