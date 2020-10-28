using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class PersonReportEntityConfiguration : IEntityTypeConfiguration<PersonReportEntity>
	{
		public void Configure(EntityTypeBuilder<PersonReportEntity> builder)
		{
			builder.ToTable(PersonReportEntity.TableName);
			builder.HasKey(t => t.PersonReportId);
			builder.Property(t => t.PersonReportUid);
			builder.Property(t => t.Text);
			builder.Property(t => t.CreationTime);
			builder.Property(t => t.IsProcessed);
			builder.Property(t => t.PersonId);
			builder.Property(t => t.AuthorId);
			builder.HasOne(t => t.Person).WithMany().HasForeignKey(x => x.PersonId);
			builder.HasOne(t => t.Author).WithMany().HasForeignKey(x => x.AuthorId);
		}
	}
}