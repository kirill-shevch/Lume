using DAL.Core;
using DAL.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace UnitTestProject.DALTests
{
	public class TestContextFactory : ICoreContextFactory
	{
		public CoreDbContext CreateDbContext()
		{
			var optionsBuilder = new DbContextOptionsBuilder<CoreDbContext>();
			optionsBuilder.UseInMemoryDatabase("LumeDB");
			return new CoreDbContext(optionsBuilder.Options);
		}
	}
}
