using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class PersonToChatEntityConfiguration : IEntityTypeConfiguration<PersonToChatEntity>
	{
		public void Configure(EntityTypeBuilder<PersonToChatEntity> builder)
		{
			builder.ToTable(PersonToChatEntity.TableName);
			builder.HasKey(t => new { t.FirstPersonId, t.SecondPersonId, t.ChatId });
			builder.Property(t => t.FirstPersonId);
			builder.Property(t => t.SecondPersonId);
			builder.Property(t => t.ChatId);

			builder.HasOne(ptc => ptc.Chat)
				.WithMany(c => c.PersonList)
				.HasForeignKey(ptc => ptc.ChatId);

			builder.HasOne(ptc => ptc.FirstPerson)
				.WithMany()
				.HasForeignKey(ptc => ptc.FirstPersonId);

			builder.HasOne(ptc => ptc.SecondPerson)
				.WithMany()
				.HasForeignKey(ptc => ptc.SecondPersonId);
		}
	}
}
