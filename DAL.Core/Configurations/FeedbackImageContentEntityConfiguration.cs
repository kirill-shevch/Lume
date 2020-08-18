using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class FeedbackImageContentEntityConfiguration : IEntityTypeConfiguration<FeedbackImageContentEntity>
	{
		public void Configure(EntityTypeBuilder<FeedbackImageContentEntity> builder)
		{
			builder.ToTable(FeedbackImageContentEntity.TableName);
			builder.HasKey(t => t.FeedbackImageContentId);
			builder.Property(t => t.FeedbackImageContentUid);
			builder.Property(t => t.FeedbackId);
		}
	}
}
