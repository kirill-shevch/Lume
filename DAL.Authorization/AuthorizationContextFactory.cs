using Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL.Authorization
{
	public class AuthorizationContextFactory
	{
		private readonly IConfiguration _configuration;
		public AuthorizationContextFactory(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public AuthorizationDbContext CreateDbContext()
		{
			var connectionString = string.IsNullOrWhiteSpace(_configuration.GetConnectionString(ConfigurationKeys.AzureConnectionString)) ?
				_configuration[ConfigurationKeys.LocalConnectionString] :
				_configuration.GetConnectionString(ConfigurationKeys.AzureConnectionString);

			var optionsBuilder = new DbContextOptionsBuilder<AuthorizationDbContext>();
			optionsBuilder.UseSqlServer(connectionString);

			return new AuthorizationDbContext(optionsBuilder.Options);
		}
	}
}