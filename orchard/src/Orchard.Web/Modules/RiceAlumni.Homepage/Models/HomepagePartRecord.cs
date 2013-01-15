using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;

namespace RiceAlumni.Homepage.Models
{
	public class HomepagePartRecord : ContentPartRecord
	{
		public virtual string LinkText { get; set; }
		public virtual string LinkTarget { get; set; }
	}
}