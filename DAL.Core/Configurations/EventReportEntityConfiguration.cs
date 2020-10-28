using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class EventReportEntityConfiguration : IEntityTypeConfiguration<EventReportEntity>
	{
		public void Configure(EntityTypeBuilder<EventReportEntity> builder)
		{
			builder.ToTable(EventReportEntity.TableName);
			builder.HasKey(t => t.EventReportId);
			builder.Property(t => t.EventReportUid);
			builder.Property(t => t.Text);
			builder.Property(t => t.CreationTime);
			builder.Property(t => t.IsProcessed);
			builder.Property(t => t.EventId);
			builder.Property(t => t.AuthorId);
			builder.HasOne(t => t.Event).WithMany().HasForeignKey(x => x.EventId);
			builder.HasOne(t => t.Author).WithMany().HasForeignKey(x => x.AuthorId);
		}
	}
}