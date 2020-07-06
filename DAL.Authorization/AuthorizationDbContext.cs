using DAL.Authorization.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Authorization
{
	public class AuthorizationDbContext : DbContext
	{
		private readonly string _connectionString;
		public AuthorizationDbContext(string connectionString) : base()
		{
			_connectionString = connectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(_connectionString);
		}

		DbSet<UserAuthEntity> UserAuthEntities { get; set; }
	}
}
