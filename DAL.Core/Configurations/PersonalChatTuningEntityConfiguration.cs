using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class PersonalChatTuningEntityConfiguration : IEntityTypeConfiguration<PersonalChatTuningEntity>
	{
		public void Configure(EntityTypeBuilder<PersonalChatTuningEntity> builder)
		{
			builder.ToTable(PersonalChatTuningEntity.TableName);
			builder.HasKey(t => new { t.ChatId, t.PersonId });
			builder.Property(t => t.ChatId);
			builder.Property(t => t.PersonId);
			builder.Property(t => t.IsMuted);

			builder.HasOne(ptc => ptc.Chat)
				.WithMany(c => c.PersonalSettings)
				.HasForeignKey(ptc => ptc.ChatId);

			builder.HasOne(ptc => ptc.Person)
				.WithMany()
				.HasForeignKey(ptc => ptc.PersonId);
		}
	}
}