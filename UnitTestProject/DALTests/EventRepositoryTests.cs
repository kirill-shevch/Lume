using DAL.Core;
using DAL.Core.Entities;
using DAL.Core.Models;
using DAL.Core.Repositories;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTestProject.DALTests
{
	[TestFixture]
	public class EventRepositoryTests
	{
		private EventRepository _eventRepository;
		[SetUp]
		public async Task Setup()
		{
			var contextFactory = new TestContextFactory();
			_eventRepository = new EventRepository(contextFactory);
			await GenerateDataInDataBase(contextFactory.CreateDbContext());
		}

		[Test]
		[TestCaseSource("SearchForEventTestData")]
		public async Task SearchForEventTests(RepositoryEventSearchFilter repositoryFilter)
		{
			var result = await _eventRepository.SearchForEvent(repositoryFilter);
			Assert.AreEqual(1, result.Count);
		}

		private async Task GenerateDataInDataBase(CoreDbContext context)
		{
			context.Database.EnsureDeleted();
			await context.EventTypeEntities.AddRangeAsync(
				new EventTypeEntity { EventTypeId = 1, EventTypeName = "TestType1" },
				new EventTypeEntity { EventTypeId = 2, EventTypeName = "TestType2" });
			await context.EventStatusEntities.AddRangeAsync(
				new EventStatusEntity { EventStatusId = 1, EventStatusName = "TestStatus1" },
				new EventStatusEntity { EventStatusId = 2, EventStatusName = "TestStatus2" });
			await context.CityEntities.AddRangeAsync(
				new CityEntity { CityId = 1, CityName = "TestCity1" },
				new CityEntity { CityId = 2, CityName = "TestCity2" },
				new CityEntity { CityId = 3, CityName = "TestCity3" });
			await context.EventEntities.AddRangeAsync(
				new EventEntity
				{
					EventId = 1,
					Name = "Name1",
					Description = "Description1",
					MinAge = 18,
					MaxAge = 21,
					StartTime = new DateTime(2020, 08, 20, 18, 0, 0),
					EndTime = new DateTime(2020, 08, 20, 23, 0, 0),
					EventTypes = new List<EventTypeToEventEntity> { new EventTypeToEventEntity { EventId = 1, EventTypeId = 0 } },
					EventStatusId = 0,
					IsOpenForInvitations = true,
					CityId = 0,
					IsOnline = false
				},
				new EventEntity
				{
					Name = "Name2",
					Description = "Description2",
					MinAge = 15,
					MaxAge = 25,
					StartTime = new DateTime(2020, 08, 20, 10, 0, 0),
					EndTime = new DateTime(2020, 08, 20, 15, 0, 0),
					EventTypes = new List<EventTypeToEventEntity> { new EventTypeToEventEntity { EventId = 1, EventTypeId = 1 } },
					EventStatusId = 1,
					IsOpenForInvitations = false,
					CityId = 1,
					IsOnline = false
				},
				new EventEntity
				{
					Name = "Name3",
					Description = "Description3",
					MinAge = 15,
					MaxAge = 25,
					StartTime = new DateTime(2020, 08, 20, 10, 0, 0),
					EndTime = new DateTime(2020, 08, 20, 15, 0, 0),
					EventTypes = new List<EventTypeToEventEntity> { new EventTypeToEventEntity { EventId = 1, EventTypeId = 1 } },
					EventStatusId = 1,
					IsOpenForInvitations = false,
					CityId = 1,
					IsOnline = true
				});
			await context.SaveChangesAsync();
		}

		public static IEnumerable<TestCaseData> SearchForEventTestData
		{
			get
			{
				yield return new TestCaseData(new RepositoryEventSearchFilter { Query = "Name1"});
				yield return new TestCaseData(new RepositoryEventSearchFilter { Query = "Description1" });
				yield return new TestCaseData(new RepositoryEventSearchFilter { MinAge = 16 });
				yield return new TestCaseData(new RepositoryEventSearchFilter { MaxAge = 23 });
				yield return new TestCaseData(new RepositoryEventSearchFilter { StartTime = new DateTime(2020, 08, 20, 18, 0, 0) });
				yield return new TestCaseData(new RepositoryEventSearchFilter { EndTime = new DateTime(2020, 08, 20, 23, 0, 0) });
				yield return new TestCaseData(new RepositoryEventSearchFilter { Type = 0 });
				yield return new TestCaseData(new RepositoryEventSearchFilter { Status = 0 });
				yield return new TestCaseData(new RepositoryEventSearchFilter { CityId = 0, IsOnline = false });
				yield return new TestCaseData(new RepositoryEventSearchFilter { CityId = 2 });
				yield return new TestCaseData(new RepositoryEventSearchFilter { IsOpenForInvitations = true });
			}
		}
	}
}
