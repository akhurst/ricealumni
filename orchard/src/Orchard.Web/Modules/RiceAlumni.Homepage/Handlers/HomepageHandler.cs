using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using RiceAlumni.Homepage.Models;

namespace RiceAlumni.Homepage.Handlers
{
	public class HomepageHandler : ContentHandler
	{
		public HomepageHandler(IRepository<HomepagePartRecord> homepagePartRepository)
		{
			Filters.Add(StorageFilter.For(homepagePartRepository));
		}
	}
}