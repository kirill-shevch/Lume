using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class EventTypeEntityConfiguration : IEntityTypeConfiguration<EventTypeEntity>
	{
		public void Configure(EntityTypeBuilder<EventTypeEntity> builder)
		{
			builder.ToTable(EventTypeEntity.TableName);
			builder.HasKey(t => t.EventTypeId);
			builder.Property(t => t.EventTypeName);
		}
	}
}