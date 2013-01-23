using System;
using Orchard.ContentManagement;

namespace RiceAlumni.Events.Models
{
	public class EventPart : ContentPart<EventPartRecord>
	{
		public virtual DateTime StartDate
		{
			get { return Record.StartDate; }
			set { Record.StartDate = value; }
		}

		public virtual DateTime EndDate
		{
			get { return Record.EndDate; }
			set { Record.EndDate = value; }
		}

		public virtual string LinkTarget
		{
			get { return Record.LinkTarget; }
			set { Record.LinkTarget = value; }
		}

		public virtual string LinkText
		{
			get { return Record.LinkText; }
			set { Record.LinkText = value; }
		}

		public virtual int PciEventId
		{
			get { return Record.PciEventId; }
			set { Record.PciEventId = value; }
		}
	}
}