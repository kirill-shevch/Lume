using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class ChatEntityConfiguration : IEntityTypeConfiguration<ChatEntity>
	{
		public void Configure(EntityTypeBuilder<ChatEntity> builder)
		{
			builder.ToTable(ChatEntity.TableName);
			builder.HasKey(t => t.ChatId);
			builder.Property(t => t.ChatUid);
			builder.Property(t => t.IsGroupChat);
			builder.HasMany(t => t.ChatMessageEntities).WithOne().HasForeignKey(t => t.ChatId);
		}
	}
}
