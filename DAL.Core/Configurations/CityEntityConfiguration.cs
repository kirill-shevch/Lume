using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class CityEntityConfiguration : IEntityTypeConfiguration<CityEntity>
	{
		public void Configure(EntityTypeBuilder<CityEntity> builder)
		{
			builder.ToTable(CityEntity.TableName);
			builder.HasKey(t => t.CityId);
			builder.Property(t => t.CityName);
		}
	}
}
