using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class EventStatusEntityConfiguration : IEntityTypeConfiguration<EventStatusEntity>
	{
		public void Configure(EntityTypeBuilder<EventStatusEntity> builder)
		{
			builder.ToTable(EventStatusEntity.TableName);
			builder.HasKey(t => t.EventStatusId);
			builder.Property(t => t.EventStatusName);
		}
	}
}
