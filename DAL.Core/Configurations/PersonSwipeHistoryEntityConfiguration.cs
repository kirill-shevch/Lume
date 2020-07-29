using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class PersonSwipeHistoryEntityConfiguration : IEntityTypeConfiguration<PersonSwipeHistoryEntity>
	{
		public void Configure(EntityTypeBuilder<PersonSwipeHistoryEntity> builder)
		{
			builder.ToTable(PersonSwipeHistoryEntity.TableName);
			builder.HasKey(t => new { t.PersonId, t.EventId });
			builder.Property(t => t.PersonId);
			builder.Property(t => t.EventId);

			builder.HasOne(ptc => ptc.Person)
				.WithMany(ptc => ptc.SwipeHistory)
				.HasForeignKey(ptc => ptc.PersonId);

			builder.HasOne(ptc => ptc.Event)
				.WithMany()
				.HasForeignKey(ptc => ptc.EventId);
		}
	}
}
