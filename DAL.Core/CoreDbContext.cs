using DAL.Core.Configurations;
using DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Core
{
	public class CoreDbContext : DbContext
	{
		public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ApplyConfiguration(new ChatEntityConfiguration());
			builder.ApplyConfiguration(new ChatImageContentEntityConfiguration());
			builder.ApplyConfiguration(new ChatMessageEntityConfiguration());
			builder.ApplyConfiguration(new EventEntityConfiguration());
			builder.ApplyConfiguration(new EventImageContentEntityConfiguration());
			builder.ApplyConfiguration(new EventStatusEntityConfiguration());
			builder.ApplyConfiguration(new EventTypeEntityConfiguration());
			builder.ApplyConfiguration(new PersonEntityConfiguration());
			builder.ApplyConfiguration(new PersonFriendListEntityConfiguration());
			builder.ApplyConfiguration(new PersonImageContentEntityConfiguration());
			builder.ApplyConfiguration(new PersonToChatEntityConfiguration());
			builder.ApplyConfiguration(new PersonToEventEntityConfiguration());
			builder.ApplyConfiguration(new ParticipantStatusEntityConfiguration());
			builder.ApplyConfiguration(new CityEntityConfiguration());
		}

		public DbSet<ChatEntity> ChatEntities { get; set; }
		public DbSet<ChatImageContentEntity> ChatImageContentEntities { get; set; }
		public DbSet<ChatMessageEntity> ChatMessageEntities { get; set; }
		public DbSet<EventEntity> EventEntities { get; set; }
		public DbSet<EventImageContentEntity> EventImageContentEntities { get; set; }
		public DbSet<EventStatusEntity> EventStatusEntities { get; set; }
		public DbSet<EventTypeEntity> EventTypeEntities { get; set; }
		public DbSet<PersonEntity> PersonEntities { get; set; }
		public DbSet<PersonFriendListEntity> PersonFriendListEntities { get; set; }
		public DbSet<PersonImageContentEntity> PersonImageContentEntities { get; set; }
		public DbSet<PersonToChatEntity> PersonToChatEntities { get; set; }
		public DbSet<PersonToEventEntity> PersonToEventEntities { get; set; }
		public DbSet<ParticipantStatusEntity> ParticipantStatusEntities { get; set; }
		public DbSet<CityEntity> CityEntities { get; set; }
	}
}