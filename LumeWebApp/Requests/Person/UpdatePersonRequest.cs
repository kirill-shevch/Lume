using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LumeWebApp.Requests.Person
{
	public class UpdatePersonRequest
	{
		public string Name { get; set; }
		public string Agenda { get; set; }
		public byte? Age { get; set; }
	}
}