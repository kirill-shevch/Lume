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
			builder.HasKey(t => new { t.PersonId, t.ChatId });
			builder.Property(t => t.PersonId);
			builder.Property(t => t.ChatId);

			builder.HasOne(ptc => ptc.Chat)
				.WithMany(c => c.PersonList)
				.HasForeignKey(ptc => ptc.ChatId);

			builder.HasOne(ptc => ptc.Person)
				.WithMany(p => p.ChatList)
				.HasForeignKey(ptc => ptc.PersonId);
		}
	}
}
