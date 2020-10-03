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
			builder.ApplyConfiguration(new EventSwipeHistoryEntityConfiguration());
			builder.ApplyConfiguration(new PersonSwipeHistoryEntityConfiguration());
			builder.ApplyConfiguration(new EventTypeToEventEntityConfiguration());
			builder.ApplyConfiguration(new FeedbackEntityConfiguration());
			builder.ApplyConfiguration(new FeedbackImageContentEntityConfiguration());
			builder.ApplyConfiguration(new BadgeEntityConfiguration());
			builder.ApplyConfiguration(new PersonToBadgeEntityConfiguration());
			builder.ApplyConfiguration(new PromoRewardRequestEntityConfiguration());
			builder.ApplyConfiguration(new PromoRewardRequestImageContentEntityConfiguration());
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
		public DbSet<PersonSwipeHistoryEntity> PersonSwipeHistoryEntities { get; set; }
		public DbSet<EventSwipeHistoryEntity> EventSwipeHistoryEntities { get; set; }
		public DbSet<EventTypeToEventEntity> EventTypeToEventEntities { get; set; }
		public DbSet<FeedbackEntity> FeedbackEntities { get; set; }
		public DbSet<FeedbackImageContentEntity> FeedbackImageContentEntities { get; set; }
		public DbSet<BadgeEntity> Badges { get; set; }
		public DbSet<PersonToBadgeEntity> PersonToBadgeEntities { get; set; }
		public DbSet<PromoRewardRequestEntity> PromoRewardRequestEntities { get; set; }
		public DbSet<PromoRewardRequestImageContentEntity> PromoRewardRequestImageContentEntities { get; set; }
	}
}