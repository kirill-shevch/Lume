using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class ParticipantStatusEntityConfiguration : IEntityTypeConfiguration<ParticipantStatusEntity>
	{
		public void Configure(EntityTypeBuilder<ParticipantStatusEntity> builder)
		{
			builder.ToTable(ParticipantStatusEntity.TableName);
			builder.HasKey(t => t.ParticipantStatusId);
			builder.Property(t => t.ParticipantStatusName);
		}
	}
}