using DAL.Core.Entities;
using DAL.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Core.Repositories
{
	public class ImageRepository : IImageRepository
	{
		private readonly CoreContextFactory _dbContextFactory;
		public ImageRepository(CoreContextFactory dbContextFactory)
		{
			_dbContextFactory = dbContextFactory;
		}


		public async Task<bool> CheckPersonImageExistence(Guid uid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.PersonImageContentEntities.AnyAsync(x => x.PersonImageContentUid == uid, cancellationToken);
			}
		}

		public async Task<bool> CheckEventImageExistence(Guid uid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.EventImageContentEntities.AnyAsync(x => x.EventImageContentUid == uid, cancellationToken);
			}
		}

		public async Task<bool> CheckChatMessageImageExistence(Guid uid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.ChatImageContentEntities.AnyAsync(x => x.ChatImageContentUid == uid, cancellationToken);
			}
		}

		public async Task<Guid?> GetPersonImageUidByHash(string hash)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var entity = await context.PersonImageContentEntities.SingleOrDefaultAsync(x => x.ContentHash == hash);
				return entity == null ? (Guid?)null : entity.PersonImageContentUid;
			}
		}

		public async Task SavePersonImage(Guid personUid, PersonImageContentEntity entity)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var personEntity = await context.PersonEntities.SingleAsync(x => x.PersonUid == personUid);
				personEntity.PersonImageContentEntity = entity;
				await context.PersonImageContentEntities.AddAsync(entity);
				await context.SaveChangesAsync();
			}
		}

		public async Task<byte[]> GetPersonImageContentByUid(Guid uid)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var entity = await context.PersonImageContentEntities.SingleAsync(x => x.PersonImageContentUid == uid);
				return entity.Content; ;
			}
		}
	}
}
