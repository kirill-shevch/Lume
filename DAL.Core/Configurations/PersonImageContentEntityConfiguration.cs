using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class PersonImageContentEntityConfiguration : IEntityTypeConfiguration<PersonImageContentEntity>
	{
		public void Configure(EntityTypeBuilder<PersonImageContentEntity> builder)
		{
			builder.ToTable(PersonImageContentEntity.TableName);
			builder.HasKey(t => t.PersonImageContentId);
			builder.Property(t => t.PersonImageContentUid);
			builder.Property(t => t.ContentHash);
			builder.Property(t => t.Content);
		}
	}
}
