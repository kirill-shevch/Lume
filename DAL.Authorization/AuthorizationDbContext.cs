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
		//	optionsBuilder.UseSqlServer(connectionString);
		//	base.OnConfiguring(optionsBuilder);	
		//}

		DbSet<UserAuthEntity> UserAuthEntities { get; set; }
	}
}
