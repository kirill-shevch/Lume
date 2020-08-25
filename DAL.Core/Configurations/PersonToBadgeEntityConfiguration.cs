using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class PersonToBadgeEntityConfiguration : IEntityTypeConfiguration<PersonToBadgeEntity>
	{
		public void Configure(EntityTypeBuilder<PersonToBadgeEntity> builder)
		{
			builder.ToTable(PersonToBadgeEntity.TableName);
			builder.HasKey(t => new { t.PersonId, t.BadgeId });
			builder.Property(t => t.PersonId);
			builder.Property(t => t.BadgeId);
			builder.Property(t => t.IsViewed);

			builder.HasOne(ptc => ptc.Person)
				.WithMany(c => c.Badges)
				.HasForeignKey(ptc => ptc.PersonId);

			builder.HasOne(ptc => ptc.Badge)
				.WithMany()
				.HasForeignKey(ptc => ptc.BadgeId);
		}
	}
}