using Orchard.ContentManagement.Records;

namespace RiceAlumni.StaffDirectory.Models
{
	public class StaffProfilePartRecord : ContentPartRecord
	{
		public virtual string Title { get; set; }
		public virtual string Email { get; set; }
		public virtual string Phone { get; set; }
	}
}