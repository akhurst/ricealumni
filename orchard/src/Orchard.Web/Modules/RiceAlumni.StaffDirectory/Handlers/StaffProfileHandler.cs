using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using RiceAlumni.StaffDirectory.Models;

namespace RiceAlumni.StaffDirectory.Handlers
{
	public class StaffProfileHandler : ContentHandler
	{
		public StaffProfileHandler(IRepository<StaffProfilePartRecord> staffProfilePartRepository)
		{
			Filters.Add(StorageFilter.For(staffProfilePartRepository));
		}
	}
}