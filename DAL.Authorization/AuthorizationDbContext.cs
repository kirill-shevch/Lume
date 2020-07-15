using DAL.Authorization.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Authorization
{
	public class AuthorizationDbContext : DbContext
	{
		public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options) : base(options)
		{
		}

		public DbSet<PersonAuthEntity> PersonAuthEntities { get; set; }
	}
}