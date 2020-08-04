using Constants;
using DAL.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL.Core
{
	public class CoreContextFactory : ICoreContextFactory
	{
		private readonly IConfiguration _configuration;
		public CoreContextFactory(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public CoreDbContext CreateDbContext()
		{
			var connectionString = string.IsNullOrWhiteSpace(_configuration.GetConnectionString(ConfigurationKeys.AzureConnectionString)) ?
				_configuration[ConfigurationKeys.LocalConnectionString] :
				_configuration.GetConnectionString(ConfigurationKeys.AzureConnectionString);

			var optionsBuilder = new DbContextOptionsBuilder<CoreDbContext>();
			optionsBuilder.UseSqlServer(connectionString);

			return new CoreDbContext(optionsBuilder.Options);
		}
	}
}