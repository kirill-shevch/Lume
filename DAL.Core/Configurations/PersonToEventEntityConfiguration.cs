using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class PersonToEventEntityConfiguration : IEntityTypeConfiguration<PersonToEventEntity>
	{
		public void Configure(EntityTypeBuilder<PersonToEventEntity> builder)
		{
			builder.ToTable(PersonToEventEntity.TableName);
			builder.HasKey(t => new { t.PersonId, t.EventId });
			builder.Property(t => t.PersonId);
			builder.Property(t => t.EventId);

			builder.HasOne(ptc => ptc.Event)
				.WithMany(c => c.Participants)
				.HasForeignKey(ptc => ptc.EventId);

			builder.HasOne(ptc => ptc.Person)
				.WithMany(p => p.Events)
				.HasForeignKey(ptc => ptc.PersonId);
		}
	}
}