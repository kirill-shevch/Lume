namespace DAL.Core.Interfaces
{
	public interface ICoreContextFactory
	{
		CoreDbContext CreateDbContext();
	}
}
