using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class EventSwipeHistoryEntityConfiguration : IEntityTypeConfiguration<EventSwipeHistoryEntity>
	{
		public void Configure(EntityTypeBuilder<EventSwipeHistoryEntity> builder)
		{
			builder.ToTable(EventSwipeHistoryEntity.TableName);
			builder.HasKey(t => new { t.EventId, t.PersonId });
			builder.Property(t => t.EventId);
			builder.Property(t => t.PersonId);

			builder.HasOne(ptc => ptc.Event)
				.WithMany(c => c.SwipeHistory)
				.HasForeignKey(ptc => ptc.EventId);

			builder.HasOne(ptc => ptc.Person)
				.WithMany()
				.HasForeignKey(ptc => ptc.PersonId);
		}
	}
}
