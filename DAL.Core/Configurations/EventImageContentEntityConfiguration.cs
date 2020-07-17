using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class EventImageContentEntityConfiguration : IEntityTypeConfiguration<EventImageContentEntity>
	{
		public void Configure(EntityTypeBuilder<EventImageContentEntity> builder)
		{
			builder.ToTable(EventImageContentEntity.TableName);
			builder.HasKey(t => t.EventImageContentId);
			builder.Property(t => t.EventImageContentUid);
			builder.Property(t => t.ContentHash);
			builder.Property(t => t.Content);
		}
	}
}