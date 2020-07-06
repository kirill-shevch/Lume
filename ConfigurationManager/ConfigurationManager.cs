using Microsoft.Extensions.Configuration;
using System.IO;

namespace ConfigurationManager
{
	public static class ConfigurationManager
	{
		public static IConfiguration Configuration { get; } = GetConfiguration();

		private static IConfiguration GetConfiguration()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location))
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables();

			return builder.Build();
		}
	}
}