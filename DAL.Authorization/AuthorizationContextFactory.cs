using Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DAL.Authorization
{
	public class AuthorizationContextFactory : IDesignTimeDbContextFactory<AuthorizationDbContext>
	{
		//private readonly IConfiguration _configuration;
		//public AuthorizationContextFactory(IConfiguration configuration)
		//{
		//	_configuration = configuration;
		//}

		public AuthorizationDbContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<AuthorizationDbContext>();
			//optionsBuilder.UseSqlServer(_configuration[ConfigurationKeys.ConnectionString]);

			return new AuthorizationDbContext(optionsBuilder.Options);
		}
	}
}