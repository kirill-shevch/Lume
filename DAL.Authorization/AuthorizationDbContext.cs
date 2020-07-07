using DAL.Authorization.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Authorization
{
	public class AuthorizationDbContext : DbContext
	{
		public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options) : base(options)
		{
		}

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	optionsBuilder.UseSqlServer("server=localhost;database=FlyerDB;User Id=sa;Password=Passw0rd");
		//	base.OnConfiguring(optionsBuilder);
		//}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserAuthEntity>().HasAlternateKey(a => a.PhoneNumber);
			modelBuilder.Entity<UserAuthEntity>().HasAlternateKey(a => a.UserUid);
		}

		public DbSet<UserAuthEntity> UserAuthEntities { get; set; }
	}
}