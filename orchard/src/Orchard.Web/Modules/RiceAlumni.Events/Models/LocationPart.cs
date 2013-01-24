using Orchard.ContentManagement;

namespace RiceAlumni.Events.Models
{
	public class LocationPart : ContentPart<LocationPartRecord>
	{
		public virtual string Name
		{
			get { return Record.Name; }
			set { Record.Name = value; }
		}

		public virtual string Address1
		{
			get { return Record.Address1; }
			set { Record.Address1 = value; }
		}
		public virtual string Building
		{
			get { return Record.Building; }
			set { Record.Building = value; }
		}

		public virtual string Room
		{
			get { return Record.Room; }
			set { Record.Room = value; }
		}

		public virtual string Address2
		{
			get { return Record.Address2; }
			set { Record.Address2 = value; }
		}

		public virtual string City
		{
			get { return Record.City; }
			set { Record.City = value; }
		}

		public virtual string State
		{
			get { return Record.State; }
			set { Record.State = value; }
		}

		public virtual string Zip
		{
			get { return Record.Zip; }
			set { Record.Zip = value; }
		}

		public virtual string MapUrl
		{
			get { return Record.MapUrl; }
			set { Record.MapUrl = value; }
		}

		public virtual double Latitude
		{
			get { return Record.Latitude; }
			set { Record.Latitude = value; }
		}

		public virtual double Longitude
		{
			get { return Record.Longitude; }
			set { Record.Longitude = value; }
		}
	}
}