using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class FeedbackEntityConfiguration : IEntityTypeConfiguration<FeedbackEntity>
	{
		public void Configure(EntityTypeBuilder<FeedbackEntity> builder)
		{
			builder.ToTable(FeedbackEntity.TableName);
			builder.HasKey(t => t.FeedbackId);
			builder.Property(t => t.FeedbackUid);
			builder.Property(t => t.FeedbackTime);
			builder.Property(t => t.Text);
			builder.Property(t => t.PhoneModel);
			builder.Property(t => t.OperatingSystem);
			builder.Property(t => t.ApplicationVersion);
			builder.Property(t => t.PersonId);

			builder.HasMany(t => t.FeedbackImageContentEntities).WithOne().HasForeignKey(t => t.FeedbackId);
			builder.HasOne(t => t.Person).WithMany(t => t.Feedbacks).HasForeignKey(t => t.PersonId);
		}
	}
}
