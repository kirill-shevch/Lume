using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Core.Configurations
{
	public class EventEntityConfiguration : IEntityTypeConfiguration<EventEntity>
	{
		public void Configure(EntityTypeBuilder<EventEntity> builder)
		{
			builder.ToTable(EventEntity.TableName);
			builder.HasKey(t => t.EventId);
			builder.Property(t => t.EventUid);
			builder.Property(t => t.Name);
			builder.Property(t => t.MinAge);
			builder.Property(t => t.MaxAge);
			builder.Property(t => t.XCoordinate);
			builder.Property(t => t.YCoordinate);
			builder.Property(t => t.Description);
			builder.Property(t => t.StartTime);
			builder.Property(t => t.EndTime);
			builder.Property(t => t.IsOpenForInvitations);
			builder.Property(t => t.IsOnline);
			builder.Property(t => t.EventTypeId);
			builder.Property(t => t.EventStatusId);
			builder.Property(t => t.AdministratorId);
			builder.Property(t => t.ChatId);
			builder.HasMany(t => t.EventImageContentEntities).WithOne().HasForeignKey(x => x.EventId);
			builder.HasOne(t => t.Chat).WithMany().HasForeignKey(x => x.ChatId);
			builder.HasOne(t => t.EventType).WithMany().HasForeignKey(x => x.EventTypeId);
			builder.HasOne(t => t.EventStatus).WithMany().HasForeignKey(x => x.EventStatusId);
			builder.HasOne(t => t.Administrator).WithMany().HasForeignKey(x => x.AdministratorId);
		}
	}
}
