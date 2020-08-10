using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class ChatImageContentEntityConfiguration : IEntityTypeConfiguration<ChatImageContentEntity>
	{
		public void Configure(EntityTypeBuilder<ChatImageContentEntity> builder)
		{
			builder.ToTable(ChatImageContentEntity.TableName);
			builder.HasKey(t => t.ChatImageContentId);
			builder.Property(t => t.ChatImageContentUid);
		}
	}
}
