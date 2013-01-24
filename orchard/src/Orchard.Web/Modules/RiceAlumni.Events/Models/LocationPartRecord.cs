using Orchard.ContentManagement.Records;

namespace RiceAlumni.Events.Models
{
	public class LocationPartRecord : ContentPartRecord
	{
		public virtual string Name { get; set; }
		public virtual string Building { get; set; }
		public virtual string Room { get; set; }
		public virtual string Address1 { get; set; }
		public virtual string Address2 { get; set; }
		public virtual string City { get; set; }
		public virtual string State { get; set; }
		public virtual string Zip { get; set; }
		public virtual string MapUrl { get; set; }
		public virtual double Latitude { get; set; }
		public virtual double Longitude { get; set; }
	}
}