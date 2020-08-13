using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class PersonFriendListEntityConfiguration : IEntityTypeConfiguration<PersonFriendListEntity>
	{
		public void Configure(EntityTypeBuilder<PersonFriendListEntity> builder)
		{
			builder.ToTable(PersonFriendListEntity.TableName);
			builder.HasKey(t => new { t.PersonId, t.FriendId });
			builder.Property(t => t.PersonId);
			builder.Property(t => t.FriendId);
			builder.Property(t => t.IsApproved);
			builder.HasOne(t => t.Person).WithMany(x => x.FriendList).HasForeignKey(x => x.PersonId);
			builder.HasOne(t => t.Friend).WithMany().HasForeignKey(x => x.FriendId);
		}
	}
}