﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;

namespace RiceAlumni.Events.Models
{
	public class EventPartRecord : ContentPartRecord	
	{
		public virtual DateTime StartDate { get; set; }
		public virtual DateTime EndDate { get; set; }
		public virtual string LinkTarget { get; set; }
		public virtual string LinkText { get; set; }
		public virtual int PciEventId { get; set; }
	}
}