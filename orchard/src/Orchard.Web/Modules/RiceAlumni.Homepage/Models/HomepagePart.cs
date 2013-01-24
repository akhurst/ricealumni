using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;

namespace RiceAlumni.Homepage.Models
{
	public class HomepagePart : ContentPart<HomepagePartRecord>
	{
		public virtual string LinkText { get { return Record.LinkText; } set { Record.LinkText = value; } }
		public virtual string LinkTarget { get { return Record.LinkTarget; } set { Record.LinkTarget = value; } }
	}
}