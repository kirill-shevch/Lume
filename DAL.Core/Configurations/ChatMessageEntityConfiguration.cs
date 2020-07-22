using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class ChatMessageEntityConfiguration : IEntityTypeConfiguration<ChatMessageEntity>
	{
		public void Configure(EntityTypeBuilder<ChatMessageEntity> builder)
		{
			builder.ToTable(ChatMessageEntity.TableName);
			builder.HasKey(t => t.ChatMessageId);
			builder.Property(t => t.ChatMessageUid);
			builder.Property(t => t.MessageTime);
			builder.Property(t => t.Content);
			builder.Property(t => t.AuthorId);

			builder.HasMany(t => t.ChatImageContentEntities).WithOne().HasForeignKey(t => t.ChatMessageId);
			builder.HasOne(t => t.Author).WithMany().HasForeignKey(t => t.AuthorId);
		}
	}
}
