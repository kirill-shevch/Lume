using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class EventTypeToEventEntityConfiguration : IEntityTypeConfiguration<EventTypeToEventEntity>
	{
		public void Configure(EntityTypeBuilder<EventTypeToEventEntity> builder)
		{
			builder.ToTable(EventTypeToEventEntity.TableName);
			builder.HasKey(t => new { t.EventTypeId, t.EventId });
			builder.Property(t => t.EventTypeId);
			builder.Property(t => t.EventId);

			builder.HasOne(ptc => ptc.Event)
				.WithMany(c => c.EventTypes)
				.HasForeignKey(ptc => ptc.EventId);

			builder.HasOne(ptc => ptc.EventType)
				.WithMany()
				.HasForeignKey(ptc => ptc.EventTypeId);
		}
	}
}